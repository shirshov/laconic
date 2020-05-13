using Shouldly;
using xf = Xamarin.Forms;
using Xunit;

namespace Laconic.Tests
{
    public class GridTests
    {
        [Fact]
        public void should_set_RowDefinitions_from_string()
        {
            var grid = new Grid {RowDefinitions = "*, 2*, Auto, 30"};

            grid.RowDefinitions.Count.ShouldBe(4);
            grid.RowDefinitions[0].Height.ShouldBe(xf.GridLength.Star);
            grid.RowDefinitions[1].Height.ShouldBe(new xf.GridLength(2, xf.GridUnitType.Star));
            grid.RowDefinitions[2].Height.ShouldBe(xf.GridLength.Auto);
            grid.RowDefinitions[3].Height.ShouldBe(new xf.GridLength(30));
        }

        [Fact]
        public void should_set_ColumnDefinitions_from_string()
        {
            var grid = new Grid {ColumnDefinitions = "2*, Auto, 30"};

            grid.ColumnDefinitions.Count.ShouldBe(3);
            grid.ColumnDefinitions[0].Width.ShouldBe(new xf.GridLength(2, xf.GridUnitType.Star));
            grid.ColumnDefinitions[1].Width.ShouldBe(xf.GridLength.Auto);
            grid.ColumnDefinitions[2].Width.ShouldBe(new xf.GridLength(30));
        }

        [Fact]
        public void should_create_rows_in_real_view()
        {
            var grid = new xf.Grid();
            Patch.Apply(grid, Diff.Calculate(null, new Grid {RowDefinitions = "*, 2*, Auto, 30"}), _ => { });

            grid.RowDefinitions.Count.ShouldBe(4);
            grid.RowDefinitions[0].Height.ShouldBe(xf.GridLength.Star);
            grid.RowDefinitions[1].Height.ShouldBe(new xf.GridLength(2, xf.GridUnitType.Star));
            grid.RowDefinitions[2].Height.ShouldBe(xf.GridLength.Auto);
            grid.RowDefinitions[3].Height.ShouldBe(new xf.GridLength(30));
        }

        [Fact]
        public void should_set_ColumnDefinitions_in_real_view()
        {
            var grid = new xf.Grid();
            Patch.Apply(grid, Diff.Calculate(null, new Grid {ColumnDefinitions = "2*, Auto, 30"}), _ => { });

            grid.ColumnDefinitions.Count.ShouldBe(3);
            grid.ColumnDefinitions[0].Width.ShouldBe(new xf.GridLength(2, xf.GridUnitType.Star));
            grid.ColumnDefinitions[1].Width.ShouldBe(xf.GridLength.Auto);
            grid.ColumnDefinitions[2].Width.ShouldBe(new xf.GridLength(30));
        }

        [Fact]
        public void should_set_ColumnDefinitions_in_real_view2()
        {
            var grid = new xf.Grid();
            Patch.Apply(grid, Diff.Calculate(null, new Grid {ColumnDefinitions = "Auto, *, Auto"}), _ => { });

            grid.ColumnDefinitions.Count.ShouldBe(3);
            grid.ColumnDefinitions[0].Width.ShouldBe(xf.GridLength.Auto);
            grid.ColumnDefinitions[1].Width.ShouldBe(xf.GridLength.Star);
            grid.ColumnDefinitions[2].Width.ShouldBe(xf.GridLength.Auto);
        }
    }
}