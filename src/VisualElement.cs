using System.Collections;
using System.Collections.Generic;
using xf = Xamarin.Forms;

namespace Laconic
{
    public abstract partial class VisualElement<T> : Element<T> where T : xf.VisualElement, new()
    {
        // TODO: why is it here, and not on Element<T>?
        protected internal override xf.BindableObject CreateView() => new T();
        
        public Dictionary<Key, IGestureRecognizer> GestureRecognizers { get; } = new Dictionary<Key, IGestureRecognizer>();

        public VisualElement() => ElementLists.Add<xf.VisualElement>(nameof(Behaviors), element => (IList)element.Behaviors);

        public VisualMarker Visual
        {
            get => GetValue<VisualMarker>(xf.VisualElement.VisualProperty);
            set => SetValue(xf.VisualElement.VisualProperty, value);
        }
        
        public IBrush Background
        {
            get => GetValue<IBrush>(xf.VisualElement.BackgroundProperty);
            set => SetValue(xf.VisualElement.BackgroundProperty, value);
        }
        
        public ElementList Behaviors {
            get => ElementLists[nameof(Behaviors)];
            set => ElementLists[nameof(Behaviors)] = value;
        }
    }
}		