using System.Collections.Generic;

namespace Laconic
{
    public class GridViewList : ViewList
    {
        internal (int Row, int Column, int RowSpan, int ColumnSpan) GetPositioning(Key key) => _positioning[key];

        internal void SetPositioning(Key key, int row, int column, int rowSpan, int columnSpan) =>
            _positioning[key] = (row, column, rowSpan, columnSpan);

        readonly Dictionary<Key, (int Row, int Column, int RowSpan, int ColumnSpan)> _positioning = new();

        public void Add(Key key, View? blueprint, int row = 0, int column = 0, int rowSpan = 1, int columnSpan = 1)
        {
           base.Add(key, blueprint);
           SetPositioning(key, row, column, rowSpan, columnSpan);
        }
        
        public static implicit operator GridViewList(Dictionary<(Key Key, int Row, int Column), View?> source)
        {
            var res = new GridViewList();
            foreach (var item in source)
            {
                res.Add(item.Key.Key, item.Value);
                res.SetPositioning(item.Key.Key, item.Key.Row, item.Key.Column, 0, 0);
            }

            return res;
        }
    }

    public class ItemsViewList : ViewList
    {
        internal readonly Dictionary<Key, string> ReuseKeys = new();

        public View? this[string reuseKey, Key key]
        {
            init
            {
                base[key] = value;
                ReuseKeys[key] = reuseKey;
            }
        }

        public void Add(string reuseKey, Key key, View? view)
        {
            Add(key, view);
            ReuseKeys[key] = reuseKey;
        }
    }
}