namespace Laconic;

public interface IGestureRecognizer
{
}

public class TapGestureRecognizer : Element<xf.TapGestureRecognizer>, IGestureRecognizer
{
    public int NumberOfTapsRequired
    {
        set => SetValue(xf.TapGestureRecognizer.NumberOfTapsRequiredProperty, value);
    }

    public Func<xf.TappedEventArgs, Signal> Tapped
    {
        set => SetEvent(nameof(Tapped), value,
            (ctl, handler) => ctl.Tapped += handler,
            (ctl, handler) => ctl.Tapped -= handler);
    }

    protected internal override xf.BindableObject CreateView() => new xf.TapGestureRecognizer();
}