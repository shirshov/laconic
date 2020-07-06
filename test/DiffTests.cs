using System.Linq;
using Xunit;
using Shouldly;
using xf = Xamarin.Forms;

namespace Laconic.Tests
{
    public class DiffTests
    {
        [Fact]
        public void should_update_property()
        {
            var operations = Diff.Calculate(new Label {Text = "foo"}, new Label {Text = "bar"});
            operations.Count().ShouldBe(1);
            var op = operations.First() as SetProperty;
            op.Property.ShouldBe(xf.Label.TextProperty);
            op.Value.ShouldBe("bar");
        }

        [Fact]
        public void should_reset_property()
        {
            var operations = Diff.Calculate(new Label {Text = "foo"}, new Label());
            operations.First()
                .ShouldBeOfType<ResetProperty>()
                .Property.ShouldBe(xf.Label.TextProperty);
        }

        [Fact]
        public void ignore_unchanged_vales()
        {
            var operations = Diff.Calculate(new Label {Text = "a"}, new Label {Text = "a"});
            operations.Count().ShouldBe(0);
        }

        [Fact]
        public void should_set_content()
        {
            var diff = Diff.Calculate(
                new ContentView(),
                new ContentView {Content = new Label {Text = "New"}}
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
            var diff = Diff.Calculate(null, new ContentPage {Content = new Label()}).ToArray();
            diff[0].ShouldBeOfType<SetContent>().ContentView.ShouldBeOfType<Label>();
        }

        [Fact]
        public void should_replace_content()
        {
            var diff = Diff.Calculate(
                new ContentView {Content = new Label()},
                new ContentView {Content = new Button()}
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
                new ContentView()
            );

            diff.First().ShouldBeOfType<RemoveContent>();
        }

        [Fact]
        public void ingore_null_content()
        {
            var diff = Diff.Calculate(null, new ContentView());

            diff.Count().ShouldBe(0);
        }

        [Fact]
        public void calc_diff_for_first_view()
        {
            var diff = Diff.Calculate(
                null,
                new ContentPage {Title = "Root Page"}
            );
            diff.First().ShouldBeOfType<SetProperty>().Value.ShouldBe("Root Page");
        }

        [Fact]
        public void should_update_content()
        {
            var diff = Diff.Calculate(
                new ContentView {Content = new Label {Text = "Foo"}},
                new ContentView {Content = new Label {Text = "Bar"}}
            );
            var upd = diff.First().ShouldBeOfType<UpdateContent>();
            var setProp = upd.Operations.First().ShouldBeOfType<SetProperty>();
            setProp.Property.ShouldBe(xf.Label.TextProperty);
            setProp.Value.ShouldBe("Bar");
        }

        [Fact]
        public void ignore_child_set_to_null()
        {
            var diff = Diff.Calculate(null, new StackLayout {["null"] = null});
            diff.Count().ShouldBe(0);
        }

        [Fact]
        public void remove_child_set_to_null()
        {
            var diff = Diff.Calculate(new StackLayout {["1"] = new Label()}, new StackLayout {["1"] = null});

            diff.Count().ShouldBe(1);
            diff.First().ShouldBeOfType<UpdateChildren>()
                .Operations.First().ShouldBeOfType<RemoveChild>()
                .Index.ShouldBe(0);
        }

        [Fact]
        public void replace_null_view_with_instance()
        {
            var diff = Diff.Calculate(new StackLayout {["1"] = null}, new StackLayout {["1"] = new Label()});
        }

        [Fact]
        public void add_children_to_new_view()
        {
            var diff = Diff.Calculate(
                null,
                new StackLayout {[1] = new Label(), [2] = new Label()}
            ).ToArray();

            var updateChildren = diff[0].ShouldBeOfType<UpdateChildren>();

            var addOne = updateChildren.Operations[0].ShouldBeOfType<AddChild>();
            addOne.Index.ShouldBe(0);
            addOne.View.ShouldBeOfType<Label>();

            var addTwo = updateChildren.Operations[1].ShouldBeOfType<AddChild>();
            addTwo.Index.ShouldBe(1);
            addTwo.View.ShouldBeOfType<Label>();
        }

        [Fact]
        public void add_second_child()
        {
            var diff = Diff.Calculate(
                new StackLayout {["first"] = new Label()},
                new StackLayout {["first"] = new Label(), ["second"] = new Button()}
            ).ToArray();

            diff[0].ShouldBeOfType<UpdateChildren>()
                .Operations[0].ShouldBeOfType<AddChild>()
                .Index.ShouldBe(1);
        }

        [Fact]
        public void wire_event()
        {
            var diff = Diff.Calculate(
                new Button(),
                new Button {Clicked = () => new Signal<string>("")}).ToArray();

            diff[0].ShouldBeOfType<SetEvent>().EventName.ShouldBe("Clicked");
        }

        [Fact]
        public void unwire_event()
        {
            var diff = Diff.Calculate(
                new Button {Clicked = () => new Signal<string>("")},
                new Button()).ToArray();

            diff[0].ShouldBeOfType<UnsetEvent>().EventName.ShouldBe("Clicked");
        }

        
        [Fact]
        public void empty_diff_if_Grid_ColumnDefinitions_identical()
        {
            var diff = Diff.Calculate(
                new Grid { ColumnDefinitions = "1, 2" },
                new Grid { ColumnDefinitions = "1, 2" }
            );
            
            diff.ShouldBeEmpty();
        }
        
        [Fact]
        public void empty_diff_if_Grid_RowDefinitions_identical()
        {
            var diff = Diff.Calculate(
                new Grid { RowDefinitions = "1, 2" },
                new Grid { RowDefinitions = "1, 2" }
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
                }).ToArray();

            var ops = diff[0].ShouldBeOfType<UpdateChildren>().Operations;

            ops[0].ShouldBeOfType<AddChild>().View.ShouldBeOfType<Label>();
            var labelPos = ops[0].ShouldBeOfType<AddChild>().Operations[0].ShouldBeOfType<GridPositionChange>();
            labelPos.Type.ShouldBe(GridPositionChangeType.Column);
            labelPos.Value.ShouldBe(1);
        }

        [Fact]
        public void add_grid_child()
        {
            var diff = Diff.Calculate(
                new Grid {[1] = new BoxView()},
                new Grid {[1] = new BoxView(), [2] = new Label()}
            ).ToArray();

            var ops = diff[0].ShouldBeOfType<UpdateChildren>().Operations;
            ops[0].ShouldBeOfType<AddChild>().View.ShouldBeOfType<Label>();
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
                }).ToArray();

            var updateGrid = diff[0].ShouldBeOfType<UpdateChildren>().Operations;

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
        public void calc_diff_for_ToolbarItems()
        {
            var diff = Diff.Calculate(
                new ContentPage {
                    ToolbarItems = {[1] = new ToolbarItem {IconImageSource = "foo"}}
                },
                new ContentPage {
                    ToolbarItems = {[1] = new ToolbarItem {IconImageSource = "bar"}}
                }
            );
            
            diff.First().ShouldBeOfType<SetToolbarItems>();
        }
    }
}