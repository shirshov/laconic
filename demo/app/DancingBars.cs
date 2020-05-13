using System;
using System.Collections.Generic;
using System.Linq;
using xf = Xamarin.Forms;

namespace Laconic.Demo
{
    // TODO:implement sliders for changing number of rows and columns
    class DancingBars : xf.ContentPage
    {
        class BarInfo
        {
            public readonly double Hue;
            public readonly double Height;

            public BarInfo(double hue, double height)
            {
                Hue = hue;
                Height = height;
            }
        }

        Binder<State> _binder;

        class State
        {
            public readonly int RowCount;
            public readonly int ColumnCount;
            public readonly BarInfo[,] Bars;

            public State(int rowCount, int columnCount, BarInfo[,] bars)
            {
                RowCount = rowCount;
                ColumnCount = columnCount;
                Bars = bars;
            }
        }

        static State CreateInitialState(int rows, int columns, Func<double> randomHue, Func<double> randomHeight)
        {
            var bars = new BarInfo[rows, columns];
            for (var row = 0; row < rows; row++)
            for (var col = 0; col < columns; col++)
                bars[row, col] = new BarInfo(randomHue(), randomHeight());

            return new State(rows, columns, bars);
        }

        static State RandomizeState(State state, Func<int, int> randomIndex, Func<double> randomHue,
            Func<double> randomHeight)
        {
            for (var i = 0; i < NumberOfBarsChangingAtOnce; i++)
            {
                state.Bars[randomIndex(state.RowCount), randomIndex(state.ColumnCount)] =
                    new BarInfo(randomHue(), randomHeight());
            }

            return state;
        }

        static Grid Row(BarInfo[] bars) => new Grid
        {
            HeightRequest = 120,
            Padding = 0,
            Margin = 20,
            ColumnSpacing = 1,
            Children = bars
                .Select((b, i) => (Bar: b, Column: i))
                .ToViewList(
                    x => (x.Column, 0, x.Column),
                    x => new BoxView
                    {
                        BackgroundColor = xf.Color.FromHsla(x.Bar.Hue, 1, 0.5),
                        HeightRequest = 10 + 100 * x.Bar.Height,
                        VerticalOptions = xf.LayoutOptions.End
                    })
        };

        static ItemsViewList CreateRows(BarInfo[,] bars)
        {
            var list = new ItemsViewList();
            for (var row = 0; row < bars.GetLength(0); row++)
            {
                var rowItems = new List<BarInfo>();
                for (var col = 0; col < bars.GetLength(1); col++)
                    rowItems.Add(bars[row, col]);
                list.Add("row", row, Row(rowItems.ToArray()));
            }

            return list;
        }

        const int NumberOfBarsChangingAtOnce = 50;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var randomIndex = new Random(23451234);
            var randomHue = new Random(09812346);
            var randomHeight = new Random(2453808);

            var initialState = CreateInitialState(100, 10, randomHue.NextDouble, randomHeight.NextDouble);

            _binder = Binder.Create(initialState,
                (s, g) =>
                {
                    var newState = g.Payload switch
                    {
                        "rand" => RandomizeState(s, randomIndex.Next, randomHue.NextDouble, randomHeight.NextDouble)
                    };
                    return newState;
                });

            Content = _binder.CreateView(state => new CollectionView {Items = CreateRows(state.Bars)});

            xf.Device.StartTimer(TimeSpan.FromMilliseconds(30), () =>
            {
                _binder.Dispatch(new Signal("rand"));
                return true;
            });
        }
    }
}