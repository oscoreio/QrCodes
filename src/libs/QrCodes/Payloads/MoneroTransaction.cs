namespace QrCodes.Payloads;

/// <summary>
/// 
/// </summary>
public class MoneroTransaction
{
    private readonly string _address, _txPaymentId, _recipientName, _txDescription;
    private readonly float? _txAmount;

    /// <summary>
    /// Creates a monero transaction payload
    /// </summary>
    /// <param name="address">Receiver's monero address</param>
    /// <param name="txAmount">Amount to transfer</param>
    /// <param name="txPaymentId">Payment id</param>
    /// <param name="recipientName">Receipient's name</param>
    /// <param name="txDescription">Reference text / payment description</param>
    /// <exception cref="InvalidOperationException"></exception>
    public MoneroTransaction(
        string address,
        float? txAmount = null,
        string? txPaymentId = null,
        string? recipientName = null,
        string? txDescription = null)
    {
        if (string.IsNullOrEmpty(address))
        {
            throw new InvalidOperationException("The address is mandatory and has to be set.");
        }
        if (txAmount is <= 0)
        {
            throw new InvalidOperationException("Value of 'txAmount' must be greater than 0.");
        }
        
        _address = address;
        _txAmount = txAmount;
        _txPaymentId = txPaymentId ?? string.Empty;
        _recipientName = recipientName ?? string.Empty;
        _txDescription = txDescription ?? string.Empty;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        var moneroUri = $"monero://{_address}{(
            !string.IsNullOrEmpty(_txPaymentId) ||
            !string.IsNullOrEmpty(_recipientName) ||
            !string.IsNullOrEmpty(_txDescription) ||
            _txAmount != null
                ? "?"
                : string.Empty)}";
        moneroUri += !string.IsNullOrEmpty(_txPaymentId)
            ? $"tx_payment_id={Uri.EscapeDataString(_txPaymentId)}&"
            : string.Empty;
        moneroUri += !string.IsNullOrEmpty(_recipientName)
            ? $"recipient_name={Uri.EscapeDataString(_recipientName)}&"
            : string.Empty;
        moneroUri += _txAmount != null
            ? $"tx_amount={_txAmount.ToString()?.Replace(",",".")}&"
            : string.Empty;
        moneroUri += !string.IsNullOrEmpty(_txDescription)
            ? $"tx_description={Uri.EscapeDataString(_txDescription)}"
            : string.Empty;
        
        return moneroUri.TrimEnd('&');
    }
}