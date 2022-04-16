namespace Laconic;

public readonly struct Thickness
{
    public static implicit operator Thickness(double uniformSize) => new Thickness(uniformSize);

    public static implicit operator Thickness((double horizontalSize, double verticalSize) values)
        => new Thickness(values.horizontalSize, values.verticalSize);

    public static implicit operator Thickness((double left, double top, double right, double bottom) values)
        => new Thickness(values.left, values.top, values.right, values.bottom);

    public double Left { get; }
    public double Top { get; }
    public double Right { get; }
    public double Bottom { get; }

    Thickness(double uniformSize) => this = new Thickness(uniformSize, uniformSize, uniformSize, uniformSize);

    Thickness(double horizontalSize, double verticalSize) =>
        this = new Thickness(horizontalSize, verticalSize, horizontalSize, verticalSize);

    Thickness(double left, double top, double right, double bottom)
    {
        this = default(Thickness);
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }

    bool Equals(Thickness other)
    {
        if (Left.Equals(other.Left) && Top.Equals(other.Top) && Right.Equals(other.Right)) {
            return Bottom.Equals(other.Bottom);
        }

        return false;
    }

    public override bool Equals(object obj) => obj switch {
        null => false,
        Thickness thickness => Equals(thickness),
        _ => false
    };

    public override int GetHashCode() =>
        (((((Left.GetHashCode() * 397) ^ Top.GetHashCode()) * 397) ^ Right.GetHashCode()) * 397) ^
        Bottom.GetHashCode();

    public static bool operator ==(Thickness left, Thickness right) => left.Equals(right);

    public static bool operator !=(Thickness left, Thickness right) => !left.Equals(right);

    public void Deconstruct(out double left, out double top, out double right, out double bottom)
    {
        left = Left;
        top = Top;
        right = Right;
        bottom = Bottom;
    }
}