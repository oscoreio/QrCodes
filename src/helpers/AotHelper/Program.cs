using QrCodes;
using QrCodes.Renderers;

Console.WriteLine("Build, rebuild or publish this app to see aot warnings.");

var qr = QrCodeGenerator.Generate("Hello World!", ErrorCorrectionLevel.Medium);

Console.WriteLine(AsciiRenderer.Render(qr, 1));

var bytes = new SkiaSharpRenderer().RenderToBytes(qr);

File.WriteAllBytes("qr.png", bytes);