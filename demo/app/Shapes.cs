using Laconic.Shapes;

namespace Laconic.Demo
{
    public class Shapes : Xamarin.Forms.ContentPage
    {
        Binder<string> _binder;

        protected override void OnAppearing()
        {
            _binder = Binder.Create("", (s, g) => s);

            Content = _binder.CreateElement(s => new ScrollView {
                Content = new StackLayout {
                    ["lineLbl"] = new Label {Text = "Line"},
                    ["line"] = new Line {
                        X1 = 40,
                        Y1 = 0,
                        X2 = 0,
                        Y2 = 120,
                        Stroke = Color.Red
                    },
                    ["rect"] = new Rectangle {
                        Fill = Color.Blue,
                        Stroke = Color.Black,
                        StrokeThickness = 3,
                        RadiusX = 50,
                        RadiusY = 10,
                        WidthRequest = 200,
                        HeightRequest = 100,
                        HorizontalOptions = LayoutOptions.Start
                    },
                    ["path"] = new Path {
                        Data = "M 10,100 C 100,0 200,200 300,100", 
                        Stroke = Color.Black, 
                        Aspect = Stretch.Uniform
                    }
                }
            });
        }
    }
}