using Laconic.Shapes;

namespace Laconic.Demo
{
    public class LoginShape : Xamarin.Forms.ContentPage
    {
        protected override void OnAppearing()
        {
            BackgroundColor = Xamarin.Forms.Color.FromHex("#222222");

            var binder = Binder.Create("", (s, g) => s);

            Content = binder.CreateElement(s => new Grid {
                ColumnDefinitions = "*,279,*",
                RowDefinitions = "*,350,*",
                ["photo", column: 1] =
                    new Image {
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.End,
                        WidthRequest = 150,
                        HeightRequest = 150,
                        Margin = (0, 0, 0, 8),
                        Source =
                            "https://devblogs.microsoft.com/xamarin/wp-content/uploads/sites/44/2019/03/Screen-Shot-2017-01-03-at-3.35.53-PM-150x150.png",
                        Clip = new EllipseGeometry {Center = new Xamarin.Forms.Point(75, 75), RadiusX = 75, RadiusY = 75}
                    },
                ["controls", row: 1, column: 1] = new Grid {
                    RowDefinitions = "34,40,16,44,20,44,*",
                    ColumnDefinitions = "22,*,*,22",
                    ["outline", rowSpan: 7, columnSpan: 4] = new Path {
                        HorizontalOptions = LayoutOptions.Fill,
                        VerticalOptions = LayoutOptions.Fill,
                        Fill = Brush.White,
                        Data =
                            "M251,0 C266.463973,-2.84068575e-15 279,12.536027 279,28 L279,276 C279,291.463973 266.463973,304 251,304 L214.607,304 L214.629319,304.009394 L202.570739,304.356889 C196.091582,304.5436 190.154631,308.020457 186.821897,313.579883 L186.821897,313.579883 L183.402481,319.283905 C177.100406,337.175023 160.04792,350 140,350 C119.890172,350 102.794306,337.095694 96.5412691,319.115947 L96.5273695,319.126964 L92.8752676,313.28194 C89.5084023,307.893423 83.6708508,304.544546 77.3197008,304.358047 L65.133,304 L28,304 C12.536027,304 1.8937905e-15,291.463973 0,276 L0,28 C-1.8937905e-15,12.536027 12.536027,2.84068575e-15 28,0 L251,0 Z",
                    },
                    ["title", row: 1, column: 1] =
                        new Label {
                            Text = "LOGIN",
                            TextColor = "#222222",
                            FontFamily = "DINBold",
                            FontSize = 22,
                            HorizontalTextAlignment = TextAlignment.Center,
                        },
                    ["reg", row: 1, column: 2] =
                        new Label {
                            Text = "REGISTER",
                            TextColor = "#222222",
                            FontFamily = "DINBold",
                            FontSize = 22,
                            HorizontalTextAlignment = TextAlignment.Center,
                            Opacity = 0.2,
                        },
                    ["usernameLabel", row: 2, column: 1, columnSpan: 2] =
                        new Label {Text = "Username", TextColor = "#222222", FontSize = 16},
                    ["username", row: 3, column: 1, columnSpan: 2] =
                        new Entry {
                            Placeholder = "Enter username",
                            Text = "daortin@microsoft.com",
                            TextColor = "#222222",
                            BackgroundColor = "#D8D8D8"
                        },
                    ["passwordLabel", row: 4, column: 1, columnSpan: 2] =
                        new Label {
                            Margin = (0, 4, 0, 0),
                            Text = "Password",
                            TextColor = "#222222",
                            FontSize = 16
                        },
                    ["password", row: 5, column: 1, columnSpan: 2] =
                        new Entry {
                            Placeholder = "Enter password",
                            TextColor = "#222222",
                            BackgroundColor = "#D8D8D8"
                        },
                    ["foo", row: 6, column: 1, columnSpan: 2] = new Grid {
                        ColumnDefinitions = "64",
                        RowDefinitions = "64",
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.End,
                        Margin = (0, 0, 0, 13),
                        ["ellipse"] =
                            new Ellipse {Fill = new SolidColorBrush("#222222"), WidthRequest = 64, HeightRequest = 64},
                        ["arrow"] = new Path {
                            Data =
                                "M15.6099294,11.0552456 L12.3765961,7.82357897 C12.2574176,7.70409826 12.0779382,7.66830385 11.9220434,7.73292537 C11.7661485,7.7975469 11.6646275,7.94982156 11.6649294,8.11857897 L11.6649294,21.2502456 C11.6649294,22.4008389 10.7321893,23.333579 9.58159609,23.333579 C8.43100286,23.333579 7.49826276,22.4008389 7.49826276,21.2502456 L7.49826276,8.11857897 C7.49789351,7.95055217 7.39663523,7.79918973 7.24146862,7.73471909 C7.08630201,7.67024846 6.90759527,7.70528741 6.78826276,7.82357897 L3.55492943,11.0552456 C2.74169013,11.8684849 1.42316875,11.8684849 0.609929471,11.0552456 C-0.203309806,10.2420063 -0.203309826,8.92348493 0.609929427,8.11024563 L8.10992943,0.610245632 C8.50036143,0.219527336 9.03007272,0 9.58242943,0 C10.1347861,0 10.6644974,0.219527336 11.0549294,0.610245632 L18.5549294,8.11024563 C19.3681687,8.92348493 19.3681687,10.2420063 18.5549294,11.0552456 C17.7416901,11.8684849 16.4231687,11.8684849 15.6099294,11.0552456 L15.6099294,11.0552456 Z",
                            Fill = Brush.White,
                            VerticalOptions = LayoutOptions.Center,
                            HorizontalOptions = LayoutOptions.Center,
                            Rotation = 90,
                        }
                    }
                }
            });
        }
    }
}