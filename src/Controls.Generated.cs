#nullable enable
using System.Net;
using Laconic.Shapes;
// ReSharper disable all

namespace Laconic;

public partial class ActivityIndicator : View<xf.ActivityIndicator>
{
    public Color Color
    {
        internal get => GetValue<Color>(xf.ActivityIndicator.ColorProperty);
        init => SetValue(xf.ActivityIndicator.ColorProperty, value);
    }
    public Boolean IsRunning
    {
        internal get => GetValue<Boolean>(xf.ActivityIndicator.IsRunningProperty);
        init => SetValue(xf.ActivityIndicator.IsRunningProperty, value);
    }
}
public partial class Border : View<xf.Border>
{
    public View Content
    {
        internal get => GetValue<View>(xf.Border.ContentProperty);
        init => SetValue(xf.Border.ContentProperty, value);
    }
    public Thickness Padding
    {
        internal get => GetValue<Thickness>(xf.Border.PaddingProperty);
        init => SetValue(xf.Border.PaddingProperty, value);
    }
    public xf.DoubleCollection StrokeDashArray
    {
        internal get => GetValue<xf.DoubleCollection>(xf.Border.StrokeDashArrayProperty);
        init => SetValue(xf.Border.StrokeDashArrayProperty, value);
    }
    public Double StrokeDashOffset
    {
        internal get => GetValue<Double>(xf.Border.StrokeDashOffsetProperty);
        init => SetValue(xf.Border.StrokeDashOffsetProperty, value);
    }
    public PenLineCap StrokeLineCap
    {
        internal get => GetValue<PenLineCap>(xf.Border.StrokeLineCapProperty);
        init => SetValue(xf.Border.StrokeLineCapProperty, value);
    }
    public PenLineJoin StrokeLineJoin
    {
        internal get => GetValue<PenLineJoin>(xf.Border.StrokeLineJoinProperty);
        init => SetValue(xf.Border.StrokeLineJoinProperty, value);
    }
    public Double StrokeMiterLimit
    {
        internal get => GetValue<Double>(xf.Border.StrokeMiterLimitProperty);
        init => SetValue(xf.Border.StrokeMiterLimitProperty, value);
    }
    public IShape StrokeShape
    {
        internal get => GetValue<IShape>(xf.Border.StrokeShapeProperty);
        init => SetValue(xf.Border.StrokeShapeProperty, value);
    }
    public Double StrokeThickness
    {
        internal get => GetValue<Double>(xf.Border.StrokeThicknessProperty);
        init => SetValue(xf.Border.StrokeThicknessProperty, value);
    }
}

public partial class BoxView : View<xf.BoxView>
{
    public Color Color
    {
        internal get => GetValue<Color>(xf.BoxView.ColorProperty);
        init => SetValue(xf.BoxView.ColorProperty, value);
    }
    public CornerRadius CornerRadius
    {
        internal get => GetValue<CornerRadius>(xf.BoxView.CornerRadiusProperty);
        init => SetValue(xf.BoxView.CornerRadiusProperty, value);
    }
}

public partial class Button : View<xf.Button>
{
    public Color BorderColor
    {
        internal get => GetValue<Color>(xf.Button.BorderColorProperty);
        init => SetValue(xf.Button.BorderColorProperty, value);
    }
    public Double BorderWidth
    {
        internal get => GetValue<Double>(xf.Button.BorderWidthProperty);
        init => SetValue(xf.Button.BorderWidthProperty, value);
    }
    public Double CharacterSpacing
    {
        internal get => GetValue<Double>(xf.Button.CharacterSpacingProperty);
        init => SetValue(xf.Button.CharacterSpacingProperty, value);
    }
    public Int32 CornerRadius
    {
        internal get => GetValue<Int32>(xf.Button.CornerRadiusProperty);
        init => SetValue(xf.Button.CornerRadiusProperty, value);
    }
    public FontAttributes FontAttributes
    {
        internal get => GetValue<FontAttributes>(xf.Button.FontAttributesProperty);
        init => SetValue(xf.Button.FontAttributesProperty, value);
    }
    public Boolean FontAutoScalingEnabled
    {
        internal get => GetValue<Boolean>(xf.Button.FontAutoScalingEnabledProperty);
        init => SetValue(xf.Button.FontAutoScalingEnabledProperty, value);
    }
    public String FontFamily
    {
        internal get => GetValue<String>(xf.Button.FontFamilyProperty);
        init => SetValue(xf.Button.FontFamilyProperty, value);
    }
    public Double FontSize
    {
        internal get => GetValue<Double>(xf.Button.FontSizeProperty);
        init => SetValue(xf.Button.FontSizeProperty, value);
    }
    public ImageSource ImageSource
    {
        internal get => GetValue<ImageSource>(xf.Button.ImageSourceProperty);
        init => SetValue(xf.Button.ImageSourceProperty, value);
    }
    public LineBreakMode LineBreakMode
    {
        internal get => GetValue<LineBreakMode>(xf.Button.LineBreakModeProperty);
        init => SetValue(xf.Button.LineBreakModeProperty, value);
    }
    public Thickness Padding
    {
        internal get => GetValue<Thickness>(xf.Button.PaddingProperty);
        init => SetValue(xf.Button.PaddingProperty, value);
    }
    public Color TextColor
    {
        internal get => GetValue<Color>(xf.Button.TextColorProperty);
        init => SetValue(xf.Button.TextColorProperty, value);
    }
    public String Text
    {
        internal get => GetValue<String>(xf.Button.TextProperty);
        init => SetValue(xf.Button.TextProperty, value);
    }
    public TextTransform TextTransform
    {
        internal get => GetValue<TextTransform>(xf.Button.TextTransformProperty);
        init => SetValue(xf.Button.TextTransformProperty, value);
    }
    public Func<Signal> Clicked
    {
        init => SetEvent(nameof(Clicked), value, (ctl, handler) => ctl.Clicked += handler, (ctl, handler) => ctl.Clicked -= handler);
    }
    public Func<Signal> Pressed
    {
        init => SetEvent(nameof(Pressed), value, (ctl, handler) => ctl.Pressed += handler, (ctl, handler) => ctl.Pressed -= handler);
    }
    public Func<Signal> Released
    {
        init => SetEvent(nameof(Released), value, (ctl, handler) => ctl.Released += handler, (ctl, handler) => ctl.Released -= handler);
    }
}

public partial class CarouselView
{
    public Boolean IsBounceEnabled
    {
        internal get => GetValue<Boolean>(xf.CarouselView.IsBounceEnabledProperty);
        init => SetValue(xf.CarouselView.IsBounceEnabledProperty, value);
    }
    public Boolean IsScrollAnimated
    {
        internal get => GetValue<Boolean>(xf.CarouselView.IsScrollAnimatedProperty);
        init => SetValue(xf.CarouselView.IsScrollAnimatedProperty, value);
    }
    public Boolean IsSwipeEnabled
    {
        internal get => GetValue<Boolean>(xf.CarouselView.IsSwipeEnabledProperty);
        init => SetValue(xf.CarouselView.IsSwipeEnabledProperty, value);
    }
    public xf.LinearItemsLayout ItemsLayout
    {
        internal get => GetValue<xf.LinearItemsLayout>(xf.CarouselView.ItemsLayoutProperty);
        init => SetValue(xf.CarouselView.ItemsLayoutProperty, value);
    }
    public Boolean Loop
    {
        internal get => GetValue<Boolean>(xf.CarouselView.LoopProperty);
        init => SetValue(xf.CarouselView.LoopProperty, value);
    }
    public Thickness PeekAreaInsets
    {
        internal get => GetValue<Thickness>(xf.CarouselView.PeekAreaInsetsProperty);
        init => SetValue(xf.CarouselView.PeekAreaInsetsProperty, value);
    }
    public Int32 Position
    {
        internal get => GetValue<Int32>(xf.CarouselView.PositionProperty);
        init => SetValue(xf.CarouselView.PositionProperty, value);
    }
}

public partial class CheckBox
{
    public Color Color
    {
        internal get => GetValue<Color>(xf.CheckBox.ColorProperty);
        init => SetValue(xf.CheckBox.ColorProperty, value);
    }
    public Boolean IsChecked
    {
        internal get => GetValue<Boolean>(xf.CheckBox.IsCheckedProperty);
        init => SetValue(xf.CheckBox.IsCheckedProperty, value);
    }
}

public partial class CollectionView
{
}

public partial class DatePicker : View<xf.DatePicker>
{
    public Double CharacterSpacing
    {
        internal get => GetValue<Double>(xf.DatePicker.CharacterSpacingProperty);
        init => SetValue(xf.DatePicker.CharacterSpacingProperty, value);
    }
    public DateTime Date
    {
        internal get => GetValue<DateTime>(xf.DatePicker.DateProperty);
        init => SetValue(xf.DatePicker.DateProperty, value);
    }
    public FontAttributes FontAttributes
    {
        internal get => GetValue<FontAttributes>(xf.DatePicker.FontAttributesProperty);
        init => SetValue(xf.DatePicker.FontAttributesProperty, value);
    }
    public Boolean FontAutoScalingEnabled
    {
        internal get => GetValue<Boolean>(xf.DatePicker.FontAutoScalingEnabledProperty);
        init => SetValue(xf.DatePicker.FontAutoScalingEnabledProperty, value);
    }
    public String FontFamily
    {
        internal get => GetValue<String>(xf.DatePicker.FontFamilyProperty);
        init => SetValue(xf.DatePicker.FontFamilyProperty, value);
    }
    public Double FontSize
    {
        internal get => GetValue<Double>(xf.DatePicker.FontSizeProperty);
        init => SetValue(xf.DatePicker.FontSizeProperty, value);
    }
    public String Format
    {
        internal get => GetValue<String>(xf.DatePicker.FormatProperty);
        init => SetValue(xf.DatePicker.FormatProperty, value);
    }
    public DateTime MaximumDate
    {
        internal get => GetValue<DateTime>(xf.DatePicker.MaximumDateProperty);
        init => SetValue(xf.DatePicker.MaximumDateProperty, value);
    }
    public DateTime MinimumDate
    {
        internal get => GetValue<DateTime>(xf.DatePicker.MinimumDateProperty);
        init => SetValue(xf.DatePicker.MinimumDateProperty, value);
    }
    public Color TextColor
    {
        internal get => GetValue<Color>(xf.DatePicker.TextColorProperty);
        init => SetValue(xf.DatePicker.TextColorProperty, value);
    }
    public TextTransform TextTransform
    {
        internal get => GetValue<TextTransform>(xf.DatePicker.TextTransformProperty);
        init => SetValue(xf.DatePicker.TextTransformProperty, value);
    }
}

public partial class Editor
{
    public EditorAutoSizeOption AutoSize
    {
        internal get => GetValue<EditorAutoSizeOption>(xf.Editor.AutoSizeProperty);
        init => SetValue(xf.Editor.AutoSizeProperty, value);
    }
    public Int32 CursorPosition
    {
        internal get => GetValue<Int32>(xf.Editor.CursorPositionProperty);
        init => SetValue(xf.Editor.CursorPositionProperty, value);
    }
    public FontAttributes FontAttributes
    {
        internal get => GetValue<FontAttributes>(xf.Editor.FontAttributesProperty);
        init => SetValue(xf.Editor.FontAttributesProperty, value);
    }
    public Boolean FontAutoScalingEnabled
    {
        internal get => GetValue<Boolean>(xf.Editor.FontAutoScalingEnabledProperty);
        init => SetValue(xf.Editor.FontAutoScalingEnabledProperty, value);
    }
    public String FontFamily
    {
        internal get => GetValue<String>(xf.Editor.FontFamilyProperty);
        init => SetValue(xf.Editor.FontFamilyProperty, value);
    }
    public Double FontSize
    {
        internal get => GetValue<Double>(xf.Editor.FontSizeProperty);
        init => SetValue(xf.Editor.FontSizeProperty, value);
    }
    public TextAlignment HorizontalTextAlignment
    {
        internal get => GetValue<TextAlignment>(xf.Editor.HorizontalTextAlignmentProperty);
        init => SetValue(xf.Editor.HorizontalTextAlignmentProperty, value);
    }
    public Boolean IsTextPredictionEnabled
    {
        internal get => GetValue<Boolean>(xf.Editor.IsTextPredictionEnabledProperty);
        init => SetValue(xf.Editor.IsTextPredictionEnabledProperty, value);
    }
    public Int32 SelectionLength
    {
        internal get => GetValue<Int32>(xf.Editor.SelectionLengthProperty);
        init => SetValue(xf.Editor.SelectionLengthProperty, value);
    }
    public TextAlignment VerticalTextAlignment
    {
        internal get => GetValue<TextAlignment>(xf.Editor.VerticalTextAlignmentProperty);
        init => SetValue(xf.Editor.VerticalTextAlignmentProperty, value);
    }
    public Func<Signal> Completed
    {
        init => SetEvent(nameof(Completed), value, (ctl, handler) => ctl.Completed += handler, (ctl, handler) => ctl.Completed -= handler);
    }
}

public partial class Entry
{
    public ClearButtonVisibility ClearButtonVisibility
    {
        internal get => GetValue<ClearButtonVisibility>(xf.Entry.ClearButtonVisibilityProperty);
        init => SetValue(xf.Entry.ClearButtonVisibilityProperty, value);
    }
    public Int32 CursorPosition
    {
        internal get => GetValue<Int32>(xf.Entry.CursorPositionProperty);
        init => SetValue(xf.Entry.CursorPositionProperty, value);
    }
    public FontAttributes FontAttributes
    {
        internal get => GetValue<FontAttributes>(xf.Entry.FontAttributesProperty);
        init => SetValue(xf.Entry.FontAttributesProperty, value);
    }
    public Boolean FontAutoScalingEnabled
    {
        internal get => GetValue<Boolean>(xf.Entry.FontAutoScalingEnabledProperty);
        init => SetValue(xf.Entry.FontAutoScalingEnabledProperty, value);
    }
    public String FontFamily
    {
        internal get => GetValue<String>(xf.Entry.FontFamilyProperty);
        init => SetValue(xf.Entry.FontFamilyProperty, value);
    }
    public Double FontSize
    {
        internal get => GetValue<Double>(xf.Entry.FontSizeProperty);
        init => SetValue(xf.Entry.FontSizeProperty, value);
    }
    public TextAlignment HorizontalTextAlignment
    {
        internal get => GetValue<TextAlignment>(xf.Entry.HorizontalTextAlignmentProperty);
        init => SetValue(xf.Entry.HorizontalTextAlignmentProperty, value);
    }
    public Boolean IsPassword
    {
        internal get => GetValue<Boolean>(xf.Entry.IsPasswordProperty);
        init => SetValue(xf.Entry.IsPasswordProperty, value);
    }
    public Boolean IsTextPredictionEnabled
    {
        internal get => GetValue<Boolean>(xf.Entry.IsTextPredictionEnabledProperty);
        init => SetValue(xf.Entry.IsTextPredictionEnabledProperty, value);
    }
    public Keyboard Keyboard
    {
        internal get => GetValue<Keyboard>(xf.Entry.KeyboardProperty);
        init => SetValue(xf.Entry.KeyboardProperty, value);
    }
    public ReturnType ReturnType
    {
        internal get => GetValue<ReturnType>(xf.Entry.ReturnTypeProperty);
        init => SetValue(xf.Entry.ReturnTypeProperty, value);
    }
    public Int32 SelectionLength
    {
        internal get => GetValue<Int32>(xf.Entry.SelectionLengthProperty);
        init => SetValue(xf.Entry.SelectionLengthProperty, value);
    }
    public TextAlignment VerticalTextAlignment
    {
        internal get => GetValue<TextAlignment>(xf.Entry.VerticalTextAlignmentProperty);
        init => SetValue(xf.Entry.VerticalTextAlignmentProperty, value);
    }
    public Func<Signal> Completed
    {
        init => SetEvent(nameof(Completed), value, (ctl, handler) => ctl.Completed += handler, (ctl, handler) => ctl.Completed -= handler);
    }
}

public partial class FileImageSource
{
    public String File
    {
        internal get => GetValue<String>(xf.FileImageSource.FileProperty);
        init => SetValue(xf.FileImageSource.FileProperty, value);
    }
}

public partial class FlyoutPage
{
    public FlyoutLayoutBehavior FlyoutLayoutBehavior
    {
        internal get => GetValue<FlyoutLayoutBehavior>(xf.FlyoutPage.FlyoutLayoutBehaviorProperty);
        init => SetValue(xf.FlyoutPage.FlyoutLayoutBehaviorProperty, value);
    }
    public Boolean IsGestureEnabled
    {
        internal get => GetValue<Boolean>(xf.FlyoutPage.IsGestureEnabledProperty);
        init => SetValue(xf.FlyoutPage.IsGestureEnabledProperty, value);
    }
    public Boolean IsPresented
    {
        internal get => GetValue<Boolean>(xf.FlyoutPage.IsPresentedProperty);
        init => SetValue(xf.FlyoutPage.IsPresentedProperty, value);
    }
    public Func<Signal> IsPresentedChanged
    {
        init => SetEvent(nameof(IsPresentedChanged), value, (ctl, handler) => ctl.IsPresentedChanged += handler, (ctl, handler) => ctl.IsPresentedChanged -= handler);
    }
}

public partial class FontImageSource
{
    public Color Color
    {
        internal get => GetValue<Color>(xf.FontImageSource.ColorProperty);
        init => SetValue(xf.FontImageSource.ColorProperty, value);
    }
    public Boolean FontAutoScalingEnabled
    {
        internal get => GetValue<Boolean>(xf.FontImageSource.FontAutoScalingEnabledProperty);
        init => SetValue(xf.FontImageSource.FontAutoScalingEnabledProperty, value);
    }
    public String FontFamily
    {
        internal get => GetValue<String>(xf.FontImageSource.FontFamilyProperty);
        init => SetValue(xf.FontImageSource.FontFamilyProperty, value);
    }
    public String Glyph
    {
        internal get => GetValue<String>(xf.FontImageSource.GlyphProperty);
        init => SetValue(xf.FontImageSource.GlyphProperty, value);
    }
    public Double Size
    {
        internal get => GetValue<Double>(xf.FontImageSource.SizeProperty);
        init => SetValue(xf.FontImageSource.SizeProperty, value);
    }
}

public partial class Grid
{
    public Double ColumnSpacing
    {
        internal get => GetValue<Double>(xf.Grid.ColumnSpacingProperty);
        init => SetValue(xf.Grid.ColumnSpacingProperty, value);
    }
    public Double RowSpacing
    {
        internal get => GetValue<Double>(xf.Grid.RowSpacingProperty);
        init => SetValue(xf.Grid.RowSpacingProperty, value);
    }
}

public partial class GridItemsLayout
{
    public Double HorizontalItemSpacing
    {
        internal get => GetValue<Double>(xf.GridItemsLayout.HorizontalItemSpacingProperty);
        init => SetValue(xf.GridItemsLayout.HorizontalItemSpacingProperty, value);
    }
    public Int32 Span
    {
        internal get => GetValue<Int32>(xf.GridItemsLayout.SpanProperty);
        init => SetValue(xf.GridItemsLayout.SpanProperty, value);
    }
    public Double VerticalItemSpacing
    {
        internal get => GetValue<Double>(xf.GridItemsLayout.VerticalItemSpacingProperty);
        init => SetValue(xf.GridItemsLayout.VerticalItemSpacingProperty, value);
    }
}

public partial class GroupableItemsView<T>
{
    public Boolean IsGrouped
    {
        internal get => GetValue<Boolean>(xf.GroupableItemsView.IsGroupedProperty);
        init => SetValue(xf.GroupableItemsView.IsGroupedProperty, value);
    }
}

public partial class HorizontalStackLayout
{
}

public partial class Image : View<xf.Image>
{
    public Aspect Aspect
    {
        internal get => GetValue<Aspect>(xf.Image.AspectProperty);
        init => SetValue(xf.Image.AspectProperty, value);
    }
    public Boolean IsAnimationPlaying
    {
        internal get => GetValue<Boolean>(xf.Image.IsAnimationPlayingProperty);
        init => SetValue(xf.Image.IsAnimationPlayingProperty, value);
    }
    public Boolean IsOpaque
    {
        internal get => GetValue<Boolean>(xf.Image.IsOpaqueProperty);
        init => SetValue(xf.Image.IsOpaqueProperty, value);
    }
    public ImageSource Source
    {
        internal get => GetValue<ImageSource>(xf.Image.SourceProperty);
        init => SetValue(xf.Image.SourceProperty, value);
    }
}

public partial class ImageButton : View<xf.ImageButton>
{
    public Aspect Aspect
    {
        internal get => GetValue<Aspect>(xf.ImageButton.AspectProperty);
        init => SetValue(xf.ImageButton.AspectProperty, value);
    }
    public Color BorderColor
    {
        internal get => GetValue<Color>(xf.ImageButton.BorderColorProperty);
        init => SetValue(xf.ImageButton.BorderColorProperty, value);
    }
    public Double BorderWidth
    {
        internal get => GetValue<Double>(xf.ImageButton.BorderWidthProperty);
        init => SetValue(xf.ImageButton.BorderWidthProperty, value);
    }
    public Int32 CornerRadius
    {
        internal get => GetValue<Int32>(xf.ImageButton.CornerRadiusProperty);
        init => SetValue(xf.ImageButton.CornerRadiusProperty, value);
    }
    public Boolean IsOpaque
    {
        internal get => GetValue<Boolean>(xf.ImageButton.IsOpaqueProperty);
        init => SetValue(xf.ImageButton.IsOpaqueProperty, value);
    }
    public Thickness Padding
    {
        internal get => GetValue<Thickness>(xf.ImageButton.PaddingProperty);
        init => SetValue(xf.ImageButton.PaddingProperty, value);
    }
    public ImageSource Source
    {
        internal get => GetValue<ImageSource>(xf.ImageButton.SourceProperty);
        init => SetValue(xf.ImageButton.SourceProperty, value);
    }
    public Func<Signal> Clicked
    {
        init => SetEvent(nameof(Clicked), value, (ctl, handler) => ctl.Clicked += handler, (ctl, handler) => ctl.Clicked -= handler);
    }
    public Func<Signal> Pressed
    {
        init => SetEvent(nameof(Pressed), value, (ctl, handler) => ctl.Pressed += handler, (ctl, handler) => ctl.Pressed -= handler);
    }
    public Func<Signal> Released
    {
        init => SetEvent(nameof(Released), value, (ctl, handler) => ctl.Released += handler, (ctl, handler) => ctl.Released -= handler);
    }
}

public partial class IndicatorView : View<xf.IndicatorView>
{
    public Int32 Count
    {
        internal get => GetValue<Int32>(xf.IndicatorView.CountProperty);
        init => SetValue(xf.IndicatorView.CountProperty, value);
    }
    public Boolean HideSingle
    {
        internal get => GetValue<Boolean>(xf.IndicatorView.HideSingleProperty);
        init => SetValue(xf.IndicatorView.HideSingleProperty, value);
    }
    public Color IndicatorColor
    {
        internal get => GetValue<Color>(xf.IndicatorView.IndicatorColorProperty);
        init => SetValue(xf.IndicatorView.IndicatorColorProperty, value);
    }
    public Double IndicatorSize
    {
        internal get => GetValue<Double>(xf.IndicatorView.IndicatorSizeProperty);
        init => SetValue(xf.IndicatorView.IndicatorSizeProperty, value);
    }
    public IndicatorShape IndicatorsShape
    {
        internal get => GetValue<IndicatorShape>(xf.IndicatorView.IndicatorsShapeProperty);
        init => SetValue(xf.IndicatorView.IndicatorsShapeProperty, value);
    }
    public Int32 MaximumVisible
    {
        internal get => GetValue<Int32>(xf.IndicatorView.MaximumVisibleProperty);
        init => SetValue(xf.IndicatorView.MaximumVisibleProperty, value);
    }
    public Int32 Position
    {
        internal get => GetValue<Int32>(xf.IndicatorView.PositionProperty);
        init => SetValue(xf.IndicatorView.PositionProperty, value);
    }
    public Color SelectedIndicatorColor
    {
        internal get => GetValue<Color>(xf.IndicatorView.SelectedIndicatorColorProperty);
        init => SetValue(xf.IndicatorView.SelectedIndicatorColorProperty, value);
    }
}

public partial class InputView<T>
{
    public Double CharacterSpacing
    {
        internal get => GetValue<Double>(xf.InputView.CharacterSpacingProperty);
        init => SetValue(xf.InputView.CharacterSpacingProperty, value);
    }
    public Boolean IsReadOnly
    {
        internal get => GetValue<Boolean>(xf.InputView.IsReadOnlyProperty);
        init => SetValue(xf.InputView.IsReadOnlyProperty, value);
    }
    public Boolean IsSpellCheckEnabled
    {
        internal get => GetValue<Boolean>(xf.InputView.IsSpellCheckEnabledProperty);
        init => SetValue(xf.InputView.IsSpellCheckEnabledProperty, value);
    }
    public Keyboard Keyboard
    {
        internal get => GetValue<Keyboard>(xf.InputView.KeyboardProperty);
        init => SetValue(xf.InputView.KeyboardProperty, value);
    }
    public Int32 MaxLength
    {
        internal get => GetValue<Int32>(xf.InputView.MaxLengthProperty);
        init => SetValue(xf.InputView.MaxLengthProperty, value);
    }
    public Color PlaceholderColor
    {
        internal get => GetValue<Color>(xf.InputView.PlaceholderColorProperty);
        init => SetValue(xf.InputView.PlaceholderColorProperty, value);
    }
    public String Placeholder
    {
        internal get => GetValue<String>(xf.InputView.PlaceholderProperty);
        init => SetValue(xf.InputView.PlaceholderProperty, value);
    }
    public Color TextColor
    {
        internal get => GetValue<Color>(xf.InputView.TextColorProperty);
        init => SetValue(xf.InputView.TextColorProperty, value);
    }
    public String Text
    {
        internal get => GetValue<String>(xf.InputView.TextProperty);
        init => SetValue(xf.InputView.TextProperty, value);
    }
    public TextTransform TextTransform
    {
        internal get => GetValue<TextTransform>(xf.InputView.TextTransformProperty);
        init => SetValue(xf.InputView.TextTransformProperty, value);
    }
}

public abstract partial class ItemsLayout<T>
{
    public SnapPointsAlignment SnapPointsAlignment
    {
        internal get => GetValue<SnapPointsAlignment>(xf.ItemsLayout.SnapPointsAlignmentProperty);
        init => SetValue(xf.ItemsLayout.SnapPointsAlignmentProperty, value);
    }
    public SnapPointsType SnapPointsType
    {
        internal get => GetValue<SnapPointsType>(xf.ItemsLayout.SnapPointsTypeProperty);
        init => SetValue(xf.ItemsLayout.SnapPointsTypeProperty, value);
    }
}

public abstract partial class ItemsView<T>
{
    public Object EmptyView
    {
        internal get => GetValue<Object>(xf.ItemsView.EmptyViewProperty);
        init => SetValue(xf.ItemsView.EmptyViewProperty, value);
    }
    public ScrollBarVisibility HorizontalScrollBarVisibility
    {
        internal get => GetValue<ScrollBarVisibility>(xf.ItemsView.HorizontalScrollBarVisibilityProperty);
        init => SetValue(xf.ItemsView.HorizontalScrollBarVisibilityProperty, value);
    }
    public ItemsUpdatingScrollMode ItemsUpdatingScrollMode
    {
        internal get => GetValue<ItemsUpdatingScrollMode>(xf.ItemsView.ItemsUpdatingScrollModeProperty);
        init => SetValue(xf.ItemsView.ItemsUpdatingScrollModeProperty, value);
    }
    public Int32 RemainingItemsThreshold
    {
        internal get => GetValue<Int32>(xf.ItemsView.RemainingItemsThresholdProperty);
        init => SetValue(xf.ItemsView.RemainingItemsThresholdProperty, value);
    }
    public ScrollBarVisibility VerticalScrollBarVisibility
    {
        internal get => GetValue<ScrollBarVisibility>(xf.ItemsView.VerticalScrollBarVisibilityProperty);
        init => SetValue(xf.ItemsView.VerticalScrollBarVisibilityProperty, value);
    }
}

public partial class Label : View<xf.Label>
{
    public Double CharacterSpacing
    {
        internal get => GetValue<Double>(xf.Label.CharacterSpacingProperty);
        init => SetValue(xf.Label.CharacterSpacingProperty, value);
    }
    public FontAttributes FontAttributes
    {
        internal get => GetValue<FontAttributes>(xf.Label.FontAttributesProperty);
        init => SetValue(xf.Label.FontAttributesProperty, value);
    }
    public Boolean FontAutoScalingEnabled
    {
        internal get => GetValue<Boolean>(xf.Label.FontAutoScalingEnabledProperty);
        init => SetValue(xf.Label.FontAutoScalingEnabledProperty, value);
    }
    public String FontFamily
    {
        internal get => GetValue<String>(xf.Label.FontFamilyProperty);
        init => SetValue(xf.Label.FontFamilyProperty, value);
    }
    public Double FontSize
    {
        internal get => GetValue<Double>(xf.Label.FontSizeProperty);
        init => SetValue(xf.Label.FontSizeProperty, value);
    }
    public FormattedString FormattedText
    {
        internal get => GetValue<FormattedString>(xf.Label.FormattedTextProperty);
        init => SetValue(xf.Label.FormattedTextProperty, value);
    }
    public TextAlignment HorizontalTextAlignment
    {
        internal get => GetValue<TextAlignment>(xf.Label.HorizontalTextAlignmentProperty);
        init => SetValue(xf.Label.HorizontalTextAlignmentProperty, value);
    }
    public LineBreakMode LineBreakMode
    {
        internal get => GetValue<LineBreakMode>(xf.Label.LineBreakModeProperty);
        init => SetValue(xf.Label.LineBreakModeProperty, value);
    }
    public Double LineHeight
    {
        internal get => GetValue<Double>(xf.Label.LineHeightProperty);
        init => SetValue(xf.Label.LineHeightProperty, value);
    }
    public Int32 MaxLines
    {
        internal get => GetValue<Int32>(xf.Label.MaxLinesProperty);
        init => SetValue(xf.Label.MaxLinesProperty, value);
    }
    public Thickness Padding
    {
        internal get => GetValue<Thickness>(xf.Label.PaddingProperty);
        init => SetValue(xf.Label.PaddingProperty, value);
    }
    public Color TextColor
    {
        internal get => GetValue<Color>(xf.Label.TextColorProperty);
        init => SetValue(xf.Label.TextColorProperty, value);
    }
    public TextDecorations TextDecorations
    {
        internal get => GetValue<TextDecorations>(xf.Label.TextDecorationsProperty);
        init => SetValue(xf.Label.TextDecorationsProperty, value);
    }
    public String Text
    {
        internal get => GetValue<String>(xf.Label.TextProperty);
        init => SetValue(xf.Label.TextProperty, value);
    }
    public TextTransform TextTransform
    {
        internal get => GetValue<TextTransform>(xf.Label.TextTransformProperty);
        init => SetValue(xf.Label.TextTransformProperty, value);
    }
    public TextType TextType
    {
        internal get => GetValue<TextType>(xf.Label.TextTypeProperty);
        init => SetValue(xf.Label.TextTypeProperty, value);
    }
    public TextAlignment VerticalTextAlignment
    {
        internal get => GetValue<TextAlignment>(xf.Label.VerticalTextAlignmentProperty);
        init => SetValue(xf.Label.VerticalTextAlignmentProperty, value);
    }
}

public partial class LinearItemsLayout
{
    public Double ItemSpacing
    {
        internal get => GetValue<Double>(xf.LinearItemsLayout.ItemSpacingProperty);
        init => SetValue(xf.LinearItemsLayout.ItemSpacingProperty, value);
    }
}

public partial class NavigationPage
{
    public Color BarBackgroundColor
    {
        internal get => GetValue<Color>(xf.NavigationPage.BarBackgroundColorProperty);
        init => SetValue(xf.NavigationPage.BarBackgroundColorProperty, value);
    }
    public Brush BarBackground
    {
        internal get => GetValue<Brush>(xf.NavigationPage.BarBackgroundProperty);
        init => SetValue(xf.NavigationPage.BarBackgroundProperty, value);
    }
    public Color BarTextColor
    {
        internal get => GetValue<Color>(xf.NavigationPage.BarTextColorProperty);
        init => SetValue(xf.NavigationPage.BarTextColorProperty, value);
    }
}

public partial class Page<T>
{
    public ImageSource BackgroundImageSource
    {
        internal get => GetValue<ImageSource>(xf.Page.BackgroundImageSourceProperty);
        init => SetValue(xf.Page.BackgroundImageSourceProperty, value);
    }
    public ImageSource IconImageSource
    {
        internal get => GetValue<ImageSource>(xf.Page.IconImageSourceProperty);
        init => SetValue(xf.Page.IconImageSourceProperty, value);
    }
    public Boolean IsBusy
    {
        internal get => GetValue<Boolean>(xf.Page.IsBusyProperty);
        init => SetValue(xf.Page.IsBusyProperty, value);
    }
    public Thickness Padding
    {
        internal get => GetValue<Thickness>(xf.Page.PaddingProperty);
        init => SetValue(xf.Page.PaddingProperty, value);
    }
    public String Title
    {
        internal get => GetValue<String>(xf.Page.TitleProperty);
        init => SetValue(xf.Page.TitleProperty, value);
    }
    public Func<Signal> Appearing
    {
        init => SetEvent(nameof(Appearing), value, (ctl, handler) => ctl.Appearing += handler, (ctl, handler) => ctl.Appearing -= handler);
    }
    public Func<Signal> Disappearing
    {
        init => SetEvent(nameof(Disappearing), value, (ctl, handler) => ctl.Disappearing += handler, (ctl, handler) => ctl.Disappearing -= handler);
    }
    public Func<Signal> LayoutChanged
    {
        init => SetEvent(nameof(LayoutChanged), value, (ctl, handler) => ctl.LayoutChanged += handler, (ctl, handler) => ctl.LayoutChanged -= handler);
    }
}

public partial class Picker : View<xf.Picker>
{
    public Double CharacterSpacing
    {
        internal get => GetValue<Double>(xf.Picker.CharacterSpacingProperty);
        init => SetValue(xf.Picker.CharacterSpacingProperty, value);
    }
    public FontAttributes FontAttributes
    {
        internal get => GetValue<FontAttributes>(xf.Picker.FontAttributesProperty);
        init => SetValue(xf.Picker.FontAttributesProperty, value);
    }
    public Boolean FontAutoScalingEnabled
    {
        internal get => GetValue<Boolean>(xf.Picker.FontAutoScalingEnabledProperty);
        init => SetValue(xf.Picker.FontAutoScalingEnabledProperty, value);
    }
    public String FontFamily
    {
        internal get => GetValue<String>(xf.Picker.FontFamilyProperty);
        init => SetValue(xf.Picker.FontFamilyProperty, value);
    }
    public Double FontSize
    {
        internal get => GetValue<Double>(xf.Picker.FontSizeProperty);
        init => SetValue(xf.Picker.FontSizeProperty, value);
    }
    public TextAlignment HorizontalTextAlignment
    {
        internal get => GetValue<TextAlignment>(xf.Picker.HorizontalTextAlignmentProperty);
        init => SetValue(xf.Picker.HorizontalTextAlignmentProperty, value);
    }
    public Int32 SelectedIndex
    {
        internal get => GetValue<Int32>(xf.Picker.SelectedIndexProperty);
        init => SetValue(xf.Picker.SelectedIndexProperty, value);
    }
    public Object SelectedItem
    {
        internal get => GetValue<Object>(xf.Picker.SelectedItemProperty);
        init => SetValue(xf.Picker.SelectedItemProperty, value);
    }
    public Color TextColor
    {
        internal get => GetValue<Color>(xf.Picker.TextColorProperty);
        init => SetValue(xf.Picker.TextColorProperty, value);
    }
    public TextTransform TextTransform
    {
        internal get => GetValue<TextTransform>(xf.Picker.TextTransformProperty);
        init => SetValue(xf.Picker.TextTransformProperty, value);
    }
    public Color TitleColor
    {
        internal get => GetValue<Color>(xf.Picker.TitleColorProperty);
        init => SetValue(xf.Picker.TitleColorProperty, value);
    }
    public String Title
    {
        internal get => GetValue<String>(xf.Picker.TitleProperty);
        init => SetValue(xf.Picker.TitleProperty, value);
    }
    public TextAlignment VerticalTextAlignment
    {
        internal get => GetValue<TextAlignment>(xf.Picker.VerticalTextAlignmentProperty);
        init => SetValue(xf.Picker.VerticalTextAlignmentProperty, value);
    }
}

public partial class ProgressBar : View<xf.ProgressBar>
{
    public Color ProgressColor
    {
        internal get => GetValue<Color>(xf.ProgressBar.ProgressColorProperty);
        init => SetValue(xf.ProgressBar.ProgressColorProperty, value);
    }
    public Double Progress
    {
        internal get => GetValue<Double>(xf.ProgressBar.ProgressProperty);
        init => SetValue(xf.ProgressBar.ProgressProperty, value);
    }
}

public partial class RadioButton : View<xf.RadioButton>
{
    public Color BorderColor
    {
        internal get => GetValue<Color>(xf.RadioButton.BorderColorProperty);
        init => SetValue(xf.RadioButton.BorderColorProperty, value);
    }
    public Double BorderWidth
    {
        internal get => GetValue<Double>(xf.RadioButton.BorderWidthProperty);
        init => SetValue(xf.RadioButton.BorderWidthProperty, value);
    }
    public Double CharacterSpacing
    {
        internal get => GetValue<Double>(xf.RadioButton.CharacterSpacingProperty);
        init => SetValue(xf.RadioButton.CharacterSpacingProperty, value);
    }
    public Object Content
    {
        internal get => GetValue<Object>(xf.RadioButton.ContentProperty);
        init => SetValue(xf.RadioButton.ContentProperty, value);
    }
    public Int32 CornerRadius
    {
        internal get => GetValue<Int32>(xf.RadioButton.CornerRadiusProperty);
        init => SetValue(xf.RadioButton.CornerRadiusProperty, value);
    }
    public FontAttributes FontAttributes
    {
        internal get => GetValue<FontAttributes>(xf.RadioButton.FontAttributesProperty);
        init => SetValue(xf.RadioButton.FontAttributesProperty, value);
    }
    public Boolean FontAutoScalingEnabled
    {
        internal get => GetValue<Boolean>(xf.RadioButton.FontAutoScalingEnabledProperty);
        init => SetValue(xf.RadioButton.FontAutoScalingEnabledProperty, value);
    }
    public String FontFamily
    {
        internal get => GetValue<String>(xf.RadioButton.FontFamilyProperty);
        init => SetValue(xf.RadioButton.FontFamilyProperty, value);
    }
    public Double FontSize
    {
        internal get => GetValue<Double>(xf.RadioButton.FontSizeProperty);
        init => SetValue(xf.RadioButton.FontSizeProperty, value);
    }
    public String GroupName
    {
        internal get => GetValue<String>(xf.RadioButton.GroupNameProperty);
        init => SetValue(xf.RadioButton.GroupNameProperty, value);
    }
    public Boolean IsChecked
    {
        internal get => GetValue<Boolean>(xf.RadioButton.IsCheckedProperty);
        init => SetValue(xf.RadioButton.IsCheckedProperty, value);
    }
    public Color TextColor
    {
        internal get => GetValue<Color>(xf.RadioButton.TextColorProperty);
        init => SetValue(xf.RadioButton.TextColorProperty, value);
    }
    public TextTransform TextTransform
    {
        internal get => GetValue<TextTransform>(xf.RadioButton.TextTransformProperty);
        init => SetValue(xf.RadioButton.TextTransformProperty, value);
    }
    public Object Value
    {
        internal get => GetValue<Object>(xf.RadioButton.ValueProperty);
        init => SetValue(xf.RadioButton.ValueProperty, value);
    }
}

public partial class RefreshView
{
    public Boolean IsRefreshing
    {
        internal get => GetValue<Boolean>(xf.RefreshView.IsRefreshingProperty);
        init => SetValue(xf.RefreshView.IsRefreshingProperty, value);
    }
    public Color RefreshColor
    {
        internal get => GetValue<Color>(xf.RefreshView.RefreshColorProperty);
        init => SetValue(xf.RefreshView.RefreshColorProperty, value);
    }
}

public partial class ReorderableItemsView<T>
{
    public Boolean CanMixGroups
    {
        internal get => GetValue<Boolean>(xf.ReorderableItemsView.CanMixGroupsProperty);
        init => SetValue(xf.ReorderableItemsView.CanMixGroupsProperty, value);
    }
    public Boolean CanReorderItems
    {
        internal get => GetValue<Boolean>(xf.ReorderableItemsView.CanReorderItemsProperty);
        init => SetValue(xf.ReorderableItemsView.CanReorderItemsProperty, value);
    }
    public Func<Signal> ReorderCompleted
    {
        init => SetEvent(nameof(ReorderCompleted), value, (ctl, handler) => ctl.ReorderCompleted += handler, (ctl, handler) => ctl.ReorderCompleted -= handler);
    }
}

public partial class ScrollView
{
    public ScrollBarVisibility HorizontalScrollBarVisibility
    {
        internal get => GetValue<ScrollBarVisibility>(xf.ScrollView.HorizontalScrollBarVisibilityProperty);
        init => SetValue(xf.ScrollView.HorizontalScrollBarVisibilityProperty, value);
    }
    public ScrollOrientation Orientation
    {
        internal get => GetValue<ScrollOrientation>(xf.ScrollView.OrientationProperty);
        init => SetValue(xf.ScrollView.OrientationProperty, value);
    }
    public ScrollBarVisibility VerticalScrollBarVisibility
    {
        internal get => GetValue<ScrollBarVisibility>(xf.ScrollView.VerticalScrollBarVisibilityProperty);
        init => SetValue(xf.ScrollView.VerticalScrollBarVisibilityProperty, value);
    }
}

public partial class SearchBar : View<xf.SearchBar>
{
    public Color CancelButtonColor
    {
        internal get => GetValue<Color>(xf.SearchBar.CancelButtonColorProperty);
        init => SetValue(xf.SearchBar.CancelButtonColorProperty, value);
    }
    public Double CharacterSpacing
    {
        internal get => GetValue<Double>(xf.SearchBar.CharacterSpacingProperty);
        init => SetValue(xf.SearchBar.CharacterSpacingProperty, value);
    }
    public Int32 CursorPosition
    {
        internal get => GetValue<Int32>(xf.SearchBar.CursorPositionProperty);
        init => SetValue(xf.SearchBar.CursorPositionProperty, value);
    }
    public FontAttributes FontAttributes
    {
        internal get => GetValue<FontAttributes>(xf.SearchBar.FontAttributesProperty);
        init => SetValue(xf.SearchBar.FontAttributesProperty, value);
    }
    public Boolean FontAutoScalingEnabled
    {
        internal get => GetValue<Boolean>(xf.SearchBar.FontAutoScalingEnabledProperty);
        init => SetValue(xf.SearchBar.FontAutoScalingEnabledProperty, value);
    }
    public String FontFamily
    {
        internal get => GetValue<String>(xf.SearchBar.FontFamilyProperty);
        init => SetValue(xf.SearchBar.FontFamilyProperty, value);
    }
    public Double FontSize
    {
        internal get => GetValue<Double>(xf.SearchBar.FontSizeProperty);
        init => SetValue(xf.SearchBar.FontSizeProperty, value);
    }
    public TextAlignment HorizontalTextAlignment
    {
        internal get => GetValue<TextAlignment>(xf.SearchBar.HorizontalTextAlignmentProperty);
        init => SetValue(xf.SearchBar.HorizontalTextAlignmentProperty, value);
    }
    public Boolean IsTextPredictionEnabled
    {
        internal get => GetValue<Boolean>(xf.SearchBar.IsTextPredictionEnabledProperty);
        init => SetValue(xf.SearchBar.IsTextPredictionEnabledProperty, value);
    }
    public Color PlaceholderColor
    {
        internal get => GetValue<Color>(xf.SearchBar.PlaceholderColorProperty);
        init => SetValue(xf.SearchBar.PlaceholderColorProperty, value);
    }
    public String Placeholder
    {
        internal get => GetValue<String>(xf.SearchBar.PlaceholderProperty);
        init => SetValue(xf.SearchBar.PlaceholderProperty, value);
    }
    public Int32 SelectionLength
    {
        internal get => GetValue<Int32>(xf.SearchBar.SelectionLengthProperty);
        init => SetValue(xf.SearchBar.SelectionLengthProperty, value);
    }
    public Color TextColor
    {
        internal get => GetValue<Color>(xf.SearchBar.TextColorProperty);
        init => SetValue(xf.SearchBar.TextColorProperty, value);
    }
    public String Text
    {
        internal get => GetValue<String>(xf.SearchBar.TextProperty);
        init => SetValue(xf.SearchBar.TextProperty, value);
    }
    public TextAlignment VerticalTextAlignment
    {
        internal get => GetValue<TextAlignment>(xf.SearchBar.VerticalTextAlignmentProperty);
        init => SetValue(xf.SearchBar.VerticalTextAlignmentProperty, value);
    }
    public Func<Signal> SearchButtonPressed
    {
        init => SetEvent(nameof(SearchButtonPressed), value, (ctl, handler) => ctl.SearchButtonPressed += handler, (ctl, handler) => ctl.SearchButtonPressed -= handler);
    }
}

public partial class SelectableItemsView<T>
{
    public Object SelectedItem
    {
        internal get => GetValue<Object>(xf.SelectableItemsView.SelectedItemProperty);
        init => SetValue(xf.SelectableItemsView.SelectedItemProperty, value);
    }
    public SelectionMode SelectionMode
    {
        internal get => GetValue<SelectionMode>(xf.SelectableItemsView.SelectionModeProperty);
        init => SetValue(xf.SelectableItemsView.SelectionModeProperty, value);
    }
}

public partial class Slider : View<xf.Slider>
{
    public Double Maximum
    {
        internal get => GetValue<Double>(xf.Slider.MaximumProperty);
        init => SetValue(xf.Slider.MaximumProperty, value);
    }
    public Color MaximumTrackColor
    {
        internal get => GetValue<Color>(xf.Slider.MaximumTrackColorProperty);
        init => SetValue(xf.Slider.MaximumTrackColorProperty, value);
    }
    public Double Minimum
    {
        internal get => GetValue<Double>(xf.Slider.MinimumProperty);
        init => SetValue(xf.Slider.MinimumProperty, value);
    }
    public Color MinimumTrackColor
    {
        internal get => GetValue<Color>(xf.Slider.MinimumTrackColorProperty);
        init => SetValue(xf.Slider.MinimumTrackColorProperty, value);
    }
    public Color ThumbColor
    {
        internal get => GetValue<Color>(xf.Slider.ThumbColorProperty);
        init => SetValue(xf.Slider.ThumbColorProperty, value);
    }
    public ImageSource ThumbImageSource
    {
        internal get => GetValue<ImageSource>(xf.Slider.ThumbImageSourceProperty);
        init => SetValue(xf.Slider.ThumbImageSourceProperty, value);
    }
    public Double Value
    {
        internal get => GetValue<Double>(xf.Slider.ValueProperty);
        init => SetValue(xf.Slider.ValueProperty, value);
    }
    public Func<Signal> DragCompleted
    {
        init => SetEvent(nameof(DragCompleted), value, (ctl, handler) => ctl.DragCompleted += handler, (ctl, handler) => ctl.DragCompleted -= handler);
    }
    public Func<Signal> DragStarted
    {
        init => SetEvent(nameof(DragStarted), value, (ctl, handler) => ctl.DragStarted += handler, (ctl, handler) => ctl.DragStarted -= handler);
    }
}

public partial class Span
{
    public Color BackgroundColor
    {
        internal get => GetValue<Color>(xf.Span.BackgroundColorProperty);
        init => SetValue(xf.Span.BackgroundColorProperty, value);
    }
    public Double CharacterSpacing
    {
        internal get => GetValue<Double>(xf.Span.CharacterSpacingProperty);
        init => SetValue(xf.Span.CharacterSpacingProperty, value);
    }
    public FontAttributes FontAttributes
    {
        internal get => GetValue<FontAttributes>(xf.Span.FontAttributesProperty);
        init => SetValue(xf.Span.FontAttributesProperty, value);
    }
    public Boolean FontAutoScalingEnabled
    {
        internal get => GetValue<Boolean>(xf.Span.FontAutoScalingEnabledProperty);
        init => SetValue(xf.Span.FontAutoScalingEnabledProperty, value);
    }
    public String FontFamily
    {
        internal get => GetValue<String>(xf.Span.FontFamilyProperty);
        init => SetValue(xf.Span.FontFamilyProperty, value);
    }
    public Double FontSize
    {
        internal get => GetValue<Double>(xf.Span.FontSizeProperty);
        init => SetValue(xf.Span.FontSizeProperty, value);
    }
    public Double LineHeight
    {
        internal get => GetValue<Double>(xf.Span.LineHeightProperty);
        init => SetValue(xf.Span.LineHeightProperty, value);
    }
    public Color TextColor
    {
        internal get => GetValue<Color>(xf.Span.TextColorProperty);
        init => SetValue(xf.Span.TextColorProperty, value);
    }
    public TextDecorations TextDecorations
    {
        internal get => GetValue<TextDecorations>(xf.Span.TextDecorationsProperty);
        init => SetValue(xf.Span.TextDecorationsProperty, value);
    }
    public String Text
    {
        internal get => GetValue<String>(xf.Span.TextProperty);
        init => SetValue(xf.Span.TextProperty, value);
    }
    public TextTransform TextTransform
    {
        internal get => GetValue<TextTransform>(xf.Span.TextTransformProperty);
        init => SetValue(xf.Span.TextTransformProperty, value);
    }
}

public partial class Stepper : View<xf.Stepper>
{
    public Double Increment
    {
        internal get => GetValue<Double>(xf.Stepper.IncrementProperty);
        init => SetValue(xf.Stepper.IncrementProperty, value);
    }
    public Double Maximum
    {
        internal get => GetValue<Double>(xf.Stepper.MaximumProperty);
        init => SetValue(xf.Stepper.MaximumProperty, value);
    }
    public Double Minimum
    {
        internal get => GetValue<Double>(xf.Stepper.MinimumProperty);
        init => SetValue(xf.Stepper.MinimumProperty, value);
    }
    public Double Value
    {
        internal get => GetValue<Double>(xf.Stepper.ValueProperty);
        init => SetValue(xf.Stepper.ValueProperty, value);
    }
}

public partial class StructuredItemsView : View<xf.StructuredItemsView>
{
    public Object Footer
    {
        internal get => GetValue<Object>(xf.StructuredItemsView.FooterProperty);
        init => SetValue(xf.StructuredItemsView.FooterProperty, value);
    }
    public Object Header
    {
        internal get => GetValue<Object>(xf.StructuredItemsView.HeaderProperty);
        init => SetValue(xf.StructuredItemsView.HeaderProperty, value);
    }
    public ItemSizingStrategy ItemSizingStrategy
    {
        internal get => GetValue<ItemSizingStrategy>(xf.StructuredItemsView.ItemSizingStrategyProperty);
        init => SetValue(xf.StructuredItemsView.ItemSizingStrategyProperty, value);
    }
    public xf.IItemsLayout ItemsLayout
    {
        internal get => GetValue<xf.IItemsLayout>(xf.StructuredItemsView.ItemsLayoutProperty);
        init => SetValue(xf.StructuredItemsView.ItemsLayoutProperty, value);
    }
}

public partial class SwipeItem
{
    public Color BackgroundColor
    {
        internal get => GetValue<Color>(xf.SwipeItem.BackgroundColorProperty);
        init => SetValue(xf.SwipeItem.BackgroundColorProperty, value);
    }
    public Boolean IsVisible
    {
        internal get => GetValue<Boolean>(xf.SwipeItem.IsVisibleProperty);
        init => SetValue(xf.SwipeItem.IsVisibleProperty, value);
    }
}

public partial class SwipeView : View<xf.SwipeView>
{
    public Double Threshold
    {
        internal get => GetValue<Double>(xf.SwipeView.ThresholdProperty);
        init => SetValue(xf.SwipeView.ThresholdProperty, value);
    }
}

public partial class Switch : View<xf.Switch>
{
    public Boolean IsToggled
    {
        internal get => GetValue<Boolean>(xf.Switch.IsToggledProperty);
        init => SetValue(xf.Switch.IsToggledProperty, value);
    }
    public Color OnColor
    {
        internal get => GetValue<Color>(xf.Switch.OnColorProperty);
        init => SetValue(xf.Switch.OnColorProperty, value);
    }
    public Color ThumbColor
    {
        internal get => GetValue<Color>(xf.Switch.ThumbColorProperty);
        init => SetValue(xf.Switch.ThumbColorProperty, value);
    }
}

public partial class TabbedPage
{
    public Color BarBackgroundColor
    {
        internal get => GetValue<Color>(xf.TabbedPage.BarBackgroundColorProperty);
        init => SetValue(xf.TabbedPage.BarBackgroundColorProperty, value);
    }
    public Brush BarBackground
    {
        internal get => GetValue<Brush>(xf.TabbedPage.BarBackgroundProperty);
        init => SetValue(xf.TabbedPage.BarBackgroundProperty, value);
    }
    public Color BarTextColor
    {
        internal get => GetValue<Color>(xf.TabbedPage.BarTextColorProperty);
        init => SetValue(xf.TabbedPage.BarTextColorProperty, value);
    }
    public Color SelectedTabColor
    {
        internal get => GetValue<Color>(xf.TabbedPage.SelectedTabColorProperty);
        init => SetValue(xf.TabbedPage.SelectedTabColorProperty, value);
    }
    public Color UnselectedTabColor
    {
        internal get => GetValue<Color>(xf.TabbedPage.UnselectedTabColorProperty);
        init => SetValue(xf.TabbedPage.UnselectedTabColorProperty, value);
    }
}

public partial class TimePicker : View<xf.TimePicker>
{
    public Double CharacterSpacing
    {
        internal get => GetValue<Double>(xf.TimePicker.CharacterSpacingProperty);
        init => SetValue(xf.TimePicker.CharacterSpacingProperty, value);
    }
    public FontAttributes FontAttributes
    {
        internal get => GetValue<FontAttributes>(xf.TimePicker.FontAttributesProperty);
        init => SetValue(xf.TimePicker.FontAttributesProperty, value);
    }
    public Boolean FontAutoScalingEnabled
    {
        internal get => GetValue<Boolean>(xf.TimePicker.FontAutoScalingEnabledProperty);
        init => SetValue(xf.TimePicker.FontAutoScalingEnabledProperty, value);
    }
    public String FontFamily
    {
        internal get => GetValue<String>(xf.TimePicker.FontFamilyProperty);
        init => SetValue(xf.TimePicker.FontFamilyProperty, value);
    }
    public Double FontSize
    {
        internal get => GetValue<Double>(xf.TimePicker.FontSizeProperty);
        init => SetValue(xf.TimePicker.FontSizeProperty, value);
    }
    public String Format
    {
        internal get => GetValue<String>(xf.TimePicker.FormatProperty);
        init => SetValue(xf.TimePicker.FormatProperty, value);
    }
    public Color TextColor
    {
        internal get => GetValue<Color>(xf.TimePicker.TextColorProperty);
        init => SetValue(xf.TimePicker.TextColorProperty, value);
    }
    public TextTransform TextTransform
    {
        internal get => GetValue<TextTransform>(xf.TimePicker.TextTransformProperty);
        init => SetValue(xf.TimePicker.TextTransformProperty, value);
    }
    public TimeSpan Time
    {
        internal get => GetValue<TimeSpan>(xf.TimePicker.TimeProperty);
        init => SetValue(xf.TimePicker.TimeProperty, value);
    }
}

public partial class VerticalStackLayout
{
}

public partial class VisualElement<T>
{
    public Double AnchorX
    {
        internal get => GetValue<Double>(xf.VisualElement.AnchorXProperty);
        init => SetValue(xf.VisualElement.AnchorXProperty, value);
    }
    public Double AnchorY
    {
        internal get => GetValue<Double>(xf.VisualElement.AnchorYProperty);
        init => SetValue(xf.VisualElement.AnchorYProperty, value);
    }
    public Color BackgroundColor
    {
        internal get => GetValue<Color>(xf.VisualElement.BackgroundColorProperty);
        init => SetValue(xf.VisualElement.BackgroundColorProperty, value);
    }
    public Geometry Clip
    {
        internal get => GetValue<Geometry>(xf.VisualElement.ClipProperty);
        init => SetValue(xf.VisualElement.ClipProperty, value);
    }
    public FlowDirection FlowDirection
    {
        internal get => GetValue<FlowDirection>(xf.VisualElement.FlowDirectionProperty);
        init => SetValue(xf.VisualElement.FlowDirectionProperty, value);
    }
    public Double HeightRequest
    {
        internal get => GetValue<Double>(xf.VisualElement.HeightRequestProperty);
        init => SetValue(xf.VisualElement.HeightRequestProperty, value);
    }
    public Boolean InputTransparent
    {
        internal get => GetValue<Boolean>(xf.VisualElement.InputTransparentProperty);
        init => SetValue(xf.VisualElement.InputTransparentProperty, value);
    }
    public Boolean IsEnabled
    {
        internal get => GetValue<Boolean>(xf.VisualElement.IsEnabledProperty);
        init => SetValue(xf.VisualElement.IsEnabledProperty, value);
    }
    public Boolean IsVisible
    {
        internal get => GetValue<Boolean>(xf.VisualElement.IsVisibleProperty);
        init => SetValue(xf.VisualElement.IsVisibleProperty, value);
    }
    public Double MaximumHeightRequest
    {
        internal get => GetValue<Double>(xf.VisualElement.MaximumHeightRequestProperty);
        init => SetValue(xf.VisualElement.MaximumHeightRequestProperty, value);
    }
    public Double MaximumWidthRequest
    {
        internal get => GetValue<Double>(xf.VisualElement.MaximumWidthRequestProperty);
        init => SetValue(xf.VisualElement.MaximumWidthRequestProperty, value);
    }
    public Double MinimumHeightRequest
    {
        internal get => GetValue<Double>(xf.VisualElement.MinimumHeightRequestProperty);
        init => SetValue(xf.VisualElement.MinimumHeightRequestProperty, value);
    }
    public Double MinimumWidthRequest
    {
        internal get => GetValue<Double>(xf.VisualElement.MinimumWidthRequestProperty);
        init => SetValue(xf.VisualElement.MinimumWidthRequestProperty, value);
    }
    public Double Opacity
    {
        internal get => GetValue<Double>(xf.VisualElement.OpacityProperty);
        init => SetValue(xf.VisualElement.OpacityProperty, value);
    }
    public Double Rotation
    {
        internal get => GetValue<Double>(xf.VisualElement.RotationProperty);
        init => SetValue(xf.VisualElement.RotationProperty, value);
    }
    public Double RotationX
    {
        internal get => GetValue<Double>(xf.VisualElement.RotationXProperty);
        init => SetValue(xf.VisualElement.RotationXProperty, value);
    }
    public Double RotationY
    {
        internal get => GetValue<Double>(xf.VisualElement.RotationYProperty);
        init => SetValue(xf.VisualElement.RotationYProperty, value);
    }
    public Double Scale
    {
        internal get => GetValue<Double>(xf.VisualElement.ScaleProperty);
        init => SetValue(xf.VisualElement.ScaleProperty, value);
    }
    public Double ScaleX
    {
        internal get => GetValue<Double>(xf.VisualElement.ScaleXProperty);
        init => SetValue(xf.VisualElement.ScaleXProperty, value);
    }
    public Double ScaleY
    {
        internal get => GetValue<Double>(xf.VisualElement.ScaleYProperty);
        init => SetValue(xf.VisualElement.ScaleYProperty, value);
    }
    public Shadow Shadow
    {
        internal get => GetValue<Shadow>(xf.VisualElement.ShadowProperty);
        init => SetValue(xf.VisualElement.ShadowProperty, value);
    }
    public Double TranslationX
    {
        internal get => GetValue<Double>(xf.VisualElement.TranslationXProperty);
        init => SetValue(xf.VisualElement.TranslationXProperty, value);
    }
    public Double TranslationY
    {
        internal get => GetValue<Double>(xf.VisualElement.TranslationYProperty);
        init => SetValue(xf.VisualElement.TranslationYProperty, value);
    }
    public Double WidthRequest
    {
        internal get => GetValue<Double>(xf.VisualElement.WidthRequestProperty);
        init => SetValue(xf.VisualElement.WidthRequestProperty, value);
    }
}

public partial class WebView : View<xf.WebView>
{
    public CookieContainer Cookies
    {
        internal get => GetValue<CookieContainer>(xf.WebView.CookiesProperty);
        init => SetValue(xf.WebView.CookiesProperty, value);
    }
    public xf.WebViewSource Source
    {
        internal get => GetValue<xf.WebViewSource>(xf.WebView.SourceProperty);
        init => SetValue(xf.WebView.SourceProperty, value);
    }
}
