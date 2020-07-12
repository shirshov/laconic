using System;
using System.Collections.Generic;
using xf = Xamarin.Forms;

namespace Laconic
{
    public partial class Button
    {
        public xf.Button.ButtonContentLayout ContentLayout {
            get => GetValue<xf.Button.ButtonContentLayout>(xf.Button.ContentLayoutProperty);
            set => SetValue(xf.Button.ContentLayoutProperty, value);
        }
    }

    public partial class Switch
    {
        public Func<xf.ToggledEventArgs, Signal> Toggled {
            set => SetEvent(nameof(Toggled), value,
                (ctl, handler) => ctl.Toggled += handler,
                (ctl, handler) => ctl.Toggled -= handler);
        }
    }

    public partial class Slider
    {
        public Func<xf.ValueChangedEventArgs, Signal> ValueChanged {
            set => SetEvent(nameof(ValueChanged), value,
                (ctl, handler) => ctl.ValueChanged += handler,
                (ctl, handler) => ctl.ValueChanged -= handler);
        }
    }

    public partial class CheckBox : View<xf.CheckBox>
    {
        public Func<xf.CheckedChangedEventArgs, Signal> CheckedChanged {
            set => SetEvent(nameof(CheckedChanged), value,
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
        public IList<string> Items
        {
            set => SetValue(xf.Picker.ItemsSourceProperty, value);
        }
        
        public Func<SelectedIndexChangedEventArgs, Signal> SelectedIndexChanged {
            set => SetEvent(nameof(SelectedIndexChanged), value, 
                (ctl, handler) => ctl.SelectedIndexChanged += (s, e) => 
                    handler(s, new SelectedIndexChangedEventArgs( ((xf.Picker)s).SelectedIndex)),
                (ctl, handler) => ctl.SelectedIndexChanged -= (s, e) => 
                    handler(s, new SelectedIndexChangedEventArgs( ((xf.Picker)s).SelectedIndex)));
        }
    }
}