using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;

namespace Laconic.CodeGen
{
    class Definitions
    {
        public static readonly Definitions NotUsed = new Definitions();
        public static readonly Definitions WrittenManually = new Definitions();
        public static readonly Definitions NotImplemented = new Definitions();


        static readonly Definitions All = new Definitions();

        public static readonly Dictionary<Type, Definitions> Defs = new Dictionary<Type, Definitions>
        {
            [typeof(AbsoluteLayout)] = WrittenManually,
            [typeof(ActivityIndicator)] = All,
            [typeof(AdaptiveTrigger)] = NotUsed,
            [typeof(Application)] = NotUsed,
            [typeof(AppLinkEntry)] = NotImplemented,
            [typeof(BackButtonBehavior)] = NotImplemented,
            [typeof(BaseMenuItem)] = NotImplemented,
            [typeof(BaseShellItem)] = NotImplemented,
            [typeof(Behavior)] = NotImplemented,
            [typeof(Behavior<>)] = NotImplemented,
            [typeof(BoxView)] = All,
            [typeof(Brush)] = WrittenManually,
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
            [typeof(ClickGestureRecognizer)] = NotImplemented,
            [typeof(CollectionView)] = WrittenManually,
            [typeof(ColumnDefinition)] = WrittenManually,
            [typeof(CompareStateTrigger)] = NotUsed,
            [typeof(ContentPage)] = WrittenManually,
            [typeof(ContentPresenter)] = NotUsed,
            [typeof(ContentView)] = WrittenManually,
            [typeof(DataTrigger)] = NotUsed,
            [typeof(DatePicker)] = All.ExceptManuallyWrittenEvents(nameof(DatePicker.DateSelected)),
            [typeof(DeviceStateTrigger)] = NotUsed,
            [typeof(Editor)] = All.WithoutBaseDeclaration().ExceptNotUsed(
                Editor.CharacterSpacingProperty,
                Editor.PlaceholderColorProperty,
                Editor.PlaceholderProperty,
                Editor.TextColorProperty,
                Editor.TextProperty),
            [typeof(Element)] = WrittenManually,
            [typeof(Entry)] = All.WithoutBaseDeclaration().ExceptNotUsed(
                Entry.ReturnCommandProperty,
                Entry.ReturnCommandParameterProperty,
                // The ones below are inherited from InputView
                Entry.CharacterSpacingProperty,
                Entry.PlaceholderColorProperty,
                Entry.PlaceholderProperty,
                Entry.TextColorProperty,
                Entry.TextProperty),
            [typeof(EntryCell)] = NotUsed,
            [typeof(EventTrigger)] = NotUsed,
            [typeof(FileImageSource)] = All.WithoutBaseDeclaration(),
            [typeof(FlexLayout)] = NotUsed,
            [typeof(FlyoutItem)] = NotImplemented,
            [typeof(FlyoutPage)] = NotImplemented,
            [typeof(FontImageSource)] = All.WithoutBaseDeclaration(),
            [typeof(FormattedString)] = WrittenManually,
            [typeof(Frame)] = All.WithoutBaseDeclaration(),
            [typeof(GestureElement)] = NotUsed,
            [typeof(GestureRecognizer)] = WrittenManually,
            [typeof(GradientBrush)] = WrittenManually,
            [typeof(GradientStop)] = WrittenManually,
            [typeof(Grid)] = All
                .WithoutBaseDeclaration()
                .ExceptWrittenManually(
                    Grid.RowDefinitionsProperty,
                    Grid.ColumnDefinitionsProperty,
                    Grid.RowProperty,
                    Grid.ColumnProperty,
                    Grid.RowSpanProperty,
                    Grid.ColumnSpanProperty),
            [typeof(GridItemsLayout)] = NotImplemented,
            [typeof(GroupableItemsView)] = NotUsed,
            [typeof(HtmlWebViewSource)] = NotUsed,
            [typeof(Image)] = All,
            [typeof(ImageButton)] = All.ExceptNotUsed(
                ImageButton.CommandProperty,
                ImageButton.CommandParameterProperty),
            [typeof(ImageCell)] = NotUsed,
            [typeof(ImageSource)] = WrittenManually,
            [typeof(IndicatorView)] = All
                .ExceptNotUsed(
                    IndicatorView.IndicatorTemplateProperty,
                    IndicatorView.ItemsSourceProperty),
            [typeof(InputView)] = All
                .WithoutBaseDeclaration()
                .TakeGenericParameter()
                .ExceptManuallyWrittenEvents(nameof(InputView.TextChanged)),
            [typeof(ItemsLayout)] = NotImplemented,
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
            [typeof(ItemsView<>)] = WrittenManually,
            [typeof(Label)] = All,
            [typeof(Layout)] = WrittenManually,
            [typeof(Layout<>)] = WrittenManually,
            [typeof(LinearGradientBrush)] = WrittenManually,
            [typeof(LinearItemsLayout)] = NotImplemented,
            [typeof(MasterDetailPage)] = NotUsed, // Obsolete in XF 5.0
            [typeof(Menu)] = NotImplemented,
            [typeof(MenuItem)] = NotImplemented,
            [typeof(MultiPage<>)] = NotUsed,
            [typeof(MultiTrigger)] = NotUsed,
            [typeof(NavigableElement)] = NotUsed,
            [typeof(NavigationPage)] = NotImplemented,
            [typeof(OpenGLView)] = NotImplemented,
            [typeof(OrientationStateTrigger)] = NotUsed,
            [typeof(Page)] = All.TakeGenericParameter().WithoutBaseDeclaration(),
            [typeof(PanGestureRecognizer)] = NotImplemented,
            [typeof(Picker)] = All
                .ExceptNotUsed(Picker.ItemsSourceProperty)
                .ExceptManuallyWrittenEvents(nameof(Picker.SelectedIndexChanged)),
            [typeof(PinchGestureRecognizer)] = NotImplemented,
            [typeof(ProgressBar)] = All,
            [typeof(RadialGradientBrush)] = WrittenManually,
            [typeof(RadioButton)] = All.ExceptManuallyWrittenEvents(nameof(RadioButton.CheckedChanged)),
            [typeof(RefreshView)] = All.WithoutBaseDeclaration().ExceptNotUsed(
                    RefreshView.CommandProperty,
                    RefreshView.CommandParameterProperty)
                .ExceptManuallyWrittenEvents(nameof(RefreshView.Refreshing)),
            [typeof(RowDefinition)] = WrittenManually,
            [typeof(RelativeLayout)] = NotImplemented,
            [typeof(ScrollView)] = All
                .WithoutBaseDeclaration()
                .ExceptManuallyWrittenEvents(nameof(ScrollView.Scrolled), nameof(ScrollView.ScrollToRequested)),
            [typeof(SearchBar)] = All.ExceptNotUsed(SearchBar.SearchCommandProperty, SearchBar.SearchCommandParameterProperty),
            [typeof(SearchHandler)] = NotUsed,
            [typeof(SelectableItemsView)] = All.WithoutBaseDeclaration()
                .TakeGenericParameter()
                .ExceptNotUsed(
                    SelectableItemsView.SelectionChangedCommandProperty,
                    SelectableItemsView.SelectionChangedCommandParameterProperty)
                .ExceptWrittenManually(SelectableItemsView.SelectedItemsProperty)
                .ExceptManuallyWrittenEvents(nameof(SelectableItemsView.SelectionChanged)),
            [typeof(Shell)] = NotImplemented,
            [typeof(ShellContent)] = NotImplemented,
            [typeof(ShellGroupItem)] = NotImplemented,
            [typeof(ShellItem)] = NotImplemented,
            [typeof(ShellSection)] = NotImplemented,
            [typeof(Slider)] = All
                .ExceptNotUsed(Slider.DragCompletedCommandProperty, Slider.DragStartedCommandProperty)
                .ExceptNotUsedEvents(nameof(Slider.ValueChanged)),
            [typeof(SolidColorBrush)] = WrittenManually,
            [typeof(Span)] = All
                .WithoutBaseDeclaration()
                .ExceptNotUsed(Span.FontProperty, Span.StyleProperty),
            
            [typeof(StackLayout)] = All.WithoutBaseDeclaration(),
            [typeof(StateTrigger)] = NotUsed,
            [typeof(StateTriggerBase)] = NotUsed,
            [typeof(Stepper)] = All.ExceptManuallyWrittenEvents(nameof(Stepper.ValueChanged)),
            [typeof(StreamImageSource)] = NotImplemented,
            [typeof(StructuredItemsView)] = All
                .ExceptNotUsed(
                    StructuredItemsView.FooterTemplateProperty,
                    StructuredItemsView.HeaderTemplateProperty),
            [typeof(SwipeGestureRecognizer)] = NotImplemented,
            [typeof(SwipeItem)] = All
                .WithoutBaseDeclaration()
                .ExceptManuallyWrittenEvents(nameof(SwipeItem.Invoked)),
            [typeof(SwipeItems)] = WrittenManually,
            [typeof(SwipeItemView)] = NotImplemented,
            [typeof(SwipeView)] = All
                .ExceptWrittenManually(
                    SwipeView.LeftItemsProperty,
                    SwipeView.RightItemsProperty,
                    SwipeView.TopItemsProperty,
                    SwipeView.BottomItemsProperty)
                .ExceptManuallyWrittenEvents(nameof(SwipeView.OpenRequested))
                .ExceptManuallyWrittenEvents(
                    nameof(SwipeView.SwipeChanging),
                    nameof(SwipeView.SwipeStarted),
                    nameof(SwipeView.SwipeEnded)),
            [typeof(Switch)] = All.ExceptManuallyWrittenEvents(nameof(Switch.Toggled)),
            [typeof(SwitchCell)] = NotUsed,
            [typeof(Tab)] = NotImplemented,
            [typeof(TabBar)] = NotImplemented,
            [typeof(TableSectionBase)] = NotUsed,
            [typeof(TableSectionBase<>)] = NotUsed,
            [typeof(TableView)] = NotUsed,
            [typeof(TapGestureRecognizer)] = WrittenManually,
            [typeof(TemplatedPage)] = NotUsed,
            [typeof(TemplatedView)] = NotUsed,
            [typeof(TextCell)] = NotUsed,
            [typeof(TimePicker)] = All,
            [typeof(ToolbarItem)] = WrittenManually,
            [typeof(Trigger)] = NotUsed,
            [typeof(TriggerBase)] = NotUsed,
            [typeof(UriImageSource)] = WrittenManually, // Requires calling Xamarin.Forms.Init()
            [typeof(UrlWebViewSource)] = NotUsed, 
            [typeof(View)] = WrittenManually,
            [typeof(ViewCell)] = NotUsed,
            [typeof(VisualElement)] = All.WithoutBaseDeclaration().TakeGenericParameter()
                .ExceptNotUsed(VisualElement.BehaviorsProperty, VisualElement.TriggersProperty)
                .ExceptWrittenManually(VisualElement.VisualProperty, VisualElement.BackgroundProperty)
                .ExceptNotUsedEvents(
                    nameof(VisualElement.BatchCommitted),
                    nameof(VisualElement.ChildrenReordered),
                    nameof(VisualElement.MeasureInvalidated),
                    nameof(VisualElement.SizeChanged),
                    nameof(VisualElement.FocusChangeRequested),
                    nameof(VisualElement.Focused),
                    nameof(VisualElement.Unfocused)
                ),
            [typeof(WebView)] = All
                .ExceptManuallyWrittenEvents(
                    nameof(WebView.EvalRequested), 
                    nameof(WebView.Navigated),
                    nameof(WebView.Navigating))
                .ExceptNotUsedEvents(nameof(WebView.EvaluateJavaScriptRequested)),
            [typeof(WebViewSource)] = NotUsed,
            // Shapes
            [typeof(Ellipse)] = WrittenManually,
            [typeof(EllipseGeometry)] = WrittenManually,
            [typeof(Geometry)] = WrittenManually,
            [typeof(GeometryGroup)] = NotImplemented,
            [typeof(Line)] = WrittenManually,
            [typeof(LineGeometry)] = WrittenManually,
            [typeof(LineSegment)] = NotImplemented,
            [typeof(Path)] = WrittenManually,
            [typeof(PathFigure)] = WrittenManually,
            [typeof(Polygon)] = WrittenManually,
            [typeof(Polyline)] = WrittenManually,
            [typeof(Xamarin.Forms.Shapes.Rectangle)] = WrittenManually,
            [typeof(Shape)] = WrittenManually,
            [typeof(ArcSegment)] = NotImplemented,
            [typeof(BezierSegment)] = NotImplemented,
            [typeof(CompositeTransform)] = NotImplemented,
            [typeof(MatrixTransform)] = NotImplemented,
            [typeof(PathGeometry)] = NotImplemented,
            [typeof(PathSegment)] = NotImplemented,
            [typeof(PolyBezierSegment)] = NotImplemented,
            [typeof(PolyLineSegment)] = NotImplemented,
            [typeof(PolyQuadraticBezierSegment)] = NotImplemented,
            [typeof(QuadraticBezierSegment)] = NotImplemented,
            [typeof(RectangleGeometry)] = NotImplemented,
            [typeof(RotateTransform)] = NotImplemented,
            [typeof(ScaleTransform)] = NotImplemented,
            [typeof(SkewTransform)] = NotImplemented,
            [typeof(Transform)] = NotImplemented,
            [typeof(TransformGroup)] = NotImplemented,
            [typeof(TranslateTransform)] = NotImplemented,
            // Misc
            [typeof(Xamarin.Forms.Internals.TemplatedItemsList<,>)] = NotUsed,
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