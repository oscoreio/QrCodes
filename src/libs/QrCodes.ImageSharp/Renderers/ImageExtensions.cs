using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;

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
    /// <param name="encoder"></param>
    /// <returns></returns>
    public static byte[] ToBytes(
        this Image image,
        IImageEncoder encoder)
    {
        image = image ?? throw new ArgumentNullException(nameof(image));
        
        using var stream = new MemoryStream();
        image.Save(stream, encoder);
        
        return stream.ToArray();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="image"></param>
    /// <param name="encoder"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<byte[]> ToBytesAsync(
        this Image image,
        IImageEncoder encoder,
        CancellationToken cancellationToken = default)
    {
        image = image ?? throw new ArgumentNullException(nameof(image));
        
        using var stream = new MemoryStream();
        
        await image.SaveAsync(
            stream: stream,
            encoder: encoder,
            cancellationToken: cancellationToken).ConfigureAwait(false);
        
        return stream.ToArray();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="image"></param>
    /// <param name="fileFormat"></param>
    /// <returns></returns>
    public static byte[] ToBytes(
        this Image image,
        FileFormat fileFormat)
    {
        using var stream = new MemoryStream();
        image.ToStream(fileFormat, stream);
        return stream.ToArray();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="image"></param>
    /// <param name="fileFormat"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static async Task<byte[]> ToBytesAsync(
        this Image image,
        FileFormat fileFormat,
        CancellationToken cancellationToken = default)
    {
        using var stream = new MemoryStream();
        
        await image.ToStreamAsync(
            fileFormat: fileFormat,
            stream: stream,
            cancellationToken: cancellationToken).ConfigureAwait(false);
        
        return stream.ToArray();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="image"></param>
    /// <param name="fileFormat"></param>
    /// <param name="stream"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void ToStream(
        this Image image,
        FileFormat fileFormat,
        Stream stream)
    {
        switch (fileFormat)
        {
            case FileFormat.Gif:
                image.SaveAsGif(stream);
                break;
            case FileFormat.Jpeg:
                image.SaveAsJpeg(stream);
                break;
            case FileFormat.Png:
                image.SaveAsPng(stream);
                break;
            case FileFormat.Bmp:
                image.SaveAsBmp(stream);
                break;
            case FileFormat.Pbm:
                image.SaveAsPbm(stream);
                break;
            case FileFormat.Tga:
                image.SaveAsTga(stream);
                break;
            case FileFormat.Tiff:
                image.SaveAsTiff(stream);
                break;
            case FileFormat.Webp:
                image.SaveAsWebp(stream);
                break;
                
            default:
                throw new ArgumentOutOfRangeException(nameof(fileFormat), fileFormat, null);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="image"></param>
    /// <param name="fileFormat"></param>
    /// <param name="stream"></param>
    /// <param name="cancellationToken"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static async Task ToStreamAsync(
        this Image image,
        FileFormat fileFormat,
        Stream stream,
        CancellationToken cancellationToken = default)
    {
        switch (fileFormat)
        {
            case FileFormat.Gif:
                await image.SaveAsGifAsync(stream, cancellationToken: cancellationToken).ConfigureAwait(false);
                break;
            case FileFormat.Jpeg:
                await image.SaveAsJpegAsync(stream, cancellationToken: cancellationToken).ConfigureAwait(false);
                break;
            case FileFormat.Png:
                await image.SaveAsPngAsync(stream, cancellationToken: cancellationToken).ConfigureAwait(false);
                break;
            case FileFormat.Bmp:
                await image.SaveAsBmpAsync(stream, cancellationToken: cancellationToken).ConfigureAwait(false);
                break;
            case FileFormat.Pbm:
                await image.SaveAsPbmAsync(stream, cancellationToken: cancellationToken).ConfigureAwait(false);
                break;
            case FileFormat.Tga:
                await image.SaveAsTgaAsync(stream, cancellationToken: cancellationToken).ConfigureAwait(false);
                break;
            case FileFormat.Tiff:
                await image.SaveAsTiffAsync(stream, cancellationToken: cancellationToken).ConfigureAwait(false);
                break;
            case FileFormat.Webp:
                await image.SaveAsWebpAsync(stream, cancellationToken: cancellationToken).ConfigureAwait(false);
                break;
                
            default:
                throw new ArgumentOutOfRangeException(nameof(fileFormat), fileFormat, null);
        }
    }
}