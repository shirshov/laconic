using System;
using System.Collections.Generic;
using System.Linq;
using xf = Xamarin.Forms;
using Evt = System.Linq.Expressions.Expression<System.Func<Laconic.Signal>>;

namespace Laconic
{
    public abstract class Layout<T> : View<T> where T : xf.Layout, new()
    {
        public xf.Thickness Padding
        {
            get => GetValue<xf.Thickness>(xf.Layout.PaddingProperty);
            set => SetValue(xf.Layout.PaddingProperty, value);
        }

		public bool IsClippedToBounds {
			set => SetValue(xf.Layout.IsClippedToBoundsProperty, value);
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
        ViewList Children { get; }
    }

    public partial class StackLayout : Layout<xf.StackLayout>, ILayout
    {
        public ViewList Children { get; } = new ViewList();

        ViewList ILayout.Children => Children;


        public View this[Key key]
        {
            set => Children[key] = value;
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
                    res.Add(new xf.RowDefinition {Height = xf.GridLength.Auto});
                }
                else if (trimmed == "*")
                {
                    res.Add(new xf.RowDefinition {Height = xf.GridLength.Star});
                }
                else if (trimmed.EndsWith("*"))
                {
                    var val = Double.Parse(trimmed.Substring(0, trimmed.Length - 1));
                    res.Add(new xf.RowDefinition {Height = new xf.GridLength(val, xf.GridUnitType.Star)});
                }
                else
                {
                    res.Add(new xf.RowDefinition {Height = new xf.GridLength(Double.Parse(trimmed))});
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
                    res.Add(new xf.ColumnDefinition {Width = xf.GridLength.Auto});
                }
                else if (trimmed == "*")
                {
                    res.Add(new xf.ColumnDefinition {Width = xf.GridLength.Star});
                }
                else if (trimmed.EndsWith("*"))
                {
                    var val = Double.Parse(trimmed.Substring(0, trimmed.Length - 1));
                    res.Add(new xf.ColumnDefinition {Width = new xf.GridLength(val, xf.GridUnitType.Star)});
                }
                else
                {
                    res.Add(new xf.ColumnDefinition {Width = new xf.GridLength(Double.Parse(trimmed))});
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

    }

    public partial class Grid : Layout<xf.Grid>, ILayout
    {
        public GridViewList Children { get; set; } = new GridViewList();

        ViewList ILayout.Children => Children;

        public View this[Key key, int row = 0, int column = 0, int rowSpan = 0, int columnSpan = 0]
        {
            // get => Children[key];
            set
            {
                Children[key] = value;
                Children.SetPositioning(key, row, column, rowSpan, columnSpan);
            }
        }

        public RowDefinitionCollection RowDefinitions { get; set; } = new RowDefinitionCollection();
        public ColumnDefinitionCollection ColumnDefinitions { get; set; } = new ColumnDefinitionCollection();
    }
}