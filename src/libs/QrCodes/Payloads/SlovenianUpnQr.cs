using System.Globalization;
using System.Text;

namespace QrCodes.Payloads;

/// <summary>
/// Keep in mind, that the ECC level has to be set to "M",
/// version to 15 and ECI to ExtendedChannelInterpolationMode.Iso8859_2
/// when generating a SlovenianUpnQr! <br/>
/// 
/// SlovenianUpnQr specification: <br/>
/// https://www.upn-qr.si/uploads/files/NavodilaZaProgramerjeUPNQR.pdf <br/>
/// </summary>
public class SlovenianUpnQr
{
    private readonly string _payerName;
    private readonly string _payerAddress;
    private readonly string _payerPlace;
    private readonly string _amount;
    private readonly string _code;
    private readonly string _purpose;
    private readonly string _deadLine;
    private readonly string _recipientIban;
    private readonly string _recipientName;
    private readonly string _recipientAddress;
    private readonly string _recipientPlace;
    private readonly string _recipientSiModel;
    private readonly string _recipientSiReference;

    private static string LimitLength(string value, int maxLength)
    {
        return (value.Length <= maxLength) ? value : value.Substring(0, maxLength);
    }

    /// <inheritdoc />
    public SlovenianUpnQr(string payerName, string payerAddress, string payerPlace, string recipientName, string recipientAddress, string recipientPlace, string recipientIban, string description, double amount, string recipientSiModel = "SI00", string recipientSiReference = "", string code = "OTHR") :
        this(payerName, payerAddress, payerPlace, recipientName, recipientAddress, recipientPlace, recipientIban, description, amount, null, recipientSiModel, recipientSiReference, code)
    { }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="payerName"></param>
    /// <param name="payerAddress"></param>
    /// <param name="payerPlace"></param>
    /// <param name="recipientName"></param>
    /// <param name="recipientAddress"></param>
    /// <param name="recipientPlace"></param>
    /// <param name="recipientIban"></param>
    /// <param name="description"></param>
    /// <param name="amount"></param>
    /// <param name="deadline"></param>
    /// <param name="recipientSiModel"></param>
    /// <param name="recipientSiReference"></param>
    /// <param name="code"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public SlovenianUpnQr(
        string payerName,
        string payerAddress,
        string payerPlace,
        string recipientName,
        string recipientAddress,
        string recipientPlace,
        string recipientIban,
        string description,
        double amount,
        DateTime? deadline,
        string recipientSiModel = "SI99",
        string recipientSiReference = "",
        string code = "OTHR")
    {
        payerName = payerName ?? throw new ArgumentNullException(nameof(payerName));
        payerAddress = payerAddress ?? throw new ArgumentNullException(nameof(payerAddress));
        payerPlace = payerPlace ?? throw new ArgumentNullException(nameof(payerPlace));
        recipientName = recipientName ?? throw new ArgumentNullException(nameof(recipientName));
        recipientAddress = recipientAddress ?? throw new ArgumentNullException(nameof(recipientAddress));
        recipientPlace = recipientPlace ?? throw new ArgumentNullException(nameof(recipientPlace));
        recipientIban = recipientIban ?? throw new ArgumentNullException(nameof(recipientIban));
        description = description ?? throw new ArgumentNullException(nameof(description));
        recipientSiModel = recipientSiModel ?? throw new ArgumentNullException(nameof(recipientSiModel));
        code = code ?? throw new ArgumentNullException(nameof(code));
        recipientSiReference = recipientSiReference ?? throw new ArgumentNullException(nameof(recipientSiReference));
        
        _payerName = LimitLength(payerName.Trim(), 33);
        _payerAddress = LimitLength(payerAddress.Trim(), 33);
        _payerPlace = LimitLength(payerPlace.Trim(), 33);
        _amount = FormatAmount(amount);
        _code = LimitLength(code.Trim().ToUpper(CultureInfo.InvariantCulture), 4);
        _purpose = LimitLength(description.Trim(), 42);
        _deadLine = deadline == null
            ? string.Empty
            : deadline.Value.ToString("dd.MM.yyyy");
        _recipientIban = LimitLength(recipientIban.Trim(), 34);
        _recipientName = LimitLength(recipientName.Trim(), 33);
        _recipientAddress = LimitLength(recipientAddress.Trim(), 33);
        _recipientPlace = LimitLength(recipientPlace.Trim(), 33);
        _recipientSiModel = LimitLength(recipientSiModel.Trim().ToUpper(CultureInfo.InvariantCulture), 4);
        _recipientSiReference = LimitLength(recipientSiReference.Trim(), 22);
    }
                       
    private static string FormatAmount(double amount)
    {
        return $"{(int)Math.Round(amount * 100.0):00000000000}";
    }

    private int CalculateChecksum()
    {
        var checkSum = 5 + _payerName.Length; //5 = UPNQR constant Length
        checkSum += _payerAddress.Length;
        checkSum += _payerPlace.Length;
        checkSum += _amount.Length;
        checkSum += _code.Length;
        checkSum += _purpose.Length;
        checkSum += _deadLine.Length;
        checkSum += _recipientIban.Length;
        checkSum += _recipientName.Length;
        checkSum += _recipientAddress.Length;
        checkSum += _recipientPlace.Length;
        checkSum += _recipientSiModel.Length;
        checkSum += _recipientSiReference.Length;
        checkSum += 19;
        
        return checkSum;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        var builder = new StringBuilder(capacity: 1024);
        builder.Append("UPNQR");
        builder.Append('\n').Append('\n').Append('\n').Append('\n').Append('\n');
        builder.Append(_payerName).Append('\n');
        builder.Append(_payerAddress).Append('\n');
        builder.Append(_payerPlace).Append('\n');
        builder.Append(_amount).Append('\n').Append('\n').Append('\n');
        builder.Append(_code.ToUpper(CultureInfo.InvariantCulture)).Append('\n');
        builder.Append(_purpose).Append('\n');
        builder.Append(_deadLine).Append('\n');
        builder.Append(_recipientIban.ToUpper(CultureInfo.InvariantCulture)).Append('\n');
        builder.Append(_recipientSiModel).Append(_recipientSiReference).Append('\n');
        builder.Append(_recipientName).Append('\n');
        builder.Append(_recipientAddress).Append('\n');
        builder.Append(_recipientPlace).Append('\n');
        builder.AppendFormat("{0:000}", CalculateChecksum()).Append('\n');
        
        return builder.ToString();
    }
}