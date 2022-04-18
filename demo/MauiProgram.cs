using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;

namespace Laconic.Demo;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
                fonts.AddFont("DIN Condensed Bold.ttf", "DINBold");
                fonts.AddFont("Font Awesome 5 Free-Solid-900.otf", "IconFont");
            });

		return builder.Build();
	}
}
