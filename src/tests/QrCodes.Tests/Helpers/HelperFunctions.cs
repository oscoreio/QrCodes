using System.Diagnostics;
using System.Text;
using System.Security.Cryptography;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using FluentAssertions;

namespace QrCodes.Tests.Helpers;

public static class HelperFunctions
{
    public static string BitmapToHash(Image img)
    {
        byte[]? imgBytes;
        using (var ms = new MemoryStream())
        {
            img.SaveAsPng(ms);
            imgBytes = ms.ToArray();
        }
        return ByteArrayToHash(imgBytes);
    }

    public static string ByteArrayToHash(byte[] data)
    {
        var md5 = MD5.Create();
        var hash = md5.ComputeHash(data);
        return BitConverter.ToString(hash).Replace("-", "").ToLower();
    }

    public static string StringToHash(string data)
    {
        return ByteArrayToHash(Encoding.UTF8.GetBytes(data));
    }

    public static void TestByDecode(Image<Rgba32> image, string desiredContent)
    {
        ZXing.ImageSharp.BarcodeReader<Rgba32> reader = new ZXing.ImageSharp.BarcodeReader<Rgba32>();
        ZXing.Result result = reader.Decode(image);
        result.Text.Should().Be(desiredContent);
    }

    public static void TestByDecode(byte[] pngCodeGfx, string desiredContent)
    {
        using var mStream = new MemoryStream(pngCodeGfx);
        ZXing.Result result;

        Image image = Image.Load(mStream);
        Type pixelType = image.GetType().GetGenericArguments()[0];
        if (pixelType == typeof(Rgba32))
        {
            ZXing.ImageSharp.BarcodeReader<Rgba32> reader = new ZXing.ImageSharp.BarcodeReader<Rgba32>();
            result = reader.Decode(image as Image<Rgba32>);
        }
        else if (pixelType == typeof(L8))
        {
            ZXing.ImageSharp.BarcodeReader<L8> reader = new ZXing.ImageSharp.BarcodeReader<L8>();
            result = reader.Decode(image as Image<L8>);
        }
        else
            throw new NotImplementedException(pixelType.ToString());
        
        result.Text.Should().Be(desiredContent);
    }

    public static void TestByHash(Image<Rgba32> image, string desiredHash) =>
        BitmapToHash(image).Should().Be(desiredHash);

    public static void TestByHash(byte[] pngCodeGfx, string desiredHash)
    {
        using var mStream = new MemoryStream(pngCodeGfx);
        var img = Image.Load(mStream);
        var result = BitmapToHash(img);
        result.Should().Be(desiredHash);
    }

    public static void TestByHash(string svg, string desiredHash) =>
        ByteArrayToHash(Encoding.UTF8.GetBytes(desiredHash));

    public static void TestImageToFile(string? path, string testName, Image<Rgba32> image)
    {
        if (string.IsNullOrEmpty(path))
            return;

        var fullPath = Path.Combine(path, $"qrtest_{testName}.png");
        image.Save(fullPath);
        
        //Process.Start(new ProcessStartInfo(fullPath) { UseShellExecute = true });
    }

    public static void TestImageToFile(string path, string testName, byte[] data)
    {
        if (String.IsNullOrEmpty(path))
            return;
        //Used logo is licensed under public domain. Ref.: https://thenounproject.com/Iconathon1/collection/redefining-women/?i=2909346
        File.WriteAllBytes(Path.Combine(path, $"qrtestPNG_{testName}.png"), data);
    }

    public static void TestImageToFile(string path, string testName, string svg)
    {
        if (String.IsNullOrEmpty(path))
            return;

        //Used logo is licensed under public domain. Ref.: https://thenounproject.com/Iconathon1/collection/redefining-women/?i=2909346
        File.WriteAllText(Path.Combine(path, $"qrtestSVG_{testName}.svg"), svg);
    }
}