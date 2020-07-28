using System.Collections.Generic;
using System.Linq;
using Laconic.CodeGeneration;

namespace Laconic
{
    [Union]
    interface __ListOperation
    {
        record AddChild(Key key, string reuseKey, int index, View blueprint, DiffOperation[] operations);
        record RemoveChild(int index);
        record UpdateChild(Key key, int index, View blueprint, IEnumerable<DiffOperation> operations);
        record ReplaceChild(int index, View newView, IEnumerable<DiffOperation> operations);
    }

    static class ViewListDiff
    {
        public static IReadOnlyList<ListOperation> Calculate(ViewList? existingItems, ViewList newItems)
        {
            string GetReuseKey(Key key)
            {
                if (!(newItems is ItemsViewList))
                    return "SAME_REUSE_KEY";
                ((ItemsViewList) newItems).ReuseKeys.TryGetValue(key, out var reuseKey);
                return reuseKey ?? "SAME_REUSE_KEY";
            }

            var res = new List<ListOperation>();
            if (existingItems == null || existingItems.Count == 0)
            {
                foreach (var item in newItems.Where(p => p.Value != null))
                    res.Add(new AddChild(item.Key, GetReuseKey(item.Key), res.Count, item.Value,
                        Diff.Calculate(null, item.Value)
                            .Concat(GridDiff.CalculateGridLayoutDiff(item.Key, existingItems, newItems)).ToArray()
                        ));
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
                        var childOps = Diff.Calculate(null, newItem)
                            .Concat(GridDiff.CalculateGridLayoutDiff(action.DestinationItem, existingItems, newItems));
                        res.Add(
                                new AddChild(action.DestinationItem, 
                                    GetReuseKey(action.DestinationItem), 
                                    index, 
                                    newItem, childOps.ToArray())
                        ); 
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
                        if (existingView == null) {
                            var items = Diff
                                .Calculate(null, newView)
                                .Concat(GridDiff.CalculateGridLayoutDiff(action.DestinationItem, 
                                    existingItems,
                                    newItems));
                                res.Add(new AddChild(action.SourceItem, 
                                    GetReuseKey(action.DestinationItem), 
                                    index, newView, items.ToArray())
                                );
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
                                res.Add(new UpdateChild(action.DestinationItem, index, newView, patch));
                        }

                        index++;
                    }
                }
            }

            return res;
        }
    }
}