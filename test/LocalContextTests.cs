namespace Laconic.Tests;

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
        xfStackLayout.Children.Count.ShouldBe(3);           
        xfStackLayout.Children[0].ShouldBe(button);
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

    [Fact]
    public void ToolBarItems_on_page()
    {
        static VisualElement<Xamarin.Forms.ContentPage> TestPage(int state) => Element.WithContext(_ => {
            return new ContentPage {
                ToolbarItems = {["save"] = new ToolbarItem {Text = "Save", Clicked = () => new Signal(null)}},
            };
        });
                
        var binder = Binder.CreateForTest(0, (s, g) => s);

        var page = binder.CreateElement(TestPage);
            
        page.ToolbarItems.Count.ShouldBe(1);
            
        binder.Send(new Signal(null));
        page.ToolbarItems.Count.ShouldBe(1);
            
        binder.Send(new Signal(null));
        page.ToolbarItems.Count.ShouldBe(1);
    }

    [Fact]
    public void child_element_has_context()
    {
        var child = Element.WithContext(ctx => {
            var (text, setState) = ctx.UseLocalState("");
            return new ContentView {
                Content = new Button {
                    Text = text,
                    Clicked = () => setState("clicked")
                }
            };
        });
            
        var binder = Binder.CreateForTest("", (s, _) => s);
            
        var page = binder.CreateElement(_ => new ContentPage {
            Content = child
        });

        var contentView = page.Content as xf.ContentView;
        var btn = contentView.Content as xf.Button;
            
        btn.Text.ShouldBe("");
            
        btn.SendClicked();
            
        btn.Text.ShouldBe("clicked");
    }
        
    [Fact]
    public void replacing_ContextElement_does_not_recycle_context()
    {
        static VisualElement<xf.StackLayout> One() => Element.WithContext("one", ctx => {
            var (state, setState) = ctx.UseLocalState("");
            return new StackLayout { ["one"] = new Label {Text = "one"} };
        });
            
        static VisualElement<xf.StackLayout> Two() => Element.WithContext("two", ctx => {
            var (state, setState) = ctx.UseLocalState(0);
            return new StackLayout { ["two"] = new Button {Text = "two"}};
        });

        VisualElement<xf.ContentPage> Container(string s)
        {
            return new ContentPage {Content = (s == "one" ? (View) One() : (View) Two())};
        }

        var binder = Binder.CreateForTest("one", (s, g) => (string)g.Payload);
        var page = binder.CreateElement(Container);
        var stackLayout = page.Content.ShouldBeOfType<xf.StackLayout>();
            
        stackLayout.Children.Count.ShouldBe(1);
        stackLayout.Children.First().ShouldBeOfType<xf.Label>().Text = "one";
            
        binder.Send(new Signal("two"));
        stackLayout.Children.Count.ShouldBe(1);
        stackLayout.Children.First().ShouldBeOfType<xf.Button>().Text = "two";
            
        binder.Send(new Signal("one"));
        stackLayout.Children.Count.ShouldBe(1);
        stackLayout.Children.First().ShouldBeOfType<xf.Label>().Text = "one";
    } 
        
    [Fact(Skip = "Not implemented yet")]
    public void nested_contexts()
    {
    }
}