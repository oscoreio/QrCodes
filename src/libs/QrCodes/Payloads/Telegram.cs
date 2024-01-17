namespace QrCodes.Payloads;

/// <summary>
/// 
/// </summary>
/// <param name="user"></param>
public class Telegram(string user)
{
    /// <inheritdoc />
    public override string ToString()
    {
        return $"https://t.me/{user}";
    }
}