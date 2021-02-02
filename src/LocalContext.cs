using System;
using System.Collections.Generic;
using System.Linq;
using xf = Xamarin.Forms;

namespace Laconic
{
    interface ILocalContextSignal
    {
        Guid ContextId { get; }
    }
    
    class SetLocalStateSignal : Signal, ILocalContextSignal
    {
        readonly Guid _contextId;

        internal SetLocalStateSignal(Guid contextId) : base(null) => _contextId = contextId;

        Guid ILocalContextSignal.ContextId => _contextId;
    }
    
    public class LocalContext
    {
        readonly Action<Signal> _dispatch;

        internal readonly Guid Id;
        readonly Dictionary<string, object> _values = new();

        internal LocalContext(Action<Signal> dispatch)
        {
            _dispatch = dispatch;
            Id = Guid.NewGuid();
        }

        public T GetValue<T>(string key) => (T)_values[key];

        public bool TryGetValue<T>(string key, out T value)
        {
            var ret = _values.TryGetValue(key, out var val);
            value = ret ? (T) val : default;
            return ret;
        }
        
        public void SetValue<T>(string key, T value) => _values[key] = value!;
        
        
        public void Send(Signal signal) => _dispatch(signal);

        internal void Clear()
        {
            foreach (var v in _values.Values.OfType<IDisposable>())
                v.Dispose();
            
            _values.Clear();
        }
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

    public static partial class LocalContextExtensions
    {
        public static (TState, Func<TState, Signal>) UseLocalState<TState>(this LocalContext context, TState initial) where TState : notnull
        {
            const string storageKey = "laconic.localstate";
            
            if (context.TryGetValue<TState>(storageKey, out var existingState)) {
                return (existingState, state => {
                    context.SetValue(storageKey, state);
                    return new SetLocalStateSignal(context.Id);
                });
            }

            context.SetValue(storageKey, initial);
            return (initial, state => {
                context.SetValue(storageKey, state);
                return new SetLocalStateSignal(context.Id);
            });
        }
    }

    public class TimerSignal : Signal, ILocalContextSignal
        {
            public TimerSignal(Guid contextId) : base(null) => ContextId = contextId;

            public Guid ContextId { get; }
        } 
        
        public class Timer : IDisposable
        {
            readonly TimeSpan _timer;
            readonly Guid _contextId;
            readonly Action<Signal> _callback;
            bool _isRunning;

            internal Timer(TimeSpan timer, Guid contextId, Action<Signal> callback, bool start)
            {
                _timer = timer;
                _contextId = contextId;
                _callback = callback;
                if (start) {
                    _isRunning = true;
                    StartTimer();
                }
            }

            void StartTimer()
            {
                var signal = new TimerSignal(_contextId);
                Xamarin.Forms.Device.StartTimer(_timer, () => {
                    if (_isRunning)
                        _callback(signal);
                    return _isRunning;
                });
            }
            
            public TimerSignal? Start()
            {
                if (!_isRunning) {
                    _isRunning = true;
                    StartTimer();
                    return new TimerSignal(_contextId);
                }

                return null;
            }

            public TimerSignal? Stop()
            {
                if (_isRunning) {
                    _isRunning = false;
                    return new TimerSignal(_contextId);
                }

                return null;
            }

            public bool IsRunning => _isRunning;
            
            public void Dispose()
            {
                 _isRunning = false;
                 System.Diagnostics.Debug.WriteLine("LACONIC: disposing Timer");
            }

        }

    public static partial class LocalContextExtensions
    {
        public static Timer UseTimer(this LocalContext context, TimeSpan duration, bool start = true)
        {
            const string storageKey = "laconic.timer";
            
            if (context.TryGetValue<Timer>(storageKey, out _)) 
                return context.GetValue<Timer>(storageKey);
            
            var timer = new Timer(duration, context.Id, context.Send, start);
            context.SetValue(storageKey, timer);
            return timer;
        }
    }
}