namespace Laconic.Tests;

public class PatchTests
{
    [Fact]
    public void set_content_view()
    {
        var contentView = new xf.ContentView();
        var diff = new[] {new SetContent(new Label(), new DiffOperation[0])};
        Patch.Apply(contentView, diff, _ => { });
        contentView.Content.ShouldBeOfType<xf.Label>();
    }

    [Fact]
    public void remove_content_view()
    {
        var frame = new xf.Frame();
        var diff = new[] {new RemoveContent()};
        Patch.Apply(frame, diff, _ => { });
        frame.Content.ShouldBeNull();
    }

    [Fact]
    public void replace_content_view()
    {
        var contentPage = new xf.ContentPage {Content = new xf.Label()};
        var diff = new[] {new SetContent(new Button(), new[] {new SetProperty(xf.Button.TextProperty, "Button")})};
        Patch.Apply(contentPage, diff, _ => { });

        contentPage.Content.ShouldBeOfType<xf.Button>().Text.ShouldBe("Button");
    }

    [Fact]
    public void update_content_view()
    {
        var frame = new xf.Frame {Content = new xf.Label {Text = "foo"}};
        var diff = new[] {new UpdateContent(new[] {new SetProperty(xf.Label.TextProperty, "bar")})};
        Patch.Apply(frame, diff, _ => { });

        frame.Content.ShouldBeOfType<xf.Label>().Text.ShouldBe("bar");
    }

    [Fact]
    public void add_child_view()
    {
        var sl = new xf.StackLayout();
        var diff = new[] {new AddChild("a", "reuseKey", 0, new Label(), new DiffOperation[0])};
        Patch.Apply(sl, new[] {new UpdateChildViews(diff)}, _ => { });

        sl.Children.Count.ShouldBe(1);
        sl.Children[0].ShouldBeOfType<xf.Label>();
    }

    [Fact]
    public void remove_child_view()
    {
        var blueprint = new VerticalStackLayout {["lbl"] = new Label()};
        var real = new xf.StackLayout();
        Patch.Apply(real, Diff.Calculate(null, blueprint), _ => { });
        real.Children.Count.ShouldBe(1);
        real.Children[0].ShouldBeOfType<xf.Label>();

        var diff = Diff.Calculate(blueprint, new VerticalStackLayout());
        Patch.Apply(real, diff, _ => { });

        real.Children.Count.ShouldBe(0);
    }

    [Fact]
    public void remove_child_view_by_setting_to_null()
    {
        var blueprint = new VerticalStackLayout {["lbl"] = new Label()};
        var real = new xf.StackLayout();
        Patch.Apply(real, Diff.Calculate(null, blueprint), _ => { });
        real.Children.Count.ShouldBe(1);
        real.Children[0].ShouldBeOfType<xf.Label>();

        var diff = Diff.Calculate(blueprint, new VerticalStackLayout {["lbl"] = null});
        Patch.Apply(real, diff, _ => { });

        real.Children.Count.ShouldBe(0);
    }

    [Fact]
    public void replace_child_view()
    {
        var real = new xf.StackLayout {Children = {new xf.Label()}};
        Patch.Apply(real,
            Diff.Calculate(
                new VerticalStackLayout {["one"] = new Label()},
                new VerticalStackLayout {["one"] = new Button()}),
            _ => { });
        real.Children[0].ShouldBeOfType<xf.Button>();
    }

    [Fact]
    public void add_grid_children()
    {
        var real = new xf.Grid();
        Patch.Apply(real,
            new[]
            {
                new UpdateChildViews(new[]
                {
                    new AddChild("a", "reuseKey", 0, new Label(), new DiffOperation[0]),
                    new AddChild("b", "reuseKey", 1, new BoxView(),
                        new[] {new GridPositionChange(GridPositionChangeType.Column, 1)}),
                    new AddChild("c", "reuseKey", 2, new Button(),
                        new[]
                        {
                            new GridPositionChange(GridPositionChangeType.Row, 1),
                            new GridPositionChange(GridPositionChangeType.ColumnSpan, 2),
                            new GridPositionChange(GridPositionChangeType.RowSpan, 2)
                        })
                })
            }, _ => { });

        var label = real.Children[0].ShouldBeOfType<xf.Label>();
        xf.Grid.GetRow(label).ShouldBe(0);
        xf.Grid.GetColumn(label).ShouldBe(0);

        var boxView = real.Children[1].ShouldBeOfType<xf.BoxView>();
        xf.Grid.GetRow(boxView).ShouldBe(0);
        xf.Grid.GetColumn(boxView).ShouldBe(1);

        var button = real.Children[2].ShouldBeOfType<xf.Button>();
        xf.Grid.GetRow(button).ShouldBe(1);
        xf.Grid.GetColumn(button).ShouldBe(0);
        xf.Grid.GetColumnSpan(button).ShouldBe(2);
        xf.Grid.GetRowSpan(button).ShouldBe(2);
    }

    [Fact]
    public void add_GestureRecognizer()
    {
        var sl = new xf.StackLayout();
        Patch.Apply(sl,
            Diff.Calculate(null,
                new VerticalStackLayout {
                    GestureRecognizers = { [0] = new TapGestureRecognizer {
                            NumberOfTapsRequired = 2, Tapped = () => new Signal("foo")
                        }
                    }
                }), 
            _ => { }
        );

        sl.GestureRecognizers.Count.ShouldBe(1);
        sl.GestureRecognizers.First().ShouldBeOfType<xf.TapGestureRecognizer>().NumberOfTapsRequired.ShouldBe(2);
    }

    [Fact]
    public void do_not_replace_GestureRecognizer_if_identical()
    {
        var sl = new xf.StackLayout();
        var blueprint = new VerticalStackLayout {
            GestureRecognizers = {
                [0] = new TapGestureRecognizer {NumberOfTapsRequired = 1, Tapped = () => new Signal("foo")}
            }
        };
        Patch.Apply(sl, Diff.Calculate(null, blueprint), _ => { });

        var recognizer = sl.GestureRecognizers.First();

        Patch.Apply(sl,
            Diff.Calculate(blueprint,
                new VerticalStackLayout { 
                    GestureRecognizers = {
                        [0] = new TapGestureRecognizer {
                            NumberOfTapsRequired = 1, Tapped = () => new Signal("foo")
                        }
                    }
                }), 
            _ => { });

        sl.GestureRecognizers.Count.ShouldBe(1);
        sl.GestureRecognizers[0].ShouldBe(recognizer);
    }

    [Fact]
    public void add_ToolbarItem()
    {
        var page = new xf.ContentPage();
        Patch.Apply(page,
            Diff.Calculate(null,
                new ContentPage {ToolbarItems = { [0] = new ToolbarItem()}
                }), 
            _ => { }
        );

        page.ToolbarItems.Count.ShouldBe(1);
        page.ToolbarItems.First().ShouldBeOfType<xf.ToolbarItem>();
    }
        
    [Fact]
    public void should_insert_child_view()
    {
        var blueprint = new VerticalStackLayout {["10"] = new Label(), ["20"] = new Button()};

        var real = new xf.StackLayout();
        Patch.Apply(real, Diff.Calculate(null, blueprint), _ => { });

        var diff = Diff.Calculate(blueprint,
            new VerticalStackLayout {["10"] = new Label(), ["15"] = new Entry(), ["20"] = new Button()});

        Patch.Apply(real, diff, _ => { });

        real.Children[1].ShouldBeOfType<xf.Entry>();
    }

    [Fact]
    public void wire_event()
    {
        var val = "initial";

        var binder = Binder.CreateForTest("state", (s, g) => {
            val = "modified";
            return s;
        });
        var real = binder.CreateElement(s =>
            new RefreshView {Refreshing = e => new Signal(s)}
        );

        real.IsRefreshing = true;
            
        val.ShouldBe("modified");
    }
        
    [Fact]
    public void unwire_event()
    {
        var binder = Binder.CreateForTest(0, (s, g) => ++s);
            
        var view = binder.CreateElement(s => new RefreshView {
            Refreshing = e => s == 0 ? new Signal("") : null
        });
            
        view.IsRefreshing = true;
        view.IsRefreshing = false;
            
        binder.State.ShouldBe(1);
            
        view.IsRefreshing = true;
        view.IsRefreshing = false;
        view.IsRefreshing = true;
        view.IsRefreshing = false;
            
        binder.State.ShouldBe(1);
    }

    [Fact]
    public void captured_value_in_event()
    {
        var binder = Binder.Create(0, (s, g) => {
            g.Payload.ShouldBe(s);
            return s + 1;
        });
        var real = binder.CreateElement(s =>
            new RefreshView {Refreshing = e => new Signal(s)}
        );
        real.IsRefreshing = true;
        real.IsRefreshing = false;
        real.IsRefreshing = true;
        real.IsRefreshing = false;
        real.IsRefreshing = true;
        real.IsRefreshing = false;
    }

    class Postprocessed : View<xf.BoxView>
    {
        protected internal override xf.BindableObject CreateView() => new xf.BoxView();

        public bool IsProcessed;
            
        public bool IsToggled {
            set => SetValue(nameof(IsToggled), value, el => { IsProcessed = true; });
        }
    }

    [Fact]
    public void property_is_post_processed()
    {
        var blueprint = new Postprocessed { IsToggled = true };

        var binder = Binder.Create(0, (s, g) => s);

        var view = binder.CreateElement(s => blueprint);
            
        blueprint.IsProcessed.ShouldBeTrue();
    }

    [Fact]
    public void change_of_LocalContext()
    {
        var stringStateView = Element.WithContext("string", ctx => {
            var (state, _) = ctx.UseLocalState("string");
            return new Label {Text = state};
        });
            
        var intStateView = Element.WithContext("int", ctx => {
            var (state, _) = ctx.UseLocalState(1);
            return new Label {Text = state.ToString()};
        });

        var alternatives = new[] {stringStateView, intStateView};
            
        var binder = Binder.CreateForTest(0, (s, g) => (int)g.Payload);

        var realView = binder.CreateElement(s => new ContentView { Content  = alternatives[s] });

        var lbl = (xf.Label) realView.Content;
        lbl.Text.ShouldBe("string");

        binder.Send(new(1));
            
        lbl.Text.ShouldBe("1");
    }
}