using System.Collections.Generic;
using xf = Xamarin.Forms;

namespace Laconic
{
	// TODO: MenuItem etc.
    public class ToolbarItem : Element<xf.ToolbarItem>
    {
        public ImageSource IconImageSource {
			set => SetValue(xf.MenuItem.IconImageSourceProperty, value);
		}

        public string Text {
            set => SetValue(xf.MenuItem.TextProperty, value);
        }
        
        public System.Func<Signal> Clicked
        {
            set => SetEvent(nameof(Clicked), value,
                (ctl, handler) => ctl.Clicked += handler,
                (ctl, handler) => ctl.Clicked -= handler);
        }

        protected internal override xf.BindableObject CreateView() => new xf.ToolbarItem();
    }

    public abstract partial class Page<T> : VisualElement<T> where T : Xamarin.Forms.Page, new()
    {
		public IDictionary<Key, ToolbarItem> ToolbarItems { get; } = new Dictionary<Key, ToolbarItem>();
    }

    public class ContentPage : Page<xf.ContentPage>, IContentHost
    {
        public View? Content { get; set; }
    }
}