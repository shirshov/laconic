using System;
using System.Collections.Generic;
using xf = Xamarin.Forms;

namespace Laconic
{
	// TODO: MenuItem etc.
    public class ToolbarItem : Element<xf.ToolbarItem>
    {
        public ImageSource IconImageSource {
			init => SetValue(xf.MenuItem.IconImageSourceProperty, value);
		}

        public string Text {
            init => SetValue(xf.MenuItem.TextProperty, value);
        }
        
        public Func<Signal> Clicked
        {
            init => SetEvent(nameof(Clicked), value,
                (ctl, handler) => ctl.Clicked += handler,
                (ctl, handler) => ctl.Clicked -= handler);
        }

        protected internal override xf.BindableObject CreateView() => new xf.ToolbarItem();
    }

    public interface Page
    {
        
    }
    
    public abstract partial class Page<T> : VisualElement<T>, Page where T : Xamarin.Forms.Page, new()
    {
		public IDictionary<Key, ToolbarItem> ToolbarItems { get; } = new Dictionary<Key, ToolbarItem>();
    }

    public class ContentPage : Page<xf.ContentPage>, IContentHost
    {
        public View? Content { get; set; }

        public override string ToString() => "ContentPage{" + Content + "}";
    }

    public partial class FlyoutPage : Page<xf.FlyoutPage>
    {
        public Element? Flyout { set; get; }
        public Element? Detail { set; get; }

        public Func<xf.BackButtonPressedEventArgs, Signal> BackButtonPressed {
            init => SetEvent(nameof(BackButtonPressed), value,
                (ctl, handler) => ctl.BackButtonPressed += handler,
                (ctl, handler) => ctl.BackButtonPressed -= handler);
        }
    }
}