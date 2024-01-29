namespace QrCodes.Payloads;

/// <inheritdoc />
public class LitecoinAddress(
    string address,
    double? amount = null,
    string? label = null,
    string? message = null)
    : BitcoinLikeCryptoCurrencyAddress(
        currencyType: BitcoinLikeCryptoCurrencyType.Litecoin,
        address: address,
        amount: amount,
        label: label,
        message: message);