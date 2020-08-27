using System.Collections.Generic;
using xf = Xamarin.Forms;

namespace Laconic
{
    enum GridPositionChangeType
    {
        Row,
        Column,
        RowSpan,
        ColumnSpan
    }

    static class GridDiff
    {
        public static DiffOperation? CalculateRowDefinitionsDiff(
            Grid? existingGrid, Grid newGrid) => (existingGrid, newGrid) switch {
            (_, null) => null,
            (null, _) => new RowDefinitionsChange(newGrid.RowDefinitions.ToArray()),
            var (e, n) when n.RowDefinitions.Equals(e.RowDefinitions) => null,
            (_, _) => new RowDefinitionsChange(newGrid.RowDefinitions.ToArray())
        };

        public static DiffOperation? CalculateColumnDefinitionsDiff(
            Grid? existingGrid, Grid newGrid) => (existingGrid, newGrid) switch {
            (_, null) => null,
            (null, _) => new ColumnDefinitionsChange(newGrid.ColumnDefinitions.ToArray()),
            var (e, n) when n.ColumnDefinitions.Equals(e.ColumnDefinitions) => null,
            (_, _) => new ColumnDefinitionsChange(newGrid.ColumnDefinitions.ToArray())
        };

        public static IEnumerable<GridPositionChange> CalculatePositioningInGrid(Key key, GridViewList? existingList,
            GridViewList newList)
        {
            var existingPos = (Row: 0, Column: 0, RowSpan: 0, ColumnSpan: 0);
            if (existingList != null && existingList.ContainsKey(key))
                existingPos = existingList.GetPositioning(key);

            var newPos = newList.GetPositioning(key);

            if (newPos.Row != existingPos.Row)
                yield return new GridPositionChange(GridPositionChangeType.Row, newPos.Row);

            if (newPos.Column != existingPos.Column)
                yield return new GridPositionChange(GridPositionChangeType.Column, newPos.Column);

            if (newPos.RowSpan != existingPos.RowSpan)
                yield return new GridPositionChange(GridPositionChangeType.RowSpan, newPos.RowSpan);

            if (newPos.ColumnSpan != existingPos.ColumnSpan)
                yield return new GridPositionChange(GridPositionChangeType.ColumnSpan, newPos.ColumnSpan);
        }
    }
}