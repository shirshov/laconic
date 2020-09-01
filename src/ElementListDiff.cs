using System.Collections.Generic;
using System.Linq;

namespace Laconic
{
    static class ElementListDiff
    {
        public static ListOperation[] Calculate(IDictionary<Key, Element>? existingItems,
            IDictionary<Key, Element> newItems, ExpandWithContext expandWithContext)
        {
            var res = new List<ListOperation>();
                
            if (existingItems == null || existingItems.Count == 0) {
                foreach (var item in newItems.Where(p => p.Value != null)) {
                    var childOps = Diff.Calculate(null, item.Value, expandWithContext).ToArray();
                    if (item.Value is IContextElement withContext)
                        res.Add(new AddChildWithContext(item.Key, "TODO: Refactor", res.Count, (View) item.Value,
                            withContext.ContextId, childOps));
                    else
                        res.Add(new AddChild(item.Key, "TODO: Refactor", res.Count, item.Value, childOps));
                }
            }
            else {

                var listDiff = new ListDiff<Key, Key>(
                    existingItems.Where(x => x.Value != null).Select(x => x.Key),
                    newItems.Where(p => p.Value != null).Select(p => p.Key).ToArray());

                var index = 0;
                foreach (var action in listDiff.Actions) {
                    if (action.ActionType == ListDiffActionType.Add) {
                        var newItem = newItems[action.DestinationItem];
                        var childOps = Diff.Calculate(null, newItem, expandWithContext);
                        res.Add(
                            new AddChild(action.DestinationItem,
                                "TODO: Refactor this",
                                index,
                                newItem!, childOps.ToArray())
                        );
                        index++;
                    }
                    else if (action.ActionType == ListDiffActionType.Remove) {
                        res.Add(new RemoveChild(index));
                    }
                    else {
                        var existingView = existingItems[action.SourceItem];
                        var newView = newItems[action.SourceItem];
                        if (existingView == null) {
                            var items = Diff
                                .Calculate(null, newView, expandWithContext);
                            res.Add(new AddChild(action.SourceItem,
                                "TODO: Refactor this",
                                index, (View) newView, items.ToArray())
                            );
                        }
                        else if (existingView.GetType() != newView.GetType()) {
                            res.Add(new ReplaceChild(index, newView,
                                Diff.Calculate(null, newView, expandWithContext)
                                    .ToArray()));
                        }
                        else {
                            var patch = Diff.Calculate(existingView, newView, expandWithContext)
                                .ToArray();
                            if (patch.Any())
                                res.Add(new UpdateChild(action.DestinationItem, index, newView,
                                    patch.ToArray()));
                        }

                        index++;
                    }
                }
            }

            return res.ToArray();
        }
    }
}