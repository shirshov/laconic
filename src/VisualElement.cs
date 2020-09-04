using System;
using System.Collections.Generic;
using xf = Xamarin.Forms;

namespace Laconic
{
    public abstract partial class VisualElement<T> : Element<T> where T : xf.VisualElement, new()
    {
        // TODO: why is it here, and not on Element<T>?
        protected internal override xf.BindableObject CreateView() => new T();
        
        public Dictionary<Key, IGestureRecognizer> GestureRecognizers { get; } = new Dictionary<Key, IGestureRecognizer>();
        
        public VisualMarker Visual
        {
            get => GetValue<VisualMarker>(xf.VisualElement.VisualProperty);
            set => SetValue(xf.VisualElement.VisualProperty, value);
        }
    }

}		