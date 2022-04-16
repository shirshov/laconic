using System.IO;
using System.Reflection;

namespace Laconic;

public abstract class ImageSource : Element
{
    public static ImageSource FromFile(string file) => new FileImageSource {File = file};

    public static ImageSource FromResource(string resource, Type resolvingType) =>
        FromResource(resource, resolvingType.GetTypeInfo().Assembly);

    public static ImageSource FromResource(string resource, Assembly? sourceAssembly = null) =>
        throw new NotImplementedException();

    public static ImageSource FromStream(Func<Stream> stream) => throw new NotImplementedException();

    public static ImageSource FromUri(Uri uri) => new UriImageSource {Uri = uri};

    public static implicit operator ImageSource?(string? source)
    {
        // Taken from xf.ImageSourceConverter.ConvertFromInvariantString(source)
        if (source == null)
            return null;

        return Uri.TryCreate(source, UriKind.Absolute, out var uri) && uri.Scheme != "file"
            ? FromUri(uri)
            : FromFile(source);
    }
}

public partial class FontImageSource : ImageSource
{
    protected internal override xf.BindableObject CreateView() => new xf.FontImageSource();
}

public partial class FileImageSource : ImageSource
{
    protected internal override xf.BindableObject CreateView() => new xf.FileImageSource();
}

public class UriImageSource : ImageSource
{
    public Uri Uri {
        get => GetValue<Uri>(xf.UriImageSource.UriProperty);
        set => SetValue(xf.UriImageSource.UriProperty, value);
    }

    protected internal override xf.BindableObject CreateView() => new xf.UriImageSource();
}