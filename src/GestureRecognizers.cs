using System.Collections.Generic;
using xf = Xamarin.Forms;

namespace Laconic
{
    public interface IGestureRecognizer
    {
        Dictionary<string, EventInfo> Events { get; }
        xf.BindableObject CreateReal();
    }

    public class TapGestureRecognizer : Element<xf.TapGestureRecognizer>, IGestureRecognizer
    {
        public int NumberOfTapsRequired
        {
            set => SetValue(xf.TapGestureRecognizer.NumberOfTapsRequiredProperty, value);
        }

        public System.Func<Signal> Tapped
        {
            set => SetEvent(nameof(Tapped), value,
                (ctl, handler) => ctl.Tapped += handler,
                (ctl, handler) => ctl.Tapped -= handler);
        }

        internal override xf.BindableObject CreateReal() => new xf.TapGestureRecognizer();

        xf.BindableObject IGestureRecognizer.CreateReal() => CreateReal();
    }
}