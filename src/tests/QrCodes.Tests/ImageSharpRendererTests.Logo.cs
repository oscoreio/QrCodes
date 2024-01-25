using QrCodes.Renderers;
using QrCodes.Renderers.Abstractions;
using QrCodes.Tests.Helpers;
using SixLabors.ImageSharp;
using Xunit;

namespace QrCodes.Tests;

public partial class ImageSharpRendererTests
{
    [Fact]
    public void can_create_qrcode_with_transparent_logo_graphic()
    {
        var data = QrCodeGenerator.Generate(QrCodeContent, ErrorCorrectionLevel.High);
        var image = ImageSharpRenderer.Render(data, new RendererSettings
        {
            PixelsPerModule = 10,
            IconBytes = H.Resources.noun_software_engineer_2909346_png.AsBytes(),
        });

        HelperFunctions.TestImageToFile(VisualTestPath, nameof(can_create_qrcode_with_transparent_logo_graphic), image);
        HelperFunctions.TestByDecode(image, QrCodeContent);
        HelperFunctions.TestByHash(image, "d19c708b8e2b28c62a6b9db3e630179a");
    }

    [Fact]
    public void can_create_qrcode_with_non_transparent_logo_graphic()
    {
        var data = QrCodeGenerator.Generate(QrCodeContent, ErrorCorrectionLevel.High);
        var image = ImageSharpRenderer.Render(data, new RendererSettings
        {
            PixelsPerModule = 10,
            IconBytes = H.Resources.noun_software_engineer_2909346_png.AsBytes(),
            IconBackgroundColor = System.Drawing.Color.White,
        });

        HelperFunctions.TestImageToFile(VisualTestPath, nameof(can_create_qrcode_with_non_transparent_logo_graphic),
            image);
        HelperFunctions.TestByDecode(image, QrCodeContent);
        HelperFunctions.TestByHash(image, "5e535aac60c1bc7ee8ec506916cd2dd8");
    }

    [Fact]
    public void can_create_qrcode_with_logo_and_with_transparent_border()
    {
        var data = QrCodeGenerator.Generate(QrCodeContent, ErrorCorrectionLevel.High);
        var image = ImageSharpRenderer.Render(data, new RendererSettings
        {
            PixelsPerModule = 10,
            IconBytes = H.Resources.noun_software_engineer_2909346_png.AsBytes(),
            IconBorderWidth = 6,
        });

        HelperFunctions.TestImageToFile(VisualTestPath, nameof(can_create_qrcode_with_logo_and_with_transparent_border),
            image);
        HelperFunctions.TestByDecode(image, QrCodeContent);
        HelperFunctions.TestByHash(image, "d19c708b8e2b28c62a6b9db3e630179a");
    }

    [Fact]
    public void can_create_qrcode_with_logo_and_with_standard_border()
    {
        var data = QrCodeGenerator.Generate(QrCodeContent, ErrorCorrectionLevel.High);
        var image = ImageSharpRenderer.Render(data, new RendererSettings
        {
            PixelsPerModule = 10,
            IconBytes = H.Resources.noun_software_engineer_2909346_png.AsBytes(),
            IconBorderWidth = 6,
            IconBackgroundColor = System.Drawing.Color.White,
        });

        HelperFunctions.TestImageToFile(VisualTestPath, nameof(can_create_qrcode_with_logo_and_with_standard_border),
            image);
        HelperFunctions.TestByDecode(image, QrCodeContent);
        HelperFunctions.TestByHash(image, "91f35d10164ccd4a9ad621e2dc81c86b");
    }

    [Fact]
    public void can_create_qrcode_with_logo_and_with_custom_border()
    {
        var data = QrCodeGenerator.Generate(QrCodeContent, ErrorCorrectionLevel.High);
        var image = ImageSharpRenderer.Render(data, new RendererSettings
        {
            PixelsPerModule = 10,
            IconBytes = H.Resources.noun_software_engineer_2909346_png.AsBytes(),
            IconBorderWidth = 6,
            BackgroundType = BackgroundType.Rectangle,
            IconBackgroundColor = System.Drawing.Color.DarkGreen,
        });

        HelperFunctions.TestImageToFile(VisualTestPath, nameof(can_create_qrcode_with_logo_and_with_custom_border),
            image);
        HelperFunctions.TestByDecode(image, QrCodeContent);
        HelperFunctions.TestByHash(image, "ce13cc3372aa477a914c9828cdad4754");
    }
}