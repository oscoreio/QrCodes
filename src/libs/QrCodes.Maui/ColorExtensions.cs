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
    public static System.Drawing.Color ToSystemDrawingColor(this Color color)
    {
        color = color ?? throw new ArgumentNullException(nameof(color));
        
        return System.Drawing.Color.FromArgb(
            alpha: (byte)(color.Alpha * 255),
            red: (byte)(color.Red * 255),
            green: (byte)(color.Green * 255),
            blue: (byte)(color.Blue * 255));
    }
}