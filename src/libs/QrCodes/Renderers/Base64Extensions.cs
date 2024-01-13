namespace QrCodes.Renderers;

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
}