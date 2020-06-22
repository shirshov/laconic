using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using xf = Xamarin.Forms;
using Evt = System.Linq.Expressions.Expression<System.Func<Laconic.Signal>>;

namespace Laconic
{
    public class EventInfo : IEquatable<EventInfo>
    {
        readonly string _expressionText;
        public readonly Action<xf.Element, Action<Signal>> Subscribe;

        public EventInfo(string expressionText, Action<xf.Element, Action<Signal>> subscribe)
        {
            _expressionText = expressionText;
            Subscribe = subscribe;
        }

        public override bool Equals(object obj) => Equals((EventInfo) obj);

        public bool Equals(EventInfo other) => _expressionText == other._expressionText;

        public override int GetHashCode()
        {
            unchecked
            {
                return (_expressionText != null ? _expressionText.GetHashCode() : 0) * 397;
            }
        }
    }

    public interface IElement
    {
        Dictionary<xf.BindableProperty, object> ProvidedValues { get; }
        Dictionary<string, EventInfo> Events { get; }
    }

    public abstract class Element : IEquatable<Element>, IElement
    {
        public Dictionary<Xamarin.Forms.BindableProperty, object> ProvidedValues { get; } =
            new Dictionary<Xamarin.Forms.BindableProperty, object>();

        public Dictionary<string, EventInfo> Events { get; } = new Dictionary<string, EventInfo>();

        protected T GetValue<T>(Xamarin.Forms.BindableProperty property) => (T) ProvidedValues[property];

        protected void SetValue(Xamarin.Forms.BindableProperty property, object value) =>
            ProvidedValues[property] = value;

        public string AutomationId
        {
            get => GetValue<string>(Xamarin.Forms.Element.AutomationIdProperty);
            set => SetValue(Xamarin.Forms.Element.AutomationIdProperty, value);
        }

        public string ClassId
        {
            get => GetValue<string>(Xamarin.Forms.Element.ClassIdProperty);
            set => SetValue(Xamarin.Forms.Element.ClassIdProperty, value);
        }

        internal abstract Xamarin.Forms.Element CreateReal();

        public override bool Equals(object other) => other is Element el && Equals(el);

        public bool Equals(Element other)
        {
            if (other.GetType() != GetType())
                return false;
            
            if (ProvidedValues.Count != other.ProvidedValues.Count)
                return false;

            if (Events.Count != other.Events.Count)
                return false;

            foreach (var item in other.ProvidedValues)
            {
                if (!ProvidedValues.TryGetValue(item.Key, out var val))
                    return false;

                if (!val.Equals(item.Value))
                    return false;
            }

            foreach (var evt in other.Events)
            {
                if (!Events.TryGetValue(evt.Key, out var val))
                    return false;

                if (!val.Equals((evt.Value)))
                    return false;
            }

            return true;
        }

        public static bool operator ==(Element lhs, Element rhs) => (lhs, rhs) switch
        {
            (null, null) => true,
            (_, null) => false,
            (null, _) => false,
            (_, _) => lhs.Equals(rhs)
        };

        public static bool operator !=(Element lhs, Element rhs) => !(lhs?.Equals(rhs) ?? false);

        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;
                foreach (var p in ProvidedValues)
                    hash = hash * 23 + p.GetHashCode();
                foreach (var e in Events)
                    hash = hash * 23 + e.GetHashCode();
                return hash;
            }
        }
    }

    public abstract class Element<T> : Element where T : xf.BindableObject, new()
    {
        protected void SetEvent(string eventName, Expression<Func<Signal>> expression,
            Action<T, EventHandler> subscribe,
            Action<T, EventHandler> unsubscribe)
        {
            // Called from patcher
            void SetUpControlEvent(xf.BindableObject real, Action<Signal> send)
            {
                var compiled = expression.Compile();
                subscribe((T) real, (sender, e) =>
                {
                    var signal = compiled();
                    send(signal);
                });
            }

            Events[eventName] = new EventInfo(expression.ToString(), SetUpControlEvent);
        }

        protected void SetEvent<TEventArgs>(string eventName, Expression<Func<TEventArgs, Signal>> expression,
            Action<T, EventHandler<TEventArgs>> subscribe,
            Action<T, EventHandler<TEventArgs>> unsubscribe)
        {
            // Called from patcher
            void SetUpControlEvent(xf.BindableObject real, Action<Signal> send)
            {
                var compiled = expression.Compile();

                subscribe((T) real, (sender, e) =>
                {
                    var signal = compiled(e);
                    send(signal);
                });
            }

            Events[eventName] = new EventInfo(expression.ToString(), SetUpControlEvent);
        }
    }

    public abstract partial class VisualElement<T> : Element<T> where T : xf.VisualElement, new()
    {
        // TODO: why is it here, and not on Element<T>?
        internal override xf.Element CreateReal() => new T();
        public IList<IGestureRecognizer> GestureRecognizers { get; } = new List<IGestureRecognizer>();
    }

    public abstract class View<T> : VisualElement<T>, View where T : xf.View, new()
    {
        public xf.LayoutOptions HorizontalOptions
        {
            get => GetValue<xf.LayoutOptions>(xf.View.HorizontalOptionsProperty);
            set => SetValue(xf.View.HorizontalOptionsProperty, value);
        }

        public xf.LayoutOptions VerticalOptions
        {
            get => GetValue<xf.LayoutOptions>(xf.View.VerticalOptionsProperty);
            set => SetValue(xf.View.VerticalOptionsProperty, value);
        }

        public xf.Thickness Margin
        {
            get => GetValue<xf.Thickness>(xf.View.MarginProperty);
            set => SetValue(xf.View.MarginProperty, value);
        }
    }

    public interface View : IElement
    {
        IList<IGestureRecognizer> GestureRecognizers { get; }
    }
}