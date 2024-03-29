﻿using QrCodes.Renderers;
using QrCodes.Renderers.Abstractions;
using Xunit;
using QrCodes.Tests.Helpers;

namespace QrCodes.Tests;

public partial class ImageSharpRendererTests
{
    const string QrCodeContent = "This is a quick test! 123#?";
    const string? VisualTestPath = null;
    //private string? VisualTestPath => Path.GetTempPath();

    [Fact]
    public void can_create_qrcode_standard_graphic()
    {
        var data = QrCodeGenerator.Generate(QrCodeContent, ErrorCorrectionLevel.High);
        var image = ImageSharpRenderer.Render(data, new RendererSettings
        {
            PixelsPerModule = 10,
        });
        
        HelperFunctions.TestImageToFile(VisualTestPath, nameof(can_create_qrcode_standard_graphic), image);
        HelperFunctions.TestByDecode(image, QrCodeContent);
        HelperFunctions.TestByHash(image, "c0f8af4256eddc7e566983e539cce389");
    }

    [Fact]
    public void can_create_qrcode_standard_graphic_hex()
    {
        var data = QrCodeGenerator.Generate(QrCodeContent, ErrorCorrectionLevel.High);
        var image = ImageSharpRenderer.Render(data, new RendererSettings
        {
            PixelsPerModule = 10,
        });
        
        HelperFunctions.TestImageToFile(VisualTestPath, nameof(can_create_qrcode_standard_graphic_hex), image);
        HelperFunctions.TestByDecode(image, QrCodeContent);
        HelperFunctions.TestByHash(image, "c0f8af4256eddc7e566983e539cce389");
    }

    [Fact]
    public void can_create_qrcode_standard_graphic_without_quietzones()
    {
        var qrCode = QrCodeGenerator.Generate(
            plainText: QrCodeContent,
            eccLevel: ErrorCorrectionLevel.High);
        var image = ImageSharpRenderer.Render(
            data: qrCode,
            settings: new RendererSettings
            {
                PixelsPerModule = 5,
                DrawQuietZones = false,
            });
        
        HelperFunctions.TestImageToFile(VisualTestPath, nameof(can_create_qrcode_standard_graphic_without_quietzones), image);
        HelperFunctions.TestByDecode(image, QrCodeContent);
        HelperFunctions.TestByHash(image, "8a2d62fa98c09d764a21466b8d6bb6c8");
    }

    [Fact]
    public void can_render_qrcode_from_helper()
    {
        var data = QrCodeGenerator.Generate(QrCodeContent, ErrorCorrectionLevel.High);
        var image = ImageSharpRenderer.Render(data, new RendererSettings
        {
            PixelsPerModule = 10,
        });

        HelperFunctions.TestImageToFile(VisualTestPath, nameof(can_render_qrcode_from_helper), image);
        HelperFunctions.TestByDecode(image, QrCodeContent);
        HelperFunctions.TestByHash(image, "c0f8af4256eddc7e566983e539cce389");
    }
}