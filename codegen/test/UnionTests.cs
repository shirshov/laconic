using System.Drawing;
using Xunit;

namespace Laconic.CodeGeneration.Tests
{
    // ReSharper disable UnusedType.Global
    // ReSharper disable UnusedMember.Global
    // ReSharper disable InconsistentNaming
    [Union]
    interface __Shape__
    {
        record Circle(double radius);
        record Rectangle(double length, double width);
    }

    [Records]
    public interface PredefinedRecords
    {
        record UserLogin(string loginName, string fullName);
        record Computer(string networkName, string model);
    }

    [Union]
    interface _NetworkAsset_
    {
        public UserLogin UserLogin();
        internal Computer Computer();
        record Printer(string model, string networkName, string location);
    }
    
    [Union]
    public interface _EdgeCases_
    {
        record OneParameter(string value);
        // It's totally fine to have parameterless cases 
        // in discriminated unions
        record UnionCaseWithoutParameter();
    }
    // ReSharper restore UnusedType.Global
    // ReSharper restore UnusedMember.Global
    // ReSharper restore InconsistentNaming

    public class UnionTests
    {
        [Fact]
        public void Union_cases_are_Records()
        {
            Assert.Equal(new Circle(1), new Circle(1));
            Assert.NotEqual(new Rectangle(1, 1), new Rectangle(2, 2).With(length: 2));
            Assert.Equal(new Rectangle(1, 2).GetHashCode(), new Rectangle(1, 2).GetHashCode());
        }

        [Fact]
        public void Union_in_switch_expression()
        {
            static string Describe(Shape shape) => shape switch {
                Circle c => $"circle: {c.Radius}",
                Rectangle rec => $"rectangle: {rec.Length}x{rec.Width}",
                _ => "unknown"
            };

            var circle = new Circle(1);
            Assert.Equal("circle: 1", Describe(circle));

            var rect = new Rectangle(2, 3);
            Assert.Equal("rectangle: 2x3", Describe(rect));
        }

        [Fact]
        public void Union_can_have_cases_defined_as_Records_elsewhere()
        {
            static string Describe(NetworkAsset asset) => asset switch {
                UserLogin (_, var fullName) => fullName,
                Computer c => c.NetworkName,
                Printer (_, _, var location) => location,
                _ => "unknown"
            };

            Assert.Equal("b", Describe(new UserLogin("a", "b")));
            Assert.Equal("a", Describe(new Computer("a", "b")));
            Assert.Equal("c", Describe(new Printer("a", "b", "c")));
        }

        [Fact]
        public void parameterless_Union_cases_are_equatable()
        {
            var x = new UnionCaseWithoutParameter();
            var y = new UnionCaseWithoutParameter();
            
            Assert.Equal(x, y);
            Assert.Equal(x.GetHashCode(), y.GetHashCode());
        }

        [Fact]
        public void single_parameter_cases_are_equatable()
        {
            var x = new OneParameter("a");
            var y = new OneParameter("a");
            
            Assert.Equal(x, y);
            Assert.Equal(x.GetHashCode(), y.GetHashCode());
        }
    }
}