using System.Globalization;

namespace Laconic;

public readonly struct Point : IConvert
{
    public static Point Zero;

    public double X { get; }

    public double Y { get; }

    public bool IsEmpty {
        get {
            if (X == 0.0) {
                return Y == 0.0;
            }

            return false;
        }
    }

    public override string ToString() =>
        $"{{X={X.ToString(CultureInfo.InvariantCulture)} Y={Y.ToString(CultureInfo.InvariantCulture)}}}";

    public object ToNative() => new Xamarin.Forms.Point(X, Y);

    public Point(double x, double y)
    {
        this = default(Point);
        X = x;
        Y = y;
    }

    public Point(Size sz)
    {
        this = default(Point);
        X = sz.Width;
        Y = sz.Height;
    }

    public override bool Equals(object o)
    {
        if (!(o is Point)) {
            return false;
        }

        return this == (Point) o;
    }

    public override int GetHashCode() => X.GetHashCode() ^ (Y.GetHashCode() * 397);

    public static explicit operator Size(Point pt) => new(pt.X, pt.Y);

    public static Point operator +(Point pt, Size sz) => new(pt.X + sz.Width, pt.Y + sz.Height);

    public static Point operator -(Point pt, Size sz) => new(pt.X - sz.Width, pt.Y - sz.Height);

    public static bool operator ==(Point ptA, Point ptB)
    {
        if (ptA.X == ptB.X) {
            return ptA.Y == ptB.Y;
        }

        return false;
    }

    public static bool operator !=(Point ptA, Point ptB)
    {
        if (ptA.X == ptB.X) {
            return ptA.Y != ptB.Y;
        }

        return true;
    }

    public static implicit operator Point((double X, double Y) value) => new(value.X, value.Y);
        
    public double Distance(Point other) => Math.Sqrt(Math.Pow(X - other.X, 2.0) + Math.Pow(Y - other.Y, 2.0));

    public void Deconstruct(out double x, out double y)
    {
        x = X;
        y = Y;
    }
}