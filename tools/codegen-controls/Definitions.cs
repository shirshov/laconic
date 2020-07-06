using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Laconic.CodeGen
{
    class Definitions
    {
        public static readonly Definitions NotUsed = new Definitions();
        public static readonly Definitions WrittenManually = new Definitions();

        static readonly Definitions All = new Definitions();

        public static readonly Dictionary<Type, Definitions> Defs = new Dictionary<Type, Definitions>
        {
            [typeof(ActivityIndicator)] = NotUsed,
            [typeof(Application)] = NotUsed,
            [typeof(AppLinkEntry)] = NotUsed,
            [typeof(BackButtonBehavior)] = NotUsed,
            [typeof(BaseMenuItem)] = NotUsed,
            [typeof(BaseShellItem)] = NotUsed,
            [typeof(Behavior)] = NotUsed,
            // [typeof(Behavior`1)] = NotUsed,
            [typeof(BoxView)] = All,
            [typeof(Button)] = All
                .ExceptNotUsed(Button.CommandProperty, Button.CommandParameterProperty)
                .ExceptWrittenManually(Button.ContentLayoutProperty),
            [typeof(CarouselView)] = All
                .WithoutBaseDeclaration()
                .ExceptNotUsed(
                    CarouselView.CurrentItemChangedCommandProperty,
                    CarouselView.CurrentItemChangedCommandParameterProperty,
                    CarouselView.CurrentItemProperty,
                    CarouselView.PositionChangedCommandProperty,
                    CarouselView.PositionChangedCommandParameterProperty)
                .ExceptManuallyWrittenEvents(
                    nameof(CarouselView.CurrentItemChanged),
                    nameof(CarouselView.PositionChanged)),
            [typeof(Cell)] = NotUsed,
            [typeof(CheckBox)] = All
                .WithoutBaseDeclaration()
                .ExceptManuallyWrittenEvents(nameof(CheckBox.CheckedChanged)),
            [typeof(ClickGestureRecognizer)] = NotUsed,
            [typeof(CollectionView)] = WrittenManually,
            [typeof(ColumnDefinition)] = WrittenManually,
            [typeof(ContentPage)] =
                WrittenManually, //All.WithoutBaseDeclaration().ExceptWrittenManually(ContentPage.ContentProperty),
            [typeof(ContentPresenter)] = NotUsed,
            [typeof(ContentView)] =
                NotUsed, //All.WithoutBaseDeclaration().ExceptWrittenManually(ContentView.ContentProperty),
            [typeof(DataTrigger)] = NotUsed,
            [typeof(DatePicker)] = All.ExceptManuallyWrittenEvents(nameof(DatePicker.DateSelected)),
            [typeof(Editor)] = All,
            [typeof(Element)] = WrittenManually,
            [typeof(Entry)] = All.ExceptNotUsed(
                Entry.ReturnCommandProperty,
                Entry.ReturnCommandParameterProperty),
            [typeof(EntryCell)] = NotUsed,
            [typeof(EventTrigger)] = NotUsed,
            [typeof(FileImageSource)] = NotUsed,
            [typeof(FlyoutItem)] = NotUsed,
            [typeof(FontImageSource)] = NotUsed,
            [typeof(FormattedString)] = NotUsed,
            [typeof(Frame)] = All.WithoutBaseDeclaration(),
            [typeof(GestureElement)] = NotUsed,
            [typeof(GestureRecognizer)] = NotUsed,
            [typeof(Grid)] = All
                .WithoutBaseDeclaration()
                .ExceptWrittenManually(
                    Grid.RowDefinitionsProperty,
                    Grid.ColumnDefinitionsProperty,
                    Grid.RowProperty,
                    Grid.ColumnProperty,
                    Grid.RowSpanProperty,
                    Grid.ColumnSpanProperty),
            [typeof(GridItemsLayout)] = NotUsed,
            [typeof(GroupableItemsView)] = NotUsed,
            [typeof(HtmlWebViewSource)] = NotUsed,
            [typeof(Image)] = All,
            [typeof(ImageButton)] = All.ExceptNotUsed(
                ImageButton.CommandProperty,
                ImageButton.CommandParameterProperty),
            [typeof(ImageCell)] = NotUsed,
            [typeof(ImageSource)] = NotUsed,
            [typeof(IndicatorView)] = All
                .ExceptNotUsed(
                    IndicatorView.IndicatorTemplateProperty,
                    IndicatorView.ItemsSourceProperty),
            [typeof(InputView)] = NotUsed,
            [typeof(ItemsLayout)] = NotUsed,
            [typeof(ItemsView)] = All
                .WithoutBaseDeclaration()
                .TakeGenericParameter()
                .ExceptNotUsed(
                    ItemsView.ItemsSourceProperty,
                    ItemsView.ItemTemplateProperty,
                    ItemsView.RemainingItemsThresholdReachedCommandProperty,
                    ItemsView.RemainingItemsThresholdReachedCommandParameterProperty)
                .ExceptManuallyWrittenEvents(
                    nameof(ItemsView.Scrolled),
                    nameof(ItemsView.ScrollToRequested),
                    "RemainingItemsThresholdReached"),
            // [typeof(ItemsView`1)] = NotUsed,
            [typeof(Label)] = All,
            [typeof(Layout)] = NotUsed,
            // [typeof(Layout`1)] = NotUsed,
            [typeof(LinearItemsLayout)] = NotUsed,
            [typeof(MasterDetailPage)] = NotUsed,
            [typeof(Menu)] = NotUsed,
            [typeof(MenuItem)] = NotUsed,
            // [typeof(MultiPage`1)] = NotUsed,
            [typeof(MultiTrigger)] = NotUsed,
            [typeof(NavigableElement)] = NotUsed,
            [typeof(NavigationPage)] = NotUsed,
            [typeof(OpenGLView)] = NotUsed,
            [typeof(Page)] = All.TakeGenericParameter().WithoutBaseDeclaration(),
            [typeof(PanGestureRecognizer)] = NotUsed,
            [typeof(Picker)] = All
                .ExceptNotUsed(Picker.ItemsSourceProperty)
                .ExceptManuallyWrittenEvents(nameof(Picker.SelectedIndexChanged)),
            [typeof(PinchGestureRecognizer)] = NotUsed,
            [typeof(ProgressBar)] = All,
            [typeof(RefreshView)] = All.ExceptNotUsed(
                RefreshView.CommandProperty,
                RefreshView.CommandParameterProperty),
            [typeof(RowDefinition)] = NotUsed,
            [typeof(ScrollView)] = All
                .WithoutBaseDeclaration()
                .ExceptManuallyWrittenEvents(nameof(ScrollView.Scrolled), nameof(ScrollView.ScrollToRequested)),
            [typeof(SearchBar)] = All.ExceptNotUsed(SearchBar.SearchCommandProperty, SearchBar.SearchCommandProperty),
            [typeof(SearchHandler)] = NotUsed,
            [typeof(SelectableItemsView)] = All.WithoutBaseDeclaration()
                .TakeGenericParameter()
                .ExceptNotUsed(
                    SelectableItemsView.SelectionChangedCommandProperty,
                    SelectableItemsView.SelectionChangedCommandParameterProperty)
                .ExceptWrittenManually(SelectableItemsView.SelectedItemsProperty)
                .ExceptManuallyWrittenEvents(nameof(SelectableItemsView.SelectionChanged)),
            [typeof(Shell)] = NotUsed,
            [typeof(ShellContent)] = NotUsed,
            [typeof(ShellGroupItem)] = NotUsed,
            [typeof(ShellItem)] = NotUsed,
            [typeof(ShellSection)] = NotUsed,
            [typeof(Slider)] = All
                .ExceptNotUsed(Slider.DragCompletedCommandProperty, Slider.DragStartedCommandProperty)
                .ExceptNotUsedEvents(nameof(Slider.ValueChanged)),
            [typeof(Span)] = NotUsed,
            [typeof(StackLayout)] = All.WithoutBaseDeclaration(),
            [typeof(Stepper)] = NotUsed,
            [typeof(StreamImageSource)] = NotUsed,
            [typeof(StructuredItemsView)] = All,
            [typeof(SwipeGestureRecognizer)] = NotUsed,
            [typeof(SwipeItem)] = NotUsed,
            [typeof(SwipeItems)] = NotUsed,
            [typeof(SwipeItemView)] = NotUsed,
            [typeof(SwipeView)] = NotUsed,
            [typeof(Switch)] = All.ExceptNotUsedEvents(nameof(Switch.Toggled)),
            [typeof(SwitchCell)] = NotUsed,
            [typeof(Tab)] = NotUsed,
            [typeof(TabBar)] = NotUsed,
            [typeof(TableSectionBase)] = NotUsed,
            // [typeof(TableSectionBase`1)] = NotUsed,
            [typeof(TableView)] = NotUsed,
            [typeof(TapGestureRecognizer)] = NotUsed,
            // [typeof(TemplatedItemsList`2)] = NotUsed,
            [typeof(TemplatedPage)] = NotUsed,
            [typeof(TemplatedView)] = NotUsed,
            [typeof(TextCell)] = NotUsed,
            [typeof(TimePicker)] = All,
            [typeof(ToolbarItem)] = NotUsed,
            [typeof(Trigger)] = NotUsed,
            [typeof(TriggerBase)] = NotUsed,
            [typeof(UriImageSource)] = NotUsed,
            [typeof(UrlWebViewSource)] = NotUsed,
            [typeof(View)] = WrittenManually,
            [typeof(ViewCell)] = NotUsed,
            [typeof(VisualElement)] = All.WithoutBaseDeclaration().TakeGenericParameter()
                .ExceptNotUsed(VisualElement.BehaviorsProperty, VisualElement.TriggersProperty)
                .ExceptNotUsedEvents(
                    nameof(VisualElement.BatchCommitted),
                    nameof(VisualElement.ChildrenReordered),
                    nameof(VisualElement.MeasureInvalidated),
                    nameof(VisualElement.SizeChanged),
                    nameof(VisualElement.FocusChangeRequested),
                    nameof(VisualElement.Focused),
                    nameof(VisualElement.Unfocused)
                ),
            [typeof(WebView)] = NotUsed,
            [typeof(WebViewSource)] = NotUsed,
        };

        Definitions ExceptNotUsed(params BindableProperty[] props) => NewWithMore(props);
        Definitions ExceptWrittenManually(params BindableProperty[] props) => NewWithMore(props);

        Definitions ExceptNotUsedEvents(params string[] eventNames)
        {
            var res = new Definitions();
            res.SkipGeneration.AddRange(this.SkipGeneration);
            res.SkipGeneration.AddRange(eventNames);
            res.DoNotInherit = DoNotInherit;
            res.HasGenericParameter = HasGenericParameter;
            return res;
        }

        Definitions ExceptManuallyWrittenEvents(params string[] eventNames)
        {
            var res = new Definitions();
            res.SkipGeneration.AddRange(this.SkipGeneration);
            res.SkipGeneration.AddRange(eventNames);
            res.DoNotInherit = DoNotInherit;
            res.HasGenericParameter = HasGenericParameter;
            return res;
        }

        Definitions WithoutBaseDeclaration()
        {
            var res = NewWithMore(new BindableProperty[0]);
            res.DoNotInherit = true;
            return res;
        }

        public bool DoNotInherit;
        public bool HasGenericParameter;

        Definitions NewWithMore(BindableProperty[] props)
        {
            var res = new Definitions();
            res.SkipGeneration.AddRange(this.SkipGeneration);
            res.SkipGeneration.AddRange(props.Select(x => x.PropertyName));
            res.DoNotInherit = this.DoNotInherit;
            res.HasGenericParameter = this.HasGenericParameter;
            return res;
        }

        Definitions TakeGenericParameter()
        {
            var res = NewWithMore(new BindableProperty[0]);
            res.HasGenericParameter = true;
            return res;
        }

        public List<string> SkipGeneration { get; } = new List<string>();
    }
}