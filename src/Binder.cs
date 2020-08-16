using System;
using System.Collections.Generic;
using System.Linq;
using xf = Xamarin.Forms;
using System.Runtime.CompilerServices;
using ContextRequestList = System.Collections.Generic.List<(Laconic.IContextElement ContextElement, Laconic.LocalContext Context, Laconic.IElement Rendered)>; 
using ContextDict = System.Collections.Generic.Dictionary<Laconic.IContextElement, Laconic.LocalContextInfo>;
                
[assembly: InternalsVisibleTo("LaconicTests")]

namespace Laconic
{
    public class MiddlewareContext<TState>
    {
        public readonly TState State;
        public readonly Signal Signal;

        internal MiddlewareContext(TState state, Signal signal) => (State, Signal) = (state, signal);

        public MiddlewareContext<TState> WithState(TState state) => new MiddlewareContext<TState>(state, Signal);
    }

    class LocalContextInfo
    {
        public readonly IElement Rendered;
        public readonly LocalContext Context;
        public readonly Func<LocalContext, Element> Make;
        public readonly xf.BindableObject View;
        
        public LocalContextInfo(IElement rendered, LocalContext context, Func<LocalContext, Element> make, xf.BindableObject view)
        {
            Rendered = rendered;
            Context = context;
            Make = make;
            View = view;
        }
    }
        
    public class Binder<TState>
    {
        readonly Func<TState, Signal, TState> _mainReducer;

        Func<MiddlewareContext<TState>, Func<MiddlewareContext<TState>, MiddlewareContext<TState>>, MiddlewareContext<TState>> _middlewarePipeline;

        readonly List<(xf.VisualElement realView, Element VirtualView, Func<TState, Element> viewDefinition)> _trackedElements 
            = new List<(xf.VisualElement realView, Element VirtualView, Func<TState, Element> viewDefinition)>();

        readonly ContextDict _elementContexts = new ContextDict();
        
        internal Binder(TState initialState, Func<TState, Signal, TState> mainReducer)
        {
            State = initialState;
            _mainReducer = mainReducer;
            _middlewarePipeline = (c, n) => n(c);
        }

        public TState State { get; private set; }

        void AddToTrackedElements(Element virtualView, xf.VisualElement realElement, Func<TState, Element> viewDefinition) 
            => _trackedElements.Add((realElement, virtualView, viewDefinition));

        static (IElement?, IElement) ExpandWithUncommittedContext(IContextElement? existingElement, IContextElement newElement,
            ContextDict contexts,
            ContextRequestList contextRequests)
        {
            LocalContext context;
            if (existingElement != null && contexts.ContainsKey(existingElement))
                context = contexts[existingElement].Context;
            else 
                context = new LocalContext();
            
            newElement.ContextId = context.Id;
            var newBlueprint = newElement.Make(context);
            contextRequests.Add((newElement, context, newBlueprint));
            return (null, newBlueprint);
        }

        void UpdateElementContexts(ContextRequestList contextRequests, List<(Guid ContextId, xf.BindableObject Element)> newElementsWithContext)
        {
            var infos = from req in contextRequests
                join real in newElementsWithContext on req.Context.Id equals real.ContextId
                select (req.ContextElement, new LocalContextInfo(req.Rendered, req.Context, req.ContextElement.Make, real.Element));
            
            foreach (var (key, value) in infos)
                _elementContexts[key] = value;
        }
        
        public T CreateElement<T>(Func<TState, VisualElement<T>> blueprintMaker)
            where T : xf.VisualElement, new()
        {
            var blueprint = (Element)blueprintMaker(State);
            var realView = new T();

            var contextRequests = new ContextRequestList();
            
            var diff = Diff.Calculate(null, blueprint, (e, n) => ExpandWithUncommittedContext(e, n, _elementContexts, contextRequests));

            var newElementsWithContext = Patch.Apply(realView, diff, Dispatch);
            
            if (blueprint is IContextElement withContext)
                newElementsWithContext.Add((withContext.ContextId, realView));
            
            UpdateElementContexts(contextRequests, newElementsWithContext);
            AddToTrackedElements(blueprint, realView, blueprintMaker);
            
            return realView;
        }

        public void Dispatch(Signal signal)
        {
            var contextRequests = new ContextRequestList();
            
            if (signal is ILocalContextSignal sig) {
                var kvp = _elementContexts.First(p => p.Value.Context.Id == sig.Id);
                var (contextElement, info) = (kvp.Key, kvp.Value);
                
                info.Context.SetValue(LocalContext.LOCAL_STATE_KEY, sig.Payload);

                var newBlueprint = info.Make(info.Context);
                var diff = Diff.Calculate(info.Rendered, newBlueprint, 
                    (e, n) => ExpandWithUncommittedContext(e, n, _elementContexts, contextRequests));
                
                var newElementsWithContext = Patch.Apply(info.View, diff, Dispatch);
                
                UpdateElementContexts(contextRequests, newElementsWithContext);
                _elementContexts[contextElement] = new LocalContextInfo(newBlueprint, info.Context, info.Make, info.View);
            } else {
                var context = _middlewarePipeline(
                    new MiddlewareContext<TState>(State, signal), 
                    c => new MiddlewareContext<TState>(_mainReducer(c.State, signal), signal));

                State = context.State;

                var items = _trackedElements.ToArray();
                for (var i = 0; i < items.Length; i++) {
                    var (realView, blueprint, blueprintFunc) = items[i];
                    
                    var newBlueprint = blueprintFunc(State);
                    
                    var diff = Diff.Calculate(blueprint, newBlueprint, 
                        (e, n) => ExpandWithUncommittedContext(e, n, _elementContexts, contextRequests));

                    var newElementsWithContext = Patch.Apply(realView, diff, Dispatch);
                    
                    UpdateElementContexts(contextRequests, newElementsWithContext);
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

    public static class Binder
    {
        public static Binder<TState> Create<TState>(TState initialState, Func<TState, Signal, TState> mainReducer)
        {
            var binder = new Binder<TState>(initialState, mainReducer);
            return binder;
        }
    }
}