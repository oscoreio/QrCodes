namespace QrCodes.Payloads;

/// <summary>
/// 
/// </summary>
public class BitcoinCashAddress : BitcoinLikeCryptoCurrencyAddress
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="address"></param>
    /// <param name="amount"></param>
    /// <param name="label"></param>
    /// <param name="message"></param>
    public BitcoinCashAddress(
        string address,
        double? amount,
        string? label = null,
        string? message = null)
        : base(
            BitcoinLikeCryptoCurrencyType.BitcoinCash,
            address,
            amount,
            label,
            message) { }
}