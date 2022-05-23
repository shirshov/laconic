namespace Laconic;

interface ColorSource { }

// class Default : ColorSource {};
// class Accent : ColorSource {};
record Hex(string Value) : ColorSource;
record Hsla(double H, double S, double L, double A = 1.0) : ColorSource;
record Rgba(byte R, byte G, byte B, byte A = 255) : ColorSource;

public readonly struct Color : IConvert, IEquatable<Color>, IBrush
{
    public static readonly Color AliceBlue = (240, 248, 255);
    public static readonly Color AntiqueWhite = (250, 235, 215);
    public static readonly Color Aqua = (0, 255, 255);
    public static readonly Color Aquamarine = (127, 255, 212);
    public static readonly Color Azure = (240, 255, 255);
    public static readonly Color Beige = (245, 245, 220);
    public static readonly Color Bisque = (255, 228, 196);
    public static readonly Color Black = (0, 0, 0);
    public static readonly Color BlanchedAlmond = (255, 235, 205);
    public static readonly Color Blue = (0, 0, 255);
    public static readonly Color BlueViolet = (138, 43, 226);
    public static readonly Color Brown = (165, 42, 42);
    public static readonly Color BurlyWood = (222, 184, 135);
    public static readonly Color CadetBlue = (95, 158, 160);
    public static readonly Color Chartreuse = (127, 255, 0);
    public static readonly Color Chocolate = (210, 105, 30);
    public static readonly Color Coral = (255, 127, 80);
    public static readonly Color CornflowerBlue = (100, 149, 237);
    public static readonly Color Cornsilk = (255, 248, 220);
    public static readonly Color Crimson = (220, 20, 60);
    public static readonly Color Cyan = (0, 255, 255);
    public static readonly Color DarkBlue = (0, 0, 139);
    public static readonly Color DarkCyan = (0, 139, 139);
    public static readonly Color DarkGoldenrod = (184, 134, 11);
    public static readonly Color DarkGray = (169, 169, 169);
    public static readonly Color DarkGreen = (0, 100, 0);
    public static readonly Color DarkKhaki = (189, 183, 107);
    public static readonly Color DarkMagenta = (139, 0, 139);
    public static readonly Color DarkOliveGreen = (85, 107, 47);
    public static readonly Color DarkOrange = (255, 140, 0);
    public static readonly Color DarkOrchid = (153, 50, 204);
    public static readonly Color DarkRed = (139, 0, 0);
    public static readonly Color DarkSalmon = (233, 150, 122);
    public static readonly Color DarkSeaGreen = (143, 188, 143);
    public static readonly Color DarkSlateBlue = (72, 61, 139);
    public static readonly Color DarkSlateGray = (47, 79, 79);
    public static readonly Color DarkTurquoise = (0, 206, 209);
    public static readonly Color DarkViolet = (148, 0, 211);
    public static readonly Color DeepPink = (255, 20, 147);
    public static readonly Color DeepSkyBlue = (0, 191, 255);
    public static readonly Color DimGray = (105, 105, 105);
    public static readonly Color DodgerBlue = (30, 144, 255);
    public static readonly Color Firebrick = (178, 34, 34);
    public static readonly Color FloralWhite = (255, 250, 240);
    public static readonly Color ForestGreen = (34, 139, 34);
    public static readonly Color Fuchsia = (255, 0, 255);
    public static readonly Color Gainsboro = (220, 220, 220);
    public static readonly Color GhostWhite = (248, 248, 255);
    public static readonly Color Gold = (255, 215, 0);
    public static readonly Color Goldenrod = (218, 165, 32);
    public static readonly Color Gray = (128, 128, 128);
    public static readonly Color Green = (0, 128, 0);
    public static readonly Color GreenYellow = (173, 255, 47);
    public static readonly Color Honeydew = (240, 255, 240);
    public static readonly Color HotPink = (255, 105, 180);
    public static readonly Color IndianRed = (205, 92, 92);
    public static readonly Color Indigo = (75, 0, 130);
    public static readonly Color Ivory = (255, 255, 240);
    public static readonly Color Khaki = (240, 230, 140);
    public static readonly Color Lavender = (230, 230, 250);
    public static readonly Color LavenderBlush = (255, 240, 245);
    public static readonly Color LawnGreen = (124, 252, 0);
    public static readonly Color LemonChiffon = (255, 250, 205);
    public static readonly Color LightBlue = (173, 216, 230);
    public static readonly Color LightCoral = (240, 128, 128);
    public static readonly Color LightCyan = (224, 255, 255);
    public static readonly Color LightGoldenrodYellow = (250, 250, 210);
    public static readonly Color LightGray = (211, 211, 211);
    public static readonly Color LightGreen = (144, 238, 144);
    public static readonly Color LightPink = (255, 182, 193);
    public static readonly Color LightSalmon = (255, 160, 122);
    public static readonly Color LightSeaGreen = (32, 178, 170);
    public static readonly Color LightSkyBlue = (135, 206, 250);
    public static readonly Color LightSlateGray = (119, 136, 153);
    public static readonly Color LightSteelBlue = (176, 196, 222);
    public static readonly Color LightYellow = (255, 255, 224);
    public static readonly Color Lime = (0, 255, 0);
    public static readonly Color LimeGreen = (50, 205, 50);
    public static readonly Color Linen = (250, 240, 230);
    public static readonly Color Magenta = (255, 0, 255);
    public static readonly Color Maroon = (128, 0, 0);
    public static readonly Color MediumAquamarine = (102, 205, 170);
    public static readonly Color MediumBlue = (0, 0, 205);
    public static readonly Color MediumOrchid = (186, 85, 211);
    public static readonly Color MediumPurple = (147, 112, 219);
    public static readonly Color MediumSeaGreen = (60, 179, 113);
    public static readonly Color MediumSlateBlue = (123, 104, 238);
    public static readonly Color MediumSpringGreen = (0, 250, 154);
    public static readonly Color MediumTurquoise = (72, 209, 204);
    public static readonly Color MediumVioletRed = (199, 21, 133);
    public static readonly Color MidnightBlue = (25, 25, 112);
    public static readonly Color MintCream = (245, 255, 250);
    public static readonly Color MistyRose = (255, 228, 225);
    public static readonly Color Moccasin = (255, 228, 181);
    public static readonly Color NavajoWhite = (255, 222, 173);
    public static readonly Color Navy = (0, 0, 128);
    public static readonly Color OldLace = (253, 245, 230);
    public static readonly Color Olive = (128, 128, 0);
    public static readonly Color OliveDrab = (107, 142, 35);
    public static readonly Color Orange = (255, 165, 0);
    public static readonly Color OrangeRed = (255, 69, 0);
    public static readonly Color Orchid = (218, 112, 214);
    public static readonly Color PaleGoldenrod = (238, 232, 170);
    public static readonly Color PaleGreen = (152, 251, 152);
    public static readonly Color PaleTurquoise = (175, 238, 238);
    public static readonly Color PaleVioletRed = (219, 112, 147);
    public static readonly Color PapayaWhip = (255, 239, 213);
    public static readonly Color PeachPuff = (255, 218, 185);
    public static readonly Color Peru = (205, 133, 63);
    public static readonly Color Pink = (255, 192, 203);
    public static readonly Color Plum = (221, 160, 221);
    public static readonly Color PowderBlue = (176, 224, 230);
    public static readonly Color Purple = (128, 0, 128);
    public static readonly Color Red = (255, 0, 0);
    public static readonly Color RosyBrown = (188, 143, 143);
    public static readonly Color RoyalBlue = (65, 105, 225);
    public static readonly Color SaddleBrown = (139, 69, 19);
    public static readonly Color Salmon = (250, 128, 114);
    public static readonly Color SandyBrown = (244, 164, 96);
    public static readonly Color SeaGreen = (46, 139, 87);
    public static readonly Color SeaShell = (255, 245, 238);
    public static readonly Color Sienna = (160, 82, 45);
    public static readonly Color Silver = (192, 192, 192);
    public static readonly Color SkyBlue = (135, 206, 235);
    public static readonly Color SlateBlue = (106, 90, 205);
    public static readonly Color SlateGray = (112, 128, 144);
    public static readonly Color Snow = (255, 250, 250);
    public static readonly Color SpringGreen = (0, 255, 127);
    public static readonly Color SteelBlue = (70, 130, 180);
    public static readonly Color Tan = (210, 180, 140);
    public static readonly Color Teal = (0, 128, 128);
    public static readonly Color Thistle = (216, 191, 216);
    public static readonly Color Tomato = (255, 99, 71);
    public static readonly Color Transparent = (255, 255, 255, 0);
    public static readonly Color Turquoise = (64, 224, 208);
    public static readonly Color Violet = (238, 130, 238);
    public static readonly Color Wheat = (245, 222, 179);
    public static readonly Color White = (255, 255, 255);
    public static readonly Color WhiteSmoke = (245, 245, 245);
    public static readonly Color Yellow = (255, 255, 0);
    public static readonly Color YellowGreen = (154, 205, 50);

    // public static Color Default => new(new Default());
    // public static Color Accent => new(new Accent());

    public static implicit operator Color(string hexValue) => new(new Hex(hexValue));

    public static implicit operator Color((byte r, byte g, byte b) rgbValues)
        => new(new Rgba(rgbValues.r, rgbValues.g, rgbValues.b));

    public static implicit operator Color((byte r, byte g, byte b, byte a) rgbValues)
        => new(new Rgba(rgbValues.r, rgbValues.g, rgbValues.b, rgbValues.a));

    readonly ColorSource _value;

    Color(ColorSource value) => _value = value;

    public static Color FromHsla(double h, double s, double l, double a = 1.0) => new(new Hsla(h, s, l, a));

    public static Color FromArgb(string value) => new (new Hex(value));

    object IConvert.ToNative() => ToXamarinFormsColor();
        
    Maui.Graphics.Color ToXamarinFormsColor() => _value switch {
        // Default _ => Maui.Graphics.Color..Default,
        // Accent _ => Maui.Graphics.Color..Accent,
        Hex h => Maui.Graphics.Color.FromArgb(h.Value),
        Hsla(var h, var s, var l, var a) => Maui.Graphics.Color.FromHsla(h, s, l, a),
        Rgba x => Maui.Graphics.Color.FromRgba(x.R, x.G, x.B, x.A),
        _ => throw new NotImplementedException($"Support for the color value '{_value}' is not implemented")
    };

    public bool Equals(Color other) => _value.Equals(other._value);

    public override bool Equals(object? obj) => obj is Color other && Equals(other);

    public override int GetHashCode() => _value.GetHashCode();

    public static bool operator ==(Color left, Color right) => left.Equals(right);

    public static bool operator !=(Color left, Color right) => !left.Equals(right);
}