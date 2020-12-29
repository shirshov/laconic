using System;
using System.Collections.Generic;
using xf = Xamarin.Forms;

namespace Laconic
{
    public partial class Button
    {
        public xf.Button.ButtonContentLayout ContentLayout {
            internal get => GetValue<xf.Button.ButtonContentLayout>(xf.Button.ContentLayoutProperty);
            init => SetValue(xf.Button.ContentLayoutProperty, value);
        }
    }

    public partial class Switch
    {
        public Func<xf.ToggledEventArgs, Signal> Toggled {
            init => SetEvent(nameof(Toggled), value,
                (ctl, handler) => ctl.Toggled += handler,
                (ctl, handler) => ctl.Toggled -= handler);
        }
    }

    public partial class Slider
    {
        public Func<xf.ValueChangedEventArgs, Signal> ValueChanged {
            init => SetEvent(nameof(ValueChanged), value,
                (ctl, handler) => ctl.ValueChanged += handler,
                (ctl, handler) => ctl.ValueChanged -= handler);
        }
    }

    public partial class CheckBox : View<xf.CheckBox>
    {
        public Func<xf.CheckedChangedEventArgs, Signal> CheckedChanged {
            init => SetEvent(nameof(CheckedChanged), value,
                (ctl, handler) => ctl.CheckedChanged += handler,
                (ctl, handler) => ctl.CheckedChanged -= handler);
        }
    }

    public partial class DatePicker
    {
        public Func<xf.DateChangedEventArgs, Signal> DateSelected {
            set => SetEvent(nameof(DateSelected), value,
                (ctl, handler) => ctl.DateSelected += handler,
                (ctl, handler) => ctl.DateSelected -= handler);
        }
    }

    public class SelectedIndexChangedEventArgs : EventArgs
    {
        public int SelectedIndex { get; }
        public SelectedIndexChangedEventArgs(int selectedIndex) => SelectedIndex = selectedIndex;
    }

    public partial class Picker
    {
        public IList<string> Items {
            init => SetValue(xf.Picker.ItemsSourceProperty, value);
        }

        public Func<SelectedIndexChangedEventArgs, Signal> SelectedIndexChanged {
            init => SetEvent(nameof(SelectedIndexChanged), value,
                (ctl, handler) => ctl.SelectedIndexChanged += (s, _) =>
                    handler(s, new SelectedIndexChangedEventArgs(((xf.Picker) s).SelectedIndex)),
                (ctl, handler) => ctl.SelectedIndexChanged -= (s, _) =>
                    handler(s, new SelectedIndexChangedEventArgs(((xf.Picker) s).SelectedIndex)));
        }
    }

    public abstract partial class InputView<T> : VisualElement<T>, View where T : Xamarin.Forms.InputView, new()
    {
        public Func<xf.TextChangedEventArgs, Signal> TextChanged {
            init => SetEvent(nameof(TextChanged), value,
                (ctl, handler) => ctl.TextChanged += handler,
                (ctl, handler) => ctl.TextChanged -= handler);
        }
    }

    public partial class Entry : InputView<xf.Entry>
    {

    }

    public partial class Editor : InputView<xf.Editor>
    {

    }

    public partial class RadioButton
    {
        public Func<xf.CheckedChangedEventArgs, Signal> CheckedChanged {
            init => SetEvent(nameof(CheckedChanged), value,
                (ctl, handler) => ctl.CheckedChanged += handler,
                (ctl, handler) => ctl.CheckedChanged -= handler);
        }
    }
}
