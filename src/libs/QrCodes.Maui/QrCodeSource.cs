using System.Diagnostics;
using DependencyPropertyGenerator;
using QrCodes.Renderers;
using QrCodes.Renderers.Abstractions;

namespace QrCodes.Maui;

/// <summary>
/// Refs: <br/>
/// https://github.com/CommunityToolkit/Maui/blob/main/src/CommunityToolkit.Maui/ImageSources/GravatarImageSource.shared.cs
/// </summary>
[DependencyProperty<string>("Value", OnChanged = nameof(OnChanged))]
[DependencyProperty<ErrorCorrectionLevel>("ErrorCorrectionLevel", DefaultValue = ErrorCorrectionLevel.High, OnChanged = nameof(OnChanged))]
[DependencyProperty<Renderer>("Renderer", DefaultValue = Renderer.SkiaSharp, OnChanged = nameof(OnChanged))]
[DependencyProperty<FileFormat>("FileFormat", DefaultValue = FileFormat.Png, OnChanged = nameof(OnChanged))]
[DependencyProperty<int>("Quality", DefaultValue = 100, OnChanged = nameof(OnChanged))]
[DependencyProperty<int>("PixelsPerModule", DefaultValue = 5, OnChanged = nameof(OnChanged))]
[DependencyProperty<bool>("DrawQuietZones", DefaultValue = true, OnChanged = nameof(OnChanged))]
[DependencyProperty<Color>("DarkColor", DefaultValueExpression = "Colors.Black", OnChanged = nameof(OnChanged))]
[DependencyProperty<Color>("LightColor", DefaultValueExpression = "Colors.White", OnChanged = nameof(OnChanged))]
[DependencyProperty<int>("IconSizePercent", DefaultValue = 15, OnChanged = nameof(OnChanged))]
[DependencyProperty<int>("IconBorderWidth", DefaultValue = 0, OnChanged = nameof(OnChanged))]
[DependencyProperty<Color>("IconBackgroundColor", DefaultValueExpression = "Colors.Transparent", OnChanged = nameof(OnChanged))]
[DependencyProperty<BackgroundType>("BackgroundType", DefaultValue = BackgroundType.Circle, OnChanged = nameof(OnChanged))]
[DependencyProperty<BackgroundType>("DotStyle", DefaultValue = BackgroundType.Rectangle, OnChanged = nameof(OnChanged))]
[DependencyProperty<bool>("ConnectDots", DefaultValue = true, OnChanged = nameof(OnChanged))]
[DependencyProperty<ImageSource>("LogoSource")]
public partial class QrCodeSource : StreamImageSource
{
    private byte[]? _logoBytes;
    
    private void OnChanged()
    {
        OnSourceChanged();
    }
    
    async partial void OnLogoSourceChanged(ImageSource? newValue)
    {
        try
        {
            _logoBytes = newValue is null
                ? null
                : await newValue.ToBytesAsync();
            
            OnSourceChanged();
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
    }

    private Task<Stream> RenderAsync(CancellationToken cancellationToken = default)
    {
        var renderer = Renderer switch
        {
            Renderer.SkiaSharp => (IRenderer)new SkiaSharpRenderer(),
            Renderer.FastPng => new FastPngRenderer(),
            _ => throw new NotImplementedException()
        };
        var stream = renderer.RenderToStream(
            data: QrCodeGenerator.Generate(
                Value ?? string.Empty,
                ErrorCorrectionLevel),
            new RendererSettings
            {
                PixelsPerModule = PixelsPerModule,
                DrawQuietZones = DrawQuietZones,
                DarkColor = (DarkColor ?? Colors.Black).ToSystemDrawingColor(),
                LightColor = (LightColor ?? Colors.White).ToSystemDrawingColor(),
                IconBytes = _logoBytes,
                IconSizePercent = IconSizePercent,
                IconBorderWidth = IconBorderWidth,
                BackgroundType = BackgroundType,
                IconBackgroundColor = (IconBackgroundColor ?? Colors.Transparent).ToSystemDrawingColor(),
                FileFormat = FileFormat,
                Quality = Quality,
                DotStyle = DotStyle,
                ConnectDots = ConnectDots,
            });

        return Task.FromResult(stream);
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