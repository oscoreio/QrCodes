using System.Text;

namespace QrCodes.Payloads;

/// <summary>
/// https://github.com/google/google-authenticator/wiki/Key-Uri-Format
/// </summary>
public class OneTimePassword
{
    /// <summary>
    /// 
    /// </summary>
    public OneTimePasswordAuthType Type { get; set; } = OneTimePasswordAuthType.TimeBased;
    
    /// <summary>
    /// 
    /// </summary>
    public string? Secret { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public OneTimePasswordAuthAlgorithm AuthAlgorithm { get; set; } = OneTimePasswordAuthAlgorithm.Sha1;

    /// <summary>
    /// 
    /// </summary>
    public string? Issuer { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public string? Label { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public int Digits { get; set; } = 6;
    
    /// <summary>
    /// 
    /// </summary>
    public int? Counter { get; set; }
    
    /// <summary>
    /// 
    /// </summary>
    public int? Period { get; set; } = 30;

    /// <summary>
    /// 
    /// </summary>
    public enum OneTimePasswordAuthType
    {
        /// <summary>
        /// A common form of two-factor authentication (2FA).
        /// Unique numeric passwords are generated with a standardized algorithm
        /// that uses the current time as an input.
        /// </summary>
        TimeBased,
        
        /// <summary>
        /// An event-based OTP where the moving factor in each code is based on a counter.
        /// </summary>
        HashBased,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum OneTimePasswordAuthAlgorithm
    {
        /// <summary>
        /// 
        /// </summary>
        Sha1,
        
        /// <summary>
        /// 
        /// </summary>
        Sha256,
        
        /// <summary>
        /// 
        /// </summary>
        Sha512,
    }

    /// <inheritdoc />
    public override string ToString()
    {
        switch (Type)
        {
            case OneTimePasswordAuthType.TimeBased:
                return TimeToString();
            case OneTimePasswordAuthType.HashBased:
                return HmacToString();
            default:
                return string.Empty;
        }
    }

    /// <summary>
    /// Note: Issuer:Label must only contain 1 : if either of the Issuer or the Label has a : then it is invalid.
    /// Defaults are 6 digits and 30 for Period
    /// </summary>
    /// <returns></returns>
    private string HmacToString()
    {
        var sb = new StringBuilder("otpauth://hotp/");
        ProcessCommonFields(sb);
        var actualCounter = Counter ?? 1;
        sb.Append("&counter=" + actualCounter);
        return sb.ToString();
    }

    private string TimeToString()
    {
        if (Period == null)
        {
            throw new InvalidOperationException("Period must be set when using OneTimePasswordAuthType.TOTP");
        }

        var sb = new StringBuilder("otpauth://totp/");

        ProcessCommonFields(sb);

        if (Period != 30)
        {
            sb.Append("&period=" + Period);
        }

        return sb.ToString();
    }

    private void ProcessCommonFields(StringBuilder sb)
    {
        if (Secret == null ||
            string.IsNullOrWhiteSpace(Secret))
        {
            throw new InvalidOperationException("Secret must be a filled out base32 encoded string");
        }
        
        string strippedSecret = Secret.Replace(" ", "");
        string? escapedIssuer = null;
        string? label = null;

        if (Issuer != null &&
            !string.IsNullOrWhiteSpace(Issuer))
        {
            if (Issuer.Contains(":"))
            {
                throw new InvalidOperationException("Issuer must not have a ':'");
            }
            escapedIssuer = Uri.EscapeDataString(Issuer);
        }

        if (Label != null &&
            !string.IsNullOrWhiteSpace(Label) &&
            Label.Contains(":"))
        {
            throw new InvalidOperationException("Label must not have a ':'");
        }

        if (Label != null && Issuer != null)
        {
            label = Issuer + ":" + Label;                    
        }
        else if (Issuer != null)
        {
            label = Issuer;
        }

        if (label != null)
        {
            sb.Append(label);
        }

        sb.Append("?secret=" + strippedSecret);

        if (escapedIssuer != null)
        {
            sb.Append("&issuer=" + escapedIssuer);
        }

        if (Digits != 6)
        {
            sb.Append("&digits=" + Digits);
        }
    }
}