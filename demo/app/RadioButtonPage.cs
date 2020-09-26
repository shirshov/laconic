using Laconic.Shapes;

namespace Laconic.Demo
{
    public class RadioButtonPage : Xamarin.Forms.ContentPage
    {
        public RadioButtonPage()
        {
            static RadioButton CalendarButton(string text, string iconGlyph, bool isChecked) => new RadioButton {
                CheckedChanged = e => new Signal("cal", text),
                Content = new Frame {
                    BorderColor = isChecked ? "FF3300" : "F3F2F1",
                    BackgroundColor="F3F2F1",
                    HasShadow = false,
                    HeightRequest = 100,
                    WidthRequest = 100,
                    HorizontalOptions = LayoutOptions.Start,
                    VerticalOptions = LayoutOptions.Start,
                    Padding = 0,
                    Content = new Grid {
                        Margin = 4,
                        WidthRequest = 100,
                        ["indicator"] = new Grid {
                            WidthRequest = 18,
                            HeightRequest = 18,
                            HorizontalOptions = LayoutOptions.End,
                            VerticalOptions = LayoutOptions.Start,
                            ["outer"] = new Ellipse {
                                Stroke = Brush.Blue,
                                WidthRequest = 16,
                                HeightRequest = 16,
                                StrokeThickness = 0.5,
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.Center,
                                Fill = Brush.White,
                            },
                            ["inner"] = new Ellipse {
                                WidthRequest = 8,
                                HeightRequest = 8,
                                Opacity = isChecked ? 1 : 0,
                                Fill = Brush.Blue,
                                VerticalOptions = LayoutOptions.Center,
                                HorizontalOptions = LayoutOptions.Center
                            }
                        },
                        ["img-lbl"]  = new StackLayout {
                            HorizontalOptions = LayoutOptions.Center,
                            VerticalOptions = LayoutOptions.Center,
                            ["img"] = new Image {
                                Source = new FontImageSource {
                                    Glyph = iconGlyph,
                                    FontFamily = "IconFont",
                                    Color = "323130",
                                    Size = 32
                                }
                            },
                            ["lbl"] = new Label { 
                                Text = text, 
                                TextColor= "323130" 
                            }
                        },
                    }
                }
            };
            
            var binder = Binder.Create((Color: "Red", Calendar: "Day"), (s, g) => g switch {
                ("clr", string clr) => (clr, s.Calendar),
                ("cal", string cal) => (s.Color, cal),
                _ => s
            });
            
            Content = binder.CreateElement(s => new StackLayout {
                Padding = 20,
                [0] = new RadioButton { Content = "Red", CheckedChanged = e => e.Value ? new Signal("clr", "Red") : null },
                [1] = new RadioButton { Content = "Green", CheckedChanged = e => e.Value ? new Signal("clr", "Green") : null },
                [2] = new RadioButton { Content = "Blue", CheckedChanged = e => e.Value ? new Signal("clr", "Blue") : null },
                ["cal"] = new StackLayout {
                    Orientation = StackOrientation.Horizontal,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    ["day"] = CalendarButton("Day", "\uf783", s.Calendar == "Day"),
                    ["week"] = CalendarButton("Week", "\uf784", s.Calendar == "Week"),
                    ["month"] = CalendarButton("Month", "\uf133", s.Calendar == "Month")
                },
                ["sep"] = new BoxView { HeightRequest = 60 },
                ["disp"] = new Label { Text = $"You have chosen: {s}"}
            });
        }
    }
}