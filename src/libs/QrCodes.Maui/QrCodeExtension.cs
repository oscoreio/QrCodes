namespace QrCodes.Maui;

/// <summary>
/// 
/// </summary>
[AcceptEmptyServiceProvider]
[ContentProperty(nameof(Value))]
public class QrCodeExtension : QrCodeSource, IMarkupExtension<StreamImageSource>
{
    /// <inheritdoc />
    public StreamImageSource ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }

    /// <inheritdoc />
    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return (this as IMarkupExtension<StreamImageSource>).ProvideValue(serviceProvider);
    }
}