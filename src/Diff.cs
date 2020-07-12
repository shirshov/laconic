using System;
using System.Collections.Generic;
using System.Linq;
using Laconic.Shapes;
using xf = Xamarin.Forms;
using PropDict = System.Collections.Generic.Dictionary<Xamarin.Forms.BindableProperty, object>;
using EventDict = System.Collections.Generic.Dictionary<string, Laconic.EventInfo>;

namespace Laconic
{
    static class Diff
    {
        static IEnumerable<IDiffOperation> CalcPropertyDiff(PropDict existingValues, PropDict newValues)
        {
            foreach (var p in newValues) {
                if (existingValues.TryGetValue(p.Key, out var val)) {
                    // Picker doesn't like items being changed while SelectedIndexChanged is being fired
                    if (p.Key == xf.Picker.ItemsSourceProperty) {
                        var oldItems = (IList<string>) val;
                        var newItems = (IList<string>) p.Value;
                        if (!newItems.SequenceEqual(oldItems))
                            yield return new SetProperty(p.Key, p.Value);
                    }
                    else if (p.Key == xf.VisualElement.ClipProperty && !val.Equals(p.Value))
                        yield return new SetClip((Geometry) p.Value);
                    else if (!val.Equals(p.Value))
                        yield return new SetProperty(p.Key, p.Value);
                }
                else {
                    if (p.Key == xf.VisualElement.ClipProperty)
                        yield return new SetClip((Geometry) p.Value);
                    yield return (new SetProperty(p.Key, p.Value));
                }
            }

            foreach (var existing in existingValues.Where(p => !newValues.ContainsKey(p.Key)))
                yield return new ResetProperty(existing.Key);
        }

        static IEnumerable<IDiffOperation> CalcEventDiff(EventDict existingEvents, EventDict newEvents)
        {
            var diffs = new List<IDiffOperation>();

            foreach (var pair in existingEvents) {
                if (!newEvents.ContainsKey(pair.Key) || newEvents[pair.Key] == null)
                    diffs.Add(new UnwireEvent(pair.Key, pair.Value.Unsubscribe));
            }
            foreach (var pair in newEvents.Where(x => x.Value != null)) {
                var (eventName, newEvt) = (pair.Key, pair.Value);
                diffs.Add(new WireEvent(eventName, newEvt.SignalMaker, newEvt.Subscribe));
            }

            return diffs;
        }

        static IEnumerable<IDiffOperation> CalcGestureRecognizerDiff(
            Dictionary<Key, IGestureRecognizer> existingRecognizers,
            Dictionary<Key, IGestureRecognizer> newRecognizers)
        {
            if (existingRecognizers.Count == 0) {
                foreach (var rec in newRecognizers.Values) {
                    yield return new AddGestureRecognizer(rec, Calculate(null, (Element) rec));
                }
            }
            else {
                var listDiff = new ListDiff<Key, Key>(
                    existingRecognizers.Keys,
                    newRecognizers.Select(x => x.Key).ToArray());

                var index = 0;
                foreach (var action in listDiff.Actions) {
                    if (action.ActionType == ListDiffActionType.Add) {
                        index++;
                        var newRecog = newRecognizers[action.DestinationItem];
                        yield return new AddGestureRecognizer(newRecog, Calculate(null, (Element) newRecog));
                    }
                    else if (action.ActionType == ListDiffActionType.Remove) {
                        yield return new RemoveGestureRecognizer(index);
                    }
                    else {
                        var existingRecog = existingRecognizers[action.DestinationItem];
                        var newRecog = newRecognizers[action.DestinationItem];
                        var ops = Calculate((Element) existingRecog, (Element) newRecog);
                        if (ops.Any())
                            yield return new UpdateGestureRecognizer(index, ops);
                        index++;
                    }
                }
            }
        }

        static IEnumerable<IDiffOperation> CalcToolbarItemsDiff(IDictionary<Key, ToolbarItem> existingItems, 
            IDictionary<Key, ToolbarItem> newItems)
        {
            if (existingItems.Count == 0) {
                foreach (var tb in newItems.Values) {
                    yield return new AddToolbarItem(tb, Calculate(null, tb));
                }
            }
            else {
                var listDiff = new ListDiff<Key, Key>(
                    existingItems.Keys,
                    newItems.Select(x => x.Key).ToArray());

                var index = 0;
                foreach (var action in listDiff.Actions) {
                    if (action.ActionType == ListDiffActionType.Add) {
                        index++;
                        var newItem = newItems[action.DestinationItem];
                        yield return new AddToolbarItem(newItem, Calculate(null, newItem));
                    }
                    else if (action.ActionType == ListDiffActionType.Remove) {
                        yield return new RemoveToolbarItem(index);
                    }
                    else {
                        var existingItem = existingItems[action.DestinationItem];
                        var newItem = newItems[action.DestinationItem];
                        var ops = Calculate(existingItem, newItem);
                        if (ops.Any())
                            yield return new UpdateToolbarItem(index, ops);
                        index++;
                    }
                }
            }
        }
        
        public static IEnumerable<IDiffOperation> Calculate(IElement? existingElement, IElement newElement)
        {
            var operations = new List<IDiffOperation>();
            if (newElement == null)
                return operations;

            operations.AddRange(CalcPropertyDiff(existingElement?.ProvidedValues ?? new PropDict(),
                newElement.ProvidedValues));
            operations.AddRange(CalcEventDiff(existingElement?.Events ?? new EventDict(), newElement.Events));
            if (newElement is View v)
                operations.AddRange(CalcGestureRecognizerDiff(
                    (existingElement as View)?.GestureRecognizers ?? new Dictionary<Key, IGestureRecognizer>(),
                    v.GestureRecognizers));

            if (newElement is ContentPage p) {
                operations.AddRange(CalcToolbarItemsDiff(
                    (existingElement as ContentPage)?.ToolbarItems ?? new Dictionary<Key, ToolbarItem>(),
                    p.ToolbarItems
                ));
            }

            switch (newElement) {
                case IContentHost newViewAsContainer: {
                    var oldContent = (existingElement as IContentHost)?.Content;
                    var newContent = newViewAsContainer.Content;
                    IDiffOperation? op = (oldContent, newContent) switch {
                        (null, null) => null,
                        (null, var n) => new SetContent(n, Calculate(null, n)),
                        (_, null) => new RemoveContent(),
                        var (o, n) when o.GetType() != n.GetType() => new SetContent(n, Calculate(null, n)),
                        var (o, n) => new UpdateContent(Calculate(o, n))
                    };
                    if (op != null)
                        operations.Add(op);
                    break;
                }
                case ILayout l: {
                    var diff = ViewListDiff.Calculate((existingElement as ILayout)?.Children, l.Children);
                    if (diff.Count > 0)
                        operations.Add(new UpdateChildren(diff));
                    break;
                }
                case CollectionView c: {
                    var diff = ViewListDiff.Calculate((existingElement as CollectionView)?.Items, c.Items);
                    if (diff.Any())
                        operations.Add(new UpdateItems(diff));
                    break;
                }
            }

            if (newElement is Grid g) {
                var rowDefDiff = GridDiff.CalculateRowDefinitionsDiff(existingElement as Grid, g);
                if (rowDefDiff != null)
                    operations.Add(rowDefDiff);

                var colDefDiff = GridDiff.CalculateColumnDefinitionsDiff(existingElement as Grid, g);
                if (colDefDiff != null)
                    operations.Add(colDefDiff);
            }

            return operations;
        }
    }

    interface IDiffOperation
    {
    }

    class AddGestureRecognizer : IDiffOperation
    {
        public readonly Element Blueprint;
        public readonly IEnumerable<IDiffOperation> Operations;

        public AddGestureRecognizer(IGestureRecognizer blueprint, IEnumerable<IDiffOperation> operations) =>
            (Blueprint, Operations) = ((Element) blueprint, operations);
    }

    class RemoveGestureRecognizer : IDiffOperation
    {
        public readonly int Index;

        public RemoveGestureRecognizer(int index) => Index = index;
    }

    class UpdateGestureRecognizer : IDiffOperation
    {
        public readonly int Index;
        public readonly IEnumerable<IDiffOperation> Operations;

        public UpdateGestureRecognizer(int index, IEnumerable<IDiffOperation> operations) =>
            (Index, Operations) = (index, operations);
    }

    class AddToolbarItem : IDiffOperation
    {
        public readonly ToolbarItem Blueprint;
        public readonly IEnumerable<IDiffOperation> Operations;

        public AddToolbarItem(ToolbarItem blueprint, IEnumerable<IDiffOperation> operations) =>
            (Blueprint, Operations) = (blueprint, operations);
    }

    class RemoveToolbarItem : IDiffOperation
    {
        public readonly int Index;

        public RemoveToolbarItem(int index) => Index = index;
    }

    class UpdateToolbarItem : IDiffOperation
    {
        public readonly int Index;
        public readonly IEnumerable<IDiffOperation> Operations;

        public UpdateToolbarItem(int index, IEnumerable<IDiffOperation> operations) =>
            (Index, Operations) = (index, operations);
    }
    
    class SetProperty : IDiffOperation
    {
        public readonly xf.BindableProperty Property;
        public readonly object Value;

        internal SetProperty(xf.BindableProperty property, object value)
        {
            Property = property;
            Value = value;
        }

        public override string ToString() => $"SetProperty: Property={Property}, Value={Value}";
    }

    class ResetProperty : IDiffOperation
    {
        public readonly xf.BindableProperty Property;
        internal ResetProperty(xf.BindableProperty property) => Property = property;
    }

    class SetContent : IDiffOperation
    {
        public readonly View ContentView;
        public readonly IEnumerable<IDiffOperation> Operations;

        public SetContent(View contentView, IEnumerable<IDiffOperation> operations) =>
            (ContentView, Operations) = (contentView, operations);
    }

    class UpdateContent : IDiffOperation
    {
        public readonly IEnumerable<IDiffOperation> Operations;
        public UpdateContent(IEnumerable<IDiffOperation> operations) => Operations = operations;
    }

    class RemoveContent : IDiffOperation
    {
    }

    class WireEvent : IDiffOperation
    {
        public readonly string EventName;
        public readonly Func<EventArgs, Signal> SignalMaker;
        public readonly Action<xf.BindableObject, EventHandler> Subscribe;

        public WireEvent( string eventName, Func<EventArgs, Signal> signalMaker, 
            Action<xf.BindableObject, EventHandler> subscribe)
        {
            EventName = eventName;
            SignalMaker = signalMaker;
            Subscribe = subscribe;
        }
    }
    
    class UnwireEvent : IDiffOperation
    {
        public readonly string EventName;
        public readonly Action<xf.BindableObject, EventHandler> Unsubscribe;

        public UnwireEvent(string eventName, Action<xf.BindableObject, EventHandler> unsubscribe) => 
            (EventName, Unsubscribe) = (eventName, unsubscribe);
    }

    class SetClip : IDiffOperation
    {
        public readonly Geometry Geometry;

        public SetClip(Geometry geometry) => Geometry = geometry;
    }
}