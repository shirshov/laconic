namespace Laconic.Demo
{
    public static class Brushes
    {
        public static StackLayout Content() => new StackLayout {
            Padding = 50,
            ["solid"] = new Frame {
                Background = Brush.DarkBlue,
                BorderColor = Color.LightGray,
                HasShadow = true,
                CornerRadius = 12,
                HeightRequest = 120,
                WidthRequest = 120
            },
            ["linear"] = new Frame {
                BorderColor = Color.LightGray,
                HasShadow = true,
                CornerRadius = 12,
                HeightRequest = 120, 
                WidthRequest = 120,
                Background = new LinearGradientBrush {
                    StartPoint = (0, 0), 
                    EndPoint = (1, 0),
                    GradientStops = {
                        [0] = new GradientStop(Color.Yellow, 0.1f),
                        [1] = new GradientStop(Color.Green, 1.0f),
                    }
                }
            },
            ["radial"] = new Frame {
                BorderColor = Color.LightGray,
                HasShadow = true,
                CornerRadius = 12,
                HeightRequest = 120, 
                WidthRequest = 120,
                Background = new RadialGradientBrush {
                    Center = (0.5, 0.5),
                    Radius = (0.5),
                    GradientStops = {
                        [0] = new GradientStop(Color.Red, 0.1f),
                        [1] = new GradientStop(Color.DarkBlue, 1.0f),
                    }
                }
            }
        };
    }
}