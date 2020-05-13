using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using xf = Xamarin.Forms;

namespace Laconic
{
    public abstract partial class ItemsView<T> : View<T> where T : xf.ItemsView, new()
    {
        public Expression<Func<Signal>> RemainingItemsThresholdReached
        {
            set => SetEvent(nameof(RemainingItemsThresholdReached), value,
                (ctl, handler) => ctl.RemainingItemsThresholdReached += handler,
                (ctl, handler) => ctl.RemainingItemsThresholdReached -= handler);
        }
    }

    public partial class CarouselView : ItemsView<xf.CarouselView>
    {
        public Expression<Func<xf.CurrentItemChangedEventArgs, Signal>> CurrentItemChanged
        {
            set => SetEvent(nameof(CurrentItemChanged), value,
                (ctl, handler) => ctl.CurrentItemChanged += handler,
                (ctl, handler) => ctl.CurrentItemChanged -= handler);
        }

        public Expression<Func<xf.PositionChangedEventArgs, Signal>> PositionChanged
        {
            set => SetEvent(nameof(PositionChanged), value,
                (ctl, handler) => ctl.PositionChanged += handler,
                (ctl, handler) => ctl.PositionChanged -= handler);
        }
    }

    public abstract class StructuredItemsView<T> : ItemsView<T> where T : xf.StructuredItemsView, new()
    {
        public object Header
        {
            get => GetValue<object>(xf.StructuredItemsView.HeaderProperty);
            set => SetValue(xf.StructuredItemsView.HeaderProperty, value);
        }

        public object Footer
        {
            get => GetValue<object>(xf.StructuredItemsView.FooterProperty);
            set => SetValue(xf.StructuredItemsView.FooterProperty, value);
        }

        public xf.ItemSizingStrategy ItemSizingStrategy
        {
            get => GetValue<xf.ItemSizingStrategy>(xf.StructuredItemsView.ItemSizingStrategyProperty);
            set => SetValue(xf.StructuredItemsView.ItemSizingStrategyProperty, value);
        }
    }

    public abstract partial class SelectableItemsView<T> : StructuredItemsView<T>
        where T : xf.SelectableItemsView, new()
    {
        public IList<object> SelectedItems
        {
            get => GetValue<IList<object>>(xf.SelectableItemsView.SelectedItemsProperty);
            set => SetValue(xf.SelectableItemsView.SelectedItemsProperty, value);
        }
    }

    public class CollectionView : SelectableItemsView<xf.CollectionView>
    {
        public ItemsViewList Items { get; set; } = new ItemsViewList();

        public Expression<Func<xf.ItemsViewScrolledEventArgs, Signal>> Scrolled
        {
            set => SetEvent(nameof(Scrolled), value,
                (ctl, handler) => ctl.Scrolled += handler,
                (ctl, handler) => ctl.Scrolled -= handler);
        }

        public Expression<Func<xf.ScrollToRequestEventArgs, Signal>> ScrollToRequested
        {
            set => SetEvent(nameof(ScrollToRequested), value,
                (ctl, handler) => ctl.ScrollToRequested += handler,
                (ctl, handler) => ctl.ScrollToRequested -= handler);
        }
    }
}