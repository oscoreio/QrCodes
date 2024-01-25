using SkiaSharp;

namespace QrCodes.Renderers;

/// <summary>
/// 
/// </summary>
public static class ImageExtensions
{
    private static SKData Encode(
        this SKImage image,
        FileFormat fileFormat,
        int quality = 100)
    {
        return image.Encode(
            format: fileFormat switch
            {
                FileFormat.Bmp => SKEncodedImageFormat.Bmp,
                FileFormat.Gif => SKEncodedImageFormat.Gif,
                FileFormat.Ico => SKEncodedImageFormat.Ico,
                FileFormat.Jpeg => SKEncodedImageFormat.Jpeg,
                FileFormat.Png => SKEncodedImageFormat.Png,
                FileFormat.Wbmp => SKEncodedImageFormat.Wbmp,
                FileFormat.Webp => SKEncodedImageFormat.Webp,
                FileFormat.Pkm => SKEncodedImageFormat.Pkm,
                FileFormat.Ktx => SKEncodedImageFormat.Ktx,
                FileFormat.Astc => SKEncodedImageFormat.Astc,
                FileFormat.Dng => SKEncodedImageFormat.Dng,
                FileFormat.Heif => SKEncodedImageFormat.Heif,
                FileFormat.Avif => SKEncodedImageFormat.Avif,
                FileFormat.Pbm => throw new NotSupportedException("PBM is not supported by SkiaSharp."),
                FileFormat.Tga => throw new NotSupportedException("TGA is not supported by SkiaSharp."),
                FileFormat.Tiff => throw new NotSupportedException("TIFF is not supported by SkiaSharp."),
                _ => throw new ArgumentOutOfRangeException(nameof(fileFormat), fileFormat, null)
            },
            quality: quality);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="image"></param>
    /// <param name="fileFormat"></param>
    /// <param name="quality"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static Stream ToStream(
        this SKImage image,
        FileFormat fileFormat,
        int quality = 100)
    {
        image = image ?? throw new ArgumentNullException(nameof(image));
        
        return image
            .Encode(fileFormat, quality)
            .AsStream();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="image"></param>
    /// <param name="fileFormat"></param>
    /// <param name="quality"></param>
    /// <returns></returns>
    public static byte[] ToBytes(
        this SKImage image,
        FileFormat fileFormat,
        int quality = 100)
    {
        image = image ?? throw new ArgumentNullException(nameof(image));
        
        return image
            .Encode(fileFormat, quality)
            .ToArray();
    }
}