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
            Clicked = () => new("inc"),
            TextColor = Color.White,
            FontSize = 20,
            BackgroundColor = Color.Coral,
            BorderColor = Color.Chocolate,
            HeightRequest = 40,
            BorderWidth = 2,
            CornerRadius = 20,
            HorizontalOptions = LayoutOptions.Center
        }
    };
}