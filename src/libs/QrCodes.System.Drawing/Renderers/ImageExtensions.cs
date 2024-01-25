using System.Drawing;
using System.Drawing.Imaging;

namespace QrCodes.Renderers;

/// <summary>
/// 
/// </summary>
public static class ImageExtensions
{
    private static void Save(
        this Image image,
        Stream stream,
        FileFormat fileFormat,
        int quality = 100)
    {
        image.Save(
            stream: stream,
            format: fileFormat switch
            {
                FileFormat.Bmp => ImageFormat.Bmp,
                FileFormat.Gif => ImageFormat.Gif,
                FileFormat.Ico => throw new NotSupportedException("ICO is not supported by System.Drawing."),
                FileFormat.Jpeg => ImageFormat.Jpeg,
                FileFormat.Png => ImageFormat.Png,
                FileFormat.Wbmp => throw new NotSupportedException("Wbmp is not supported by System.Drawing."),
                FileFormat.Webp => throw new NotSupportedException("Webp is not supported by System.Drawing."),
                FileFormat.Pkm => throw new NotSupportedException("Pkm is not supported by System.Drawing."),
                FileFormat.Ktx => throw new NotSupportedException("Ktx is not supported by System.Drawing."),
                FileFormat.Astc => throw new NotSupportedException("Astc is not supported by System.Drawing."),
                FileFormat.Dng => throw new NotSupportedException("Dng is not supported by System.Drawing."),
                FileFormat.Heif => throw new NotSupportedException("Heif is not supported by System.Drawing."),
                FileFormat.Avif => throw new NotSupportedException("Avif is not supported by System.Drawing."),
                FileFormat.Pbm => throw new NotSupportedException("PBM is not supported by System.Drawing."),
                FileFormat.Tga => throw new NotSupportedException("TGA is not supported by System.Drawing."),
                FileFormat.Tiff => ImageFormat.Tiff,
                _ => throw new ArgumentOutOfRangeException(nameof(fileFormat), fileFormat, null)
            });
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="image"></param>
    /// <param name="fileFormat"></param>
    /// <param name="quality"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static Stream ToStream(
        this Image image,
        FileFormat fileFormat,
        int quality = 100)
    {
        image = image ?? throw new ArgumentNullException(nameof(image));

        var stream = new MemoryStream();
        image.Save(stream, fileFormat, quality);
        
        return stream;
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
        FileFormat fileFormat,
        int quality = 100)
    {
        image = image ?? throw new ArgumentNullException(nameof(image));
        
        using var stream = new MemoryStream();
        image.Save(stream, fileFormat, quality);
        
        return stream.ToArray();
    }
}