using Xunit;
using QrCodes.Renderers;
using QrCodes.Tests.Helpers;

namespace QrCodes.Tests;

/****************************************************************************************************
 * Note: Test cases compare the outcome visually even if it's slower than a byte-wise compare.
 *       This is necessary, because the Deflate implementation differs on the different target
 *       platforms and thus the outcome, even if visually identical, differs. Thus only a visual
 *       test method makes sense. In addition bytewise differences shouldn't be important, if the
 *       visual outcome is identical and thus the qr code is identical/scannable.
 ****************************************************************************************************/
public class FastPngRendererTests
{
    const string QRCodeContent = "This is a quick test! 123#?";
    const string VisualTestPath = null;

    [Fact]
    public void can_render_pngbyte_qrcode_blackwhite()
    {
        var data = QrCodeGenerator.Generate(QRCodeContent, ErrorCorrectionLevel.Low);
        var pngCodeGfx = FastPngRenderer.Render(data, 5);
        
        HelperFunctions.TestByHash(pngCodeGfx, "90869fd365fe75e8aef3da40765dd5cc");
        HelperFunctions.TestByDecode(pngCodeGfx, QRCodeContent);
        HelperFunctions.TestImageToFile(VisualTestPath, nameof(can_render_pngbyte_qrcode_blackwhite), pngCodeGfx);
    }

    [Fact]
    public void can_render_pngbyte_qrcode_color()
    {
        var data = QrCodeGenerator.Generate(QRCodeContent, ErrorCorrectionLevel.Low);
        var pngCodeGfx = FastPngRenderer.Render(data, 5, [255, 0, 0], [0, 0, 255]);

        HelperFunctions.TestByHash(pngCodeGfx, "55093e9b9e39dc8368721cb535844425");
        // HelperFunctions.TestByDecode(pngCodeGfx, QRCodeContent); => Not decodable
        HelperFunctions.TestImageToFile(VisualTestPath, nameof(can_render_pngbyte_qrcode_color), pngCodeGfx);
    }


    [Fact]
    public void can_render_pngbyte_qrcode_color_with_alpha()
    {
        var data = QrCodeGenerator.Generate(QRCodeContent, ErrorCorrectionLevel.Low);
        var pngCodeGfx = FastPngRenderer.Render(data, 5, [255, 255, 255, 127], [0, 0, 255]);

        HelperFunctions.TestByHash(pngCodeGfx, "afc7674cb4849860cbf73684970e5332");
        // HelperFunctions.TestByDecode(pngCodeGfx, QRCodeContent); => Not decodable
        HelperFunctions.TestImageToFile(VisualTestPath, nameof(can_render_pngbyte_qrcode_color_with_alpha), pngCodeGfx);
    }

    [Fact]
    public void can_render_pngbyte_qrcode_color_without_quietzones()
    {
        var data = QrCodeGenerator.Generate(QRCodeContent, ErrorCorrectionLevel.Low);
        var pngCodeGfx = FastPngRenderer.Render(data, 5, [255, 255, 255, 127], [0, 0, 255], false);

        HelperFunctions.TestByHash(pngCodeGfx, "af60811deaa524e0d165baecdf40ab72");
        // HelperFunctions.TestByDecode(pngCodeGfx, QRCodeContent); => not decodable
        HelperFunctions.TestImageToFile(VisualTestPath, nameof(can_render_pngbyte_qrcode_color_without_quietzones), pngCodeGfx);
    }

    [Fact]
    public void can_render_pngbyte_qrcode_from_helper()
    {
        var data = QrCodeGenerator.Generate(QRCodeContent, ErrorCorrectionLevel.Low);
        var pngCodeGfx = FastPngRenderer.Render(data, 10);

        //Create QR code                   
        HelperFunctions.TestByHash(pngCodeGfx, "e649d6a485873ac18b5aab791f325284");
        HelperFunctions.TestByDecode(pngCodeGfx, QRCodeContent);
        HelperFunctions.TestImageToFile(VisualTestPath, nameof(can_render_pngbyte_qrcode_from_helper), pngCodeGfx);
    }

    [Fact]
    public void can_render_pngbyte_qrcode_from_helper_2()
    {
        var data = QrCodeGenerator.Generate("This is a quick test! 123#?", ErrorCorrectionLevel.Low);
        var pngCodeGfx = FastPngRenderer.Render(data, 5, [255, 255, 255, 127], [0, 0, 255]);

        HelperFunctions.TestByHash(pngCodeGfx, "afc7674cb4849860cbf73684970e5332");
        // HelperFunctions.TestByDecode(pngCodeGfx, QRCodeContent); => not decodable
        HelperFunctions.TestImageToFile(VisualTestPath, nameof(can_render_pngbyte_qrcode_from_helper_2), pngCodeGfx);
    }
}