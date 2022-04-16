namespace Laconic;

public struct Size
{
    readonly double _width;

    readonly double _height;

    public static readonly Size Zero = new();

    public bool IsZero {
        get {
            if (_width == 0.0) {
                return _height == 0.0;
            }

            return false;
        }
    }

    public double Width => _width;

    public double Height => _height;

    public Size(double width, double height)
    {
        if (double.IsNaN(width)) {
            throw new ArgumentException("NaN is not a valid value for width");
        }

        if (double.IsNaN(height)) {
            throw new ArgumentException("NaN is not a valid value for height");
        }

        _width = width;
        _height = height;
    }

    public static Size operator +(Size s1, Size s2) => new Size(s1._width + s2._width, s1._height + s2._height);

    public static Size operator -(Size s1, Size s2) => new Size(s1._width - s2._width, s1._height - s2._height);

    public static Size operator *(Size s1, double value) => new Size(s1._width * value, s1._height * value);

    public static bool operator ==(Size s1, Size s2)
    {
        if (s1._width == s2._width) {
            return s1._height == s2._height;
        }

        return false;
    }

    public static bool operator !=(Size s1, Size s2)
    {
        if (s1._width == s2._width) {
            return s1._height != s2._height;
        }

        return true;
    }

    public static explicit operator Point(Size size) => new Point(size.Width, size.Height);

    bool Equals(Size other) => _width.Equals(other._width) && _height.Equals(other._height);

    public override bool Equals(object obj) => obj switch {
        null => false,
        Size size => Equals(size),
        _ => false
    };

    public override int GetHashCode() => (_width.GetHashCode() * 397) ^ _height.GetHashCode();
}