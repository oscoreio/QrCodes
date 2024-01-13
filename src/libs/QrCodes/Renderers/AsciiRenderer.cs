using System.Text;

namespace QrCodes.Renderers;

/// <summary>
/// 
/// </summary>
public static class AsciiRenderer
{
    /// <summary>
    /// Returns a strings that contains the resulting QR code as ASCII chars.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="repeatPerModule">Number of repeated darkColorString/whiteSpaceString per module.</param>
    /// <param name="darkColorString">String for use as dark color modules. In case of string make sure whiteSpaceString has the same length.</param>
    /// <param name="whiteSpaceString">String for use as white modules (whitespace). In case of string make sure darkColorString has the same length.</param>
    /// <param name="drawQuietZones"></param>
    /// <param name="endOfLine">End of line separator. (Default: \n)</param>
    /// <returns></returns>
    public static string Render(
        QrCode data,
        int repeatPerModule,
        string darkColorString = "██",
        string whiteSpaceString = "  ",
        bool drawQuietZones = true,
        string endOfLine = "\n")
    {
        return string.Join(
            endOfLine,
            GetLineByLineGraphic(
                data: data,
                repeatPerModule,
                darkColorString,
                whiteSpaceString,
                drawQuietZones));
    }

    /// <summary>
    /// Returns an array of strings that contains each line of the resulting QR code as ASCII chars.
    /// </summary>
    /// <param name="data"></param>
    /// <param name="repeatPerModule">Number of repeated darkColorString/whiteSpaceString per module.</param>
    /// <param name="darkColorString">String for use as dark color modules. In case of string make sure whiteSpaceString has the same length.</param>
    /// <param name="whiteSpaceString">String for use as white modules (whitespace). In case of string make sure darkColorString has the same length.</param>
    /// <param name="drawQuietZones"></param>
    /// <returns></returns>
    public static string[] GetLineByLineGraphic(
        QrCode data,
        int repeatPerModule,
        string darkColorString = "██",
        string whiteSpaceString = "  ",
        bool drawQuietZones = true)
    {
        data = data ?? throw new ArgumentNullException(nameof(data));
        darkColorString = darkColorString ?? throw new ArgumentNullException(nameof(darkColorString));
        
        var qrCode = new List<string>();
        //We need to adjust the repeatPerModule based on number of characters in darkColorString
        //(we assume whiteSpaceString has the same number of characters)
        //to keep the QR code as square as possible.
        var quietZonesModifier = (drawQuietZones ? 0 : 8);
        var quietZonesOffset = (int)(quietZonesModifier * 0.5);
        var adjustmentValueForNumberOfCharacters = darkColorString.Length / 2 != 1 ? darkColorString.Length / 2 : 0;
        var verticalNumberOfRepeats = repeatPerModule + adjustmentValueForNumberOfCharacters;
        var sideLength = (data.ModuleMatrix.Count - quietZonesModifier) * verticalNumberOfRepeats;
        for (var y = 0; y < sideLength; y++)
        {
            var lineBuilder = new StringBuilder();
            for (var x = 0; x < data.ModuleMatrix.Count - quietZonesModifier; x++)
            {
                var module = data.ModuleMatrix[x + quietZonesOffset][((y + verticalNumberOfRepeats) / verticalNumberOfRepeats - 1)+quietZonesOffset];
                for (var i = 0; i < repeatPerModule; i++)
                {
                    lineBuilder.Append(module ? darkColorString : whiteSpaceString);
                }
            }
            qrCode.Add(lineBuilder.ToString());
        }
        return qrCode.ToArray();
    }
}