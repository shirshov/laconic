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
            var real = binder.CreateElement(state => new ContentPage {Title = "Page"});
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
            var signalIntercepted = new Signal("");
            var binder = Binder.Create("state", (state, signal) =>
            {
                signalIntercepted = signal;
                return (string) signal.Payload!;
            });
            binder.ProcessSignal(new TestSignal("new state"));
            signalIntercepted.Payload.ShouldBe("new state");
            binder.State.ShouldBe("new state");
        }

        [Fact]
        public void view_changes_after_signal()
        {
            var binder = Binder.Create("state", (state, signal) => (string) signal.Payload!);
            var view = binder.CreateElement(state => new Label {Text = state});
            view.Text.ShouldBe("state");
            binder.ProcessSignal(new TestSignal("new state"));
            view.Text.ShouldBe("new state");
        }

        [Fact]
        public void child_view_change_after_signal()
        {
            var binder = Binder.Create("state", (state, signal) => (string) signal.Payload!);
            var stackLayout = binder.CreateElement(state => new StackLayout {[1] = new Label {Text = state}});
            var label = (xf.Label) stackLayout.Children[0];
            label.Text.ShouldBe("state");
            binder.ProcessSignal(new TestSignal("new state"));
            label.Text.ShouldBe("new state");
        }
        
        [Fact]
        public void handle_view_updated_by_local_context()
        {
            var one = Element.WithContext(ctx => {
                var (text, setText) = ctx.UseLocalState("");
                return  new StackLayout {
                    [0] = new Button {Text = "Reveal", Clicked = () => setText("new element")},
                    [1] = text == "" ? null : new Label {Text = text}
                };
            });
            
            var two = new StackLayout {[0] = new Label {Text = "Second View"}};

            var binder = Binder.CreateForTest(1, (s, g) => (int)g.Payload);
            
            var page = binder.CreateElement(s => new ContentPage {Content = s == 1 ? one : two});
            var sl = (xf.StackLayout) page.Content;
            
            sl.Children.Count.ShouldBe(1);
            
            var button = (xf.Button) sl.Children[0];
            button.SendClicked();

            sl.Children.Count.ShouldBe(2);
            
            binder.Send(new(2));
            
            sl.Children[0].ShouldBeOfType<xf.Label>().Text.ShouldBe("Second View");
            sl.Children.Count.ShouldBe(1);
        }
    }
}