namespace QrCodes.Payloads;

/// <summary>
/// 
/// </summary>
public class Mail
{
    private readonly string _mailReceiver, _subject, _message;
    private readonly MailEncoding _encoding;
          
    /// <summary>
    /// Creates an email payload with subject and message/text
    /// </summary>
    /// <param name="mailReceiver">Receiver's email address</param>
    /// <param name="subject">Subject line of the email</param>
    /// <param name="message">Message content of the email</param>
    /// <param name="encoding">Payload encoding type. Choose dependent on your QR Code scanner app.</param>
    public Mail(
        string? mailReceiver = null,
        string? subject = null,
        string? message = null,
        MailEncoding encoding = MailEncoding.MailTo)
    {
        _mailReceiver = mailReceiver ?? string.Empty;
        _subject = subject ?? string.Empty;
        _message = message ?? string.Empty;
        _encoding = encoding;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        var returnVal = string.Empty;
        switch (_encoding)
        {
            case MailEncoding.MailTo:
                var parts = new List<string>();
                if (!string.IsNullOrEmpty(_subject))
                    parts.Add("subject=" + Uri.EscapeDataString(_subject));
                if (!string.IsNullOrEmpty(_message))
                    parts.Add("body=" + Uri.EscapeDataString(_message));
                var queryString = parts.Count != 0
                    ? $"?{string.Join("&", parts.ToArray())}"
                    : "";
                returnVal = $"mailto:{_mailReceiver}{queryString}";
                break;
            case MailEncoding.MatMsg:
                returnVal = $"MATMSG:TO:{_mailReceiver};SUB:{_subject.EscapeInput()};BODY:{_message.EscapeInput()};;";
                break;
            case MailEncoding.Smtp:
                returnVal = $"SMTP:{_mailReceiver}:{_subject.EscapeInput(true)}:{_message.EscapeInput(true)}";
                break;
        }
        return returnVal;
    }

    /// <summary>
    /// 
    /// </summary>
    public enum MailEncoding
    {
        /// <summary>
        /// 
        /// </summary>
        MailTo,
        
        /// <summary>
        /// 
        /// </summary>
        MatMsg,
        
        /// <summary>
        /// 
        /// </summary>
        Smtp,
    }
}