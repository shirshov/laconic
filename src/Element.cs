using System;
using System.Collections.Generic;
using xf = Xamarin.Forms;

namespace Laconic
{
    // TODO: bad name
    interface IConvert
    {
        object ToNative();
    }

    public abstract class Element : IEquatable<Element>
    {
        public Dictionary<xf.BindableProperty, object?> ProvidedValues { get; } = new();

        // TODO: this should be hidden from the app developer
        public Dictionary<string, EventInfo> Events { get; } = new();

        // TODO: this should be hidden from the app developer
        protected T GetValue<T>(xf.BindableProperty property) => (T) ProvidedValues[property]!;

        protected void SetValue(xf.BindableProperty property, object? value) =>
            ProvidedValues[property] = value;

        protected internal readonly ElementListCollection ElementLists = new();

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
            where T : xf.VisualElement, new() => new(maker);
        
        public static ContextElement<T> WithContext<T>(string contextKey, Func<LocalContext, VisualElement<T>> maker)
            where T : xf.VisualElement, new() => new(maker, contextKey);

        internal string? ContextKey;
        
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
                if (val == null & item.Value == null)
                    return true;
                if (!val!.Equals(item.Value))
                    return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            unchecked {
                return (ProvidedValues.GetHashCode() * 397) ^ Events.GetHashCode();
            }
        }

        public static bool operator ==(Element? lhs, Element? rhs) => (lhs, rhs) switch {
            (null, null) => true,
            (null, _) => false,
            (_, null) => false,
            _ => lhs.Equals(rhs)
        };

        public static bool operator !=(Element? lhs, Element? rhs) => (lhs, rhs) switch {
            (null, null) => false,
            (null, _) => true,
            (_, null) => true,
            _ => !lhs.Equals(rhs)
        };

        virtual internal void UpdateFrom(Element element)
        {
            ProvidedValues.Clear();
            foreach (var p in element.ProvidedValues)
                ProvidedValues.Add(p.Key, p.Value);

            Events.Clear();
            foreach (var e in element.Events)
                Events.Add(e.Key, e.Value);

            if (this is ILayout l) {
                l.Children = ((ILayout)element).Children;
            }

            if (this is IContentHost ch) {
                ch.Content = ((IContentHost) element).Content;
            }
        }

        public override string ToString()
        {
            var res = GetType().Name + "{";
            foreach (var p in ProvidedValues)
                res += p.Key.PropertyName + "=" + p.Value + ",";
            res += "}";
            return res;
        }
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

        public bool Equals(PostProcessInfo? other) => (other?.Value, Value) switch {
            (null, null) => true,
            (null, _) => false,
            (_, null) => false,
            var (v1, v2) => v1.Equals(v2)
        };

        public override bool Equals(object? other) => Equals(other as PostProcessInfo);

        public override int GetHashCode() => Value?.GetHashCode() ?? base.GetHashCode();

        public static bool operator ==(PostProcessInfo? left, PostProcessInfo? right) => Equals(left, right);

        public static bool operator !=(PostProcessInfo? left, PostProcessInfo? right) => !Equals(left, right);
    }
    
    public abstract class Element<T> : Element where T : xf.BindableObject
    {
        protected void SetValue(string name, object? value, Action<T> process) 
			// TODO: this doesn't allow more than one postprocessed property 
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
                    _ => signalMaker(), 
                    (ctl, handler) => subscribe((T) ctl, handler),
                    (ctl, handler) => unsubscribe((T)ctl, handler)
                );
            }
            else if (Events.ContainsKey(eventName)) {
                Events.Remove(eventName);
            }
        }

    }
}