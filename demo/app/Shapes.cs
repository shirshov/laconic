using Laconic.Shapes;
using xf = Xamarin.Forms;

namespace Laconic.Demo
{
    public class Shapes : xf.ContentPage
    {
        Binder<string> _binder;

        protected override void OnAppearing()
        {
            _binder = Binder.Create("", (s, g) => s);

            Content = _binder.CreateView(s => new ScrollView {
                Content = new StackLayout {
                    ["lineLbl"] = new Label {Text = "Line"},
                    ["line"] = new Line {
                        X1 = 40,
                        Y1 = 0,
                        X2 = 0,
                        Y2 = 120,
                        Stroke = xf.Color.Red
                    },
                    ["rect"] = new Rectangle {
                        Fill = xf.Color.Blue,
                        Stroke = xf.Color.Black,
                        StrokeThickness = 3,
                        RadiusX = 50,
                        RadiusY = 10,
                        WidthRequest = 200,
                        HeightRequest = 100,
                        HorizontalOptions = xf.LayoutOptions.Start
                    },
                    ["path"] = new Path {
                        Data = "M 10,100 C 100,0 200,200 300,100", Stroke = xf.Color.Black, Aspect = xf.Stretch.Uniform
                    }
                }
            });
        }
    }
}