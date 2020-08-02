using System.Collections.Generic;
using System.Linq;
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
            Grid? existingGrid, Grid newGrid) => (existingGrid, newGrid) switch
        {
            (_, null) => null,
            (null, _) => new RowDefinitionsChange(newGrid.RowDefinitions.ToArray()),
            var (e, n) when n.RowDefinitions.Equals(e.RowDefinitions) => null,
            (_, _) => new RowDefinitionsChange(newGrid.RowDefinitions.ToArray())
        };

        public static DiffOperation? CalculateColumnDefinitionsDiff(
            Grid? existingGrid, Grid newGrid) => (existingGrid, newGrid) switch
        {
            (_, null) => null,
            (null, _) => new ColumnDefinitionsChange(newGrid.ColumnDefinitions.ToArray()),
            var (e, n) when n.ColumnDefinitions.Equals(e.ColumnDefinitions) => null,
            (_, _) => new ColumnDefinitionsChange(newGrid.ColumnDefinitions.ToArray())
        };

        public static IEnumerable<GridPositionChange> CalculateGridLayoutDiff(Key key, ViewList? existingList,
            ViewList newList)
        {
            var existingGridList = existingList as GridViewList;
            if (!(newList is GridViewList newGridList))
                return new GridPositionChange[0];

            var res = new List<GridPositionChange>();

            var existingPos = (Row: 0, Column: 0, RowSpan: 0, ColumnSpan: 0);
            if (existingGridList != null && existingGridList.ContainsKey(key))
                existingPos = existingGridList.GetPositioning(key);

            var newPos = newGridList.GetPositioning(key);

            if (newPos.Row != existingPos.Row)
                res.Add(new GridPositionChange(GridPositionChangeType.Row, newPos.Row));

            if (newPos.Column != existingPos.Column)
                res.Add(new GridPositionChange(GridPositionChangeType.Column, newPos.Column));

            if (newPos.RowSpan != existingPos.RowSpan)
                res.Add(new GridPositionChange(GridPositionChangeType.RowSpan, newPos.RowSpan));

            if (newPos.ColumnSpan != existingPos.ColumnSpan)
                res.Add(new GridPositionChange(GridPositionChangeType.ColumnSpan, newPos.ColumnSpan));

            return res;
        }
    }
}