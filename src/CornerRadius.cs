namespace Laconic;

public readonly struct CornerRadius : IConvert
{
    readonly bool _isParameterized;

    public double TopLeft { get; }
    public double TopRight { get; }
    public double BottomLeft { get; }
    public double BottomRight { get; }

    CornerRadius(double uniformRadius) : this(uniformRadius, uniformRadius, uniformRadius, uniformRadius)
    {
    }

    CornerRadius(double topLeft, double topRight, double bottomLeft, double bottomRight)
    {
        _isParameterized = true;

        TopLeft = topLeft;
        TopRight = topRight;
        BottomLeft = bottomLeft;
        BottomRight = bottomRight;
    }

    public static implicit operator CornerRadius(double uniformRadius) => new(uniformRadius);

    public static implicit operator CornerRadius(
        (double topLeft, double topRight, double bottomLeft, double bottomRight) values)
        => new(values.topLeft, values.topRight, values.bottomLeft, values.bottomRight);

    bool Equals(CornerRadius other)
    {
        if (!_isParameterized && !other._isParameterized)
            return true;

        return TopLeft == other.TopLeft && TopRight == other.TopRight && BottomLeft == other.BottomLeft &&
               BottomRight == other.BottomRight;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj))
            return false;

        return obj is CornerRadius cornerRadius && Equals(cornerRadius);
    }

    public override int GetHashCode()
    {
        unchecked {
            var hashCode = TopLeft.GetHashCode();
            hashCode = (hashCode * 397) ^ TopRight.GetHashCode();
            hashCode = (hashCode * 397) ^ BottomLeft.GetHashCode();
            hashCode = (hashCode * 397) ^ BottomRight.GetHashCode();
            return hashCode;
        }
    }

    public static bool operator ==(CornerRadius left, CornerRadius right) => left.Equals(right);

    public static bool operator !=(CornerRadius left, CornerRadius right) => !left.Equals(right);

    public void Deconstruct(out double topLeft, out double topRight, out double bottomLeft, out double bottomRight)
    {
        topLeft = TopLeft;
        topRight = TopRight;
        bottomLeft = BottomLeft;
        bottomRight = BottomRight;
    }

    object IConvert.ToNative() =>
        _isParameterized
            ? new Xamarin.Forms.CornerRadius(TopLeft, TopRight, BottomLeft, BottomRight)
            : new Xamarin.Forms.CornerRadius(TopLeft);
}