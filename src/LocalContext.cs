using System.Threading;
using System.Threading.Tasks;

namespace Laconic;

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
    string? ContextKey { get; set; }
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
        
    string? IContextElement.ContextKey { get; set; }
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
    readonly string _contextKey;
    readonly TimeSpan _period;
    readonly Action<Signal> _callback;
    PeriodicTimer _timer;
    DateTimeOffset _startTime;

    internal Timer(TimeSpan period, string contextKey, Action<Signal> callback, bool start)
    {
        _period = period;
        _contextKey = contextKey;
        _callback = callback;
        if (start) {
            _startTime = DateTimeOffset.Now;
            StartTimer();
        }
    }
        
    void StartTimer()
    {
        Console.WriteLine("StartTimer");
        Stop();
        
        var signal = new TimerSignal(_contextKey);
        _timer = new PeriodicTimer(_period);
        Task.Run(async () => {
            while (await _timer.WaitForNextTickAsync()) {
                _callback(signal);
            }
        });
    }
        
    public TimerSignal? Start()
    {
        if (_timer == null) {
            _startTime = DateTimeOffset.Now;
            StartTimer();
            return new TimerSignal(_contextKey);
        }

        return null;
    }

    public TimerSignal? Stop()
    {
        if (_timer != null) {
        Console.WriteLine("Disposing Timer");
            _startTime = DateTimeOffset.MinValue;
            _timer.Dispose();
            _timer = null;
            return new TimerSignal(_contextKey);
        }

        return null;
    }

    public bool IsRunning => _timer != null;
    public TimeSpan Elapsed => IsRunning ? DateTimeOffset.Now  - _startTime : TimeSpan.Zero;

    public void Dispose()
    {
        _timer?.Dispose();
        _timer = null;
    }
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