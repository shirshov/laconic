// Laconic version of TipCalc example from https://www.mvvmcross.com/documentation/tutorials/tipcalc/the-tip-calc-tutorial?scroll=1892

using System;
using Laconic;

namespace Laconic.Template
{
    public class App : Xamarin.Forms.Application
    {
        // Everything the app displays and manipulates is kept in an immutable state:
        record State
        {
            public double SubTotal { get; init; }
            public double Generosity { get; init; }
            public double Tip => SubTotal * Generosity / 100.0;
        }

        // ... and this is a function to modify the state:
        // given the current state and a signal, calculate and return the new state:
        static State Reducer(State state, Signal signal) => signal switch {
            // Signal class provides Deconstruct method with two out params; usual usage is (ID, Payload)
            ("subtotal", string newVal) => state with {SubTotal = Double.Parse(newVal)},
            ("generosity", double newVal) => state with {Generosity = newVal},
            _ => state
        };

        // ... given the current state, describe how the entire app's UI must look:
        static ContentPage UI(State state) => new() {
            Content = new StackLayout {
                Margin = 50,
                // ...for adding child views use Directory initialization syntax:
                ["lbl-sub"] = new Label {Text = "Subtotal"},
                ["subtotal"] = new Entry {
                    Text = state.SubTotal.ToString(),
                    Keyboard = Keyboard.Numeric,
                    // ... instead of event subscriptions write lambdas that return an instance of Signal.
                    // When the user provides input Laconic will call those lambdas:
                    TextChanged = e => new( /* ID: */"subtotal", /* Payload: */ e.NewTextValue)
                },
                ["lbl-gen"] = new Label {Text = "Generosity"},
                ["slider"] = new Slider {
                    Maximum = 100,
                    Value = state.Generosity,
                    ValueChanged = e => new("generosity", e.NewValue)
                },
                ["lbl-tip-cap"] = new Label {Text = "Tip to leave"},
                ["lbl-tip"] = new Label {Text = state.Tip.ToString()}
            }
        };

        Binder<State> _binder;

        public App()
        {
            // ... finally, bind everything together:
            _binder = Binder.Create(new State {SubTotal = 100, Generosity = 10}, Reducer);
            MainPage = _binder.CreateElement(UI);
        }
    }
}


// Temporary stub: records won't work without this 
namespace System.Runtime.CompilerServices
{
    sealed class IsExternalInit
    {
    }
}