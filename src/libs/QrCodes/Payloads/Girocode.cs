using System.Globalization;
using System.Text;

namespace QrCodes.Payloads;

/// <summary>
/// Keep in mind, that the ECC level has to be set to "M" when generating a Girocode! <br/>
/// Girocode specification: <br/>
/// http://www.europeanpaymentscouncil.eu/index.cfm/knowledge-bank/epc-documents/quick-response-code-guidelines-to-enable-data-capture-for-the-initiation-of-a-sepa-credit-transfer/epc069-12-quick-response-code-guidelines-to-enable-data-capture-for-the-initiation-of-a-sepa-credit-transfer1/ <br/>
/// </summary>
public class Girocode
{
    private readonly string _br = "\n";
    private readonly string _iban, _bic, _name, _purposeOfCreditTransfer, _remittanceInformation, _messageToGirocodeUser;
    private readonly decimal _amount;
    private readonly GirocodeVersion _version;
    private readonly GirocodeEncoding _encoding;
    private readonly TypeOfRemittance _typeOfRemittance;

    /// <summary>
    /// Generates the payload for a Girocode (QR-Code with credit transfer information).
    /// Attention: When using Girocode payload, QR code must be generated with ECC level M!
    /// </summary>
    /// <param name="iban">Account number of the Beneficiary. Only IBAN is allowed.</param>
    /// <param name="bic">BIC of the Beneficiary Bank.</param>
    /// <param name="name">Name of the Beneficiary.</param>
    /// <param name="amount">Amount of the Credit Transfer in Euro.
    /// (Amount must be more than 0.01 and less than 999999999.99)</param>
    /// <param name="remittanceInformation">Remittance Information (Purpose-/reference text). (optional)</param>
    /// <param name="typeOfRemittance">Type of remittance information. Either structured (e.g. ISO 11649 RF Creditor Reference) and max. 35 chars or unstructured and max. 140 chars.</param>
    /// <param name="purposeOfCreditTransfer">Purpose of the Credit Transfer (optional)</param>
    /// <param name="messageToGirocodeUser">Beneficiary to originator information. (optional)</param>
    /// <param name="version">Girocode version. Either 001 or 002. Default: 001.</param>
    /// <param name="encoding">Encoding of the Girocode payload. Default: ISO-8859-1</param>
    public Girocode(
        string iban,
        string bic,
        string name,
        decimal amount,
        string remittanceInformation = "",
        TypeOfRemittance typeOfRemittance = TypeOfRemittance.Unstructured,
        string purposeOfCreditTransfer = "",
        string messageToGirocodeUser = "",
        GirocodeVersion version = GirocodeVersion.Version1,
        GirocodeEncoding encoding = GirocodeEncoding.Iso8859Part1)
    {
        name = name ?? throw new ArgumentNullException(nameof(name));
        remittanceInformation = remittanceInformation ?? throw new ArgumentNullException(nameof(remittanceInformation));
        purposeOfCreditTransfer = purposeOfCreditTransfer ?? throw new ArgumentNullException(nameof(purposeOfCreditTransfer));
        messageToGirocodeUser = messageToGirocodeUser ?? throw new ArgumentNullException(nameof(messageToGirocodeUser));

        this._version = version;
        this._encoding = encoding;
        if (!iban.IsValidIban())
            throw new ArgumentException("The IBAN entered isn't valid.");
        this._iban = iban.Replace(" ","").ToUpper(CultureInfo.InvariantCulture);
        if (!bic.IsValidBic())
            throw new ArgumentException("The BIC entered isn't valid.");
        this._bic = bic.Replace(" ", "").ToUpper(CultureInfo.InvariantCulture);
        if (name.Length > 70)
            throw new ArgumentException("(Payee-)Name must be shorter than 71 chars.");
        this._name = name;
        if (amount.ToString(CultureInfo.InvariantCulture).Replace(",", ".").Contains(".") && amount.ToString(CultureInfo.InvariantCulture).Replace(",",".").Split('.')[1].TrimEnd('0').Length > 2)
            throw new ArgumentException("Amount must have less than 3 digits after decimal point.");
        if (amount < 0.01m || amount > 999999999.99m)
            throw new ArgumentException("Amount has to at least 0.01 and must be smaller or equal to 999999999.99.");
        this._amount = amount;
        if (purposeOfCreditTransfer.Length > 4)
            throw new ArgumentException("Purpose of credit transfer can only have 4 chars at maximum.");
        this._purposeOfCreditTransfer = purposeOfCreditTransfer;
        if (typeOfRemittance == TypeOfRemittance.Unstructured && remittanceInformation.Length > 140)
            throw new ArgumentException("Unstructured reference texts have to shorter than 141 chars.");
        if (typeOfRemittance == TypeOfRemittance.Structured && remittanceInformation.Length > 35)
            throw new ArgumentException("Structured reference texts have to shorter than 36 chars.");
        this._typeOfRemittance = typeOfRemittance;
        this._remittanceInformation = remittanceInformation;
        if (messageToGirocodeUser.Length > 70)
            throw new ArgumentException("Message to the Girocode-User reader texts have to shorter than 71 chars.");
        this._messageToGirocodeUser = messageToGirocodeUser;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        var girocodePayload = "BCD" + _br;
        girocodePayload += ((_version == GirocodeVersion.Version1) ? "001" : "002") + _br;
        girocodePayload += (int)_encoding + 1 + _br;
        girocodePayload += "SCT" + _br;
        girocodePayload += _bic + _br;
        girocodePayload += _name + _br;
        girocodePayload += _iban + _br;
        girocodePayload += $"EUR{_amount:0.00}".Replace(",",".") + _br;
        girocodePayload += _purposeOfCreditTransfer + _br;
        girocodePayload += ((_typeOfRemittance == TypeOfRemittance.Structured)
            ? _remittanceInformation
            : string.Empty) + _br;
        girocodePayload += ((_typeOfRemittance == TypeOfRemittance.Unstructured)
            ? _remittanceInformation
            : string.Empty) + _br;
        girocodePayload += _messageToGirocodeUser;

        return ConvertStringToEncoding(girocodePayload, _encoding switch
        {
            GirocodeEncoding.Utf8 => "UTF-8",
            GirocodeEncoding.Iso8859Part1 => "ISO-8859-1",
            GirocodeEncoding.Iso8859Part2 => "ISO-8859-2",
            GirocodeEncoding.Iso8859Part4 => "ISO-8859-4",
            GirocodeEncoding.Iso8859Part5 => "ISO-8859-5",
            GirocodeEncoding.Iso8859Part7 => "ISO-8859-7",
            GirocodeEncoding.Iso8859Part10 => "ISO-8859-10",
            GirocodeEncoding.Iso8859Part15 => "ISO-8859-15",
            _ => string.Empty,
        });
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    public static string ConvertStringToEncoding(string message, string encoding)
    {
        var iso = Encoding.GetEncoding(encoding);
        var utf8 = Encoding.UTF8;
        byte[] utfBytes = utf8.GetBytes(message);
        byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
        return iso.GetString(isoBytes, 0, isoBytes.Length);
    }
    
    /// <summary>
    /// 
    /// </summary>
    public enum GirocodeVersion
    {
        /// <summary>
        /// 
        /// </summary>
        Version1,
            
        /// <summary>
        /// 
        /// </summary>
        Version2
    }

    /// <summary>
    /// 
    /// </summary>
    public enum TypeOfRemittance
    {
        /// <summary>
        /// 
        /// </summary>
        Structured,
            
        /// <summary>
        /// 
        /// </summary>
        Unstructured
    }

    /// <summary>
    /// 
    /// </summary>
    public enum GirocodeEncoding
    {
        /// <summary>
        /// 
        /// </summary>
        Utf8,
            
        /// <summary>
        /// 
        /// </summary>
        Iso8859Part1,
            
        /// <summary>
        /// 
        /// </summary>
        Iso8859Part2,
            
        /// <summary>
        /// 
        /// </summary>
        Iso8859Part4,
            
        /// <summary>
        /// 
        /// </summary>
        Iso8859Part5,
            
        /// <summary>
        /// 
        /// </summary>
        Iso8859Part7,
            
        /// <summary>
        /// 
        /// </summary>
        Iso8859Part10,
            
        /// <summary>
        /// 
        /// </summary>
        Iso8859Part15,
    }
}