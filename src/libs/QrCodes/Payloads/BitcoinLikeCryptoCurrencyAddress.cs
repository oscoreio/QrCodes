using System.Globalization;

namespace QrCodes.Payloads;

/// <summary>
/// 
/// </summary>
public class BitcoinLikeCryptoCurrencyAddress
{
    private readonly BitcoinLikeCryptoCurrencyType _currencyType;
    private readonly string _address, _label = string.Empty, _message = string.Empty;
    private readonly double? _amount;

    /// <summary>
    /// Generates a Bitcoin like cryptocurrency payment payload. QR Codes with this payload can open a payment app.
    /// </summary>
    /// <param name="currencyType">Bitcoin like cryptocurrency address of the payment receiver</param>
    /// <param name="address">Bitcoin like cryptocurrency address of the payment receiver</param>
    /// <param name="amount">Amount of coins to transfer</param>
    /// <param name="label">Reference label</param>
    /// <param name="message">Reference text aka message</param>
    // ReSharper disable once MemberCanBeProtected.Global
    public BitcoinLikeCryptoCurrencyAddress(
        BitcoinLikeCryptoCurrencyType currencyType,
        string address,
        double? amount,
        string? label = null,
        string? message = null)
    {
        _currencyType = currencyType;
        _address = address;

        if (!string.IsNullOrEmpty(label))
        {
            _label = Uri.EscapeDataString(label);
        }

        if (!string.IsNullOrEmpty(message))
        {
            _message = Uri.EscapeDataString(message);
        }

        _amount = amount;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        string? query = null;

        var queryValues = new[]{
            new KeyValuePair<string, string?>("label", _label),
            new KeyValuePair<string, string?>("message", _message),
            new KeyValuePair<string, string?>("amount", _amount?.ToString("#.########", CultureInfo.InvariantCulture))
        };

        if (queryValues.Any(keyPair => !string.IsNullOrEmpty(keyPair.Value)))
        {
            query = "?" + string.Join("&", queryValues
                .Where(keyPair => !string.IsNullOrEmpty(keyPair.Value))
                .Select(keyPair => $"{keyPair.Key}={keyPair.Value}")
                .ToArray());
        }

        return $"{Enum.GetName(typeof(BitcoinLikeCryptoCurrencyType), _currencyType)?.ToLower(CultureInfo.InvariantCulture)}:{_address}{query}";
    }

    /// <summary>
    /// 
    /// </summary>
    public enum BitcoinLikeCryptoCurrencyType
    {
        /// <summary>
        /// 
        /// </summary>
        Bitcoin,
        
        /// <summary>
        /// 
        /// </summary>
        BitcoinCash,
        
        /// <summary>
        /// 
        /// </summary>
        Litecoin
    }
}