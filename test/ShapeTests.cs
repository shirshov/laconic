using Laconic.Shapes;

namespace Laconic.Tests;

public class ShapeTests
{
    [Fact]
    public void Line_diff()
    {
        var noDiff = Diff.Calculate(new Line {X1 = 1, Y1 = 2, X2 = 3, Y2 = 4},
            new Line {X1 = 1, Y1 = 2, X2 = 3, Y2 = 4});
        noDiff.Count().ShouldBe(0);

        var diff = Diff.Calculate(new Line {X1 = 100, Y1 = 2, X2 = 3, Y2 = 4},
            new Line {X1 = 1, Y1 = 2, X2 = 3, Y2 = 4});
        var prop = diff.First().ShouldBeOfType<SetProperty>();
        prop.Property.ShouldBe(xf.Shapes.Line.X1Property);
        prop.Value.ShouldBe(1);
    }

    [Fact]
    public void Clip_property_is_set()
    {
        var img = new Image {Clip = new EllipseGeometry {Center = new xf.Point(10, 10), RadiusX = 3, RadiusY = 5}};
        var diff = Diff.Calculate(null, img).ToArray();

        diff[0].ShouldBeOfType<SetChildElement>();
    }

    [Fact]
    public void Clip_property_not_set_if_value_is_identical()
    {
        var img = new Image {Clip = new EllipseGeometry {Center = new xf.Point(10, 10), RadiusX = 3, RadiusY = 5}};
        var diff = Diff.Calculate(img,
            new Image {Clip = new EllipseGeometry {Center = new xf.Point(10, 10), RadiusX = 3, RadiusY = 5}});
        diff.ShouldBeEmpty();
    }

    [Fact]
    public void diffing_Path_as_Clip()
    {
        var img = new Image {Clip = new PathGeometry("M 10,100 C 100,0 200,200 300,100")};
        var diff = Diff.Calculate(null, img);

        diff.First().ShouldBeOfType<SetChildElement>();
    }
}