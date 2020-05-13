using System.Collections.Generic;
using System.Linq;
using xf = Xamarin.Forms;
using PropDict = System.Collections.Generic.Dictionary<Xamarin.Forms.BindableProperty, object>;
using EventDict = System.Collections.Generic.Dictionary<string, Laconic.EventInfo>;

namespace Laconic
{
    static class Diff
    {
        static IEnumerable<IDiffOperation> CalcPropertyDiff(PropDict existingValues, PropDict newValues)
        {
            foreach (var p in newValues)
            {
                if (existingValues.TryGetValue(p.Key, out var val))
                {
                    if (!val.Equals(p.Value))
                        yield return new SetProperty(p.Key, p.Value);
                }
                else
                {
                    yield return (new SetProperty(p.Key, p.Value));
                }
            }

            foreach (var existing in existingValues.Where(p => !newValues.ContainsKey(p.Key)))
                yield return new ResetProperty(existing.Key);
        }

        static IEnumerable<IDiffOperation> CalcEventDiff(EventDict existingEvents, EventDict newEvents)
        {
            foreach (var evt in newEvents)
            {
                if (existingEvents.TryGetValue(evt.Key, out var val))
                {
                    if (val.ToString() != evt.Value.ToString())
                        yield return new SetEvent(evt.Key, evt.Value);
                }
                else
                {
                    yield return new SetEvent(evt.Key, evt.Value);
                }
            }

            foreach (var existing in existingEvents.Where(evt => !newEvents.ContainsKey(evt.Key)))
                yield return new UnsetEvent(existing.Key);
        }

        static IEnumerable<IDiffOperation> CalcGestureRecognizerDiff(IList<IGestureRecognizer> existingRecognizers,
            IList<IGestureRecognizer> newRecognizers)

        {
            if (newRecognizers.SequenceEqual(existingRecognizers))
                return new IDiffOperation[0];

            return new[] {new SetGestureRecognizers(newRecognizers)};
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
                    (existingElement as View)?.GestureRecognizers ?? new List<IGestureRecognizer>(),
                    v.GestureRecognizers));

            switch (newElement)
            {
                case IContentHost newViewAsContainer:
                {
                    var oldContent = (existingElement as IContentHost)?.Content;
                    var newContent = newViewAsContainer.Content;
                    IDiffOperation? op = (oldContent, newContent) switch
                    {
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
                case ILayout l:
                {
                    var diff = ViewListDiff.Calculate((existingElement as ILayout)?.Children, l.Children);
                    if (diff.Count > 0)
                        operations.Add(new UpdateChildren(diff));
                    break;
                }
                case CollectionView c:
                {
                    var op = new UpdateItems(
                        ViewListDiff.Calculate((existingElement as CollectionView)?.Items, c.Items));
                    operations.Add(op);
                    break;
                }
            }

            if (newElement is Grid g)
            {
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

    class SetGestureRecognizers : IDiffOperation
    {
        public readonly IList<IGestureRecognizer> Recognizers;

        public SetGestureRecognizers(IList<IGestureRecognizer> recognizers) => Recognizers = recognizers;
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

    class SetEvent : IDiffOperation
    {
        public readonly string EventName;
        public readonly EventInfo Handler;

        public SetEvent(string eventName, EventInfo handler)
        {
            Handler = handler;
            EventName = eventName;
        }
    }

    class UnsetEvent : IDiffOperation
    {
        public readonly string EventName;

        public UnsetEvent(string eventName) => EventName = eventName;
    }
}