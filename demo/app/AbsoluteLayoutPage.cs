namespace Laconic.Demo
{
    public class AbsoluteLayoutPage : Xamarin.Forms.ContentPage 
    {
        public AbsoluteLayoutPage()
        {
            var binder = Binder.Create(0, (s, _) => s);
            Content = binder.CreateElement(_ => new AbsoluteLayout {
                ["b", (0.5, 0, 100, 25), AbsoluteLayoutFlags.PositionProportional] = new BoxView { Color = Color.Blue},
                ["g", (0 ,0.5, 25, 100), AbsoluteLayoutFlags.PositionProportional] = new BoxView { Color = Color.Green},
                ["r", (1, 0.5,25,100), AbsoluteLayoutFlags.PositionProportional] = new BoxView { Color = Color.Red},
                ["k", (0.5,1,100,25), AbsoluteLayoutFlags.PositionProportional] = new BoxView { Color = Color.Black},
                ["t", (0.5, 0.5, 110, 25), AbsoluteLayoutFlags.PositionProportional] = new Label {
                    Text = "Centered text",
                } 
            });
        }       
    }
}