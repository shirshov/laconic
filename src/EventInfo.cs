namespace Laconic;

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