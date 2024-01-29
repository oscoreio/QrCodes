using System.Globalization;

namespace QrCodes.Payloads;

/// <summary>
/// Generates a Ethereum like cryptocurrency payment payload. <br/>
/// A standard way of representing various transactions, especially payment requests in ether and ERC-20 tokens as URLs. <br/>
/// QR Codes with this payload can open a payment app. <br/>
/// According: https://github.com/ethereum/ercs/blob/master/ERCS/erc-681.md <br/>
/// Example: "ethereum:0xfb6916095ca1df60bb79Ce92ce3ea74c37c5d359?value=2.014e18" <br/>
/// </summary>
/// <param name="address">Bitcoin like cryptocurrency address of the payment receiver</param>
/// <param name="value">Amount of coins to transfer</param>
public class EthereumAddress(
    string address,
    double? value = null)
{
    /// <inheritdoc />
    public override string ToString()
    {
        var query = string.Empty;

        var queryValues = new[]{
            new KeyValuePair<string, string?>("value", value?.ToString("#.000e0", CultureInfo.InvariantCulture))
        };

        if (queryValues.Any(keyPair => !string.IsNullOrEmpty(keyPair.Value)))
        {
            query = "?" + string.Join("&", queryValues
                .Where(keyPair => !string.IsNullOrEmpty(keyPair.Value))
                .Select(keyPair => $"{keyPair.Key}={keyPair.Value}")
                .ToArray());
        }

        return $"ethereum:{address}{query}";
    }
}