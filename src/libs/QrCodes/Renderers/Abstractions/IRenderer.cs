namespace QrCodes.Renderers.Abstractions;

/// <summary>
/// Renders a QR code as bytes or stream.
/// </summary>
public interface IRenderer
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    byte[] RenderToBytes(
        QrCode data,
        RendererSettings? settings = null);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    Stream RenderToStream(
        QrCode data,
        RendererSettings? settings = null);
}