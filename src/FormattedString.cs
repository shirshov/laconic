using System.Collections;
using System.Collections.Generic;
using xf = Xamarin.Forms;

namespace Laconic
{
    public partial class Span : Element<xf.Span>
    {
        internal override xf.BindableObject CreateView() => throw new System.NotImplementedException();
    }
    
    public class FormattedString : Element<xf.FormattedString>, IEnumerable<Span>, IConvert
    {
        readonly List<Span> _spans = new List<Span>();
        
        public void Add(string text) => _spans.Add(new Span{Text = text});

        public void Add(Span span) => _spans.Add(span);

        public IEnumerator<Span> GetEnumerator() => throw new System.NotImplementedException();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        
        internal override xf.BindableObject CreateView() => throw new System.NotImplementedException();

        object IConvert.ToNative()
        {
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