using System;

namespace Laconic.Maps
{
    public readonly struct Position
    {
        public Position(double latitude, double longitude)
        {
            Latitude = Math.Min(Math.Max(latitude, -90.0), 90.0);
            Longitude = Math.Min(Math.Max(longitude, -180.0), 180.0);
        }

        public double Latitude { get; }
        public double Longitude { get; }

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
                return false;
            var position = (Position) obj;
            return this.Latitude == position.Latitude && this.Longitude == position.Longitude;
        }

        public override int GetHashCode()
        {
            var num1 = Latitude;
            var num2 = num1.GetHashCode() * 397;
            num1 = Longitude;
            var hashCode = num1.GetHashCode();
            return num2 ^ hashCode;
        }

        public static bool operator ==(Position left, Position right) => Equals(left, right);

        public static bool operator !=(Position left, Position right) => !Equals(left, right);
        
        public static implicit operator Position((double latitude, double longitude) value) =>
            new Position(value.latitude, value.longitude);
    }
}