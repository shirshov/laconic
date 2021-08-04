using System;
using System.Collections.Generic;
using System.Linq;
using xf = Xamarin.Forms;
using System.Runtime.ExceptionServices;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
                
[assembly: InternalsVisibleTo("LaconicTests")]
[assembly: InternalsVisibleTo("MapTests")]

namespace Laconic
{
    public class MiddlewareContext<TState>
    {
        public readonly TState State;
        public readonly Signal Signal;

        internal MiddlewareContext(TState state, Signal signal) => (State, Signal) = (state, signal);

        public MiddlewareContext<TState> WithState(TState state) => new(state, Signal);
    }

    // TODO: this is quick and dirty solution for NavigationPage, should be redesigned
    interface IDoDispatch
    {
        void SetDispatcher(Action<Signal> dispatch);
    }

    public class Binder<TState>
    {
        record TrackedElement(WeakReference<xf.VisualElement> View, Element RenderedBlueprint, Func<TState, Element> BlueprintMaker);
        record LocalContextInfo(LocalContext Context, Func<LocalContext, Element> BlueprintMaker, Element RenderedBlueprint, WeakReference<xf.BindableObject>? View);
        
        readonly Func<TState, Signal, TState> _mainReducer;
        Func<MiddlewareContext<TState>, Func<MiddlewareContext<TState>, MiddlewareContext<TState>>, MiddlewareContext<TState>> _middlewarePipeline;
        
        readonly List<TrackedElement> _trackedElements = new();
        Dictionary<string, LocalContextInfo> _elementContexts = new();
        
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
            var view = (xf.VisualElement)Patch.CreateView(blueprint, Send);
            var expansionInfo = _elementContexts.Values
                .Select(x => new ExpansionInfo(x.Context, x.RenderedBlueprint, x.BlueprintMaker));
            var (expandedRootElement, activeContexts) = ContextExpander.Expand(blueprint, expansionInfo, Send);
            // TODO: Diffs should run on the background thread
            var diff = Diff.Calculate(null, expandedRootElement);

            _suppressEvents = true;
            var newElementsWithContext = Patch.Apply(view, diff, Send).AsEnumerable();
            _suppressEvents = false;
            
            _trackedElements.Add(new TrackedElement(new WeakReference<xf.VisualElement>(view), expandedRootElement, blueprintMaker));
            
            // TODO: contexts should be per root element?
            var updatedContexts = new Dictionary<string, LocalContextInfo>();
            
            foreach (var info in activeContexts) {
                var realView = newElementsWithContext.FirstOrDefault(x => x.ContextKey == info.Context.Key).Element;
                if (realView == null) {
                    _elementContexts[info.Context.Key].View.TryGetTarget(out realView);
                }
                updatedContexts.Add(
                    info.Context.Key,
                    new LocalContextInfo(
                        info.Context,
                        info.BlueprintMaker,
                        info.Blueprint,
                        new WeakReference<xf.BindableObject>(realView)
                    )
                );
            }
            _elementContexts = updatedContexts;
            
            return (T)view;
        }

        bool _suppressEvents;

        public void Send(Signal? signal)
        {
            if (signal == null)
                return;
            
            if (_suppressEvents)
                return;

            if (_synchronizationContext is TestSynchronizationContext)
                ProcessSignal(signal);
            else 
                _channel.Writer.WriteAsync(signal);
        }

        internal void ProcessSignal(Signal signal)
        {
            if (signal is ILocalContextSignal sig) {
                var (_, info) = _elementContexts.First(p => p.Value.Context.Key == sig.ContextKey);
                
                var newBlueprint = info.BlueprintMaker(info.Context);
                var expansionInfos = _elementContexts.Values.Select(x => new ExpansionInfo(x.Context, x.RenderedBlueprint, x.BlueprintMaker));
                var (expanded, _) = ContextExpander.Expand(newBlueprint, expansionInfos, Send);
                var diff = Diff.Calculate(info.RenderedBlueprint, expanded);

                _synchronizationContext.Send(_ => {
                    var isViewAlive = info.View.TryGetTarget(out var view);
                    if (!isViewAlive) {
                        info.Context.Clear();
                        _elementContexts.Remove(sig.ContextKey);
                        return;
                    }

                    _suppressEvents = true;
                    Patch.Apply(view, diff, Send);
                    _suppressEvents = false;
                    
                    info.RenderedBlueprint.UpdateFrom(expanded);
                }, null);
            } else {
                ExceptionDispatchInfo? reducerException = null;
                MiddlewareContext<TState>? context = null;
                try {
                    context = _middlewarePipeline(
                        new MiddlewareContext<TState>(State, signal), 
                        c => new MiddlewareContext<TState>(_mainReducer(c.State, signal), signal));
                }
                catch (Exception ex)
                {
                    reducerException = ExceptionDispatchInfo.Capture(ex);
                }

                // This runs on the main thread. 
                // TODO: do diffing on the background one
                _synchronizationContext.Send(_ => {

                    if (reducerException != null)
                        reducerException.Throw();

                    var copy = _trackedElements.ToArray();
                    _trackedElements.Clear();
                    
                    foreach (var trackedRoot in copy) {
                        if (!trackedRoot.View.TryGetTarget(out var aliveView)) continue;

                        var expansionInfos = _elementContexts.Values.Select(x =>
                            new ExpansionInfo(x.Context, x.RenderedBlueprint, x.BlueprintMaker));
                        var (expandedRootElement, activeContexts) = ContextExpander.Expand(
                            trackedRoot.BlueprintMaker(context!.State), expansionInfos, Send);
                        // TODO: Diffs should run on the background thread
                        var diff = Diff.Calculate(trackedRoot.RenderedBlueprint, expandedRootElement);

                        _suppressEvents = true;
                        var newElementsWithContext = Patch.Apply(aliveView, diff, Send);
                        _suppressEvents = false;
                        
                        _trackedElements.Add(trackedRoot with {RenderedBlueprint = expandedRootElement});
                        
                        var updatedContexts = new Dictionary<string, LocalContextInfo>();
                        foreach (var info in activeContexts) {
                            var realView = newElementsWithContext.FirstOrDefault(x => x.ContextKey == info.Context.Key).Element;
                            if (realView == null) {
                                _elementContexts[info.Context.Key].View.TryGetTarget(out realView);
                            }
                            updatedContexts.Add(
                                info.Context.Key,
                                new LocalContextInfo(
                                    info.Context,
                                    info.BlueprintMaker,
                                    info.Blueprint,
                                    new WeakReference<xf.BindableObject>(realView)
                                )
                            );
                        }

                        foreach (var removed in _elementContexts.Keys.Where(x => !updatedContexts.ContainsKey(x)))
                            _elementContexts[removed].Context.Clear();
                        
                        _elementContexts = updatedContexts;
                    }
                    State = context!.State;
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