using System.Drawing;
using System.Drawing.Drawing2D;
using QrCodes.Renderers.Abstractions;

// ReSharper disable SuggestVarOrType_BuiltInTypes

namespace QrCodes.Renderers;

/// <summary>
/// 
/// </summary>
public class SystemDrawingRenderer : IRenderer
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static Bitmap Render(
        QrCode data,
        RendererSettings? settings = null)
    {
        data = data ?? throw new ArgumentNullException(nameof(data));
        settings ??= new RendererSettings();

        if (settings.PixelSizeFactor > 1)
        {
            throw new ArgumentException("The parameter pixelSize must be between 0 and 1. (0-100%)");
        }
        
        var pixelSize = (int)Math.Min(
            settings.PixelsPerModule,
            Math.Floor(settings.PixelsPerModule / settings.PixelSizeFactor));

        var numModules = data.ModuleMatrix.Count - (settings.DrawQuietZones ? 0 : 8);
        var offset = settings.DrawQuietZones ? 0 : 4;
        var size = numModules * settings.PixelsPerModule;

        var bitmap = new Bitmap(size, size);

        using var graphics = Graphics.FromImage(bitmap);
        using var lightBrush = new SolidBrush(settings.LightColor);
        using var darkBrush = new SolidBrush(settings.DarkColor);
        
        // make background transparent
        using (var brush = new SolidBrush(settings.BackgroundColor))
        {
            graphics.FillRectangle(brush, new Rectangle(0, 0, size, size));
        }
        
        //Render background if set
        if (settings.BackgroundImageStyle != BackgroundImageStyle.None &&
            settings.BackgroundBytes != null)
        {
            using var backgroundBitmap = new Bitmap(new MemoryStream(settings.BackgroundBytes));
            switch (settings.BackgroundImageStyle)
            {
                case BackgroundImageStyle.Fill:
                {
                    using var resized = Resize(backgroundBitmap, size);
                    graphics.DrawImage(
                        resized,
                        0,
                        0);
                    break;
                }
                case BackgroundImageStyle.DataAreaOnly:
                {
                    var bgOffset = 4 - offset;
                    using var resized = Resize(
                        backgroundBitmap,
                        size - 2 * bgOffset * settings.PixelsPerModule);
                    graphics.DrawImage(
                        resized, 
                        bgOffset * settings.PixelsPerModule, 
                        bgOffset * settings.PixelsPerModule);
                    break;
                }
            }
        }
                        

        using var darkModulePixel = MakeDotPixel(settings.PixelsPerModule, pixelSize, darkBrush);
        using var lightModulePixel = MakeDotPixel(settings.PixelsPerModule, pixelSize, lightBrush);

        for (var x = 0; x < numModules; x += 1)
        {
            for (var y = 0; y < numModules; y += 1)
            {
                var rectangleF = new Rectangle(
                    x * settings.PixelsPerModule,
                    y * settings.PixelsPerModule,
                    settings.PixelsPerModule,
                    settings.PixelsPerModule);

                var pixelIsDark = data.ModuleMatrix[offset + y][offset + x];
                var solidBrush = pixelIsDark ? darkBrush : lightBrush;
                var pixelImage = pixelIsDark ? darkModulePixel : lightModulePixel;

                if (!IsPartOfFinderPattern(x, y, numModules, offset))
                    if (settings is { DrawQuietZones: true, QuietZoneStyle: QuietZoneStyle.Flat } &&
                        IsPartOfQuietZone(x, y, numModules))
                        graphics.FillRectangle(solidBrush, rectangleF);
                    else
                        graphics.DrawImage(pixelImage, rectangleF);
                else if (settings.FinderPatternBytes == null)
                    graphics.FillRectangle(solidBrush, rectangleF);
            }
        }
        if (settings.FinderPatternBytes != null)
        {
            using var finderPatternImage = new Bitmap(new MemoryStream(settings.FinderPatternBytes));
            var finderPatternSize = 7 * settings.PixelsPerModule;
            graphics.DrawImage(finderPatternImage, new Rectangle(0, 0, finderPatternSize, finderPatternSize));
            graphics.DrawImage(finderPatternImage, new Rectangle(size - finderPatternSize, 0, finderPatternSize, finderPatternSize));
            graphics.DrawImage(finderPatternImage, new Rectangle(0, size - finderPatternSize, finderPatternSize, finderPatternSize));
        }
        graphics.Save();

        return bitmap;
    }

    /// <summary>
    /// If the pixelSize is bigger than the pixelsPerModule or may end up filling the Module making a traditional QR code.
    /// </summary>
    /// <param name="pixelsPerModule">Pixels used per module rendered</param>
    /// <param name="pixelSize">Size of the dots</param>
    /// <param name="brush">Color of the pixels</param>
    /// <returns></returns>
    private static Bitmap MakeDotPixel(int pixelsPerModule, int pixelSize, SolidBrush brush)
    {            
        // draw a dot
        using var bitmap = new Bitmap(pixelSize, pixelSize);
        using (var graphics = Graphics.FromImage(bitmap))
        {
            graphics.FillEllipse(brush, new Rectangle(0, 0, pixelSize, pixelSize));
            graphics.Save();
        }

        var pixelWidth = Math.Min(pixelsPerModule, pixelSize);
        var margin = Math.Max((pixelsPerModule - pixelWidth) / 2, 0);

        // center the dot in the module and crop to stay the right size.
        var cropped = new Bitmap(pixelsPerModule, pixelsPerModule);
        using (var graphics = Graphics.FromImage(cropped))
        {
            graphics.DrawImage(bitmap, new Rectangle(margin, margin, pixelWidth, pixelWidth),
                new RectangleF(((float)pixelSize - pixelWidth) / 2, ((float)pixelSize - pixelWidth) / 2, pixelWidth, pixelWidth),
                GraphicsUnit.Pixel);
            graphics.Save();
        }

        return cropped;
    }


    /// <summary>
    /// Checks if a given module(-position) is part of the quiet zone of a QR code
    /// </summary>
    /// <param name="x">X position</param>
    /// <param name="y">Y position</param>
    /// <param name="numModules">Total number of modules per row</param>
    /// <returns>true, if position is part of quiet zone</returns>
    private static bool IsPartOfQuietZone(int x, int y, int numModules)
    {
        return
            x < 4 || //left 
            y < 4 || //top
            x > numModules - 5 || //right
            y > numModules - 5; //bottom                
    }


    /// <summary>
    /// Checks if a given module(-position) is part of one of the three finder patterns of a QR code
    /// </summary>
    /// <param name="x">X position</param>
    /// <param name="y">Y position</param>
    /// <param name="numModules">Total number of modules per row</param>
    /// <param name="offset">Offset in modules (usually depending on drawQuietZones parameter)</param>
    /// <returns>true, if position is part of any finder pattern</returns>
    private static bool IsPartOfFinderPattern(int x, int y, int numModules, int offset)
    {
        var cornerSize = 11 - offset;
        var outerLimitLow = (numModules - cornerSize - 1);
        var outerLimitHigh = outerLimitLow + 8;
        var invertedOffset = 4 - offset;
        return
            (x >= invertedOffset && x < cornerSize && y >= invertedOffset && y < cornerSize) || //Top-left finder pattern
            (x > outerLimitLow && x < outerLimitHigh && y >= invertedOffset && y < cornerSize) || //Top-right finder pattern
            (x >= invertedOffset && x < cornerSize && y > outerLimitLow && y < outerLimitHigh); //Bottom-left finder pattern
    }

    /// <summary>
    /// Resize to a square bitmap, but maintain the aspect ratio by padding transparently.
    /// </summary>
    /// <param name="image"></param>
    /// <param name="newSize"></param>
    /// <returns>Resized image as bitmap</returns>
    private static Bitmap Resize(Bitmap image, int newSize)
    {
        float scale = Math.Min((float)newSize / image.Width, (float)newSize / image.Height);
        var scaledWidth = (int)(image.Width * scale);
        var scaledHeight = (int)(image.Height * scale);
        var offsetX = (newSize - scaledWidth) / 2;
        var offsetY = (newSize - scaledHeight) / 2;

        var bm = new Bitmap(newSize, newSize);

        using var graphics = Graphics.FromImage(bm);
        using var brush = new SolidBrush(Color.Transparent);
        graphics.FillRectangle(brush, new Rectangle(0, 0, newSize, newSize));

        graphics.InterpolationMode = InterpolationMode.High;
        graphics.CompositingQuality = CompositingQuality.HighQuality;
        graphics.SmoothingMode = SmoothingMode.AntiAlias;
        
        using var scaledImage = new Bitmap(image, new Size(scaledWidth, scaledHeight));
        graphics.DrawImage(scaledImage, new Rectangle(offsetX, offsetY, scaledWidth, scaledHeight));

        return bm;
    }
    
    
    
    /// <inheritdoc />
    public byte[] RenderToBytes(
        QrCode data,
        RendererSettings? settings = null)
    {
        settings ??= new RendererSettings();
        
        using var image = Render(data: data, settings: settings);
        
        return image.ToBytes(
            fileFormat: settings.FileFormat,
            quality: settings.Quality);
    }

    /// <inheritdoc />
    public Stream RenderToStream(
        QrCode data,
        RendererSettings? settings = null)
    {
        settings ??= new RendererSettings();
        
        using var image = Render(data: data, settings: settings);
        
        return image.ToStream(
            fileFormat: settings.FileFormat,
            quality: settings.Quality);
    }
}