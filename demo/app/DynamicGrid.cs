using System;
using System.Linq;
using static Laconic.Signal;
using xf = Xamarin.Forms;

namespace Laconic.Demo
{
    public class DynamicGrid : xf.ContentPage
    {
        static (int Rows, int Columns) Reducer((int Rows, int Columns) state, Signal signal) => signal switch
        {
            ("r", double val) => ((int) Math.Round(val), state.Columns),
            ("c", double val) => (state.Rows, (int) Math.Round(val)),
            _ => throw new NotImplementedException()
        };

        static StackLayout GridConstructionKit((int Rows, int Columns) state) => new StackLayout
        {
            BackgroundColor = xf.Color.Bisque,
            Padding = 50,
            ["rowsLabel"] = new Label {Text = "Rows:"},
            ["rowsSlider"] =
                new Slider {Maximum = 10, Minimum = 2, Value = state.Rows, ValueChanged = e => Send("r", e.NewValue)},
            ["colsLabel"] = new Label {Text = "Columns:"},
            ["colsSlider"] = new Slider
            {
                Maximum = 6, Minimum = 2, Value = state.Columns, ValueChanged = e => Send("c", e.NewValue)
            },
            ["grid"] = new Grid
            {
                Children = (from r in Enumerable.Range(0, state.Rows)
                        from c in Enumerable.Range(0, state.Columns)
                        select (Row: r, Column: c))
                    .ToGridViewList(x => (x.Row * state.Columns + x.Column, x.Row, x.Column),
                        x => new Label
                        {
                            Text = $"R{x.Row}C{x.Column}",
                            VerticalTextAlignment = xf.TextAlignment.Center,
                            HorizontalTextAlignment = xf.TextAlignment.Center,
                            HeightRequest = 70,
                            BackgroundColor = xf.Color.Chocolate,
                            TextColor = xf.Color.White
                        })
            },
        };

        Binder<(int Rows, int Columns)> _binder;

        public DynamicGrid()
        {
            _binder = Binder.Create((2, 2), Reducer);
            Content = _binder.CreateView(GridConstructionKit);

            BackgroundColor = xf.Color.Bisque;
        }
    }
}