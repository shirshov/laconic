using Xunit;
using Shouldly;
using xf = Xamarin.Forms;

namespace Laconic.Tests
{
    class State
    {
    }

    public class BinderTests
    {
        [Fact]
        void page_can_be_created_from_binder()
        {
            var binder = Binder.Create(new State(), (state, signal) => state);
            var real = binder.CreatePage(state => new ContentPage {Title = "Page"});
            real.ShouldBeOfType<xf.ContentPage>();
            real.Title.ShouldBe("Page");
        }

        class TestSignal : Signal<string>
        {
            public TestSignal(string payload) : base(payload)
            {
            }
        }

        [Fact]
        public void signal_is_processed()
        {
            Signal signalIntercepted = null;
            var binder = Binder.Create("state", (state, signal) =>
            {
                signalIntercepted = signal;
                return (string) signal.Payload;
            });
            binder.Dispatch(new TestSignal("new state"));
            signalIntercepted.Payload.ShouldBe("new state");
            binder.State.ShouldBe("new state");
        }

        [Fact]
        public void view_changes_after_signal()
        {
            var binder = Binder.Create("state", (state, signal) => (string) signal.Payload);
            var view = binder.CreateView(state => new Label {Text = state});
            view.Text.ShouldBe("state");
            binder.Dispatch(new TestSignal("new state"));
            view.Text.ShouldBe("new state");
        }

        [Fact]
        public void child_view_change_after_signal()
        {
            var binder = Binder.Create("state", (state, signal) => (string) signal.Payload);
            var stackLayout = binder.CreateView(state => new StackLayout {[1] = new Label {Text = state}});
            var label = (xf.Label) stackLayout.Children[0];
            label.Text.ShouldBe("state");
            binder.Dispatch(new TestSignal("new state"));
            label.Text.ShouldBe("new state");
        }
    }
}