using System;

namespace Laconic.Maps
{
    public sealed class MapSpan
    {
        public MapSpan(Position center, double latitudeDegrees, double longitudeDegrees)
        {
            Center = center;
            LatitudeDegrees = Math.Min(Math.Max(latitudeDegrees, 8.993216059187306E-06), 90.0);
            LongitudeDegrees = Math.Min(Math.Max(longitudeDegrees, 8.993216059187306E-06), 180.0);
        }

        public Position Center { get; }
        public double LatitudeDegrees { get; }
        public double LongitudeDegrees { get; }
        public Distance Radius => new Distance(1000.0 * Math.Min(LatitudeDegreesToKm(LatitudeDegrees), LongitudeDegreesToKm(Center, LongitudeDegrees)) / 2.0);
        
        public MapSpan ClampLatitude(double north, double south)
        {
            north = Math.Min(Math.Max(north, 0.0), 90.0);
            south = Math.Max(Math.Min(south, 0.0), -90.0);
            var latitude = Math.Max(Math.Min(Center.Latitude, north), south);
            var val2 = Math.Min(north - latitude, -south + latitude) * 2.0;
            return new MapSpan(new Position(latitude, Center.Longitude), Math.Min(LatitudeDegrees, val2), LongitudeDegrees);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
                return false;
            if ((object) this == obj)
                return true;
            return (object) (obj as MapSpan) != null && this.Equals((MapSpan) obj);
        }

        public static MapSpan FromCenterAndRadius(Position center, Distance radius) => new MapSpan(center, 2.0 * DistanceToLatitudeDegrees(radius), 2.0 * DistanceToLongitudeDegrees(center, radius));
        
        public override int GetHashCode() => (Center.GetHashCode() * 397 ^ LongitudeDegrees.GetHashCode()) * 397 ^ LatitudeDegrees.GetHashCode();

        public static bool operator ==(MapSpan left, MapSpan right) => Equals((object) left, (object) right);

        public static bool operator !=(MapSpan left, MapSpan right) => !Equals((object) left, (object) right);
        
        public MapSpan WithZoom(double zoomFactor)
        {
            var val2 = Math.Min(90.0 - Center.Latitude, 90.0 + Center.Latitude) * 2.0;
            return new MapSpan(Center, Math.Min(LatitudeDegrees / zoomFactor, val2), LongitudeDegrees / zoomFactor);
        }

        static double DistanceToLatitudeDegrees(Distance distance) => distance.Kilometers / 40030.173592041145 * 360.0;

        static double DistanceToLongitudeDegrees(Position position, Distance distance)
        {
            var num = MapSpan.LatitudeCircumferenceKm(position);
            return distance.Kilometers / num * 360.0;
        }

        private bool Equals(MapSpan other)
        {
            if (Center.Equals((object) other.Center))
            {
                var num = LongitudeDegrees;
                if (num.Equals(other.LongitudeDegrees))
                {
                    num = LatitudeDegrees;
                    return num.Equals(other.LatitudeDegrees);
                }
            }
            return false;
        }

        static double LatitudeCircumferenceKm(Position position) => 40030.173592041145 * Math.Cos(position.Latitude * Math.PI / 180.0);

        static double LatitudeDegreesToKm(double latitudeDegrees) => 40030.173592041145 * latitudeDegrees / 360.0;

        static double LongitudeDegreesToKm(Position position, double longitudeDegrees) => LatitudeCircumferenceKm(position) * longitudeDegrees / 360.0;
    }
}