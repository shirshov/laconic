namespace Laconic.Maps
{
    public class Pin : Element<Xamarin.Forms.Maps.Pin>
    {
        public PinType Type
        {
            set => SetValue(Xamarin.Forms.Maps.Pin.TypeProperty, (Xamarin.Forms.Maps.PinType)value);
        }

        public Position Position
        {
            set => SetValue(Xamarin.Forms.Maps.Pin.PositionProperty, new Xamarin.Forms.Maps.Position(value.Latitude, value.Longitude));
        }

        public string Label
        {
            set => SetValue(Xamarin.Forms.Maps.Pin.LabelProperty, value);
        }

        public string Address
        {
            set => SetValue(Xamarin.Forms.Maps.Pin.AddressProperty, value);
        }
        
        protected override Xamarin.Forms.BindableObject CreateView() => new Xamarin.Forms.Maps.Pin();
    }
}