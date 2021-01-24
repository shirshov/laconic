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
                foreach (var (key, el) in newItems.Where(p => p.Value != null)) {
                    var childOps = Diff.Calculate(null, el, expandWithContext).ToArray();
                    // TODO: Refactor. Reuse key shouldn't be necessary when it's not used
                    if (el is IContextElement withContext)
                        res.Add(new AddChildWithContext(key, "", res.Count, (View) el, withContext.ContextId, childOps));
                    else
                        res.Add(new AddChild(key, "", res.Count, el, childOps));
                }
            }
            else {

                var listDiff = new ListDiff<Key, Key>(
                    existingItems.Where(x => x.Value != null).Select(x => x.Key),
                    newItems.Where(p => p.Value != null).Select(p => p.Key));

                var index = 0;
                foreach (var action in listDiff.Actions) {
                    if (action.ActionType == ListDiffActionType.Add) {
                        var newItem = newItems[action.DestinationItem];
                        var childOps = Diff.Calculate(null, newItem, expandWithContext).ToArray();
                        res.Add(new AddChild(action.DestinationItem, "TODO: Refactor this", index, newItem!, childOps));
                        index++;
                    }
                    else if (action.ActionType == ListDiffActionType.Remove) {
                        res.Add(new RemoveChild(index));
                    }
                    else {
                        var existingView = existingItems[action.SourceItem];
                        var newView = newItems[action.SourceItem];
                        if (existingView == null) {
                            var items = Diff.Calculate(null, newView, expandWithContext).ToArray();
                            res.Add(new AddChild(action.SourceItem, "TODO: Refactor this", index, newView, items));
                        }
                        else if (existingView.GetType() != newView.GetType()) {
                            var ops = Diff.Calculate(null, newView, expandWithContext).ToArray();
                            res.Add(new ReplaceChild(index, newView, ops));
                        }
                        else {
                            var patch = Diff.Calculate(existingView, newView, expandWithContext).ToArray();
                            if (patch.Any())
                                res.Add(new UpdateChild(action.DestinationItem, index, newView, patch));
                        }

                        index++;
                    }
                }
            }

            return res.ToArray();
        }
    }
}