using xf = Xamarin.Forms;

//  https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/layouts/grid

namespace Laconic.Demo
{
    public class Calculator : xf.ContentPage
    {
        public Calculator()
        {
            var plainButtonColor = xf.Color.FromHex("eee");
            var darkerButtonColor = xf.Color.FromHex("ddd");
            var orangeButtonColor = xf.Color.FromHex("E8AD00");

            Button CalcButton(string text, xf.Color backgroundColor, xf.Color? textColor = null) => new Button
            {
                Text = text,
                BackgroundColor = backgroundColor,
                TextColor = textColor ?? xf.Color.Black,
                CornerRadius = 0,
                FontSize = 40
            };

            BackgroundColor = xf.Color.FromHex("404040");

            var binder = Binder.Create(0, (s, g) => s);

            Content = binder.CreateView(s => new Grid
            {
                RowSpacing = 1,
                ColumnSpacing = 1,
                RowDefinitions = "150, *, *, *, *, *",
                ColumnDefinitions = "*, *, *, *",
                ["result", columnSpan: 4] =
                    new Label
                    {
                        Text = "0",
                        HorizontalTextAlignment = xf.TextAlignment.End,
                        VerticalTextAlignment = xf.TextAlignment.End,
                        TextColor = xf.Color.White,
                        FontSize = 60
                    },
                ["C", 1]      = CalcButton("C", darkerButtonColor),
                ["+/-", 1, 1] = CalcButton("+/-", darkerButtonColor),
                ["%", 1, 2]   = CalcButton("%", darkerButtonColor),
                ["div", 1, 3] = CalcButton("div", orangeButtonColor, xf.Color.White),
                ["7", 2]      = CalcButton("7", plainButtonColor),
                ["8", 2, 1]   = CalcButton("8", plainButtonColor),
                ["9", 2, 2]   = CalcButton("9", plainButtonColor),
                ["X", 2, 3]   = CalcButton("X", orangeButtonColor, xf.Color.White),
                ["4", 3]      = CalcButton("4", plainButtonColor),
                ["5", 3, 1]   = CalcButton("5", plainButtonColor),
                ["6", 3, 2]   = CalcButton("6", plainButtonColor),
                ["-", 3, 3]   = CalcButton("-", orangeButtonColor, xf.Color.White),
                ["1", 4]      = CalcButton("1", plainButtonColor),
                ["2", 4, 1]   = CalcButton("2", plainButtonColor),
                ["3", 4, 2]   = CalcButton("3", plainButtonColor),
                ["+", 4, 3]   = CalcButton("+", orangeButtonColor, xf.Color.White),
                [".", 5, 2]   = CalcButton(".", plainButtonColor),
                ["=", 5, 3]   = CalcButton("=", orangeButtonColor, xf.Color.White),
                ["0", 5, 0, columnSpan: 2] = CalcButton("0", plainButtonColor)
            });
        }
    }
}