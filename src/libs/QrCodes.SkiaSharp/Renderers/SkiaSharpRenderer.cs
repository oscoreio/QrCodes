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
        
        canvas.Clear(settings.LightColor.ToSkiaSharpColor());
        
        DrawQrCode(
            data: data,
            canvas: canvas,
            settings: settings,
            moduleOffset: moduleOffset);
        
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
                        canvas.DrawRect(
                            rect: centerDest,
                            paint: paint);
                        break;
                    
                    
                    case BackgroundType.RoundRectangle:
                        canvas.DrawRoundRect(
                            rect: centerDest,
                            rx: iconDestWidth * 0.25F,
                            ry: iconDestHeight * 0.25F,
                            paint: paint);
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
        RendererSettings settings,
        int moduleOffset)
    {
        using var darkPaint = new SKPaint();
        darkPaint.Color = settings.DarkColor.ToSkiaSharpColor();
        darkPaint.Style = SKPaintStyle.Fill;
        
        for (var modY = moduleOffset; modY < data.ModuleMatrix.Count - moduleOffset; modY++)
        {
            for (var modX = moduleOffset; modX < data.ModuleMatrix.Count - moduleOffset; modX++)
            {
                if (!data.ModuleMatrix[modY][modX])
                {
                    continue;
                }
                
                switch (settings.DotStyle)
                {
                    case BackgroundType.Circle:
                        canvas.DrawCircle(
                            cx: (modX - moduleOffset + 0.5F) * settings.PixelsPerModule,
                            cy: (modY - moduleOffset + 0.5F) * settings.PixelsPerModule,
                            radius: settings.PixelsPerModule * 0.5F,
                            paint: darkPaint);
                        break;
                    
                    case BackgroundType.Rectangle:
                        canvas.DrawRect(
                            x: (modX - moduleOffset) * settings.PixelsPerModule,
                            y: (modY - moduleOffset) * settings.PixelsPerModule,
                            w: settings.PixelsPerModule,
                            h: settings.PixelsPerModule,
                            paint: darkPaint);
                        break;
                    
                    case BackgroundType.RoundRectangle:
                        canvas.DrawRoundRect(
                            x: (modX - moduleOffset) * settings.PixelsPerModule,
                            y: (modY - moduleOffset) * settings.PixelsPerModule,
                            w: settings.PixelsPerModule,
                            h: settings.PixelsPerModule,
                            paint: darkPaint,
                            rx: settings.PixelsPerModule * 0.25F,
                            ry: settings.PixelsPerModule * 0.25F);
                        break;
                    
                    default:
                        throw new ArgumentOutOfRangeException(nameof(settings), settings.BackgroundType, null);
                }
                
                if (settings is { ConnectDots: true, DotStyle: not BackgroundType.Rectangle })
                {
                    if (modX < data.ModuleMatrix.Count - moduleOffset - 1 &&
                        data.ModuleMatrix[modY][modX] && data.ModuleMatrix[modY][modX + 1])
                    {
                        canvas.DrawRect(
                            x: (modX - moduleOffset + 0.5F) * settings.PixelsPerModule,
                            y: (modY - moduleOffset) * settings.PixelsPerModule,
                            w: settings.PixelsPerModule,
                            h: settings.PixelsPerModule,
                            paint: darkPaint);
                    }
                    if (modY < data.ModuleMatrix.Count - moduleOffset - 1 &&
                        data.ModuleMatrix[modY][modX] && data.ModuleMatrix[modY + 1][modX])
                    {
                        canvas.DrawRect(
                            x: (modX - moduleOffset) * settings.PixelsPerModule,
                            y: (modY - moduleOffset + 0.5F) * settings.PixelsPerModule,
                            w: settings.PixelsPerModule,
                            h: settings.PixelsPerModule,
                            paint: darkPaint);
                    }
                }
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