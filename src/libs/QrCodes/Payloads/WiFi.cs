using System.Text.RegularExpressions;

namespace QrCodes.Payloads;

/// <summary>
/// 
/// </summary>
public class WiFi
{
    private readonly string _ssid, _password, _authenticationMode;
    private readonly bool _isHiddenSsid;

    /// <summary>
    /// Generates a WiFi payload. Scanned by a QR Code scanner app, the device will connect to the WiFi.
    /// </summary>
    /// <param name="ssid">SSID of the WiFi network</param>
    /// <param name="password">Password of the WiFi network</param>
    /// <param name="authenticationMode">Authentication mode (WEP, WPA, WPA2)</param>
    /// <param name="isHiddenSsid">Set flag, if the WiFi network hides its SSID</param>
    /// <param name="escapeHexStrings">Set flag, if ssid/password is delivered as HEX string. Note: May not be supported on iOS devices.</param>
    public WiFi(
        string ssid,
        string password,
        Authentication authenticationMode,
        bool isHiddenSsid = false,
        bool escapeHexStrings = true)
    {
        ssid = ssid ?? throw new ArgumentNullException(nameof(ssid));
        password = password ?? throw new ArgumentNullException(nameof(password));
        
        _ssid = ssid.EscapeInput();
        _ssid = escapeHexStrings &&
                    IsHexStyle(_ssid) ? "\"" + _ssid + "\"" : _ssid;
        _password = password.EscapeInput();
        _password = escapeHexStrings && IsHexStyle(_password) ? "\"" + _password + "\"" : _password;
        _authenticationMode = authenticationMode.ToString();
        _isHiddenSsid = isHiddenSsid;
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return
            $"WIFI:T:{_authenticationMode};S:{_ssid};P:{_password};{(_isHiddenSsid ? "H:true" : string.Empty)};";
    }

    private static bool IsHexStyle(string input)
    {
        return Regex.IsMatch(input, @"\A\b[0-9a-fA-F]+\b\Z") ||
               Regex.IsMatch(input, @"\A\b(0[xX])?[0-9a-fA-F]+\b\Z");
    }
    
    // ReSharper disable InconsistentNaming
    
    /// <summary>
    /// 
    /// </summary>
    public enum Authentication
    {
        /// <summary>
        /// 
        /// </summary>
        WEP,
            
        /// <summary>
        /// 
        /// </summary>
        WPA,
            
        /// <summary>
        /// 
        /// </summary>
        // ReSharper disable once IdentifierTypo
        nopass,
    }
}