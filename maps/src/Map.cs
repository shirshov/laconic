using System.Collections;
using xf = Xamarin.Forms;

namespace Laconic.Maps
{
    public enum MapType
    {
        Street,
        Satellite,
        Hybrid
    }

      public enum PinType
      {
        Generic,
        Place,
        SavedPin,
        SearchResult
      }

      public class Map : View<Xamarin.Forms.Maps.Map>
    {
        public Map()
        {
            // TODO: generic parameter or casting should not be necessary
            ElementLists.Add<xf.Maps.Map>(nameof(Pins), map => (IList)map.Pins);
            ElementLists.Add<xf.Maps.Map>(nameof(MapElements), map => (IList)map.MapElements);
        }

        public ElementList Pins => ElementLists[nameof(Pins)];

        public ElementList MapElements {
            get => ElementLists[nameof(MapElements)];
            set => ElementLists[nameof(MapElements)] = value;
        }

        public Maps.MapSpan VisibleRegion {
            set => SetValue(nameof(VisibleRegion), value, map => {
                if (value != null)
                    map.MoveToRegion(new xf.Maps.MapSpan(
                        new xf.Maps.Position(value.Center.Latitude, value.Center.Longitude), value.LatitudeDegrees, value.LongitudeDegrees));
            });
        }

        public MapType MapType {
            get => (MapType)GetValue<xf.Maps.MapType>(xf.Maps.Map.MapTypeProperty);
            set => SetValue(xf.Maps.Map.MapTypeProperty, (xf.Maps.MapType)value);
        }

        public bool IsShowingUser {
            get => GetValue<bool>(xf.Maps.Map.IsShowingUserProperty);
            set => SetValue(xf.Maps.Map.IsShowingUserProperty, value);
        }

        public bool TrafficEnabled {
            get => GetValue<bool>(xf.Maps.Map.TrafficEnabledProperty);
            set => SetValue(xf.Maps.Map.TrafficEnabledProperty, value);
        }
        
        public bool HasScrollEnabled {
            get => GetValue<bool>(xf.Maps.Map.HasScrollEnabledProperty);
            set => SetValue(xf.Maps.Map.HasScrollEnabledProperty, value);
        }
        
        public bool HasZoomEnabled {
            get => GetValue<bool>(xf.Maps.Map.HasZoomEnabledProperty);
            set => SetValue(xf.Maps.Map.HasZoomEnabledProperty, value);
        }
    }
}