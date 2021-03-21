using System;
using System.Collections.Generic;
using System.Linq;
using xf = Xamarin.Forms;

namespace Laconic
{
    interface ILocalContextSignal
    {
        string ContextKey { get; }
    }
    
    class SetLocalStateSignal : Signal, ILocalContextSignal
    {
        readonly string _contextKey;

        internal SetLocalStateSignal(string contextKey) : base(null) => _contextKey = contextKey;

        string ILocalContextSignal.ContextKey => _contextKey;
    }
    
    public class LocalContext
    {
        readonly Action<Signal> _dispatch;

        internal readonly string Key;
        
        readonly Dictionary<string, object> _values = new();

        internal LocalContext(Action<Signal> dispatch, string key)
        {
            _dispatch = dispatch;
            Key = key;
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
        string ContextKey { get; set; }
    }
    
    public class ContextElement<T> : VisualElement<T>, IContextElement, View where T: xf.VisualElement, new()
    {
        readonly Func<LocalContext, Element> _maker;

        public ContextElement(Func<LocalContext, Element> maker, string? key = null)
        {
            _maker = maker;
            ((IContextElement)this).ContextKey = key;  
        }

        Element IContextElement.Make(LocalContext context) => _maker(context);
        
        string IContextElement.ContextKey { get; set; }
    }

    public static partial class LocalContextExtensions
    {
        public static (TState, Func<TState, Signal>) UseLocalState<TState>(this LocalContext context, TState initial) where TState : notnull
        {
            const string storageKey = "laconic.localstate";
            
            if (context.TryGetValue<TState>(storageKey, out var existingState)) {
                return (existingState, state => {
                    context.SetValue(storageKey, state);
                    return new SetLocalStateSignal(context.Key);
                });
            }

            context.SetValue(storageKey, initial);
            return (initial, state => {
                context.SetValue(storageKey, state);
                return new SetLocalStateSignal(context.Key);
            });
        }
    }

    public class TimerSignal : Signal, ILocalContextSignal
    {
        public TimerSignal(string contextId) : base(null) => ContextKey = contextId;

        public string ContextKey { get; }
    } 
        
    public class Timer : IDisposable
    {
        readonly TimeSpan _timer;
        readonly string _contextKey;
        readonly Action<Signal> _callback;
        bool _isRunning;
        DateTimeOffset _startTime;

        internal Timer(TimeSpan timer, string contextKey, Action<Signal> callback, bool start)
        {
            _timer = timer;
            _contextKey = contextKey;
            _callback = callback;
            if (start) {
                _isRunning = true;
                StartTimer();
            }
        }
        
        void StartTimer()
        {
            var signal = new TimerSignal(_contextKey);
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
                _startTime = DateTimeOffset.Now;
                StartTimer();
                return new TimerSignal(_contextKey);
            }

            return null;
        }

        public TimerSignal? Stop()
        {
            if (_isRunning) {
                _isRunning = false;
                _startTime = DateTimeOffset.MinValue;
                return new TimerSignal(_contextKey);
            }

            return null;
        }

        public bool IsRunning => _isRunning;
        public TimeSpan Elapsed => _isRunning ? DateTimeOffset.Now  - _startTime : TimeSpan.Zero;
        
        public void Dispose() => _isRunning = false;
    }

    public static partial class LocalContextExtensions
    {
        public static Timer UseTimer(this LocalContext context, TimeSpan duration, bool start = true)
        {
            const string storageKey = "laconic.timer";
            
            if (context.TryGetValue<Timer>(storageKey, out _)) 
                return context.GetValue<Timer>(storageKey);
            
            var timer = new Timer(duration, context.Key, context.Send, start);
            context.SetValue(storageKey, timer);
            return timer;
        }
    }
}