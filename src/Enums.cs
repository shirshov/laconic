namespace Laconic;

[Flags]
public enum AbsoluteLayoutFlags
{
    None = 0,
    XProportional = 1 << 0,
    YProportional = 1 << 1,
    WidthProportional = 1 << 2,
    HeightProportional = 1 << 3,
    PositionProportional = 1 | 1 << 1,
    SizeProportional = 1 << 2 | 1 << 3,
    All = ~0
}

public enum Easing
{
    Linear,
    SinOut,
    SinIn,
    SinInOut,
    CubicIn,
    CubicOut,
    CubicInOut,
    BounceOut,
    BounceIn,
    SpringIn,
    SpringOut
}
    
public enum ExpanderState
{
    Expanding,
    Expanded,
    Collapsing,
    Collapsed
}
    
[Flags]
public enum FontAttributes
{
    None = 0x0,
    Bold = 0x1,
    Italic = 0x2
}
    
public enum FlyoutLayoutBehavior

{
    Default = 0,
    SplitOnLandscape = 1,
    Split = 2,
    Popover = 3,
    SplitOnPortrait = 4
}

public enum ReturnType
{
    Default,
    Done,
    Go,
    Next,
    Search,
    Send
}

public enum IndicatorShape
{
    Circle,
    Square
}

public enum ScrollBarVisibility
{
    Default,
    Always,
    Never
}

public enum ItemsUpdatingScrollMode
{
    KeepItemsInView,
    KeepScrollOffset,
    KeepLastItemInView
}

public enum LayoutOptions
{
    Start,
    Center,
    End,
    Fill
}
    
public enum LineBreakMode
{
    NoWrap,
    WordWrap,
    CharacterWrap,
    HeadTruncation,
    TailTruncation,
    MiddleTruncation
}

[Flags]
public enum TextDecorations
{
    None = 0x0,
    Underline = 0x1,
    Strikethrough = 0x2
}

public enum TextType
{
    Text,
    Html
}

public enum ScrollOrientation
{
    Vertical,
    Horizontal,
    Both,
    Neither
}

public enum SelectionMode
{
    None,
    Single,
    Multiple
}

public enum StackOrientation
{
    Vertical,
    Horizontal
}

public enum ItemSizingStrategy
{
    MeasureAllItems,
    MeasureFirstItem
}

public enum FlowDirection
{
    MatchParent,
    LeftToRight,
    RightToLeft
}

public enum TextAlignment
{
    Start = Maui.TextAlignment.Start,
    End = Maui.TextAlignment.End,
    Center = Maui.TextAlignment.Center
}
    
public enum TextTransform
{
    None = 0 ,
    Default,
    Lowercase,
    Uppercase
}

public enum Aspect
{
    AspectFit,
    AspectFill,
    Fill
}

public enum ClearButtonVisibility
{
    Never,
    WhileEditing
}

public enum EditorAutoSizeOption
{
    Disabled,
    TextChanges
}

[Flags]
public enum LayoutAlignment
{
    Start = 0x0,
    Center = 0x1,
    End = 0x2,
    Fill = 0x3
}

public enum Stretch
{
    None,
    Fill,
    Uniform,
    UniformToFill
}

public enum VisualMarker
{
    MatchParent,
    Default
}

public enum Keyboard
{
    Plain,
    Chat,
    Default,
    Email,
    Numeric,
    Telephone,
    Text,
    Url
}

public enum ItemsLayoutOrientation
{
    Vertical,
    Horizontal,
}


  public enum SnapPointsAlignment
  {
    Start,
    Center,
    End,
  }

  public enum SnapPointsType
  {
    None,
    Mandatory,
    MandatorySingle,
  }
