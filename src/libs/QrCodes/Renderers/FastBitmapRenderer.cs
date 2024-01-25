using QrCodes.Renderers.Abstractions;

namespace QrCodes.Renderers;

/// <summary>
/// 
/// </summary>
// ReSharper disable once InconsistentNaming
public class FastBitmapRenderer : IRenderer
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static byte[] Render(
        QrCode data,
        RendererSettings? settings = null)
    {
        data = data ?? throw new ArgumentNullException(nameof(data));
        settings ??= new RendererSettings();
        
        var sideLength = data.ModuleMatrix.Count * settings.PixelsPerModule;

        var moduleDark = new []{ settings.DarkColor.B, settings.DarkColor.G, settings.DarkColor.R };
        var moduleLight = new []{ settings.LightColor.B, settings.LightColor.G, settings.LightColor.R };

        List<byte> bmp = [];

        //header
        bmp.AddRange(new byte[] { 0x42, 0x4D, 0x4C, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x1A, 0x00, 0x00, 0x00, 0x0C, 0x00, 0x00, 0x00 });

        //width
        bmp.AddRange(IntTo4Byte(sideLength));
        //height
        bmp.AddRange(IntTo4Byte(sideLength));

        //header end
        bmp.AddRange(new byte[] { 0x01, 0x00, 0x18, 0x00 });

        //draw qr code
        for (var x = sideLength-1; x >= 0; x = x - settings.PixelsPerModule)
        {
            for (int pm = 0; pm < settings.PixelsPerModule; pm++)
            {
                for (var y = 0; y < sideLength; y = y + settings.PixelsPerModule)
                {
                    var module =
                        data.ModuleMatrix[(x + settings.PixelsPerModule)/settings.PixelsPerModule - 1][(y + settings.PixelsPerModule)/settings.PixelsPerModule - 1];
                    for (int i = 0; i < settings.PixelsPerModule; i++)
                    {
                        bmp.AddRange(module ? moduleDark : moduleLight);
                    }
                }
                if (sideLength%4 != 0)
                {
                    for (int i = 0; i < sideLength%4; i++)
                    {
                        bmp.Add(0x00);
                    }
                }
            }
        }

        //finalize with terminator
        bmp.AddRange(new byte[] { 0x00, 0x00 });

        return bmp.ToArray();
    }

    private static byte[] IntTo4Byte(int inp)
    {
        byte[] bytes = new byte[2];
        unchecked
        {
            bytes[1] = (byte)(inp >> 8);
            bytes[0] = (byte)(inp);
        }
        return bytes;
    }

    /// <inheritdoc />
    public byte[] RenderToBytes(
        QrCode data,
        RendererSettings? settings = null)
    {
        return Render(data, settings);
    }

    /// <inheritdoc />
    public Stream RenderToStream(
        QrCode data,
        RendererSettings? settings = null)
    {
        return new MemoryStream(RenderToBytes(data, settings));
    }
}