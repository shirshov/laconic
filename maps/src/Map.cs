using System.Collections.Generic;
using System.Linq;
using xf = Xamarin.Forms;

namespace Laconic.Maps
{
    public class Pin : Element<xf.Maps.Pin>, IElement
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
    
    public class Map : View<Xamarin.Forms.Maps.Map>, ICustomElementCollection
    {
 //       public static readonly BindableProperty MoveToLastRegionOnLayoutChangeProperty = BindableProperty.Create(nameof (MoveToLastRegionOnLayoutChange), typeof (bool), typeof (Map), (object) true);
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
        
        public IDictionary<Key, Pin> Pins { get; } = new Dictionary<Key, Pin>(); 

        IDictionary<Key, IElement> ICustomElementCollection.Children => Pins.ToDictionary(x => x.Key, x => (IElement)x.Value);
        
        void ICustomElementCollection.RemoveAt(Xamarin.Forms.Element parent, int index) => (parent as Xamarin.Forms.Maps.Map).Pins.RemoveAt(index);

        void ICustomElementCollection.Insert(Xamarin.Forms.Element parent, int index, Xamarin.Forms.BindableObject child) =>
            (parent as Xamarin.Forms.Maps.Map).Pins.Insert(index, (Xamarin.Forms.Maps.Pin)child);

        void ICustomElementCollection.Set(Xamarin.Forms.Element parent, int index, Xamarin.Forms.Element child) =>
            (parent as Xamarin.Forms.Maps.Map).Pins[index] = (Xamarin.Forms.Maps.Pin) child;

        Xamarin.Forms.Element ICustomElementCollection.Get(Xamarin.Forms.Element parent, int index) => (parent as Xamarin.Forms.Maps.Map).Pins[index];
    }
}