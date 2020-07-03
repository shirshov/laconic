using xf = Xamarin.Forms;
using Evt = System.Linq.Expressions.Expression<System.Func<Laconic.Signal>>;

namespace Laconic
{
    public interface IGestureRecognizer
    {
    }

    public class TapGestureRecognizer : Element<xf.TapGestureRecognizer>, IGestureRecognizer
    {
        public int NumberOfTapsRequired
        {
            set => SetValue(xf.TapGestureRecognizer.NumberOfTapsRequiredProperty, value);
        }

        public Evt Tapped
        {
            set => SetEvent(nameof(Tapped), value,
                (ctl, handler) => ctl.Tapped += handler,
                (ctl, handler) => ctl.Tapped -= handler);
        }

        internal override xf.BindableObject CreateReal() => new xf.TapGestureRecognizer();
    }
}