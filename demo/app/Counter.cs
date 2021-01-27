namespace Laconic.Demo
{
    public static class Counter
    {
        public static StackLayout Content(int state) => new StackLayout
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
    }
}