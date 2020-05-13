using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using xf = Xamarin.Forms;

namespace Laconic
{
    static class Patch
    {
        internal static void Apply(xf.Element element, IEnumerable<IDiffOperation> operations, Action<Signal> dispatch)
        {
            xf.View GetRealViewContent() => element switch
            {
                xf.ContentView cv => cv.Content,
                xf.ContentPage cp => cp.Content,
                xf.ScrollView sv => sv.Content,
                // TODO: fallback to ContentAttribute
                _ => throw new NotImplementedException("Unknown type of content view")
            };

            void SetRealViewContent(xf.View? content)
            {
                switch (element)
                {
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

            foreach (var op in operations)
            {
                Action patchingAction = op switch
                {
                    SetProperty p => () => element.SetValue(p.Property, p.Value),
                    ResetProperty p => () => element.ClearValue(p.Property),
                    RemoveContent _ => () => SetRealViewContent(null),
                    SetContent sc => () =>
                    {
                        var childView = (xf.View?) CreateRealView((Element) sc.ContentView);
                        Apply(childView!, sc.Operations, dispatch);
                        SetRealViewContent(childView);
                    },
                    UpdateContent uc => () => Apply(GetRealViewContent(), uc.Operations, dispatch),
                    UpdateChildren uc => () => ViewListPatch.Apply(GetChildren(), uc.Operations, dispatch),
                    RowDefinitionsChange rdc => () =>
                    {
                        var grid = (xf.Grid) element;
                        grid.RowDefinitions.Clear();
                        foreach (var d in rdc.Definitions)
                            grid.RowDefinitions.Add(d);
                    },
                    ColumnDefinitionsChange rdc => () =>
                    {
                        var grid = (xf.Grid) element;
                        grid.ColumnDefinitions.Clear();
                        foreach (var d in rdc.Definitions)
                            grid.ColumnDefinitions.Add(d);
                    },
                    GridPositionChange gpc => () =>
                    {
                        Action<xf.BindableObject, int> change = gpc.Type switch
                        {
                            GridPositionChangeType.Row => xf.Grid.SetRow,
                            GridPositionChangeType.Column => xf.Grid.SetColumn,
                            GridPositionChangeType.RowSpan => xf.Grid.SetRowSpan,
                            GridPositionChangeType.ColumnSpan => xf.Grid.SetColumnSpan,
                            _ => throw new InvalidOperationException("Unknown grid position change")
                        };
                        change(element, gpc.Value);
                    },
                    SetEvent evt => () => evt.Handler.Subscribe(element, dispatch),
                    UpdateItems ui => () => ViewListPatch.PatchItemsSource((xf.ItemsView) element, ui, dispatch),
                    SetGestureRecognizers rec => () =>
                    {
                        var view = (xf.View) element;
                        view.GestureRecognizers.Clear();
                        foreach (var gestureRecognizer in rec.Recognizers)
                        {
                            var r = (Element) gestureRecognizer;
                            var newRec = (xf.GestureRecognizer) r.CreateReal();
                            foreach (var p in r.ProvidedValues)
                                newRec.SetValue(p.Key, p.Value);

                            foreach (var e in r.Events)
                                e.Value.Subscribe(newRec, dispatch);

                            view.GestureRecognizers.Add(newRec);
                        }
                    },
                    _ => throw new InvalidOperationException("Diff operation not supported: " + op)
                };
                patchingAction();
            }
        }

        internal static xf.Element CreateRealView(Element definition) => definition.CreateReal();
    }
}