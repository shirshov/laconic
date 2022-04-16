namespace Laconic;

public class Signal
{
    public readonly object? Payload;
    readonly object? _p1;
    readonly object? _p2;
        
    public Signal(object? payload) => (Payload, _p1, _p2) = (payload, payload, null);
    public Signal(object p1, object? p2) => (Payload, _p1, _p2) = (p1, p1, p2);

    public void Deconstruct(out object? p1, out object? p2) => (p1, p2) = (_p1, _p2);

    public override string ToString() => $"{GetType()}: {_p1} {_p2}";
}

public class Signal<T> : Signal
{
    public Signal(T payload) : base(payload)
    {
    }

    public new T Payload => (T) base.Payload!;
}