namespace QrCodes.Exceptions;

/// <summary>
/// 
/// </summary>
[Serializable]
public class DataTooLongException : Exception
{
    /// <summary>
    /// 
    /// </summary>
    public DataTooLongException()
    {
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="message"></param>
    public DataTooLongException(
        string message)
        : base(message)
    {
    }

    /// <summary>
    /// Represents an exception that is thrown when data exceeds a defined length.
    /// </summary>
    /// <remarks>
    /// This exception is typically thrown when a value exceeds the maximum length allowed.
    /// </remarks>
    /// <seealso cref="System.Exception" />
    public DataTooLongException(
        string message,
        Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eccLevel"></param>
    /// <param name="encodingMode"></param>
    /// <param name="maxSizeByte"></param>
    public DataTooLongException(
        string eccLevel,
        string encodingMode,
        int maxSizeByte) : base(
        $"The given payload exceeds the maximum size of the QR code standard. " +
        $"The maximum size allowed for the chosen parameters " +
        $"(ECC level={eccLevel}, EncodingMode={encodingMode}) is {maxSizeByte} byte."
    )
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eccLevel"></param>
    /// <param name="encodingMode"></param>
    /// <param name="version"></param>
    /// <param name="maxSizeByte"></param>
    public DataTooLongException(
        string eccLevel,
        string encodingMode,
        int version,
        int maxSizeByte) : base(
        $"The given payload exceeds the maximum size of the QR code standard. " +
        $"The maximum size allowed for the chosen parameters " +
        $"(ECC level={eccLevel}, EncodingMode={encodingMode}, FixedVersion={version}) is {maxSizeByte} byte."
    )
    {
    }
}