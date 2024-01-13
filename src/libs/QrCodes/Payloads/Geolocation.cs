namespace QrCodes.Payloads;

/// <summary>
/// 
/// </summary>
public class Geolocation
{
    private readonly string _latitude, _longitude;
    private readonly GeolocationEncoding _encoding;

    /// <summary>
    /// Generates a geo location payload. Supports raw location (GEO encoding) or Google Maps link (GoogleMaps encoding)
    /// </summary>
    /// <param name="latitude">Latitude with . as splitter</param>
    /// <param name="longitude">Longitude with . as splitter</param>
    /// <param name="encoding">Encoding type - GEO or GoogleMaps</param>
    public Geolocation(string latitude, string longitude, GeolocationEncoding encoding = GeolocationEncoding.Geo)
    {
        latitude = latitude ?? throw new ArgumentNullException(nameof(latitude));
        longitude = longitude ?? throw new ArgumentNullException(nameof(longitude));
        
        _latitude = latitude.Replace(",",".");
        _longitude = longitude.Replace(",", ".");
        _encoding = encoding;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        switch (_encoding)
        {
            case GeolocationEncoding.Geo:
                return $"geo:{_latitude},{_longitude}";
            case GeolocationEncoding.GoogleMaps:
                return $"http://maps.google.com/maps?q={_latitude},{_longitude}";
            default:
                return "geo:";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum GeolocationEncoding
    {
        /// <summary>
        /// 
        /// </summary>
        Geo,
        
        /// <summary>
        /// 
        /// </summary>
        GoogleMaps
    }
}