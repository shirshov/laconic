using xfs = Microsoft.Maui.Controls.Shapes;

namespace Laconic.Shapes;

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

public class Geometry<T> : Element<T>, Geometry where T : xfs.Geometry, new()
{
    protected internal override xf.BindableObject CreateView() => new T();
}

public class EllipseGeometry : Geometry<xfs.EllipseGeometry>
{
    
    // TODO: why not use Point from Laconic namespace?
    public Maui.Graphics.Point Center {
        set => SetValue(xfs.EllipseGeometry.CenterProperty, value);
    }

    public double RadiusX {
        set => SetValue(xfs.EllipseGeometry.RadiusXProperty, value);
    }

    public double RadiusY {
        set => SetValue(xfs.EllipseGeometry.RadiusYProperty, value);
    }
}

public class PathGeometry : Geometry<xfs.PathGeometry>
{
    public PathGeometry(string data)
    {
        var figures = new xfs.PathFigureCollection();
        xfs.PathFigureCollectionConverter.ParseStringToPathFigureCollection(figures, data);
        Figures = figures;
    }

    public FillRule FillRule {
        set => SetValue(xfs.PathGeometry.FillRuleProperty, value);
    }

    public xfs.PathFigureCollection Figures {
        set => SetValue(xfs.PathGeometry.FiguresProperty, value);
    }
}

public class LineGeometry : Geometry<xfs.LineGeometry>
{
    public Maui.Graphics.Point StartPoint {
        set => SetValue(xfs.LineGeometry.StartPointProperty, value);
    }

    public Maui.Graphics.Point EndPoint {
        set => SetValue(xfs.LineGeometry.EndPointProperty, value);
    }
}

public class RectangleGeometry : Geometry<xfs.RectangleGeometry>
{
    public Rectangle Rect;
}

public abstract class Shape<T> : View<T> where T : xf.View, new()
{
    public Stretch Aspect {
        get => GetValue<Stretch>(xfs.Shape.AspectProperty);
        set => SetValue(xfs.Shape.AspectProperty, value);
    }

    public IBrush Fill {
        get => GetValue<IBrush>(xfs.Shape.FillProperty);
        set => SetValue(xfs.Shape.FillProperty, value);
    }

    public xf.DoubleCollection StrokeDashArray {
        get => GetValue<xf.DoubleCollection>(xfs.Shape.StrokeDashArrayProperty);
        set => SetValue(xfs.Shape.StrokeDashArrayProperty, value);
    }

    public double StrokeDashOffset {
        get => GetValue<double>(xfs.Shape.StrokeDashOffsetProperty);
        set => SetValue(xfs.Shape.StrokeDashOffsetProperty, value);
    }

    public PenLineCap StrokeLineCap {
        get => GetValue<PenLineCap>(xfs.Shape.StrokeLineCapProperty);
        set => SetValue(xfs.Shape.StrokeLineCapProperty, value);
    }

    public PenLineJoin StrokeLineJoin {
        get => GetValue<PenLineJoin>(xfs.Shape.StrokeLineJoinProperty);
        set => SetValue(xfs.Shape.StrokeLineJoinProperty, value);
    }

    public IBrush Stroke {
        get => GetValue<IBrush>(xfs.Shape.StrokeProperty);
        set => SetValue(xfs.Shape.StrokeProperty, value);
    }

    public double StrokeThickness {
        get => GetValue<double>(xfs.Shape.StrokeThicknessProperty);
        set => SetValue(xfs.Shape.StrokeThicknessProperty, value);
    }
}

public class Ellipse : Shape<xfs.Ellipse>
{
}

public class Path : Shape<xfs.Path>
{
    public string Data {
        set {
            var geom = new xfs.PathGeometry();
            xfs.PathFigureCollectionConverter.ParseStringToPathFigureCollection(geom.Figures, value);
            SetValue(xfs.Path.DataProperty, geom);
        }
    }
}

public class Line : Shape<xfs.Line>
{
    public double X1 {
        set => SetValue(xfs.Line.X1Property, value);
    }

    public double Y1 {
        set => SetValue(xfs.Line.Y1Property, value);
    }

    public double X2 {
        set => SetValue(xfs.Line.X2Property, value);
    }

    public double Y2 {
        set => SetValue(xfs.Line.Y2Property, value);
    }
}

public class Rectangle : Shape<xfs.Rectangle>
{
    public double RadiusX {
        set => SetValue(xfs.Rectangle.RadiusXProperty, value);
    }

    public double RadiusY {
        set => SetValue(xfs.Rectangle.RadiusYProperty, value);
    }
}

public class Polygon : Shape<xfs.Polygon>
{
    public xf.PointCollection Points {
        set => SetValue(xfs.Polygon.PointsProperty, value);
    }

    public FillRule FillRule {
        set => SetValue(xfs.Polygon.FillRuleProperty, value);
    }
}

public class Polyline : Shape<xfs.Polyline>
{
    public xf.PointCollection Points {
        set => SetValue(xfs.Polyline.PointsProperty, value);
    }

    public FillRule FillRule {
        set => SetValue(xfs.Polyline.FillRuleProperty, value);
    }
}