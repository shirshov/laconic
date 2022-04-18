using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace Laconic.Demo;

[Register("AppDelegate")]
public class AppDelegate : MauiUIApplicationDelegate
{
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
