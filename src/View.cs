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
            init => SetValue(xf.View.HorizontalOptionsProperty, value);
        }

        public LayoutOptions VerticalOptions {
            init => SetValue(xf.View.VerticalOptionsProperty, value);
        }

        public Thickness Margin {
            init => SetValue(xf.View.MarginProperty, value);
        }
    }
}