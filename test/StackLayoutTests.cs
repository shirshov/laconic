namespace Laconic.Tests;

public class StackLayoutTests
{
    [Fact]
    public void can_add_children()
    {
        var sl = new VerticalStackLayout {
            [1] = new Label { Text = "lbl1"},
            [2] = new Label { Text = "lbl2" }
        };

        sl.Children.Count().ShouldBe(2);

        var binder = Binder.Create("state", (state, _) => state);
        var real = binder.CreateElement(state => new VerticalStackLayout {
            ["1"] = new Label { Text = "lbl1"},
            ["2"] = new Label { Text = "lbl2" }
        });

        real.Children.Count.ShouldBe(2);
        real.Children[0].ShouldBeOfType<xf.Label>();
        real.Children[1].ShouldBeOfType<xf.Label>();
    }

    [Fact]
    public void children_from_LINQ()
    {
        var s = new VerticalStackLayout {
            Children = Enumerable.Range(1, 5).ToViewList(i => i, i => new Label {Text = "Item " + i})
        };
    }
}