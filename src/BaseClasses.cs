using System;
using System.Collections.Generic;
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

    public interface IElement
    {
        Dictionary<xf.BindableProperty, object> ProvidedValues { get; }
        Dictionary<string, EventInfo> Events { get; }
    }

    public abstract class Element : IEquatable<Element>, IElement
    {
        public Dictionary<xf.BindableProperty, object> ProvidedValues { get; } =
            new Dictionary<xf.BindableProperty, object>();

        public Dictionary<string, EventInfo> Events { get; } = new Dictionary<string, EventInfo>();

        protected T GetValue<T>(xf.BindableProperty property) => (T) ProvidedValues[property];

        protected void SetValue(xf.BindableProperty property, object value) =>
            ProvidedValues[property] = value;

        public string AutomationId {
            get => GetValue<string>(xf.Element.AutomationIdProperty);
            set => SetValue(xf.Element.AutomationIdProperty, value);
        }

        public string ClassId {
            get => GetValue<string>(xf.Element.ClassIdProperty);
            set => SetValue(xf.Element.ClassIdProperty, value);
        }

        internal abstract xf.BindableObject CreateReal();
        
        public static ContextElement<T> WithContext<T>(Func<LocalContext, VisualElement<T>> maker)
            where T : xf.VisualElement, new() => new ContextElement<T>(maker);

        public override bool Equals(object other) => other is Element el && Equals(el);

        public bool Equals(Element other)
        {
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
        
        public static bool operator ==(Element lhs, Element rhs) => lhs.Equals(rhs);
        public static bool operator !=(Element lhs, Element rhs) => !lhs.Equals(rhs);
    }
    
    public abstract class Element<T> : Element where T : xf.BindableObject
    {
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

        protected void SetEvent<TEventArgs>(string eventName, Func<TEventArgs, Signal> signalMaker,
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
    }

    public abstract partial class VisualElement<T> : Element<T> where T : xf.VisualElement, new()
    {
        // TODO: why is it here, and not on Element<T>?
        internal override xf.BindableObject CreateReal() => new T();
        
        public Dictionary<Key, IGestureRecognizer> GestureRecognizers { get; } = new Dictionary<Key, IGestureRecognizer>();
    }

    public abstract class View<T> : VisualElement<T>, View where T : xf.View, new()
    {
        public xf.LayoutOptions HorizontalOptions {
            get => GetValue<xf.LayoutOptions>(xf.View.HorizontalOptionsProperty);
            set => SetValue(xf.View.HorizontalOptionsProperty, value);
        }

        public xf.LayoutOptions VerticalOptions {
            get => GetValue<xf.LayoutOptions>(xf.View.VerticalOptionsProperty);
            set => SetValue(xf.View.VerticalOptionsProperty, value);
        }

        public xf.Thickness Margin {
            get => GetValue<xf.Thickness>(xf.View.MarginProperty);
            set => SetValue(xf.View.MarginProperty, value);
        }
    }

    public interface View : IElement
    {
        Dictionary<Key, IGestureRecognizer> GestureRecognizers { get; }
    }
}		