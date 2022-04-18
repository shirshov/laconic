using ChanceNET;

namespace Laconic.Demo;

public class App : Microsoft.Maui.Controls.Application
{
    record State(
        int CurrentItem,
        bool IsFlyoutPresented,
        (string Title, Func<State, View> Maker)[] Items,
        int Counter,
        (int Rows, int Columns) Grid,
        Calculator.State Calculator,
        Person[] Persons, 
        Navigation.State Navigation);

    static State MainReducer(State state, Signal signal) => signal switch {
        ("IsPresentedChanged", _) => state with {IsFlyoutPresented = !state.IsFlyoutPresented},
        ("ShowItem", int index) => state with {CurrentItem = index, IsFlyoutPresented = false},
        ("inc", _) => state with {Counter = state.Counter + 1},
        GridSignal g => state with {Grid = DynamicGrid.Reducer(state.Grid, g)},
        Calculator.CalculatorSignal g => state with {Calculator = Calculator.MainReducer(state.Calculator, g)},
        _ => state with { Navigation = Navigation.Reducer(state.Navigation, signal) }
    };

    static Label MenuItem(string title, int index, bool isSelected) => new() {
        Text = title,
        TextColor = Color.White,
        FontAttributes = isSelected ? FontAttributes.Bold : FontAttributes.None,
        HeightRequest = 40,
        GestureRecognizers = {["tap"] = new TapGestureRecognizer {Tapped = () => new Signal("ShowItem", index)}}
    };

    static ContentPage Flyout(State state) => new() {
        BackgroundColor = Color.Chocolate,
        IconImageSource = new FontImageSource {Glyph = "\uf0c9", FontFamily = "IconFont", Color = Color.Chocolate},
        Title = "Laconic Demo",
        Content = new ScrollView {
            Content = new StackLayout {
                Padding = (10, 100, 10, 10),
                Children = state.Items.ToViewList(
                    x => x.Title,
                    x => MenuItem(x.Title, Array.IndexOf(state.Items, x), Array.IndexOf(state.Items, x) == state.CurrentItem)
                )
            }
        }
    };

    static ContentPage MakeDemoPage(State state) => new() {
        Title = state.Items[state.CurrentItem].Title,
        Content = state.Items[state.CurrentItem].Maker(state)
    };

    public App()
    {
        var initialState = new State(
            0, 
            false, 
            new (string, Func<State, View>)[] {
                ("Counter", s => Counter.Content(s.Counter)),
                ("Dynamic Grid", s => DynamicGrid.Content(s.Grid)),
                ("Calculator (Grid)", s => Calculator.Content(s.Calculator)),
                ("Collection View", s => GroupedCollectionView.Content(s.Persons)),
                ("Entry and Editor",  _ => (View)EntryAndEditor.Content()),
                ("Dancing Bars (Performance)", _ => (View)DancingBars.Content()),
                ("AbsoluteLayout", _ => AbsoluteLayoutPage.Content()),
                ("FormattedString", _ => FormattedStringPage.Content()),
                ("Shapes", _ => Shapes.Content()),
                ("Shapes - Login Page", _ => LoginShape.Content()),
                ("Brushes", _ => Brushes.Content()),
                ("SwipeView", _ => (View)SwipeViewPage.Content()),
                ("RadioButton", _ => (View)RadioButtonPage.Content()),
                ("WebView", _ => WebViewPage.Content()),
                ("Timer", _ => (View)Timer.Content()),
                ("Behavior", _ => BehaviorPage.Content()),
                ("Navigation", _ => null) // Handled below
            },
            0, // Counter
            (2, 2), // Grid
            new Calculator.Initial(),
            GroupedCollectionView.InitialState(),
            Navigation.InitialState()
        );

        var binder = Binder.Create(initialState, MainReducer);

        MainPage = binder.CreateElement(state => new FlyoutPage {
            Flyout = Flyout(state),
            IsPresented = state.IsFlyoutPresented,
            IsPresentedChanged = () => new Signal("IsPresentedChanged"),
            Detail = state.CurrentItem == state.Items.Length - 1
                ? Navigation.TabbedPage(state.Navigation)
                : new NavigationPage(new NavigationStack("root"), _ => MakeDemoPage(state))
        });
    }
}
