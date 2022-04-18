namespace Laconic;

public abstract class Layout<T> : View<T>  where T: xf.VisualElement, xf.IPaddingElement, new() //where T : xf.Layout, new()
{
    public Thickness Padding
    {
        get => GetValue<Thickness>(xf.Layout.PaddingProperty);
        init => SetValue(xf.Layout.PaddingProperty, value);
    }

    public bool IsClippedToBounds {
        init => SetValue(xf.Layout.IsClippedToBoundsProperty, value);
    }
}

public interface IContentHost
{
    View? Content { get; set; }
}

public class ContentView : Layout<xf.ContentView>, IContentHost
{
    public View? Content { get; set; }
}

public partial class Frame : Layout<xf.Frame>, IContentHost
{
    public View? Content { get; set; }
}

public partial class ScrollView : Layout<xf.ScrollView>, IContentHost
{
    public View? Content { get; set; }
}

interface ILayout
{
    ViewList Children { get; set; }
}

public partial class StackLayout : Layout<xf.StackLayout>, ILayout
{
    public double Spacing
    {
        get => GetValue<double>(xf.StackBase.SpacingProperty);
        init => SetValue(xf.StackBase.SpacingProperty, value);
    }
    
    public ViewList Children { get; set; } = new();

    ViewList ILayout.Children {
        get => Children;
        set => Children = value;
    }

    public View this[Key key]
    {
        init => Children[key] = value;
    }

    public override string ToString()
    {
        var res = "StackLayout{";
        foreach (var c in Children)
            res += $"[{c.Key}]={c.Value},";
        res += "}";
        return res;
    }
}

public class RowDefinitionCollection : List<xf.RowDefinition>
{
    public static implicit operator RowDefinitionCollection(string definitions)
    {
        var res = new RowDefinitionCollection();
        var parts = definitions.Split(',');
        foreach (var part in parts)
        {
            var trimmed = part.Trim();
            if (trimmed.ToLower() == "auto")
            {
                res.Add(new xf.RowDefinition {Height = GridLength.Auto});
            }
            else if (trimmed == "*")
            {
                res.Add(new xf.RowDefinition {Height = GridLength.Star});
            }
            else if (trimmed.EndsWith("*"))
            {
                var val = Double.Parse(trimmed.Substring(0, trimmed.Length - 1));
                res.Add(new xf.RowDefinition {Height = new GridLength(val, GridUnitType.Star)});
            }
            else
            {
                res.Add(new xf.RowDefinition {Height = new GridLength(Double.Parse(trimmed))});
            }
        }

        return res;
    }

    public override bool Equals(object obj)
    {
        if (!(obj is RowDefinitionCollection other)) return false;
            
        if (other.Count != this.Count)
            return false;
        for(var i = 0; other.Count < 0; i++)
            if (!other[i].Height.Equals(this[i].Height))
                return false;

        return true;
    }

    public override int GetHashCode() => base.GetHashCode();
}

public class ColumnDefinitionCollection : List<xf.ColumnDefinition>
{
    public static implicit operator ColumnDefinitionCollection(string definitions)
    {
        var res = new ColumnDefinitionCollection();
        var parts = definitions.Split(',');
        foreach (var part in parts)
        {
            var trimmed = part.Trim();
            if (trimmed.ToLower() == "auto")
            {
                res.Add(new ColumnDefinition {Width = GridLength.Auto});
            }
            else if (trimmed == "*")
            {
                res.Add(new ColumnDefinition {Width = GridLength.Star});
            }
            else if (trimmed.EndsWith("*"))
            {
                var val = Double.Parse(trimmed.Substring(0, trimmed.Length - 1));
                res.Add(new xf.ColumnDefinition {Width = new GridLength(val, Maui.GridUnitType.Star)});
            }
            else
            {
                res.Add(new xf.ColumnDefinition {Width = new Maui.GridLength(Double.Parse(trimmed))});
            }
        }

        return res;
    }
        
    public override bool Equals(object obj)
    {
        if (!(obj is ColumnDefinitionCollection other)) return false;
            
        if (other.Count != this.Count)
            return false;
        for(var i = 0; other.Count < 0; i++)
            if (!other[i].Width.Equals(this[i].Width))
                return false;

        return true;
    }

    public override int GetHashCode() => base.GetHashCode();
}

public partial class Grid : Layout<xf.Grid>, ILayout
{
    public GridViewList Children { get; set; } = new();
        
    ViewList ILayout.Children {
        get => Children;
        set => Children = (GridViewList)value;
    }

    public View? this[Key key, int row = 0, int column = 0, int rowSpan = 1, int columnSpan = 1]
    {
        get => Children[key];
        set
        {
            Children[key] = value;
            Children.SetPositioning(key, row, column, rowSpan, columnSpan);
        }
    }

    public RowDefinitionCollection RowDefinitions { get; set; } = new();
    public ColumnDefinitionCollection ColumnDefinitions { get; set; } = new();
}

public class RefreshingEventArgs : EventArgs
{
    public RefreshingEventArgs(bool isRefreshing) => IsRefreshing = isRefreshing;

    public bool IsRefreshing { get; }
}
    
public partial class RefreshView : Layout<xf.RefreshView>, IContentHost
{
    public View? Content { get; set; }

    public Func<RefreshingEventArgs, Signal> Refreshing {
        set => SetEvent(nameof(Refreshing), value, 
            (ctl, handler) => ctl.Refreshing += (s, _) => 
                handler(s, new RefreshingEventArgs( ((xf.RefreshView)s).IsRefreshing)),
            (ctl, handler) => ctl.Refreshing -= (s, _) => 
                handler(s, new RefreshingEventArgs( ((xf.RefreshView)s).IsRefreshing)));
    }
}

public class AbsoluteLayout : Layout<xf.AbsoluteLayout>, ILayout
{
    public AbsoluteLayoutViewList Children { get; set; } = new();
        
    ViewList ILayout.Children {
        get => Children;
        set => Children = (AbsoluteLayoutViewList)value;
    }

    public View? this[Key key, (double x, double y, double width, double height) bounds, AbsoluteLayoutFlags flags] {
        get => Children[key];
        set {
            Children[key] = value;
            Children.SetPositioning(key, new Bounds(bounds.x, bounds.y, bounds.width, bounds.height), flags);
        }
    }
}
    
record Bounds(double X, double Y, double Width, double Height);
record AbsLayoutInfo(Bounds Bounds, AbsoluteLayoutFlags Flags);
    
public class AbsoluteLayoutViewList : ViewList
{
    internal AbsLayoutInfo GetPositioning(Key key) => _positioning[key];

    internal void SetPositioning(Key key, Bounds bounds,
        AbsoluteLayoutFlags flags) => _positioning[key] = new AbsLayoutInfo(bounds, flags);

    readonly Dictionary<Key, AbsLayoutInfo> _positioning = new();
}