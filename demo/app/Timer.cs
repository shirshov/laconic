using System;

namespace Laconic.Demo
{
    class Timer
    {
        static StackLayout View(string display, string buttonTitle, Func<Signal> buttonAction) => new() {
            Spacing = 30,
            Padding = 30,
            ["display"] = new Label {
                Text = display, FontSize = 20, HorizontalOptions = LayoutOptions.Center
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

        public static VisualElement<Xamarin.Forms.StackLayout> Content() => Element.WithContext(ctx => {
            var (state, setState) = ctx.UseLocalState(0);
            var timer = ctx.UseTimer(TimeSpan.FromMilliseconds(300), start: false);

            setState(state + 1);
            return View(state.ToString(),
                timer.IsRunning ? "Stop" : "Start",
                timer.IsRunning ? () => timer.Stop() : () => timer.Start()
            );
        });
    }
}