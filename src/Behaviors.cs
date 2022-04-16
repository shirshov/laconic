namespace Laconic;

public abstract class Behavior : Element
{
    public static readonly xf.BindableProperty ValueProperty = xf.BindableProperty.Create(
        "Value",
        typeof(object),
        typeof(xf.Behavior));
}

public abstract class Behavior<T> : Behavior where T : Xamarin.Forms.VisualElement
{
    protected Behavior()
    {
    }
        
    protected Behavior(object? value) => SetValue(ValueProperty, value);

    protected internal override xf.BindableObject CreateView() => new BehaviorAdapter<T>(this);

    protected internal abstract void OnAttachedTo(T bindable);

    protected internal virtual void OnDetachingFrom(T bindable)
    {
    }
        
    protected internal virtual void OnValuesUpdated(object value)
    {
    }
}

class BehaviorAdapter<T> : xf.Behavior<T> where T : xf.VisualElement
{
    readonly Behavior<T> _internal;
        
    public BehaviorAdapter(Behavior<T> behavior) => _internal = behavior;

    bool _isAttached;
        
    protected override void OnAttachedTo(T bindable)
    {
        _internal.OnAttachedTo(bindable);
        _isAttached = true;
    }

    protected override void OnDetachingFrom(T bindable)
    {
        _internal.OnDetachingFrom(bindable);
        _isAttached = false;
    }

    protected override void OnPropertyChanged(string propertyName = null)
    {
        if (propertyName == Behavior.ValueProperty.PropertyName && _isAttached) {
            _internal.OnValuesUpdated(GetValue(Behavior.ValueProperty));
        }
    }
}