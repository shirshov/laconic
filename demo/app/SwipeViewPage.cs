namespace Laconic.Demo
{
    public class SwipeViewPage : Xamarin.Forms.ContentPage
    {
        public SwipeViewPage()
        {
            var binder = Binder.Create("", (s, g) => g.Payload.ToString());
            Content = binder.CreateElement(s => new StackLayout {
                [0] = new SwipeView {
                    HeightRequest = 50,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    LeftItems = {
                        ["fav"] = new SwipeItem {
                            Text = "Favourite",
                            IconImageSource = new FontImageSource { Size = 15, FontFamily = "IconFont", Glyph = "\uf02e" },
                            BackgroundColor = Color.LightGreen,
                            Invoked = _ => new Signal("Favourite")
                        },
                        ["del"] = new SwipeItem {
                            Text = "Delete",
                            IconImageSource = new FontImageSource { Size = 15, FontFamily = "IconFont", Glyph = "\uf2ed" },
                            BackgroundColor = Color.LightPink,
                            Invoked = _ => new Signal("Delete")
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
                ["lbl"] = s == "" ? null : new Label {
                    Text = $"Invoked: {s}",
                    HorizontalOptions = LayoutOptions.Center,
                }
            });
        }
    }
}