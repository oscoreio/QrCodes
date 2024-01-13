using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using QrCodes.Renderers;

namespace QrCodes.Benchmarks;

// ReSharper disable UnassignedField.Global
[MemoryDiagnoser]
[MarkdownExporterAttribute.GitHub]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
[CategoriesColumn]
[HideColumns("Error", "StdDev", "StdDev", "RatioSD")]
public class Benchmarks
{
    private readonly QrCode _qrCode = QrCodeGenerator.Generate("Hello, world!", ErrorCorrectionLevel.High);
    
    [Benchmark(Baseline = true)]
    [BenchmarkCategory("Renderers")]
    public byte[] ImageSharpRenderer_Png() => ImageSharpRenderer.Render(_qrCode).ToBytes(FileFormat.Png);
    
    [BenchmarkCategory("Renderers")]
    public byte[] ImageSharpRenderer_Jpeg() => ImageSharpRenderer.Render(_qrCode).ToBytes(FileFormat.Jpeg);
    
    [BenchmarkCategory("Renderers")]
    public byte[] ImageSharpRenderer_Bmp() => ImageSharpRenderer.Render(_qrCode).ToBytes(FileFormat.Bmp);
    
    [Benchmark]
    [BenchmarkCategory("Renderers")]
    public byte[] PngRenderer_() => PngRenderer.Render(_qrCode);
    
    [Benchmark]
    [BenchmarkCategory("Renderers")]
    public byte[] BitmapRenderer_() => BitmapRenderer.Render(_qrCode);
    
    [Benchmark]
    [BenchmarkCategory("Renderers")]
    public string SvgRenderer_() => SvgRenderer.Render(_qrCode);
}