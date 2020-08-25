namespace Laconic.Demo
{
    public class EntryAndEditor : Xamarin.Forms.ContentPage
    {
        static StackLayout EntryWithTextChanged(string state) => new StackLayout {
            BackgroundColor = Color.Bisque,
            Padding = 20,
            Spacing = 20,
            ["entry label"] = new Label {
                Text = "Entry:"
            },
            ["entry"] = new Entry {
                Text = state,
                Placeholder = "Type something",
                TextChanged = e => new Signal(e.NewTextValue)
            },
            ["editor label"] = new Label {
                Text = "Editor:"
            },
            ["editor"] = new Editor {
                Placeholder = "Type something",
                HeightRequest = 100,
                Text = state,
                TextChanged = e => new Signal(e.NewTextValue)
            },
            ["Numbers label"] = new Label {
                Text = "Entry with numeric keyboard:"
            },
            ["numbers entry"] = new Entry {
                Placeholder = "Type something",
                Keyboard = Keyboard.Numeric,
                Text = state,
                TextChanged = e => new Signal(e.NewTextValue)
            }
        };
        
        Binder<string> _binder;
        
        public EntryAndEditor()
        {
            _binder = Binder.Create("", (s, g) => (string)g.Payload);
            Content = _binder.CreateElement(EntryWithTextChanged);
        }
    }
}