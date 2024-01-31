# QrCodes

[![Nuget package](https://img.shields.io/nuget/vpre/Oscore.QrCodes.Maui)](https://www.nuget.org/packages/Oscore.QrCodes.Maui/)
[![CI/CD](https://github.com/oscoreio/QrCodes/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/oscoreio/QrCodes/actions/workflows/dotnet.yml)
[![License: MIT](https://img.shields.io/github/license/oscoreio/QrCodes)](https://github.com/oscoreio/QrCodes/blob/main/LICENSE)

Modern and efficient cross-platform QR code generation, rendering and serialization.  
![qr](assets/qr.png)

### 🔥 Features 🔥
- Use SkiaSharp/ImageSharp instead of System.Drawing to be cross-platform.
- Support latest dotnet versions.
- Supports trimming/NativeAOT.
- Generate QR code with logo image.
- Allows many different styles.
- Supports many predefined payloads:
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
  - PDF
  - FastPngRenderer - fast but not support all features
  - Bitmap(.bmp) - fast but not support all features
  - ImageSharp - powerful, allows many features and export formats
  - SkiaSharp - powerful, allows many features and export formats
  - System.Drawing - legacy, only windows, not support all features
- Supports helpers for MAUI
  - QrCodeSource - ImageSource to produce QR code
  - QrCodeExtension markup extension - Simplifies usage of QrCodeSource

### Usage
```
// Base library with all payloads and some renderers(Ascii, Base64, Pdf, FastBitmap, FastPng, SVG, PostScript)
dotnet add package Oscore.QrCodes

// SkiaSharpRenderer(Gif, Jpeg, Png, WebP, Bmp, Ico, Wbmp, Pkm, Ktx, Astc, Dng, Heif, Avif)
dotnet add package Oscore.QrCodes.SkiaSharp
// ImageSharpRenderer(Gif, Jpeg, Png, WebP, Bmp, Pbm, Tga, Tiff)
dotnet add package Oscore.QrCodes.ImageSharp
// SystemDrawingRenderer(Gif, Jpeg, Png, Bmp, Tiff)
dotnet add package Oscore.QrCodes.System.Drawing

// MAUI helpers(QrCodeSource and QrCodeExtension markup extension). Uses SkiaSharpRenderer.
dotnet add package Oscore.QrCodes.Maui
```

#### Generate QR code
```csharp
var qrCode = QrCodeGenerator.Generate(
    plainText: new Mail(email: "hello@oscore.io").ToString(),
    eccLevel: ErrorCorrectionLevel.High);
using var pngBytes = SkiaSharpRenderer.RenderToBytes(
    data,
    settings: new RendererSettings
    {
        DarkColor = Color.Red,
    });
```

#### Generate ImageSource for MAUI
You can test all variants using [QrCodes.SampleApp MAUI app](sample)
```
xmlns:qr="clr-namespace:QrCodes.Maui;assembly=QrCodes.Maui"
```
```xml
<Image Source="{qr:QrCode 'https://oscore.io/'}" />
```

### Links
- https://github.com/SixLabors/ImageSharp
- https://github.com/codebude/QRCoder
- https://github.com/JPlenert/QRCoder-ImageSharp
- https://dev.to/vhugogarcia/generate-qr-code-in-net-maui-3c8n
- https://qrapi.io/
- https://github.com/manuelbl/QrCodeGenerator
- https://qr.io/
- https://github.com/guitarrapc/SkiaSharp.QrCode

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

### Disclaimer
Although the library includes many things, 
we only use and support QrCodes/SkiaSharp/Maui functionality.  
But we will be happy to accept your PR regarding ImageSharp/SystemDrawing/WPF/other renderers.