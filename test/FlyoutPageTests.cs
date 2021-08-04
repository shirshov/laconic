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

        [Fact]
        public void Flyout_Detail_type_changes()
        {
            var b = Binder.CreateForTest(true, (s, _) => !s);

            var stack = new NavigationStack("root");
            var flyoutPage = b.CreateElement(isNavigationPage => new FlyoutPage {
                Flyout = new ContentPage {Title = "FO"},
                Detail = isNavigationPage
                    ? new NavigationPage(stack, _ => new ContentPage())
                    : new ContentPage()
            });
            
            flyoutPage.Detail.ShouldBeOfType<xf.NavigationPage>();

            b.Send(new(null));
            
            flyoutPage.Detail.ShouldBeOfType<xf.ContentPage>();
            
            b.Send(new(null));
            
            flyoutPage.Detail.ShouldBeOfType<xf.NavigationPage>();
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