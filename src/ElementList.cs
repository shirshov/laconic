using System;
using System.Collections;
using System.Collections.Generic;
using xf = Xamarin.Forms;

namespace Laconic
{
    
    class ElementListInfo
    {
        public readonly ElementList List;
        public readonly Func<xf.BindableObject, IList> ListGetter;
        
        public ElementListInfo(ElementList list, Func<xf.BindableObject, IList> listGetter)
        {
            List = list;
            ListGetter = listGetter;
        }
    }
    public class ElementList : Dictionary<Key, Element>
    {
        public static implicit operator ElementList(Dictionary<int, Element> source)
        {
            var res = new ElementList();
            foreach (var p in source)
                res.Add(p.Key, p.Value);

            return res;
        }
        
        public static implicit operator ElementList(Dictionary<string, Element> source)
        {
            var res = new ElementList();
            foreach (var p in source)
                res.Add(p.Key, p.Value);

            return res;
        }
    }
    
    
    public class ElementListCollection : IDictionary<string, ElementList>
    {
        internal readonly Dictionary<string, ElementListInfo>  Inner = new Dictionary<string, ElementListInfo>();

        public void Add<TListOwner>(string listName, Func<TListOwner, IList> listGetter)
            where TListOwner : xf.BindableObject =>
            Inner.Add(listName, new ElementListInfo(new ElementList(), el => listGetter((TListOwner)el)));

        IEnumerator<KeyValuePair<string, ElementList>> IEnumerable<KeyValuePair<string, ElementList>>.GetEnumerator() => throw new NotImplementedException();

        IEnumerator IEnumerable.GetEnumerator() => (this as IDictionary<string, ElementList>).GetEnumerator();

        void ICollection<KeyValuePair<string, ElementList>>.Add(KeyValuePair<string, ElementList> item) 
            => throw new InvalidOperationException("Use Add(string, ElementList) method instead");

        void ICollection<KeyValuePair<string, ElementList>>.Clear() => throw new NotImplementedException();

        bool ICollection<KeyValuePair<string, ElementList>>.Contains(KeyValuePair<string, ElementList> item) => throw new NotImplementedException();

        void ICollection<KeyValuePair<string, ElementList>>.CopyTo(KeyValuePair<string, ElementList>[] array, int arrayIndex) => throw new NotImplementedException();

        bool ICollection<KeyValuePair<string, ElementList>>.Remove(KeyValuePair<string, ElementList> item) => throw new NotImplementedException();

        int ICollection<KeyValuePair<string, ElementList>>.Count => Inner.Count;
        
        public bool IsReadOnly => false;

        void IDictionary<string, ElementList>.Add(string key, ElementList value) => throw new NotImplementedException();

        bool IDictionary<string, ElementList>.ContainsKey(string key) => Inner.ContainsKey(key);

        bool IDictionary<string, ElementList>.Remove(string key) => throw new NotImplementedException();

        bool IDictionary<string, ElementList>.TryGetValue(string key, out ElementList value) => throw new NotImplementedException();

        public ElementList this[string key] {
            get => Inner[key].List;
            set => Inner[key] = new ElementListInfo(value, Inner[key].ListGetter);
        }

        ICollection<string> IDictionary<string, ElementList>.Keys { get; }
        ICollection<ElementList> IDictionary<string, ElementList>.Values { get; }
    }

}