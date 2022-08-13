namespace Laconic.Demo;

class GridSignal : Signal
{
    public GridSignal(string type, double value) : base(type, value)
    {
    }
}
    
static class DynamicGrid
{
    public static (int Rows, int Columns) Reducer((int Rows, int Columns) state, GridSignal signal) => signal switch
    {
        ("r", double val) => ((int)Math.Round(val), state.Columns),
        ("c", double val) => (state.Rows, (int)Math.Round(val)),
        _ => throw new NotImplementedException()
    };

    static GridViewList StateToGridChildren((int Rows, int Columns) state)
    {
        GridViewList ret = new();
        
        var dict = from r in Enumerable.Range(0, state.Rows)
            from c in Enumerable.Range(0, state.Columns)
            select (Row: r, Column: c);
        
        foreach (var x in dict) {
            ret.Add($"R{x.Row}C{x.Column}",
                new Label {
                    Text = $"R{x.Row}C{x.Column}",
                    VerticalTextAlignment = TextAlignment.Center,
                    HorizontalTextAlignment = TextAlignment.Center,
                    HeightRequest = 70,
                    BackgroundColor = Color.Chocolate,
                    TextColor = Color.White
                },
                x.Row,
                x.Column
            );
        }

        return ret;
    }
    
    public static VerticalStackLayout Content((int Rows, int Columns) state) => new() {
        BackgroundColor = Color.Bisque,
        Padding = 50,
        ["rowsLabel"] = new Label {Text = "Rows:"},
        ["rowsSlider"] =
            new Slider {Maximum = 10, Minimum = 2, Value = state.Rows, ValueChanged = e => new GridSignal("r", e.NewValue)},
        ["colsLabel"] = new Label {Text = "Columns:"},
        ["colsSlider"] = new Slider {Maximum = 6, Minimum = 2, Value = state.Columns, ValueChanged = e => new GridSignal("c", e.NewValue)},
        ["grid"] = new Grid {
            RowDefinitions = string.Join(',' , new string('*', state.Rows).ToArray()),
            ColumnDefinitions = string.Join(',' , new string('*', state.Columns).ToArray()),
            RowSpacing = 1,
            ColumnSpacing = 1,
            Children = StateToGridChildren(state)
        },
    };
}