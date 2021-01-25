using Shouldly;
using Xunit;
using xf = Xamarin.Forms;

namespace Laconic.Tests
{
    public class FlyoutPageTests
    {
        [Fact]
        public void FlyoutPage_can_be_created()
        {
            var binder = Binder.Create("", (s, _) => s);
            var fp = binder.CreateElement(_ => new FlyoutPage {
                Flyout = new ContentPage { Title = "Flyout"},
                Detail = new ContentPage { Title = "Detail"},
            });

            fp.Flyout.ShouldBeOfType<xf.ContentPage>().Title.ShouldBe("Flyout");
            fp.Detail.ShouldBeOfType<xf.ContentPage>().Title.ShouldBe("Detail");
        }
    }
}