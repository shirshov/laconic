namespace Laconic.Demo;

static class EntryAndEditor
{
    public static VisualElement<Xamarin.Forms.StackLayout> Content() => Element.WithContext("entry", ctx => {
        var (text, setState) = ctx.UseLocalState("");
        return new StackLayout {
            BackgroundColor = Color.Bisque,
            Padding = 20,
            Spacing = 20,
            ["entry label"] = new Label {Text = "Entry:"},
            ["entry"] =
                new Entry {
                    Text = text, Placeholder = "Type something", TextChanged = e => setState(e.NewTextValue)
                },
            ["editor label"] = new Label {Text = "Editor:"},
            ["editor"] =
                new Editor {
                    Placeholder = "Type something",
                    HeightRequest = 100,
                    Text = text,
                    TextChanged = e => setState(e.NewTextValue)
                },
            ["Numbers label"] = new Label {Text = "Entry with numeric keyboard:"},
            ["numbers entry"] = new Entry {
                Placeholder = "Type something",
                Keyboard = Keyboard.Numeric,
                Text = text,
                TextChanged = e => setState(e.NewTextValue)
            }
        };
    });
}