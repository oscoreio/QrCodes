namespace QrCodes.Payloads;

/// <summary>
/// 
/// </summary>
public class BitcoinAddress : BitcoinLikeCryptoCurrencyAddress
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="address"></param>
    /// <param name="amount"></param>
    /// <param name="label"></param>
    /// <param name="message"></param>
    public BitcoinAddress(
        string address,
        double? amount,
        string? label = null,
        string? message = null)
        : base(
            BitcoinLikeCryptoCurrencyType.Bitcoin,
            address,
            amount,
            label,
            message) { }
}