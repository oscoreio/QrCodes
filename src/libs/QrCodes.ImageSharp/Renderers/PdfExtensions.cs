using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;

/* This renderer is inspired by RemusVasii: https://github.com/codebude/QRCoder/issues/223 */
namespace QrCodes.Renderers;

/// <summary>
/// 
/// </summary>
// ReSharper disable once InconsistentNaming
public static class QrCodeExtensions
{
    /// <summary>
    /// Creates a PDF document with given colors DPI and quality
    /// </summary>
    /// <param name="data"></param>
    /// <param name="stream"></param>
    /// <param name="pixelsPerModule"></param>
    /// <param name="darkColor"></param>
    /// <param name="lightColor"></param>
    /// <param name="dpi"></param>
    /// <param name="jpgQuality"></param>
    /// <returns></returns>
    public static void SaveAsPdf(
        this QrCode data,
        Stream stream,
        int pixelsPerModule,
        Color darkColor,
        Color lightColor,
        int dpi = 150,
        long jpgQuality = 85)
    {
        data = data ?? throw new ArgumentNullException(nameof(data));
        
        // Transform to JPG
        byte[]? jpgArray;
        using (var image = ImageSharpRenderer.Render(data, pixelsPerModule, darkColor, lightColor))
        {
            jpgArray = image.ToBytes(new JpegEncoder
            {
                Quality = (int)jpgQuality,
            });
        }
        
        var imageWidthAndHeight = data.ModuleMatrix.Count * pixelsPerModule;
        
        jpgArray.SaveAsPdf(stream, imageWidthAndHeight, dpi);
    }
}