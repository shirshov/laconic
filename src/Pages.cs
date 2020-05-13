using xf = Xamarin.Forms;

namespace Laconic
{
    public abstract partial class Page<T> : VisualElement<T> where T : Xamarin.Forms.Page, new()
    {
    }

    public class ContentPage : Page<xf.ContentPage>, IContentHost
    {
        public View? Content { get; set; }
    }
}