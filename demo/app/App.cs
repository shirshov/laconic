using System;
using ChanceNET;
using Xamarin.Forms.Internals;
using xf = Xamarin.Forms;

[assembly: xf.ExportFont("DIN Condensed Bold.ttf", Alias = "DINBold")]
[assembly: xf.ExportFont("Font Awesome 5 Free-Solid-900.otf", Alias = "IconFont")]

namespace Laconic.Demo
{
    public class App : xf.Application
    {
        record State(
            int CurrentItem, 
            bool IsFlyoutPresented, 
            (string Title, Func<State, View> Maker)[] Items,
            int Counter,
            (int Rows, int Columns) Grid,
            Calculator.State Calculator,
            Person[] Persons);

        static State MainReducer(State state, Signal signal) => signal switch {
            ("IsPresentedChanged", _) => state with {IsFlyoutPresented = !state.IsFlyoutPresented},
            ("ShowItem", int index) => state with {CurrentItem = index, IsFlyoutPresented = false},
            ("inc", _)  => state with {Counter = state.Counter + 1},
            GridSignal g => state with {Grid = DynamicGrid.Reducer(state.Grid, g)},
            Calculator.CalculatorSignal g => state with { Calculator = Calculator.MainReducer(state.Calculator, g)},
            _ => state
        };
        
        static Label MenuItem(string title, int index, bool isSelected) => new Label {
            Text = title,
            TextColor = Color.White,
            FontAttributes = isSelected ? FontAttributes.Bold : FontAttributes.None,
            HeightRequest = 40,
            GestureRecognizers = {
                ["tap"] = new TapGestureRecognizer { Tapped = () => new("ShowItem", index) }
            }
        };

        static ContentPage Flyout(State state) => new(){
            BackgroundColor = Color.Chocolate,
            IconImageSource = new FontImageSource {
                Glyph ="\uf0c9",
                FontFamily = "IconFont"
            },
            Title = "Laconic Demo",
            Content = new ScrollView {
                Content =new StackLayout {
                Padding =  (10, 100, 10, 10),
                Children = state.Items.ToViewList(
                    x => x.Title, 
                    x => MenuItem(x.Title,  state.Items.IndexOf(x), state.Items.IndexOf(x) == state.CurrentItem)
                )}
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
                   ("Timer", _ => (View)Timer.Content())
                },
                0, // Counter
                (2, 2), // Grid
                new Calculator.Initial(), 
                GroupedCollectionView.Initial()
            );

            var binder = Binder.Create(initialState, MainReducer);

            MainPage = binder.CreateElement(state => new FlyoutPage {
                Flyout = Flyout(state),
                IsPresented = state.IsFlyoutPresented,
                IsPresentedChanged = () => new ("IsPresentedChanged"),
                Detail = MakeDemoPage(state)
            });
        }
    }
}




// Records won't work without this 
namespace System.Runtime.CompilerServices
{
    sealed class IsExternalInit
    {
    }
}