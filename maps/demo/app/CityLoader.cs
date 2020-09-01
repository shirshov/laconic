using System.IO;
using System.Linq;
using System.Reflection;
using GeoJSON.Net.Feature;
using Newtonsoft.Json;

namespace Laconic.Maps.Demo
{
    class CityLoader
    {
        public static City[] Load()
        {
            var assembly = typeof(City).GetTypeInfo().Assembly;
            // data source: https://github.com/drei01/geojson-world-cities
            var stream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".largest-cities.geojson");
            var json = "";
            using (var reader = new StreamReader (stream))
            {  
                 json = reader.ReadToEnd ();
            }

            var source = JsonConvert.DeserializeObject <FeatureCollection>(json);

            static Polygon[] GetPolygons(FeatureCollection col, string cityName)
            {
                // Query
                
                var sourcePolygons =
                    from feat in col.Features
                    from prop in feat.Properties
                    from poly in (feat.Geometry as GeoJSON.Net.Geometry.Polygon).Coordinates
                    where prop.Value.Equals(cityName.ToUpper())
                    select poly.Coordinates;

                // Transform

                return sourcePolygons.Select(
                    p => new Polygon {
                        FillColor = (0, 0, 255, 25),
                        StrokeColor = Color.Blue,
                        StrokeWidth = 1,
                        Geopath = p.Select(x => new Position(x.Latitude, x.Longitude)).ToList()
                    }).ToArray();
            }

            return new[] {
                new City("Tokyo", 37_400_068, GetPolygons(source, "Tokyo"), false),
                new City("Delhi",  28_514_000, GetPolygons(source, "New Delhi"), false), // can't find boundaries for Delhi
                new City("Shanghai", 25_582_000, GetPolygons(source, "Shanghai"), false),
                new City("São Paulo", 21_650_000, GetPolygons(source, "Sao Paulo"), false),
                new City("Mexico City", 21_650_000, GetPolygons(source, "Mexico City"), false)
            };
        }
        
    }
}