using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Shouldly;
using xf = Xamarin.Forms;

namespace Laconic.Tests
{
    public class NavigationPageTests
    {
        [Fact]
        public void NavigationStack_Remove()
        {
            var stack = new NavigationStack("foo", ("bar", 1));
            
            stack = stack.Remove(("bar", 1));
            
            stack.Count.ShouldBe(1);
            stack.First().ShouldBe("foo");
        }
        
        [Fact]
        public void pages_Diff_calculated()
        {
            var navPage =  new NavigationPage(new NavigationStack("root"), _ => new ContentPage());
            var diff = Diff.Calculate(null, navPage);
            
            diff.Count().ShouldBe(2);
            var listUpdate = diff.ToArray()[1].ShouldBeOfType<UpdateChildElementList>();
            var addPage = listUpdate.Operations.First().ShouldBeOfType<AddChild>();
            addPage.Blueprint.ShouldBeOfType<ContentPage>();
        }
        
        [Fact]
        public void page_added_and_removed_on_stack()
        {
            var stack = new NavigationStack("root");

            ContentPage PageMaker(object frame) => frame switch {
                "root" => new ContentPage {Title = "Root"},
                "details" => new ContentPage {Title = "Details"}
            };

            var existingNavPage = new NavigationPage(stack, PageMaker);
            existingNavPage.ElementLists["Pages"].Count.ShouldBe(1);
 
            stack.Frames.Add(new("details", -1));
            existingNavPage.ElementLists["Pages"].Count.ShouldBe(1);

            var added = new NavigationPage(stack, PageMaker);
            added.ElementLists["Pages"].Count.ShouldBe(2);

            stack.Frames.RemoveAt(1);
            existingNavPage.ElementLists["Pages"].Count.ShouldBe(1);
            added.ElementLists["Pages"].Count.ShouldBe(2);

            var removed = new NavigationPage(stack, PageMaker);
            removed.ElementLists["Pages"].Count.ShouldBe(1);
        }

        [Fact]
        public void real_NavPage_created()
        {
            var b = Binder.CreateForTest("", (_, _) => "");

            var real = b.CreateElement(_ => new NavigationPage(new NavigationStack("root"), _ => new ContentPage()));

            real.Pages.Count().ShouldBe(1);
        }

        [Fact]
        public void page_pushed_then_popped_with_custom_Signal()
        {
            var stack = new NavigationStack("root");
            var b = Binder.CreateForTest(stack, (s, g) => g switch {
                ("open-details", _) p => s.Push(p.Payload),
                ("close-details", _) _ => s.Pop()
            });

            var real = b.CreateElement(s => new NavigationPage(stack, _ => new ContentPage()));

            stack.Frames.Count.ShouldBe(1);
            real.Pages.Count().ShouldBe(1);
            
            b.Send(new("open-details"));
            
            stack.Frames.Count.ShouldBe(2);
            real.Pages.Count().ShouldBe(2);
            
            b.Send(new("close-details"));
            
            stack.Frames.Count.ShouldBe(1);
            real.Pages.Count().ShouldBe(1);
        }
        
        [Fact]
        public void Diff_on_Pages()
        {
            var p1 = new NavigationPage(new NavigationStack("root"), f => new ContentPage {Title = (string) f});
            var p2 = new NavigationPage(new NavigationStack("root", "details"), f => new ContentPage {Title = (string) f});

            var diff = Diff.Calculate(p1, p2);
            diff.Count().ShouldBe(1);
        }

        [Fact]
        public void child_page_with_LocalContext()
        {
            var b = Binder.CreateForTest(new NavigationStack("root"), (s, g) => g switch {
                ("open-details", _) p => s.Push(p.Payload),
                ("close-details", _) _ => s.Pop()
            });

            var real = b.CreateElement(s => new NavigationPage(s, _ =>
                Element.WithContext(_ => new ContentPage {Title = "Test"})));
            
            real.CurrentPage.Title.ShouldBe("Test");
        }

        [Fact]
        public async Task when_NavigationPage_is_not_top_level()
        {
            var b = Binder.CreateForTest(new NavigationStack("root", "details"), (s, _) => s);

            var flyoutPage = b.CreateElement(s => new FlyoutPage {
                Flyout = new ContentPage {Title = "FO"},
                Detail = new NavigationPage(s, _ => new ContentPage())
            });
            // Should not throw:
            await (flyoutPage.Detail as xf.NavigationPage).PopAsync();
        }
    }
}