using System.Drawing;
using QrCodes.Renderers.Abstractions;
using SkiaSharp;

// ReSharper disable SuggestVarOrType_BuiltInTypes

namespace QrCodes.Renderers;

/// <summary>
/// 
/// </summary>
public class SkiaSharpRenderer : IRenderer
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static SKImage Render(
        QrCode data,
        RendererSettings? settings = null)
    {
        data = data ?? throw new ArgumentNullException(nameof(data));
        settings ??= new RendererSettings();
        
        int moduleOffset = settings.DrawQuietZones ? 0 : 4;
        int size = (data.ModuleMatrix.Count - moduleOffset * 2) * settings.PixelsPerModule;

        using var bitmap = new SKBitmap(new SKImageInfo(width: size, height: size));
        using var canvas = new SKCanvas(bitmap);
        DrawQrCode(
            data: data,
            canvas: canvas,
            pixelsPerModule: settings.PixelsPerModule,
            moduleOffset: moduleOffset,
            darkColor: settings.DarkColor.ToSkiaSharpColor(),
            lightColor: settings.LightColor.ToSkiaSharpColor());
        
        if (settings is { IconBytes: not null, IconSizePercent: > 0 and <= 100 })
        {
            using var iconBitmap = SKBitmap.Decode(settings.IconBytes);
            float iconDestWidth = settings.IconSizePercent * bitmap.Width / 100f;
            float iconDestHeight = iconDestWidth * iconBitmap.Height / iconBitmap.Width;
            var iconDestRect = new SKRect(
                left: (bitmap.Width - iconDestWidth) / 2,
                top: (bitmap.Height - iconDestHeight) / 2,
                right: (bitmap.Width + iconDestWidth) / 2,
                bottom: (bitmap.Height + iconDestHeight) / 2);
            
            if (settings.IconBackgroundColor != Color.Transparent)
            {
                var centerDest = iconDestRect;
                centerDest.Inflate(settings.IconBorderWidth, settings.IconBorderWidth);
                
                using var paint = new SKPaint();
                paint.Color = settings.IconBackgroundColor.ToSkiaSharpColor();
                paint.Style = SKPaintStyle.Fill;
                paint.IsAntialias = true;

                switch (settings.BackgroundType)
                {
                    case BackgroundType.Circle:
                        canvas.DrawCircle(
                            cx: bitmap.Width / 2.0f,
                            cy: bitmap.Height / 2.0f,
                            radius: iconDestWidth / 2.0f + settings.IconBorderWidth,
                            paint: paint);
                        break;
                    case BackgroundType.Rectangle:
                        canvas.DrawRect(centerDest, paint);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(settings), settings.BackgroundType, null);
                }
            }

            using var resizedIconBitmap = iconBitmap.Resize(
                info: new SKImageInfo(
                    width: (int)iconDestWidth,
                    height: (int)iconDestHeight),
                quality: SKFilterQuality.High);
            
            canvas.DrawBitmap(
                resizedIconBitmap,
                new SKPoint((int)iconDestRect.Left, (int)iconDestRect.Top));
        }
        
        return SKImage.FromBitmap(bitmap);
    }

    private static void DrawQrCode(
        QrCode data,
        SKCanvas canvas,
        int pixelsPerModule,
        int moduleOffset,
        SKColor darkColor,
        SKColor lightColor)
    {
        using var lightPaint = new SKPaint();
        lightPaint.Color = lightColor;
        lightPaint.Style = SKPaintStyle.Fill;
        using var darkPaint = new SKPaint();
        darkPaint.Color = darkColor;
        darkPaint.Style = SKPaintStyle.Fill;
        
        for (var modY = moduleOffset; modY < data.ModuleMatrix.Count - moduleOffset; modY++)
        {
            for (var modX = moduleOffset; modX < data.ModuleMatrix.Count - moduleOffset; modX++)
            {
                canvas.DrawRect(
                    x: (modX - moduleOffset) * pixelsPerModule,
                    y: (modY - moduleOffset) * pixelsPerModule,
                    w: pixelsPerModule,
                    h: pixelsPerModule,
                    paint: data.ModuleMatrix[modY][modX]
                        ? darkPaint
                        : lightPaint);
            }
        }
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