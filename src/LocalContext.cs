using System;
using System.Collections.Generic;
using xf = Xamarin.Forms;

namespace Laconic
{
    interface ILocalContextSignal
    {
        Guid Id { get; }
        object? Payload { get; }
    }
    class SetLocalStateSignal<T> : Signal<T>, ILocalContextSignal
    {
        readonly Guid _id;

        internal SetLocalStateSignal(Guid id, T payload) : base(payload) => _id = id;

        Guid ILocalContextSignal.Id => _id;

        object? ILocalContextSignal.Payload => Payload;
    }
    
    public class LocalContext
    {
        readonly Action<Signal> _dispatch;
        internal const string LOCAL_STATE_KEY = "laconic.localstate";
        
        internal readonly Guid Id;
        readonly Dictionary<string, object> _values = new Dictionary<string, object>();

        internal LocalContext(Action<Signal> dispatch)
        {
            _dispatch = dispatch;
            Id = Guid.NewGuid();
        }

        internal T GetValue<T>(string key) => (T)_values[key];

        internal void SetValue<T>(string key, T value) => _values[key] = value!;
        
        public (TState, Func<TState, Signal>) UseLocalState<TState>(TState initial) where TState : notnull
        {
            if (_values.TryGetValue(LOCAL_STATE_KEY, out var existingState)) {
                return ((TState)existingState, state => new SetLocalStateSignal<TState>(Id, state));
            }

            _values[LOCAL_STATE_KEY] = initial;
            return (initial, state => new SetLocalStateSignal<TState>(Id, state));
        }

        public void Send(Signal signal) => _dispatch(signal);
    }

    interface IContextElement
    {
        Element Make(LocalContext context);
        Guid ContextId { get; set; }
    }
    
    public class ContextElement<T> : VisualElement<T>, IContextElement, View where T: xf.VisualElement, new()
    {
        readonly Func<LocalContext, Element> _maker;

        public ContextElement(Func<LocalContext, Element> maker) => _maker = maker;

        Element IContextElement.Make(LocalContext context) => _maker(context);
        
        Guid IContextElement.ContextId { get; set; }
    }
}