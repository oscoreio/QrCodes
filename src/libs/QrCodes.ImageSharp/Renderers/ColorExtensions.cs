using SixLabors.ImageSharp;

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
    public static Color ToImageSharpColor(this System.Drawing.Color color)
    {
        return Color.FromRgba(
            r: color.R,
            g: color.G,
            b: color.B,
            a: color.A);
    }
}