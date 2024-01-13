using QrCodes.Renderers;
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
        var image = ImageSharpRenderer.Render(data, 10, Color.Black, Color.Transparent,
            icon: Image.Load(H.Resources.noun_software_engineer_2909346_png.AsStream()));

        HelperFunctions.TestImageToFile(VisualTestPath, nameof(can_create_qrcode_with_transparent_logo_graphic), image);
        HelperFunctions.TestByDecode(image, QrCodeContent);
        HelperFunctions.TestByHash(image, "d19c708b8e2b28c62a6b9db3e630179a");
    }

    [Fact]
    public void can_create_qrcode_with_non_transparent_logo_graphic()
    {
        var data = QrCodeGenerator.Generate(QrCodeContent, ErrorCorrectionLevel.High);
        var image = ImageSharpRenderer.Render(data, 10, Color.Black, Color.White,
            icon: Image.Load(H.Resources.noun_software_engineer_2909346_png.AsStream()));

        HelperFunctions.TestImageToFile(VisualTestPath, nameof(can_create_qrcode_with_non_transparent_logo_graphic),
            image);
        HelperFunctions.TestByDecode(image, QrCodeContent);
        HelperFunctions.TestByHash(image, "5e535aac60c1bc7ee8ec506916cd2dd8");
    }

    [Fact]
    public void can_create_qrcode_with_logo_and_with_transparent_border()
    {
        var data = QrCodeGenerator.Generate(QrCodeContent, ErrorCorrectionLevel.High);
        var image = ImageSharpRenderer.Render(data, 10, Color.Black, Color.Transparent, iconBorderWidth: 6,
            icon: Image.Load(H.Resources.noun_software_engineer_2909346_png.AsStream()));

        HelperFunctions.TestImageToFile(VisualTestPath, nameof(can_create_qrcode_with_logo_and_with_transparent_border),
            image);
        HelperFunctions.TestByDecode(image, QrCodeContent);
        HelperFunctions.TestByHash(image, "d19c708b8e2b28c62a6b9db3e630179a");
    }

    [Fact]
    public void can_create_qrcode_with_logo_and_with_standard_border()
    {
        var data = QrCodeGenerator.Generate(QrCodeContent, ErrorCorrectionLevel.High);
        var image = ImageSharpRenderer.Render(data, 10, Color.Black, Color.White, iconBorderWidth: 6,
            icon: Image.Load(H.Resources.noun_software_engineer_2909346_png.AsStream()));

        HelperFunctions.TestImageToFile(VisualTestPath, nameof(can_create_qrcode_with_logo_and_with_standard_border),
            image);
        HelperFunctions.TestByDecode(image, QrCodeContent);
        HelperFunctions.TestByHash(image, "91f35d10164ccd4a9ad621e2dc81c86b");
    }

    [Fact]
    public void can_create_qrcode_with_logo_and_with_custom_border()
    {
        var data = QrCodeGenerator.Generate(QrCodeContent, ErrorCorrectionLevel.High);
        var image = ImageSharpRenderer.Render(data, 10, Color.Black, Color.Transparent, iconBorderWidth: 6,
            iconBackgroundColor: Color.DarkGreen,
            icon: Image.Load(H.Resources.noun_software_engineer_2909346_png.AsStream()));

        HelperFunctions.TestImageToFile(VisualTestPath, nameof(can_create_qrcode_with_logo_and_with_custom_border),
            image);
        HelperFunctions.TestByDecode(image, QrCodeContent);
        HelperFunctions.TestByHash(image, "ce13cc3372aa477a914c9828cdad4754");
    }
}