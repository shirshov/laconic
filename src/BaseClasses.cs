using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using xf = Xamarin.Forms;

namespace Laconic
{
    public class EventInfo
    {
        public readonly Func<EventArgs, Signal> SignalMaker;
        public readonly Action<xf.BindableObject, EventHandler> Subscribe;
        public readonly Action<xf.BindableObject, EventHandler> Unsubscribe;

        public EventInfo(Func<EventArgs, Signal> signalMaker, 
            Action<xf.BindableObject, EventHandler> subscribe,
            Action<xf.BindableObject, EventHandler> unsubscribe)
        {
            SignalMaker = signalMaker;
            Subscribe = subscribe;
            Unsubscribe = unsubscribe;
        }
    }

    // TODO: is this interface necessary?
    public interface IElement
    {
        Dictionary<xf.BindableProperty, object> ProvidedValues { get; }
        Dictionary<string, EventInfo> Events { get; }
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
    }

    class ElementListInfo
    {
        public readonly ElementList List;
        public readonly Func<xf.Element, IList> ListGetter;
        
        public ElementListInfo(ElementList list, Func<xf.Element, IList> listGetter)
        {
            List = list;
            ListGetter = listGetter;
        }
    }
    public class ElementListCollection : IDictionary<string, ElementList>
    {
        internal readonly Dictionary<string, ElementListInfo>  Inner = new Dictionary<string, ElementListInfo>();

        public void Add<TListOwner>(string listName, Func<TListOwner, IList> listGetter)
            where TListOwner : xf.Element =>
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

    public abstract class Element : IEquatable<Element>, IElement
    {
        public Dictionary<xf.BindableProperty, object?> ProvidedValues { get; } =
            new Dictionary<xf.BindableProperty, object>();

        // TODO: this should be hidden from the app developer
        public Dictionary<string, EventInfo> Events { get; } = new Dictionary<string, EventInfo>();

        // TODO: this should be hidden from the app developer
        protected T GetValue<T>(xf.BindableProperty property) => (T) ProvidedValues[property];

        protected void SetValue(xf.BindableProperty property, object? value) =>
            ProvidedValues[property] = value;

        protected internal readonly ElementListCollection ElementLists = new ElementListCollection();

        public string AutomationId {
            get => GetValue<string>(xf.Element.AutomationIdProperty);
            set => SetValue(xf.Element.AutomationIdProperty, value);
        }

        public string ClassId {
            get => GetValue<string>(xf.Element.ClassIdProperty);
            set => SetValue(xf.Element.ClassIdProperty, value);
        }

        protected internal abstract xf.BindableObject CreateView();
        
        public static ContextElement<T> WithContext<T>(Func<LocalContext, VisualElement<T>> maker)
            where T : xf.VisualElement, new() => new ContextElement<T>(maker);

        public override bool Equals(object other) => other is Element el && Equals(el);

        public bool Equals(Element? other)
        {
            if (ReferenceEquals(null, other))
                return false;
            
            if (other.GetType() != GetType())
                return false;

            if (ProvidedValues.Count != other.ProvidedValues.Count)
                return false;

            if (Events.Count != other.Events.Count)
                return false;

            foreach (var item in other.ProvidedValues) {
                if (!ProvidedValues.TryGetValue(item.Key, out var val))
                    return false;

                if (!val.Equals(item.Value))
                    return false;
            }
            return true;
        }
        
        public override int GetHashCode()
        {
            unchecked
            {
                return (ProvidedValues.GetHashCode() * 397) ^ Events.GetHashCode();
            }
        }
        
        public static bool operator ==(Element? lhs, Element? rhs) => lhs?.Equals(rhs) ?? false;
        public static bool operator !=(Element? lhs, Element? rhs) => !lhs?.Equals(rhs) ?? false;
    }
    
    class PostProcessInfo : IEquatable<PostProcessInfo>
    {
        public static readonly xf.BindableProperty PostProcessedProperty =
            xf.BindableProperty.Create("Laconic.Postprocess", typeof(xf.BindableObject), typeof(PostProcessInfo));
            
        public readonly object? Value;
        public readonly Action<xf.BindableObject> Process;

        public PostProcessInfo(object? value, Action<xf.BindableObject> process)
        {
            Value = value;
            Process = process;
        }

        public bool Equals(PostProcessInfo? other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (Value == null && other.Value != null) return false;
            if (Value == null && other.Value == null) return true;
            return Value.Equals(other.Value);
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PostProcessInfo) obj);
        }

        public override int GetHashCode() => Value.GetHashCode();

        public static bool operator ==(PostProcessInfo? left, PostProcessInfo? right) => Equals(left, right);

        public static bool operator !=(PostProcessInfo? left, PostProcessInfo? right) => !Equals(left, right);
    }
    
    public abstract class Element<T> : Element where T : xf.BindableObject
    {
        protected void SetValue(string name, object value, Action<T> process) 
            => ProvidedValues[PostProcessInfo.PostProcessedProperty] = new PostProcessInfo(value, el => process((T)el));

        protected void SetEvent<TEventArgs>(string eventName, Func<TEventArgs, Signal>? signalMaker,
            Action<T, EventHandler<TEventArgs>> subscribe,
            Action<T, EventHandler<TEventArgs>> unsubscribe) where TEventArgs : EventArgs
        {
            if (signalMaker != null) {
                Events[eventName] = new EventInfo(
                    e => signalMaker((TEventArgs)e),
                    (ctl, handler) => subscribe((T)ctl, (s, e) => handler(s, e)),
                    (ctl, handler) => unsubscribe((T)ctl, (s, e) => handler(s, e))
                );
            }
            else if (Events.ContainsKey(eventName)) {
                Events.Remove(eventName);
            }
        }

        protected void SetEvent(
            string eventName, 
            Func<Signal>? signalMaker,
            Action<T, EventHandler> subscribe,
            Action<T, EventHandler> unsubscribe)
        {
            if (signalMaker != null) {
                Events[eventName] = new EventInfo(
                    e => signalMaker(), 
                    (ctl, handler) => subscribe((T) ctl, handler),
                    (ctl, handler) => unsubscribe((T)ctl, handler)
                );
            }
            else if (Events.ContainsKey(eventName)) {
                Events.Remove(eventName);
            }
        }

    }

    public abstract partial class VisualElement<T> : Element<T> where T : xf.VisualElement, new()
    {
        // TODO: why is it here, and not on Element<T>?
        protected internal override xf.BindableObject CreateView() => new T();
        
        public Dictionary<Key, IGestureRecognizer> GestureRecognizers { get; } = new Dictionary<Key, IGestureRecognizer>();
        
        public VisualMarker Visual
        {
            get => GetValue<VisualMarker>(xf.VisualElement.VisualProperty);
            set => SetValue(xf.VisualElement.VisualProperty, value);
        }
    }

    public abstract class View<T> : VisualElement<T>, View where T : xf.View, new()
    {
        public LayoutOptions HorizontalOptions {
            get => GetValue<LayoutOptions>(xf.View.HorizontalOptionsProperty);
            set => SetValue(xf.View.HorizontalOptionsProperty, value);
        }

        public LayoutOptions VerticalOptions {
            get => GetValue<LayoutOptions>(xf.View.VerticalOptionsProperty);
            set => SetValue(xf.View.VerticalOptionsProperty, value);
        }

        public Thickness Margin {
            get => GetValue<Thickness>(xf.View.MarginProperty);
            set => SetValue(xf.View.MarginProperty, value);
        }
    }
    
    public interface View : IElement
    {
        Dictionary<Key, IGestureRecognizer> GestureRecognizers { get; }
    }

    // TODO: bad name
    interface IConvert
    {
        object ToNative();
    }
}		