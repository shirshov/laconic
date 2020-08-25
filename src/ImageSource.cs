using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using xf = Xamarin.Forms;

namespace Laconic
{
    public abstract class ImageSource : IElement
    {
        readonly Dictionary<xf.BindableProperty, object>
            _providedValues = new Dictionary<xf.BindableProperty, object>();

        Dictionary<xf.BindableProperty, object> IElement.ProvidedValues => _providedValues;
        Dictionary<string, EventInfo> IElement.Events => throw new NotImplementedException();

        protected T GetValue<T>(xf.BindableProperty property) => (T) _providedValues[property];

        protected void SetValue(xf.BindableProperty property, object value) => _providedValues[property] = value;

        public static ImageSource FromFile(string file) => new FileImageSource {File = file};

        public static ImageSource FromResource(string resource, Type resolvingType) =>
            FromResource(resource, resolvingType.GetTypeInfo().Assembly);

        public static ImageSource FromResource(string resource, Assembly? sourceAssembly = null) =>
            throw new NotImplementedException();

        public static ImageSource FromStream(Func<Stream> stream) => throw new NotImplementedException();

        public static ImageSource FromUri(Uri uri) => new UriImageSource {Uri = uri};

        public static implicit operator ImageSource?(string source)
        {
            // Taken from xf.ImageSourceConverter.ConvertFromInvariantString(source)
            if (source == null)
                return null;

            return Uri.TryCreate(source, UriKind.Absolute, out var uri) && uri.Scheme != "file"
                ? FromUri(uri)
                : FromFile(source);
        }

        internal xf.ImageSource ToXamarinFormsImageSource()
        {
            switch (this) {
                case FontImageSource s:
                    var native = new xf.FontImageSource();
                    if (_providedValues.ContainsKey(xf.FontImageSource.FontFamilyProperty))
                        native.FontFamily = s.FontFamily;
                    if (_providedValues.ContainsKey(xf.FontImageSource.GlyphProperty))
                        native.Glyph = s.Glyph;
                    if (_providedValues.ContainsKey(xf.FontImageSource.SizeProperty))
                        native.Size = s.Size;
                    if (_providedValues.ContainsKey(xf.FontImageSource.ColorProperty))
                        native.Color = s.Color.ToXamarinFormsColor();
                    return native;
                case FileImageSource s:
                    return xf.ImageSource.FromFile(s.File);
                case UriImageSource s:
                    return xf.ImageSource.FromUri(s.Uri);
                default:
                    throw new NotImplementedException($"ImageSource {GetType()} is not implemented.");
            }
        }
    }

    public partial class FontImageSource : ImageSource
    {
    }

    public partial class FileImageSource : ImageSource
    {
    }

    public class UriImageSource : ImageSource
    {
        public Uri Uri {
            get => GetValue<Uri>(xf.UriImageSource.UriProperty);
            set => SetValue(xf.UriImageSource.UriProperty, value);
        }
    }
}