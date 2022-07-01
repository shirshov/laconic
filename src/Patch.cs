namespace Laconic;

static class Patch
{
    class EventSubscription
    {
        public static readonly xf.BindableProperty EventSubscriptionsProperty = xf.BindableProperty.CreateAttached(
            nameof(EventSubscriptionsProperty), 
            typeof(Dictionary<string, EventSubscription>), 
            typeof(EventSubscription), null);

        readonly Action<Signal> _dispatch;
        public Func<EventArgs, Signal> SignalMaker;
            
        public EventSubscription(Func<EventArgs, Signal> signalMaker, Action<Signal> dispatch)
        {
            SignalMaker = signalMaker;
            _dispatch = dispatch;
        }

        public void EventHandler(object sender, EventArgs e)
        {
            var signal = SignalMaker(e);
            _dispatch(signal);
        }
    }
    
    internal static List<(string? ContextKey, xf.BindableObject Element)> Apply(xf.BindableObject element, 
        IEnumerable<DiffOperation> operations,
        Action<Signal> dispatch)
    {
        xf.View GetRealViewContent() => element switch {
            xf.ContentView cv => cv.Content,
            xf.ContentPage cp => cp.Content,
            xf.ScrollView sv => sv.Content,
            // TODO: fallback to ContentAttribute
            _ => throw new NotImplementedException("Unknown type of content view")
        };

        void SetRealViewContent(xf.View? content)
        {
            switch (element) {
                case xf.ContentView cv:
                    cv.Content = content;
                    break;
                case xf.ContentPage cp:
                    cp.Content = content;
                    break;
                case xf.ScrollView sv:
                    sv.Content = content;
                    break;
                default:
                    throw new NotImplementedException("Unknown type of content view");
            }
        }

        var newWithContext = new List<(string? ContextKey, xf.BindableObject View)>();

        var postProcessOps = operations.Where(o => (o as SetProperty)?.Value is PostProcessInfo);
        var otherOps = operations.Where(o => (o as SetProperty)?.Value is not PostProcessInfo);
        foreach (var op in otherOps.Concat(postProcessOps)) {
            Action patchingAction = op switch {
                ChangeContextKey cck => () => { newWithContext.Add((cck.NewKey, element)); },
                SetChildElementToNull co => () => element.SetValue(co.ChildElementProperty, null),
                SetChildElement co => () => {
                    var newChildObject = co.CreateElement();
                    Apply(newChildObject, co.Operations, dispatch);
                    element.SetValue(co.ChildElementProperty, newChildObject);
                },
                UpdateChildElement co => () => {
                    var childObject = (xf.BindableObject)element.GetValue(co.ChildElementProperty);
                    Apply(childObject, co.Operations, dispatch);
                },
                SetProperty p => () => {
                    if (p.Value is PostProcessInfo info)
                        info.Process(element);
                    else
                        element.SetValue(p.Property, ConvertToNative(p.Value));
                },
                ResetProperty p => () => element.ClearValue(p.Property),
                RemoveContent _ => () => SetRealViewContent(null),
                SetContent sc => () => {
                    var el = (Element) sc.ContentView;
                    var childView = (xf.View?) CreateView(el, dispatch);
                    newWithContext.AddRange(Apply(childView!, sc.Operations, dispatch));
                    SetRealViewContent(childView);
                },
                UpdateContent uc => () => newWithContext.AddRange(Apply(GetRealViewContent(), uc.Operations, dispatch)),
                UpdateChildViews uc => () => newWithContext.AddRange(ViewListPatch.Apply(
                    ((xf.Layout) element).Children, uc.Operations, dispatch)),
                UpdateChildElementList uc => () => newWithContext.AddRange(ViewListPatch.ApplyToChildElements(uc.GetList(element), uc.Operations, dispatch)),
                RowDefinitionsChange rdc => () => {
                    var grid = (xf.Grid) element;
                    grid.RowDefinitions.Clear();
                    foreach (var d in rdc.Definitions)
                        grid.RowDefinitions.Add(d);
                },
                ColumnDefinitionsChange rdc => () => {
                    var grid = (xf.Grid) element;
                    grid.ColumnDefinitions.Clear();
                    foreach (var d in rdc.Definitions)
                        grid.ColumnDefinitions.Add(d);
                },
                GridPositionChange gpc => () => {
                    Action<xf.BindableObject, int> change = gpc.Type switch {
                        GridPositionChangeType.Row => xf.Grid.SetRow,
                        GridPositionChangeType.Column => xf.Grid.SetColumn,
                        GridPositionChangeType.RowSpan => xf.Grid.SetRowSpan,
                        GridPositionChangeType.ColumnSpan => xf.Grid.SetColumnSpan,
                        _ => throw new InvalidOperationException("Unknown grid position change")
                    };
                    change(element, gpc.Value);
                },
                SetAbsoluteLayoutPositioning (var bounds, var flags) => () => {
                    xf.AbsoluteLayout.SetLayoutBounds(element, new Maui.Graphics.Rect(bounds.X, bounds.Y, bounds.Width, bounds.Height));
                    xf.AbsoluteLayout.SetLayoutFlags(element, (Maui.Layouts.AbsoluteLayoutFlags)ConvertToNative(flags));
                },
                WireEvent evt => () => {
                    var subs = (Dictionary<string, EventSubscription>?)element.GetValue(EventSubscription.EventSubscriptionsProperty);
                    if (subs == null) {
                        subs = new Dictionary<string, EventSubscription>();
                        element.SetValue(EventSubscription.EventSubscriptionsProperty, subs);
                    }

                    if (!subs.TryGetValue(evt.EventName, out var sub)) {
                        sub = new EventSubscription(evt.SignalMaker, dispatch);
                        evt.Subscribe(element, sub.EventHandler);
                        subs[evt.EventName] = sub;
                    }
                    sub.SignalMaker = evt.SignalMaker;
                },
                UnwireEvent evt => () => {
                    var subs = (Dictionary<string, EventSubscription>)element.GetValue(EventSubscription.EventSubscriptionsProperty);
                    var sub = subs[evt.EventName];
                    evt.Unsubscribe(element, sub.EventHandler);
                },
                UpdateItems ui => () => {
                    var itemsView = (xf.ItemsView) element;
                    ViewListPatch.PatchItemsSource(itemsView, ui, dispatch, itemsView.ItemsSource);
                },
                AddGestureRecognizer agr => () => {
                    var view = (xf.View) element;
                    var realRec = ((Element)agr.Blueprint).CreateView();
                    Apply(realRec, agr.Operations, dispatch);
                    view.GestureRecognizers.Add((xf.IGestureRecognizer)realRec);
                },
                RemoveGestureRecognizer rm => () => {
                    var view = (xf.View)element;
                    // TODO: unsubscribe
                    view.GestureRecognizers.RemoveAt(rm.Index);
                },
                UpdateGestureRecognizer ugr => () => {
                    var view = (xf.View) element;
                    Apply((xf.BindableObject) view.GestureRecognizers[ugr.Index], ugr.Operations, dispatch);
                },
                AddToolbarItem tb => () => {
                    var view = (xf.ToolbarItem)tb.Blueprint.CreateView();
                    Apply(view, tb.Operations, dispatch);
                    var page = (xf.ContentPage) element;
                    page.ToolbarItems.Add(view);
                },
                RemoveToolbarItem tb => () => {
                    var page = (xf.ContentPage) element;
                    page.ToolbarItems.RemoveAt(tb.Index);
                },
                UpdateToolbarItem tb => () => {
                    var page = (xf.ContentPage) element;
                    Apply(page.ToolbarItems[tb.Index], tb.Operations, dispatch);
                },
                UpdateFlyoutPage fp => () => {
                    var nativePage = (xf.FlyoutPage) element;

                    switch (fp.FlyoutOperation) {
                        case SetFlyoutPageFlyout setFlyout:
                            var flyout = new xf.ContentPage();
                            if (setFlyout.Page is IContextElement ctxEl)
                                newWithContext.Add((ctxEl.ContextKey, flyout));
                            newWithContext.AddRange(Apply(flyout, setFlyout.Operations, dispatch));
                            nativePage.Flyout = flyout;
                            break;
                        case UpdateFlyoutPageFlyout updateFlyout:
                            newWithContext.AddRange(Apply(nativePage.Flyout, updateFlyout.Operations, dispatch));
                            break;
                    }

                    switch (fp.DetailOperation) {
                        case SetFlyoutPageDetail setDetail:
                            var content = CreateView(setDetail.DetailPage, dispatch);
                            if (setDetail.DetailPage.ContextKey != null)
                                newWithContext.Add((setDetail.DetailPage.ContextKey, content));
                            newWithContext.AddRange(Apply(content, setDetail.Operations, dispatch));
                            nativePage.Detail = (xf.Page)content;
                            break;
                        case UpdateFlyoutPageDetail updateDetail: 
                            newWithContext.AddRange(Apply(nativePage.Detail, updateDetail.Operations, dispatch));
                            break;
                    }
                },
                _ => throw new InvalidOperationException("Diff operation not supported: " + op)
            };
            patchingAction();
        }

        return newWithContext;
    }

    internal static xf.BindableObject CreateView(Element definition, Action<Signal> dispatch)
    {
        (definition as IDoDispatch)?.SetDispatcher(dispatch);
        return definition.CreateView();
    }

    internal static object ConvertToNative(object value) => value switch {
        AbsoluteLayoutFlags _ => (Maui.Layouts.AbsoluteLayoutFlags)value,
        FontAttributes _ => (xf.FontAttributes) value,
        ReturnType _ => (Maui.ReturnType) value,
        IndicatorShape _ => (xf.IndicatorShape) value,
        ScrollBarVisibility _ => (Maui.ScrollBarVisibility) value,
        ItemsUpdatingScrollMode _ => (xf.ItemsUpdatingScrollMode) value,
        LineBreakMode _ => (Maui.LineBreakMode) value,
        TextDecorations _ => (Maui.TextDecorations) value,
        TextType _ => (Maui.TextType) value,
        ScrollOrientation _ => (Maui.ScrollOrientation) value,
        SelectionMode _ => (xf.SelectionMode) value,
        StackOrientation _ => (xf.StackOrientation) value,
        ItemSizingStrategy _ => (xf.ItemSizingStrategy) value,
        FlowDirection _ => (Maui.FlowDirection) value,
        TextAlignment _ => (Maui.TextAlignment) value,
        TextTransform _ => (Maui.TextTransform) value,
        Aspect _ => (Maui.Aspect) value,
        ClearButtonVisibility _ => (Maui.ClearButtonVisibility) value,
        EditorAutoSizeOption _ => (xf.EditorAutoSizeOption) value,
        LayoutAlignment _ => (xf.LayoutAlignment) value,
        Stretch _ => (xf.Stretch)value,
        FlyoutLayoutBehavior _ => (xf.FlyoutLayoutBehavior)value,
        VisualMarker vm => vm switch {
            VisualMarker.Default => xf.VisualMarker.Default,
            VisualMarker.MatchParent => xf.VisualMarker.MatchParent,
            _ => throw new NotImplementedException($"Support for VisualMarker.{vm} is not implemented")
        },
        SnapPointsAlignment => (xf.SnapPointsAlignment)value,
        SnapPointsType => (xf.SnapPointsType)value,
        // Easing is a class in Xamarin.Forms
        Easing e => e switch {
            Easing.Linear => Maui.Easing.Linear,
            Easing.BounceIn => Maui.Easing.BounceIn,
            Easing.BounceOut => Maui.Easing.BounceOut,
            Easing.CubicIn => Maui.Easing.CubicIn,
            Easing.CubicOut => Maui.Easing.CubicOut,
            Easing.SinIn => Maui.Easing.SinIn,
            Easing.SinOut => Maui.Easing.SinOut,
            Easing.SpringIn => Maui.Easing.SpringIn,
            Easing.SpringOut => Maui.Easing.SpringOut,
            Easing.CubicInOut => Maui.Easing.CubicInOut,
            Easing.SinInOut => Maui.Easing.SinInOut,
            _ => throw new NotImplementedException($"Support for Easing.{e} is not implemented")
        },
        // Keyboard is a class in Xamarin.Forms
        Keyboard k => k switch {
            Keyboard.Plain => Maui.Keyboard.Plain,
            Keyboard.Chat => Maui.Keyboard.Chat,
            Keyboard.Default => Maui.Keyboard.Default,
            Keyboard.Email => Maui.Keyboard.Email,
            Keyboard.Numeric => Maui.Keyboard.Numeric,
            Keyboard.Telephone => Maui.Keyboard.Telephone,
            Keyboard.Text => Maui.Keyboard.Text,
            Keyboard.Url => Maui.Keyboard.Url,
            _ => throw new NotImplementedException($"Support for Keyboard.{k} is not implemented")
        },
        // Shapes enums
        Shapes.FillRule _ => (xf.Shapes.FillRule)value,
        Shapes.PenLineCap _ => (xf.Shapes.PenLineCap)value,
        Shapes.PenLineJoin _ => (xf.Shapes.PenLineJoin)value,
        // LayoutOptions is a struct in Xamarin.Forms
        LayoutOptions.Start => xf.LayoutOptions.Start,
        LayoutOptions.Center => xf.LayoutOptions.Center,
        LayoutOptions.End => xf.LayoutOptions.End,
        LayoutOptions.Fill => xf.LayoutOptions.Fill,
        // custom types 
        Thickness t => new Maui.Thickness(t.Left, t.Top, t.Right, t.Bottom),
        IConvert c => c.ToNative(),
        _ => value
    };
}