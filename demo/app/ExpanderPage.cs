namespace Laconic.Demo
{
    public class ExpanderPage : Xamarin.Forms.ContentPage
    {
        public ExpanderPage()
        {
            var binder = Binder.Create(0, (s, g) => s);
            Content = binder.CreateElement(s => new StackLayout {
                Padding =(50, 50),
                ["e"] = new Expander {
                    Header = new Label {Text = "Baboon", FontAttributes = FontAttributes.Bold, FontSize = 18},
                    Content = new Grid {
                        Padding = 10,
                        ColumnDefinitions = "Auto, Auto",
                        ["img"] = new Image {
                            Source = "http://upload.wikimedia.org/wikipedia/commons/thumb/f/fc/Papio_anubis_%28Serengeti%2C_2009%29.jpg/200px-Papio_anubis_%28Serengeti%2C_2009%29.jpg",
                            Aspect = Aspect.AspectFill,
                            HeightRequest = 120,
                            WidthRequest = 120 
                        },
                        ["txt", 0, 1] = new Label {
                            Text = "Baboons are African and Arabian Old World monkeys belonging to the genus Papio, part of the subfamily Cercopithecinae.",
                            FontAttributes = FontAttributes.Italic
                        }
                    },
                }
            });
        }
    }
}