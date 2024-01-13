using DependencyPropertyGenerator;

namespace QrCodes.Maui;

/// <summary>
/// 
/// </summary>
[AcceptEmptyServiceProvider]
[ContentProperty(nameof(Value))]
[DependencyProperty<string>("Value")]
[DependencyProperty<ErrorCorrectionLevel>("ErrorCorrectionLevel", DefaultValue = ErrorCorrectionLevel.High)]
[DependencyProperty<Renderer>("Renderer", DefaultValue = Renderer.ImageSharpPng)]
[DependencyProperty<int>("PixelsPerModule", DefaultValue = 10)]
[DependencyProperty<bool>("DrawQuietZones", DefaultValue = true)]
[DependencyProperty<Color>("DarkColor", DefaultValueExpression = "Colors.Black")]
[DependencyProperty<Color>("LightColor", DefaultValueExpression = "Colors.White")]
[DependencyProperty<int>("IconSizePercent", DefaultValue = 15)]
[DependencyProperty<int>("IconBorderWidth", DefaultValue = 6)]
[DependencyProperty<Color>("IconBackgroundColor")]
[DependencyProperty<ImageSource>("LogoSource")]
public partial class QrCodeExtension : BindableObject, IMarkupExtension<BindingBase>
{
	/// <inheritdoc />
	public BindingBase ProvideValue(IServiceProvider serviceProvider)
	{
		var source = new QrCodeSource
		{
			// Pass initial values to reduce re-rendering
			Value = Value,
			ErrorCorrectionLevel = ErrorCorrectionLevel,
			Renderer = Renderer,
			PixelsPerModule = PixelsPerModule,
			DrawQuietZones = DrawQuietZones,
			DarkColor = DarkColor,
			LightColor = LightColor,
			IconSizePercent = IconSizePercent,
			IconBorderWidth = IconBorderWidth,
			IconBackgroundColor = IconBackgroundColor,
			LogoSource = LogoSource,
		};
		source.SetBinding(QrCodeSource.ValueProperty, new Binding(nameof(Value), source: this));
		source.SetBinding(QrCodeSource.ErrorCorrectionLevelProperty, new Binding(nameof(ErrorCorrectionLevel), source: this));
		source.SetBinding(QrCodeSource.RendererProperty, new Binding(nameof(Renderer), source: this));
		source.SetBinding(QrCodeSource.PixelsPerModuleProperty, new Binding(nameof(PixelsPerModule), source: this));
		source.SetBinding(QrCodeSource.DrawQuietZonesProperty, new Binding(nameof(DrawQuietZones), source: this));
		source.SetBinding(QrCodeSource.LogoSourceProperty, new Binding(nameof(LogoSource), source: this));
		source.SetBinding(QrCodeSource.DarkColorProperty, new Binding(nameof(DarkColor), source: this));
		source.SetBinding(QrCodeSource.LightColorProperty, new Binding(nameof(LightColor), source: this));
		source.SetBinding(QrCodeSource.IconSizePercentProperty, new Binding(nameof(IconSizePercent), source: this));
		source.SetBinding(QrCodeSource.IconBorderWidthProperty, new Binding(nameof(IconBorderWidth), source: this));
		source.SetBinding(QrCodeSource.IconBackgroundColorProperty, new Binding(nameof(IconBackgroundColor), source: this));
		
		return new Binding(Binding.SelfPath, source: source);
	}

	/// <inheritdoc />
	object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
	{
		return (this as IMarkupExtension<BindingBase>).ProvideValue(serviceProvider);
	}
}