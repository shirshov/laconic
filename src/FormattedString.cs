using System;
using System.Collections;
using System.Collections.Generic;
using xf = Xamarin.Forms;

namespace Laconic
{
    public partial class Span : Element<xf.Span>
    {
        protected internal override xf.BindableObject CreateView() => throw new NotImplementedException();
    }
    
    public class FormattedString : IConvert, IEnumerable<Span>
    {
        readonly List<Span> _spans = new List<Span>();
        
        public void Add(string text) => _spans.Add(new Span{Text = text});

        public void Add(Span span) => _spans.Add(span);

        public IEnumerator<Span> GetEnumerator() => throw new NotImplementedException();
        
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        object IConvert.ToNative()
        {
            // TODO: this is very inefficient. Must plug FormattedString into diff/patch properly.
            var ret = new xf.FormattedString();
            foreach (var span in _spans) {
                var newSpan = new xf.Span();
                foreach (var p in span.ProvidedValues)
                    newSpan.SetValue(p.Key, Patch.ConvertToNative(p.Value));
                ret.Spans.Add(newSpan);
            }

            return ret;
        }
    }
}