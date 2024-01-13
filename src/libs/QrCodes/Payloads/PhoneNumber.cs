namespace QrCodes.Payloads;

/// <summary>
/// 
/// </summary>
public class PhoneNumber
{
    private readonly string _number;

    /// <summary>
    /// Generates a phone call payload
    /// </summary>
    /// <param name="number">Phone number of the receiver</param>
    public PhoneNumber(string number)
    {
        _number = number;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"tel:{_number}";
    }
}