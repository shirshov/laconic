namespace Laconic.Demo;

static class SwipeViewPage
{
    public static VisualElement<xf.VerticalStackLayout> Content() => Element.WithContext(ctx => {
        var (text, setText) = ctx.UseLocalState("");
            
        return new VerticalStackLayout {
            [0] = new SwipeView {
                HeightRequest = 50,
                VerticalOptions = LayoutOptions.Center,
                LeftItems = {
                    ["fav"] = new SwipeItem {
                        Text = "Favourite",
                        IconImageSource = new FontImageSource {Size = 15, FontFamily = "IconFont", Glyph = "\uf02e"},
                        BackgroundColor = Color.LightGreen,
                        Invoked = _ => setText("Favourite")
                    },
                    ["del"] = new SwipeItem {
                        Text = "Delete",
                        IconImageSource = new FontImageSource {Size = 15, FontFamily = "IconFont", Glyph = "\uf2ed"},
                        BackgroundColor = Color.LightPink,
                        Invoked = _ => setText("Delete")
                    }
                },
                Content = new Grid {
                    HeightRequest = 60,
                    WidthRequest = 300,
                    [0] = new Label {
                        Text = "Swipe right",
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    }
                }
            },
            ["lbl"] = text == "" ? null : new Label {
                Text = $"Invoked: {text}",
                HorizontalOptions = LayoutOptions.Center,
                Margin = (0, 50)
            }
        };
    });
}