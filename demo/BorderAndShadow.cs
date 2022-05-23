using Laconic.Shapes;

namespace Laconic.Demo;

static class BorderAndShadow
{
    public static VerticalStackLayout Content() => new() {
        Padding = (30, 50),
        ["border"] = new Border {
            StrokeThickness = 4,
            Background = Color.FromArgb("#2B0B98"),
            Padding = (16, 8),
            HorizontalOptions = LayoutOptions.Center,
            StrokeShape = new RoundRectangle {
                CornerRadius = (40, 0, 0, 40)
            },
            Stroke = new LinearGradientBrush
            {
                EndPoint = new Point(0, 1),
                GradientStops = {
                    ["orange"] = new GradientStop { Color = Color.Orange, Offset = 0.1f },
                    ["brown"] = new GradientStop { Color = Color.Brown, Offset = 1.0f }
                },
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
            Shadow = new() {
                Brush = Color.Black,
                Offset = (20, 20),
                Radius = 40,
                Opacity = 0.8f
            }
        }
    };
}