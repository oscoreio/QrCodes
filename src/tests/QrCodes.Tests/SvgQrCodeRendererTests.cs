using QrCodes.Renderers;
using Xunit;
using FluentAssertions;
using QrCodes.Tests.Helpers;
using SixLabors.ImageSharp;

namespace QrCodes.Tests;

public class SvgQrCodeRendererTests
{
    const string QRCodeContent = "This is a quick test! 123#?";
    const string VisualTestPath = null;

    [Fact]
    public void can_render_svg_qrcode_simple()
    {
        var data = QrCodeGenerator.Generate(QRCodeContent, ErrorCorrectionLevel.High);
        var svg = SvgRenderer.Render(data, 5);
        
        HelperFunctions.TestImageToFile(VisualTestPath, nameof(can_render_svg_qrcode_simple), svg);
        HelperFunctions.TestByHash(svg, "5c251275a435a9aed7e591eb9c2e9949");
    }

    [Fact]
    public void can_render_svg_qrcode()
    {
        var data = QrCodeGenerator.Generate(QRCodeContent, ErrorCorrectionLevel.High);
        var svg = SvgRenderer.Render(data, 10, Color.Red.ToHexWithPrefix(), Color.White.ToHexWithPrefix());

        HelperFunctions.TestImageToFile(VisualTestPath, nameof(can_render_svg_qrcode), svg);
        HelperFunctions.TestByHash(svg, "1baa8c6ac3bd8c1eabcd2c5422dd9f78");
    }

    [Fact]
    public void can_render_svg_qrcode_viewbox_mode()
    {
        var data = QrCodeGenerator.Generate(QRCodeContent, ErrorCorrectionLevel.High);
        var svg = SvgRenderer.Render(data, new System.Drawing.Size(128, 128));

        HelperFunctions.TestImageToFile(VisualTestPath, nameof(can_render_svg_qrcode_viewbox_mode), svg);
        HelperFunctions.TestByHash(svg, "56719c7db39937c74377855a5dc4af0a");
    }

    [Fact]
    public void can_render_svg_qrcode_viewbox_mode_viewboxattr()
    {
        var data = QrCodeGenerator.Generate(QRCodeContent, ErrorCorrectionLevel.High);
        var svg = SvgRenderer.Render(data, new System.Drawing.Size(128, 128), sizingMode: SvgRenderer.SizingMode.ViewBoxAttribute);

        HelperFunctions.TestImageToFile(VisualTestPath, nameof(can_render_svg_qrcode_viewbox_mode_viewboxattr), svg);
        HelperFunctions.TestByHash(svg, "788afdb693b0b71eed344e495c180b60");
    }

    [Fact]
    public void can_render_svg_qrcode_without_quietzones()
    {
        var data = QrCodeGenerator.Generate(QRCodeContent, ErrorCorrectionLevel.High);
        var svg = SvgRenderer.Render(data, 10, Color.Red.ToHexWithPrefix(), Color.White.ToHexWithPrefix(), false);

        HelperFunctions.TestImageToFile(VisualTestPath, nameof(can_render_svg_qrcode_without_quietzones), svg);
        HelperFunctions.TestByHash(svg, "2a582427d86b51504c08ebcbcf0472bd");
    }

    [Fact]
    public void can_render_svg_qrcode_without_quietzones_hex()
    {
        var data = QrCodeGenerator.Generate(QRCodeContent, ErrorCorrectionLevel.High);
        var svg = SvgRenderer.Render(data, 10, "#000000", "#ffffff", false);

        HelperFunctions.TestImageToFile(VisualTestPath, nameof(can_render_svg_qrcode_without_quietzones_hex), svg);
        HelperFunctions.TestByHash(svg, "4ab0417cc6127e347ca1b2322c49ed7d");
    }

    [Fact]
    public void can_render_svg_qrcode_with_png_logo()
    {
        var logoBitmap = Image.Load(H.Resources.noun_software_engineer_2909346_png.AsStream());
        var logoObj = new SvgRenderer.SvgLogo(iconRasterizedPngBytes: logoBitmap.ToBytes(FileFormat.Png), 15);
        logoObj.Type.Should().Be(SvgRenderer.SvgLogo.MediaType.Png);

        var data = QrCodeGenerator.Generate(QRCodeContent, ErrorCorrectionLevel.High);
        var svg = SvgRenderer.Render(data, 10, Color.DarkGray.ToHexWithPrefix(), Color.White.ToHexWithPrefix(), logo: logoObj);

        HelperFunctions.TestImageToFile(VisualTestPath, nameof(can_render_svg_qrcode_with_png_logo), svg);
        HelperFunctions.TestByHash(svg, "78e02e8ba415f15817d5ed88c4afca31");           
    }

    [Fact]
    public void can_render_svg_qrcode_with_svg_logo_embedded()
    {
        var logoSvg = H.Resources.noun_Scientist_2909361_svg.AsString();
        var logoObj = new SvgRenderer.SvgLogo(logoSvg, 20);
        logoObj.Type.Should().Be(SvgRenderer.SvgLogo.MediaType.Svg);

        var data = QrCodeGenerator.Generate(QRCodeContent, ErrorCorrectionLevel.High);
        var svg = SvgRenderer.Render(data, 10, Color.DarkGray.ToHexWithPrefix(), Color.White.ToHexWithPrefix(), logo: logoObj);

        HelperFunctions.TestImageToFile(VisualTestPath, nameof(can_render_svg_qrcode_with_svg_logo_embedded), svg);
        HelperFunctions.TestByHash(svg, "855eb988d3af035abd273ed1629aa952");
          
    }

    [Fact]
    public void can_render_svg_qrcode_with_svg_logo_image_tag()
    {
        var logoSvg = H.Resources.noun_Scientist_2909361_svg.AsString();
        var logoObj = new SvgRenderer.SvgLogo(logoSvg, 20, iconEmbedded: false);

        var data = QrCodeGenerator.Generate(QRCodeContent, ErrorCorrectionLevel.High);
        var svg = SvgRenderer.Render(data, 10, Color.DarkGray.ToHexWithPrefix(), Color.White.ToHexWithPrefix(), logo: logoObj);

        HelperFunctions.TestImageToFile(VisualTestPath, nameof(can_render_svg_qrcode_with_svg_logo_image_tag), svg);
        HelperFunctions.TestByHash(svg, "bd442ea77d45a41a4f490b8d41591e04");
    }

    [Fact]
    public void can_render_svg_qrcode_from_helper()
    {
        var data = QrCodeGenerator.Generate("A", ErrorCorrectionLevel.Quartile);
        var svg = SvgRenderer.Render(data, 2, "#000000", "#ffffff");

        HelperFunctions.TestImageToFile(VisualTestPath, nameof(can_render_svg_qrcode_from_helper), svg);
        HelperFunctions.TestByHash(svg, "f5ec37aa9fb207e3701cc0d86c4a357d");
    }
}