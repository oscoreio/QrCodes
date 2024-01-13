namespace QrCodes.Maui;

/// <summary>
/// 
/// </summary>
public static class ImageSourceExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="imageSource"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static async Task<Stream> ToStreamAsync(this ImageSource imageSource)
    {
        if (Application.Current?.Handler?.MauiContext is not {} mauiContext)
        {
            throw new InvalidOperationException("MauiContext is null");
        }

        var result = await imageSource.GetPlatformImageAsync(mauiContext).ConfigureAwait(false);
#if IOS || MACCATALYST
        return result?.Value.AsPNG()?.AsStream() ??
               throw new InvalidOperationException("Result is null");
#else
        throw new NotImplementedException();
#endif
    }
}