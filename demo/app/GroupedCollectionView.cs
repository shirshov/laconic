using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using ChanceNET;
using xf = Xamarin.Forms;

namespace Laconic.Demo
{
    public class GroupedCollectionView : xf.ContentPage
    {
        Binder<ImmutableList<Person>> _binder;

        // TODO: implement adding people
        static ImmutableList<Person> Reducer(ImmutableList<Person> state, Signal signal) => signal switch
        {
            ("add", Person val) => state.Add(val),
            _ => state
        };

        static View SectionHeaderRow(string text) => new Grid
        {
            Padding = new xf.Thickness(15, 0),
            HeightRequest = 40,
            ["letter"] = new Label
            {
                Text = text,
                FontSize = 18,
                FontAttributes = xf.FontAttributes.Bold,
                BackgroundColor = xf.Color.Chocolate,
                TextColor = xf.Color.White,
                WidthRequest = 40,
                HorizontalOptions = xf.LayoutOptions.Start,
                VerticalTextAlignment = xf.TextAlignment.Center,
                HorizontalTextAlignment = xf.TextAlignment.Center,
            },
            ["underline"] = new BoxView
            {
                BackgroundColor = xf.Color.Chocolate, HeightRequest = 2, VerticalOptions = xf.LayoutOptions.End
            }
        };

        static View ItemRow(string name, string phone) => new StackLayout
        {
            Orientation = xf.StackOrientation.Horizontal,
            Padding = new xf.Thickness(30, 0),
            HeightRequest = 30,
            ["name"] = new Label {Text = name},
            ["phone"] = new Label
            {
                Text = phone, TextColor = xf.Color.Gray, HorizontalOptions = xf.LayoutOptions.EndAndExpand
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

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var chance = new Chance();
            var list = Enumerable.Range(1, 200).Select(_ => chance.Person());

            _binder = Binder.Create(ImmutableList.CreateRange(list), Reducer);

            Content = _binder.CreateView(state => new StackLayout
            {
                ["list"] = new CollectionView
                {
                    Items = GroupedItems(state).ToItemsList(x => x.ReuseKey, x => x.Key, x => x.View)
                }
            });
        }
    }
}