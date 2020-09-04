using System.Collections.Generic;
using xf = Xamarin.Forms;

namespace Laconic
{
    public interface View 
    {
        Dictionary<Key, IGestureRecognizer> GestureRecognizers { get; }
    }
    public abstract class View<T> : VisualElement<T>, View where T : xf.View, new()
    {
        public LayoutOptions HorizontalOptions {
            get => GetValue<LayoutOptions>(xf.View.HorizontalOptionsProperty);
            set => SetValue(xf.View.HorizontalOptionsProperty, value);
        }

        public LayoutOptions VerticalOptions {
            get => GetValue<LayoutOptions>(xf.View.VerticalOptionsProperty);
            set => SetValue(xf.View.VerticalOptionsProperty, value);
        }

        public Thickness Margin {
            get => GetValue<Thickness>(xf.View.MarginProperty);
            set => SetValue(xf.View.MarginProperty, value);
        }
    }

}