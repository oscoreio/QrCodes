using System.Globalization;

namespace QrCodes.Renderers;

/// <summary>
/// 
/// </summary>
public static class PostscriptRenderer
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="pointsPerModule"></param>
    /// <param name="epsFormat"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static string Render(
        QrCode data,
        int pointsPerModule,
        bool epsFormat = false)
    {
        data = data ?? throw new ArgumentNullException(nameof(data));
        
        var viewBox = new Size(
            pointsPerModule * data.ModuleMatrix.Count,
            pointsPerModule * data.ModuleMatrix.Count);
        
        return Render(
            data,
            viewBox,
            "#000000",
            "#ffffff",
            true,
            epsFormat);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="pointsPerModule"></param>
    /// <param name="darkColorHex"></param>
    /// <param name="lightColorHex"></param>
    /// <param name="drawQuietZones"></param>
    /// <param name="epsFormat"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static string Render(
        QrCode data,
        int pointsPerModule,
        string darkColorHex,
        string lightColorHex,
        bool drawQuietZones = true,
        bool epsFormat = false)
    {
        data = data ?? throw new ArgumentNullException(nameof(data));
        
        var viewBox = new Size(
            pointsPerModule * data.ModuleMatrix.Count,
            pointsPerModule * data.ModuleMatrix.Count);
        
        return Render(
            data,
            viewBox,
            darkColorHex,
            lightColorHex,
            drawQuietZones,
            epsFormat);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="viewBox"></param>
    /// <param name="drawQuietZones"></param>
    /// <param name="epsFormat"></param>
    /// <returns></returns>
    public static string Render(
        QrCode data,
        Size viewBox,
        bool drawQuietZones = true,
        bool epsFormat = false)
    {
        return Render(
            data,
            viewBox,
            "#000000",
            "#ffffff",
            drawQuietZones,
            epsFormat);
    }

    private static int GetHexRed(string hex)
    {
        return int.Parse(
            hex.Substring(1, 2), 
            NumberStyles.HexNumber);
    }
    
    private static int GetHexGreen(string hex)
    {
        return int.Parse(
            hex.Substring(3, 2), 
            NumberStyles.HexNumber);
    }
    
    private static int GetHexBlue(string hex)
    {
        return int.Parse(
            hex.Substring(5, 2), 
            NumberStyles.HexNumber);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="viewBox"></param>
    /// <param name="darkColorHex"></param>
    /// <param name="lightColorHex"></param>
    /// <param name="drawQuietZones"></param>
    /// <param name="epsFormat"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public static string Render(
        QrCode data,
        Size viewBox,
        string darkColorHex,
        string lightColorHex,
        bool drawQuietZones = true,
        bool epsFormat = false)
    {
        data = data ?? throw new ArgumentNullException(nameof(data));
        darkColorHex = darkColorHex ?? throw new ArgumentNullException(nameof(darkColorHex));
        lightColorHex = lightColorHex ?? throw new ArgumentNullException(nameof(lightColorHex));

        var offset = drawQuietZones ? 0 : 4;
        var drawableModulesCount = data.ModuleMatrix.Count - (drawQuietZones ? 0 : offset * 2);
        var pointsPerModule = Math.Min(viewBox.Width, viewBox.Height) / (double)drawableModulesCount;

        string psFile = string.Format(
            CultureInfo.InvariantCulture,
            @"%!PS-Adobe-3.0 {3}
%%Creator: QRCoder.NET
%%Title: QRCode
%%CreationDate: {0}
%%DocumentData: Clean7Bit
%%Origin: 0
%%DocumentMedia: Default {1} {1} 0 () ()
%%BoundingBox: 0 0 {1} {1}
%%LanguageLevel: 2 
%%Pages: 1
%%Page: 1 1
%%EndComments
%%BeginConstants
/sz {1} def
/sc {2} def
%%EndConstants
%%BeginFeature: *PageSize Default
<< /PageSize [ sz sz ] /ImagingBBox null >> setpagedevice
%%EndFeature
",
            DateTime.Now.ToString("s"),
            CleanSvgVal(viewBox.Width),
            CleanSvgVal(pointsPerModule),
            epsFormat
                ? "EPSF-3.0"
                : string.Empty);
        psFile += string.Format(
            CultureInfo.InvariantCulture,
            @"%%BeginFunctions 
/csquare {{
    newpath
    0 0 moveto
    0 1 rlineto
    1 0 rlineto
    0 -1 rlineto
    closepath
    setrgbcolor
    fill
}} def
/f {{ 
    {0} {1} {2} csquare
    1 0 translate
}} def
/b {{ 
    1 0 translate
}} def 
/background {{ 
    {3} {4} {5} csquare 
}} def
/nl {{
    -{6} -1 translate
}} def
%%EndFunctions
%%BeginBody
0 0 moveto
gsave
sz sz scale
background
grestore
gsave
sc sc scale
0 {6} 1 sub translate
", 
            CleanSvgVal(GetHexRed(darkColorHex) / 255.0),
            CleanSvgVal(GetHexGreen(darkColorHex) / 255.0),
            CleanSvgVal(GetHexBlue(darkColorHex) / 255.0),
            CleanSvgVal(GetHexRed(lightColorHex) / 255.0),
            CleanSvgVal(GetHexGreen(lightColorHex) / 255.0),
            CleanSvgVal(GetHexBlue(lightColorHex) / 255.0),
            drawableModulesCount);

        for (int xi = offset; xi < offset + drawableModulesCount; xi++)
        {
            if (xi > offset)
                psFile += "nl\n";
            for (int yi = offset; yi < offset + drawableModulesCount; yi++)
            {
                psFile += (data.ModuleMatrix[xi][yi] ? "f " : "b ");
            }
            psFile += "\n";
        }
        
        return psFile + @"%%EndBody
grestore
showpage   
%%EOF
";
    }

    private static string CleanSvgVal(double input)
    {
        //Clean double values for international use/formats
        return input.ToString(CultureInfo.InvariantCulture);
    }
}