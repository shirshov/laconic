using System;
using System.Linq;
using Xunit;
using Shouldly;
using xf = Xamarin.Forms;

namespace Laconic.Tests
{
    public class PatchTests
    {
        (IElement, IElement) NoopExpander(IContextElement? x, IContextElement y) => ((IElement) x, (IElement) y);
        
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
            var blueprint = new StackLayout {["lbl"] = new Label()};
            var real = new xf.StackLayout();
            Patch.Apply(real, Diff.Calculate(null, blueprint, NoopExpander), _ => { });
            real.Children.Count.ShouldBe(1);
            real.Children[0].ShouldBeOfType<xf.Label>();

            var diff = Diff.Calculate(blueprint, new StackLayout(), NoopExpander);
            Patch.Apply(real, diff, _ => { });

            real.Children.Count.ShouldBe(0);
        }

        [Fact]
        public void remove_child_view_by_setting_to_null()
        {
            var blueprint = new StackLayout {["lbl"] = new Label()};
            var real = new xf.StackLayout();
            Patch.Apply(real, Diff.Calculate(null, blueprint, NoopExpander), _ => { });
            real.Children.Count.ShouldBe(1);
            real.Children[0].ShouldBeOfType<xf.Label>();

            var diff = Diff.Calculate(blueprint, new StackLayout {["lbl"] = null}, NoopExpander);
            Patch.Apply(real, diff, _ => { });

            real.Children.Count.ShouldBe(0);
        }

        [Fact]
        public void replace_child_view()
        {
            var real = new xf.StackLayout {Children = {new xf.Label()}};
            Patch.Apply(real,
                Diff.Calculate(
                    new StackLayout {["one"] = new Label()},
                    new StackLayout {["one"] = new Button()}, NoopExpander),
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
                    new StackLayout {
                        GestureRecognizers = { [0] = new TapGestureRecognizer {
                            NumberOfTapsRequired = 2, Tapped = () => new Signal("foo")
                        }
                    }
                }, NoopExpander), 
                _ => { }
            );

            sl.GestureRecognizers.Count.ShouldBe(1);
            sl.GestureRecognizers.First().ShouldBeOfType<xf.TapGestureRecognizer>().NumberOfTapsRequired.ShouldBe(2);
        }

        [Fact]
        public void do_not_replace_GestureRecognizer_if_identical()
        {
            var sl = new xf.StackLayout();
            var blueprint = new StackLayout {
                GestureRecognizers = {
                    [0] = new TapGestureRecognizer {NumberOfTapsRequired = 1, Tapped = () => new Signal("foo")}
                }
            };
            Patch.Apply(sl, Diff.Calculate(null, blueprint, NoopExpander), _ => { });

            var recognizer = sl.GestureRecognizers.First();

            Patch.Apply(sl,
                Diff.Calculate(blueprint,
                    new StackLayout { 
                        GestureRecognizers = {
                            [0] = new TapGestureRecognizer {
                                NumberOfTapsRequired = 1, Tapped = () => new Signal("foo")
                            }
                        }
                    }, NoopExpander), 
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
                }, NoopExpander), 
                _ => { }
            );

            page.ToolbarItems.Count.ShouldBe(1);
            page.ToolbarItems.First().ShouldBeOfType<xf.ToolbarItem>();
        }
        
        [Fact]
        public void should_insert_child_view()
        {
            var blueprint = new StackLayout {["10"] = new Label(), ["20"] = new Button()};

            var real = new xf.StackLayout();
            Patch.Apply(real, Diff.Calculate(null, blueprint, NoopExpander), _ => { });

            var diff = Diff.Calculate(blueprint,
                new StackLayout {["10"] = new Label(), ["15"] = new Entry(), ["20"] = new Button()}, NoopExpander);

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
    }
}