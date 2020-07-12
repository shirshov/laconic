using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using xf = Xamarin.Forms;

namespace Laconic
{
    static class ViewListPatch
    {
        internal static void Apply(IList<xf.View> list, IEnumerable<IListOperation> operations, Action<Signal> dispatch)
        {
            foreach (var op in operations)
            {
                Action patchAction = op switch
                {
                    RemoveChild rc => () => list.RemoveAt(rc.Index),
                    UpdateChild uc => () => Patch.Apply(list[uc.Index], uc.Operations, dispatch),
                    ReplaceChild rc => () =>
                    {
                        var real = (xf.View) Patch.CreateReal((Element) rc.NewView);
                        Patch.Apply(real, rc.Operations, dispatch);
                        list[rc.Index] = real;
                    },
                    AddChild acv => () =>
                    {
                        var real = Patch.CreateReal((Element) acv.Blueprint);
                        Patch.Apply(real, acv.Operations, dispatch);
                        list.Insert(acv.Index, (xf.View) real);
                    },
                    _ => () => throw new InvalidOperationException($"Unknown Diff operation: {op.GetType()}")
                };
                patchAction();
            }
        }

        internal static void PatchItemsSource(xf.ItemsView view, UpdateItems update, Action<Signal> dispatch)
        {
            if (view.ItemsSource == null)
            {
                var source = new ObservableCollection<BindingContextItem>();

                foreach (var op in update.Operations.OfType<AddChild>())
                    source.Add(new BindingContextItem(op.ReuseKey, op.Blueprint));

                view.ItemTemplate = new ItemsViewTemplateSelector(dispatch);
                view.ItemsSource = source;
            }
            else
            {
                var source = (ObservableCollection<BindingContextItem>) view.ItemsSource;
                foreach (var op in update.Operations)
                {
                    Action patchAction = op switch
                    {
                        RemoveChild rc => () => source.RemoveAt(rc.Index),
                        UpdateChild uc => () =>
                        {
                            if (!uc.Operations.Any())
                                return;
                            source[uc.Index].UpdateView(uc.View);
                        },
                        ReplaceChild _ => () =>
                            throw new InvalidOperationException("ItemsViewList should never replace child views"),
                        AddChild acv => () => source.Add(new BindingContextItem(acv.ReuseKey, acv.Blueprint)),
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
        public View Blueprint;

        public BindingContextItem(string reuseKey, View view)
        {
            ReuseKey = reuseKey;
            Blueprint = view;
        }

        public Action<BindingContextItem, View>? UpdateRenderedView;
        
        public void UpdateView(View blueprint)
        {
            Blueprint = blueprint;
            UpdateRenderedView?.Invoke(this, blueprint);
        }
    }

    class ItemsViewTemplateSelector : xf.DataTemplateSelector
    {
        readonly Dictionary<xf.VisualElement, View> _renderedBlueprints = new Dictionary<xf.VisualElement, View>();
        
        readonly Action<Signal> _dispatch;
        readonly Dictionary<string, xf.DataTemplate> _templates = new Dictionary<string, xf.DataTemplate>();

        internal ItemsViewTemplateSelector(Action<Signal> dispatch) => _dispatch = dispatch;

        int templateNo = 1;
        
        protected override xf.DataTemplate OnSelectTemplate(object item, xf.BindableObject container)
        {
            // TODO: trow:
            //On Android, there can be no more than 20 different data templates per ListView
            // https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/templates/data-templates/selector

            var contextItem = (BindingContextItem) item;
            if (!_templates.ContainsKey(contextItem.ReuseKey))
            {
                var template = new xf.DataTemplate(() =>
                {
                    var newRealView = (xf.View)Patch.CreateReal((Element) contextItem.Blueprint);
                    var diff = Diff.Calculate(null, contextItem.Blueprint);
                    Patch.Apply(newRealView, diff, _dispatch);
                    newRealView.BindingContextChanged += OnBindingContextChanged;
                    _renderedBlueprints[newRealView] = contextItem.Blueprint;
                    return newRealView;
                });
                _templates.Add(contextItem.ReuseKey, template);
            }

            return _templates[contextItem.ReuseKey];
        }

        void OnBindingContextChanged(object sender, EventArgs e)
        {
            var realView = (xf.VisualElement) sender;

            var contextItem = (BindingContextItem) realView.BindingContext;
            _renderedBlueprints.TryGetValue(realView, out var renderedBlueprint);
			
            if (renderedBlueprint != null && renderedBlueprint != contextItem.Blueprint) {
                Patch.Apply(realView, Diff.Calculate(renderedBlueprint, contextItem.Blueprint), _dispatch);
                _renderedBlueprints[realView] = contextItem.Blueprint;
            }

            contextItem.UpdateRenderedView = (item, newBlueprint) =>
            {
				var rendered = _renderedBlueprints[realView];
                if (ReferenceEquals(newBlueprint, rendered))
                    return;

                Patch.Apply(realView, Diff.Calculate(rendered, newBlueprint), _dispatch);
				_renderedBlueprints[realView] = newBlueprint;
            };
        }
    }
}