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
        internal static void Apply(xf.BindableObject element, IEnumerable<DiffOperation> operations,
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

            foreach (var op in operations) {
                Action patchingAction = op switch {
                    SetProperty p => () => element.SetValue(p.Property, p.Value),
                    ResetProperty p => () => element.ClearValue(p.Property),
                    RemoveContent _ => () => SetRealViewContent(null),
                    SetContent sc => () => {
                        var childView = (xf.View?) CreateReal((Element) sc.ContentView);
                        Apply(childView!, sc.Operations, dispatch);
                        SetRealViewContent(childView);
                    },
                    UpdateContent uc => () => Apply(GetRealViewContent(), uc.Operations, dispatch),
                    UpdateChildren uc => () => ViewListPatch.Apply(GetChildren(), uc.Operations, dispatch),
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
                    UpdateItems ui => () => ViewListPatch.PatchItemsSource((xf.ItemsView) element, ui, dispatch),
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
                        var realGeom = geomEl.CreateReal();
                        foreach (var p in geomEl.ProvidedValues)
                            realGeom.SetValue(p.Key, p.Value);
                        element.SetValue(xf.VisualElement.ClipProperty, realGeom);
                    },
                    AddToolbarItem tb => () => {
                         var real = (xf.ToolbarItem)tb.Blueprint.CreateReal();
                         Apply(real, tb.Operations, dispatch);
                         var page = (xf.ContentPage) element;
                         page.ToolbarItems.Add(real);
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
        }

        internal static xf.BindableObject CreateReal(Element definition) => definition.CreateReal();
    }
}