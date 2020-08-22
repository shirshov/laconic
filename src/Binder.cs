using System;
using System.Collections.Generic;
using System.Linq;
using xf = Xamarin.Forms;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using ContextRequestList = System.Collections.Generic.List<(Laconic.IContextElement ContextElement, Laconic.LocalContext Context, Laconic.Element Rendered)>; 
                
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

    public class Binder<TState>
    {
        class TrackedElement
        {
            public readonly WeakReference<xf.VisualElement> View;
            public readonly Element RenderedBlueprint;
            public readonly Func<TState, Element> BlueprintMaker;

            public TrackedElement(WeakReference<xf.VisualElement> view, Element renderedBlueprint, Func<TState, Element> blueprintMaker)
            {
                View = view;
                RenderedBlueprint = renderedBlueprint;
                BlueprintMaker = blueprintMaker;
            }

            public TrackedElement With(Element newBlueprint) => new TrackedElement(View, newBlueprint, BlueprintMaker);
        }

        class LocalContextInfo
        {
            public readonly WeakReference<xf.BindableObject> View;
            public readonly Element RenderedBlueprint;
            public readonly Func<LocalContext, Element> BlueprintMaker;
            public readonly LocalContext Context;
            
            public LocalContextInfo(WeakReference<xf.BindableObject> view, Element renderedBlueprint, 
                Func<LocalContext, Element> blueprintMaker, LocalContext context)
            {
                View = view;
                RenderedBlueprint = renderedBlueprint;
                BlueprintMaker = blueprintMaker;
                Context = context;
            }

            public LocalContextInfo With(Element newBlueprint) 
                => new LocalContextInfo(View, newBlueprint, BlueprintMaker, Context);
        }
    
        readonly Func<TState, Signal, TState> _mainReducer;

        Func<MiddlewareContext<TState>, Func<MiddlewareContext<TState>, MiddlewareContext<TState>>, MiddlewareContext<TState>> _middlewarePipeline;

        readonly List<TrackedElement> _trackedElements = new List<TrackedElement>();

        readonly  Dictionary<IContextElement, LocalContextInfo> _elementContexts 
            = new Dictionary<IContextElement, LocalContextInfo>();
        readonly Channel<Signal> _channel;
        readonly SynchronizationContext _synchronizationContext;

        internal Binder(TState initialState, Func<TState, Signal, TState> mainReducer, SynchronizationContext synchronizationContext)
        {
            State = initialState;
            _mainReducer = mainReducer;
            _middlewarePipeline = (c, n) => n(c);
            _synchronizationContext = synchronizationContext;

            _channel = Channel.CreateUnbounded<Signal>(new UnboundedChannelOptions {SingleReader = true});
            Task.Run(async () => {
                while (true) {
                    var s = await _channel.Reader.ReadAsync();
                    ProcessSignal(s);
                }
            });
        }

        public TState State { get; private set; }

        public T CreateElement<T>(Func<TState, VisualElement<T>> blueprintMaker) where T : xf.VisualElement, new()
        {
            var blueprint = (Element)blueprintMaker(State);
            var view = new T();

            var contextRequests = new ContextRequestList();
            
            var diff = Diff.Calculate(null, blueprint, (e, n) => ExpandWithContext(e, n, _elementContexts, contextRequests));

            var newElementsWithContext = Patch.Apply(view, diff, Send);
            
            if (blueprint is IContextElement withContext)
                newElementsWithContext.Add((withContext.ContextId, view));
            
            UpdateElementContexts(contextRequests, newElementsWithContext);
            _trackedElements.Add(new TrackedElement(new WeakReference<xf.VisualElement>(view), blueprint, blueprintMaker));
            
            return view;
        }

        public void Send(Signal? signal)
        {
            if (signal == null)
                return;
            
            if (_synchronizationContext is TestSynchronizationContext)
                ProcessSignal(signal);
            else 
                _channel.Writer.WriteAsync(signal);
        }

        static (IElement?, IElement) ExpandWithContext(IContextElement? existingElement, 
            IContextElement newElement,
            IReadOnlyDictionary<IContextElement, LocalContextInfo> contexts,
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

            IElement? existingExpanded = null;
            if (existingElement != null && contexts.ContainsKey(existingElement))
                existingExpanded = contexts[existingElement].RenderedBlueprint;
            
            return (existingExpanded, newBlueprint);
        }

        void UpdateElementContexts(ContextRequestList contextRequests, 
            IEnumerable<(Guid ContextId, xf.BindableObject Element)> newElementsWithContext)
        {
            var infos = from req in contextRequests
                join real in newElementsWithContext on req.Context.Id equals real.ContextId
                select new {
                    req.ContextElement, 
                    ContextInfo = new LocalContextInfo(
                        new WeakReference<xf.BindableObject>(real.Element), 
                        req.Rendered, 
                        req.ContextElement.Make,
                        req.Context)
                };
            
            foreach (var info in infos)
                _elementContexts[info.ContextElement] = info.ContextInfo;
        }
        
        internal void ProcessSignal(Signal signal)
        {
            var contextRequests = new ContextRequestList();
            
            if (signal is ILocalContextSignal sig) {
                var kvp = _elementContexts.First(p => p.Value.Context.Id == sig.Id);
                var (contextElement, info) = (kvp.Key, kvp.Value);
                
                info.Context.SetValue(LocalContext.LOCAL_STATE_KEY, sig.Payload);

                var newBlueprint = info.BlueprintMaker(info.Context);
                var diff = Diff.Calculate(info.RenderedBlueprint, newBlueprint, 
                    (e, n) => ExpandWithContext(e, n, _elementContexts, contextRequests));

                _synchronizationContext.Send(_ => {
                    var isViewAlive = info.View.TryGetTarget(out var view); 
                    if (!isViewAlive)
                        throw new InvalidOperationException("View with local context was disposed.");
                    var newElementsWithContext = Patch.Apply(view, diff, Send);
                    
                    UpdateElementContexts(contextRequests, newElementsWithContext);
                    _elementContexts[contextElement] = info.With(newBlueprint); 
                }, null);
            } else {
                var context = _middlewarePipeline(
                    new MiddlewareContext<TState>(State, signal), 
                    c => new MiddlewareContext<TState>(_mainReducer(c.State, signal), signal));

                // This runs on the main thread. 
                // TODO: do diffing on the background one
                _synchronizationContext.Send(_ => {

                    var copy = _trackedElements.ToArray();
                    _trackedElements.Clear();
                    var aliveTrackedElements = new List<TrackedElement>();
                    
                    foreach (var el in copy) {
                        if (!el.View.TryGetTarget(out var aliveView)) continue;
                        
                        aliveTrackedElements.Add(el);

                        var newBlueprint = el.BlueprintMaker(context.State);
                        
                        // TODO: Diffs should run on the background thread
                        var diff = Diff.Calculate(el.RenderedBlueprint, newBlueprint,
                            (e, n) => ExpandWithContext(e, n, _elementContexts, contextRequests));

                        var newElementsWithContext = Patch.Apply(aliveView, diff, Send);

                        UpdateElementContexts(contextRequests, newElementsWithContext);
                        _trackedElements.Add(el.With(newBlueprint));
                    }
                    State = context.State;
                }, null);
            }
        }

        public void UseMiddleware(Func<MiddlewareContext<TState>,
            Func<MiddlewareContext<TState>, MiddlewareContext<TState>>,
            MiddlewareContext<TState>> middleware)
        {
            var currentPipeline = _middlewarePipeline;
            _middlewarePipeline = (c, n) => currentPipeline(c, c1 => middleware(c1, n));
        }
    }

    class TestSynchronizationContext : SynchronizationContext
    {
        public override void Send(SendOrPostCallback d, object state) => d.Invoke(null);
    }
    
    public static class Binder
    {
        public static Binder<TState> Create<TState>(TState initialState, Func<TState, Signal, TState> mainReducer)
        {
            var binder = new Binder<TState>(initialState, mainReducer, SynchronizationContext.Current);
            return binder;
        }

        internal static Binder<TState> CreateForTest<TState>(TState initialState, Func<TState, Signal, TState> mainReducer)
        {
            var binder = new Binder<TState>(initialState, mainReducer, new TestSynchronizationContext());
            return binder;
        }
    }
}