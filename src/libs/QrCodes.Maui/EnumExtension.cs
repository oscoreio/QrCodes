using System.Collections;

namespace QrCodes.Maui;

/// <summary>
/// 
/// </summary>
[AcceptEmptyServiceProvider]
[ContentProperty(nameof(Type))]
public class EnumExtension : IMarkupExtension<IList>
{
    /// <summary>
    /// 
    /// </summary>
    public Type? Type { get; set; }
    
    /// <inheritdoc />
    public IList ProvideValue(IServiceProvider serviceProvider)
    {
        if (Type is null)
        {
            return Array.Empty<object>();
        }
        
        return Enum
            .GetValues(Type)
            .Cast<object>()
            .ToArray();
    }

    /// <inheritdoc />
    object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
    {
        return (this as IMarkupExtension<IList>).ProvideValue(serviceProvider);
    }
}