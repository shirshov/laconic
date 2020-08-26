namespace Laconic.Demo
{
    public class FormattedStringPage : Xamarin.Forms.ContentPage
    {
        static Label Note(int _) => new Label {
            Margin = (30, 30),
            VerticalOptions = LayoutOptions.Center,
            FormattedText = new FormattedString {
                new Span {Text = "As ", FontFamily = "AmericanTypewriter" },
                new Span {Text = "you ", FontSize = 30},
                new Span {Text = "value ", FontAttributes = FontAttributes.Bold | FontAttributes.Italic},
                new Span {Text = "your life or ",BackgroundColor = Color.Yellow },
                new Span {Text = "your ", CharacterSpacing = 10},
                new Span {Text = "reason ", LineHeight = 15},
                new Span {Text = "keep away ", TextColor = Color.Blue},
                new Span {Text = "from the ", TextDecorations = TextDecorations.Underline, },
                "moor" // plain text is allowed
            }
        };

        public FormattedStringPage()
        {
            var binder = Binder.Create(0, (s, g) => s);

            Content = binder.CreateElement(Note);
        }
    }
}