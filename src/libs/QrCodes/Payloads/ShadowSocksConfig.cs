using System.Text;

// ReSharper disable UnusedMember.Global

namespace QrCodes.Payloads;

/// <summary>
/// 
/// </summary>
public class ShadowSocksConfig
{
    private readonly string _hostname, _password, _tag, _methodStr, _parameter;
    // ReSharper disable once NotAccessedField.Local
    private readonly Method _method;
    private readonly int _port;
    private readonly Dictionary<string, string> _encryptionTexts = new() {
        { "Chacha20IetfPoly1305", "chacha20-ietf-poly1305" },
        { "Aes128Gcm", "aes-128-gcm" },
        { "Aes192Gcm", "aes-192-gcm" },
        { "Aes256Gcm", "aes-256-gcm" },

        { "XChacha20IetfPoly1305", "xchacha20-ietf-poly1305" },

        { "Aes128Cfb", "aes-128-cfb" },
        { "Aes192Cfb", "aes-192-cfb" },
        { "Aes256Cfb", "aes-256-cfb" },
        { "Aes128Ctr", "aes-128-ctr" },
        { "Aes192Ctr", "aes-192-ctr" },
        { "Aes256Ctr", "aes-256-ctr" },
        { "Camellia128Cfb", "camellia-128-cfb" },
        { "Camellia192Cfb", "camellia-192-cfb" },
        { "Camellia256Cfb", "camellia-256-cfb" },
        { "Chacha20Ietf", "chacha20-ietf" },

        { "Aes256Cb", "aes-256-cfb" },

        { "Aes128Ofb", "aes-128-ofb" },
        { "Aes192Ofb", "aes-192-ofb" },
        { "Aes256Ofb", "aes-256-ofb" },
        { "Aes128Cfb1", "aes-128-cfb1" },
        { "Aes192Cfb1", "aes-192-cfb1" },
        { "Aes256Cfb1", "aes-256-cfb1" },
        { "Aes128Cfb8", "aes-128-cfb8" },
        { "Aes192Cfb8", "aes-192-cfb8" },
        { "Aes256Cfb8", "aes-256-cfb8" },

        { "Chacha20", "chacha20" },
        { "BfCfb", "bf-cfb" },
        { "Rc4Md5", "rc4-md5" },
        { "Salsa20", "salsa20" },

        { "DesCfb", "des-cfb" },
        { "IdeaCfb", "idea-cfb" },
        { "Rc2Cfb", "rc2-cfb" },
        { "Cast5Cfb", "cast5-cfb" },
        { "Salsa20Ctr", "salsa20-ctr" },
        { "Rc4", "rc4" },
        { "SeedCfb", "seed-cfb" },
        { "Table", "table" }
    };

    /// <summary>
    /// Generates a ShadowSocks proxy config payload.
    /// </summary>
    /// <param name="hostname">Hostname of the ShadowSocks proxy</param>
    /// <param name="port">Port of the ShadowSocks proxy</param>
    /// <param name="password">Password of the SS proxy</param>
    /// <param name="method">Encryption type</param>
    /// <param name="tag">Optional tag line</param>
    public ShadowSocksConfig(
        string hostname,
        int port,
        string password,
        Method method,
        string? tag = null) :
        this(hostname, port, password, method, new Dictionary<string, string>(), tag ?? string.Empty)
    { }

    /// <summary>
    /// Generates a ShadowSocks proxy config payload.
    /// </summary>
    /// <param name="hostname">Hostname of the ShadowSocks proxy</param>
    /// <param name="port">Port of the ShadowSocks proxy</param>
    /// <param name="password">Password of the SS proxy</param>
    /// <param name="method">Encryption type</param>
    /// <param name="pluginOption"></param>
    /// <param name="tag">Optional tag line</param>
    /// <param name="plugin"></param>
    public ShadowSocksConfig(
        string hostname,
        int port,
        string password,
        Method method,
        string plugin,
        string pluginOption,
        string? tag = null) :
        this(hostname, port, password, method, new Dictionary<string, string>
        {
            ["plugin"] = plugin + (
                string.IsNullOrEmpty(pluginOption)
                    ? ""
                    : $";{pluginOption}"
            )
        }, tag ?? string.Empty)
    { }
    
    private readonly Dictionary<string, string> _urlEncodeTable = new()
    {
        [" "] = "+",
        ["\0"] = "%00",
        ["\t"] = "%09",
        ["\n"] = "%0a",
        ["\r"] = "%0d",
        ["\""] = "%22",
        ["#"] = "%23",
        ["$"] = "%24",
        ["%"] = "%25",
        ["&"] = "%26",
        ["'"] = "%27",
        ["+"] = "%2b",
        [","] = "%2c",
        ["/"] = "%2f",
        [":"] = "%3a",
        [";"] = "%3b",
        ["<"] = "%3c",
        ["="] = "%3d",
        [">"] = "%3e",
        ["?"] = "%3f",
        ["@"] = "%40",
        ["["] = "%5b",
        ["\\"] = "%5c",
        ["]"] = "%5d",
        ["^"] = "%5e",
        ["`"] = "%60",
        ["{"] = "%7b",
        ["|"] = "%7c",
        ["}"] = "%7d",
        ["~"] = "%7e",
    };

    private string UrlEncode(string i)
    {
        string j = i;
        foreach (var kv in _urlEncodeTable)
        {
            j = j.Replace(kv.Key, kv.Value);
        }
        return j;
    }

    /// <summary>
    /// Generates a ShadowSocks proxy config payload.
    /// </summary>
    /// <param name="hostname">Hostname of the ShadowSocks proxy</param>
    /// <param name="port">Port of the ShadowSocks proxy</param>
    /// <param name="password">Password of the SS proxy</param>
    /// <param name="method">Encryption type</param>
    /// <param name="parameters"></param>
    /// <param name="tag">Optional tag line</param>
    // ReSharper disable once MemberCanBePrivate.Global
    public ShadowSocksConfig(
        string hostname,
        int port,
        string password,
        Method method,
        Dictionary<string, string> parameters,
        string? tag = null)
    {
        _hostname = Uri.CheckHostName(hostname) == UriHostNameType.IPv6
            ? $"[{hostname}]"
            : hostname;
        if (port < 1 || port > 65535)
            throw new ArgumentException("Value of 'port' must be within 0 and 65535.");
        _port = port;
        _password = password;
        _method = method;
        _methodStr = _encryptionTexts[method.ToString()];
        _tag = tag ?? string.Empty;

        _parameter =
            string.Join("&",
                parameters.Select(
                    kv => $"{UrlEncode(kv.Key)}={UrlEncode(kv.Value)}"
                ).ToArray());
    }

    /// <inheritdoc />
    public override string ToString()
    {
        if (string.IsNullOrEmpty(_parameter))
        {
            var connectionString = $"{_methodStr}:{_password}@{_hostname}:{_port}";
            var connectionStringEncoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(connectionString));
            return $"ss://{connectionStringEncoded}{(!string.IsNullOrEmpty(_tag) ? $"#{_tag}" : string.Empty)}";
        }
        var authString = $"{_methodStr}:{_password}";
        var authStringEncoded = Convert.ToBase64String(Encoding.UTF8.GetBytes(authString))
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');
        return $"ss://{authStringEncoded}@{_hostname}:{_port}/?{_parameter}{(!string.IsNullOrEmpty(_tag) ? $"#{_tag}" : string.Empty)}";
    }

    /// <summary>
    /// 
    /// </summary>
    public enum Method
    {
        /// <summary>
        /// AEAD
        /// </summary>
        Chacha20IetfPoly1305,
        
        /// <summary>
        /// AEAD
        /// </summary>
        Aes128Gcm,
        
        /// <summary>
        /// AEAD
        /// </summary>
        Aes192Gcm,
        
        /// <summary>
        /// AEAD
        /// </summary>
        Aes256Gcm,
        
        /// <summary>
        /// AEAD, not standard
        /// </summary>
        XChacha20IetfPoly1305,
        
        /// <summary>
        /// Stream cipher
        /// </summary>
        Aes128Cfb,
        
        /// <summary>
        /// Stream cipher
        /// </summary>
        Aes192Cfb,
        
        /// <summary>
        /// Stream cipher
        /// </summary>
        Aes256Cfb,
        
        /// <summary>
        /// Stream cipher
        /// </summary>
        Aes128Ctr,
        
        /// <summary>
        /// Stream cipher
        /// </summary>
        Aes192Ctr,
        
        /// <summary>
        /// Stream cipher
        /// </summary>
        Aes256Ctr,
        
        /// <summary>
        /// Stream cipher
        /// </summary>
        Camellia128Cfb,
        
        /// <summary>
        /// Stream cipher
        /// </summary>
        Camellia192Cfb,
        
        /// <summary>
        /// Stream cipher
        /// </summary>
        Camellia256Cfb,
        
        /// <summary>
        /// Stream cipher
        /// </summary>
        Chacha20Ietf,
        
        /// <summary>
        /// alias of Aes256Cfb
        /// </summary>
        Aes256Cb,
        
        /// <summary>
        /// Stream cipher, not standard
        /// </summary>
        Aes128Ofb,
        
        /// <summary>
        /// Stream cipher, not standard
        /// </summary>
        Aes192Ofb,
        
        /// <summary>
        /// Stream cipher, not standard
        /// </summary>
        Aes256Ofb,
        
        /// <summary>
        /// Stream cipher, not standard
        /// </summary>
        Aes128Cfb1,
        
        /// <summary>
        /// Stream cipher, not standard
        /// </summary>
        Aes192Cfb1,
        
        /// <summary>
        /// Stream cipher, not standard
        /// </summary>
        Aes256Cfb1,
        
        /// <summary>
        /// Stream cipher, not standard
        /// </summary>
        Aes128Cfb8,
        
        /// <summary>
        /// Stream cipher, not standard
        /// </summary>
        Aes192Cfb8,
        
        /// <summary>
        /// Stream cipher, not standard
        /// </summary>
        Aes256Cfb8,
        
        /// <summary>
        /// Stream cipher, deprecated
        /// </summary>
        Chacha20,
        
        /// <summary>
        /// Stream cipher, deprecated
        /// </summary>
        BfCfb,
        
        /// <summary>
        /// Stream cipher, deprecated
        /// </summary>
        Rc4Md5,
        
        /// <summary>
        /// Stream cipher, deprecated
        /// </summary>
        Salsa20,
        
        /// <summary>
        /// Not standard and not in active use
        /// </summary>
        DesCfb,
        
        /// <summary>
        /// Not standard and not in active use
        /// </summary>
        IdeaCfb,
        
        /// <summary>
        /// Not standard and not in active use
        /// </summary>
        Rc2Cfb,
        
        /// <summary>
        /// Not standard and not in active use
        /// </summary>
        Cast5Cfb,
        
        /// <summary>
        /// Not standard and not in active use
        /// </summary>
        Salsa20Ctr,
        
        /// <summary>
        /// Not standard and not in active use
        /// </summary>
        Rc4,
        
        /// <summary>
        /// Not standard and not in active use
        /// </summary>
        SeedCfb,
        
        /// <summary>
        /// Not standard and not in active use
        /// </summary>
        Table,
    }
}