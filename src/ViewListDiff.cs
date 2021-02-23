using System.Collections.Generic;
using System.Linq;

namespace Laconic
{
    static class ViewListDiff
    {
        public static ListOperation[] Calculate(IDictionary<Key, View?>? existingItems, IDictionary<Key, View?> newItems)
        {
            string GetReuseKey(Key key)
            {
                if (!(newItems is ItemsViewList))
                    return "SAME_REUSE_KEY";
                ((ItemsViewList) newItems).ReuseKeys.TryGetValue(key, out var reuseKey);
                return reuseKey ?? "SAME_REUSE_KEY";
            }

            var res = new List<ListOperation>();
            if (existingItems == null || existingItems.Count == 0) {
                foreach (var item in newItems.Where(p => p.Value != null)) {
                    var childOps = Diff.Calculate(null, (Element)item.Value!)
                        .Concat(CalculatePositioningInParentDiff(item.Key, existingItems, newItems));
                        res.Add(new AddChild(item.Key, GetReuseKey(item.Key), res.Count, (Element)item.Value!, childOps.ToArray()));
                }
            }
            else
            {
                var listDiff = new ListDiff<Key, Key>(
                    existingItems.Where(x => x.Value != null).Select(x => x.Key),
                    newItems.Where(p => p.Value != null).Select(p => p.Key).ToArray());

                var index = 0;
                foreach (var action in listDiff.Actions)
                {
                    if (action.ActionType == ListDiffActionType.Add)
                    {
                        var newItem = newItems[action.DestinationItem];
                        var childOps = Diff.Calculate(null, (Element)newItem!)
                            .Concat(CalculatePositioningInParentDiff(action.DestinationItem, existingItems, newItems));
                        res.Add(
                                new AddChild(action.DestinationItem, 
                                    GetReuseKey(action.DestinationItem), 
                                    index, 
                                    (Element)newItem!, childOps.ToArray())
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
                                .Calculate(null, (Element)newView!)
                                .Concat(CalculatePositioningInParentDiff(action.DestinationItem, 
                                    existingItems,
                                    newItems));
                                res.Add(new AddChild(action.SourceItem, 
                                    GetReuseKey(action.DestinationItem), 
                                    index, (Element)newView!, items.ToArray())
                                );
                        }
                        else if (existingView.GetType() != newView!.GetType())
                        {
                            res.Add(new ReplaceChild(index, (Element)newView, 
                                Diff.Calculate(null, (Element)newView)
                                .Concat(CalculatePositioningInParentDiff(action.SourceItem, existingItems, newItems))
                                .ToArray()));
                        }
                        else {
                            var patch = Diff.Calculate((Element)existingView, (Element)newView)
                                .Concat(CalculatePositioningInParentDiff(action.SourceItem, existingItems, newItems))
                                .ToArray();
                            if (patch.Any())
                                res.Add(new UpdateChild(action.DestinationItem, index, (Element)newView, patch.ToArray()));
                        }

                        index++;
                    }
                }
            }

            return res.ToArray();
        }
        
        static IEnumerable<DiffOperation> CalculatePositioningInParentDiff(
            Key key, 
            IDictionary<Key, View?>? existingList,
            IDictionary<Key, View?> newList) => newList switch {
                GridViewList gvl => GridDiff.CalculatePositioningInGrid(key, (GridViewList?) existingList, gvl),
                AbsoluteLayoutViewList avl => AbsoluteLayoutDiff.Calculate(key, (AbsoluteLayoutViewList?)existingList, avl),
                _ => new DiffOperation[0]
        };
    }
}