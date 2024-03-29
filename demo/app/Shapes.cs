using Laconic.Shapes;

namespace Laconic.Demo;

static class Shapes
{
    public static ScrollView Content() => new() {
        Padding = 20,
        Content = new StackLayout {
            ["line"] = new Line {
                X1 = 40,
                Y1 = 0,
                X2 = 0,
                Y2 = 120,
                Stroke = Brush.Red,
                StrokeThickness = 5
            },
            ["rect"] = new Rectangle {
                Fill = Brush.Blue,
                Stroke = Brush.Black,
                StrokeThickness = 3,
                RadiusX = 50,
                RadiusY = 10,
                WidthRequest = 200,
                HeightRequest = 100,
                HorizontalOptions = LayoutOptions.Start
            },
            ["path"] = new Path {
                Data = "M 10,100 C 100,0 200,200 300,100", 
                Stroke = Brush.Black, 
                StrokeThickness = 3,
                Aspect = Stretch.Uniform
            }
        }
    };
}