using System.Drawing;
using SkiaSharp;

namespace QrCodes.Renderers;

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
    public static SKColor ToSkiaSharpColor(this Color color)
    {
        return new SKColor(
            red: color.R,
            green: color.G,
            blue: color.B,
            alpha: color.A);
    }
}