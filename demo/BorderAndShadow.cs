namespace Laconic.Demo;

static class BorderAndShadow
{
    public static VerticalStackLayout Content() => new() {
        Padding = (30, 50),
        ["border"] = new Border {
            Stroke = Color.FromArgb("#C49B33"),
            Background = Color.FromArgb("#2B0B98"),
            StrokeThickness = 4,
            Padding = (16, 8),
            HorizontalOptions = LayoutOptions.Center,
            StrokeShape = new Laconic.Shapes.RoundRectangle {
                CornerRadius = (40, 0, 0, 40)
            },
            Content = new Label {
                Text = "Laconic",
                TextColor = Color.White,
                FontSize = 18,
                FontAttributes = FontAttributes.Bold
            }
        },
        ["shadow"] = new Image {
            Source = "dotnet_bot.png",
            WidthRequest = 250,
            HeightRequest = 310,
            Shadow = new Shadow() {
                Brush = Color.Black,
                Offset = (20, 20),
                Radius = 40,
                Opacity = 0.8f
            }
        }
    };
}