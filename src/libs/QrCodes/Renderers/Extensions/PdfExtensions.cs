using System.Globalization;
using QrCodes.Renderers.Abstractions;

namespace QrCodes.Renderers.Extensions;

/// <summary>
/// 
/// </summary>
public static class PdfExtensions
{
    private static readonly byte[] PdfBinaryComment = [0x25, 0xe2, 0xe3, 0xcf, 0xd3];
    
    /// <summary>
    /// Creates a PDF document with given colors DPI and quality
    /// </summary>
    /// <param name="renderer"></param>
    /// <param name="data"></param>
    /// <param name="fileFormat"></param>
    /// <param name="quality"></param>
    /// <param name="dpi"></param>
    /// <param name="settings"></param>
    /// <returns></returns>
    public static byte[] RenderToPdf(
        this IRenderer renderer,
        QrCode data,
        int dpi = 150,
        FileFormat fileFormat = FileFormat.Jpeg,
        int quality = 85,
        RendererSettings? settings = null)
    {
        renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
        data = data ?? throw new ArgumentNullException(nameof(data));
        settings ??= new RendererSettings();
        settings.FileFormat = fileFormat;
        settings.Quality = quality;
        
        var bytes = renderer.RenderToBytes(data, settings);
        
        var imageWidthAndHeight = data.ModuleMatrix.Count * settings.PixelsPerModule;
        
        //var imageWidthAndHeight = data.ModuleMatrix.Count * pixelsPerModule;
        var pdfMediaSize = (imageWidthAndHeight * 72 / dpi)
            .ToString(CultureInfo.InvariantCulture);
            
        //Create PDF document
        using var stream = new MemoryStream();
        using var writer = new StreamWriter(stream, System.Text.Encoding.GetEncoding("ASCII"));
        var xrefs = new List<long>();

        writer.Write("%PDF-1.5\r\n");
        writer.Flush();

        stream.Write(PdfBinaryComment, 0, PdfBinaryComment.Length);
        writer.WriteLine();

        writer.Flush();
        xrefs.Add(stream.Position);

        writer.Write(
            xrefs.Count + " 0 obj\r\n" +
            "<<\r\n" +
            "/Type /Catalog\r\n" +
            "/Pages 2 0 R\r\n" +
            ">>\r\n" +
            "endobj\r\n"
        );

        writer.Flush();
        xrefs.Add(stream.Position);

        writer.Write(
            xrefs.Count + " 0 obj\r\n" +
            "<<\r\n" +
            "/Count 1\r\n" +
            "/Kids [ <<\r\n" +
            "/Type /Page\r\n" +
            "/Parent 2 0 R\r\n" +
            "/MediaBox [0 0 " + pdfMediaSize + " " + pdfMediaSize + "]\r\n" +
            "/Resources << /ProcSet [ /PDF /ImageC ]\r\n" +
            "/XObject << /Im1 4 0 R >> >>\r\n" +
            "/Contents 3 0 R\r\n" +
            ">> ]\r\n" +
            ">>\r\n" +
            "endobj\r\n"
        );

        var x = "q\r\n" +
                pdfMediaSize + " 0 0 " + pdfMediaSize + " 0 0 cm\r\n" +
                "/Im1 Do\r\n" +
                "Q";

        writer.Flush();
        xrefs.Add(stream.Position);

        writer.Write(
            xrefs.Count + " 0 obj\r\n" +
            "<< /Length " + x.Length + " >>\r\n" +
            "stream\r\n" +
            x + "endstream\r\n" +
            "endobj\r\n"
        );

        writer.Flush();
        xrefs.Add(stream.Position);

        writer.Write(
            xrefs.Count + " 0 obj\r\n" +
            "<<\r\n" +
            "/Name /Im1\r\n" +
            "/Type /XObject\r\n" +
            "/Subtype /Image\r\n" +
            "/Width " + imageWidthAndHeight + "/Height " + imageWidthAndHeight + "/Length 5 0 R\r\n" +
            "/Filter /DCTDecode\r\n" +
            "/ColorSpace /DeviceRGB\r\n" +
            "/BitsPerComponent 8\r\n" +
            ">>\r\n" +
            "stream\r\n"
        );
        writer.Flush();
        stream.Write(bytes, 0, bytes.Length);
        writer.Write(
            "\r\n" +
            "endstream\r\n" +
            "endobj\r\n"
        );

        writer.Flush();
        xrefs.Add(stream.Position);

        writer.Write(
            xrefs.Count + " 0 obj\r\n" +
            bytes.Length + " endobj\r\n"
        );

        writer.Flush();
        var startxref = stream.Position;

        writer.Write(
            "xref\r\n" +
            "0 " + (xrefs.Count + 1) + "\r\n" +
            "0000000000 65535 f\r\n"
        );

        foreach (var refValue in xrefs)
            writer.Write(refValue.ToString("0000000000", CultureInfo.InvariantCulture) + " 00000 n\r\n");

        writer.Write(
            "trailer\r\n" +
            "<<\r\n" +
            "/Size " + (xrefs.Count + 1) + "\r\n" +
            "/Root 1 0 R\r\n" +
            ">>\r\n" +
            "startxref\r\n" +
            startxref + "\r\n" +
            "%%EOF"
        );

        writer.Flush();
        
        return stream.ToArray();
    }
}