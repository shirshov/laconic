using System;

namespace Laconic.Demo
{
    public class CounterPage : Xamarin.Forms.ContentPage
    {
        static StackLayout Counter(int state) => new StackLayout
        {
            Padding = 50,
            ["lbl"] = new Label
            {
                Text = $"You clicked {state} times",
                FontSize = 30,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Center
            },
            ["btn"] = new Button
            {
                Text = "Click Me",
                Clicked = () => new Signal("inc"),
                TextColor = Color.White,
                FontSize = 20,
                BackgroundColor = Color.Coral,
                BorderColor = Color.Chocolate,
                BorderWidth = 3,
                CornerRadius = 10,
                HorizontalOptions = LayoutOptions.Center,
                Padding = (30, 0)
            }
        };

        static int Reducer(int state, Signal signal) => signal.Payload switch
        {
            "inc" => state + 1,
            "dec" => throw new NotImplementedException(), // Left as an exercise for the reader
            _ => state
        };

        readonly Binder<int> _binder;

        public CounterPage()
        {
            _binder = Binder.Create(0, Reducer);
            Content = _binder.CreateElement(Counter);

            BackgroundColor = Xamarin.Forms.Color.Bisque;
        }
    }
}