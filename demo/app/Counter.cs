using System;
using xf = Xamarin.Forms;

namespace Laconic.Demo
{
    public class CounterPage : xf.ContentPage
    {
        static StackLayout Counter(int state) => new StackLayout
        {
            Padding = 50,
            ["lbl"] = new Label
            {
                Text = $"You clicked {state} times",
                FontSize = 30,
                FontAttributes = xf.FontAttributes.Bold,
                VerticalOptions = xf.LayoutOptions.CenterAndExpand,
                HorizontalOptions = xf.LayoutOptions.Center
            },
            ["btn"] = new Button
            {
                Text = "Click Me",
                Clicked = () => new Signal("inc"),
                TextColor = xf.Color.White,
                FontSize = 20,
                BackgroundColor = xf.Color.Coral,
                BorderColor = xf.Color.Chocolate,
                BorderWidth = 3,
                CornerRadius = 10,
                HorizontalOptions = xf.LayoutOptions.Center,
                Padding = new xf.Thickness(30, 0)
            }
        };

        static int Reducer(int state, Signal signal) => signal.Payload switch
        {
            "inc" => state + 1,
            "dec" => throw new NotImplementedException(), // Left as an exercise for for reader
            _ => state
        };

        readonly Binder<int> _binder;

        public CounterPage()
        {
            _binder = Binder.Create(0, Reducer);
            Content = _binder.CreateElement(Counter);

            BackgroundColor = xf.Color.Bisque;
        }
    }
}