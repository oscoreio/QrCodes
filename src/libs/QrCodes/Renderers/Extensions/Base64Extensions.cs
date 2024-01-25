using QrCodes.Renderers.Abstractions;

namespace QrCodes.Renderers.Extensions;

/// <summary>
/// 
/// </summary>
public static class Base64Extensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bytes"></param>
    /// <returns></returns>
    public static string ToBase64(this byte[] bytes)
    {
        return Convert.ToBase64String(bytes, Base64FormattingOptions.None);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="renderer"></param>
    /// <param name="data"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static string RenderToBase64(
        this IRenderer renderer,
        QrCode data,
        RendererSettings? settings = null)
    {
        renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
        
        return renderer
            .RenderToBytes(data, settings)
            .ToBase64();
    }
}