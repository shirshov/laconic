using System.Collections.Generic;
using System.Linq;
using xf = Xamarin.Forms;
using PropDict = System.Collections.Generic.Dictionary<Xamarin.Forms.BindableProperty, object?>;
using EventDict = System.Collections.Generic.Dictionary<string, Laconic.EventInfo>;

namespace Laconic
{
    delegate (Element?, Element) ExpandWithContext(IContextElement? existingElement, IContextElement newElement);
    
    static class Diff
    {
        static IEnumerable<DiffOperation> CalcPropertyDiff(PropDict existingValues, PropDict newValues, 
            ExpandWithContext expandWithContext)
        {
            foreach (var newProp in newValues) {
                if (existingValues.TryGetValue(newProp.Key, out var existingPropValue)) {
                    // Picker doesn't like items being changed while SelectedIndexChanged is being fired
                    if (newProp.Key == xf.Picker.ItemsSourceProperty) {
                        var oldItems =  existingPropValue as IList<string> ?? new List<string>();
                        var newItems = (IList<string>) newProp.Value!;
                        if (!newItems.SequenceEqual(oldItems))
                            yield return new SetProperty(newProp.Key, newProp.Value!);
                    }
                    else if (newProp.Value is Element child) {
                        var childDiff = Calculate(existingPropValue as Element, child, expandWithContext).ToArray();
                        if (childDiff.Length > 0)
                            yield return new UpdateChildElement(newProp.Key, childDiff);
                    }
                    else if (existingPropValue == null & newProp.Value == null)
                        break;
                    else if (existingPropValue == null)
                        yield return new SetProperty(newProp.Key, newProp.Value!);
                    else if (!existingPropValue.Equals(newProp.Value))
                        yield return new SetProperty(newProp.Key, newProp.Value!);
                }
                else {
                    switch (newProp.Value)
                    {
                        case Element _ when newProp.Value == null:
                            yield return new SetChildElementToNull(newProp.Key);
                            break;
                        case Element child:
                            var ops = Calculate(null, child, expandWithContext).ToArray();
                            yield return new SetChildElement(newProp.Key, child.CreateView, ops);
                            break;
                        default: 
                            yield return (new SetProperty(newProp.Key, newProp.Value!));
                            break;
                    }
                }
            }

            foreach (var existing in existingValues.Where(p => !newValues.ContainsKey(p.Key)))
                yield return new ResetProperty(existing.Key);
        }

        static IEnumerable<DiffOperation> CalcEventDiff(EventDict existingEvents, EventDict newEvents)
        {
            var diffs = new List<DiffOperation>();

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

        static IEnumerable<DiffOperation> CalcGestureRecognizerDiff(
            Dictionary<Key, IGestureRecognizer> existingRecognizers,
            Dictionary<Key, IGestureRecognizer> newRecognizers,
            ExpandWithContext expandWithContext)
        {
            if (existingRecognizers.Count == 0) {
                foreach (var rec in newRecognizers.Values) {
                    yield return new AddGestureRecognizer(rec, Calculate(null, (Element) rec, expandWithContext).ToArray());
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
                        yield return new AddGestureRecognizer(newRecog, Calculate(null, (Element) newRecog, expandWithContext).ToArray());
                    }
                    else if (action.ActionType == ListDiffActionType.Remove) {
                        yield return new RemoveGestureRecognizer(index);
                    }
                    else {
                        var existingRecog = existingRecognizers[action.DestinationItem];
                        var newRecog = newRecognizers[action.DestinationItem];
                        var ops = Calculate((Element) existingRecog, (Element) newRecog, expandWithContext);
                        if (ops.Any())
                            yield return new UpdateGestureRecognizer(index, ops.ToArray());
                        index++;
                    }
                }
            }
        }

        static IEnumerable<DiffOperation> CalcToolbarItemsDiff(IDictionary<Key, ToolbarItem> existingItems, 
            IDictionary<Key, ToolbarItem> newItems,
            ExpandWithContext expandWithContext)
        {
            if (existingItems.Count == 0) {
                foreach (var tb in newItems.Values) {
                    yield return new AddToolbarItem(tb, Calculate(null, tb, expandWithContext).ToArray());
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
                        yield return new AddToolbarItem(newItem, Calculate(null, newItem, expandWithContext).ToArray());
                    }
                    else if (action.ActionType == ListDiffActionType.Remove) {
                        yield return new RemoveToolbarItem(index);
                    }
                    else {
                        var existingItem = existingItems[action.DestinationItem];
                        var newItem = newItems[action.DestinationItem];
                        var ops = Calculate(existingItem, newItem, expandWithContext);
                        if (ops.Any())
                            yield return new UpdateToolbarItem(index, ops.ToArray());
                        index++;
                    }
                }
            }
        }
        
        public static IEnumerable<DiffOperation> Calculate(Element? existingElement, Element newElement, 
             ExpandWithContext expandWithContext)
        {
            var operations = new List<DiffOperation>();
            if (newElement == null)
                return operations;

            if (newElement is IContextElement contextElement) {
                var (existingExpanded, newExpanded) = expandWithContext((IContextElement)existingElement!, contextElement);
                return Calculate((Element)existingExpanded!, newExpanded!, expandWithContext);
            }
            
            operations.AddRange(CalcPropertyDiff(existingElement?.ProvidedValues ?? new PropDict(),
                newElement.ProvidedValues, expandWithContext));
            operations.AddRange(CalcEventDiff(existingElement?.Events ?? new EventDict(), newElement.Events));
            
            if (newElement is View v)
                operations.AddRange(CalcGestureRecognizerDiff(
                    (existingElement as View)?.GestureRecognizers ?? new Dictionary<Key, IGestureRecognizer>(),
                    v.GestureRecognizers,
                    expandWithContext));

            if (newElement is ContentPage p) {
                operations.AddRange(CalcToolbarItemsDiff(
                    (existingElement as ContentPage)?.ToolbarItems ?? new Dictionary<Key, ToolbarItem>(),
                    p.ToolbarItems,
                    expandWithContext
                ));
            }

            foreach (var elList in newElement.ElementLists.Inner) {
                ElementListInfo? existingInfo = null;
                existingElement?.ElementLists?.Inner?.TryGetValue(elList.Key, out existingInfo);
                var newInfo = elList.Value;
                var ops = ElementListDiff.Calculate(existingInfo?.List, newInfo.List, expandWithContext);
                if (ops.Any())
                    operations.Add(new UpdateChildElementList(newInfo.ListGetter, ops));
            }

            switch (newElement) {
                case IContentHost newViewAsContainer: {
                    var oldContent = (existingElement as IContentHost)?.Content;
                    var newContent = newViewAsContainer.Content;
                    DiffOperation? op = (oldContent, newContent) switch {
                        (null, null) => null,
                        (null, var n) => new SetContent(n, Calculate(null, (Element)n, expandWithContext).ToArray()),
                        (_, null) => new RemoveContent(),
                        var (o, n) when o.GetType() != n.GetType() => new SetContent(n, Calculate(null, (Element)n, expandWithContext).ToArray()),
                        var (o, n) => new UpdateContent(Calculate((Element)o, (Element)n, expandWithContext).ToArray())
                    };
                    if (op != null)
                        operations.Add(op);
                    break;
                }
                case ILayout l: {
                    var diff = ViewListDiff.Calculate((existingElement as ILayout)?.Children, l.Children, expandWithContext);
                    if (diff.Length > 0)
                        operations.Add(new UpdateChildViews(diff.ToArray()));
                    break;
                }
                case IItemSourceView c: {
                    var diff = ViewListDiff.Calculate((existingElement as IItemSourceView)?.Items, c.Items, expandWithContext);
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
}