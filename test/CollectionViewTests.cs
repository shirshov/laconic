namespace Laconic.Tests;

public class CollectionViewTests
{
    [Fact]
    public void should_create_ItemsSource()
    {
        var colView = new xf.CollectionView();
        Patch.Apply(colView,
            Diff.Calculate(null,
                new CollectionView
                {
                    Items = {["key1"] = new Label {Text = "One"}, ["key2"] = new Label {Text = "Two"},}
                }), _ => { });
        var source = (IList<BindingContextItem>) colView.ItemsSource;

        source[0].Blueprint.ShouldBeOfType<Label>().Text.ShouldBe("One");
        source[1].Blueprint.ShouldBeOfType<Label>().Text.ShouldBe("Two");
    }

    [Fact]
    public void should_add_an_item_to_ItemsSource()
    {
        var colView = new xf.CollectionView();
        var blueprint = new CollectionView {Items = {["key1"] = new Label {Text = "One"},}};
        Patch.Apply(colView, Diff.Calculate(null, blueprint), _ => { });

        Patch.Apply(colView,
            Diff.Calculate(blueprint,
                new CollectionView
                {
                    Items = {["key1"] = new Label {Text = "One"}, ["key2"] = new Label {Text = "Two"},}
                }), _ => { });

        var source = (IList<BindingContextItem>) colView.ItemsSource;

        source[0].Blueprint.ShouldBeOfType<Label>().Text.ShouldBe("One");
        source[1].Blueprint.ShouldBeOfType<Label>().Text.ShouldBe("Two");
    }

    [Fact]
    public void should_update_item()
    {
        var colView = new xf.CollectionView();
        var original = new CollectionView
        {
            Items = {["key1"] = new Label {Text = "One"}, ["key2"] = new Label {Text = "Two"}}
        };
        Patch.Apply(colView, Diff.Calculate(null, original), _ => { });

        Patch.Apply(colView,
            Diff.Calculate(original,
                new CollectionView
                {
                    Items = {["key1"] = new Label {Text = "One"}, ["key2"] = new Label {Text = "Two updated"},}
                }), _ => { });

        var source = (IList<BindingContextItem>) colView.ItemsSource;

        source[0].Blueprint.ShouldBeOfType<Label>().Text.ShouldBe("One");
        source[1].Blueprint.ShouldBeOfType<Label>().Text.ShouldBe("Two updated");
    }

    [Fact]
    public void should_set_ReuseKey()
    {
        var colView = new xf.CollectionView();
        Patch.Apply(colView,
            Diff.Calculate(null,
                new CollectionView {Items = {["label", "key1"] = new Label(), ["button", "key2"] = new Button()}}),
            _ => { });
        var source = (IList<BindingContextItem>) colView.ItemsSource;

        source[0].Blueprint.ShouldBeOfType<Label>();
        source[0].ReuseKey.ShouldBe("label");
        source[1].ReuseKey.ShouldBe("button");
    }
}