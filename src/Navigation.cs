using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using xf = Xamarin.Forms;

namespace Laconic
{
    public enum PresentationStyle { Normal, Modal, ModalFormSheet}
    
    record NavigationMetadata(object Data, PresentationStyle PresentationStyle);
    
    public class NavigationStack
    {
        internal readonly List<NavigationMetadata> Frames;
        
        public NavigationStack(params object[] frameData) => Frames = frameData.Select(f => new NavigationMetadata(f, PresentationStyle.Normal)).ToList();


        public NavigationStack Push(object data, PresentationStyle presentationStyle = PresentationStyle.Normal)
        {
            Frames.Add(new(data, presentationStyle));
            return this;
        }

        public NavigationStack Pop()
        {
            Frames.RemoveAt(Frames.Count - 1);
            return this;
        }
    }

    public class SwipeBackGestureSignal : Signal
    {
        internal SwipeBackGestureSignal() : base(null)
        {
        }
    }
    
    class SwipeBackGestureBehavior : Behavior<xf.NavigationPage>
    {
        readonly NavigationStack _stack;
        Action<Signal> _dispatch;

        public SwipeBackGestureBehavior(NavigationStack stack, Action<Signal> dispatch)
        {
            _stack = stack;
            _dispatch = dispatch;
        }

        void HandleBackGesture(object sender, xf.NavigationEventArgs e)
        {
            var meta = (NavigationMetadata)e.Page.GetValue(NavigationPage.LaconicNavigationMetadataProperty);
            if (meta.Data == _stack.Frames[^1].Data) {
                _stack.Pop();
                _dispatch(new SwipeBackGestureSignal());
            }
        }

        public override void OnAttachedTo(xf.NavigationPage bindable) => bindable.PopRequested += HandleBackGesture;
        public override void OnDetachingFrom(xf.NavigationPage bindable) => bindable.PopRequested -= HandleBackGesture;
    }
    
    public delegate VisualElement<xf.ContentPage> PageMakerDelegate(object data);

    public partial class NavigationPage : Page<xf.NavigationPage>, IDoDispatch
    {
        public static readonly xf.BindableProperty LaconicNavigationMetadataProperty = xf.BindableProperty.CreateAttached(
            "LaconicNavigationMetadata", typeof(NavigationMetadata), typeof(NavigationPage), null);

        Action<Signal>? _dispatch;

        public NavigationPage(NavigationStack stack, PageMakerDelegate pageMaker)
        {
            Behaviors.Add("swipe-back", new SwipeBackGestureBehavior(stack, s => _dispatch!(s)));

            var pages = ElementLists.Add<Xamarin.Forms.NavigationPage>( "Pages", real => new RealPagesAdapter(stack, real));
            foreach (var f in stack.Frames) {
                var p = pageMaker(f.Data);
                p.ProvidedValues[LaconicNavigationMetadataProperty] = f;
                pages.Add(f.Data.ToString(), p);
            }
        }
        
        public void SetDispatcher(Action<Signal> dispatch) => _dispatch = dispatch;
    }

    class RealPagesAdapter : IList
    {
        readonly xf.NavigationPage _page;
        readonly NavigationStack _stack;

        public RealPagesAdapter(NavigationStack stack, xf.NavigationPage page)
        {
            _stack = stack;
            _page = page;
        }

        public object this[int index] {
            get => _page.Pages.ElementAt(index);
            set => throw new NotSupportedException();
        }
        // TODO: Insert and Add should be handled differently: Add should call PushAsync, Insert manipulates Pages collection
        public int Add(object value) => throw new NotImplementedException();
        
        public void Insert(int index, object value)
        {
            var newPage = (xf.Page)value;
            var meta = _stack.Frames[index];
            newPage.SetValue(NavigationPage.LaconicNavigationMetadataProperty, meta);
            if (meta.PresentationStyle == PresentationStyle.Modal)
                _page.Navigation.PushModalAsync(newPage);
            else if (meta.PresentationStyle == PresentationStyle.ModalFormSheet) {
                newPage.On<xf.PlatformConfiguration.iOS>().SetModalPresentationStyle(UIModalPresentationStyle.FormSheet);
                _page.Navigation.PushModalAsync(newPage);
            }
            else
                _page.Navigation.PushAsync(newPage);
        }

        public void RemoveAt(int index)
        {
            // Only one modal page is allowed in stack
            if (_page.Navigation.ModalStack.Count == 1)
                _page.Navigation.PopModalAsync();
            
            // Swipe back, stack modified by the GestureBehavior:
            else if (index == _page.Pages.Count())
                return;
            
            // Pop requested in code
            _page.Navigation.PopAsync();
        }
        
        #region Disgusting

        public bool IsFixedSize => throw new NotSupportedException();
        public bool IsReadOnly => throw new NotSupportedException();
        public int Count => throw new NotSupportedException();
        public IEnumerator GetEnumerator() => throw new NotSupportedException();
        public void CopyTo(Array array, int index) => throw new NotSupportedException();
        public bool IsSynchronized => throw new NotSupportedException();
        public object SyncRoot => throw new NotSupportedException();
        public void Clear() => throw new NotSupportedException();
        public bool Contains(object value) => throw new NotSupportedException();
        public int IndexOf(object value) => throw new NotSupportedException();
        public void Remove(object value) => throw new NotSupportedException();

        #endregion
    }
}