using System;

namespace Laconic.Maps
{
    /// <summary>Struct that encapsulates a distance (natively stored as a double of meters).</summary>
    /// <remarks>To be added.</remarks>
    public struct Distance
    {
        public Distance(double meters) => this.Meters = meters;
        
        public double Meters { get; }

        public double Miles => this.Meters / 1609.344;

        public double Kilometers => this.Meters / 1000.0;

        public static Distance FromMiles(double miles)
        {
            if (miles < 0.0)
                miles = 0.0;
            return new Distance(miles * 1609.344);
        }

        public static Distance FromMeters(double meters)
        {
            if (meters < 0.0)
                meters = 0.0;
            return new Distance(meters);
        }

        public static Distance FromKilometers(double kilometers)
        {
            if (kilometers < 0.0)
                kilometers = 0.0;
            return new Distance(kilometers * 1000.0);
        }

        public static Distance BetweenPositions(Position position1, Position position2)
        {
            var radians1 = position1.Latitude.ToRadians();
            var radians2 = position1.Longitude.ToRadians();
            var radians3 = position2.Latitude.ToRadians();
            var radians4 = position2.Longitude.ToRadians();
            var num1 = Math.Sin((radians3 - radians1) / 2.0);
            var num2 = num1 * num1;
            var num3 = radians2;
            var num4 = Math.Sin((radians4 - num3) / 2.0);
            var num5 = num4 * num4;
            var d = num2 + Math.Cos(radians1) * Math.Cos(radians3) * num5;
            return FromKilometers(12742.0 * Math.Atan2(Math.Sqrt(d), Math.Sqrt(1.0 - d)));
        }

        public bool Equals(Distance other) => this.Meters.Equals(other.Meters);

        public override bool Equals(object? obj) => obj is Distance other && Equals(other);

        public override int GetHashCode() => Meters.GetHashCode();

        public static bool operator ==(Distance left, Distance right) => left.Equals(right);

        public static bool operator !=(Distance left, Distance right) => !left.Equals(right);
    }

    static class GeographyUtils
    {
        public static double ToRadians(this double degrees) => degrees * Math.PI / 180.0;

        public static double ToDegrees(this double radians) => radians / Math.PI * 180.0;
    }

}