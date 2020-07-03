using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
                        var real = Patch.CreateReal((Element) acv.View);
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
                    source.Add(new BindingContextItem(op.ReuseKey, op.View));

                view.ItemsSource = source;
                view.ItemTemplate = new ItemsViewTemplateSelector(dispatch);
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
                        AddChild acv => () => source.Add(new BindingContextItem(acv.ReuseKey, acv.View)),
                        _ => () => throw new InvalidOperationException($"Unknown Diff operation: {op.GetType()}")
                    };
                    patchAction();
                }
            }
        }
    }

    class BindingContextItem : INotifyPropertyChanged
    {
        public readonly string ReuseKey;
        public View View;

        public BindingContextItem(string reuseKey, View view)
        {
            ReuseKey = reuseKey;
            View = view;
        }

        public void UpdateView(View view)
        {
            View = view;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(View)));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }

    class ItemsViewTemplateSelector : xf.DataTemplateSelector
    {
        static readonly xf.BindableProperty RenderedBlueprintProperty = xf.BindableProperty.CreateAttached(
            nameof(RenderedBlueprintProperty), typeof(View), typeof(xf.View), null);

        readonly Action<Signal> _dispatch;
        readonly Dictionary<string, xf.DataTemplate> _templates = new Dictionary<string, xf.DataTemplate>();

        internal ItemsViewTemplateSelector(Action<Signal> dispatch) => _dispatch = dispatch;

        protected override xf.DataTemplate OnSelectTemplate(object item, xf.BindableObject container)
        {
            // TODO: trow:
            //On Android, there can be no more than 20 different data templates per ListView
            // https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/templates/data-templates/selector

            var bindingContextItem = (BindingContextItem) item;
            if (!_templates.ContainsKey(bindingContextItem.ReuseKey))
            {
                var template = new xf.DataTemplate(() =>
                {
                    var newRealView = Patch.CreateReal((Element) bindingContextItem.View);
                    newRealView.BindingContextChanged += OnBindingContextChanged;
                    return newRealView;
                });
                _templates.Add(bindingContextItem.ReuseKey, template);
            }

            return _templates[bindingContextItem.ReuseKey];
        }

        void OnBindingContextChanged(object sender, EventArgs e)
        {
            var realView = (xf.VisualElement) sender;
            var info = (BindingContextItem) realView.BindingContext;
            var diff = Diff.Calculate((View) realView.GetValue(RenderedBlueprintProperty), info.View);
            Patch.Apply(realView, diff, _dispatch);
            realView.SetValue(RenderedBlueprintProperty, info.View);
            info.PropertyChanged += (s, _) =>
            {
                var contextItem = (BindingContextItem) s;
                var renderedBlueprint = (View) realView.GetValue(RenderedBlueprintProperty);
                var newBlueprint = contextItem.View;
                if (ReferenceEquals(newBlueprint, renderedBlueprint))
                    return;

                Patch.Apply(realView, Diff.Calculate(renderedBlueprint, newBlueprint), _dispatch);
                realView.SetValue(RenderedBlueprintProperty, newBlueprint);
            };
        }
    }
}