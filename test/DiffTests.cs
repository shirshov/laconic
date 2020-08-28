using System;
using System.Linq;
using Xunit;
using Shouldly;
using xf = Xamarin.Forms;

namespace Laconic.Tests
{
    public class DiffTests
    {
        (IElement, IElement) NoopExpander(IContextElement? x, IContextElement y) => ((IElement) x, (IElement) y);
        
        [Fact]
        public void should_update_property()
        {
            var operations = Diff.Calculate(new Label {Text = "foo"}, new Label {Text = "bar"}, NoopExpander);
            operations.Count().ShouldBe(1);
            var op = operations.First() as SetProperty;
            op.Property.ShouldBe(xf.Label.TextProperty);
            op.Value.ShouldBe("bar");
        }

        [Fact]
        public void should_reset_property()
        {
            var operations = Diff.Calculate(new Label {Text = "foo"}, new Label(), NoopExpander);
            operations.First()
                .ShouldBeOfType<ResetProperty>()
                .Property.ShouldBe(xf.Label.TextProperty);
        }

        [Fact]
        public void ignore_unchanged_vales()
        {
            var operations = Diff.Calculate(new Label {Text = "a"}, new Label {Text = "a"}, NoopExpander);
            operations.Count().ShouldBe(0);
        }

        [Fact]
        public void should_set_content()
        {
            var diff = Diff.Calculate(
                new ContentView(),
                new ContentView {Content = new Label {Text = "New"}},
                NoopExpander
            );

            diff.First()
                .ShouldBeOfType<SetContent>()
                .ContentView
                .ShouldBeOfType<Label>()
                .Text.ShouldBe("New");
        }

        [Fact]
        public void set_content_of_ContentPage()
        {
            var diff = Diff.Calculate(null, new ContentPage {Content = new Label()}, NoopExpander).ToArray();
            diff[0].ShouldBeOfType<SetContent>().ContentView.ShouldBeOfType<Label>();
        }

        [Fact]
        public void should_replace_content()
        {
            var diff = Diff.Calculate(
                new ContentView {Content = new Label()},
                new ContentView {Content = new Button()}, 
                NoopExpander
            );

            diff.First()
                .ShouldBeOfType<SetContent>()
                .ContentView
                .ShouldBeOfType<Button>();
        }

        [Fact]
        public void set_content_to_null()
        {
            var diff = Diff.Calculate(
                new ContentView {Content = new Label()},
                new ContentView(),
                NoopExpander
            );

            diff.First().ShouldBeOfType<RemoveContent>();
        }

        [Fact]
        public void ingore_null_content()
        {
            var diff = Diff.Calculate(null, new ContentView(), NoopExpander);

            diff.Count().ShouldBe(0);
        }

        [Fact]
        public void ignore_null_for_child_views()
        {
            var diff = Diff.Calculate(null, new StackLayout {["ignored"] = null}, NoopExpander);
            diff.Count().ShouldBe(0);
        }
        
        [Fact]
        public void calc_diff_for_first_view()
        {
            var diff = Diff.Calculate(
                null,
                new ContentPage {Title = "Root Page"},
                NoopExpander
            );
            diff.First().ShouldBeOfType<SetProperty>().Value.ShouldBe("Root Page");
        }

        [Fact]
        public void should_update_content()
        {
            var diff = Diff.Calculate(
                new ContentView {Content = new Label {Text = "Foo"}},
                new ContentView {Content = new Label {Text = "Bar"}},
                NoopExpander
            );
            var upd = diff.First().ShouldBeOfType<UpdateContent>();
            var setProp = upd.Operations.First().ShouldBeOfType<SetProperty>();
            setProp.Property.ShouldBe(xf.Label.TextProperty);
            setProp.Value.ShouldBe("Bar");
        }

        [Fact]
        public void ignore_child_initially_set_to_null()
        {
            var diff = Diff.Calculate(null, new StackLayout {["null"] = null}, NoopExpander);
            diff.Count().ShouldBe(0);
        }

        [Fact]
        public void remove_child_set_to_null()
        {
            var diff = Diff.Calculate(new StackLayout {["1"] = new Label()}, new StackLayout {["1"] = null}, NoopExpander);

            diff.Count().ShouldBe(1);
            diff.First().ShouldBeOfType<UpdateChildViews>()
                .Operations.First().ShouldBeOfType<RemoveChild>()
                .Index.ShouldBe(0);
        }

        [Fact]
        public void replace_null_view_with_instance()
        {
            var diff = Diff.Calculate(new StackLayout {["1"] = null}, new StackLayout {["1"] = new Label()}, NoopExpander);
        }

        [Fact]
        public void noop_if_null_child_replaces_null_child()
        {
            var diff = Diff.Calculate(new StackLayout {["1"] = null}, new StackLayout {["1"] = null}, NoopExpander);

            diff.Count().ShouldBe(0);
        }
        
        [Fact]
        public void add_children_to_new_view()
        {
            var diff = Diff.Calculate(
                null,
                new StackLayout {[1] = new Label(), [2] = new Label()},
                NoopExpander
            ).ToArray();

            var updateChildren = diff[0].ShouldBeOfType<UpdateChildViews>();

            var addOne = updateChildren.Operations[0].ShouldBeOfType<AddChild>();
            addOne.Index.ShouldBe(0);
            addOne.Blueprint.ShouldBeOfType<Label>();

            var addTwo = updateChildren.Operations[1].ShouldBeOfType<AddChild>();
            addTwo.Index.ShouldBe(1);
            addTwo.Blueprint.ShouldBeOfType<Label>();
        }

        [Fact]
        public void add_second_child()
        {
            var diff = Diff.Calculate(
                new StackLayout {["first"] = new Label()},
                new StackLayout {["first"] = new Label(), ["second"] = new Button()},
                NoopExpander
            ).ToArray();

            diff[0].ShouldBeOfType<UpdateChildViews>()
                .Operations[0].ShouldBeOfType<AddChild>()
                .Index.ShouldBe(1);
        }

        [Fact]
        public void wire_event()
        {
            var diff = Diff.Calculate(
                new Button(),
                new Button {Clicked = () => new Signal<string>("")}, NoopExpander).ToArray();

            diff[0].ShouldBeOfType<WireEvent>();
        }

        [Fact]
        public void unwire_event()
        {
            var diff = Diff.Calculate(
                new Button {Clicked = () => new Signal<string>("")},
                new Button(),
                NoopExpander
            ).ToArray();

            diff[0].ShouldBeOfType<UnwireEvent>();
        }
        
        [Fact]
        public void transfer_event_subscription_on_diffing()
        {
            var binder = Binder.Create(new StateWrapper(1), (s, g) => {
                g.Payload.ShouldBe(s);
                return new StateWrapper(s.Value + 1);
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
        
        [Fact]
        public void empty_diff_if_Grid_ColumnDefinitions_identical()
        {
            var diff = Diff.Calculate(
                new Grid { ColumnDefinitions = "1, 2" },
                new Grid { ColumnDefinitions = "1, 2" },
                NoopExpander
            );
            
            diff.ShouldBeEmpty();
        }
        
        [Fact]
        public void empty_diff_if_Grid_RowDefinitions_identical()
        {
            var diff = Diff.Calculate(
                new Grid { RowDefinitions = "1, 2" },
                new Grid { RowDefinitions = "1, 2" },
                NoopExpander
            );
            
            diff.ShouldBeEmpty();
        }
        
        [Fact]
        public void set_new_grid_children_positions()
        {
            var diff = Diff.Calculate(
                null,
                new Grid
                {
                    ["label", column: 1] = new Label(),
                    ["box", row: 1] = new BoxView(),
                    ["button", row: 1, columnSpan: 2] = new Button()
                }, NoopExpander).ToArray();

            var ops = diff[0].ShouldBeOfType<UpdateChildViews>().Operations;

            ops[0].ShouldBeOfType<AddChild>().Blueprint.ShouldBeOfType<Label>();
            var labelPos = ops[0].ShouldBeOfType<AddChild>().Operations[0].ShouldBeOfType<GridPositionChange>();
            labelPos.Type.ShouldBe(GridPositionChangeType.Column);
            labelPos.Value.ShouldBe(1);
        }

        [Fact]
        public void add_grid_child()
        {
            var diff = Diff.Calculate(
                new Grid {[1] = new BoxView()},
                new Grid {[1] = new BoxView(), [2] = new Label()},
                NoopExpander
            ).ToArray();

            var ops = diff[0].ShouldBeOfType<UpdateChildViews>().Operations;
            ops[0].ShouldBeOfType<AddChild>().Blueprint.ShouldBeOfType<Label>();
        }

        [Fact]
        public void change_grid_children_position()
        {
            var diff = Diff.Calculate(
                new Grid {["label"] = new Label(), ["box"] = new BoxView(), ["button"] = new Button()},
                new Grid
                {
                    ["label", column: 1] = new Label(),
                    ["box", row: 1] = new BoxView(),
                    ["button", row: 1, columnSpan: 2] = new Button()
                }, NoopExpander).ToArray();

            var updateGrid = diff[0].ShouldBeOfType<UpdateChildViews>().Operations;

            var labelPosChanges = updateGrid[0].ShouldBeOfType<UpdateChild>().Operations.Cast<GridPositionChange>()
                .ToArray();
            labelPosChanges[0].Type.ShouldBe(GridPositionChangeType.Column);
            labelPosChanges[0].Value.ShouldBe(1);

            var boxPosChanges = updateGrid[1].ShouldBeOfType<UpdateChild>().Operations.Cast<GridPositionChange>()
                .ToArray();
            boxPosChanges[0].Type.ShouldBe(GridPositionChangeType.Row);
            boxPosChanges[0].Value.ShouldBe(1);

            var buttonPosChanges = updateGrid[2].ShouldBeOfType<UpdateChild>().Operations.Cast<GridPositionChange>()
                .ToArray();
            buttonPosChanges[0].Type.ShouldBe(GridPositionChangeType.Row);
            buttonPosChanges[0].Value.ShouldBe(1);
            buttonPosChanges[1].Type.ShouldBe(GridPositionChangeType.ColumnSpan);
            buttonPosChanges[1].Value.ShouldBe(2);
        }

        [Fact]
        public void add_GestureRecognizer()
        {
            var diff = Diff.Calculate(
                new ContentView(),
                new ContentView { GestureRecognizers = { [0] = new TapGestureRecognizer() }},
                NoopExpander
            );
            
            diff.First().ShouldBeOfType<AddGestureRecognizer>();
        }

        [Fact]
        public void remove_GestureRecognizer()
        {
            var diff = Diff.Calculate(
                new ContentView { GestureRecognizers = { [0] = new TapGestureRecognizer() }},
                new ContentView(),
                NoopExpander
            );
            
            diff.First().ShouldBeOfType<RemoveGestureRecognizer>();
        }

        [Fact]
        public void update_GestureRecognizer()
        {
            var diff = Diff.Calculate(
                new ContentView {
                    GestureRecognizers = {
                        [0] = new TapGestureRecognizer(),
                        [1] = new TapGestureRecognizer { NumberOfTapsRequired = 1 },
                    }
                },
                new ContentView {
                    GestureRecognizers = {
                        [0] = new TapGestureRecognizer(),
                        [1] = new TapGestureRecognizer { NumberOfTapsRequired = 2 },
                    }
                },
                NoopExpander
            );

            var upd = diff.First().ShouldBeOfType<UpdateGestureRecognizer>();
            upd.Index.ShouldBe(1);
            upd.Operations.First().ShouldBeOfType<SetProperty>().Value.ShouldBe(2);
        }

        [Fact]
        public void add_ToolbarItem()
        {
            var diff = Diff.Calculate(
                new ContentPage(),
                new ContentPage { ToolbarItems = { [0] = new ToolbarItem() }},
                NoopExpander
            );
            
            diff.First().ShouldBeOfType<AddToolbarItem>();
        }

        [Fact]
        public void remove_ToolbarItem()
        {
            var diff = Diff.Calculate(
                new ContentPage {ToolbarItems = {[0] = new ToolbarItem()}},
                new ContentPage(),
                NoopExpander
            );
            
            diff.First().ShouldBeOfType<RemoveToolbarItem>().Index.ShouldBe(0);
        }

        [Fact]
        public void update_ToolbarItem()
        {
            var diff = Diff.Calculate(
                new ContentPage {
                    ToolbarItems = {
                        [0] = new ToolbarItem(),
                        [1] = new ToolbarItem { IconImageSource = "foo" },
                    }
                },
                new ContentPage {
                    ToolbarItems = {
                        [0] = new ToolbarItem(),
                        [1] = new ToolbarItem { IconImageSource = "bar"},
                    }
                },
                NoopExpander
            );

            var upd = diff.First().ShouldBeOfType<UpdateToolbarItem>();
            upd.Index.ShouldBe(1);
            var op = upd.Operations
                .First().ShouldBeOfType<UpdateChildElement>()
                .Operations.First().ShouldBeOfType<SetProperty>();
                
            op.Property.ShouldBe(xf.FileImageSource.FileProperty);
            op.Value.ShouldBe("bar");
        }

        [Fact]
        public void no_diff_if_GestureRecognizer_is_not_changed()
        {
            var diff = Diff.Calculate(
                new ContentView { GestureRecognizers = { [0] = new TapGestureRecognizer { NumberOfTapsRequired = 2} } },
                new ContentView { GestureRecognizers = { [0] = new TapGestureRecognizer { NumberOfTapsRequired = 2} } },
                NoopExpander
            );

            diff.Count().ShouldBe(0);
        }

        // This class is here to have primitive values wrapped, not used directly.
        // This is more realistic scenario, and might help catching edge cases
        class StateWrapper
        {
            public readonly int Value;
            public StateWrapper(int val) => Value = val;
            public override string ToString() => Value.ToString();
        }

        [Fact]
        public void transfer_GestureRecognizer_subscription()
        {
            var binder = Binder.CreateForTest(new StateWrapper(1), (s, g) => 
                new StateWrapper(s.Value + (int)g.Payload)
            );
            
            var view = binder.CreateElement(s => new ContentView {
                GestureRecognizers = {
                    [0] = new TapGestureRecognizer {
                        Tapped = () => new Signal(s.Value)
                    }
                }
            });

            (view.GestureRecognizers[0] as xf.TapGestureRecognizer).SendTapped(view);
            binder.State.Value.ShouldBe(2);
            
            (view.GestureRecognizers[0] as xf.TapGestureRecognizer).SendTapped(view);
            binder.State.Value.ShouldBe(4);
        }
    }
}