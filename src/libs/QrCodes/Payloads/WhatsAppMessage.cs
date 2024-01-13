using System.Text.RegularExpressions;

namespace QrCodes.Payloads;

/// <summary>
/// 
/// </summary>
public class WhatsAppMessage
{
    private readonly string _number, _message;

    /// <summary>
    /// Let's you compose a WhatApp message and send it the receiver number.
    /// </summary>
    /// <param name="number">
    /// Receiver phone number where the <number/> is a full phone number in international format. 
    /// Omit any zeroes, brackets, or dashes when adding the phone number in international format.
    /// Use: 1XXXXXXXXXX | Don't use: +001-(XXX)XXXXXXX
    /// </param>
    /// <param name="message">The message</param>
    public WhatsAppMessage(string number, string message)
    {
        _number = number;
        _message = message;
    }

    /// <summary>
    /// Let's you compose a WhatApp message.
    /// When scanned the user is asked to choose a contact who will receive the message.
    /// </summary>
    /// <param name="message">The message</param>
    public WhatsAppMessage(string message)
    {
        _number = string.Empty;
        _message = message;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        var cleanedPhone = Regex.Replace(
            _number,
            "^[0+]+|[ ()-]",
            string.Empty);
        
        return $"https://wa.me/{cleanedPhone}?text={Uri.EscapeDataString(_message)}";
    }
}