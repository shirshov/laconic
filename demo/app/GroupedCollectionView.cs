using ChanceNET;

namespace Laconic.Demo;

static class GroupedCollectionView
{
    static View SectionHeaderRow(string text) => new Grid
    {
        Padding = (15, 0),
        HeightRequest = 40,
        ["letter"] = new Label
        {
            Text = text,
            FontSize = 18,
            FontAttributes = FontAttributes.Bold,
            BackgroundColor = Color.Chocolate,
            TextColor = Color.White,
            WidthRequest = 40,
            HorizontalOptions = LayoutOptions.Start,
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalTextAlignment = TextAlignment.Center,
        },
        ["underline"] = new BoxView
        {
            BackgroundColor = Color.Chocolate, HeightRequest = 2, VerticalOptions = LayoutOptions.End
        }
    };

    static View ItemRow(string name, string phone) => new StackLayout
    {
        Orientation = StackOrientation.Horizontal,
        Padding = (30, 0),
        HeightRequest = 30,
        ["name"] = new Label {Text = name},
        ["phone"] = new Label
        {
            Text = phone, TextColor = Color.Gray, HorizontalOptions = LayoutOptions.EndAndExpand
        }
    };

    // A helper function that converts a sequence "Alice", "Bob"
    // to "A", "Alice", "B", "Bob" and creates corresponding blueprints 
    static IEnumerable<(string ReuseKey, string Key, View View)> GroupedItems(IEnumerable<Person> state)
    {
        var grouped = state.ToLookup(x => x.LastName.Substring(0, 1), x => x);
        foreach (var group in grouped.OrderBy(x => x.Key))
        {
            yield return ("header", group.Key, SectionHeaderRow(group.Key.ToUpper()));
            foreach (var item in group.OrderBy(x => x.LastName))
                yield return ("item", item.GetHashCode().ToString(),
                    ItemRow(item.LastName + ", " + item.FirstName, item.Phone));
        }
    }

    public static Person[] InitialState()
    {
        var chance = new Chance();
        return Enumerable.Range(1, 200).Select(_ => chance.Person()).ToArray();
    }
        
    public static StackLayout Content(IEnumerable<Person> state) => new StackLayout {
        ["list"] = new CollectionView
        {
            Items = GroupedItems(state).ToItemsList(x => x.ReuseKey, x => x.Key, x => x.View)
        }
    };
}