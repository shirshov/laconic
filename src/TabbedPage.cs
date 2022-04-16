namespace Laconic;

public class CurrentPageChangedEventArgs : EventArgs
{
    public int Index { get; }

    public CurrentPageChangedEventArgs(int index) => Index = index;
}
    
public partial class TabbedPage : Page<Xamarin.Forms.TabbedPage>
{
    public TabbedPage() => ElementLists.Add<Xamarin.Forms.TabbedPage>(nameof(Children), tp => (IList) tp.Children);
        
    public ElementList Children => ElementLists[nameof(Children)];

    public int CurrentPage {
        init => SetValue(nameof(CurrentPage), value, tp => {
            tp.CurrentPage = tp.Children[value];
        });
    }
        
    public Page this[Key key]
    {
        init => Children[key] = (Element)value;
    }

    EventHandler<CurrentPageChangedEventArgs>? _handler;

    void OnCurrentPageChanged(object sender, EventArgs args)
    {
        var page = (Xamarin.Forms.TabbedPage) sender;
        var index = page.Children.IndexOf(page.CurrentPage);
        if (index != -1)
            _handler!(page, new CurrentPageChangedEventArgs(index));
    }

    public Func<CurrentPageChangedEventArgs, Signal> CurrentPageChanged {
        init => SetEvent(nameof(CurrentPageChanged), value,
            (ctl, handler) => {
                _handler = handler;
                ctl.CurrentPageChanged += OnCurrentPageChanged;
            },
            (ctl, _) => {
                _handler = null;
                ctl.CurrentPageChanged -= OnCurrentPageChanged;
            });
    }
}