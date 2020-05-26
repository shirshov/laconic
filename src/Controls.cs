using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using xf = Xamarin.Forms;

namespace Laconic
{
    public partial class Button
    {
        public xf.Button.ButtonContentLayout ContentLayout
        {
            get => GetValue<xf.Button.ButtonContentLayout>(xf.Button.ContentLayoutProperty);
            set => SetValue(xf.Button.ContentLayoutProperty, value);
        }
    }

    public partial class Switch
    {
        public Expression<Func<xf.ToggledEventArgs, Signal>> Toggled
        {
            set => SetEvent(nameof(Toggled), value,
                (ctl, handler) => ctl.Toggled += handler,
                (ctl, handler) => ctl.Toggled -= handler);
        }
    }

    public partial class Slider
    {
        public Expression<Func<xf.ValueChangedEventArgs, Signal>> ValueChanged
        {
            set => SetEvent(nameof(ValueChanged), value,
                (ctl, handler) => ctl.ValueChanged += handler,
                (ctl, handler) => ctl.ValueChanged -= handler);
        }
    }

    public partial class CheckBox : View<xf.CheckBox>
    {
        public Expression<Func<xf.CheckedChangedEventArgs, Signal>> CheckedChanged
        {
            set => SetEvent(nameof(CheckedChanged), value,
                (ctl, handler) => ctl.CheckedChanged += handler,
                (ctl, handler) => ctl.CheckedChanged -= handler);
        }
    }

    public partial class DatePicker
    {
        public Expression<Func<xf.DateChangedEventArgs, Signal>> DateSelected
        {
            set => SetEvent(nameof(DateSelected), value,
                (ctl, handler) => ctl.DateSelected += handler,
                (ctl, handler) => ctl.DateSelected -= handler);
        }
    }

    public partial class Picker
    {
        public IList<string> Items
        {
            set => SetValue(xf.Picker.ItemsSourceProperty, value);
        }
        
        public Expression<Func<int, Signal>> SelectedIndexChanged
        {
            set => SetEvent(nameof(SelectedIndexChanged), value, 
                (ctl, handler) => ctl.SelectedIndexChanged += (s, e) => handler(s, ((xf.Picker)s).SelectedIndex),
                (ctl, handler) => ctl.SelectedIndexChanged -= (s, e) => handler(s, ((xf.Picker)s).SelectedIndex));
        }
    }
}