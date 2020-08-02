#nullable enable
using System;
using System.Collections;
using xf = Xamarin.Forms;
using Laconic.Shapes;
// ReSharper disable all

namespace Laconic
{
    public partial class BoxView : View<xf.BoxView>
    {
        public xf.Color Color
        {
            get => GetValue<xf.Color>(xf.BoxView.ColorProperty);
            set => SetValue(xf.BoxView.ColorProperty, value);
        }
        public xf.CornerRadius CornerRadius
        {
            get => GetValue<xf.CornerRadius>(xf.BoxView.CornerRadiusProperty);
            set => SetValue(xf.BoxView.CornerRadiusProperty, value);
        }
    }

    public partial class Button : View<xf.Button>
    {
        public xf.Color BorderColor
        {
            get => GetValue<xf.Color>(xf.Button.BorderColorProperty);
            set => SetValue(xf.Button.BorderColorProperty, value);
        }
        public Double BorderWidth
        {
            get => GetValue<Double>(xf.Button.BorderWidthProperty);
            set => SetValue(xf.Button.BorderWidthProperty, value);
        }
        public Double CharacterSpacing
        {
            get => GetValue<Double>(xf.Button.CharacterSpacingProperty);
            set => SetValue(xf.Button.CharacterSpacingProperty, value);
        }
        public Int32 CornerRadius
        {
            get => GetValue<Int32>(xf.Button.CornerRadiusProperty);
            set => SetValue(xf.Button.CornerRadiusProperty, value);
        }
        public xf.FontAttributes FontAttributes
        {
            get => GetValue<xf.FontAttributes>(xf.Button.FontAttributesProperty);
            set => SetValue(xf.Button.FontAttributesProperty, value);
        }
        public String FontFamily
        {
            get => GetValue<String>(xf.Button.FontFamilyProperty);
            set => SetValue(xf.Button.FontFamilyProperty, value);
        }
        public xf.Font Font
        {
            get => GetValue<xf.Font>(xf.Button.FontProperty);
            set => SetValue(xf.Button.FontProperty, value);
        }
        public Double FontSize
        {
            get => GetValue<Double>(xf.Button.FontSizeProperty);
            set => SetValue(xf.Button.FontSizeProperty, value);
        }
        public xf.ImageSource Image
        {
            get => GetValue<xf.ImageSource>(xf.Button.ImageSourceProperty);
            set => SetValue(xf.Button.ImageSourceProperty, value);
        }
        public xf.Thickness Padding
        {
            get => GetValue<xf.Thickness>(xf.Button.PaddingProperty);
            set => SetValue(xf.Button.PaddingProperty, value);
        }
        public xf.Color TextColor
        {
            get => GetValue<xf.Color>(xf.Button.TextColorProperty);
            set => SetValue(xf.Button.TextColorProperty, value);
        }
        public String Text
        {
            get => GetValue<String>(xf.Button.TextProperty);
            set => SetValue(xf.Button.TextProperty, value);
        }
        public Func<Signal> Clicked
        {
            set => SetEvent(nameof(Clicked), value, (ctl, handler) => ctl.Clicked += handler, (ctl, handler) => ctl.Clicked -= handler);
        }
        public Func<Signal> Pressed
        {
            set => SetEvent(nameof(Pressed), value, (ctl, handler) => ctl.Pressed += handler, (ctl, handler) => ctl.Pressed -= handler);
        }
        public Func<Signal> Released
        {
            set => SetEvent(nameof(Released), value, (ctl, handler) => ctl.Released += handler, (ctl, handler) => ctl.Released -= handler);
        }
    }

    public partial class CarouselView
    {
        public Boolean IsBounceEnabled
        {
            get => GetValue<Boolean>(xf.CarouselView.IsBounceEnabledProperty);
            set => SetValue(xf.CarouselView.IsBounceEnabledProperty, value);
        }
        public Boolean IsScrollAnimated
        {
            get => GetValue<Boolean>(xf.CarouselView.IsScrollAnimatedProperty);
            set => SetValue(xf.CarouselView.IsScrollAnimatedProperty, value);
        }
        public Boolean IsSwipeEnabled
        {
            get => GetValue<Boolean>(xf.CarouselView.IsSwipeEnabledProperty);
            set => SetValue(xf.CarouselView.IsSwipeEnabledProperty, value);
        }
        public xf.LinearItemsLayout ItemsLayout
        {
            get => GetValue<xf.LinearItemsLayout>(xf.CarouselView.ItemsLayoutProperty);
            set => SetValue(xf.CarouselView.ItemsLayoutProperty, value);
        }
        public xf.Thickness PeekAreaInsets
        {
            get => GetValue<xf.Thickness>(xf.CarouselView.PeekAreaInsetsProperty);
            set => SetValue(xf.CarouselView.PeekAreaInsetsProperty, value);
        }
        public Int32 Position
        {
            get => GetValue<Int32>(xf.CarouselView.PositionProperty);
            set => SetValue(xf.CarouselView.PositionProperty, value);
        }
    }

    public partial class CheckBox
    {
        public xf.Color Color
        {
            get => GetValue<xf.Color>(xf.CheckBox.ColorProperty);
            set => SetValue(xf.CheckBox.ColorProperty, value);
        }
        public Boolean IsChecked
        {
            get => GetValue<Boolean>(xf.CheckBox.IsCheckedProperty);
            set => SetValue(xf.CheckBox.IsCheckedProperty, value);
        }
    }

    public partial class DatePicker : View<xf.DatePicker>
    {
        public Double CharacterSpacing
        {
            get => GetValue<Double>(xf.DatePicker.CharacterSpacingProperty);
            set => SetValue(xf.DatePicker.CharacterSpacingProperty, value);
        }
        public DateTime Date
        {
            get => GetValue<DateTime>(xf.DatePicker.DateProperty);
            set => SetValue(xf.DatePicker.DateProperty, value);
        }
        public xf.FontAttributes FontAttributes
        {
            get => GetValue<xf.FontAttributes>(xf.DatePicker.FontAttributesProperty);
            set => SetValue(xf.DatePicker.FontAttributesProperty, value);
        }
        public String FontFamily
        {
            get => GetValue<String>(xf.DatePicker.FontFamilyProperty);
            set => SetValue(xf.DatePicker.FontFamilyProperty, value);
        }
        public Double FontSize
        {
            get => GetValue<Double>(xf.DatePicker.FontSizeProperty);
            set => SetValue(xf.DatePicker.FontSizeProperty, value);
        }
        public String Format
        {
            get => GetValue<String>(xf.DatePicker.FormatProperty);
            set => SetValue(xf.DatePicker.FormatProperty, value);
        }
        public DateTime MaximumDate
        {
            get => GetValue<DateTime>(xf.DatePicker.MaximumDateProperty);
            set => SetValue(xf.DatePicker.MaximumDateProperty, value);
        }
        public DateTime MinimumDate
        {
            get => GetValue<DateTime>(xf.DatePicker.MinimumDateProperty);
            set => SetValue(xf.DatePicker.MinimumDateProperty, value);
        }
        public xf.Color TextColor
        {
            get => GetValue<xf.Color>(xf.DatePicker.TextColorProperty);
            set => SetValue(xf.DatePicker.TextColorProperty, value);
        }
    }

    public partial class Editor
    {
        public xf.EditorAutoSizeOption AutoSize
        {
            get => GetValue<xf.EditorAutoSizeOption>(xf.Editor.AutoSizeProperty);
            set => SetValue(xf.Editor.AutoSizeProperty, value);
        }
        public xf.FontAttributes FontAttributes
        {
            get => GetValue<xf.FontAttributes>(xf.Editor.FontAttributesProperty);
            set => SetValue(xf.Editor.FontAttributesProperty, value);
        }
        public String FontFamily
        {
            get => GetValue<String>(xf.Editor.FontFamilyProperty);
            set => SetValue(xf.Editor.FontFamilyProperty, value);
        }
        public Double FontSize
        {
            get => GetValue<Double>(xf.Editor.FontSizeProperty);
            set => SetValue(xf.Editor.FontSizeProperty, value);
        }
        public Boolean IsTextPredictionEnabled
        {
            get => GetValue<Boolean>(xf.Editor.IsTextPredictionEnabledProperty);
            set => SetValue(xf.Editor.IsTextPredictionEnabledProperty, value);
        }
        public Func<Signal> Completed
        {
            set => SetEvent(nameof(Completed), value, (ctl, handler) => ctl.Completed += handler, (ctl, handler) => ctl.Completed -= handler);
        }
    }

    public partial class Entry
    {
        public xf.ClearButtonVisibility ClearButtonVisibility
        {
            get => GetValue<xf.ClearButtonVisibility>(xf.Entry.ClearButtonVisibilityProperty);
            set => SetValue(xf.Entry.ClearButtonVisibilityProperty, value);
        }
        public Int32 CursorPosition
        {
            get => GetValue<Int32>(xf.Entry.CursorPositionProperty);
            set => SetValue(xf.Entry.CursorPositionProperty, value);
        }
        public xf.FontAttributes FontAttributes
        {
            get => GetValue<xf.FontAttributes>(xf.Entry.FontAttributesProperty);
            set => SetValue(xf.Entry.FontAttributesProperty, value);
        }
        public String FontFamily
        {
            get => GetValue<String>(xf.Entry.FontFamilyProperty);
            set => SetValue(xf.Entry.FontFamilyProperty, value);
        }
        public Double FontSize
        {
            get => GetValue<Double>(xf.Entry.FontSizeProperty);
            set => SetValue(xf.Entry.FontSizeProperty, value);
        }
        public xf.TextAlignment HorizontalTextAlignment
        {
            get => GetValue<xf.TextAlignment>(xf.Entry.HorizontalTextAlignmentProperty);
            set => SetValue(xf.Entry.HorizontalTextAlignmentProperty, value);
        }
        public Boolean IsPassword
        {
            get => GetValue<Boolean>(xf.Entry.IsPasswordProperty);
            set => SetValue(xf.Entry.IsPasswordProperty, value);
        }
        public Boolean IsTextPredictionEnabled
        {
            get => GetValue<Boolean>(xf.Entry.IsTextPredictionEnabledProperty);
            set => SetValue(xf.Entry.IsTextPredictionEnabledProperty, value);
        }
        public xf.ReturnType ReturnType
        {
            get => GetValue<xf.ReturnType>(xf.Entry.ReturnTypeProperty);
            set => SetValue(xf.Entry.ReturnTypeProperty, value);
        }
        public Int32 SelectionLength
        {
            get => GetValue<Int32>(xf.Entry.SelectionLengthProperty);
            set => SetValue(xf.Entry.SelectionLengthProperty, value);
        }
        public xf.TextAlignment VerticalTextAlignment
        {
            get => GetValue<xf.TextAlignment>(xf.Entry.VerticalTextAlignmentProperty);
            set => SetValue(xf.Entry.VerticalTextAlignmentProperty, value);
        }
        public Func<Signal> Completed
        {
            set => SetEvent(nameof(Completed), value, (ctl, handler) => ctl.Completed += handler, (ctl, handler) => ctl.Completed -= handler);
        }
    }

    public partial class Frame
    {
        public xf.Color BorderColor
        {
            get => GetValue<xf.Color>(xf.Frame.BorderColorProperty);
            set => SetValue(xf.Frame.BorderColorProperty, value);
        }
        public Single CornerRadius
        {
            get => GetValue<Single>(xf.Frame.CornerRadiusProperty);
            set => SetValue(xf.Frame.CornerRadiusProperty, value);
        }
        public Boolean HasShadow
        {
            get => GetValue<Boolean>(xf.Frame.HasShadowProperty);
            set => SetValue(xf.Frame.HasShadowProperty, value);
        }
    }

    public partial class Grid
    {
        public Double ColumnSpacing
        {
            get => GetValue<Double>(xf.Grid.ColumnSpacingProperty);
            set => SetValue(xf.Grid.ColumnSpacingProperty, value);
        }
        public Double RowSpacing
        {
            get => GetValue<Double>(xf.Grid.RowSpacingProperty);
            set => SetValue(xf.Grid.RowSpacingProperty, value);
        }
    }

    public partial class Image : View<xf.Image>
    {
        public xf.Aspect Aspect
        {
            get => GetValue<xf.Aspect>(xf.Image.AspectProperty);
            set => SetValue(xf.Image.AspectProperty, value);
        }
        public Boolean IsAnimationPlaying
        {
            get => GetValue<Boolean>(xf.Image.IsAnimationPlayingProperty);
            set => SetValue(xf.Image.IsAnimationPlayingProperty, value);
        }
        public Boolean IsOpaque
        {
            get => GetValue<Boolean>(xf.Image.IsOpaqueProperty);
            set => SetValue(xf.Image.IsOpaqueProperty, value);
        }
        public xf.ImageSource Source
        {
            get => GetValue<xf.ImageSource>(xf.Image.SourceProperty);
            set => SetValue(xf.Image.SourceProperty, value);
        }
    }

    public partial class ImageButton : View<xf.ImageButton>
    {
        public xf.Aspect Aspect
        {
            get => GetValue<xf.Aspect>(xf.ImageButton.AspectProperty);
            set => SetValue(xf.ImageButton.AspectProperty, value);
        }
        public xf.Color BorderColor
        {
            get => GetValue<xf.Color>(xf.ImageButton.BorderColorProperty);
            set => SetValue(xf.ImageButton.BorderColorProperty, value);
        }
        public Double BorderWidth
        {
            get => GetValue<Double>(xf.ImageButton.BorderWidthProperty);
            set => SetValue(xf.ImageButton.BorderWidthProperty, value);
        }
        public Int32 CornerRadius
        {
            get => GetValue<Int32>(xf.ImageButton.CornerRadiusProperty);
            set => SetValue(xf.ImageButton.CornerRadiusProperty, value);
        }
        public Boolean IsOpaque
        {
            get => GetValue<Boolean>(xf.ImageButton.IsOpaqueProperty);
            set => SetValue(xf.ImageButton.IsOpaqueProperty, value);
        }
        public xf.Thickness Padding
        {
            get => GetValue<xf.Thickness>(xf.ImageButton.PaddingProperty);
            set => SetValue(xf.ImageButton.PaddingProperty, value);
        }
        public xf.ImageSource Source
        {
            get => GetValue<xf.ImageSource>(xf.ImageButton.SourceProperty);
            set => SetValue(xf.ImageButton.SourceProperty, value);
        }
        public Func<Signal> Clicked
        {
            set => SetEvent(nameof(Clicked), value, (ctl, handler) => ctl.Clicked += handler, (ctl, handler) => ctl.Clicked -= handler);
        }
        public Func<Signal> Pressed
        {
            set => SetEvent(nameof(Pressed), value, (ctl, handler) => ctl.Pressed += handler, (ctl, handler) => ctl.Pressed -= handler);
        }
        public Func<Signal> Released
        {
            set => SetEvent(nameof(Released), value, (ctl, handler) => ctl.Released += handler, (ctl, handler) => ctl.Released -= handler);
        }
    }

    public partial class IndicatorView : View<xf.IndicatorView>
    {
        public Int32 Count
        {
            get => GetValue<Int32>(xf.IndicatorView.CountProperty);
            set => SetValue(xf.IndicatorView.CountProperty, value);
        }
        public Boolean HideSingle
        {
            get => GetValue<Boolean>(xf.IndicatorView.HideSingleProperty);
            set => SetValue(xf.IndicatorView.HideSingleProperty, value);
        }
        public xf.Color IndicatorColor
        {
            get => GetValue<xf.Color>(xf.IndicatorView.IndicatorColorProperty);
            set => SetValue(xf.IndicatorView.IndicatorColorProperty, value);
        }
        public Double IndicatorSize
        {
            get => GetValue<Double>(xf.IndicatorView.IndicatorSizeProperty);
            set => SetValue(xf.IndicatorView.IndicatorSizeProperty, value);
        }
        public xf.IndicatorShape IndicatorsShape
        {
            get => GetValue<xf.IndicatorShape>(xf.IndicatorView.IndicatorsShapeProperty);
            set => SetValue(xf.IndicatorView.IndicatorsShapeProperty, value);
        }
        public Int32 MaximumVisible
        {
            get => GetValue<Int32>(xf.IndicatorView.MaximumVisibleProperty);
            set => SetValue(xf.IndicatorView.MaximumVisibleProperty, value);
        }
        public Int32 Position
        {
            get => GetValue<Int32>(xf.IndicatorView.PositionProperty);
            set => SetValue(xf.IndicatorView.PositionProperty, value);
        }
        public xf.Color SelectedIndicatorColor
        {
            get => GetValue<xf.Color>(xf.IndicatorView.SelectedIndicatorColorProperty);
            set => SetValue(xf.IndicatorView.SelectedIndicatorColorProperty, value);
        }
    }

    public partial class InputView<T>
    {
        public Double CharacterSpacing
        {
            get => GetValue<Double>(xf.InputView.CharacterSpacingProperty);
            set => SetValue(xf.InputView.CharacterSpacingProperty, value);
        }
        public Boolean IsReadOnly
        {
            get => GetValue<Boolean>(xf.InputView.IsReadOnlyProperty);
            set => SetValue(xf.InputView.IsReadOnlyProperty, value);
        }
        public Boolean IsSpellCheckEnabled
        {
            get => GetValue<Boolean>(xf.InputView.IsSpellCheckEnabledProperty);
            set => SetValue(xf.InputView.IsSpellCheckEnabledProperty, value);
        }
        public xf.Keyboard Keyboard
        {
            get => GetValue<xf.Keyboard>(xf.InputView.KeyboardProperty);
            set => SetValue(xf.InputView.KeyboardProperty, value);
        }
        public Int32 MaxLength
        {
            get => GetValue<Int32>(xf.InputView.MaxLengthProperty);
            set => SetValue(xf.InputView.MaxLengthProperty, value);
        }
        public xf.Color PlaceholderColor
        {
            get => GetValue<xf.Color>(xf.InputView.PlaceholderColorProperty);
            set => SetValue(xf.InputView.PlaceholderColorProperty, value);
        }
        public String Placeholder
        {
            get => GetValue<String>(xf.InputView.PlaceholderProperty);
            set => SetValue(xf.InputView.PlaceholderProperty, value);
        }
        public xf.Color TextColor
        {
            get => GetValue<xf.Color>(xf.InputView.TextColorProperty);
            set => SetValue(xf.InputView.TextColorProperty, value);
        }
        public String Text
        {
            get => GetValue<String>(xf.InputView.TextProperty);
            set => SetValue(xf.InputView.TextProperty, value);
        }
    }

    public abstract partial class ItemsView<T>
    {
        public Object EmptyView
        {
            get => GetValue<Object>(xf.ItemsView.EmptyViewProperty);
            set => SetValue(xf.ItemsView.EmptyViewProperty, value);
        }
        public xf.DataTemplate EmptyViewTemplate
        {
            get => GetValue<xf.DataTemplate>(xf.ItemsView.EmptyViewTemplateProperty);
            set => SetValue(xf.ItemsView.EmptyViewTemplateProperty, value);
        }
        public xf.ScrollBarVisibility HorizontalScrollBarVisibility
        {
            get => GetValue<xf.ScrollBarVisibility>(xf.ItemsView.HorizontalScrollBarVisibilityProperty);
            set => SetValue(xf.ItemsView.HorizontalScrollBarVisibilityProperty, value);
        }
        public xf.ItemsUpdatingScrollMode ItemsUpdatingScrollMode
        {
            get => GetValue<xf.ItemsUpdatingScrollMode>(xf.ItemsView.ItemsUpdatingScrollModeProperty);
            set => SetValue(xf.ItemsView.ItemsUpdatingScrollModeProperty, value);
        }
        public Int32 RemainingItemsThreshold
        {
            get => GetValue<Int32>(xf.ItemsView.RemainingItemsThresholdProperty);
            set => SetValue(xf.ItemsView.RemainingItemsThresholdProperty, value);
        }
        public xf.ScrollBarVisibility VerticalScrollBarVisibility
        {
            get => GetValue<xf.ScrollBarVisibility>(xf.ItemsView.VerticalScrollBarVisibilityProperty);
            set => SetValue(xf.ItemsView.VerticalScrollBarVisibilityProperty, value);
        }
    }

    public partial class Label : View<xf.Label>
    {
        public Double CharacterSpacing
        {
            get => GetValue<Double>(xf.Label.CharacterSpacingProperty);
            set => SetValue(xf.Label.CharacterSpacingProperty, value);
        }
        public xf.FontAttributes FontAttributes
        {
            get => GetValue<xf.FontAttributes>(xf.Label.FontAttributesProperty);
            set => SetValue(xf.Label.FontAttributesProperty, value);
        }
        public String FontFamily
        {
            get => GetValue<String>(xf.Label.FontFamilyProperty);
            set => SetValue(xf.Label.FontFamilyProperty, value);
        }
        public xf.Font Font
        {
            get => GetValue<xf.Font>(xf.Label.FontProperty);
            set => SetValue(xf.Label.FontProperty, value);
        }
        public Double FontSize
        {
            get => GetValue<Double>(xf.Label.FontSizeProperty);
            set => SetValue(xf.Label.FontSizeProperty, value);
        }
        public xf.FormattedString FormattedText
        {
            get => GetValue<xf.FormattedString>(xf.Label.FormattedTextProperty);
            set => SetValue(xf.Label.FormattedTextProperty, value);
        }
        public xf.TextAlignment HorizontalTextAlignment
        {
            get => GetValue<xf.TextAlignment>(xf.Label.HorizontalTextAlignmentProperty);
            set => SetValue(xf.Label.HorizontalTextAlignmentProperty, value);
        }
        public xf.LineBreakMode LineBreakMode
        {
            get => GetValue<xf.LineBreakMode>(xf.Label.LineBreakModeProperty);
            set => SetValue(xf.Label.LineBreakModeProperty, value);
        }
        public Double LineHeight
        {
            get => GetValue<Double>(xf.Label.LineHeightProperty);
            set => SetValue(xf.Label.LineHeightProperty, value);
        }
        public Int32 MaxLines
        {
            get => GetValue<Int32>(xf.Label.MaxLinesProperty);
            set => SetValue(xf.Label.MaxLinesProperty, value);
        }
        public xf.Thickness Padding
        {
            get => GetValue<xf.Thickness>(xf.Label.PaddingProperty);
            set => SetValue(xf.Label.PaddingProperty, value);
        }
        public xf.Color TextColor
        {
            get => GetValue<xf.Color>(xf.Label.TextColorProperty);
            set => SetValue(xf.Label.TextColorProperty, value);
        }
        public xf.TextDecorations TextDecorations
        {
            get => GetValue<xf.TextDecorations>(xf.Label.TextDecorationsProperty);
            set => SetValue(xf.Label.TextDecorationsProperty, value);
        }
        public String Text
        {
            get => GetValue<String>(xf.Label.TextProperty);
            set => SetValue(xf.Label.TextProperty, value);
        }
        public xf.TextType TextType
        {
            get => GetValue<xf.TextType>(xf.Label.TextTypeProperty);
            set => SetValue(xf.Label.TextTypeProperty, value);
        }
        public xf.TextAlignment VerticalTextAlignment
        {
            get => GetValue<xf.TextAlignment>(xf.Label.VerticalTextAlignmentProperty);
            set => SetValue(xf.Label.VerticalTextAlignmentProperty, value);
        }
    }

    public partial class Page<T>
    {
        public xf.ImageSource BackgroundImageSource
        {
            get => GetValue<xf.ImageSource>(xf.Page.BackgroundImageSourceProperty);
            set => SetValue(xf.Page.BackgroundImageSourceProperty, value);
        }
        public xf.ImageSource IconImageSource
        {
            get => GetValue<xf.ImageSource>(xf.Page.IconImageSourceProperty);
            set => SetValue(xf.Page.IconImageSourceProperty, value);
        }
        public Boolean IsBusy
        {
            get => GetValue<Boolean>(xf.Page.IsBusyProperty);
            set => SetValue(xf.Page.IsBusyProperty, value);
        }
        public xf.Thickness Padding
        {
            get => GetValue<xf.Thickness>(xf.Page.PaddingProperty);
            set => SetValue(xf.Page.PaddingProperty, value);
        }
        public String Title
        {
            get => GetValue<String>(xf.Page.TitleProperty);
            set => SetValue(xf.Page.TitleProperty, value);
        }
        public Func<Signal> Appearing
        {
            set => SetEvent(nameof(Appearing), value, (ctl, handler) => ctl.Appearing += handler, (ctl, handler) => ctl.Appearing -= handler);
        }
        public Func<Signal> Disappearing
        {
            set => SetEvent(nameof(Disappearing), value, (ctl, handler) => ctl.Disappearing += handler, (ctl, handler) => ctl.Disappearing -= handler);
        }
        public Func<Signal> LayoutChanged
        {
            set => SetEvent(nameof(LayoutChanged), value, (ctl, handler) => ctl.LayoutChanged += handler, (ctl, handler) => ctl.LayoutChanged -= handler);
        }
    }

    public partial class Picker : View<xf.Picker>
    {
        public Double CharacterSpacing
        {
            get => GetValue<Double>(xf.Picker.CharacterSpacingProperty);
            set => SetValue(xf.Picker.CharacterSpacingProperty, value);
        }
        public xf.FontAttributes FontAttributes
        {
            get => GetValue<xf.FontAttributes>(xf.Picker.FontAttributesProperty);
            set => SetValue(xf.Picker.FontAttributesProperty, value);
        }
        public String FontFamily
        {
            get => GetValue<String>(xf.Picker.FontFamilyProperty);
            set => SetValue(xf.Picker.FontFamilyProperty, value);
        }
        public Double FontSize
        {
            get => GetValue<Double>(xf.Picker.FontSizeProperty);
            set => SetValue(xf.Picker.FontSizeProperty, value);
        }
        public Int32 SelectedIndex
        {
            get => GetValue<Int32>(xf.Picker.SelectedIndexProperty);
            set => SetValue(xf.Picker.SelectedIndexProperty, value);
        }
        public Object SelectedItem
        {
            get => GetValue<Object>(xf.Picker.SelectedItemProperty);
            set => SetValue(xf.Picker.SelectedItemProperty, value);
        }
        public xf.Color TextColor
        {
            get => GetValue<xf.Color>(xf.Picker.TextColorProperty);
            set => SetValue(xf.Picker.TextColorProperty, value);
        }
        public xf.Color TitleColor
        {
            get => GetValue<xf.Color>(xf.Picker.TitleColorProperty);
            set => SetValue(xf.Picker.TitleColorProperty, value);
        }
        public String Title
        {
            get => GetValue<String>(xf.Picker.TitleProperty);
            set => SetValue(xf.Picker.TitleProperty, value);
        }
    }

    public partial class ProgressBar : View<xf.ProgressBar>
    {
        public xf.Color ProgressColor
        {
            get => GetValue<xf.Color>(xf.ProgressBar.ProgressColorProperty);
            set => SetValue(xf.ProgressBar.ProgressColorProperty, value);
        }
        public Double Progress
        {
            get => GetValue<Double>(xf.ProgressBar.ProgressProperty);
            set => SetValue(xf.ProgressBar.ProgressProperty, value);
        }
    }

    public partial class RefreshView
    {
        public Boolean IsRefreshing
        {
            get => GetValue<Boolean>(xf.RefreshView.IsRefreshingProperty);
            set => SetValue(xf.RefreshView.IsRefreshingProperty, value);
        }
        public xf.Color RefreshColor
        {
            get => GetValue<xf.Color>(xf.RefreshView.RefreshColorProperty);
            set => SetValue(xf.RefreshView.RefreshColorProperty, value);
        }
        public Func<Signal> Refreshing
        {
            set => SetEvent(nameof(Refreshing), value, (ctl, handler) => ctl.Refreshing += handler, (ctl, handler) => ctl.Refreshing -= handler);
        }
    }

    public partial class ScrollView
    {
        public xf.ScrollBarVisibility HorizontalScrollBarVisibility
        {
            get => GetValue<xf.ScrollBarVisibility>(xf.ScrollView.HorizontalScrollBarVisibilityProperty);
            set => SetValue(xf.ScrollView.HorizontalScrollBarVisibilityProperty, value);
        }
        public xf.ScrollOrientation Orientation
        {
            get => GetValue<xf.ScrollOrientation>(xf.ScrollView.OrientationProperty);
            set => SetValue(xf.ScrollView.OrientationProperty, value);
        }
        public xf.ScrollBarVisibility VerticalScrollBarVisibility
        {
            get => GetValue<xf.ScrollBarVisibility>(xf.ScrollView.VerticalScrollBarVisibilityProperty);
            set => SetValue(xf.ScrollView.VerticalScrollBarVisibilityProperty, value);
        }
    }

    public partial class SearchBar : View<xf.SearchBar>
    {
        public xf.Color CancelButtonColor
        {
            get => GetValue<xf.Color>(xf.SearchBar.CancelButtonColorProperty);
            set => SetValue(xf.SearchBar.CancelButtonColorProperty, value);
        }
        public Double CharacterSpacing
        {
            get => GetValue<Double>(xf.SearchBar.CharacterSpacingProperty);
            set => SetValue(xf.SearchBar.CharacterSpacingProperty, value);
        }
        public xf.FontAttributes FontAttributes
        {
            get => GetValue<xf.FontAttributes>(xf.SearchBar.FontAttributesProperty);
            set => SetValue(xf.SearchBar.FontAttributesProperty, value);
        }
        public String FontFamily
        {
            get => GetValue<String>(xf.SearchBar.FontFamilyProperty);
            set => SetValue(xf.SearchBar.FontFamilyProperty, value);
        }
        public Double FontSize
        {
            get => GetValue<Double>(xf.SearchBar.FontSizeProperty);
            set => SetValue(xf.SearchBar.FontSizeProperty, value);
        }
        public xf.TextAlignment HorizontalTextAlignment
        {
            get => GetValue<xf.TextAlignment>(xf.SearchBar.HorizontalTextAlignmentProperty);
            set => SetValue(xf.SearchBar.HorizontalTextAlignmentProperty, value);
        }
        public xf.Color PlaceholderColor
        {
            get => GetValue<xf.Color>(xf.SearchBar.PlaceholderColorProperty);
            set => SetValue(xf.SearchBar.PlaceholderColorProperty, value);
        }
        public String Placeholder
        {
            get => GetValue<String>(xf.SearchBar.PlaceholderProperty);
            set => SetValue(xf.SearchBar.PlaceholderProperty, value);
        }
        public Object SearchCommandParameter
        {
            get => GetValue<Object>(xf.SearchBar.SearchCommandParameterProperty);
            set => SetValue(xf.SearchBar.SearchCommandParameterProperty, value);
        }
        public xf.Color TextColor
        {
            get => GetValue<xf.Color>(xf.SearchBar.TextColorProperty);
            set => SetValue(xf.SearchBar.TextColorProperty, value);
        }
        public String Text
        {
            get => GetValue<String>(xf.SearchBar.TextProperty);
            set => SetValue(xf.SearchBar.TextProperty, value);
        }
        public xf.TextAlignment VerticalTextAlignment
        {
            get => GetValue<xf.TextAlignment>(xf.SearchBar.VerticalTextAlignmentProperty);
            set => SetValue(xf.SearchBar.VerticalTextAlignmentProperty, value);
        }
        public Func<Signal> SearchButtonPressed
        {
            set => SetEvent(nameof(SearchButtonPressed), value, (ctl, handler) => ctl.SearchButtonPressed += handler, (ctl, handler) => ctl.SearchButtonPressed -= handler);
        }
    }

    public partial class SelectableItemsView<T>
    {
        public Object SelectedItem
        {
            get => GetValue<Object>(xf.SelectableItemsView.SelectedItemProperty);
            set => SetValue(xf.SelectableItemsView.SelectedItemProperty, value);
        }
        public xf.SelectionMode SelectionMode
        {
            get => GetValue<xf.SelectionMode>(xf.SelectableItemsView.SelectionModeProperty);
            set => SetValue(xf.SelectableItemsView.SelectionModeProperty, value);
        }
    }

    public partial class Slider : View<xf.Slider>
    {
        public Double Maximum
        {
            get => GetValue<Double>(xf.Slider.MaximumProperty);
            set => SetValue(xf.Slider.MaximumProperty, value);
        }
        public xf.Color MaximumTrackColor
        {
            get => GetValue<xf.Color>(xf.Slider.MaximumTrackColorProperty);
            set => SetValue(xf.Slider.MaximumTrackColorProperty, value);
        }
        public Double Minimum
        {
            get => GetValue<Double>(xf.Slider.MinimumProperty);
            set => SetValue(xf.Slider.MinimumProperty, value);
        }
        public xf.Color MinimumTrackColor
        {
            get => GetValue<xf.Color>(xf.Slider.MinimumTrackColorProperty);
            set => SetValue(xf.Slider.MinimumTrackColorProperty, value);
        }
        public xf.Color ThumbColor
        {
            get => GetValue<xf.Color>(xf.Slider.ThumbColorProperty);
            set => SetValue(xf.Slider.ThumbColorProperty, value);
        }
        public xf.ImageSource ThumbImageSource
        {
            get => GetValue<xf.ImageSource>(xf.Slider.ThumbImageSourceProperty);
            set => SetValue(xf.Slider.ThumbImageSourceProperty, value);
        }
        public Double Value
        {
            get => GetValue<Double>(xf.Slider.ValueProperty);
            set => SetValue(xf.Slider.ValueProperty, value);
        }
        public Func<Signal> DragCompleted
        {
            set => SetEvent(nameof(DragCompleted), value, (ctl, handler) => ctl.DragCompleted += handler, (ctl, handler) => ctl.DragCompleted -= handler);
        }
        public Func<Signal> DragStarted
        {
            set => SetEvent(nameof(DragStarted), value, (ctl, handler) => ctl.DragStarted += handler, (ctl, handler) => ctl.DragStarted -= handler);
        }
    }

    public partial class StackLayout
    {
        public xf.StackOrientation Orientation
        {
            get => GetValue<xf.StackOrientation>(xf.StackLayout.OrientationProperty);
            set => SetValue(xf.StackLayout.OrientationProperty, value);
        }
        public Double Spacing
        {
            get => GetValue<Double>(xf.StackLayout.SpacingProperty);
            set => SetValue(xf.StackLayout.SpacingProperty, value);
        }
    }

    public partial class StructuredItemsView : View<xf.StructuredItemsView>
    {
        public Object Footer
        {
            get => GetValue<Object>(xf.StructuredItemsView.FooterProperty);
            set => SetValue(xf.StructuredItemsView.FooterProperty, value);
        }
        public xf.DataTemplate FooterTemplate
        {
            get => GetValue<xf.DataTemplate>(xf.StructuredItemsView.FooterTemplateProperty);
            set => SetValue(xf.StructuredItemsView.FooterTemplateProperty, value);
        }
        public Object Header
        {
            get => GetValue<Object>(xf.StructuredItemsView.HeaderProperty);
            set => SetValue(xf.StructuredItemsView.HeaderProperty, value);
        }
        public xf.DataTemplate HeaderTemplate
        {
            get => GetValue<xf.DataTemplate>(xf.StructuredItemsView.HeaderTemplateProperty);
            set => SetValue(xf.StructuredItemsView.HeaderTemplateProperty, value);
        }
        public xf.ItemSizingStrategy ItemSizingStrategy
        {
            get => GetValue<xf.ItemSizingStrategy>(xf.StructuredItemsView.ItemSizingStrategyProperty);
            set => SetValue(xf.StructuredItemsView.ItemSizingStrategyProperty, value);
        }
        public xf.IItemsLayout ItemsLayout
        {
            get => GetValue<xf.IItemsLayout>(xf.StructuredItemsView.ItemsLayoutProperty);
            set => SetValue(xf.StructuredItemsView.ItemsLayoutProperty, value);
        }
    }

    public partial class Switch : View<xf.Switch>
    {
        public Boolean IsToggled
        {
            get => GetValue<Boolean>(xf.Switch.IsToggledProperty);
            set => SetValue(xf.Switch.IsToggledProperty, value);
        }
        public xf.Color OnColor
        {
            get => GetValue<xf.Color>(xf.Switch.OnColorProperty);
            set => SetValue(xf.Switch.OnColorProperty, value);
        }
        public xf.Color ThumbColor
        {
            get => GetValue<xf.Color>(xf.Switch.ThumbColorProperty);
            set => SetValue(xf.Switch.ThumbColorProperty, value);
        }
    }

    public partial class TimePicker : View<xf.TimePicker>
    {
        public Double CharacterSpacing
        {
            get => GetValue<Double>(xf.TimePicker.CharacterSpacingProperty);
            set => SetValue(xf.TimePicker.CharacterSpacingProperty, value);
        }
        public xf.FontAttributes FontAttributes
        {
            get => GetValue<xf.FontAttributes>(xf.TimePicker.FontAttributesProperty);
            set => SetValue(xf.TimePicker.FontAttributesProperty, value);
        }
        public String FontFamily
        {
            get => GetValue<String>(xf.TimePicker.FontFamilyProperty);
            set => SetValue(xf.TimePicker.FontFamilyProperty, value);
        }
        public Double FontSize
        {
            get => GetValue<Double>(xf.TimePicker.FontSizeProperty);
            set => SetValue(xf.TimePicker.FontSizeProperty, value);
        }
        public String Format
        {
            get => GetValue<String>(xf.TimePicker.FormatProperty);
            set => SetValue(xf.TimePicker.FormatProperty, value);
        }
        public xf.Color TextColor
        {
            get => GetValue<xf.Color>(xf.TimePicker.TextColorProperty);
            set => SetValue(xf.TimePicker.TextColorProperty, value);
        }
        public TimeSpan Time
        {
            get => GetValue<TimeSpan>(xf.TimePicker.TimeProperty);
            set => SetValue(xf.TimePicker.TimeProperty, value);
        }
    }

    public partial class VisualElement<T>
    {
        public Double AnchorX
        {
            get => GetValue<Double>(xf.VisualElement.AnchorXProperty);
            set => SetValue(xf.VisualElement.AnchorXProperty, value);
        }
        public Double AnchorY
        {
            get => GetValue<Double>(xf.VisualElement.AnchorYProperty);
            set => SetValue(xf.VisualElement.AnchorYProperty, value);
        }
        public xf.Color BackgroundColor
        {
            get => GetValue<xf.Color>(xf.VisualElement.BackgroundColorProperty);
            set => SetValue(xf.VisualElement.BackgroundColorProperty, value);
        }
        public Geometry Clip
        {
            get => GetValue<Geometry>(xf.VisualElement.ClipProperty);
            set => SetValue(xf.VisualElement.ClipProperty, value);
        }
        public xf.FlowDirection FlowDirection
        {
            get => GetValue<xf.FlowDirection>(xf.VisualElement.FlowDirectionProperty);
            set => SetValue(xf.VisualElement.FlowDirectionProperty, value);
        }
        public Double HeightRequest
        {
            get => GetValue<Double>(xf.VisualElement.HeightRequestProperty);
            set => SetValue(xf.VisualElement.HeightRequestProperty, value);
        }
        public Boolean InputTransparent
        {
            get => GetValue<Boolean>(xf.VisualElement.InputTransparentProperty);
            set => SetValue(xf.VisualElement.InputTransparentProperty, value);
        }
        public Boolean IsEnabled
        {
            get => GetValue<Boolean>(xf.VisualElement.IsEnabledProperty);
            set => SetValue(xf.VisualElement.IsEnabledProperty, value);
        }
        public Boolean IsTabStop
        {
            get => GetValue<Boolean>(xf.VisualElement.IsTabStopProperty);
            set => SetValue(xf.VisualElement.IsTabStopProperty, value);
        }
        public Boolean IsVisible
        {
            get => GetValue<Boolean>(xf.VisualElement.IsVisibleProperty);
            set => SetValue(xf.VisualElement.IsVisibleProperty, value);
        }
        public Double MinimumHeightRequest
        {
            get => GetValue<Double>(xf.VisualElement.MinimumHeightRequestProperty);
            set => SetValue(xf.VisualElement.MinimumHeightRequestProperty, value);
        }
        public Double MinimumWidthRequest
        {
            get => GetValue<Double>(xf.VisualElement.MinimumWidthRequestProperty);
            set => SetValue(xf.VisualElement.MinimumWidthRequestProperty, value);
        }
        public Double Opacity
        {
            get => GetValue<Double>(xf.VisualElement.OpacityProperty);
            set => SetValue(xf.VisualElement.OpacityProperty, value);
        }
        public Double Rotation
        {
            get => GetValue<Double>(xf.VisualElement.RotationProperty);
            set => SetValue(xf.VisualElement.RotationProperty, value);
        }
        public Double RotationX
        {
            get => GetValue<Double>(xf.VisualElement.RotationXProperty);
            set => SetValue(xf.VisualElement.RotationXProperty, value);
        }
        public Double RotationY
        {
            get => GetValue<Double>(xf.VisualElement.RotationYProperty);
            set => SetValue(xf.VisualElement.RotationYProperty, value);
        }
        public Double Scale
        {
            get => GetValue<Double>(xf.VisualElement.ScaleProperty);
            set => SetValue(xf.VisualElement.ScaleProperty, value);
        }
        public Double ScaleX
        {
            get => GetValue<Double>(xf.VisualElement.ScaleXProperty);
            set => SetValue(xf.VisualElement.ScaleXProperty, value);
        }
        public Double ScaleY
        {
            get => GetValue<Double>(xf.VisualElement.ScaleYProperty);
            set => SetValue(xf.VisualElement.ScaleYProperty, value);
        }
        public xf.Style Style
        {
            get => GetValue<xf.Style>(xf.VisualElement.StyleProperty);
            set => SetValue(xf.VisualElement.StyleProperty, value);
        }
        public Int32 TabIndex
        {
            get => GetValue<Int32>(xf.VisualElement.TabIndexProperty);
            set => SetValue(xf.VisualElement.TabIndexProperty, value);
        }
        public Double TranslationX
        {
            get => GetValue<Double>(xf.VisualElement.TranslationXProperty);
            set => SetValue(xf.VisualElement.TranslationXProperty, value);
        }
        public Double TranslationY
        {
            get => GetValue<Double>(xf.VisualElement.TranslationYProperty);
            set => SetValue(xf.VisualElement.TranslationYProperty, value);
        }
        public xf.IVisual Visual
        {
            get => GetValue<xf.IVisual>(xf.VisualElement.VisualProperty);
            set => SetValue(xf.VisualElement.VisualProperty, value);
        }
        public Double WidthRequest
        {
            get => GetValue<Double>(xf.VisualElement.WidthRequestProperty);
            set => SetValue(xf.VisualElement.WidthRequestProperty, value);
        }
    }
}