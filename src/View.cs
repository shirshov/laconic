namespace Laconic;

public interface View 
{
    Dictionary<Key, IGestureRecognizer> GestureRecognizers { get; }
}
public abstract class View<T> : VisualElement<T>, View where T: xf.VisualElement, Maui.IView, new()
{
    public LayoutOptions HorizontalOptions {
        init => SetValue(xf.View.HorizontalOptionsProperty, value);
    }

    public LayoutOptions VerticalOptions {
        init => SetValue(xf.View.VerticalOptionsProperty, value);
    }

    public Thickness Margin {
        init => SetValue(xf.View.MarginProperty, value);
    }
}