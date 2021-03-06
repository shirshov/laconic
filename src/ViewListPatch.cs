using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using xf = Xamarin.Forms;

namespace Laconic
{
    static class ViewListPatch
    {
        internal static List<(string, xf.BindableObject)> Apply(IList<xf.View> list, IEnumerable<ListOperation> operations, Action<Signal> dispatch)
        {
            var withContext = new List<(string, xf.BindableObject)>();
            
            foreach (var op in operations) {
                Action patchAction = op switch {
                    RemoveChild rc => () => list.RemoveAt(rc.Index),
                    UpdateChild uc => () => Patch.Apply(list[uc.Index], uc.Operations, dispatch),
                    ReplaceChild rc => () => {
                        var real = (xf.View) Patch.CreateView(rc.NewView);
                        // TODO: add to withContext here and below
                        Patch.Apply(real, rc.Operations, dispatch);
                        list[rc.Index] = real;
                    },
                    AddChild acv => () => {
                        var real = Patch.CreateView(acv.Blueprint);
                        var v = Patch.Apply(real, acv.Operations, dispatch);
                        withContext.AddRange(v);
                        list.Insert(acv.Index, (xf.View) real);
                        if (acv.Blueprint.ContextKey != null)
                            withContext.Add((acv.Blueprint.ContextKey, real));
                    },
                    _ => () => throw new InvalidOperationException($"Unknown Diff operation: {op.GetType()}")
                };
                patchAction();
            }
            return withContext;
        }

        // TODO: this method should be somewhere else
        internal static void ApplyToChildElements (IList list, ListOperation[] operations, Action<Signal> dispatch)
        {
            foreach (var op in operations) {
                Action patchAction = op switch {
                    RemoveChild rc => () => list.RemoveAt(rc.Index),
                    UpdateChild uc => () => Patch.Apply((xf.BindableObject)list[uc.Index], uc.Operations, dispatch),
                    ReplaceChild rc => () => {
                        var real = (xf.View) Patch.CreateView(rc.NewView);
                        Patch.Apply(real, rc.Operations, dispatch);
                        list[rc.Index] = real;
                    },
                    AddChild acv => () => {
                        var real = Patch.CreateView(acv.Blueprint);
                        Patch.Apply(real, acv.Operations, dispatch);
                        list.Insert(acv.Index, real);
                    },
                    _ => () => throw new InvalidOperationException($"Unknown Diff operation: {op.GetType()}")
                };
                patchAction();
            }
        }
        
        internal static void PatchItemsSource(xf.ItemsView itemsView, 
            UpdateItems update, 
            Action<Signal> dispatch, 
            IEnumerable? itemsSource)
        {
            if (itemsSource == null) {
                var source = new ObservableCollection<BindingContextItem>();

                foreach (var op in update.Operations.OfType<AddChild>())
                    source.Add(new BindingContextItem(op.ReuseKey, op.Key, (View)op.Blueprint));

                itemsView.ItemTemplate = new ItemsViewTemplateSelector(dispatch);
                itemsView.ItemsSource = source;
            }
            else {
                var source = (ObservableCollection<BindingContextItem>) itemsSource;
                foreach (var op in update.Operations) {
                    Action patchAction = op switch {
                        AddChild ac => () => source.Insert(ac.Index, new BindingContextItem(ac.ReuseKey, ac.Key, (View)ac.Blueprint)),
                        RemoveChild rc => () => source.RemoveAt(rc.Index),
                        UpdateChild uc => () => {
                            var selector = (ItemsViewTemplateSelector) itemsView.ItemTemplate;
                            selector.UpdateRendered(uc.Key, (View)uc.Blueprint);
                            source[uc.Index].Blueprint = (View)uc.Blueprint;
                        },
                        ReplaceChild _ => () => throw new InvalidOperationException("ItemsViewList should never" +
                            " replace child views"),
                        _ => () => throw new InvalidOperationException($"Unknown Diff operation: {op.GetType()}")
                    };
                    patchAction();
                }
            }
        }
    }

    class BindingContextItem
    {
        public readonly string ReuseKey;
        public readonly Key Key;
        public View Blueprint;

        public BindingContextItem(string reuseKey, Key key, View blueprint)
        {
            ReuseKey = reuseKey;
            Key = key;
            Blueprint = blueprint;
        }
    }

    class ItemsViewTemplateSelector : xf.DataTemplateSelector
    {
        readonly Dictionary<xf.VisualElement, (Key Key, View Blueprint)> _renderedBlueprints
            = new Dictionary<xf.VisualElement, (Key, View)>();

        readonly Action<Signal> _dispatch;
        readonly Dictionary<string, xf.DataTemplate> _templates = new Dictionary<string, xf.DataTemplate>();

        internal ItemsViewTemplateSelector(Action<Signal> dispatch)
        {
            _dispatch = dispatch;
        }

        protected override xf.DataTemplate OnSelectTemplate(object item, xf.BindableObject container)
        {
            // TODO: trow:
            //On Android, there can be no more than 20 different data templates per ListView
            // https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/templates/data-templates/selector

            var contextItem = (BindingContextItem) item;
            if (!_templates.ContainsKey(contextItem.ReuseKey)) {
                var template = new xf.DataTemplate(() => {
                    var newRealView = (xf.View) Patch.CreateView((Element) contextItem.Blueprint);
                    var diff = Diff.Calculate(null, (Element)contextItem.Blueprint);
                    Patch.Apply(newRealView, diff, _dispatch);
                    newRealView.BindingContextChanged += OnBindingContextChanged;
                    _renderedBlueprints[newRealView] = (contextItem.Key, contextItem.Blueprint);
                    return newRealView;
                });
                _templates.Add(contextItem.ReuseKey, template);
            }

            return _templates[contextItem.ReuseKey];
        }

        void OnBindingContextChanged(object sender, EventArgs e)
        {
            var realView = (xf.VisualElement) sender;
            
            if (realView.BindingContext != null) {
                var contextItem = (BindingContextItem) realView.BindingContext;
                _renderedBlueprints.TryGetValue(realView, out var value);

                Patch.Apply(realView, Diff.Calculate((Element)value.Blueprint, (Element)contextItem.Blueprint), _dispatch);
                _renderedBlueprints[realView] = (contextItem.Key, contextItem.Blueprint);
            }
        }

        public void UpdateRendered(Key key, View newBlueprint)
        {
            foreach (var (view, rendered) in _renderedBlueprints.Select(x => (x.Key, x.Value)).ToArray()) {
                if (rendered.Key == key) {
                    var diff = Diff.Calculate((Element)rendered.Blueprint, (Element)newBlueprint);
                    Patch.Apply(view, diff, _dispatch);
                    _renderedBlueprints[view] = (key, newBlueprint);
                }
            }
        }
    }
}