// The UI code is ported from here:
//  https://docs.microsoft.com/en-us/xamarin/xamarin-forms/user-interface/layouts/grid

// The logic is ported from here:
// https://github.com/fsprojects/Fabulous/blob/master/Fabulous.XamarinForms/samples/Calculator/Calculator/Calculator.fs

namespace Laconic.Demo;

static class Calculator
{
    enum Operator { Add, Subtract, Multiply, Divide }

    public interface CalculatorSignal { }

    class OperatorSignal : Signal<Operator>, CalculatorSignal
    {
        public OperatorSignal(Operator payload) : base(payload) { }
    }

    class DigitSignal : Signal<int>, CalculatorSignal
    {
        public DigitSignal(int payload) : base(payload) { }
    }

    class EqualsSignal : Signal, CalculatorSignal
    {
        public EqualsSignal() : base(null) { }
    }

    class ClearSignal : Signal, CalculatorSignal
    {
        public ClearSignal() : base(null) { }
    }

    public interface State { }

    public class Initial : State { }

    record Operand(double Value) : State;

    record OperandOperator(double Operand, Operator Op) : State;

    record OperandOperatorOperand(double Operand1, Operator Op, double Operand2) : State;

    record Result(double Value) : State;

    class Error : State { }

    static double CalculateOperation(double operand1, double operand2, Operator @operator) => @operator switch {
        Operator.Add => operand1 + operand2,
        Operator.Subtract => operand1 - operand2,
        Operator.Multiply => operand1 * operand2,
        Operator.Divide => operand1 / operand2,
        _ => throw new NotImplementedException($"Operator not implemented: {@operator}")
    };

    static State Calculate(State state, CalculatorSignal signal) => state switch {
        OperandOperatorOperand (_, Operator.Divide, 0.0) => new Error(),
        OperandOperatorOperand (var operand1, var @operator, var operand2) => signal switch {
            EqualsSignal _ => new Result(CalculateOperation(operand1, operand2, @operator)),
            // pass the result in as the start of a new calculation (1 + 1 + -> 2 +)
            OperatorSignal op => new OperandOperator(CalculateOperation(operand1, operand2, @operator), op.Payload),
            _ => state
        },
        _ => state
    };

    public static State MainReducer(State state, CalculatorSignal signal) => signal switch {
        ClearSignal _ => new Initial(),
        DigitSignal (int digit, null) => state switch {
            var s when s is Initial || s is Error || s is Result => new Operand(digit),
            Operand op => new Operand(op.Value * 10 + digit),
            OperandOperator (var operand, var @operator) => new OperandOperatorOperand(operand, @operator, digit),
            OperandOperatorOperand(var op1, var @operator, var op2) => new OperandOperatorOperand(op1, @operator,
                op2 * 10 + digit),
            _ => state
        },
        OperatorSignal (Operator @operator, _) => state switch {
            var s when s is Initial || s is Error => state,
            Result r => new OperandOperator(r.Value, @operator),
            Operand operand => new OperandOperator(operand.Value, @operator),
            OperandOperator(var operand, _) => new OperandOperator(operand, @operator),
            OperandOperatorOperand _ => Calculate(state, signal),
            _ => state
        },
        EqualsSignal _ => Calculate(state, signal),
        _ => throw new NotImplementedException($"Signal support is not implemented: {signal}")
    };

    static string Display(State state) => state switch {
        Initial _ => "0",
        Operand op => op.Value.ToString(),
        OperandOperator (var op, _) => op.ToString(),
        OperandOperatorOperand(_, _, var op) => op.ToString(),
        Result res => res.Value.ToString(),
        Error _ => "Error",
        _ => state.ToString()
    };

    static Button CalcButton(string text, Signal signal, Color backgroundColor, Color? textColor = null
    ) => new() {
        Text = text,
        BackgroundColor = backgroundColor,
        TextColor = textColor ?? Color.Black,
        CornerRadius = 0,
        FontSize = 40,
        Clicked = () => signal
    };

    const string plainButtonColor = "eee";
    const string darkerButtonColor = "ddd";
    const string orangeButtonColor = "E8AD00";

    public static Grid Content(State state) => new Grid {
        BackgroundColor = "404040",
        RowSpacing = 1,
        ColumnSpacing = 1,
        RowDefinitions = "150, *, *, *, *, *",
        ColumnDefinitions = "*, *, *, *",
        ["result", columnSpan: 4] =
            new Label {
                Text = Display(state),
                HorizontalTextAlignment = TextAlignment.End,
                VerticalTextAlignment = TextAlignment.End,
                TextColor = Color.White,
                FontSize = 60
            },
        ["C", 1] = CalcButton("C", new ClearSignal(), darkerButtonColor),
        ["+/-", 1, 1] = CalcButton("+/-", null, darkerButtonColor),
        ["%", 1, 2] = CalcButton("%", null, darkerButtonColor),
        ["div", 1, 3] = CalcButton("div", new OperatorSignal(Operator.Divide), orangeButtonColor, Color.White),
        ["7", 2] = CalcButton("7", new DigitSignal(7), plainButtonColor),
        ["8", 2, 1] = CalcButton("8", new DigitSignal(8), plainButtonColor),
        ["9", 2, 2] = CalcButton("9", new DigitSignal(9), plainButtonColor),
        ["X", 2, 3] = CalcButton("X", new OperatorSignal(Operator.Multiply), orangeButtonColor, Color.White),
        ["4", 3] = CalcButton("4", new DigitSignal(4), plainButtonColor),
        ["5", 3, 1] = CalcButton("5", new DigitSignal(5), plainButtonColor),
        ["6", 3, 2] = CalcButton("6", new DigitSignal(6), plainButtonColor),
        ["-", 3, 3] = CalcButton("-", new OperatorSignal(Operator.Subtract), orangeButtonColor, Color.White),
        ["1", 4] = CalcButton("1", new DigitSignal(1), plainButtonColor),
        ["2", 4, 1] = CalcButton("2", new DigitSignal(2), plainButtonColor),
        ["3", 4, 2] = CalcButton("3", new DigitSignal(3), plainButtonColor),
        ["+", 4, 3] = CalcButton("+", new OperatorSignal(Operator.Add), orangeButtonColor, Color.White),
        [".", 5, 2] = CalcButton(".", new EqualsSignal(), plainButtonColor),
        ["=", 5, 3] = CalcButton("=", new EqualsSignal(), orangeButtonColor, Color.White),
        ["0", 5, 0, columnSpan: 2] = CalcButton("0", new DigitSignal(0), plainButtonColor)
    };
}