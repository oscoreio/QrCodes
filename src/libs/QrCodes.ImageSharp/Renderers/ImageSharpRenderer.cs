using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

// ReSharper disable SuggestVarOrType_BuiltInTypes

namespace QrCodes.Renderers;

/// <summary>
/// 
/// </summary>
public static class ImageSharpRenderer
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="pixelsPerModule"></param>
    /// <param name="darkColor"></param>
    /// <param name="lightColor"></param>
    /// <param name="icon"></param>
    /// <param name="iconSizePercent"></param>
    /// <param name="iconBorderWidth"></param>
    /// <param name="drawQuietZones"></param>
    /// <param name="iconBackgroundColor"></param>
    /// <returns></returns>
    public static Image<Rgba32> Render(
        QrCode data,
        int pixelsPerModule = 5,
        Color? darkColor = null,
        Color? lightColor = null,
        bool drawQuietZones = true,
        Image? icon = null,
        int iconSizePercent = 15,
        int iconBorderWidth = 0,
        Color? iconBackgroundColor = null)
    {
        data = data ?? throw new ArgumentNullException(nameof(data));
        darkColor ??= Color.Black;
        lightColor ??= Color.White;
        
        int moduleOffset = drawQuietZones ? 0 : 4;
        int size = (data.ModuleMatrix.Count - moduleOffset * 2) * pixelsPerModule;

        var image = new Image<Rgba32>(size, size);
        DrawQrCode(data, image, pixelsPerModule, moduleOffset, darkColor.Value, lightColor.Value);
        
        if (icon != null && iconSizePercent is > 0 and <= 100)
        {
            float iconDestWidth = iconSizePercent * image.Width / 100f;
            float iconDestHeight = iconDestWidth * icon.Height / icon.Width;
            var iconDestRect = new RectangleF(
                x: (image.Width - iconDestWidth) / 2,
                y: (image.Height - iconDestHeight) / 2,
                width: iconDestWidth,
                height: iconDestHeight);
            var centerDest = iconDestRect;
            centerDest.Inflate(iconBorderWidth, iconBorderWidth);

            if (iconBorderWidth > 0)
            {
                iconBackgroundColor ??= Color.White;
                if (iconBackgroundColor != Color.Transparent)
                {
                    image.ProcessPixelRows(accessor =>
                    {
                        for (int y = (int)centerDest.Top; y <= (int)centerDest.Bottom; y++)
                        {
                            Span<Rgba32> pixelRow = accessor.GetRowSpan(y);

                            for (int x = (int)centerDest.Left; x <= (int)centerDest.Right; x++)
                            {
                                pixelRow[x] = iconBackgroundColor ?? lightColor ?? Color.White;
                            }
                        }
                    });
                }
            }

            image.Mutate(context =>
            {
                using var sizedIcon = icon.Clone(x =>
                {
                    x.Resize((int)iconDestWidth, (int)iconDestHeight);
                });
                
                context.DrawImage(
                    sizedIcon,
                    new Point((int)iconDestRect.X, (int)iconDestRect.Y),
                    opacity: 1);
            });
        }
        /*


        var size = (this.QrCodeData.ModuleMatrix.Count - (drawQuietZones ? 0 : 8)) * pixelsPerModule;
        var offset = drawQuietZones ? 0 : 4 * pixelsPerModule;

        var image = new Image<Rgba32>(size, size);

        using (var gfx = Graphics.FromImage(bmp))
        using (var lightBrush = new SolidBrush(lightColor))
        using (var darkBrush = new SolidBrush(darkColor))
        {
            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
            gfx.CompositingQuality = CompositingQuality.HighQuality;
            gfx.Clear(lightColor);
            var drawIconFlag = icon != null && iconSizePercent > 0 && iconSizePercent <= 100;

            for (var x = 0; x < size + offset; x = x + pixelsPerModule)
            {
                for (var y = 0; y < size + offset; y = y + pixelsPerModule)
                {
                    var moduleBrush = this.QrCodeData.ModuleMatrix[(y + pixelsPerModule) / pixelsPerModule - 1][(x + pixelsPerModule) / pixelsPerModule - 1] ? darkBrush : lightBrush;
                    gfx.FillRectangle(moduleBrush , new Rectangle(x - offset, y - offset, pixelsPerModule, pixelsPerModule));
                }
            }

            if (drawIconFlag)
            {
                float iconDestWidth = iconSizePercent * bmp.Width / 100f;
                float iconDestHeight = drawIconFlag ? iconDestWidth * icon.Height / icon.Width : 0;
                float iconX = (bmp.Width - iconDestWidth) / 2;
                float iconY = (bmp.Height - iconDestHeight) / 2;
                var centerDest = new RectangleF(iconX - iconBorderWidth, iconY - iconBorderWidth, iconDestWidth + iconBorderWidth * 2, iconDestHeight + iconBorderWidth * 2);
                var iconDestRect = new RectangleF(iconX, iconY, iconDestWidth, iconDestHeight);
                var iconBgBrush = iconBackgroundColor != null ? new SolidBrush((Color)iconBackgroundColor) : lightBrush;
                //Only render icon/logo background, if iconBorderWith is set > 0
                if (iconBorderWidth > 0)
                {
                    using (GraphicsPath iconPath = CreateRoundedRectanglePath(centerDest, iconBorderWidth * 2))
                    {
                        gfx.FillPath(iconBgBrush, iconPath);
                    }
                }
                gfx.DrawImage(icon, iconDestRect, new RectangleF(0, 0, icon.Width, icon.Height), GraphicsUnit.Pixel);
            }

            gfx.Save();
        }
        
        */
        
        return image;
    }

    private static void DrawQrCode(
        QrCode data,
        Image<Rgba32> image,
        int pixelsPerModule,
        int moduleOffset,
        Color darkColor,
        Color lightColor)
    {
        var row = new Rgba32[image.Width];

        image.ProcessPixelRows(accessor =>
        {
            for (var modY = moduleOffset; modY < data.ModuleMatrix.Count - moduleOffset; modY++)
            {
                // Generate row for this y-Module
                for (var modX = moduleOffset; modX < data.ModuleMatrix.Count - moduleOffset; modX++)
                {
                    for (var idx = 0; idx < pixelsPerModule; idx++)
                    {
                        row[(modX - moduleOffset) * pixelsPerModule + idx] = data.ModuleMatrix[modY][modX]
                            ? darkColor
                            : lightColor;
                    }
                }

                // Copy the prepared row to the image
                for (var idx = 0; idx < pixelsPerModule; idx++)
                {
                    Span<Rgba32> pixelRow = accessor.GetRowSpan((modY - moduleOffset) * pixelsPerModule + idx);
                    row.CopyTo(pixelRow);
                }
            }
        });
    }
    /*
    internal GraphicsPath CreateRoundedRectanglePath(RectangleF rect, int cornerRadius)
    {
        var roundedRect = new GraphicsPath();
        roundedRect.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
        roundedRect.AddLine(rect.X + cornerRadius, rect.Y, rect.Right - cornerRadius * 2, rect.Y);
        roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
        roundedRect.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, rect.Y + rect.Height - cornerRadius * 2);
        roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
        roundedRect.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom);
        roundedRect.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
        roundedRect.AddLine(rect.X, rect.Bottom - cornerRadius * 2, rect.X, rect.Y + cornerRadius * 2);
        roundedRect.CloseFigure();
        return roundedRect;
    }
    */
}