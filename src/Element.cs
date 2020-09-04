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
        public Dictionary<Xamarin.Forms.BindableProperty, object?> ProvidedValues { get; } =
            new Dictionary<Xamarin.Forms.BindableProperty, object>();

        // TODO: this should be hidden from the app developer
        public Dictionary<string, EventInfo> Events { get; } = new Dictionary<string, EventInfo>();

        // TODO: this should be hidden from the app developer
        protected T GetValue<T>(Xamarin.Forms.BindableProperty property) => (T) ProvidedValues[property];

        protected void SetValue(Xamarin.Forms.BindableProperty property, object? value) =>
            ProvidedValues[property] = value;

        protected internal readonly ElementListCollection ElementLists = new ElementListCollection();

        public string AutomationId {
            get => GetValue<string>(Xamarin.Forms.Element.AutomationIdProperty);
            set => SetValue(Xamarin.Forms.Element.AutomationIdProperty, value);
        }

        public string ClassId {
            get => GetValue<string>(Xamarin.Forms.Element.ClassIdProperty);
            set => SetValue(Xamarin.Forms.Element.ClassIdProperty, value);
        }

        protected internal abstract Xamarin.Forms.BindableObject CreateView();
        
        public static ContextElement<T> WithContext<T>(Func<LocalContext, VisualElement<T>> maker)
            where T : Xamarin.Forms.VisualElement, new() => new ContextElement<T>(maker);

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
}