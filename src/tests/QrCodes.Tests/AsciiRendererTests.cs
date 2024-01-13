using QrCodes.Renderers;
using Xunit;
using FluentAssertions;

namespace QrCodes.Tests;

public class AsciiRendererTests
{                        
    [Fact]
    public void can_render_ascii_qrcode()
    {
        var targetCode = "                                                          \n                                                          \n                                                          \n                                                          \n        ██████████████    ██  ██    ██████████████        \n        ██          ██  ██    ████  ██          ██        \n        ██  ██████  ██  ██  ██  ██  ██  ██████  ██        \n        ██  ██████  ██      ██      ██  ██████  ██        \n        ██  ██████  ██  ██          ██  ██████  ██        \n        ██          ██    ████████  ██          ██        \n        ██████████████  ██  ██  ██  ██████████████        \n                        ██  ████                          \n        ██████████  ████      ████████  ██  ████          \n        ████    ██    ██    ████      ████████  ██        \n            ██  ██  ██████████  ██  ██  ██  ████          \n        ██      ██    ████  ████  ████                    \n          ████████  ██████            ████  ██  ██        \n                                  ████████                \n        ██████████████  ████  ████  ██  ████  ████        \n        ██          ██            ████████                \n        ██  ██████  ██  ██  ██  ██    ██    ██  ██        \n        ██  ██████  ██  ██████    ██  ██                  \n        ██  ██████  ██  ██  ██  ██  ██  ████  ████        \n        ██          ██  ████  ████        ██  ██          \n        ██████████████  ██████          ██  ██████        \n                                                          \n                                                          \n                                                          \n                                                          ";

        //Create QR code
        var data = QrCodeGenerator.Generate("A05", ErrorCorrectionLevel.Quartile);
        var asciiCode = AsciiRenderer.Render(data, 1);

        asciiCode.Should().Be(targetCode);
    }

    [Fact]
    public void can_render_ascii_qrcode_without_quietzones()
    {
        var data = QrCodeGenerator.Generate(
            plainText: "A05",
            eccLevel: ErrorCorrectionLevel.Quartile);
        var asciiCode = AsciiRenderer.Render(
            data: data,
            repeatPerModule: 1,
            drawQuietZones: false);

        asciiCode.Should().Be(expected: @"██████████████    ██  ██    ██████████████
██          ██  ██    ████  ██          ██
██  ██████  ██  ██  ██  ██  ██  ██████  ██
██  ██████  ██      ██      ██  ██████  ██
██  ██████  ██  ██          ██  ██████  ██
██          ██    ████████  ██          ██
██████████████  ██  ██  ██  ██████████████
                ██  ████                  
██████████  ████      ████████  ██  ████  
████    ██    ██    ████      ████████  ██
    ██  ██  ██████████  ██  ██  ██  ████  
██      ██    ████  ████  ████            
  ████████  ██████            ████  ██  ██
                          ████████        
██████████████  ████  ████  ██  ████  ████
██          ██            ████████        
██  ██████  ██  ██  ██  ██    ██    ██  ██
██  ██████  ██  ██████    ██  ██          
██  ██████  ██  ██  ██  ██  ██  ████  ████
██          ██  ████  ████        ██  ██  
██████████████  ██████          ██  ██████");
    }

    [Fact]
    public void can_render_ascii_qrcode_with_custom_symbols()
    {
        var data = QrCodeGenerator.Generate("A", ErrorCorrectionLevel.Quartile);
        var asciiCode = AsciiRenderer.Render(
            data,
            repeatPerModule: 2,
            "X",
            " ");

        asciiCode.Should().Be(@"                                                          
                                                          
                                                          
                                                          
                                                          
                                                          
                                                          
                                                          
        XXXXXXXXXXXXXX        XXXX  XXXXXXXXXXXXXX        
        XXXXXXXXXXXXXX        XXXX  XXXXXXXXXXXXXX        
        XX          XX  XXXXXX  XX  XX          XX        
        XX          XX  XXXXXX  XX  XX          XX        
        XX  XXXXXX  XX    XXXXXXXX  XX  XXXXXX  XX        
        XX  XXXXXX  XX    XXXXXXXX  XX  XXXXXX  XX        
        XX  XXXXXX  XX    XXXX      XX  XXXXXX  XX        
        XX  XXXXXX  XX    XXXX      XX  XXXXXX  XX        
        XX  XXXXXX  XX  XX    XX    XX  XXXXXX  XX        
        XX  XXXXXX  XX  XX    XX    XX  XXXXXX  XX        
        XX          XX    XX        XX          XX        
        XX          XX    XX        XX          XX        
        XXXXXXXXXXXXXX  XX  XX  XX  XXXXXXXXXXXXXX        
        XXXXXXXXXXXXXX  XX  XX  XX  XXXXXXXXXXXXXX        
                          XXXXXXXX                        
                          XXXXXXXX                        
            XX  XXXXXX  XXXXXX    XX    XX    XX          
            XX  XXXXXX  XXXXXX    XX    XX    XX          
        XX  XXXXXX    XXXX  XXXXXXXX    XXXXXX  XX        
        XX  XXXXXX    XXXX  XXXXXXXX    XXXXXX  XX        
                    XX  XX  XX    XX    XX  XX            
                    XX  XX  XX    XX    XX  XX            
          XX          XX        XX  XX  XXXXXX            
          XX          XX        XX  XX  XXXXXX            
          XX  XXXXXXXX  XXXX  XX    XXXXXXXX    XX        
          XX  XXXXXXXX  XXXX  XX    XXXXXXXX    XX        
                        XX    XXXXXXXX        XXXX        
                        XX    XXXXXXXX        XXXX        
        XXXXXXXXXXXXXX      XXXXXXXX    XX  XXXXXX        
        XXXXXXXXXXXXXX      XXXXXXXX    XX  XXXXXX        
        XX          XX  XXXXXX        XXXXXXXX            
        XX          XX  XXXXXX        XXXXXXXX            
        XX  XXXXXX  XX  XX  XXXX        XX  XXXX          
        XX  XXXXXX  XX  XX  XXXX        XX  XXXX          
        XX  XXXXXX  XX    XXXX        XXXXXXXX            
        XX  XXXXXX  XX    XXXX        XXXXXXXX            
        XX  XXXXXX  XX  XX  XXXXXXXX    XX  XXXXXX        
        XX  XXXXXX  XX  XX  XXXXXXXX    XX  XXXXXX        
        XX          XX    XX            XXXX    XX        
        XX          XX    XX            XXXX    XX        
        XXXXXXXXXXXXXX    XX    XXXXXX  XXXX  XXXX        
        XXXXXXXXXXXXXX    XX    XXXXXX  XXXX  XXXX        
                                                          
                                                          
                                                          
                                                          
                                                          
                                                          
                                                          
                                                          ");
    }

    [Fact]
    public void can_render_ascii_qrcode_from_helper()
    {
        AsciiRenderer.Render(
            data: QrCodeGenerator.Generate(
                plainText: "A",
                eccLevel: ErrorCorrectionLevel.Quartile),
            repeatPerModule: 2,
            darkColorString: "X",
            whiteSpaceString: " ")
            .Should().Be(@"                                                          
                                                          
                                                          
                                                          
                                                          
                                                          
                                                          
                                                          
        XXXXXXXXXXXXXX        XXXX  XXXXXXXXXXXXXX        
        XXXXXXXXXXXXXX        XXXX  XXXXXXXXXXXXXX        
        XX          XX  XXXXXX  XX  XX          XX        
        XX          XX  XXXXXX  XX  XX          XX        
        XX  XXXXXX  XX    XXXXXXXX  XX  XXXXXX  XX        
        XX  XXXXXX  XX    XXXXXXXX  XX  XXXXXX  XX        
        XX  XXXXXX  XX    XXXX      XX  XXXXXX  XX        
        XX  XXXXXX  XX    XXXX      XX  XXXXXX  XX        
        XX  XXXXXX  XX  XX    XX    XX  XXXXXX  XX        
        XX  XXXXXX  XX  XX    XX    XX  XXXXXX  XX        
        XX          XX    XX        XX          XX        
        XX          XX    XX        XX          XX        
        XXXXXXXXXXXXXX  XX  XX  XX  XXXXXXXXXXXXXX        
        XXXXXXXXXXXXXX  XX  XX  XX  XXXXXXXXXXXXXX        
                          XXXXXXXX                        
                          XXXXXXXX                        
            XX  XXXXXX  XXXXXX    XX    XX    XX          
            XX  XXXXXX  XXXXXX    XX    XX    XX          
        XX  XXXXXX    XXXX  XXXXXXXX    XXXXXX  XX        
        XX  XXXXXX    XXXX  XXXXXXXX    XXXXXX  XX        
                    XX  XX  XX    XX    XX  XX            
                    XX  XX  XX    XX    XX  XX            
          XX          XX        XX  XX  XXXXXX            
          XX          XX        XX  XX  XXXXXX            
          XX  XXXXXXXX  XXXX  XX    XXXXXXXX    XX        
          XX  XXXXXXXX  XXXX  XX    XXXXXXXX    XX        
                        XX    XXXXXXXX        XXXX        
                        XX    XXXXXXXX        XXXX        
        XXXXXXXXXXXXXX      XXXXXXXX    XX  XXXXXX        
        XXXXXXXXXXXXXX      XXXXXXXX    XX  XXXXXX        
        XX          XX  XXXXXX        XXXXXXXX            
        XX          XX  XXXXXX        XXXXXXXX            
        XX  XXXXXX  XX  XX  XXXX        XX  XXXX          
        XX  XXXXXX  XX  XX  XXXX        XX  XXXX          
        XX  XXXXXX  XX    XXXX        XXXXXXXX            
        XX  XXXXXX  XX    XXXX        XXXXXXXX            
        XX  XXXXXX  XX  XX  XXXXXXXX    XX  XXXXXX        
        XX  XXXXXX  XX  XX  XXXXXXXX    XX  XXXXXX        
        XX          XX    XX            XXXX    XX        
        XX          XX    XX            XXXX    XX        
        XXXXXXXXXXXXXX    XX    XXXXXX  XXXX  XXXX        
        XXXXXXXXXXXXXX    XX    XXXXXX  XXXX  XXXX        
                                                          
                                                          
                                                          
                                                          
                                                          
                                                          
                                                          
                                                          ");
    }
}