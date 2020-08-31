using Shouldly;
using maps = Xamarin.Forms.Maps;

using Xunit;

namespace Laconic.Maps.Tests
{
    public class MapTests
    {
        [Fact]
        public void Pin_added_to_list()
        {
            static Map MapMaker(int s) => new Map {Pins = {[0] = new Pin {Label = s.ToString()}}};

            var binder = Binder.Create(0, (s, g) => s + 1);

            var map = binder.CreateElement(MapMaker);
            
            map.Pins.Count.ShouldBe(1);
            map.Pins[0].Label.ShouldBe("0");
        }

        [Fact(Skip = "Map moves, but VisibleRegion property is not updated")]
        public void VisibleRegion_changed()
        {
            static Map MapMaker(int s) => new Map {
                VisibleRegion = maps.MapSpan.FromCenterAndRadius(new maps.Position(0, 0), maps.Distance.FromMeters(100))
            };

            var binder = Binder.Create(0, (s, g) => s + 1);

            var map = binder.CreateElement(MapMaker);
            
            map.VisibleRegion.ShouldBe(maps.MapSpan.FromCenterAndRadius(new maps.Position(0, 0), maps.Distance.FromMeters(100)));
            
        }
    }
}