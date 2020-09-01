namespace Laconic.Maps
{
    public abstract class MapElement<T> : Element<T> where T : Xamarin.Forms.BindableObject
    {
        public Color StrokeColor
        {
            get => GetValue<Color>(Xamarin.Forms.Maps.MapElement.StrokeColorProperty);
            set => SetValue(Xamarin.Forms.Maps.MapElement.StrokeColorProperty,  value);
        }

        public float StrokeWidth
        {
            get => GetValue<float>(Xamarin.Forms.Maps.MapElement.StrokeWidthProperty);
            set => SetValue(Xamarin.Forms.Maps.MapElement.StrokeWidthProperty, value);
        }
    }
}