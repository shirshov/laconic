namespace Laconic.Demo;

public static class Navigation
{
    enum PageType { Root, Details, Modal } 
    enum SignalType { Push, PushModal, Pop, PopToRoot, RemoveFromStack }
        
    public record State(NavigationStack Stack, int RunningNumber);
        
    public static State InitialState() => new(new NavigationStack(PageType.Root), 0);

    public static State Reducer(State state, Signal signal)
    {
        var stack = state.Stack;
        var newCount = state.RunningNumber + 1;
        return signal switch {
            (SignalType.Push, _) => state with { Stack = stack.Push((PageType.Details, newCount)), RunningNumber = newCount },
            (SignalType.PushModal, _) => state with {
                Stack = stack.PushModal(PageType.Modal) },
            (SignalType.Pop, _) => state with { Stack = stack.Pop() },
            (SignalType.PopToRoot, _) => InitialState(),
            (SignalType.RemoveFromStack, object data) =>  state with { Stack = stack.Remove(data) },
            _ => state
        };
    }

    static FontImageSource FontIcon(string glyph) => new() { Glyph = glyph, FontFamily = "IconFont", Color = Color.Chocolate};
        
    static Button Button(string text, Func<Signal> signal) => new() {
        Text = text,
        Clicked = signal,
        BorderColor = Color.Chocolate,
        BackgroundColor = Color.Chocolate,
        TextColor = Color.White,
        CornerRadius = 20,
        HeightRequest = 40,
    };

    static Label Legend(string text) => new() {
        Text = text,
        HorizontalOptions = LayoutOptions.Center,
        HorizontalTextAlignment = TextAlignment.Center,
        TextColor = Color.Gray
    };
        
    static ContentPage RootPage() => new() {
        TitleView = new SearchBar { Margin = 5 },
        Title = "Root",
        BackButtonTitle = "",
        ToolbarItems = {
            ["bell"] = new ToolbarItem {
                IconImageSource = FontIcon("\uf0f3")
            }
        },
        Content = new VerticalStackLayout {
            Padding = 20,
            Spacing = 10,
            ["push-modeless"] = Button("Push", () => new Signal(SignalType.Push)),
            ["push-modal"] = Button("Push Modal", () => new Signal(SignalType.PushModal)),
            ["explain"] = Legend("This tab hosts a NavigationPage")
        }
    };

    static ContentPage DetailsPage(string subtitle) => new() {
        Title = $"Details/{subtitle}",
        BackButtonTitle = "",
        Content = new VerticalStackLayout {
            Padding = 20,
            Spacing = 20,
            ["push-modeless"] = Button("Push", () => new Signal(SignalType.Push)),
            ["push-modal"] = Button("Push Modal", () => new Signal(SignalType.PushModal)),
            ["pop"] = Button("Pop", () => new Signal(SignalType.Pop)),
            ["pop-to-root"] = Button("Pop To Root", () => new Signal(SignalType.PopToRoot)),
        }
    };

    static ContentPage ModalPage() => new() {
        Content = new Grid {
            Padding = (20, 20),
            RowDefinitions = "*,Auto",
            ["title"] = new Label {
                Text = "Modal Page", 
                FontSize = 20,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.Start,
                HorizontalOptions = LayoutOptions.Center,
            },
            ["btn", row: 1] = Button("Close", () => new Signal(SignalType.Pop))
        }
    };

    static ContentPage EditPage(NavigationStack stack)
    {
        var items = new ItemsViewList();

        items.Add("header-row-reuse-key", "header-row-key",
            new ContentView {
                Padding = (20, 0),
                Content = new Label {
                    Text = "Root",
                }
            }
        );
            
        foreach (var stackData in stack.Skip(1)) {
            items.Add("page-row-reuse-key", $"{stackData}",
                new Grid {
                    Padding = (20, 0),
                    ColumnDefinitions = "*, Auto",
                    ["navstack-data"] = new Label {
                        Text = stackData.ToString(),
                        VerticalOptions = LayoutOptions.Center
                    },
                    ["remove-btn", column: 1] = new Button {
                        Text = "Remove", 
                        Clicked = () => new Signal(SignalType.RemoveFromStack, stackData),
                        TextColor = Color.Chocolate,
                        BorderColor = Color.Chocolate,
                        BorderWidth = 1,
                        CornerRadius = 15,
                        HeightRequest = 30,
                        Margin = 10
                    }
                }
            );
        }
            
        items.Add("add-button-row-reuse-key", "add-button-row-key",
            new ContentView {
                Padding = 20,
                Content = Button("Push", () => new Signal(SignalType.Push))
            });

        items.Add("legend-row-reuse-key, ", "legend-row-reuse-key",
            new ContentView {
                Padding = (20, 0),
                Content = Legend("This tabs hosts a ContentPage with controls for manipulating the stack of the NavigationPage on the Home tab")
            }
        );
            
        return new ContentPage { 
            Title = "Edit",
            IconImageSource = FontIcon("\uf013"),
            Padding = (0, 50, 0, 0),
            Content = new CollectionView { Items = items } 
        };
    }

    static ContentPage PageFactory(object data) => data switch {
        PageType.Root => RootPage(),
        (PageType.Details, int level) => DetailsPage(level.ToString()),
        PageType.Modal => ModalPage(),
    };

    // class AndroidBottomTabs : Behavior<xf.TabbedPage>
    // {
    //     protected override void OnAttachedTo(Xamarin.Forms.TabbedPage bindable) => 
    //         bindable.On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
    // }
        
    public static TabbedPage TabbedPage(State state) => new() {
        BarBackgroundColor = Color.Gainsboro,
        BarTextColor = Color.Chocolate,
        SelectedTabColor = Color.Chocolate,
        // Behaviors = { 
        //     ["bottom-tabs"] = new AndroidBottomTabs(),  
        // },
        ["home-tab"] = new NavigationPage(state.Stack, PageFactory) {
            Title = "Home",
            BarBackgroundColor = Color.White,
            BarTextColor = Color.Chocolate,
            IconImageSource = FontIcon("\uf015")
        },
        ["edit-tab"] = EditPage(state.Stack)
    };
}