namespace QrCodes.Payloads;

/// <summary>
/// 
/// </summary>
public class SkypeCall
{
    private readonly string _skypeUsername;

    /// <summary>
    /// Generates a Skype call payload
    /// </summary>
    /// <param name="skypeUsername">Skype username which will be called</param>
    public SkypeCall(string skypeUsername)
    {
        _skypeUsername = skypeUsername;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"skype:{_skypeUsername}?call";
    }
}