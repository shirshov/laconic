using System.Linq;
using Shouldly;
using Xunit;
using xf = Xamarin.Forms;

namespace Laconic.Tests
{
    public class ImageSourceTests
    {
        [Fact]
        public void FontImageSource_can_be_created_and_updated()
        {
            var bp1 = new Image {
                Source = new FontImageSource {FontFamily = "Arial", Glyph = "a", Size = 13, Color = Color.Red}
            };
            var bp2 = new Image {
                Source = new FontImageSource {FontFamily = "Helvetica", Glyph = "h", Size = 15, Color = Color.Green}
            };
            
            var binder = Binder.CreateForTest(0, (s, g) => s + 1);
            var img = binder.CreateElement(s => s == 0 ? bp1 : bp2);
            
            var imgSource = img.Source.ShouldBeOfType<xf.FontImageSource>();

            imgSource.FontFamily.ShouldBe("Arial");
            imgSource.Glyph.ShouldBe("a");
            imgSource.Size.ShouldBe(13);
            imgSource.Color.ShouldBe(xf.Color.Red);
            
            binder.Send(new Signal(null));

            imgSource = img.Source.ShouldBeOfType<xf.FontImageSource>();
             
            imgSource.FontFamily.ShouldBe("Helvetica");
            imgSource.Glyph.ShouldBe("h");
            imgSource.Size.ShouldBe(15);
            imgSource.Color.ShouldBe(xf.Color.Green);
        }
    }
}