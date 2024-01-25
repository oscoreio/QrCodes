using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Bmp;
using SixLabors.ImageSharp.Formats.Gif;
using SixLabors.ImageSharp.Formats.Pbm;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;

namespace QrCodes.Renderers;

/// <summary>
/// 
/// </summary>
public static class ImageExtensions
{
    /// <summary>
    /// Gets the hexadecimal representation of the color instance in #rrggbbaa form.
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string ToHexWithPrefix(
        this Color color)
    {
        return $"#{color.ToHex()}";
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="image"></param>
    /// <param name="fileFormat"></param>
    /// <param name="quality"></param>
    /// <returns></returns>
    public static byte[] ToBytes(
        this Image image,
        FileFormat fileFormat = FileFormat.Png,
        int quality = 100)
    {
        using var stream = new MemoryStream();
        image.ToStream(stream, fileFormat, quality);
        
        return stream.ToArray();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="image"></param>
    /// <param name="fileFormat"></param>
    /// <param name="stream"></param>
    /// <param name="quality"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void ToStream(
        this Image image,
        Stream stream,
        FileFormat fileFormat = FileFormat.Png,
        int quality = 100)
    {
        image = image ?? throw new ArgumentNullException(nameof(image));

        var encoder = fileFormat switch
        {
            FileFormat.Png => (IImageEncoder)new PngEncoder(),
            FileFormat.Bmp => new BmpEncoder(),
            FileFormat.Gif => new GifEncoder(),
            FileFormat.Jpeg => new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder
            {
                Quality = quality,
            },
            FileFormat.Webp => new WebpEncoder
            {
                Quality = quality,
            },
            FileFormat.Pbm => new PbmEncoder(),
            FileFormat.Tga => new SixLabors.ImageSharp.Formats.Tga.TgaEncoder(),
            FileFormat.Tiff => new SixLabors.ImageSharp.Formats.Tiff.TiffEncoder(),
            
            FileFormat.Ico => throw new NotSupportedException("Not supported."),
            FileFormat.Wbmp => throw new NotSupportedException("Not supported."),
            FileFormat.Pkm => throw new NotSupportedException("Not supported."),
            FileFormat.Ktx => throw new NotSupportedException("Not supported."),
            FileFormat.Astc => throw new NotSupportedException("Not supported."),
            FileFormat.Dng => throw new NotSupportedException("Not supported."),
            FileFormat.Heif => throw new NotSupportedException("Not supported."),
            FileFormat.Avif => throw new NotSupportedException("Not supported."),
            
            _ => throw new ArgumentOutOfRangeException(nameof(fileFormat), fileFormat, null),
        };
        
        image.Save(stream, encoder);
    }
}