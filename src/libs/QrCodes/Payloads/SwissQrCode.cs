using System.Globalization;
using System.Text.RegularExpressions;

// ReSharper disable NotAccessedField.Local

namespace QrCodes.Payloads;

/// <summary>
/// Keep in mind, that the ECC level has to be set to "M" when generating a SwissQrCode! <br/>
/// SwissQrCode specification:  <br/>
/// - (de) https://www.paymentstandards.ch/dam/downloads/ig-qr-bill-de.pdf <br/>
/// - (en) https://www.paymentstandards.ch/dam/downloads/ig-qr-bill-en.pdf <br/>
/// Changes between version 1.0 and 2.0:  <br/>
/// https://www.paymentstandards.ch/dam/downloads/change-documentation-qrr-de.pdf <br/>
/// </summary>
public class SwissQrCode
{
    private readonly string _br = "\r\n";
    private readonly string _alternativeProcedure1, _alternativeProcedure2;
    private readonly Iban _iban;
    private readonly decimal? _amount;
    private readonly Contact? _creditor, _ultimateCreditor, _debitor;
    private readonly Currency _currency;
    private readonly DateTime? _requestedDateOfPayment;
    private readonly Reference _reference;
    private readonly AdditionalInformation _additionalInformation;

    /// <summary>
    /// Generates the payload for a SwissQrCode v2.0. (Don't forget to use ECC-Level=M, EncodingMode=UTF-8 and to set the Swiss flag icon to the final QR code.)
    /// </summary>
    /// <param name="iban">IBAN object</param>
    /// <param name="currency">Currency (either EUR or CHF)</param>
    /// <param name="creditor">Creditor (payee) information</param>
    /// <param name="reference">Reference information</param>
    /// <param name="additionalInformation"></param>
    /// <param name="debitor">Debitor (payer) information</param>
    /// <param name="amount">Amount</param>
    /// <param name="requestedDateOfPayment">Requested date of debitor's payment</param>
    /// <param name="ultimateCreditor">Ultimate creditor information (use only in consultation with your bank - for future use only!)</param>
    /// <param name="alternativeProcedure1">Optional command for alternative processing mode - line 1</param>
    /// <param name="alternativeProcedure2">Optional command for alternative processing mode - line 2</param>
    public SwissQrCode(
        Iban iban,
        Currency currency,
        Contact creditor,
        Reference reference,
        AdditionalInformation? additionalInformation = null,
        Contact? debitor = null,
        decimal? amount = null,
        DateTime? requestedDateOfPayment = null,
        Contact? ultimateCreditor = null,
        string? alternativeProcedure1 = null,
        string? alternativeProcedure2 = null)
    {
        _iban = iban ?? throw new ArgumentNullException(nameof(iban));

        _creditor = creditor;
        _ultimateCreditor = ultimateCreditor;

        _additionalInformation = additionalInformation ?? new AdditionalInformation();

        if (amount != null && amount.ToString()?.Length > 12)
            throw new ArgumentException("Amount (including decimals) must be shorter than 13 places.");
        _amount = amount;

        _currency = currency;
        _requestedDateOfPayment = requestedDateOfPayment;
        _debitor = debitor;
        
        _reference = reference ?? throw new ArgumentNullException(nameof(reference));
        if (iban.IsQrIban && reference.RefType != Reference.ReferenceType.QRR)
            throw new ArgumentException("If QR-IBAN is used, you have to choose \"QRR\" as reference type!");
        if (!iban.IsQrIban && reference.RefType == Reference.ReferenceType.QRR)
            throw new ArgumentException("If non QR-IBAN is used, you have to choose either \"SCOR\" or \"NON\" as reference type!");

        if (alternativeProcedure1 is { Length: > 100 })
            throw new ArgumentException("Alternative procedure information block 1 must be shorter than 101 chars.");
        _alternativeProcedure1 = alternativeProcedure1 ?? string.Empty;
        if (alternativeProcedure2 is { Length: > 100 })
            throw new ArgumentException("Alternative procedure information block 2 must be shorter than 101 chars.");
        _alternativeProcedure2 = alternativeProcedure2 ?? string.Empty;
    }

    /// <summary>
    /// 
    /// </summary>
    public class AdditionalInformation
    {
        private readonly string _unstructuredMessage, _billInformation;

        /// <summary>
        /// Creates an additional information object. Both parameters are optional and must be shorter than 141 chars in combination.
        /// </summary>
        /// <param name="unstructuredMessage">Unstructured text message</param>
        /// <param name="billInformation">Bill information</param>
        public AdditionalInformation(
            string? unstructuredMessage = null,
            string? billInformation = null)
        {
            if ((unstructuredMessage?.Length ?? 0) + (billInformation?.Length ?? 0) > 140)
                throw new ArgumentException("Unstructured message and bill information must be shorter than 141 chars in total/combined.");
            _unstructuredMessage = unstructuredMessage ?? string.Empty;
            _billInformation = billInformation ?? string.Empty;
            Trailer = "EPD";
        }

        /// <summary>
        /// 
        /// </summary>
        public string UnstructuredMessage => !string.IsNullOrEmpty(_unstructuredMessage)
            ? _unstructuredMessage.Replace("\n", "")
            : string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string BillInformation => !string.IsNullOrEmpty(_billInformation)
            ? _billInformation.Replace("\n", "")
            : string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string Trailer { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Reference
    {
        private readonly string _reference;
        private readonly ReferenceTextType? _referenceTextType;

        /// <summary>
        /// Creates a reference object which must be passed to the SwissQrCode instance
        /// </summary>
        /// <param name="referenceType">Type of the reference (QRR, SCOR or NON)</param>
        /// <param name="reference">Reference text</param>
        /// <param name="referenceTextType">Type of the reference text (QR-reference or Creditor Reference)</param>                
        public Reference(
            ReferenceType referenceType,
            string? reference = null,
            ReferenceTextType? referenceTextType = null)
        {
            RefType = referenceType;
            _referenceTextType = referenceTextType;

            if (referenceType == ReferenceType.NON && reference != null)
                throw new ArgumentException("Reference is only allowed when referenceType not equals \"NON\"");
            if (referenceType != ReferenceType.NON && reference != null && referenceTextType == null)
                throw new ArgumentException("You have to set an ReferenceTextType when using the reference text.");
            if (referenceTextType == ReferenceTextType.QrReference && reference is { Length: > 27 })
                throw new ArgumentException("QR-references have to be shorter than 28 chars.");
            if (referenceTextType == ReferenceTextType.QrReference && reference != null && !Regex.IsMatch(reference, @"^[0-9]+$"))
                throw new ArgumentException("QR-reference must exist out of digits only.");
            if (referenceTextType == ReferenceTextType.QrReference && reference != null && !ChecksumMod10(reference))
                throw new ArgumentException("QR-references is invalid. Checksum error.");
            if (referenceTextType == ReferenceTextType.CreditorReferenceIso11649 && reference is { Length: > 25 })
                throw new ArgumentException("Creditor references (ISO 11649) have to be shorter than 26 chars.");

            _reference = reference ?? string.Empty;                   
        }

        /// <summary>
        /// 
        /// </summary>
        public ReferenceType RefType { get; }

        /// <summary>
        /// 
        /// </summary>
        public string ReferenceText => !string.IsNullOrEmpty(_reference)
            ? _reference.Replace("\n", "")
            : string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="digits"></param>
        /// <returns></returns>
        // ReSharper disable once MemberCanBePrivate.Global
        public static bool ChecksumMod10(string digits)
        {
            if (string.IsNullOrEmpty(digits) || digits.Length < 2)
                return false;
            int[] mods = [0, 9, 4, 6, 8, 2, 7, 1, 3, 5];

            int remainder = 0;
            for (int i = 0; i < digits.Length - 1; i++)
            {
                var num = Convert.ToInt32(digits[i]) - 48;
                remainder = mods[(num + remainder) % 10];
            }
            var checksum = (10 - remainder) % 10;
            return checksum == Convert.ToInt32(digits[digits.Length - 1]) - 48;
        }
        
        /// <summary>
        /// Reference type. When using a QR-IBAN you have to use either "QRR" or "SCOR"
        /// </summary>
        // ReSharper disable InconsistentNaming
        public enum ReferenceType
        {
            /// <summary>
            /// 
            /// </summary>
            QRR,
            
            /// <summary>
            /// 
            /// </summary>
            SCOR,
            
            /// <summary>
            /// 
            /// </summary>
            NON
        }

        /// <summary>
        /// 
        /// </summary>
        public enum ReferenceTextType
        {
            /// <summary>
            /// 
            /// </summary>
            QrReference,
            
            /// <summary>
            /// 
            /// </summary>
            CreditorReferenceIso11649
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Iban
    {
        private readonly string _iban;
        private readonly IbanType _ibanType;

        /// <summary>
        /// IBAN object with type information
        /// </summary>
        /// <param name="iban">IBAN</param>
        /// <param name="ibanType">Type of IBAN (normal or QR-IBAN)</param>
        public Iban(string iban, IbanType ibanType)
        {
            iban = iban ?? throw new ArgumentNullException(nameof(iban));
            
            if (ibanType == IbanType.Iban && !iban.IsValidIban())
                throw new ArgumentException("The IBAN entered isn't valid.");
            if (ibanType == IbanType.QrIban && !IsValidQrIban(iban))
                throw new ArgumentException("The QR-IBAN entered isn't valid.");
            if (!iban.StartsWith("CH", StringComparison.Ordinal) &&
                !iban.StartsWith("LI", StringComparison.Ordinal))
                throw new ArgumentException("The IBAN must start with \"CH\" or \"LI\".");
            _iban = iban;
            _ibanType = ibanType;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsQrIban => _ibanType == IbanType.QrIban;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="iban"></param>
        /// <returns></returns>
        public static bool IsValidQrIban(string iban)
        {
            iban = iban ?? throw new ArgumentNullException(nameof(iban));
        
            var foundQrIid = false;
            try
            {
                var ibanCleared = iban.ToUpper(CultureInfo.InvariantCulture).Replace(" ", "").Replace("-", "");
                var possibleQrIid = Convert.ToInt32(ibanCleared.Substring(4, 5));
                foundQrIid = possibleQrIid is >= 30000 and <= 31999;
            }
            catch (Exception)
            {
                // ignored
            }
        
            return iban.IsValidIban() && foundQrIid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _iban.Replace("-", "").Replace("\n", "").Replace(" ","");
        }

        /// <summary>
        /// 
        /// </summary>
        public enum IbanType
        {
            /// <summary>
            /// 
            /// </summary>
            Iban,
            
            /// <summary>
            /// 
            /// </summary>
            QrIban
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Contact
    {
        private static readonly HashSet<string> TwoLetterCodes = ValidTwoLetterCodes();
        private const string Br = "\r\n";
        private readonly string _name;
        private readonly string _streetOrAddressLine1;
        private readonly string _houseNumberOrAddressLine2;
        private readonly string _zipCode;
        private readonly string _city;
        private readonly string _country;
        private readonly AddressType _adrType;

        /// <summary>
        /// Contact type. Can be used for payee, ultimate payee, etc. with address in structured mode (S).
        /// </summary>
        /// <param name="name">Last name or company (optional first name)</param>
        /// <param name="zipCode">Zip-/Postcode</param>
        /// <param name="city">City name</param>
        /// <param name="country">Two-letter country code as defined in ISO 3166-1</param>
        /// <param name="street">Street name without house number</param>
        /// <param name="houseNumber">House number</param>
        [Obsolete("This constructor is deprecated. Use WithStructuredAddress instead.")]
        public Contact(
            string name,
            string zipCode,
            string city,
            string country,
            string? street = null, 
            string? houseNumber = null)
            : this(name, zipCode, city, country, street ?? string.Empty, houseNumber ?? string.Empty, AddressType.StructuredAddress)
        {
        }


        /// <summary>
        /// Contact type. Can be used for payee, ultimate payee, etc. with address in combined mode (K).
        /// </summary>
        /// <param name="name">Last name or company (optional first name)</param>
        /// <param name="country">Two-letter country code as defined in ISO 3166-1</param>
        /// <param name="addressLine1">Address line 1</param>
        /// <param name="addressLine2">Address line 2</param>
        [Obsolete("This constructor is deprecated. Use WithCombinedAddress instead.")]
        public Contact(string name, string country, string addressLine1, string addressLine2)
            : this(name, string.Empty, string.Empty, country, addressLine1, addressLine2, AddressType.CombinedAddress)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="zipCode"></param>
        /// <param name="city"></param>
        /// <param name="country"></param>
        /// <param name="street"></param>
        /// <param name="houseNumber"></param>
        /// <returns></returns>
        public static Contact WithStructuredAddress(
            string name,
            string zipCode,
            string city,
            string country,
            string? street = null,
            string? houseNumber = null)
        {
            return new Contact(name, zipCode, city, country, street ?? string.Empty, houseNumber ?? string.Empty, AddressType.StructuredAddress);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="country"></param>
        /// <param name="addressLine1"></param>
        /// <param name="addressLine2"></param>
        /// <returns></returns>
        public static Contact WithCombinedAddress(
            string name, string country, string addressLine1, string addressLine2)
        {
            return new Contact(name, string.Empty, string.Empty, country, addressLine1, addressLine2, AddressType.CombinedAddress);
        }


        private Contact(
            string name,
            string zipCode,
            string city,
            string country,
            string streetOrAddressLine1,
            string houseNumberOrAddressLine2,
            AddressType addressType)
        {
            //Pattern extracted from https://qr-validation.iso-payments.ch as explained in https://github.com/codebude/QRCoder/issues/97
            var charsetPattern = @"^([a-zA-Z0-9\.,;:'\ \+\-/\(\)?\*\[\]\{\}\\`´~ ]|[!""#%&<>÷=@_$£]|[àáâäçèéêëìíîïñòóôöùúûüýßÀÁÂÄÇÈÉÊËÌÍÎÏÒÓÔÖÙÚÛÜÑ])*$";

            _adrType = addressType;

            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Name must not be empty.");
            if (name.Length > 70)
                throw new ArgumentException("Name must be shorter than 71 chars.");
            if (!Regex.IsMatch(name, charsetPattern))
                throw new ArgumentException($"Name must match the following pattern as defined in pain.001: {charsetPattern}");
            _name = name;

            if (AddressType.StructuredAddress == _adrType)
            {
                if (!string.IsNullOrEmpty(streetOrAddressLine1) && (streetOrAddressLine1.Length > 70))
                    throw new ArgumentException("Street must be shorter than 71 chars.");
                if (!string.IsNullOrEmpty(streetOrAddressLine1) && !Regex.IsMatch(streetOrAddressLine1, charsetPattern))
                    throw new ArgumentException($"Street must match the following pattern as defined in pain.001: {charsetPattern}");
                _streetOrAddressLine1 = streetOrAddressLine1;

                if (!string.IsNullOrEmpty(houseNumberOrAddressLine2) && houseNumberOrAddressLine2.Length > 16)
                    throw new ArgumentException("House number must be shorter than 17 chars.");
                _houseNumberOrAddressLine2 = houseNumberOrAddressLine2;
            }
            else
            {
                if (!string.IsNullOrEmpty(streetOrAddressLine1) && (streetOrAddressLine1.Length > 70))
                    throw new ArgumentException("Address line 1 must be shorter than 71 chars.");
                if (!string.IsNullOrEmpty(streetOrAddressLine1) && !Regex.IsMatch(streetOrAddressLine1, charsetPattern))
                    throw new ArgumentException($"Address line 1 must match the following pattern as defined in pain.001: {charsetPattern}");
                _streetOrAddressLine1 = streetOrAddressLine1;

                if (string.IsNullOrEmpty(houseNumberOrAddressLine2))
                    throw new ArgumentException("Address line 2 must be provided for combined addresses (address line-based addresses).");
                if (!string.IsNullOrEmpty(houseNumberOrAddressLine2) && (houseNumberOrAddressLine2.Length > 70))
                    throw new ArgumentException("Address line 2 must be shorter than 71 chars.");
                if (!string.IsNullOrEmpty(houseNumberOrAddressLine2) && !Regex.IsMatch(houseNumberOrAddressLine2, charsetPattern))
                    throw new ArgumentException($"Address line 2 must match the following pattern as defined in pain.001: {charsetPattern}");
                _houseNumberOrAddressLine2 = houseNumberOrAddressLine2;
            }

            if (AddressType.StructuredAddress == _adrType) {
                if (string.IsNullOrEmpty(zipCode))
                    throw new ArgumentException("Zip code must not be empty.");
                if (zipCode.Length > 16)
                    throw new ArgumentException("Zip code must be shorter than 17 chars.");
                if (!Regex.IsMatch(zipCode, charsetPattern))
                    throw new ArgumentException($"Zip code must match the following pattern as defined in pain.001: {charsetPattern}");
                _zipCode = zipCode;

                if (string.IsNullOrEmpty(city))
                    throw new ArgumentException("City must not be empty.");
                if (city.Length > 35)
                    throw new ArgumentException("City name must be shorter than 36 chars.");
                if (!Regex.IsMatch(city, charsetPattern))
                    throw new ArgumentException($"City name must match the following pattern as defined in pain.001: {charsetPattern}");
                _city = city;
            }
            else
            {
                _zipCode = _city = string.Empty;
            }

            if (!IsValidTwoLetterCode(country))
                throw new ArgumentException("Country must be a valid \"two letter\" country code as defined by  ISO 3166-1, but it isn't.");

            _country = country;
        }

        private static bool IsValidTwoLetterCode(string code) => TwoLetterCodes.Contains(code);

        private static HashSet<string> ValidTwoLetterCodes()
        {
            string[] codes = ["AF", "AL", "DZ", "AS", "AD", "AO", "AI", "AQ", "AG", "AR", "AM", "AW", "AU", "AT", "AZ", "BS", "BH", "BD", "BB", "BY", "BE", "BZ", "BJ", "BM", "BT", "BO", "BQ", "BA", "BW", "BV", "BR", "IO", "BN", "BG", "BF", "BI", "CV", "KH", "CM", "CA", "KY", "CF", "TD", "CL", "CN", "CX", "CC", "CO", "KM", "CG", "CD", "CK", "CR", "CI", "HR", "CU", "CW", "CY", "CZ", "DK", "DJ", "DM", "DO", "EC", "EG", "SV", "GQ", "ER", "EE", "SZ", "ET", "FK", "FO", "FJ", "FI", "FR", "GF", "PF", "TF", "GA", "GM", "GE", "DE", "GH", "GI", "GR", "GL", "GD", "GP", "GU", "GT", "GG", "GN", "GW", "GY", "HT", "HM", "VA", "HN", "HK", "HU", "IS", "IN", "ID", "IR", "IQ", "IE", "IM", "IL", "IT", "JM", "JP", "JE", "JO", "KZ", "KE", "KI", "KP", "KR", "KW", "KG", "LA", "LV", "LB", "LS", "LR", "LY", "LI", "LT", "LU", "MO", "MG", "MW", "MY", "MV", "ML", "MT", "MH", "MQ", "MR", "MU", "YT", "MX", "FM", "MD", "MC", "MN", "ME", "MS", "MA", "MZ", "MM", "NA", "NR", "NP", "NL", "NC", "NZ", "NI", "NE", "NG", "NU", "NF", "MP", "MK", "NO", "OM", "PK", "PW", "PS", "PA", "PG", "PY", "PE", "PH", "PN", "PL", "PT", "PR", "QA", "RE", "RO", "RU", "RW", "BL", "SH", "KN", "LC", "MF", "PM", "VC", "WS", "SM", "ST", "SA", "SN", "RS", "SC", "SL", "SG", "SX", "SK", "SI", "SB", "SO", "ZA", "GS", "SS", "ES", "LK", "SD", "SR", "SJ", "SE", "CH", "SY", "TW", "TJ", "TZ", "TH", "TL", "TG", "TK", "TO", "TT", "TN", "TR", "TM", "TC", "TV", "UG", "UA", "AE", "GB", "US", "UM", "UY", "UZ", "VU", "VE", "VN", "VG", "VI", "WF", "EH", "YE", "ZM", "ZW", "AX"];
            return new HashSet<string>(codes, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string contactData = $"{(AddressType.StructuredAddress == _adrType ? "S" : "K")}{Br}"; //AdrTp
            contactData += _name.Replace("\n", "") + Br; //Name
            contactData += (!string.IsNullOrEmpty(_streetOrAddressLine1)
                ? _streetOrAddressLine1.Replace("\n","")
                : string.Empty) + Br; //StrtNmOrAdrLine1
            contactData += (!string.IsNullOrEmpty(_houseNumberOrAddressLine2)
                ? _houseNumberOrAddressLine2.Replace("\n", "")
                : string.Empty) + Br; //BldgNbOrAdrLine2
            contactData += _zipCode.Replace("\n", "") + Br; //PstCd
            contactData += _city.Replace("\n", "") + Br; //TwnNm
            contactData += _country + Br; //Ctry
            return contactData;
        }

        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        public enum AddressType
        {
            /// <summary>
            /// 
            /// </summary>
            StructuredAddress,
            
            /// <summary>
            /// 
            /// </summary>
            CombinedAddress,
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        //Header "logical" element
        var swissQrCodePayload = "SPC" + _br; //QRType
        swissQrCodePayload += "0200" + _br; //Version
        swissQrCodePayload += "1" + _br; //Coding

        //CdtrInf "logical" element
        swissQrCodePayload += _iban + _br; //IBAN


        //Cdtr "logical" element
        swissQrCodePayload += _creditor?.ToString() ?? string.Empty;

        //UltmtCdtr "logical" element
        //Since version 2.0 ultimate creditor was marked as "for future use" and has to be delivered empty in any case!
        swissQrCodePayload += string.Concat(Enumerable.Repeat(_br, 7).ToArray());

        //CcyAmtDate "logical" element
        //Amount has to use . as decimal separator in any case. See https://www.paymentstandards.ch/dam/downloads/ig-qr-bill-en.pdf page 27.
        swissQrCodePayload += (_amount != null ? $"{_amount:0.00}".Replace(",", ".") : string.Empty) + _br; //Amt
        swissQrCodePayload += _currency + _br; //Ccy                
        //Removed in S-QR version 2.0
        //SwissQrCodePayload += (requestedDateOfPayment != null ?  ((DateTime)requestedDateOfPayment).ToString("yyyy-MM-dd") : string.Empty) + br; //ReqdExctnDt

        //UltmtDbtr "logical" element
        if (_debitor != null)
            swissQrCodePayload += _debitor.ToString();
        else
            swissQrCodePayload += string.Concat(Enumerable.Repeat(_br, 7).ToArray());


        //RmtInf "logical" element
        swissQrCodePayload += _reference.RefType + _br; //Tp
        swissQrCodePayload += (!string.IsNullOrEmpty(_reference.ReferenceText) ? _reference.ReferenceText : string.Empty) + _br; //Ref
                               

        //AddInf "logical" element
        swissQrCodePayload += (!string.IsNullOrEmpty(_additionalInformation.UnstructuredMessage)
            ? _additionalInformation.UnstructuredMessage
            : string.Empty) + _br; //Ustrd
        swissQrCodePayload += _additionalInformation.Trailer + _br; //Trailer
        swissQrCodePayload += (!string.IsNullOrEmpty(_additionalInformation.BillInformation)
            ? _additionalInformation.BillInformation
            : string.Empty) + _br; //StrdBkgInf

        //AltPmtInf "logical" element
        if (!string.IsNullOrEmpty(_alternativeProcedure1))
            swissQrCodePayload += _alternativeProcedure1.Replace("\n", "") + _br; //AltPmt
        if (!string.IsNullOrEmpty(_alternativeProcedure2))
            swissQrCodePayload += _alternativeProcedure2.Replace("\n", "") + _br; //AltPmt

        //S-QR specification 2.0, chapter 4.2.3
        if (swissQrCodePayload.EndsWith(_br, StringComparison.Ordinal))
            swissQrCodePayload = swissQrCodePayload.Remove(swissQrCodePayload.Length - _br.Length);

        return swissQrCodePayload;
    }




    /// <summary>
    /// ISO 4217 currency codes
    /// </summary>
    // ReSharper disable InconsistentNaming
    public enum Currency
    {
        /// <summary>
        /// 
        /// </summary>
        CHF = 756,
        
        /// <summary>
        /// 
        /// </summary>
        EUR = 978
    }
}