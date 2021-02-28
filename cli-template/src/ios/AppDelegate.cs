using Foundation;
using UIKit;

namespace Laconic.Template.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
        
        static void Main(string[] args) => UIApplication.Main(args, null, "AppDelegate");
    }
}
