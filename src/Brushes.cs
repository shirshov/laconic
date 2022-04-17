namespace Laconic;

public interface IBrush
{
        
}
    
public abstract class Brush
{
    public static IBrush Default => new SolidColorBrush(null);

    public static readonly SolidColorBrush AliceBlue = new(Color.AliceBlue);
    public static readonly SolidColorBrush AntiqueWhite = new(Color.AntiqueWhite);
    public static readonly SolidColorBrush Aqua = new(Color.Aqua);
    public static readonly SolidColorBrush Aquamarine = new(Color.Aquamarine);
    public static readonly SolidColorBrush Azure = new(Color.Azure);
    public static readonly SolidColorBrush Beige = new(Color.Beige);
    public static readonly SolidColorBrush Bisque = new(Color.Bisque);
    public static readonly SolidColorBrush Black = new(Color.Black);
    public static readonly SolidColorBrush BlanchedAlmond = new(Color.BlanchedAlmond);
    public static readonly SolidColorBrush Blue = new(Color.Blue);
    public static readonly SolidColorBrush BlueViolet = new(Color.BlueViolet);
    public static readonly SolidColorBrush Brown = new(Color.Brown);
    public static readonly SolidColorBrush BurlyWood = new(Color.BurlyWood);
    public static readonly SolidColorBrush CadetBlue = new(Color.CadetBlue);
    public static readonly SolidColorBrush Chartreuse = new(Color.Chartreuse);
    public static readonly SolidColorBrush Chocolate = new(Color.Chocolate);
    public static readonly SolidColorBrush Coral = new(Color.Coral);
    public static readonly SolidColorBrush CornflowerBlue = new(Color.CornflowerBlue);
    public static readonly SolidColorBrush Cornsilk = new(Color.Cornsilk);
    public static readonly SolidColorBrush Crimson = new(Color.Crimson);
    public static readonly SolidColorBrush Cyan = new(Color.Cyan);
    public static readonly SolidColorBrush DarkBlue = new(Color.DarkBlue);
    public static readonly SolidColorBrush DarkCyan = new(Color.DarkCyan);
    public static readonly SolidColorBrush DarkGoldenrod = new(Color.DarkGoldenrod);
    public static readonly SolidColorBrush DarkGray = new(Color.DarkGray);
    public static readonly SolidColorBrush DarkGreen = new(Color.DarkGreen);
    public static readonly SolidColorBrush DarkKhaki = new(Color.DarkKhaki);
    public static readonly SolidColorBrush DarkMagenta = new(Color.DarkMagenta);
    public static readonly SolidColorBrush DarkOliveGreen = new(Color.DarkOliveGreen);
    public static readonly SolidColorBrush DarkOrange = new(Color.DarkOrange);
    public static readonly SolidColorBrush DarkOrchid = new(Color.DarkOrchid);
    public static readonly SolidColorBrush DarkRed = new(Color.DarkRed);
    public static readonly SolidColorBrush DarkSalmon = new(Color.DarkSalmon);
    public static readonly SolidColorBrush DarkSeaGreen = new(Color.DarkSeaGreen);
    public static readonly SolidColorBrush DarkSlateBlue = new(Color.DarkSlateBlue);
    public static readonly SolidColorBrush DarkSlateGray = new(Color.DarkSlateGray);
    public static readonly SolidColorBrush DarkTurquoise = new(Color.DarkTurquoise);
    public static readonly SolidColorBrush DarkViolet = new(Color.DarkViolet);
    public static readonly SolidColorBrush DeepPink = new(Color.DeepPink);
    public static readonly SolidColorBrush DeepSkyBlue = new(Color.DeepSkyBlue);
    public static readonly SolidColorBrush DimGray = new(Color.DimGray);
    public static readonly SolidColorBrush DodgerBlue = new(Color.DodgerBlue);
    public static readonly SolidColorBrush Firebrick = new(Color.Firebrick);
    public static readonly SolidColorBrush FloralWhite = new(Color.FloralWhite);
    public static readonly SolidColorBrush ForestGreen = new(Color.ForestGreen);
    public static readonly SolidColorBrush Fuchsia = new(Color.Fuchsia);
    public static readonly SolidColorBrush Gainsboro = new(Color.Gainsboro);
    public static readonly SolidColorBrush GhostWhite = new(Color.GhostWhite);
    public static readonly SolidColorBrush Gold = new(Color.Gold);
    public static readonly SolidColorBrush Goldenrod = new(Color.Goldenrod);
    public static readonly SolidColorBrush Gray = new(Color.Gray);
    public static readonly SolidColorBrush Green = new(Color.Green);
    public static readonly SolidColorBrush GreenYellow = new(Color.GreenYellow);
    public static readonly SolidColorBrush Honeydew = new(Color.Honeydew);
    public static readonly SolidColorBrush HotPink = new(Color.HotPink);
    public static readonly SolidColorBrush IndianRed = new(Color.IndianRed);
    public static readonly SolidColorBrush Indigo = new(Color.Indigo);
    public static readonly SolidColorBrush Ivory = new(Color.Ivory);
    public static readonly SolidColorBrush Khaki = new(Color.Ivory);
    public static readonly SolidColorBrush Lavender = new(Color.Lavender);
    public static readonly SolidColorBrush LavenderBlush = new(Color.LavenderBlush);
    public static readonly SolidColorBrush LawnGreen = new(Color.LawnGreen);
    public static readonly SolidColorBrush LemonChiffon = new(Color.LemonChiffon);
    public static readonly SolidColorBrush LightBlue = new(Color.LightBlue);
    public static readonly SolidColorBrush LightCoral = new(Color.LightCoral);
    public static readonly SolidColorBrush LightCyan = new(Color.LightCyan);
    public static readonly SolidColorBrush LightGoldenrodYellow = new(Color.LightGoldenrodYellow);
    public static readonly SolidColorBrush LightGray = new(Color.LightGray);
    public static readonly SolidColorBrush LightGreen = new(Color.LightGreen);
    public static readonly SolidColorBrush LightPink = new(Color.LightPink);
    public static readonly SolidColorBrush LightSalmon = new(Color.LightSalmon);
    public static readonly SolidColorBrush LightSeaGreen = new(Color.LightSeaGreen);
    public static readonly SolidColorBrush LightSkyBlue = new(Color.LightSkyBlue);
    public static readonly SolidColorBrush LightSlateGray = new(Color.LightSlateGray);
    public static readonly SolidColorBrush LightSteelBlue = new(Color.LightSteelBlue);
    public static readonly SolidColorBrush LightYellow = new(Color.LightYellow);
    public static readonly SolidColorBrush Lime = new(Color.Lime);
    public static readonly SolidColorBrush LimeGreen = new(Color.LimeGreen);
    public static readonly SolidColorBrush Linen = new(Color.Linen);
    public static readonly SolidColorBrush Magenta = new(Color.Magenta);
    public static readonly SolidColorBrush Maroon = new(Color.Maroon);
    public static readonly SolidColorBrush MediumAquamarine = new(Color.MediumAquamarine);
    public static readonly SolidColorBrush MediumBlue = new(Color.MediumBlue);
    public static readonly SolidColorBrush MediumOrchid = new(Color.MediumOrchid);
    public static readonly SolidColorBrush MediumPurple = new(Color.MediumPurple);
    public static readonly SolidColorBrush MediumSeaGreen = new(Color.MediumSeaGreen);
    public static readonly SolidColorBrush MediumSlateBlue = new(Color.MediumSlateBlue);
    public static readonly SolidColorBrush MediumSpringGreen = new(Color.MediumSpringGreen);
    public static readonly SolidColorBrush MediumTurquoise = new(Color.MediumTurquoise);
    public static readonly SolidColorBrush MediumVioletRed = new(Color.MediumVioletRed);
    public static readonly SolidColorBrush MidnightBlue = new(Color.MidnightBlue);
    public static readonly SolidColorBrush MintCream = new(Color.MintCream);
    public static readonly SolidColorBrush MistyRose = new(Color.MistyRose);
    public static readonly SolidColorBrush Moccasin = new(Color.Moccasin);
    public static readonly SolidColorBrush NavajoWhite = new(Color.NavajoWhite);
    public static readonly SolidColorBrush Navy = new(Color.Navy);
    public static readonly SolidColorBrush OldLace = new(Color.DarkBlue);
    public static readonly SolidColorBrush Olive = new(Color.Olive);
    public static readonly SolidColorBrush OliveDrab = new(Color.OliveDrab);
    public static readonly SolidColorBrush Orange = new(Color.Orange);
    public static readonly SolidColorBrush OrangeRed = new(Color.OrangeRed);
    public static readonly SolidColorBrush Orchid = new(Color.Orchid);
    public static readonly SolidColorBrush PaleGoldenrod = new(Color.PaleGoldenrod);
    public static readonly SolidColorBrush PaleGreen = new(Color.MistyRose);
    public static readonly SolidColorBrush PaleTurquoise = new(Color.PaleTurquoise);
    public static readonly SolidColorBrush PaleVioletRed = new(Color.PaleVioletRed);
    public static readonly SolidColorBrush PapayaWhip = new(Color.PapayaWhip);
    public static readonly SolidColorBrush PeachPuff = new(Color.PeachPuff);
    public static readonly SolidColorBrush Peru = new(Color.Peru);
    public static readonly SolidColorBrush Pink = new(Color.Pink);
    public static readonly SolidColorBrush Plum = new(Color.Plum);
    public static readonly SolidColorBrush PowderBlue = new(Color.PowderBlue);
    public static readonly SolidColorBrush Purple = new(Color.Purple);
    public static readonly SolidColorBrush Red = new(Color.Red);
    public static readonly SolidColorBrush RosyBrown = new(Color.RosyBrown);
    public static readonly SolidColorBrush RoyalBlue = new(Color.RoyalBlue);
    public static readonly SolidColorBrush SaddleBrown = new(Color.SaddleBrown);
    public static readonly SolidColorBrush Salmon = new(Color.Salmon);
    public static readonly SolidColorBrush SandyBrown = new(Color.SandyBrown);
    public static readonly SolidColorBrush SeaGreen = new(Color.SeaGreen);
    public static readonly SolidColorBrush SeaShell = new(Color.SeaShell);
    public static readonly SolidColorBrush Sienna = new(Color.Sienna);
    public static readonly SolidColorBrush Silver = new(Color.Silver);
    public static readonly SolidColorBrush SkyBlue = new(Color.SkyBlue);
    public static readonly SolidColorBrush SlateBlue = new(Color.SlateBlue);
    public static readonly SolidColorBrush SlateGray = new(Color.SlateGray);
    public static readonly SolidColorBrush Snow = new(Color.Snow);
    public static readonly SolidColorBrush SpringGreen = new(Color.SpringGreen);
    public static readonly SolidColorBrush SteelBlue = new(Color.SteelBlue);
    public static readonly SolidColorBrush Tan = new(Color.Tan);
    public static readonly SolidColorBrush Teal = new(Color.Teal);
    public static readonly SolidColorBrush Thistle = new(Color.Thistle);
    public static readonly SolidColorBrush Tomato = new(Color.Tomato);
    public static readonly SolidColorBrush Transparent = new(Color.Transparent);
    public static readonly SolidColorBrush Turquoise = new(Color.Turquoise);
    public static readonly SolidColorBrush Violet = new(Color.Violet);
    public static readonly SolidColorBrush Wheat = new(Color.Wheat);
    public static readonly SolidColorBrush White = new(Color.White);
    public static readonly SolidColorBrush WhiteSmoke = new(Color.WhiteSmoke);
    public static readonly SolidColorBrush Yellow = new(Color.Yellow);
    public static readonly SolidColorBrush YellowGreen = new(Color.YellowGreen);
}

public abstract class Brush<T> : Element<T> where T : xf.BindableObject, new()
{
    protected internal override xf.BindableObject CreateView() => new T();
}
    
public class SolidColorBrush : Brush<xf.SolidColorBrush>, IBrush
{
    public SolidColorBrush(Color color) => Color = color;

    public Color Color {
        set => SetValue(xf.SolidColorBrush.ColorProperty, value);
    }
}

public class GradientStop : Element<xf.GradientStop>
{
    public GradientStop(Color color, float offset)
    {
        Color = color;
        Offset = offset;
    }
        
    public Color Color {
        set => SetValue(xf.GradientStop.ColorProperty, value);
    }

    public float Offset {
        set => SetValue(xf.GradientStop.OffsetProperty, value);
    } 
        
    protected internal override xf.BindableObject CreateView() => new xf.GradientStop();
}

public abstract class GradientBrush<T> : Brush<T> where T : xf.BindableObject, new()
{
    public GradientBrush() => ElementLists.Add<xf.GradientBrush>(nameof(GradientStops), brush => brush.GradientStops);

    public ElementList GradientStops {
        get => ElementLists[nameof(GradientStops)];
        set => ElementLists[nameof(GradientStops)] = value;
    } 
}

public class LinearGradientBrush : GradientBrush<xf.LinearGradientBrush>, IBrush
{
    public Point StartPoint {
        set => SetValue(xf.LinearGradientBrush.StartPointProperty, value);
    }

    public Point EndPoint {
        set => SetValue(xf.LinearGradientBrush.EndPointProperty, value);
    }
}

public class RadialGradientBrush : GradientBrush<xf.RadialGradientBrush>, IBrush
{
    public Point Center {
        set => SetValue(xf.RadialGradientBrush.CenterProperty, value);
    }

    public double Radius {
        set => SetValue(xf.RadialGradientBrush.RadiusProperty, value);
    }
}