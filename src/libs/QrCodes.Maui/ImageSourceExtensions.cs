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
        var value = result?.Value ?? throw new InvalidOperationException("Result value is null");
#if IOS || MACCATALYST
        return value.AsPNG()?.AsStream() ??
               throw new InvalidOperationException("AsPNG() returns null");
#elif ANDROID
        var bitmap = Android.Graphics.Bitmap.CreateBitmap(
            width: value.IntrinsicWidth,
            height: value.IntrinsicHeight,
            config: Android.Graphics.Bitmap.Config.Argb8888!);
        value.Draw(new Android.Graphics.Canvas(bitmap));
        
        var stream = new MemoryStream();
        
        await bitmap.CompressAsync(
            format: Android.Graphics.Bitmap.CompressFormat.Png!,
            quality: 100,
            stream: stream).ConfigureAwait(false);
        
        return stream;
#elif WINDOWS
        throw new PlatformNotSupportedException();
#else
        throw new PlatformNotSupportedException();
#endif
    }
}