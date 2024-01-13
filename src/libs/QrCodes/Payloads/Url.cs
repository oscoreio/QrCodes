namespace QrCodes.Payloads;

/// <summary>
/// 
/// </summary>
public class Url
{
    private readonly string _url;

    /// <summary>
    /// Generates a link. If not given, http/https protocol will be added.
    /// </summary>
    /// <param name="url">Link url target</param>
    public Url(string url)
    {
        _url = url;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return !_url.StartsWith("http", StringComparison.Ordinal)
            ? "http://" + _url
            : _url;
    }
}