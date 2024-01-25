# QrCodes
Modern and efficient cross-platform QR code generation, rendering and serialization. 
It contains various payloads and framework helpers for MAUI.  

### 🔥 Features 🔥
- Use SkiaSharp/ImageSharp instead of System.Drawing to be cross-platform.
- Support latest dotnet versions.
- Generate QR code with logo image.
- Supports next payloads
  - BezahlCode
  - Bitcoin like address
  - Bookmark
  - CalendarEvent
  - ContactData
  - Geolocation
  - Girocode
  - Mail
  - MMS
  - MoneroTransaction
  - OneTimePassword
  - PhoneNumber
  - ShadowSocksConfig
  - SkypeCall
  - SloveniaUpnQR
  - SMS
  - SwissQRCode
  - Url
  - WhatsAppMessage
  - Telegram
  - Wi-Fi
- Supports next renderers
  - Ascii
  - Base64
  - SVG
  - PostScript
  - FastPngRenderer - fast but not support all features
  - Bitmap(.bmp) - fast but not support all features
  - ImageSharp - powerful, allows many features and export formats
  - SkiaSharp - powerful, allows many features and export formats
  - PDF - powered by ImageSharp
- Supports helpers for MAUI
  - QrCodeSource - ImageSource to produce QR code
  - QrCodeExtension markup extension - Simplifies usage of QrCodeSource

### Usage
```
// Base library with all payloads and some renderers(Ascii, Base64, Bitmap, PNG, SVG, PostScript)
dotnet add package QrCodes
// ImageSharpRenderer(Gif, Jpeg, Png, WebP, Bmp, Pbm, Tga, Tiff), Export to PDF
dotnet add package QrCodes.ImageSharp
// SkiaSharpRenderer(Gif, Jpeg, Png, WebP, Bmp, Ico, Wbmp, Pkm, Ktx, Astc, Dng, Heif, Avif), Export to PDF
dotnet add package QrCodes.ImageSharp
// MAUI helpers(QrCodeSource and QrCodeExtension markup extension). Uses SkiaSharpRenderer.
dotnet add package QrCodes.Maui
```

#### Generate QR code with logo image
```csharp
var qrCode = QrCodeGenerator.Generate(
    plainText: new Telegram(user: "havendv").ToString(),
    eccLevel: ErrorCorrectionLevel.High);
using var image = SkiaSharpRenderer.Render(
    data: qrCode,
    pixelsPerModule: 5,
    darkColor: Color.Black,
    lightColor: Color.White,
    drawQuietZones: false);
var bytes = image.ToBytes(FileFormat.Png);
```

#### Generate ImageSource for MAUI
You can test all variants using [QrCodes.SampleApp MAUI app](sample)
```
xmlns:qr="clr-namespace:QrCodes.Maui;assembly=QrCodes.Maui"
```
```xml
<Image Source="{qr:QrCode 'Fixed value'}" />
```

### Links
- https://github.com/SixLabors/ImageSharp
- https://github.com/codebude/QRCoder
- https://github.com/JPlenert/QRCoder-ImageSharp
- https://dev.to/vhugogarcia/generate-qr-code-in-net-maui-3c8n
- https://qrapi.io/
- https://github.com/manuelbl/QrCodeGenerator
- https://qr.io/

### Benchmarks
You can view the reports for each version [here](benchmarks)

<!--BENCHMARKS_START-->
```

BenchmarkDotNet v0.13.12, macOS Sonoma 14.2.1 (23C71) [Darwin 23.2.0]
Apple M1 Pro, 1 CPU, 10 logical and 10 physical cores
.NET SDK 8.0.100
  [Host]     : .NET 8.0.0 (8.0.23.53103), Arm64 RyuJIT AdvSIMD
  DefaultJob : .NET 8.0.0 (8.0.23.53103), Arm64 RyuJIT AdvSIMD

Categories=Renderers  

```
| Method                  | Mean        | Ratio | Gen0     | Gen1     | Gen2    | Allocated | Alloc Ratio |
|------------------------ |------------:|------:|---------:|---------:|--------:|----------:|------------:|
| SkiaSharpRenderer_Png   | 1,065.14 μs |  1.00 |        - |        - |       - |   1.52 KB |        1.00 |
| SkiaSharpRenderer_Jpeg  |   331.18 μs |  0.31 |   1.9531 |        - |       - |  13.23 KB |        8.72 |
| SkiaSharpRenderer_Bmp   |          NA |     ? |       NA |       NA |      NA |        NA |           ? |
| ImageSharpRenderer_Png  |   417.70 μs |  0.40 |   1.9531 |   0.4883 |       - |   48.1 KB |       31.71 |
| ImageSharpRenderer_Jpeg |   297.34 μs |  0.28 |   9.7656 |   1.4648 |  0.4883 |  57.02 KB |       37.60 |
| ImageSharpRenderer_Bmp  |    67.90 μs |  0.06 |  79.2236 |  44.5557 | 43.3350 | 363.08 KB |      239.40 |
| FastPngRenderer_        |    43.58 μs |  0.04 |   0.8545 |        - |       - |   5.39 KB |        3.56 |
| BitmapRenderer_         |   519.42 μs |  0.49 | 219.7266 | 219.7266 | 36.1328 | 368.75 KB |      243.15 |
| SvgRenderer_            |    41.03 μs |  0.04 |   8.9111 |        - |       - |  54.95 KB |       36.23 |

Benchmarks with issues:
  Benchmarks.SkiaSharpRenderer_Bmp: DefaultJob

<!--BENCHMARKS_END-->

### Legal information and credits

It was forked from the [QRCoder-ImageSharp](https://github.com/JPlenert/QRCoder-ImageSharp) project.  
QRCoder is a project by [Raffael Herrmann](https://raffaelherrmann.de) and was first released in 10/2013.  
QRCoder-ImageSharp is a project by [Joerg Plenert](https://plenert.net).  
It's licensed under the [MIT license](https://github.com/JPlenert/QRCoder.ImageSharp/blob/master/license.txt).