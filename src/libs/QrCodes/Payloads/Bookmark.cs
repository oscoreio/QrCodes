namespace QrCodes.Payloads;

/// <summary>
/// 
/// </summary>
public class Bookmark
{
    private readonly string _url, _title;

    /// <summary>
    /// Generates a bookmark payload. Scanned by an QR Code reader, this one creates a browser bookmark.
    /// </summary>
    /// <param name="url">Url of the bookmark</param>
    /// <param name="title">Title of the bookmark</param>
    public Bookmark(string url, string title)
    {
        _url = url.EscapeInput();
        _title = title.EscapeInput();
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return $"MEBKM:TITLE:{_title};URL:{_url};;";
    }
}