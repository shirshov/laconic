using Shouldly;
using System.Linq;
using maps = Xamarin.Forms.Maps;

using Xunit;

namespace Laconic.Maps.Tests
{
    public class MapTests
    {
        [Fact]
        public void one_Pin_added_to_map()
        {
            static Map MapMaker(int s) => new Map {Pins = {[0] = new Pin {Label = s.ToString()}}};

            var binder = Binder.CreateForTest(0, (s, g) => s);

            var map = binder.CreateElement(MapMaker);
            
            map.Pins.Count.ShouldBe(1);
            map.Pins[0].Label.ShouldBe("0");
        }

        [Fact]
        public void Pin_is_updated()
        {
            static Map MapMaker(int s) => new Map {Pins = {[0] = new Pin {Label = s.ToString()}}};

            var binder = Binder.CreateForTest(0, (s, g) => s + 1);

            var map = binder.CreateElement(MapMaker);
            
            binder.Send(new Signal(""));
            
            map.Pins.Count.ShouldBe(1);
            map.Pins[0].Label.ShouldBe("1");
        }

        [Fact]
        public void Pin_removed_from_list()
        {
            static Map MapMaker(int s) => new Map {Pins = {[0] = s > 0 ? null : new Pin {Label = s.ToString()}}};

            var binder = Binder.CreateForTest(0, (s, g) => s + 1);

            var map = binder.CreateElement(MapMaker);
            
            map.Pins.Count.ShouldBe(1);
            map.Pins[0].Label.ShouldBe("0");
            
            binder.Send(new Signal(""));
            
            map.Pins.Count.ShouldBe(0);
        }

        [Fact]
        public void MapElements_list_is_set_directly()
        {
            var lists = new[] {
                new {Key = 0, Polygons = new[] {new Polygon()} },
                new {Key = 0, Polygons = new Polygon[0] }
            };

            Map MapMaker(int s) => new Map {MapElements = lists[s].Polygons.ToDictionary(x => 0, x => (Element)x)};

            var binder = Binder.CreateForTest(0, (s, g) => s + 1);

            var map = binder.CreateElement(MapMaker);
           
            map.MapElements.Count.ShouldBe(1);
            
            binder.Send(new Signal(""));
 
            map.MapElements.Count.ShouldBe(0);
        }
        
        [Fact(Skip = "Map moves, but VisibleRegion property is not updated")]
        public void VisibleRegion_changed()
        {
            static Map MapMaker(int s) => new Map {
                VisibleRegion = MapSpan.FromCenterAndRadius(new Position(0, 0), Distance.FromMeters(100))
            };

            var binder = Binder.CreateForTest(0, (s, g) => s + 1);

            var map = binder.CreateElement(MapMaker);
            
            map.VisibleRegion.ShouldBe(maps.MapSpan.FromCenterAndRadius(new maps.Position(0, 0), maps.Distance.FromMeters(100)));
        }
    }
}