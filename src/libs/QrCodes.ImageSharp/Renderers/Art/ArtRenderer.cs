using SixLabors.ImageSharp;

namespace QrCodes.Renderers.Art;

/// <summary>
/// 
/// </summary>
public static class ArtRenderer
{
    /// <summary>
    /// Renders an art-style QR code with dots as modules and various user settings
    /// </summary>
    /// <param name="data"></param>
    /// <param name="pixelsPerModule">Amount of px each dark/light module of the QR code shall take place in the final QR code image</param>
    /// <param name="darkColor">Color of the dark modules. Default - Black</param>
    /// <param name="lightColor">Color of the light modules. Default - White</param>
    /// <param name="backgroundColor">Color of the background. Default - Transparent</param>
    /// <param name="backgroundImage">A bitmap object that will be used as background picture</param>
    /// <param name="pixelSizeFactor">Value between 0.0 to 1.0 that defines how big the module dots are. The bigger the value, the less round the dots will be.</param>
    /// <param name="drawQuietZones">If true a white border is drawn around the whole QR Code</param>
    /// <param name="quietZoneRenderingStyle">Style of the quiet zones</param>
    /// <param name="backgroundImageStyle">Style of the background image (if set). Fill=spanning complete graphic; DataAreaOnly=Don't paint background into quietzone</param>
    /// <param name="finderPatternImage">Optional image that should be used instead of the default finder patterns</param>
    /// <returns>QRCode graphic as bitmap</returns>
    public static Image GetGraphic(
        QrCode data,
        int pixelsPerModule,
        Color? darkColor = null,
        Color? lightColor = null,
        Color? backgroundColor = null,
        Image? backgroundImage = null,
        double pixelSizeFactor = 0.8, 
        bool drawQuietZones = true,
        QuietZoneStyle quietZoneRenderingStyle = QuietZoneStyle.Dotted, 
        BackgroundImageStyle backgroundImageStyle = BackgroundImageStyle.DataAreaOnly,
        Image? finderPatternImage = null)
    {
        darkColor ??= Color.Black;
        lightColor ??= Color.White;
        backgroundColor ??= Color.Transparent;
        
        throw new NotImplementedException();
    }

}