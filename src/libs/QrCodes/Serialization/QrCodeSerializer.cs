using System.Collections;
using System.IO.Compression;

namespace QrCodes.Serialization;

/// <summary>
/// 
/// </summary>
public static class QrCodeSerializer
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="bytes"></param>
    /// <param name="compressMode"></param>
    /// <exception cref="Exception"></exception>
    public static QrCode Deserialize(
        byte[] bytes,
        Compression compressMode = Compression.Uncompressed)
    {
        //Decompress
        if (compressMode == Compression.Deflate)
        {
            using var input = new MemoryStream(bytes.ToArray());
            using var output = new MemoryStream();
            using (var stream = new DeflateStream(input, CompressionMode.Decompress))
                stream.CopyTo(output);
            bytes = [..output.ToArray()];
        }
        else if (compressMode == Compression.GZip)
        {
            using var input = new MemoryStream(bytes.ToArray());
            using var output = new MemoryStream();
            using (var stream = new GZipStream(input, CompressionMode.Decompress))
                stream.CopyTo(output);
            bytes = [..output.ToArray()];
        }

        if (bytes[0] != 0x51 || bytes[1] != 0x52 || bytes[2] != 0x52)
            throw new InvalidOperationException("Invalid raw data file. Filetype doesn't match \"QRR\".");

        //Set QR code version
        var sideLength = (int)bytes[4];
        var version = (sideLength - 21 - 8) / 4 + 1;

        //Unpack
        var modules = new Queue<bool>(8 * (bytes.Length - 5));
        foreach (var b in bytes.Skip(5))
        {
            for (var i = 7; i >= 0; i--)
            {
                modules.Enqueue((b & (1 << i)) != 0);
            }
        }

        //Build module matrix
        var moduleMatrix = new List<BitArray>(sideLength);
        for (var y = 0; y < sideLength; y++)
        {
            moduleMatrix.Add(new BitArray(sideLength));
            for (var x = 0; x < sideLength; x++)
            {
                moduleMatrix[y][x] = modules.Dequeue();
            }
        }
        
        return new QrCode(version, moduleMatrix);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="compressMode"></param>
    /// <returns></returns>
    public static byte[] Serialize(
        QrCode data,
        Compression compressMode = Compression.Uncompressed)
    {
        data = data ?? throw new ArgumentNullException(nameof(data));
        
        var bytes = new List<byte>();

        //Add header - signature ("QRR")
        bytes.AddRange(new byte[]{ 0x51, 0x52, 0x52, 0x00 });

        //Add header - row size
        bytes.Add((byte)data.ModuleMatrix.Count);

        //Build data queue
        var dataQueue = new Queue<int>();
        foreach (var row in data.ModuleMatrix)
        {
            foreach (var module in row)
            {
                dataQueue.Enqueue((bool)module ? 1 : 0);
            }
        }
        for (int i = 0; i < 8 - (data.ModuleMatrix.Count * data.ModuleMatrix.Count) % 8; i++)
        {
            dataQueue.Enqueue(0);
        }

        //Process queue
        while (dataQueue.Count > 0)
        {
            byte b = 0;
            for (int i = 7; i >= 0; i--)
            {
                b += (byte)(dataQueue.Dequeue() << i);
            }
            bytes.Add(b);
        }
        var rawData = bytes.ToArray();

        //Compress stream (optional)
        if (compressMode == Compression.Deflate)
        {
            using var output = new MemoryStream();
            using (var stream = new DeflateStream(output, CompressionMode.Compress))
            {
                stream.Write(rawData, 0, rawData.Length);
            }
            rawData = output.ToArray();
        }
        else if (compressMode == Compression.GZip)
        {
            using var output = new MemoryStream();
            using (GZipStream gzipStream = new GZipStream(output, CompressionMode.Compress, true))
            {
                gzipStream.Write(rawData, 0, rawData.Length);
            }
            rawData = output.ToArray();
        }
        
        return rawData;
    }
}