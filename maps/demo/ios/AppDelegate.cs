using Foundation;
using UIKit;
using Xamarin;

namespace Laconic.Maps.Demo
{
    [Register("AppDelegate")]
    public class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Xamarin.Forms.Forms.Init();
            FormsMaps.Init();
            // FormsMaterial.Init();
            
            LoadApplication(new MapsApp());

            return base.FinishedLaunching(app, options);
        }
    }
}