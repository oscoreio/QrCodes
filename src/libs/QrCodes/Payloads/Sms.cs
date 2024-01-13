namespace QrCodes.Payloads;

/// <summary>
/// 
/// </summary>
public class Sms
{
    private readonly string _number, _subject;
    private readonly SmsEncoding _encoding;

    /// <summary>
    /// Creates a SMS payload without text
    /// </summary>
    /// <param name="number">Receiver phone number</param>
    /// <param name="encoding">Encoding type</param>
    public Sms(
        string number,
        SmsEncoding encoding = SmsEncoding.Sms)
    {
        _number = number;
        _subject = string.Empty;
        _encoding = encoding;
    }

    /// <summary>
    /// Creates a SMS payload with text (subject)
    /// </summary>
    /// <param name="number">Receiver phone number</param>
    /// <param name="subject">Text of the SMS</param>
    /// <param name="encoding">Encoding type</param>
    public Sms(
        string number,
        string subject,
        SmsEncoding encoding = SmsEncoding.Sms)
    {
        _number = number;
        _subject = subject;
        _encoding = encoding;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        var returnVal = string.Empty;
        switch (_encoding)
        {
            case SmsEncoding.Sms:
            {
                var queryString = string.Empty;
                if (!string.IsNullOrEmpty(_subject))
                    queryString = $"?body={Uri.EscapeDataString(_subject)}";                        
                returnVal = $"sms:{_number}{queryString}";
                break;
            }
            case SmsEncoding.SmsIos:
            {
                var queryString = string.Empty;
                if (!string.IsNullOrEmpty(_subject))
                    queryString = $";body={Uri.EscapeDataString(_subject)}";
                returnVal = $"sms:{_number}{queryString}";
                break;
            }
            case SmsEncoding.SmsTo:
                returnVal = $"SMSTO:{_number}:{_subject}";
                break;                    
        }
        return returnVal;
    }

    /// <summary>
    /// 
    /// </summary>
    public enum SmsEncoding
    {
        /// <summary>
        ///
        /// </summary>
        Sms,
        
        /// <summary>
        /// 
        /// </summary>
        SmsTo,
        
        /// <summary>
        /// SMS for iOS
        /// </summary>
        SmsIos
    }
}