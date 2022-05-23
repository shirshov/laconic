using Microsoft.Maui.Controls;

namespace Laconic;

public class Shadow : Element<xf.Shadow>
{
    public float Radius {
        init => SetValue(xf.Shadow.RadiusProperty, value);
    }

    public float Opacity {
        init => SetValue(xf.Shadow.OpacityProperty, value);
    }

    public IBrush Brush {
        init => SetValue(xf.Shadow.BrushProperty, value);
    }

    public Point Offset {
        init => SetValue(xf.Shadow.OffsetProperty, value);
    }

    protected internal override BindableObject CreateView() => new xf.Shadow();
}

public abstract partial class VisualElement<T> : Element<T> where T : xf.VisualElement, new()
{
    // TODO: why is it here, and not on Element<T>?
    protected internal override xf.BindableObject CreateView() => new T();
        
    public Dictionary<Key, IGestureRecognizer> GestureRecognizers { get; } = new();

    public VisualElement() => ElementLists.Add<xf.VisualElement>(nameof(Behaviors), element => (IList)element.Behaviors);

    public VisualMarker Visual
    {
        get => GetValue<VisualMarker>(xf.VisualElement.VisualProperty);
        set => SetValue(xf.VisualElement.VisualProperty, value);
    }
        
    public IBrush Background
    {
        get => GetValue<IBrush>(xf.VisualElement.BackgroundProperty);
        set => SetValue(xf.VisualElement.BackgroundProperty, value);
    }
        
    public ElementList Behaviors {
        get => ElementLists[nameof(Behaviors)];
        set => ElementLists[nameof(Behaviors)] = value;
    }
}