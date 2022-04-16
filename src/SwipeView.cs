namespace Laconic;

public partial class SwipeView : IContentHost
{
    public SwipeView()
    {
        ElementLists.Add<Xamarin.Forms.SwipeView>(nameof(LeftItems), v => new SwipeItemsAdapter(v.LeftItems));
        ElementLists.Add<Xamarin.Forms.SwipeView>(nameof(RightItems), v => new SwipeItemsAdapter(v.RightItems));
        ElementLists.Add<Xamarin.Forms.SwipeView>(nameof(TopItems), v => new SwipeItemsAdapter(v.TopItems));
        ElementLists.Add<Xamarin.Forms.SwipeView>(nameof(BottomItems), v => new SwipeItemsAdapter(v.BottomItems));
    }

    public ElementList LeftItems => ElementLists[nameof(LeftItems)];
    public ElementList RightItems => ElementLists[nameof(RightItems)];
    public ElementList TopItems => ElementLists[nameof(TopItems)];
    public ElementList BottomItems => ElementLists[nameof(BottomItems)];

    public View? Content { get; set; }

    public Func<Xamarin.Forms.SwipeStartedEventArgs, Signal> SwipeStarted {
        init => SetEvent(nameof(SwipeStarted), value,
            (ctl, handler) => ctl.SwipeStarted += handler,
            (ctl, handler) => ctl.SwipeStarted -= handler);
    }

    public Func<Xamarin.Forms.SwipeEndedEventArgs, Signal> SwipeEnded {
        init => SetEvent(nameof(SwipeEnded), value,
            (ctl, handler) => ctl.SwipeEnded += handler,
            (ctl, handler) => ctl.SwipeEnded -= handler);
    }

    public Func<Xamarin.Forms.SwipeChangingEventArgs, Signal> SwipeChanging {
        init => SetEvent(nameof(SwipeChanging), value,
            (ctl, handler) => ctl.SwipeChanging += handler,
            (ctl, handler) => ctl.SwipeChanging -= handler);
    }

    public Func<Xamarin.Forms.OpenRequestedEventArgs, Signal> OpenRequested {
        init => SetEvent(nameof(OpenRequested), value, 
            (ctl, handler) => ctl.OpenRequested += handler,
            (ctl, handler) => ctl.OpenRequested -= handler);
    }
        
    public Func<Xamarin.Forms.CloseRequestedEventArgs, Signal> CloseRequested {
        init => SetEvent(nameof(CloseRequested), value, 
            (ctl, handler) => ctl.CloseRequested += handler,
            (ctl, handler) => ctl.CloseRequested -= handler);
    }
}
    
public partial class SwipeItem : Element<xf.SwipeItem>
{
    // TODO: this properties are up the hierarchy in XF
    public bool IsEnabled {
        init => SetValue(xf.MenuItem.IsEnabledProperty, value);
    }

    public bool IsDestructive {
        init => SetValue(xf.MenuItem.IsDestructiveProperty, value);
    }

    public string Text {
        init => SetValue(xf.MenuItem.TextProperty, value);
    }

    public ImageSource IconImageSource {
        init => SetValue(xf.MenuItem.IconImageSourceProperty, value);
    }

    // TODO: this event is declared EventHandler<EventArgs> Invoked, but not EventHandler Invoked ?
    public Func<EventArgs, Signal> Invoked {
        init => SetEvent(nameof(Invoked), value,
            (ctl, handler) => ctl.Invoked += handler,
            (ctl, handler) => ctl.Invoked -= handler);
    }

    protected internal override xf.BindableObject CreateView() => new xf.SwipeItem();
}

class SwipeItemsAdapter : IList
{
    readonly xf.SwipeItems _source;

    public SwipeItemsAdapter(xf.SwipeItems source) => _source = source;

    public IEnumerator GetEnumerator() => throw new NotImplementedException(); //_source.GetEnumerator();

    public void CopyTo(Array array, int index) => throw new NotImplementedException();

    public int Count => _source.Count;

    public bool IsSynchronized => throw new NotImplementedException();
    public object SyncRoot => throw new NotImplementedException();
    public int Add(object value) => throw new NotImplementedException();
    public void Clear() => throw new NotImplementedException();
    public bool Contains(object value) => throw new NotImplementedException();
    public int IndexOf(object value) => throw new NotImplementedException();

    public void Insert(int index, object value) => _source.Insert(index, (xf.ISwipeItem) value);

    public void Remove(object value) => throw new NotImplementedException();

    public void RemoveAt(int index) => _source.RemoveAt(index);

    public bool IsFixedSize { get; } = false;
    public bool IsReadOnly { get; } = false;

    public object this[int index] {
        get => _source[index];
        set => throw new NotImplementedException();
    }
}