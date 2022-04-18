namespace Laconic.Demo;

static class Timer
{
    static StackLayout View(TimeSpan elapsed, string buttonTitle, Func<Signal> buttonAction) => new() {
        Spacing = 30,
        Padding = 30,
        ["display"] = new Label {
            Text = elapsed.TotalSeconds.ToString("0.0"), 
            FontFamily = "DINBold",
            FontSize = 50, 
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center
        },
        ["button"] = new Button {
            Text = buttonTitle,
            TextColor = Color.White,
            FontSize = 20,
            Clicked = buttonAction,
            BackgroundColor = Color.Coral,
            BorderColor = Color.Chocolate,
            BorderWidth = 2,
            CornerRadius = 20,
            HorizontalOptions = LayoutOptions.Center,
            Padding = (30, 0)
        }
    };

    public static VisualElement<xf.StackLayout> Content() => Element.WithContext("timer", ctx => {
        var timer = ctx.UseTimer(TimeSpan.FromMilliseconds(100), start: false);

        return View(timer.Elapsed,
            timer.IsRunning ? "Stop" : "Start",
            timer.IsRunning ? () => timer.Stop() : () => timer.Start()
        );
    });
}