using Foundation;
using UIKit;

namespace Laconic.Maps.Demo
{
    [Register("AppDelegate")]
    public class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Xamarin.Forms.Forms.Init();
            Xamarin.FormsMaps.Init();
            
            LoadApplication(new MapsApp());

            return base.FinishedLaunching(app, options);
        }
    }
}