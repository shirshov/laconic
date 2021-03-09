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
            var binder = Binder.CreateForTest("", (s, _) => s);
            var fp = binder.CreateElement(_ => new FlyoutPage {
                Flyout = new ContentPage { Title = "Flyout"},
                Detail = new ContentPage { Title = "Detail"},
            });

            fp.Flyout.ShouldBeOfType<xf.ContentPage>().Title.ShouldBe("Flyout");
            fp.Detail.ShouldBeOfType<xf.ContentPage>().Title.ShouldBe("Detail");
        }

        [Fact]
        public void Detail_in_NavigationPage()
        {
            var binder = Binder.CreateForTest("", (s, _) => s);
            var fp = binder.CreateElement(_ => new FlyoutPage {
                Flyout = new ContentPage { Title = "Flyout"},
                Detail = new NavigationPage(new NavigationStack("root"), _ => new ContentPage { Title = "Detail" }),
            });

            fp.Flyout.ShouldBeOfType<xf.ContentPage>().Title.ShouldBe("Flyout");
            fp.Detail.ShouldBeOfType<xf.NavigationPage>().CurrentPage.Title.ShouldBe("Detail");
        }
        
        [Fact]
        public void Detail_with_LocalContext()
        {
            var detail = Element.WithContext(ctx => {
                var (text, setState) = ctx.UseLocalState("");
                return new ContentPage {
                    Content = new Button {
                        Text = text,
                        Clicked = () => setState("clicked")
                    }
                };
            });
            
            var binder = Binder.CreateForTest("", (s, _) => s);
            var fp = binder.CreateElement(_ => new FlyoutPage {
                Flyout = new ContentPage { Title = "Flyout" },
                Detail = detail
            });

            var contentPage = (xf.ContentPage) fp.Detail;
            var btn = contentPage.Content as xf.Button;
            
            btn.Text.ShouldBe("");
            
            btn.SendClicked();
            
            btn.Text.ShouldBe("clicked");
        }
        
        [Fact(Skip = "Failing after LocalContext refactoring")]
        public void Flyout_with_LocalContext()
        {
            var flyout = Element.WithContext(ctx => {
                var (text, setState) = ctx.UseLocalState("");
                return new ContentPage {
                    Content = new Button {
                        Text = text,
                        Clicked = () => setState("clicked")
                    }
                };
            });
            
            var binder = Binder.CreateForTest("", (s, _) => s);
            var fp = binder.CreateElement(_ => new FlyoutPage {
                Title = "_",
                Flyout = flyout,
                Detail = new ContentPage(),
            });

            var btn = (fp.Flyout as xf.ContentPage).Content as xf.Button;
            
            btn.Text.ShouldBe("");
            
            btn.SendClicked();
            
            btn.Text.ShouldBe("clicked");
        }
    }
}