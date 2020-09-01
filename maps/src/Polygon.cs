using System.Collections.Generic;

namespace Laconic.Maps
{
    public class Polygon : MapElement<Xamarin.Forms.Maps.Polygon>
    {
        protected override Xamarin.Forms.BindableObject CreateView() => new Xamarin.Forms.Maps.Polygon();

        public Color FillColor
        {
            get =>  GetValue<Color>(Xamarin.Forms.Maps.Polygon.FillColorProperty);
            set => SetValue(Xamarin.Forms.Maps.Polygon.FillColorProperty, value);
        }

        public List<Position> Geopath {
            set => SetValue(nameof(Geopath), value, polygon => {
                polygon.Geopath.Clear();
                foreach (var x in value)
                    polygon.Geopath.Add(new Xamarin.Forms.Maps.Position(x.Latitude, x.Longitude));
            });
        }
    }
}