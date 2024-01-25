using System.Collections;
using System.Drawing;
using System.Text;

namespace QrCodes.Renderers;

/// <summary>
/// 
/// </summary>
public static class SvgRenderer
{
    /// <summary>
    /// Returns a QR code as SVG string with custom colors (in HEX syntax), optional quietzone and logo
    /// </summary>
    /// <param name="data"></param>
    /// <param name="pixelsPerModule">The pixel size each b/w module is drawn</param>
    /// <param name="darkColorHex">The color of the dark/black modules in hex (e.g. #000000) representation</param>
    /// <param name="lightColorHex">The color of the light/white modules in hex (e.g. #ffffff) representation</param>
    /// <param name="drawQuietZones">If true a white border is drawn around the whole QR Code</param>
    /// <param name="sizingMode">Defines if width/height or viewbox should be used for size definition</param>
    /// <param name="logo">A (optional) logo to be rendered on the code (either Bitmap or SVG)</param>
    /// <returns>SVG as string</returns>
    public static string Render(
        QrCode data,
        int pixelsPerModule = 5,
        string darkColorHex = "#000000",
        string lightColorHex = "#ffffff",
        bool drawQuietZones = true,
        SizingMode sizingMode = SizingMode.WidthHeightAttribute,
        SvgLogo? logo = null)
    {
        data = data ?? throw new ArgumentNullException(nameof(data));
        
        var offset = drawQuietZones ? 0 : 4;
        var edgeSize = data.ModuleMatrix.Count * pixelsPerModule - (offset * 2 * pixelsPerModule);
        var viewBox = new Size(edgeSize, edgeSize);
        
        return Render(
            data,
            viewBox,
            darkColorHex,
            lightColorHex,
            drawQuietZones,
            sizingMode,
            logo);
    }

    /// <summary>
    /// Returns a QR code as SVG string with custom colors (in HEX syntax), optional quietzone and logo
    /// </summary>
    /// <param name="data"></param>
    /// <param name="viewBox">The viewbox of the QR code graphic</param>
    /// <param name="darkColorHex">The color of the dark/black modules in hex (e.g. #000000) representation</param>
    /// <param name="lightColorHex">The color of the light/white modules in hex (e.g. #ffffff) representation</param>
    /// <param name="drawQuietZones">If true a white border is drawn around the whole QR Code</param>
    /// <param name="sizingMode">Defines if width/height or viewbox should be used for size definition</param>
    /// <param name="logo">A (optional) logo to be rendered on the code (either Bitmap or SVG)</param>
    /// <returns>SVG as string</returns>
    public static string Render(
        QrCode data,
        Size viewBox,
        string darkColorHex = "#000000",
        string lightColorHex = "#ffffff",
        bool drawQuietZones = true,
        SizingMode sizingMode = SizingMode.WidthHeightAttribute,
        SvgLogo? logo = null)
    {
        data = data ?? throw new ArgumentNullException(nameof(data));

        int offset = drawQuietZones ? 0 : 4;
        int drawableModulesCount = data.ModuleMatrix.Count - (drawQuietZones ? 0 : offset * 2);
        double pixelsPerModule = Math.Min(viewBox.Width, viewBox.Height) / (double)drawableModulesCount;
        double qrSize = drawableModulesCount * pixelsPerModule;
        string svgSizeAttributes = (sizingMode == SizingMode.WidthHeightAttribute) ? $@"width=""{viewBox.Width}"" height=""{viewBox.Height}""" : $@"viewBox=""0 0 {viewBox.Width} {viewBox.Height}""";
        ImageAttributes? logoAttr = null;
        if (logo != null)
            logoAttr = GetLogoAttributes(logo, viewBox);

        // Merge horizontal rectangles
        int[,] matrix = new int[drawableModulesCount, drawableModulesCount];
        for (int yi = 0; yi < drawableModulesCount; yi += 1)
        {
            BitArray bitArray = data.ModuleMatrix[yi+offset];

            int x0 = -1;
            int xL = 0;
            for (int xi = 0; xi < drawableModulesCount; xi += 1)
            {
                matrix[yi, xi] = 0;
                if (bitArray[xi+offset] && (logo == null || !logo.FillLogoBackground() || !IsBlockedByLogo((xi+offset)*pixelsPerModule, (yi+offset) * pixelsPerModule, logoAttr, pixelsPerModule)))
                {
                    if(x0 == -1)
                    {
                        x0 = xi;
                    }
                    xL += 1;
                }
                else
                {
                    if(xL > 0)
                    {
                        matrix[yi, x0] = xL;
                        x0 = -1;
                        xL = 0;
                    }
                }
            }

            if (xL > 0)
            {
                matrix[yi, x0] = xL;
            }
        }

        StringBuilder svgFile = new StringBuilder($@"<svg version=""1.1"" baseProfile=""full"" shape-rendering=""crispEdges"" {svgSizeAttributes} xmlns=""http://www.w3.org/2000/svg"" xmlns:xlink=""http://www.w3.org/1999/xlink"">");
        svgFile.AppendLine($@"<rect x=""0"" y=""0"" width=""{CleanSvgVal(qrSize)}"" height=""{CleanSvgVal(qrSize)}"" fill=""{lightColorHex}"" />");
        for (int yi = 0; yi < drawableModulesCount; yi += 1)
        {
            double y = yi * pixelsPerModule;
            for (int xi = 0; xi < drawableModulesCount; xi += 1)
            {
                int xL = matrix[yi, xi];
                if(xL > 0)
                {
                    // Merge vertical rectangles
                    int yL = 1;
                    for (int y2 = yi + 1; y2 < drawableModulesCount; y2 += 1)
                    {
                        if(matrix[y2, xi] == xL)
                        {
                            matrix[y2, xi] = 0;
                            yL += 1;
                        }
                        else
                        {
                            break;
                        }
                    }

                    // Output SVG rectangles
                    double x = xi * pixelsPerModule;
                    if (logo == null || !logo.FillLogoBackground() || !IsBlockedByLogo(x, y, logoAttr, pixelsPerModule))
                        svgFile.AppendLine($@"<rect x=""{CleanSvgVal(x)}"" y=""{CleanSvgVal(y)}"" width=""{CleanSvgVal(xL * pixelsPerModule)}"" height=""{CleanSvgVal(yL * pixelsPerModule)}"" fill=""{darkColorHex}"" />");                       
                }
            }
        }

        //Render logo, if set
        if (logo != null && logoAttr != null)
        {                   
            if (!logo.IsEmbedded())
            {
                svgFile.AppendLine($@"<svg width=""100%"" height=""100%"" version=""1.1"" xmlns = ""http://www.w3.org/2000/svg"">");
                svgFile.AppendLine($@"<image x=""{CleanSvgVal(logoAttr.Value.X)}"" y=""{CleanSvgVal(logoAttr.Value.Y)}"" width=""{CleanSvgVal(logoAttr.Value.Width)}"" height=""{CleanSvgVal(logoAttr.Value.Height)}"" xlink:href=""{logo.DataUri}"" />");
                svgFile.AppendLine(@"</svg>");
            }
            else
            {
                var rawLogo = (string)logo.RawLogo;                 
                var svg = System.Xml.Linq.XDocument.Parse(rawLogo);
                if (svg.Root != null)
                {
                    svg.Root.SetAttributeValue("x", CleanSvgVal(logoAttr.Value.X));
                    svg.Root.SetAttributeValue("y", CleanSvgVal(logoAttr.Value.Y));
                    svg.Root.SetAttributeValue("width", CleanSvgVal(logoAttr.Value.Width));
                    svg.Root.SetAttributeValue("height", CleanSvgVal(logoAttr.Value.Height));
                    svg.Root.SetAttributeValue("shape-rendering", "geometricPrecision");
                }
                svgFile.AppendLine(svg.ToString(System.Xml.Linq.SaveOptions.DisableFormatting).Replace("svg:", ""));                    
            }
        }

        svgFile.Append(@"</svg>");
        return svgFile.ToString();
    }

    private static bool IsBlockedByLogo(double x, double y, ImageAttributes? attr, double pixelPerModule)
    {
        return
            attr != null &&
            x + pixelPerModule >= attr.Value.X &&
            x <= attr.Value.X + attr.Value.Width &&
            y + pixelPerModule >= attr.Value.Y &&
            y <= attr.Value.Y + attr.Value.Height;
    }

    private static ImageAttributes GetLogoAttributes(SvgLogo logo, Size viewBox)
    {
        var imgWidth = logo.IconSizePercent / 100d * viewBox.Width;
        var imgHeight = logo.IconSizePercent / 100d * viewBox.Height;
        var imgPosX = viewBox.Width / 2d - imgWidth / 2d;
        var imgPosY = viewBox.Height / 2d - imgHeight / 2d;
        return new ImageAttributes()
        {
            Width = imgWidth,
            Height = imgHeight,
            X = imgPosX,
            Y = imgPosY
        };
    }

    private struct ImageAttributes
    {
        public double Width;
        public double Height;
        public double X;
        public double Y;
    }

    private static string CleanSvgVal(double input)
    {
        //Clean double values for international use/formats
        //We use explicitly "G15" to avoid differences between .NET full and Core platforms
        //https://stackoverflow.com/questions/64898117/tostring-has-a-different-behavior-between-net-462-and-net-core-3-1
        return input.ToString("G15", System.Globalization.CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Mode of sizing attribution on svg root node
    /// </summary>
    public enum SizingMode
    {
        /// <summary>
        /// 
        /// </summary>
        WidthHeightAttribute,
        
        /// <summary>
        /// 
        /// </summary>
        ViewBoxAttribute
    }

    /// <summary>
    /// Represents a logo graphic that can be rendered on a SvgQRCode
    /// </summary>
    public class SvgLogo
    {
        private string _logoData;
        
        /// <summary>
        /// Returns the media type of the logo
        /// </summary>
        /// <returns></returns>
        public MediaType Type { get; }
        
        /// <summary>
        /// Returns how much of the QR code should be covered by the logo (in percent)
        /// </summary>
        /// <returns></returns>
        public int IconSizePercent { get; }
        
        private bool _fillLogoBackground;
        
        /// <summary>
        /// Returns the raw logo's data
        /// </summary>
        /// <returns></returns>
        public object RawLogo { get; }
        
        private bool _isEmbedded;

        /// <summary>
        /// Create a logo object to be used in SvgQRCode renderer
        /// </summary>
        /// <param name="iconRasterizedPngBytes">Logo to be rendered as Bitmap/rasterized graphic</param>
        /// <param name="iconSizePercent">Degree of percentage coverage of the QR code by the logo</param>
        /// <param name="fillLogoBackground">If true, the background behind the logo will be cleaned</param>
        public SvgLogo(byte[] iconRasterizedPngBytes, int iconSizePercent = 15, bool fillLogoBackground = true)
        {
            IconSizePercent = iconSizePercent;
            _logoData = Convert.ToBase64String(iconRasterizedPngBytes, Base64FormattingOptions.None);
            Type = MediaType.Png;
            _fillLogoBackground = fillLogoBackground;
            RawLogo = iconRasterizedPngBytes;
            _isEmbedded = false;
        }

        /// <summary>
        /// Create a logo object to be used in SvgQRCode renderer
        /// </summary>
        /// <param name="iconVectorized">Logo to be rendered as SVG/vectorized graphic/string</param>
        /// <param name="iconSizePercent">Degree of percentage coverage of the QR code by the logo</param>
        /// <param name="fillLogoBackground">If true, the background behind the logo will be cleaned</param>
        /// <param name="iconEmbedded">If true, the logo will embedded as native svg instead of embedding it as image-tag</param>
        public SvgLogo(string iconVectorized, int iconSizePercent = 15, bool fillLogoBackground = true, bool iconEmbedded = true)
        {
            IconSizePercent = iconSizePercent;
            _logoData = Convert.ToBase64String(Encoding.UTF8.GetBytes(iconVectorized), Base64FormattingOptions.None);
            Type = MediaType.Svg;
            _fillLogoBackground = fillLogoBackground;
            RawLogo = iconVectorized;
            _isEmbedded = iconEmbedded;
        }

        /// <summary>
        /// Defines, if the logo shall be natively embedded.
        /// true=native svg embedding, false=embedding via image-tag
        /// </summary>
        /// <returns></returns>
        public bool IsEmbedded()
        {
            return _isEmbedded;
        }

        /// <summary>
        /// Returns the logo as data-uri
        /// </summary>
        /// <returns></returns>
        public string DataUri => $"data:{Type switch
        {
            MediaType.Png => "image/png",
            MediaType.Svg => "image/svg+xml",
            _ => string.Empty,
        }};base64,{_logoData}";

        /// <summary>
        /// Returns if the background of the logo should be cleaned (no QR modules will be rendered behind the logo)
        /// </summary>
        /// <returns></returns>
        public bool FillLogoBackground()
        {
            return _fillLogoBackground;
        }

        /// <summary>
        /// Media types for SvgLogos
        /// </summary>
        public enum MediaType
        {
            /// <summary>
            /// 
            /// </summary>
            Png = 0,
            
            /// <summary>
            /// 
            /// </summary>
            Svg = 1,
        }

    }
}