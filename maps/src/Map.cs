using System.Collections;
using xf = Xamarin.Forms;

namespace Laconic.Maps
{
    public class Pin : Element<xf.Maps.Pin>
    {
        public xf.Maps.PinType Type
        {
            set => SetValue(xf.Maps.Pin.TypeProperty, value);
        }

        public xf.Maps.Position Position
        {
            set => SetValue(xf.Maps.Pin.PositionProperty, value);
        }

        public string Label
        {
            set => SetValue(xf.Maps.Pin.LabelProperty, value);
        }

        public string Address
        {
            set => SetValue(xf.Maps.Pin.AddressProperty, value);
        }
        
        protected override xf.BindableObject CreateView() => new xf.Maps.Pin();
    }
    
    public class Map : View<Xamarin.Forms.Maps.Map>
    {
        public Map()
        {
            // TODO: generic parameter, casting should not be necessary
            ElementLists.Add<xf.Maps.Map>(nameof(Pins), map => (IList)map.Pins);
            ElementLists.Add<xf.Maps.Map>(nameof(MapElements), map => (IList)map.MapElements);
        }

        public ElementList Pins => ElementLists[nameof(Pins)];

        public ElementList MapElements => ElementLists[nameof(MapElements)];
        
        public xf.Maps.MapSpan? VisibleRegion {
            set => SetValue(nameof(VisibleRegion), value, map => {
                if (value != null)
                    map.MoveToRegion(value);
            });
        }

        public xf.Maps.MapType MapType {
            get => GetValue<xf.Maps.MapType>(xf.Maps.Map.MapTypeProperty);
            set => SetValue(xf.Maps.Map.MapTypeProperty, value);
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