using Laconic.Shapes;

namespace Laconic.Demo;

static class RadioButtonPage
{
    record State(string Color, string Calendar);

    static RadioButton CalendarButton(string text, string iconGlyph, bool isChecked, Func<Signal> setState) => new() {
        CheckedChanged = e => setState(),
        Content = /*new Frame {
            BorderColor = isChecked ? "FF3300" : "F3F2F1",
            BackgroundColor = "F3F2F1",
            HasShadow = false,
            HeightRequest = 100,
            WidthRequest = 100,
            HorizontalOptions = LayoutOptions.Start,
            VerticalOptions = LayoutOptions.Start,
            Padding = 0,
            Content =*/ new Grid {
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
                ["img-lbl"] = new VerticalStackLayout {
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    ["img"] = new Image {
                        Source = new FontImageSource {
                            Glyph = iconGlyph, FontFamily = "IconFont", Color = "323130", Size = 32
                        }
                    },
                    ["lbl"] = new Label {Text = text, TextColor = "323130"}
                },
            }
        //}
    };

    public static VisualElement<xf.VerticalStackLayout> Content() => Element.WithContext("radio", ctx => {
        var (state, setState) = ctx.UseLocalState(new State("", ""));
            
        return new VerticalStackLayout {
            Padding = 20,
            [0] =
                new RadioButton {
                    Content = "Red", CheckedChanged = e => e.Value ? setState(state with {Color = "Red"}) : null
                },
            [1] =
                new RadioButton {
                    Content = "Green", CheckedChanged = e => e.Value ? setState(state with {Color = "Green"}) : null
                },
            [2] =
                new RadioButton {
                    Content = "Blue", CheckedChanged = e => e.Value ? setState(state with {Color = "Blue"}) : null
                },
            ["cal"] = new HorizontalStackLayout {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                ["day"] = CalendarButton("Day", "\uf783", state.Calendar == "Day", () => setState(state with { Calendar =  "Date" })),
                ["week"] = CalendarButton("Week", "\uf784", state.Calendar == "Week", () => setState(state with { Calendar =  "Week" })),
                ["month"] = CalendarButton("Month", "\uf133", state.Calendar == "Month", () => setState(state with { Calendar =  "Month" }))
            },
            ["sep"] = new BoxView {HeightRequest = 60},
            ["disp"] = new Label {Text = $"You have chosen: Color={state.Color}, Calendar={state.Calendar}"}
        };
    });
}