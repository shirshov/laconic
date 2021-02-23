namespace Laconic.Demo
{
    static class AbsoluteLayoutPage
    {
        public static AbsoluteLayout Content() => new AbsoluteLayout {
            ["b", (0.5, 0, 100, 25), AbsoluteLayoutFlags.PositionProportional] = new BoxView { Color = Color.Blue},
            ["g", (0 ,0.5, 25, 100), AbsoluteLayoutFlags.PositionProportional] = new BoxView { Color = Color.Green},
            ["r", (1, 0.5,25,100), AbsoluteLayoutFlags.PositionProportional] = new BoxView { Color = Color.Red},
            ["k", (0.5,1,100,25), AbsoluteLayoutFlags.PositionProportional] = new BoxView { Color = Color.Black},
            ["t", (0.5, 0.5, 110, 25), AbsoluteLayoutFlags.PositionProportional] = new Label {
                Text = "Centered text",
            } 
        };
    }
}