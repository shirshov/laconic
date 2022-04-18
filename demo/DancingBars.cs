namespace Laconic.Demo;

// TODO:implement sliders for changing number of rows and columns
static class DancingBars
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

    static GridViewList StateToGridChildren(BarInfo[] bars)
    {
        GridViewList ret = new();
        
        foreach (var x in bars.Select((b, i) => (Bar: b, Column: i)))
        {
            ret.Add(
                x.Column, 
                new BoxView {
                   BackgroundColor = Color.FromHsla(x.Bar.Hue, 1, 0.5),
                   HeightRequest = 10 + 100 * x.Bar.Height,
                   VerticalOptions = LayoutOptions.End
               }, 
               column: x.Column);
        }

        return ret;
    }
    
    static Grid Row(BarInfo[] bars) => new() {
        HeightRequest = 120,
        Padding = 0,
        Margin = 20,
        ColumnSpacing = 1,
        ColumnDefinitions = string.Join("," , new string('*', bars.Length).ToArray()),
        Children = StateToGridChildren(bars)
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

    // These two are instantiated as static to avoid unnecessary allocations
    static Random _random = new(23451234);
    static State _initialState = CreateInitialState(100, 10, _random.NextDouble, _random.NextDouble);
        
    public static VisualElement<xf.CollectionView> Content() => Element.WithContext("bars", ctx => {
        ctx.UseTimer(TimeSpan.FromMilliseconds(30));
        var (state, setState) = ctx.UseLocalState(_initialState);
        var newState = RandomizeState(state, _random.Next, _random.NextDouble, _random.NextDouble);
        setState(newState);
        return new CollectionView {Items = CreateRows(state.Bars)};
    });
}