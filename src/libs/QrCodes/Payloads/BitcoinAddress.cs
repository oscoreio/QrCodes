namespace QrCodes.Payloads;

/// <inheritdoc />
public class BitcoinAddress(
    string address,
    double? amount = null,
    string? label = null,
    string? message = null)
    : BitcoinLikeCryptoCurrencyAddress(
        currencyType: BitcoinLikeCryptoCurrencyType.Bitcoin,
        address: address,
        amount: amount,
        label: label,
        message: message);