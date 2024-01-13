namespace QrCodes;

/// <summary>
/// Error correction level.
/// These define the tolerance levels for how much of the code
/// can be lost before the code cannot be recovered.
/// </summary>
public enum ErrorCorrectionLevel
{
    /// <summary>
    /// 7% may be lost before recovery is not possible
    /// </summary>
    Low,
    
    /// <summary>
    /// 15% may be lost before recovery is not possible
    /// </summary>
    Medium,
    
    /// <summary>
    /// 25% may be lost before recovery is not possible
    /// </summary>
    Quartile,
    
    /// <summary>
    /// 30% may be lost before recovery is not possible
    /// </summary>
    High,
}