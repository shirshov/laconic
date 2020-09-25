namespace Laconic.Demo
{
    public class WebViewPage : Xamarin.Forms.ContentPage
    {
        public WebViewPage()
        {
            var binder = Binder.Create("", (s, g) => s);
            Content = binder.CreateElement(s => new WebView {
                Source = "https://google.com"
            });
        }
    }
}