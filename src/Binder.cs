using System;
using System.Collections.Generic;
using xf = Xamarin.Forms;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LaconicTests")]

namespace Laconic
{
    public class Binder<TState>
    {
        readonly Func<TState, Signal, TState> _mainReducer;

        internal Binder(TState initialState, Func<TState, Signal, TState> mainReducer)
        {
            State = initialState;
            _mainReducer = mainReducer;
        }

        public TState State { get; private set; }

        public xf.ContentPage CreatePage(Func<TState, ContentPage> viewDefinition)
        {
            var realPage = new xf.ContentPage();
            var virtualPage = viewDefinition(State);
            var diff = Diff.Calculate(null, virtualPage);
            Patch.Apply(realPage, diff, Dispatch);
            AddToTrackedElements(virtualPage, realPage, viewDefinition);
            return realPage;
        }

        public TRealView CreateView<TRealView>(Func<TState, View<TRealView>> viewDefinition)
            where TRealView : xf.View, new()
        {
            var realView = new TRealView();
            var virtualView = viewDefinition(State);
            var diff = Diff.Calculate(null, virtualView);
            Patch.Apply(realView, diff, Dispatch);
            AddToTrackedElements(virtualView, realView, viewDefinition);
            return realView;
        }

        readonly List<(xf.VisualElement realView, Element VirtualView, Func<TState, Element> viewDefinition)>
            _trackedElements
                = new List<(xf.VisualElement realView, Element VirtualView, Func<TState, Element> viewDefinition)>();

        void AddToTrackedElements(Element virtualView, xf.VisualElement realElement,
            Func<TState, Element> viewDefinition)
        {
            _trackedElements.Add((realElement, virtualView, viewDefinition));
        }

        public void Dispatch(Signal signal)
        {
            var newState = _mainReducer(State, signal);
            State = newState;

            var items = _trackedElements.ToArray();
            for (var i = 0; i < items.Length; i++)
            {
                var (realView, virtualView, viewFunction) = items[i];
                var newVirtualView = viewFunction(newState);
                var diff = Diff.Calculate(virtualView, newVirtualView);
                Patch.Apply(realView, diff, Dispatch);
                _trackedElements[i] = (realView, newVirtualView, viewFunction);
            }
        }
    }

    public static class Binder
    {
        public static Binder<TState> Create<TState>(TState initialState, Func<TState, Signal, TState> mainReducer)
        {
            var binder = new Binder<TState>(initialState, mainReducer);
            return binder;
        }
    }
}