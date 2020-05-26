using System;
using System.Collections.Generic;
using System.Linq;

namespace Laconic
{
    public static class ToViewListExtensions
    {
        public static Dictionary<Key, View> ToViewList<T>(this IEnumerable<T> source,
            Func<T, Key> keySelector, Func<T, View> itemSelector) =>
            source.ToDictionary(keySelector, itemSelector);

        public static Dictionary<(Key, int Row, int Column), View> ToGridViewList<T>(this IEnumerable<T> source,
            Func<T, (Key Key, int Row, int Column)> keySelector, Func<T, View> itemSelector) =>
            source.ToDictionary(keySelector, itemSelector);

        public static ItemsViewList ToItemsList<T>(this IEnumerable<T> source,
            Func<T, string> reuseKeySelector, Func<T, Key> keySelector, Func<T, View> viewSelector)
        {
            var res = new ItemsViewList();
            foreach (var item in source)
            {
                var key = keySelector(item);
                res.Add(key, viewSelector(item));
                res.ReuseKeys[key] = reuseKeySelector(item);
            }

            return res;
        }
    }
}