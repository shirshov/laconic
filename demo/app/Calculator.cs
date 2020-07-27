using System;
using Laconic.CodeGen;
using xf = Xamarin.Forms;

//  https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/layouts/grid

namespace Laconic.Demo.Calculator
{
    enum Operator { Add, Subtract, Multiply, Divide }

    class OperatorSignal : Signal<Operator> { public OperatorSignal(Operator payload) : base(payload) { } }
    class DigitSignal : Signal<int> { public DigitSignal(int payload) : base(payload) { } }
    class EqualsSignal : Signal { public EqualsSignal() : base(null) { } }
    class ClearSignal : Signal { public ClearSignal() : base(null) { } }

    [Union]
    interface __State
    {
        record Initial();
        record Operand(double value);
        record OperandOperator(double operand, Operator op);
        record OperandOperatorOperand(double operand1, Operator op, double operand2);
        record Result(double value);
        record Error();
    }

    public class Calculator : xf.ContentPage
    {
        static double CalculateOperation(double operand1, double operand2, Operator @operator) => @operator switch {
            Operator.Add => operand1 + operand2,
            Operator.Subtract => operand1 - operand2,
            Operator.Multiply => operand1 * operand2,
            Operator.Divide => operand1 / operand2
        };

        static State Calculate(State state, Signal signal) => state switch {
            OperandOperatorOperand (_, Operator.Divide, 0.0) => new Error(),
            OperandOperatorOperand (var operand1, var @operator, var operand2) => signal switch {
                EqualsSignal _ => new Result(CalculateOperation(operand1, operand2, @operator)),
                // pass the result in as the start of a new calculation (1 + 1 + -> 2 +)
                OperatorSignal op => new OperandOperator(CalculateOperation(operand1, operand2, @operator), op.Payload),
                _ => state
            },
            _ => state
        };

        static State MainReducer(State state, Signal signal) => signal switch {
            ClearSignal _ => new Initial(),
            DigitSignal (int digit, null) => state switch {
                var s when s is Initial || s is Error || s is Result => new Operand(digit),
                Operand op => new Operand(op.Value * 10 + digit),
                OperandOperator (var operand, var @operator) => new OperandOperatorOperand(operand, @operator, digit),
                OperandOperatorOperand(var op1, var @operator, var op2) => new OperandOperatorOperand(op1, @operator, op2 * 10 + digit)
            },
            OperatorSignal (Operator @operator, _) => state switch {
                var s when s is Initial || s is Error => state,
                Result r => new OperandOperator(r.Value, @operator), // TODO: single value records should be implicitly cast?
                Operand operand => new OperandOperator(operand.Value, @operator),
                OperandOperator(var operand, _) => new OperandOperator(operand, @operator),
                OperandOperatorOperand _ => Calculate(state, signal),
            },
            EqualsSignal _ => Calculate(state, signal)
        };

        static string Display(State state) => state switch {
            Initial _ => "0",
            Operand op => op.Value.ToString(),
            OperandOperator (var op, _) => op.ToString(),
            OperandOperatorOperand(_, _, var op) => op.ToString(),
            Result res => res.Value.ToString(),
            Error _ => "Error"
        };

        public Calculator()
        {
            var plainButtonColor = xf.Color.FromHex("eee");
            var darkerButtonColor = xf.Color.FromHex("ddd");
            var orangeButtonColor = xf.Color.FromHex("E8AD00");

            Button CalcButton(string text, Func<Signal> signalMaker, xf.Color backgroundColor, xf.Color? textColor = null
                ) => new Button {
                Text = text,
                BackgroundColor = backgroundColor,
                TextColor = textColor ?? xf.Color.Black,
                CornerRadius = 0,
                FontSize = 40,
                Clicked = signalMaker
            };

            BackgroundColor = xf.Color.FromHex("404040");

            State initialState = new Initial();
            var binder = Binder.Create(initialState, MainReducer);

            Content = binder.CreateView(s => new Grid {
                RowSpacing = 1,
                ColumnSpacing = 1,
                RowDefinitions = "150, *, *, *, *, *",
                ColumnDefinitions = "*, *, *, *",
                ["result", columnSpan: 4] =
                    new Label {
                        Text = Display(s) ,
                        HorizontalTextAlignment = xf.TextAlignment.End,
                        VerticalTextAlignment = xf.TextAlignment.End,
                        TextColor = xf.Color.White,
                        FontSize = 60
                    },
                ["C", 1] = CalcButton("C", () => new ClearSignal(), darkerButtonColor),
                ["+/-", 1, 1] = CalcButton("+/-", null, darkerButtonColor),
                ["%", 1, 2] = CalcButton("%", null, darkerButtonColor),
                ["div", 1, 3] = CalcButton("div", () => new OperatorSignal(Operator.Divide), orangeButtonColor, xf.Color.White),
                ["7", 2] = CalcButton("7", () => new DigitSignal(7),  plainButtonColor),
                ["8", 2, 1] = CalcButton("8", () => new DigitSignal(8), plainButtonColor),
                ["9", 2, 2] = CalcButton("9", () => new DigitSignal(9), plainButtonColor),
                ["X", 2, 3] = CalcButton("X", () => new OperatorSignal(Operator.Multiply), orangeButtonColor, xf.Color.White),
                ["4", 3] = CalcButton("4", () => new DigitSignal(4), plainButtonColor),
                ["5", 3, 1] = CalcButton("5",() => new DigitSignal(5), plainButtonColor),
                ["6", 3, 2] = CalcButton("6", () => new DigitSignal(6), plainButtonColor),
                ["-", 3, 3] = CalcButton("-", () => new OperatorSignal(Operator.Subtract), orangeButtonColor, xf.Color.White),
                ["1", 4] = CalcButton("1", () => new DigitSignal(1), plainButtonColor),
                ["2", 4, 1] = CalcButton("2",() => new DigitSignal(2), plainButtonColor),
                ["3", 4, 2] = CalcButton("3", () => new DigitSignal(3), plainButtonColor),
                ["+", 4, 3] = CalcButton("+", () => new OperatorSignal(Operator.Add), orangeButtonColor, xf.Color.White),
                [".", 5, 2] = CalcButton(".", () => new EqualsSignal(), plainButtonColor),
                ["=", 5, 3] = CalcButton("=", () => new EqualsSignal(), orangeButtonColor, xf.Color.White),
                ["0", 5, 0, columnSpan: 2] = CalcButton("0", () => new DigitSignal(0), plainButtonColor)
            });
        }
    }
}