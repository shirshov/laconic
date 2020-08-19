using System;
using System.Linq;
using System.Net;
using Xunit;
using xf = Xamarin.Forms;
using Shouldly;

namespace Laconic.Tests
{
    public class LocalContextTests
    {
        [Fact]
        public void LocalState_is_updated()
        {
            var binder = Binder.CreateForTest(0, (s, g) => s);
            var button = binder.CreateElement(s => Element.WithContext(ctx => {
                var (state, setState) = ctx.UseLocalState("initial");
                return new Button {Text = state, Clicked = () => setState("changed")};
            }));
            
            button.Text.ShouldBe("initial");
            
            button.SendClicked();

            button.Text.ShouldBe("changed");
        }

        [Fact]
        public void Diff_expands_elements_with_context()
        {
            var label = Element.WithContext(ctx => {
                var (state, _) = ctx.UseLocalState("initial");
                return new Label {Text = state};
            });

            var context = new LocalContext();
            IElement existingExpanded = null;

            (IElement, IElement) ExpandWithContext(IContextElement existing, IContextElement newOne)
            {
                existingExpanded ??= existing?.Make(context);
                return (existingExpanded, newOne.Make(context));
            }

            var diff = Diff.Calculate(null, label, ExpandWithContext);
            diff.First().ShouldBeOfType<SetProperty>().Value.ShouldBe("initial");

            diff = Diff.Calculate(label, label, ExpandWithContext);
            diff.ShouldBeEmpty();
            
            context.SetValue(LocalContext.LOCAL_STATE_KEY, "modified");

            diff = Diff.Calculate(label, label, ExpandWithContext);
            
            diff.Count().ShouldBe(1);
            diff.First().ShouldBeOfType<SetProperty>().Value.ShouldBe("modified");
        }

        [Fact]
        public void Diff_handles_AddChildWithRequest()
        {
            var sl = new StackLayout {Children = {[1] = Element.WithContext(ctx => new Label())}};
            var context = new LocalContext();
            var diff = Diff.Calculate(null, sl, (x, y) => (null, y.Make(context)));
            
            diff.First().ShouldBeOfType<UpdateChildren>()
                .Operations.First().ShouldBeOfType<AddChildWithContext>();
        }

        [Fact]
        public void each_child_has_own_context()
        {
            var child1 = Element.WithContext(ctx => {
                var (state, setter) = ctx.UseLocalState("child1");
                return new Button {Text = state, Clicked = () => setter("child1 updated")};
            });
            
            var child2 = Element.WithContext(ctx => {
                var (state, setter) = ctx.UseLocalState("child2");
                return new Button {Text = state, Clicked = () => setter("child2 updated")};
            });
            
            var binder = Binder.CreateForTest(0, (s, g) => s);
            var sl = binder.CreateElement(s => new StackLayout {Children = {[1] = child1, [2] = child2}});

            var btn1 = (xf.Button) sl.Children[0];
            btn1.Text.ShouldBe("child1");
            
            var btn2 = (xf.Button) sl.Children[1];
            btn2.Text.ShouldBe("child2");
            
            btn1.SendClicked();
            
            btn1.Text.ShouldBe("child1 updated");
            btn2.Text.ShouldBe("child2");
            
            btn2.SendClicked();
            
            btn1.Text.ShouldBe("child1 updated");
            btn2.Text.ShouldBe("child2 updated");
        }

        [Fact]
        public void local_context_is_preserved_on_global_signal()
        {
            var binder = Binder.CreateForTest(0, (s, g) => s + 1);
            var button = binder.CreateElement(globalState => Element.WithContext(ctx => {
                var (localState, setter) = ctx.UseLocalState(0);
                return new Button { Text = $"G{globalState}L{localState}", Clicked = () => setter(localState + 1)};
            }));
            
            button.Text.ShouldBe("G0L0");
            
            button.SendClicked();
            
            button.Text.ShouldBe("G0L1");
            
            binder.Send(new Signal(""));
            
            button.Text.ShouldBe("G1L1");
        }

        [Fact]
        public void event_subscription_is_transferred()
        {
            var counter = 0;
            var binder = Binder.CreateForTest(0, (s, g) => s + 1);
            var button = binder.CreateElement(s => Element.WithContext(ctx => {
                var (state, setter) = ctx.UseLocalState(0);
                counter++;
                return new Button {Clicked = () => setter(++state)};
            }));
            
            counter.ShouldBe(1);
            
            button.SendClicked();
            counter.ShouldBe(2);
            binder.State.ShouldBe(0);
            
            button.SendClicked();
            counter.ShouldBe(3);
            binder.State.ShouldBe(0);
            
            button.SendClicked();
            counter.ShouldBe(4);
            binder.State.ShouldBe(0);
        }

        [Fact]
        public void event_subscription_for_child_is_transferred()
        {
            var counter = 0;
            var binder = Binder.CreateForTest(0, (s, g) => s + 1);
            var sl = binder.CreateElement(s => new StackLayout {
                ["a"] = Element.WithContext(ctx => {
                    var (state, setter) = ctx.UseLocalState(0);
                    counter++;
                    return new Button {Clicked = () => setter(++state)};
                })
            });

            var button = (xf.Button) sl.Children[0];
            
            counter.ShouldBe(1);
            
            button.SendClicked();
            counter.ShouldBe(2);
            binder.State.ShouldBe(0);
            
            button.SendClicked();
            counter.ShouldBe(3);
            binder.State.ShouldBe(0);
            
            button.SendClicked();
            counter.ShouldBe(4);
            binder.State.ShouldBe(0);
        }
        
        [Fact]
        public void children_are_added_from_local_state_update()
        {
            var counter = 0;
            var binder = Binder.CreateForTest(0, (s, g) => s);
            var xfStackLayout = binder.CreateElement(s => Element.WithContext(ctx => {
                var (state, setter) = ctx.UseLocalState(0);
                counter = state;
                var sl = new StackLayout();
                sl.Children["btn"] = new Button { Clicked = () => setter(state + 1)};
                for (var i = 0; i < state; i++) {
                    sl.Children[i]  = new Label { Text = i.ToString()};
                }
                return sl;
            }));
            
            counter.ShouldBe(0);
            xfStackLayout.Children.Count.ShouldBe(1);
            
            var button = xfStackLayout.Children[0].ShouldBeOfType<xf.Button>();
            
            button.SendClicked();
            
            xfStackLayout.Children[0].ShouldBe(button);
            counter.ShouldBe(1);
            xfStackLayout.Children.Count.ShouldBe(2);

            button.SendClicked();
            
            counter.ShouldBe(2);
            xfStackLayout.Children[0].ShouldBe(button);
            xfStackLayout.Children.Count.ShouldBe(3);           
        }

        [Fact]
        public void elements_with_local_context_created_from_middleware()
        {
            xf.StackLayout sl = null;
            var binder = Binder.CreateForTest(0, (s, g) => s);
            binder.UseMiddleware((ctx, next) => {
                sl = binder.CreateElement(s => Element.WithContext(_ => new StackLayout {[0] = new Button()}));
                return next(ctx);
            });
            
            binder.Send(new Signal(""));
            sl.Children.Count.ShouldBe(1);
        }

        [Fact(Skip = "Not implemented yet")]
        public void nested_contexts()
        {
        }
    }
}