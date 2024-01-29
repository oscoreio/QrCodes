using System.Globalization;

namespace QrCodes.Payloads;

/// <summary>
/// Generates a Bitcoin like cryptocurrency payment payload. <br/>
/// QR Codes with this payload can open a payment app. <br/>
/// According: https://en.bitcoin.it/wiki/BIP_0021 <br/>
/// Example: "bitcoin:175tWpb8K1S7NmH4Zx6rewF9WQrcZv245W?amount=0.123" <br/>
/// </summary>
/// <param name="currencyType">Bitcoin like cryptocurrency address of the payment receiver</param>
/// <param name="address">Bitcoin like cryptocurrency address of the payment receiver</param>
/// <param name="amount">Amount of coins to transfer</param>
/// <param name="label">Reference label</param>
/// <param name="message">Reference text aka message</param>
public class BitcoinLikeCryptoCurrencyAddress(
    BitcoinLikeCryptoCurrencyAddress.BitcoinLikeCryptoCurrencyType currencyType,
    string address,
    double? amount = null,
    string? label = null,
    string? message = null)
{
    /// <inheritdoc />
    public override string ToString()
    {
        var query = string.Empty;

        var queryValues = new[]{
            new KeyValuePair<string, string?>("label", Uri.EscapeDataString(label ?? string.Empty)),
            new KeyValuePair<string, string?>("message", Uri.EscapeDataString(message ?? string.Empty)),
            new KeyValuePair<string, string?>("amount", amount?.ToString("#.########", CultureInfo.InvariantCulture))
        };

        if (queryValues.Any(keyPair => !string.IsNullOrEmpty(keyPair.Value)))
        {
            query = "?" + string.Join("&", queryValues
                .Where(keyPair => !string.IsNullOrEmpty(keyPair.Value))
                .Select(keyPair => $"{keyPair.Key}={keyPair.Value}")
                .ToArray());
        }

        return $"{Enum.GetName(typeof(BitcoinLikeCryptoCurrencyType), currencyType)?.ToLower(CultureInfo.InvariantCulture)}:{address}{query}";
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
        Litecoin,
    }
}