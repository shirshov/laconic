namespace Laconic.Demo;

static class Counter
{
    public static Grid Content(int state) => new() {
        Padding = 50,
        RowDefinitions = "*, Auto",
        ["lbl"] = new Label
        {
            Text = $"You clicked {state} time(s)",
            FontSize = 30,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center
        },
        ["btn", row: 1] = new Button
        {
            Text = "Click Me",
            Clicked = () => new Signal("inc"),
            TextColor = Color.White,
            FontSize = 20,
            BackgroundColor = Color.Coral,
            BorderColor = Color.Chocolate,
            BorderWidth = 2,
            CornerRadius = 20,
            HeightRequest = 40,
            HorizontalOptions = LayoutOptions.Center,
            Padding = (30, 0)
        }
    };
}