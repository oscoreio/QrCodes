using SixLabors.ImageSharp.PixelFormats;

namespace QrCodes.Maui;

/// <summary>
/// 
/// </summary>
public static class ColorExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="color"></param>
    /// <returns></returns>
    public static SixLabors.ImageSharp.Color ToImageSharpColor(this Color color)
    {
        color = color ?? throw new ArgumentNullException(nameof(color));
        
        return new SixLabors.ImageSharp.Color(new Rgba32(
            r: color.Red,
            g: color.Green,
            b: color.Blue,
            a: color.Alpha));
    }
}