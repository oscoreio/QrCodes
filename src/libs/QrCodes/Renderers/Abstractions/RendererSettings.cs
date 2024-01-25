using System.Drawing;

namespace QrCodes.Renderers.Abstractions;

#pragma warning disable CA1819

/// <summary>
/// 
/// </summary>
public class RendererSettings
{
    /// <summary>
    /// Amount of px each dark/light module of the QR code shall take place in the final QR code image.
    /// </summary>
    public int PixelsPerModule { get; set; } = 5;
    
    /// <summary>
    /// Color of the dark modules. Default - Black.
    /// </summary>
    public Color DarkColor { get; set; } = Color.Black;
    
    /// <summary>
    /// Color of the light modules. Default - White.
    /// </summary>
    public Color LightColor { get; set; } = Color.White;
    
    /// <summary>
    /// If true a white border is drawn around the whole QR Code. Default - true.
    /// </summary>
    public bool DrawQuietZones { get; set; } = true;
    
    /// <summary>
    /// 
    /// </summary>
    public byte[]? IconBytes { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public int IconSizePercent { get; set; } = 15;
    
    /// <summary>
    /// 
    /// </summary>
    public int IconBorderWidth { get; set; }
    
    /// <summary>
    /// A bitmap object that will be used as background picture
    /// </summary>
    public byte[]? BackgroundBytes { get; set; }
    
    /// <summary>
    /// Optional image that should be used instead of the default finder patterns.
    /// </summary>
    public byte[]? FinderPatternBytes { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public BackgroundType DotStyle { get; set; } = BackgroundType.Rectangle;
    
    /// <summary>
    /// Connect the dots with lines. Default - true.
    /// </summary>
    public bool ConnectDots { get; set; } = true;
    
    /// <summary>
    /// 
    /// </summary>
    public BackgroundType BackgroundType { get; set; } = BackgroundType.Circle;
    
    /// <summary>
    /// Style of the background image (if set). <br/>
    /// None = default <br/>
    /// Fill = spanning complete graphic <br/>
    /// DataAreaOnly = Don't paint background into quiet zone <br/>
    /// Logo = Only behind logo <br/>
    /// </summary>
    public BackgroundImageStyle BackgroundImageStyle { get; set; } = BackgroundImageStyle.None;
    
    /// <summary>
    /// Style of the quiet zones. <br/>
    /// </summary>
    public QuietZoneStyle QuietZoneStyle { get; set; } = QuietZoneStyle.Dotted;
    
    /// <summary>
    /// Color of the background. Default - Transparent.
    /// </summary>
    public Color BackgroundColor { get; set; } = Color.Transparent;
    
    /// <summary>
    /// Value between 0.0 to 1.0 that defines how big the module dots are. <br/>
    /// The bigger the value, the less round the dots will be. <br/>
    /// Default - 1.0.
    /// </summary>
    public double PixelSizeFactor { get; set; } = 1.0;
    
    /// <summary>
    /// Color of the icon background. Default - Transparent.
    /// </summary>
    public Color IconBackgroundColor { get; set; } = Color.Transparent;
    
    /// <summary>
    /// 
    /// </summary>
    public FileFormat FileFormat { get; set; } = FileFormat.Png;
    
    /// <summary>
    /// 
    /// </summary>
    public int Quality { get; set; } = 100;
}