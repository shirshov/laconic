namespace Laconic.Demo;

static class Counter
{
    public static VerticalStackLayout Content(int state) => new() {
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
            BorderWidth = 2,
            CornerRadius = 20,
            HorizontalOptions = LayoutOptions.Center,
            Padding = (30, 0)
        }
    };
}