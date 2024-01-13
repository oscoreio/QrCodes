namespace QrCodes.Payloads;

/// <summary>
/// 
/// </summary>
public class Mms
{
    private readonly string _number, _subject;
    private readonly MmsEncoding _encoding;

    /// <summary>
    /// Creates a MMS payload without text
    /// </summary>
    /// <param name="number">Receiver phone number</param>
    /// <param name="encoding">Encoding type</param>
    public Mms(string number, MmsEncoding encoding = MmsEncoding.Mms)
    {
        _number = number;
        _subject = string.Empty;
        _encoding = encoding;
    }

    /// <summary>
    /// Creates a MMS payload with text (subject)
    /// </summary>
    /// <param name="number">Receiver phone number</param>
    /// <param name="subject">Text of the MMS</param>
    /// <param name="encoding">Encoding type</param>
    public Mms(string number, string subject, MmsEncoding encoding = MmsEncoding.Mms)
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
            case MmsEncoding.MmsTo:
                var queryStringMmsTo = string.Empty;
                if (!string.IsNullOrEmpty(_subject))
                    queryStringMmsTo = $"?subject={Uri.EscapeDataString(_subject)}";
                returnVal = $"mmsto:{_number}{queryStringMmsTo}";
                break;
            case MmsEncoding.Mms:
                var queryStringMms = string.Empty;
                if (!string.IsNullOrEmpty(_subject))
                    queryStringMms = $"?body={Uri.EscapeDataString(_subject)}";
                returnVal = $"mms:{_number}{queryStringMms}";
                break;
        }
        return returnVal;
    }

    /// <summary>
    /// 
    /// </summary>
    public enum MmsEncoding
    {
        /// <summary>
        /// 
        /// </summary>
        Mms,
        
        /// <summary>
        /// 
        /// </summary>
        MmsTo
    }
}