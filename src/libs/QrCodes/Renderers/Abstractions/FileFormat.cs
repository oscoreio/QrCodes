namespace QrCodes.Renderers;

/// <summary>
/// 
/// </summary>
public enum FileFormat
{
    /// <summary>
    /// The BMP image format. <br/>
    /// Available in SkiaSharp and ImageSharp.
    /// </summary>
    Bmp,
    
    /// <summary>
    /// The GIF image format. <br/>
    /// Available in SkiaSharp and ImageSharp.
    /// </summary>
    Gif,
    
    /// <summary>
    /// The ICO image format. <br/>
    /// Available in SkiaSharp.
    /// </summary>
    Ico,
    
    /// <summary>
    /// The JPEG image format. <br/>
    /// Available in SkiaSharp and ImageSharp.
    /// </summary>
    Jpeg,
    
    /// <summary>
    /// The PNG image format. <br/>
    /// Available in SkiaSharp and ImageSharp.
    /// </summary>
    Png,
    
    /// <summary>
    /// The WBMP image format. <br/>
    /// Available in SkiaSharp.
    /// </summary>
    Wbmp,
    
    /// <summary>
    /// The WEBP image format. <br/>
    /// Available in SkiaSharp and ImageSharp.
    /// </summary>
    Webp,
    
    /// <summary>
    /// The PKM image format. <br/>
    /// Available in SkiaSharp.
    /// </summary>
    Pkm,
    
    /// <summary>
    /// The KTX image format. <br/>
    /// Available in SkiaSharp.
    /// </summary>
    Ktx,
    
    /// <summary>
    /// The ASTC image format. <br/>
    /// Available in SkiaSharp.
    /// </summary>
    Astc,
    
    /// <summary>
    /// The Adobe DNG image format. <br/>
    /// Available in SkiaSharp.
    /// </summary>
    Dng,
    
    /// <summary>
    /// The HEIF or High Efficiency Image File format. <br/>
    /// Available in SkiaSharp.
    /// </summary>
    Heif,
    
    /// <summary>
    /// V1 Image File Format (AVIF) is an open,
    /// royalty-free image file format specification
    /// for storing images or image sequences compressed
    /// with AV1 in the HEIF container format. <br/>
    /// Available in SkiaSharp.
    /// </summary>
    Avif,
    
    /// <summary>
    /// Portable bitmap format. <br/>
    /// Available in ImageSharp.
    /// </summary>
    Pbm,
        
    /// <summary>
    /// Truevision TGA, often referred to as TARGA, is a raster graphics file format. <br/>
    /// Available in ImageSharp.
    /// </summary>
    Tga,
        
    /// <summary>
    /// Tagged Image File Format, a file format for storing images. <br/>
    /// Available in ImageSharp.
    /// </summary>
    Tiff,
}