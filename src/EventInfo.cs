using System;

namespace Laconic
{
    public class EventInfo
    {
        public readonly Func<EventArgs, Signal> SignalMaker;
        public readonly Action<Xamarin.Forms.BindableObject, EventHandler> Subscribe;
        public readonly Action<Xamarin.Forms.BindableObject, EventHandler> Unsubscribe;

        public EventInfo(Func<EventArgs, Signal> signalMaker, 
            Action<Xamarin.Forms.BindableObject, EventHandler> subscribe,
            Action<Xamarin.Forms.BindableObject, EventHandler> unsubscribe)
        {
            SignalMaker = signalMaker;
            Subscribe = subscribe;
            Unsubscribe = unsubscribe;
        }
    }
}