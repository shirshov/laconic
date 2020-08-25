using System;
using System.Collections.Generic;
using xf = Xamarin.Forms;

namespace Laconic
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
    
    static class Patch
    {
        internal static List<(Guid ContextId, xf.BindableObject Element)> Apply(xf.BindableObject element, 
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

            IList<xf.View> GetChildren() => ((xf.Layout<xf.View>) element).Children;

            var withContext = new List<(Guid, xf.BindableObject)>();
            
            foreach (var op in operations) {
                Action patchingAction = op switch {
                    SetProperty p => () => {
                        var native = ConvertToNative(p.Value);
                        element.SetValue(p.Property, native);
                        var updated = element.GetValue(p.Property);
                    },
                    ResetProperty p => () => element.ClearValue(p.Property),
                    RemoveContent _ => () => SetRealViewContent(null),
                    SetContent sc => () => {
                        var childView = (xf.View?) CreateView((Element) sc.ContentView);
                        withContext.AddRange(Apply(childView!, sc.Operations, dispatch));
                        SetRealViewContent(childView);
                    },
                    UpdateContent uc => () => withContext.AddRange(Apply(GetRealViewContent(), uc.Operations, dispatch)),
                    UpdateChildren uc => () => withContext.AddRange(ViewListPatch.Apply(GetChildren(), uc.Operations, dispatch)),
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
                    WireEvent evt => () => {
                        var subs = (Dictionary<string, EventSubscription>)element.GetValue(EventSubscription.EventSubscriptionsProperty);
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
                    UpdateItems ui => () => ViewListPatch.PatchItemsSource((xf.ItemsView) element, ui, dispatch, 
                        (x, y) => ((IElement)x, (IElement)y)),
                    AddGestureRecognizer agr => () => {
                        var view = (xf.View) element;
                        var realRec = agr.Blueprint.CreateReal();
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
                    SetClip sc => () => {
                        var geomEl = (Element) sc.Geometry;
                        var realGeom = geomEl.CreateView();
                        foreach (var p in geomEl.ProvidedValues)
                            realGeom.SetValue(p.Key, p.Value);
                        element.SetValue(xf.VisualElement.ClipProperty, realGeom);
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
                    _ => throw new InvalidOperationException("Diff operation not supported: " + op)
                };
                patchingAction();
            }

            return withContext;
        }

        internal static xf.BindableObject CreateView(Element definition) => definition.CreateView();

        static object ConvertToNative(object value) => value switch {
            FontAttributes _ => (xf.FontAttributes) value,
            ReturnType _ => (xf.ReturnType) value,
            IndicatorShape _ => (xf.IndicatorShape) value,
            ScrollBarVisibility _ => (xf.ScrollBarVisibility) value,
            ItemsUpdatingScrollMode _ => (xf.ItemsUpdatingScrollMode) value,
            LineBreakMode _ => (xf.LineBreakMode) value,
            TextDecorations _ => (xf.TextDecorations) value,
            TextType _ => (xf.TextType) value,
            ScrollOrientation _ => (xf.ScrollOrientation) value,
            SelectionMode _ => (xf.SelectionMode) value,
            StackOrientation _ => (xf.StackOrientation) value,
            ItemSizingStrategy _ => (xf.ItemSizingStrategy) value,
            FlowDirection _ => (xf.FlowDirection) value,
            TextAlignment _ => (xf.TextAlignment) value,
            Aspect _ => (xf.Aspect) value,
            ClearButtonVisibility _ => (xf.ClearButtonVisibility) value,
            EditorAutoSizeOption _ => (xf.EditorAutoSizeOption) value,
            LayoutAlignment _ => (xf.LayoutAlignment) value,
            Stretch _ => (xf.Stretch)value,
            VisualMarker vm => vm switch {
                VisualMarker.Default => xf.VisualMarker.Default,
                VisualMarker.Material => xf.VisualMarker.Material,
                VisualMarker.MatchParent => xf.VisualMarker.MatchParent,
                _ => throw new NotImplementedException($"Support for VisualMarker.{vm} is not implemented")
            },
            // Keyboard is a class in Xamarin.Forms
            Keyboard k => k switch {
                Keyboard.Plain => xf.Keyboard.Plain,
                Keyboard.Chat => xf.Keyboard.Chat,
                Keyboard.Default => xf.Keyboard.Default,
                Keyboard.Email => xf.Keyboard.Email,
                Keyboard.Numeric => xf.Keyboard.Numeric,
                Keyboard.Telephone => xf.Keyboard.Telephone,
                Keyboard.Text => xf.Keyboard.Text,
                Keyboard.Url => xf.Keyboard.Url,
                _ => throw new NotImplementedException($"Support for Keyboard.{k} is not implemented")
            },
            // Shapes enums
            Shapes.FillRule _ => (xf.Shapes.FillRule)value,
            Shapes.PenLineCap _ => (xf.Shapes.PenLineCap)value,
            Shapes.PenLineJoin _ => (xf.Shapes.PenLineJoin)value,
            // LayoutOptions is a struct in Xamarin.Forms
            LayoutOptions l when l == LayoutOptions.Start => xf.LayoutOptions.Start,
            LayoutOptions l when l == LayoutOptions.Center => xf.LayoutOptions.Center,
            LayoutOptions l when l == LayoutOptions.End => xf.LayoutOptions.End,
            LayoutOptions l when l == LayoutOptions.Fill => xf.LayoutOptions.Fill,
            LayoutOptions l when l == LayoutOptions.StartAndExpand => xf.LayoutOptions.StartAndExpand,
            LayoutOptions l when l == LayoutOptions.CenterAndExpand => xf.LayoutOptions.CenterAndExpand,     
            LayoutOptions l when l == LayoutOptions.EndAndExpand => xf.LayoutOptions.EndAndExpand,
            LayoutOptions l when l == LayoutOptions.FillAndExpand => xf.LayoutOptions.FillAndExpand,
            // custom types 
            Thickness t => new xf.Thickness(t.Left, t.Top, t.Right, t.Bottom),
            Color c => c.ToXamarinFormsColor(),
            CornerRadius r => r.ToXamarinFormsCornerRadius(),
            ImageSource s => s.ToXamarinFormsImageSource(),
            _ => value
        };
    }
}