using System.Collections.Generic;
using System.Linq;

namespace Laconic
{
    interface IListOperation
    {
    }

    class UpdateChildren : IDiffOperation
    {
        public readonly IReadOnlyList<IListOperation> Operations;
        public UpdateChildren(IReadOnlyList<IListOperation> operations) => Operations = operations;

        public override string ToString()
        {
            var res = "UpdateChildren:\n";
            foreach (var op in Operations)
                res += "  " + op + "\n";
            return res;
        }
    }

    class AddChild : IListOperation
    {
        public readonly int Index;
        public readonly View View;
        public readonly IReadOnlyList<IDiffOperation> Operations;
        public readonly string ReuseKey;

        public AddChild(int index, View view, IEnumerable<IDiffOperation> operations, string reuseKey) =>
            (Index, View, Operations, ReuseKey) = (index, view, operations.ToList(), reuseKey);
    }

    class RemoveChild : IListOperation
    {
        public readonly int Index;
        public RemoveChild(int index) => Index = index;
    }

    class UpdateChild : IListOperation
    {
        public readonly int Index;
        public readonly View View;
        public readonly IEnumerable<IDiffOperation> Operations;

        public UpdateChild(int index, View view, IEnumerable<IDiffOperation> operations) =>
            (Index, View, Operations) = (index, view, operations);

        public override string ToString()
        {
            var res = $"UpdateChild: [{Index}]\n";
            foreach (var op in Operations)
                res += "  " + op + "\n";
            return res;
        }
    }

    class ReplaceChild : IListOperation
    {
        public readonly int Index;
        public readonly View NewView;
        public readonly IEnumerable<IDiffOperation> Operations;

        public ReplaceChild(int index, View newView, IEnumerable<IDiffOperation> operations) =>
            (Index, NewView, Operations) = (index, newView, operations);
    }

    class UpdateItems : IDiffOperation
    {
        public readonly IReadOnlyList<IListOperation> Operations;
        public UpdateItems(IReadOnlyList<IListOperation> operations) => Operations = operations;

        public override string ToString()
        {
            var res = "UpdateItems:\n";
            foreach (var op in Operations)
                res += "  " + op + "\n";
            return res;
        }
    }

    static class ViewListDiff
    {
        public static IReadOnlyList<IListOperation> Calculate(ViewList? existingItems, ViewList newItems)
        {
            string GetReuseKey(Key key)
            {
                if (!(newItems is ItemsViewList))
                    return "SAME_REUSE_KEY";
                ((ItemsViewList) newItems).ReuseKeys.TryGetValue(key, out var reuseKey);
                return reuseKey ?? "SAME_REUSE_KEY";
            }

            var res = new List<IListOperation>();
            if (existingItems == null || existingItems.Count == 0)
            {
                foreach (var item in newItems.Where(p => p.Value != null))
                    res.Add(new AddChild(res.Count, item.Value,
                        Diff.Calculate(null, item.Value)
                            .Concat(GridDiff.CalculateGridLayoutDiff(item.Key, existingItems, newItems)),
                        GetReuseKey(item.Key)));
            }
            else
            {
                var listDiff = new ListDiff<Key, Key>(
                    existingItems.Keys,
                    newItems.Where(p => p.Value != null).Select(p => p.Key).ToArray());

                var index = 0;
                foreach (var action in listDiff.Actions)
                {
                    if (action.ActionType == ListDiffActionType.Add)
                    {
                        var newItem = newItems[action.DestinationItem];
                        res.Add(new AddChild(index, newItem, Diff.Calculate(null, newItem)
                                .Concat(GridDiff.CalculateGridLayoutDiff(action.DestinationItem, existingItems,
                                    newItems)),
                            GetReuseKey(action.DestinationItem)));
                        index++;
                    }
                    else if (action.ActionType == ListDiffActionType.Remove)
                    {
                        res.Add(new RemoveChild(index));
                    }
                    else
                    {
                        var existingView = existingItems[action.SourceItem];
                        var newView = newItems[action.SourceItem];
                        if (existingView == null)
                        {
                            res.Add(new AddChild(index, newView, Diff.Calculate(null, newView)
                                    .Concat(GridDiff.CalculateGridLayoutDiff(action.DestinationItem, existingItems,
                                        newItems)),
                                GetReuseKey(action.DestinationItem)));
                        }
                        else if (existingView.GetType() != newView.GetType())
                        {
                            res.Add(new ReplaceChild(index, newView, Diff.Calculate(null, newView)
                                .Concat(GridDiff.CalculateGridLayoutDiff(action.SourceItem, existingItems, newItems))));
                        }
                        else
                        {
                            var patch = Diff.Calculate(existingView, newView)
                                .Concat(GridDiff.CalculateGridLayoutDiff(action.SourceItem, existingItems, newItems))
                                .ToArray();
                            if (patch.Length != 0)
                                res.Add(new UpdateChild(index, newView, patch));
                        }

                        index++;
                    }
                }
            }

            return res;
        }
    }
}