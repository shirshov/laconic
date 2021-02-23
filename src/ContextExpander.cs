using System;
using System.Collections.Generic;
using System.Linq;
using xf = Xamarin.Forms;

namespace Laconic
{
    record ExpansionInfo(LocalContext Context, Element Blueprint, Func<LocalContext, Element> BlueprintMaker);
    
    static class ContextExpander
    {
        public static (Element, IEnumerable<ExpansionInfo>) Expand(
            Element element, IEnumerable<ExpansionInfo> contexts,
            Action<Signal> dispatch) => Process(element is IContextElement {ContextKey: { }} ce1 ? $".[{ce1.ContextKey}]" : ".", element, contexts, dispatch);

        static (Element, IEnumerable<ExpansionInfo>) Process(
            string keyPath,
            Element element,
            IEnumerable<ExpansionInfo> existingInfos,
            Action<Signal> dispatch)
        {
            var usedInfos = new List<ExpansionInfo>();
                
            if (element is IContentHost host) {
                var content = host.Content;
                var contentKey = content is IContextElement {ContextKey: { }} ce1 ? $"/Content[{ce1.ContextKey}]" : "/Content";
                var (el, con) = Process(keyPath + contentKey, (Element)content, existingInfos, dispatch);
                host.Content = (View)el;
                usedInfos.AddRange(con);
            }

            if (element is ILayout l) {
                foreach (var (key, child) in l.Children.Select(x => (x.Key, x.Value)).ToArray()) {
                    var (el, con) = Process(keyPath + "/" + ((child as IContextElement)?.ContextKey ?? key), 
                        (Element) child, existingInfos, dispatch);
                    l.Children[key] = (View)el;
                    usedInfos.AddRange(con);
                }
            }

            if (element is FlyoutPage fp) {
                var flyout = fp.Flyout;
                var contentKey =  flyout is IContextElement {ContextKey: { }} f ? $"/Flyout[{f.ContextKey}]" : "/Flyout";
                var (el, con) = Process(keyPath + contentKey, (Element)flyout, existingInfos, dispatch);
                fp.Flyout = (ContentPage)el;
                usedInfos.AddRange(con);
                
                var detail = fp.Detail;
                contentKey = detail is IContextElement {ContextKey: { }} d ? $"/Detail[{d.ContextKey}]" : "/Detail";
                (el, con) = Process(keyPath + contentKey, detail, existingInfos, dispatch);
                if (detail is IContextElement)
                    detail.ContextKey = keyPath + contentKey;
                fp.Detail = (ContentPage)el;
                usedInfos.AddRange(con);
            }
            
            if (element is IContextElement ce) {
                var existing = existingInfos.FirstOrDefault(x => x.Context.Key == keyPath);
                if (existing == null) {
                    var context = new LocalContext(dispatch, keyPath);
                    var newEl = ce.Make(context);
                    newEl.ContextKey = keyPath;
                    usedInfos.Add(new(context, newEl, ce.Make));
                    element = newEl;
                }
                else {
                    var el = ce.Make(existing.Context);
                    usedInfos.Add(existing);
                    element = el;
                }
            }
                    
            return (element, usedInfos);
        }
    }
}