using System.Collections;

// ReSharper disable once CheckNamespace

namespace QrCodes;

/// <summary>
/// 
/// </summary>
public class QrCode
{
    /// <summary>
    /// 
    /// </summary>
    public int Version { get; private set; }

    /// <summary>
    /// 
    /// </summary>
    public IList<BitArray> ModuleMatrix { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="version"></param>
    public QrCode(int version)
    {
        Version = version;
        var size = ModulesPerSideFromVersion(version);
        ModuleMatrix = new List<BitArray>();
        for (var i = 0; i < size; i++)
            ModuleMatrix.Add(new BitArray(size));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="version"></param>
    /// <param name="moduleMatrix"></param>
    public QrCode(int version, IList<BitArray> moduleMatrix)
    {
        Version = version;
        ModuleMatrix = moduleMatrix;
    }
    
    private static int ModulesPerSideFromVersion(int version)
    {
        return 21 + (version - 1) * 4;
    }
}