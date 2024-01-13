using System.Diagnostics;
using DependencyPropertyGenerator;
using QrCodes.Renderers;

namespace QrCodes.Maui;

/// <summary>
/// 
/// </summary>
[DependencyProperty<string>("Value", OnChanged = nameof(OnChanged))]
[DependencyProperty<ErrorCorrectionLevel>("ErrorCorrectionLevel", DefaultValue = ErrorCorrectionLevel.High, OnChanged = nameof(OnChanged))]
[DependencyProperty<Renderer>("Renderer", DefaultValue = Renderer.ImageSharpPng, OnChanged = nameof(OnChanged))]
[DependencyProperty<int>("PixelsPerModule", DefaultValue = 10, OnChanged = nameof(OnChanged))]
[DependencyProperty<bool>("DrawQuietZones", DefaultValue = true, OnChanged = nameof(OnChanged))]
[DependencyProperty<Color>("DarkColor", DefaultValueExpression = "Colors.Black", OnChanged = nameof(OnChanged))]
[DependencyProperty<Color>("LightColor", DefaultValueExpression = "Colors.White", OnChanged = nameof(OnChanged))]
[DependencyProperty<int>("IconSizePercent", DefaultValue = 15, OnChanged = nameof(OnChanged))]
[DependencyProperty<int>("IconBorderWidth", DefaultValue = 6, OnChanged = nameof(OnChanged))]
[DependencyProperty<Color>("IconBackgroundColor", OnChanged = nameof(OnChanged))]
[DependencyProperty<ImageSource>("LogoSource")]
public partial class QrCodeSource : StreamImageSource
{
    private SixLabors.ImageSharp.Image? _logoImage;
    
    private void OnChanged()
    {
        OnSourceChanged();
    }
    
    async partial void OnLogoSourceChanged(ImageSource? newValue)
    {
        try
        {
            await using var stream = newValue is null
                ? null
                : await newValue.ToStreamAsync();
            _logoImage?.Dispose();
            _logoImage = stream is null
                ? null
                : await SixLabors.ImageSharp.Image.LoadAsync(stream);
            
            OnSourceChanged();
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
    }

    private async Task<Stream> RenderAsync(CancellationToken cancellationToken = default)
    {
        var qrCode = QrCodeGenerator.Generate(
            Value ?? string.Empty,
            ErrorCorrectionLevel);
        
        switch (Renderer)
        {
            case Renderer.ImageSharpGif:
            case Renderer.ImageSharpBmp:
            case Renderer.ImageSharpJpeg:
            case Renderer.ImageSharpPbm:
            case Renderer.ImageSharpPng:
            case Renderer.ImageSharpTga:
            case Renderer.ImageSharpTiff:
            case Renderer.ImageSharpWebp:
            {
                using var image = ImageSharpRenderer.Render(
                    qrCode,
                    pixelsPerModule: PixelsPerModule,
                    drawQuietZones: DrawQuietZones,
                    darkColor: DarkColor?.ToImageSharpColor(),
                    lightColor: LightColor?.ToImageSharpColor(),
                    icon: _logoImage,
                    iconSizePercent: IconSizePercent,
                    iconBorderWidth: IconBorderWidth,
                    iconBackgroundColor: IconBackgroundColor?.ToImageSharpColor());

                var bytes = await image.ToBytesAsync(
                    fileFormat: Renderer switch
                    {
                        Renderer.ImageSharpGif => FileFormat.Gif,
                        Renderer.ImageSharpBmp => FileFormat.Bmp,
                        Renderer.ImageSharpJpeg => FileFormat.Jpeg,
                        Renderer.ImageSharpPbm => FileFormat.Pbm,
                        Renderer.ImageSharpPng => FileFormat.Png,
                        Renderer.ImageSharpTga => FileFormat.Tga,
                        Renderer.ImageSharpTiff => FileFormat.Tiff,
                        Renderer.ImageSharpWebp => FileFormat.Webp,
                        _ => FileFormat.Png,
                    },
                    cancellationToken: cancellationToken).ConfigureAwait(false);

                return new MemoryStream(bytes);
            }

            case Renderer.Png:
            {
                var bytes = PngRenderer.Render(
                    qrCode,
                    pixelsPerModule: PixelsPerModule,
                    drawQuietZones: DrawQuietZones);

                return new MemoryStream(bytes);
            }
            
            default:
            {
                throw new NotImplementedException();
            }
        }
    }
    
    /// <summary>
    /// 
    /// </summary>
    public QrCodeSource()
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        Stream = RenderAsync;
    }
}