using System;
using System.Collections.Generic;
using System.Linq;
using xf = Xamarin.Forms;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LaconicTests")]

namespace Laconic
{
    public class Binder<TState>
    {
        readonly Func<TState, Signal, TState> _mainReducer;

        Func<MiddlewareContext<TState>, Func<MiddlewareContext<TState>,
            MiddlewareContext<TState>>, MiddlewareContext<TState>> _middlewarePipeline;

        internal Binder(TState initialState, Func<TState, Signal, TState> mainReducer)
        {
            State = initialState;
            _mainReducer = mainReducer;
            _middlewarePipeline = (c, n) => n(c);
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

        public TRealView CreateView<TRealView>(Func<TState, View<TRealView>> blueprintMaker)
            where TRealView : xf.View, new()
        {
            var realView = new TRealView();
            var virtualView = blueprintMaker(State);
            var diff = Diff.Calculate(null, virtualView);
            Patch.Apply(realView, diff, Dispatch);
            AddToTrackedElements(virtualView, realView, blueprintMaker);
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
            var context = _middlewarePipeline(new MiddlewareContext<TState>(State, signal), c =>
                new MiddlewareContext<TState>(_mainReducer(c.State, signal), signal));

            State = context.State;

            var items = _trackedElements.ToArray();
            for (var i = 0; i < items.Length; i++) {
                var (realView, blueprint, blueprintFunc) = items[i];
                var newBlueprint = blueprintFunc(State);
                var diff = Diff.Calculate(blueprint, newBlueprint);
                if (diff.Any()) {
                    Patch.Apply(realView, diff, Dispatch);
                    _trackedElements[i] = (realView, newBlueprint, blueprintFunc);
                }
            }
        }

        public void UseMiddleware(Func<MiddlewareContext<TState>,
            Func<MiddlewareContext<TState>, MiddlewareContext<TState>>,
            MiddlewareContext<TState>> middleware)
        {
            var oldPipeline = _middlewarePipeline;
            _middlewarePipeline = (c, n) => oldPipeline(c, c1 => middleware(c1, n));
        }
    }

    public class MiddlewareContext<TState>
    {
        internal MiddlewareContext(TState state, Signal signal) => (State, Signal) = (state, signal);

        public readonly TState State;
        public readonly Signal Signal;

        public MiddlewareContext<TState> WithState(TState state) => new MiddlewareContext<TState>(state, Signal);
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