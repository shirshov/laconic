using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Laconic
{
    public class ViewList : IDictionary<Key, View?>
    {
        readonly Dictionary<Key, View?> _internalStorage = new Dictionary<Key, View?>();

        public ViewList()
        {
        }

        ViewList(IEnumerable<(Key, View)> source)
        {
            foreach (var (key, view) in source)
                _internalStorage.Add(key, view);
        }

        public View? this[Key key]
        {
            get => _internalStorage[key];
            set => _internalStorage.Add(key, value);
        }

        public int Count => _internalStorage.Count;

        internal IEnumerable<Key> Keys => _internalStorage.Keys;

        ICollection<Key> IDictionary<Key, View?>.Keys => _internalStorage.Keys;
        ICollection<View?> IDictionary<Key, View?>.Values => throw new NotSupportedException();

        int ICollection<KeyValuePair<Key, View?>>.Count => _internalStorage.Count;

        bool ICollection<KeyValuePair<Key, View?>>.IsReadOnly => throw new NotSupportedException();

        public void Add(Key key, View? value) => _internalStorage.Add(key, value);

        void ICollection<KeyValuePair<Key, View?>>.Add(KeyValuePair<Key, View?> item) =>
            throw new NotSupportedException();

        void ICollection<KeyValuePair<Key, View?>>.Clear() => throw new NotSupportedException();

        bool ICollection<KeyValuePair<Key, View?>>.Contains(KeyValuePair<Key, View?> item) =>
            throw new NotSupportedException();

        public bool ContainsKey(Key key) => _internalStorage.ContainsKey(key);

        void ICollection<KeyValuePair<Key, View?>>.CopyTo(KeyValuePair<Key, View?>[] array, int arrayIndex) =>
            throw new NotSupportedException();

        IEnumerator<KeyValuePair<Key, View?>> IEnumerable<KeyValuePair<Key, View?>>.GetEnumerator() =>
            _internalStorage.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _internalStorage.GetEnumerator();

        bool IDictionary<Key, View?>.Remove(Key key) => throw new NotSupportedException();

        bool ICollection<KeyValuePair<Key, View?>>.Remove(KeyValuePair<Key, View?> item) =>
            throw new NotSupportedException();

        bool IDictionary<Key, View?>.TryGetValue(Key key, out View value) => throw new NotSupportedException();

        public static implicit operator ViewList(Dictionary<Key, View> source) =>
            new ViewList(source.Select(x => (x.Key, x.Value)));

        public static implicit operator ViewList(Dictionary<string, View> source) =>
            new ViewList(source.Select(x => ((Key) x.Key, x.Value)));

        public static implicit operator ViewList(Dictionary<int, View> source) =>
            new ViewList(source.Select(x => ((Key) x.Key, x.Value)));

        public static implicit operator ViewList(Dictionary<long, View> source) =>
            new ViewList(source.Select(x => ((Key) x.Key, x.Value)));
    }
}