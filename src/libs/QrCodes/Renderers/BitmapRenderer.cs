using System.Globalization;

namespace QrCodes.Renderers;

/// <summary>
/// 
/// </summary>
// ReSharper disable once InconsistentNaming
public static class BitmapRenderer
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="pixelsPerModule"></param>
    /// <returns></returns>
    public static byte[] Render(
        QrCode data,
        int pixelsPerModule = 5)
    {
        return Render(
            data: data,
            pixelsPerModule: pixelsPerModule,
            darkColorRgb: [0x00, 0x00, 0x00],
            lightColorRgb: [0xFF, 0xFF, 0xFF]);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="pixelsPerModule"></param>
    /// <param name="darkColorHtmlHex"></param>
    /// <param name="lightColorHtmlHex"></param>
    /// <returns></returns>
    public static byte[] Render(
        QrCode data,
        int pixelsPerModule,
        string darkColorHtmlHex,
        string lightColorHtmlHex)
    {
        darkColorHtmlHex = darkColorHtmlHex ?? throw new ArgumentNullException(nameof(darkColorHtmlHex));
        lightColorHtmlHex = lightColorHtmlHex ?? throw new ArgumentNullException(nameof(lightColorHtmlHex));
        
        return Render(
            data: data,
            pixelsPerModule,
            darkColorRgb: HexColorToByteArray(darkColorHtmlHex),
            lightColorRgb: HexColorToByteArray(lightColorHtmlHex));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="pixelsPerModule"></param>
    /// <param name="darkColorRgb"></param>
    /// <param name="lightColorRgb"></param>
    /// <returns></returns>
    public static byte[] Render(
        QrCode data,
        int pixelsPerModule,
        byte[] darkColorRgb,
        byte[] lightColorRgb)
    {
        data = data ?? throw new ArgumentNullException(nameof(data));
        
        var sideLength = data.ModuleMatrix.Count * pixelsPerModule;

        var moduleDark = darkColorRgb.Reverse().ToArray();
        var moduleLight = lightColorRgb.Reverse().ToArray();

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
        for (var x = sideLength-1; x >= 0; x = x - pixelsPerModule)
        {
            for (int pm = 0; pm < pixelsPerModule; pm++)
            {
                for (var y = 0; y < sideLength; y = y + pixelsPerModule)
                {
                    var module =
                        data.ModuleMatrix[(x + pixelsPerModule)/pixelsPerModule - 1][(y + pixelsPerModule)/pixelsPerModule - 1];
                    for (int i = 0; i < pixelsPerModule; i++)
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

    /// <summary>
    /// Takes hexadecimal color string #000000 and returns byte[]{ 0, 0, 0 }
    /// </summary>
    /// <param name="colorString">Color in HEX format like #ffffff</param>
    /// <returns></returns>
    public static byte[] HexColorToByteArray(string colorString)
    {
        colorString = colorString ?? throw new ArgumentNullException(nameof(colorString));
        
        if (colorString.StartsWith("#", StringComparison.Ordinal))
        {
            colorString = colorString.Substring(1);
        }
        
        byte[] byteColor = new byte[colorString.Length / 2];
        for (int i = 0; i < byteColor.Length; i++)
            byteColor[i] = byte.Parse(colorString.Substring(i * 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
        return byteColor;
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
}