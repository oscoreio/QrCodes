namespace QrCodes.Payloads;

/// <summary>
/// 
/// </summary>
public class LitecoinAddress : BitcoinLikeCryptoCurrencyAddress
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="address"></param>
    /// <param name="amount"></param>
    /// <param name="label"></param>
    /// <param name="message"></param>
    public LitecoinAddress(
        string address,
        double? amount,
        string? label = null,
        string? message = null)
        : base(
            currencyType: BitcoinLikeCryptoCurrencyType.Litecoin,
            address: address,
            amount: amount,
            label: label,
            message: message) { }
}