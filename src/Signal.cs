namespace Laconic
{
    public class Signal
    {
        public readonly object Payload;
        public static Signal Send(object payload) => new Signal(payload);
        public static Signal Send(object p1, object p2) => new Signal(p1, p2);

        public Signal(object payload) => Payload = payload;
        public Signal(object p1, object p2) => Payload = (p1, p2);

        public void Deconstruct(out object p1, out object p2)
        {
            (p1, p2) = ((object, object)) Payload;
        }
    }

    public class Signal<T> : Signal
    {
        public Signal(T payload) : base(payload)
        {
        }

        public new T Payload => (T) base.Payload;
    }
}