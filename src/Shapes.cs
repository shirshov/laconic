using Xamarin.Forms.Shapes;
using xf = Xamarin.Forms;

namespace Laconic.Shapes
{
    public enum FillRule
    {
        EvenOdd,
        Nonzero
    }
    public enum PenLineCap
    {
        Flat,
        Square,
        Round
    }
    
    public enum PenLineJoin
    {
        Miter,
        Bevel,
        Round
    }

    public interface Geometry
    {
    }

    public class Geometry<T> : Element<T>, Geometry where T : xf.Shapes.Geometry, new()
    {
        internal override xf.BindableObject CreateView() => new T();
    }

    public class EllipseGeometry : Geometry<xf.Shapes.EllipseGeometry>
    {
        public xf.Point Center {
            set => SetValue(xf.Shapes.EllipseGeometry.CenterProperty, value);
        }

        public double RadiusX {
            set => SetValue(xf.Shapes.EllipseGeometry.RadiusXProperty, value);
        }

        public double RadiusY {
            set => SetValue(xf.Shapes.EllipseGeometry.RadiusYProperty, value);
        }
    }

    public class PathGeometry : Geometry<xf.Shapes.PathGeometry>
    {
        readonly string _data;

        public PathGeometry(string data)
        {
            _data = data;
            var figures = new PathFigureCollection();
            xf.Shapes.PathFigureCollectionConverter.ParseStringToPathFigureCollection(figures, data);
            Figures = figures;
        }

        public FillRule FillRule {
            set => SetValue(xf.Shapes.PathGeometry.FillRuleProperty, value);
        }

        public PathFigureCollection Figures {
            set => SetValue(xf.Shapes.PathGeometry.FiguresProperty, value);
        }
    }

    public class LineGeometry : Geometry<xf.Shapes.LineGeometry>
    {
        public xf.Point StartPoint {
            set => SetValue(xf.Shapes.LineGeometry.StartPointProperty, value);
        }

        public xf.Point EndPoint {
            set => SetValue(xf.Shapes.LineGeometry.EndPointProperty, value);
        }
    }

    public class RectangleGeometry : Geometry<xf.Shapes.RectangleGeometry>
    {
        public xf.Rectangle Rect;
    }

    public abstract class Shape<T> : View<T> where T : xf.View, new()
    {
        public Stretch Aspect {
            get => GetValue<Stretch>(xf.Shapes.Shape.AspectProperty);
            set => SetValue(xf.Shapes.Shape.AspectProperty, value);
        }

        public Color Fill {
            get => GetValue<Color>(xf.Shapes.Shape.FillProperty);
            set => SetValue(xf.Shapes.Shape.FillProperty, value);
        }

        public xf.DoubleCollection StrokeDashArray {
            get => GetValue<xf.DoubleCollection>(xf.Shapes.Shape.StrokeDashArrayProperty);
            set => SetValue(xf.Shapes.Shape.StrokeDashArrayProperty, value);
        }

        public double StrokeDashOffset {
            get => GetValue<double>(xf.Shapes.Shape.StrokeDashOffsetProperty);
            set => SetValue(xf.Shapes.Shape.StrokeDashOffsetProperty, value);
        }

        public PenLineCap StrokeLineCap {
            get => GetValue<PenLineCap>(xf.Shapes.Shape.StrokeLineCapProperty);
            set => SetValue(xf.Shapes.Shape.StrokeLineCapProperty, value);
        }

        public PenLineJoin StrokeLineJoin {
            get => GetValue<PenLineJoin>(xf.Shapes.Shape.StrokeLineJoinProperty);
            set => SetValue(xf.Shapes.Shape.StrokeLineJoinProperty, value);
        }

        public Color Stroke {
            get => GetValue<Color>(xf.Shapes.Shape.StrokeProperty);
            set => SetValue(xf.Shapes.Shape.StrokeProperty, value);
        }

        public double StrokeThickness {
            get => GetValue<double>(xf.Shapes.Shape.StrokeThicknessProperty);
            set => SetValue(xf.Shapes.Shape.StrokeThicknessProperty, value);
        }
    }

    public class Ellipse : Shape<xf.Shapes.Ellipse>
    {
    }

    public class Path : Shape<xf.Shapes.Path>
    {
        public string Data {
            set {
                var geom = new xf.Shapes.PathGeometry();
                xf.Shapes.PathFigureCollectionConverter.ParseStringToPathFigureCollection(geom.Figures, value);
                SetValue(xf.Shapes.Path.DataProperty, geom);
            }
        }
    }

    public class Line : Shape<xf.Shapes.Line>
    {
        public double X1 {
            set => SetValue(xf.Shapes.Line.X1Property, value);
        }

        public double Y1 {
            set => SetValue(xf.Shapes.Line.Y1Property, value);
        }

        public double X2 {
            set => SetValue(xf.Shapes.Line.X2Property, value);
        }

        public double Y2 {
            set => SetValue(xf.Shapes.Line.Y2Property, value);
        }
    }

    public class Rectangle : Shape<xf.Shapes.Rectangle>
    {
        public double RadiusX {
            set => SetValue(xf.Shapes.Rectangle.RadiusXProperty, value);
        }

        public double RadiusY {
            set => SetValue(xf.Shapes.Rectangle.RadiusYProperty, value);
        }
    }

    public class Polygon : Shape<xf.Shapes.Polygon>
    {
        public PointCollection Points {
            set => SetValue(xf.Shapes.Polygon.PointsProperty, value);
        }

        public FillRule FillRule {
            set => SetValue(xf.Shapes.Polygon.FillRuleProperty, value);
        }
    }

    public class Polyline : Shape<xf.Shapes.Polyline>
    {
        public PointCollection Points {
            set => SetValue(xf.Shapes.Polyline.PointsProperty, value);
        }

        public FillRule FillRule {
            set => SetValue(xf.Shapes.Polyline.FillRuleProperty, value);
        }
    }
}