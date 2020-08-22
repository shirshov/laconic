using Xunit;
using Shouldly;

namespace Laconic.CodeGeneration.Tests
{
    public class Signal
    {
        public readonly object? Payload;
        readonly object? _p1;
        readonly object? _p2;
        
        public Signal(object? payload) => (Payload, _p1, _p2) = (payload, payload, null);
        public Signal(object p1, object? p2) => (Payload, _p1, _p2) = (p1, p1, p2);

        public void Deconstruct(out object? p1, out object? p2) => (p1, p2) = (_p1, _p2);

        public override string ToString() => $"{GetType()}: {_p1?.ToString()} {_p2?.ToString()}";
    }

    public class Signal<T> : Signal
    {
        public Signal(T payload) : base(payload)
        {
        }

        public new T Payload => (T) base.Payload!;
    }
 
    [Signals]
    interface __MySignal
    {
        Signal NoPayload();
        Signal WithOneParam(string id);
        Signal WithTwoParams(int id, string name);
        Signal WithThreeParams(int id, string first, string second);
    }
        
    public class SignalTests
    {
        [Fact]
        public void Signal_generation_works()
        {
            var noPayload = new NoPayload();

            noPayload.ShouldBeAssignableTo<Signal>();
            noPayload.ShouldBeAssignableTo<MySignal>();
            noPayload.Payload.ShouldBeNull();
            
            var twoParams = new WithTwoParams(1, "one");
            twoParams.Id.ShouldBe(1);
            twoParams.Name.ShouldBe("one");
            var (id, name) = twoParams;
            id.ShouldBe(1);
            name.ShouldBe("one");

            var threeParams = new WithThreeParams(1, "one", "two");
            
            threeParams.Id.ShouldBe(1);
            threeParams.First.ShouldBe("one");
            threeParams.Second.ShouldBe("two");
        }
    }
}