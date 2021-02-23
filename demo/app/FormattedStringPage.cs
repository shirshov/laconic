namespace Laconic.Demo
{
    static class FormattedStringPage
    {
        public static Label Content() => new() {
            Margin = (30, 30),
            VerticalOptions = LayoutOptions.Center,
            FormattedText = new FormattedString {
                new Span {Text = "As ", FontFamily = "AmericanTypewriter" }, // the font is iOS only
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
    }
}