namespace QrCodes.Payloads;

/// <inheritdoc />
public class BitcoinCashAddress(
    string address,
    double? amount = null,
    string? label = null,
    string? message = null)
    : BitcoinLikeCryptoCurrencyAddress(
        currencyType: BitcoinLikeCryptoCurrencyType.BitcoinCash,
        address: address,
        amount: amount,
        label: label,
        message: message);