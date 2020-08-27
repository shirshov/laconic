using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Laconic
{
    public class ViewList : IDictionary<Key, IElement>
    {
        readonly Dictionary<Key, IElement> _internalStorage = new Dictionary<Key, IElement>();

        public ViewList()
        {
        }

        ViewList(IEnumerable<(Key, View)> source)
        {
            foreach (var (key, view) in source)
                _internalStorage.Add(key, view);
        }

        public IElement this[Key key]
        {
            get => _internalStorage[key];
            set => _internalStorage.Add(key, value);
        }

        public int Count => _internalStorage.Count;

        internal IEnumerable<Key> Keys => _internalStorage.Keys;

        ICollection<Key> IDictionary<Key, IElement>.Keys => _internalStorage.Keys;
        ICollection<IElement> IDictionary<Key, IElement>.Values => throw new NotImplementedException();

        int ICollection<KeyValuePair<Key, IElement>>.Count => _internalStorage.Count;

        bool ICollection<KeyValuePair<Key, IElement>>.IsReadOnly => throw new NotImplementedException();

        public void Add(Key key, IElement value) => _internalStorage.Add(key, value);

        void ICollection<KeyValuePair<Key, IElement>>.Add(KeyValuePair<Key, IElement> item) =>
            throw new NotImplementedException();

        void ICollection<KeyValuePair<Key, IElement>>.Clear() => throw new NotImplementedException();

        bool ICollection<KeyValuePair<Key, IElement>>.Contains(KeyValuePair<Key, IElement> item) =>
            throw new NotImplementedException();

        public bool ContainsKey(Key key) => _internalStorage.ContainsKey(key);

        void ICollection<KeyValuePair<Key, IElement>>.CopyTo(KeyValuePair<Key, IElement>[] array, int arrayIndex) =>
            throw new NotImplementedException();

        IEnumerator<KeyValuePair<Key, IElement>> IEnumerable<KeyValuePair<Key, IElement>>.GetEnumerator() =>
            _internalStorage.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _internalStorage.GetEnumerator();

        bool IDictionary<Key, IElement>.Remove(Key key) => throw new NotImplementedException();

        bool ICollection<KeyValuePair<Key, IElement>>.Remove(KeyValuePair<Key, IElement> item) =>
            throw new NotImplementedException();

        bool IDictionary<Key, IElement>.TryGetValue(Key key, out IElement value) => throw new NotImplementedException();

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