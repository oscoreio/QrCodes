using System.Text;
using System.Collections;
using System.Globalization;

// ReSharper disable CheckNamespace
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Local

namespace QrCodes;

/// <summary>
/// 
/// </summary>
public static class QrCodeGenerator
{
    private static readonly char[] AlphanumEncTable = [' ', '$', '%', '*', '+', '-', '.', '/', ':'];
    private static readonly int[] CapacityBaseValues = [41, 25, 17, 10, 34, 20, 14, 8, 27, 16, 11, 7, 17, 10, 7, 4, 77, 47, 32, 20, 63, 38, 26, 16, 48, 29, 20, 12, 34, 20, 14, 8, 127, 77, 53, 32, 101, 61, 42, 26, 77, 47, 32, 20, 58, 35, 24, 15, 187, 114, 78, 48, 149, 90, 62, 38, 111, 67, 46, 28, 82, 50, 34, 21, 255, 154, 106, 65, 202, 122, 84, 52, 144, 87, 60, 37, 106, 64, 44, 27, 322, 195, 134, 82, 255, 154, 106, 65, 178, 108, 74, 45, 139, 84, 58, 36, 370, 224, 154, 95, 293, 178, 122, 75, 207, 125, 86, 53, 154, 93, 64, 39, 461, 279, 192, 118, 365, 221, 152, 93, 259, 157, 108, 66, 202, 122, 84, 52, 552, 335, 230, 141, 432, 262, 180, 111, 312, 189, 130, 80, 235, 143, 98, 60, 652, 395, 271, 167, 513, 311, 213, 131, 364, 221, 151, 93, 288, 174, 119, 74, 772, 468, 321, 198, 604, 366, 251, 155, 427, 259, 177, 109, 331, 200, 137, 85, 883, 535, 367, 226, 691, 419, 287, 177, 489, 296, 203, 125, 374, 227, 155, 96, 1022, 619, 425, 262, 796, 483, 331, 204, 580, 352, 241, 149, 427, 259, 177, 109, 1101, 667, 458, 282, 871, 528, 362, 223, 621, 376, 258, 159, 468, 283, 194, 120, 1250, 758, 520, 320, 991, 600, 412, 254, 703, 426, 292, 180, 530, 321, 220, 136, 1408, 854, 586, 361, 1082, 656, 450, 277, 775, 470, 322, 198, 602, 365, 250, 154, 1548, 938, 644, 397, 1212, 734, 504, 310, 876, 531, 364, 224, 674, 408, 280, 173, 1725, 1046, 718, 442, 1346, 816, 560, 345, 948, 574, 394, 243, 746, 452, 310, 191, 1903, 1153, 792, 488, 1500, 909, 624, 384, 1063, 644, 442, 272, 813, 493, 338, 208, 2061, 1249, 858, 528, 1600, 970, 666, 410, 1159, 702, 482, 297, 919, 557, 382, 235, 2232, 1352, 929, 572, 1708, 1035, 711, 438, 1224, 742, 509, 314, 969, 587, 403, 248, 2409, 1460, 1003, 618, 1872, 1134, 779, 480, 1358, 823, 565, 348, 1056, 640, 439, 270, 2620, 1588, 1091, 672, 2059, 1248, 857, 528, 1468, 890, 611, 376, 1108, 672, 461, 284, 2812, 1704, 1171, 721, 2188, 1326, 911, 561, 1588, 963, 661, 407, 1228, 744, 511, 315, 3057, 1853, 1273, 784, 2395, 1451, 997, 614, 1718, 1041, 715, 440, 1286, 779, 535, 330, 3283, 1990, 1367, 842, 2544, 1542, 1059, 652, 1804, 1094, 751, 462, 1425, 864, 593, 365, 3517, 2132, 1465, 902, 2701, 1637, 1125, 692, 1933, 1172, 805, 496, 1501, 910, 625, 385, 3669, 2223, 1528, 940, 2857, 1732, 1190, 732, 2085, 1263, 868, 534, 1581, 958, 658, 405, 3909, 2369, 1628, 1002, 3035, 1839, 1264, 778, 2181, 1322, 908, 559, 1677, 1016, 698, 430, 4158, 2520, 1732, 1066, 3289, 1994, 1370, 843, 2358, 1429, 982, 604, 1782, 1080, 742, 457, 4417, 2677, 1840, 1132, 3486, 2113, 1452, 894, 2473, 1499, 1030, 634, 1897, 1150, 790, 486, 4686, 2840, 1952, 1201, 3693, 2238, 1538, 947, 2670, 1618, 1112, 684, 2022, 1226, 842, 518, 4965, 3009, 2068, 1273, 3909, 2369, 1628, 1002, 2805, 1700, 1168, 719, 2157, 1307, 898, 553, 5253, 3183, 2188, 1347, 4134, 2506, 1722, 1060, 2949, 1787, 1228, 756, 2301, 1394, 958, 590, 5529, 3351, 2303, 1417, 4343, 2632, 1809, 1113, 3081, 1867, 1283, 790, 2361, 1431, 983, 605, 5836, 3537, 2431, 1496, 4588, 2780, 1911, 1176, 3244, 1966, 1351, 832, 2524, 1530, 1051, 647, 6153, 3729, 2563, 1577, 4775, 2894, 1989, 1224, 3417, 2071, 1423, 876, 2625, 1591, 1093, 673, 6479, 3927, 2699, 1661, 5039, 3054, 2099, 1292, 3599, 2181, 1499, 923, 2735, 1658, 1139, 701, 6743, 4087, 2809, 1729, 5313, 3220, 2213, 1362, 3791, 2298, 1579, 972, 2927, 1774, 1219, 750, 7089, 4296, 2953, 1817, 5596, 3391, 2331, 1435, 3993, 2420, 1663, 1024, 3057, 1852, 1273, 784];
    private static readonly int[] CapacityEccBaseValues = [19, 7, 1, 19, 0, 0, 16, 10, 1, 16, 0, 0, 13, 13, 1, 13, 0, 0, 9, 17, 1, 9, 0, 0, 34, 10, 1, 34, 0, 0, 28, 16, 1, 28, 0, 0, 22, 22, 1, 22, 0, 0, 16, 28, 1, 16, 0, 0, 55, 15, 1, 55, 0, 0, 44, 26, 1, 44, 0, 0, 34, 18, 2, 17, 0, 0, 26, 22, 2, 13, 0, 0, 80, 20, 1, 80, 0, 0, 64, 18, 2, 32, 0, 0, 48, 26, 2, 24, 0, 0, 36, 16, 4, 9, 0, 0, 108, 26, 1, 108, 0, 0, 86, 24, 2, 43, 0, 0, 62, 18, 2, 15, 2, 16, 46, 22, 2, 11, 2, 12, 136, 18, 2, 68, 0, 0, 108, 16, 4, 27, 0, 0, 76, 24, 4, 19, 0, 0, 60, 28, 4, 15, 0, 0, 156, 20, 2, 78, 0, 0, 124, 18, 4, 31, 0, 0, 88, 18, 2, 14, 4, 15, 66, 26, 4, 13, 1, 14, 194, 24, 2, 97, 0, 0, 154, 22, 2, 38, 2, 39, 110, 22, 4, 18, 2, 19, 86, 26, 4, 14, 2, 15, 232, 30, 2, 116, 0, 0, 182, 22, 3, 36, 2, 37, 132, 20, 4, 16, 4, 17, 100, 24, 4, 12, 4, 13, 274, 18, 2, 68, 2, 69, 216, 26, 4, 43, 1, 44, 154, 24, 6, 19, 2, 20, 122, 28, 6, 15, 2, 16, 324, 20, 4, 81, 0, 0, 254, 30, 1, 50, 4, 51, 180, 28, 4, 22, 4, 23, 140, 24, 3, 12, 8, 13, 370, 24, 2, 92, 2, 93, 290, 22, 6, 36, 2, 37, 206, 26, 4, 20, 6, 21, 158, 28, 7, 14, 4, 15, 428, 26, 4, 107, 0, 0, 334, 22, 8, 37, 1, 38, 244, 24, 8, 20, 4, 21, 180, 22, 12, 11, 4, 12, 461, 30, 3, 115, 1, 116, 365, 24, 4, 40, 5, 41, 261, 20, 11, 16, 5, 17, 197, 24, 11, 12, 5, 13, 523, 22, 5, 87, 1, 88, 415, 24, 5, 41, 5, 42, 295, 30, 5, 24, 7, 25, 223, 24, 11, 12, 7, 13, 589, 24, 5, 98, 1, 99, 453, 28, 7, 45, 3, 46, 325, 24, 15, 19, 2, 20, 253, 30, 3, 15, 13, 16, 647, 28, 1, 107, 5, 108, 507, 28, 10, 46, 1, 47, 367, 28, 1, 22, 15, 23, 283, 28, 2, 14, 17, 15, 721, 30, 5, 120, 1, 121, 563, 26, 9, 43, 4, 44, 397, 28, 17, 22, 1, 23, 313, 28, 2, 14, 19, 15, 795, 28, 3, 113, 4, 114, 627, 26, 3, 44, 11, 45, 445, 26, 17, 21, 4, 22, 341, 26, 9, 13, 16, 14, 861, 28, 3, 107, 5, 108, 669, 26, 3, 41, 13, 42, 485, 30, 15, 24, 5, 25, 385, 28, 15, 15, 10, 16, 932, 28, 4, 116, 4, 117, 714, 26, 17, 42, 0, 0, 512, 28, 17, 22, 6, 23, 406, 30, 19, 16, 6, 17, 1006, 28, 2, 111, 7, 112, 782, 28, 17, 46, 0, 0, 568, 30, 7, 24, 16, 25, 442, 24, 34, 13, 0, 0, 1094, 30, 4, 121, 5, 122, 860, 28, 4, 47, 14, 48, 614, 30, 11, 24, 14, 25, 464, 30, 16, 15, 14, 16, 1174, 30, 6, 117, 4, 118, 914, 28, 6, 45, 14, 46, 664, 30, 11, 24, 16, 25, 514, 30, 30, 16, 2, 17, 1276, 26, 8, 106, 4, 107, 1000, 28, 8, 47, 13, 48, 718, 30, 7, 24, 22, 25, 538, 30, 22, 15, 13, 16, 1370, 28, 10, 114, 2, 115, 1062, 28, 19, 46, 4, 47, 754, 28, 28, 22, 6, 23, 596, 30, 33, 16, 4, 17, 1468, 30, 8, 122, 4, 123, 1128, 28, 22, 45, 3, 46, 808, 30, 8, 23, 26, 24, 628, 30, 12, 15, 28, 16, 1531, 30, 3, 117, 10, 118, 1193, 28, 3, 45, 23, 46, 871, 30, 4, 24, 31, 25, 661, 30, 11, 15, 31, 16, 1631, 30, 7, 116, 7, 117, 1267, 28, 21, 45, 7, 46, 911, 30, 1, 23, 37, 24, 701, 30, 19, 15, 26, 16, 1735, 30, 5, 115, 10, 116, 1373, 28, 19, 47, 10, 48, 985, 30, 15, 24, 25, 25, 745, 30, 23, 15, 25, 16, 1843, 30, 13, 115, 3, 116, 1455, 28, 2, 46, 29, 47, 1033, 30, 42, 24, 1, 25, 793, 30, 23, 15, 28, 16, 1955, 30, 17, 115, 0, 0, 1541, 28, 10, 46, 23, 47, 1115, 30, 10, 24, 35, 25, 845, 30, 19, 15, 35, 16, 2071, 30, 17, 115, 1, 116, 1631, 28, 14, 46, 21, 47, 1171, 30, 29, 24, 19, 25, 901, 30, 11, 15, 46, 16, 2191, 30, 13, 115, 6, 116, 1725, 28, 14, 46, 23, 47, 1231, 30, 44, 24, 7, 25, 961, 30, 59, 16, 1, 17, 2306, 30, 12, 121, 7, 122, 1812, 28, 12, 47, 26, 48, 1286, 30, 39, 24, 14, 25, 986, 30, 22, 15, 41, 16, 2434, 30, 6, 121, 14, 122, 1914, 28, 6, 47, 34, 48, 1354, 30, 46, 24, 10, 25, 1054, 30, 2, 15, 64, 16, 2566, 30, 17, 122, 4, 123, 1992, 28, 29, 46, 14, 47, 1426, 30, 49, 24, 10, 25, 1096, 30, 24, 15, 46, 16, 2702, 30, 4, 122, 18, 123, 2102, 28, 13, 46, 32, 47, 1502, 30, 48, 24, 14, 25, 1142, 30, 42, 15, 32, 16, 2812, 30, 20, 117, 4, 118, 2216, 28, 40, 47, 7, 48, 1582, 30, 43, 24, 22, 25, 1222, 30, 10, 15, 67, 16, 2956, 30, 19, 118, 6, 119, 2334, 28, 18, 47, 31, 48, 1666, 30, 34, 24, 34, 25, 1276, 30, 20, 15, 61, 16];
    private static readonly int[] AlignmentPatternBaseValues = [0, 0, 0, 0, 0, 0, 0, 6, 18, 0, 0, 0, 0, 0, 6, 22, 0, 0, 0, 0, 0, 6, 26, 0, 0, 0, 0, 0, 6, 30, 0, 0, 0, 0, 0, 6, 34, 0, 0, 0, 0, 0, 6, 22, 38, 0, 0, 0, 0, 6, 24, 42, 0, 0, 0, 0, 6, 26, 46, 0, 0, 0, 0, 6, 28, 50, 0, 0, 0, 0, 6, 30, 54, 0, 0, 0, 0, 6, 32, 58, 0, 0, 0, 0, 6, 34, 62, 0, 0, 0, 0, 6, 26, 46, 66, 0, 0, 0, 6, 26, 48, 70, 0, 0, 0, 6, 26, 50, 74, 0, 0, 0, 6, 30, 54, 78, 0, 0, 0, 6, 30, 56, 82, 0, 0, 0, 6, 30, 58, 86, 0, 0, 0, 6, 34, 62, 90, 0, 0, 0, 6, 28, 50, 72, 94, 0, 0, 6, 26, 50, 74, 98, 0, 0, 6, 30, 54, 78, 102, 0, 0, 6, 28, 54, 80, 106, 0, 0, 6, 32, 58, 84, 110, 0, 0, 6, 30, 58, 86, 114, 0, 0, 6, 34, 62, 90, 118, 0, 0, 6, 26, 50, 74, 98, 122, 0, 6, 30, 54, 78, 102, 126, 0, 6, 26, 52, 78, 104, 130, 0, 6, 30, 56, 82, 108, 134, 0, 6, 34, 60, 86, 112, 138, 0, 6, 30, 58, 86, 114, 142, 0, 6, 34, 62, 90, 118, 146, 0, 6, 30, 54, 78, 102, 126, 150, 6, 24, 50, 76, 102, 128, 154, 6, 28, 54, 80, 106, 132, 158, 6, 32, 58, 84, 110, 136, 162, 6, 26, 54, 82, 110, 138, 166, 6, 30, 58, 86, 114, 142, 170];
    private static readonly int[] RemainderBits = [0, 7, 7, 7, 7, 7, 0, 0, 0, 0, 0, 0, 0, 3, 3, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4, 4, 4, 3, 3, 3, 3, 3, 3, 3, 0, 0, 0, 0, 0, 0];

    internal static List<AlignmentPattern> AlignmentPatternTable { get; } = CreateAlignmentPatternTable();
    internal static List<EccInfo> CapacityEccTable { get; } = CreateCapacityEccTable();
    internal static List<VersionInfo> CapacityTable { get; } = CreateCapacityTable();
    internal static List<Antilog> GaloisField { get; } = CreateAntilogTable();
    internal static Dictionary<char, int> AlphanumEncDict { get; } = CreateAlphanumEncDict();

    /// <summary>
    /// Calculates the QR code data which than can be used
    /// in one of the rendering classes to generate a graphical representation.
    /// </summary>
    /// <param name="plainText">The payload which shall be encoded in the QR code</param>
    /// <param name="eccLevel">The level of error correction data</param>
    /// <param name="forceUtf8">Shall the generator be forced to work in UTF-8 mode?</param>
    /// <param name="utf8Bom">Should the byte-order-mark be used?</param>
    /// <param name="eciMode">Which ECI mode shall be used?</param>
    /// <param name="requestedVersion">Set fixed QR code target version.</param>
    /// <exception cref="Exceptions.DataTooLongException">
    /// Thrown when the payload is too big to be encoded in a QR code.
    /// </exception>
    /// <returns>Returns the raw QR code data which can be used for rendering.</returns>
    public static QrCode Generate(
        string plainText,
        ErrorCorrectionLevel eccLevel,
        bool forceUtf8 = false,
        bool utf8Bom = false,
        ExtendedChannelInterpolationMode eciMode = ExtendedChannelInterpolationMode.Default,
        int requestedVersion = -1)
    {
        return GenerateQrCode(plainText, eccLevel, forceUtf8, utf8Bom, eciMode, requestedVersion);
    }

    /// <summary>
    /// Calculates the QR code data which than can be used in one of the rendering classes to generate a graphical representation.
    /// </summary>
    /// <param name="binaryData">A byte array which shall be encoded/stored in the QR code</param>
    /// <param name="eccLevel">The level of error correction data</param>
    /// <exception cref="Exceptions.DataTooLongException">Thrown when the payload is too big to be encoded in a QR code.</exception>
    /// <returns>Returns the raw QR code data which can be used for rendering.</returns>
    public static QrCode CreateQrCode(
        byte[] binaryData,
        ErrorCorrectionLevel eccLevel)
    {
        return GenerateQrCode(binaryData, eccLevel);
    }

    /// <summary>
    /// Calculates the QR code data which than can be used in one of the rendering classes to generate a graphical representation.
    /// </summary>
    /// <param name="plainText">The payload which shall be encoded in the QR code</param>
    /// <param name="eccLevel">The level of error correction data</param>
    /// <param name="forceUtf8">Shall the generator be forced to work in UTF-8 mode?</param>
    /// <param name="utf8Bom">Should the byte-order-mark be used?</param>
    /// <param name="eciMode">Which ECI mode shall be used?</param>
    /// <param name="requestedVersion">Set fixed QR code target version.</param>
    /// <exception cref="Exceptions.DataTooLongException">Thrown when the payload is too big to be encoded in a QR code.</exception>
    /// <returns>Returns the raw QR code data which can be used for rendering.</returns>
    public static QrCode GenerateQrCode(
        string plainText,
        ErrorCorrectionLevel eccLevel,
        bool forceUtf8 = false,
        bool utf8Bom = false,
        ExtendedChannelInterpolationMode eciMode = ExtendedChannelInterpolationMode.Default,
        int requestedVersion = -1)
    {
        plainText = plainText ?? throw new ArgumentNullException(nameof(plainText));
        
        EncodingMode encoding = GetEncodingFromPlaintext(plainText, forceUtf8);
        var codedText = PlainTextToBinary(plainText, encoding, eciMode, utf8Bom, forceUtf8);
        var dataInputLength = GetDataLength(encoding, plainText, codedText, forceUtf8);
        int version = requestedVersion;
        if (version == -1)
        {
            version = GetVersion(dataInputLength+(eciMode != ExtendedChannelInterpolationMode.Default?2:0), encoding, eccLevel);
        }
        else
        {
            //Version was passed as fixed version via parameter. Thus let's check if chosen version is valid.
            var minVersion = GetVersion(dataInputLength + (eciMode != ExtendedChannelInterpolationMode.Default ? 2 : 0), encoding, eccLevel);
            if (minVersion > version)
            {
                var maxSizeByte = CapacityTable[version - 1].Details.First(x => x.ErrorCorrectionLevel == eccLevel).CapacityDict[encoding];
                throw new Exceptions.DataTooLongException(eccLevel.ToString(), encoding.ToString(), version, maxSizeByte);
            }                    
        }

        string modeIndicator = String.Empty;
        if (eciMode != ExtendedChannelInterpolationMode.Default)
        {
            modeIndicator = DecToBin((int)EncodingMode.Eci, 4);
            modeIndicator += DecToBin((int)eciMode, 8);
        }
        modeIndicator += DecToBin((int)encoding, 4);
        var countIndicator = DecToBin(dataInputLength, GetCountIndicatorLength(version, encoding));
        var bitString = modeIndicator + countIndicator;

        bitString += codedText;

        return GenerateQrCode(bitString, eccLevel, version);
    }

    /// <summary>
    /// Calculates the QR code data which than can be used in one of the rendering classes to generate a graphical representation.
    /// </summary>
    /// <param name="binaryData">A byte array which shall be encoded/stored in the QR code</param>
    /// <param name="eccLevel">The level of error correction data</param>
    /// <exception cref="Exceptions.DataTooLongException">Thrown when the payload is too big to be encoded in a QR code.</exception>
    /// <returns>Returns the raw QR code data which can be used for rendering.</returns>
    public static QrCode GenerateQrCode(
        byte[] binaryData,
        ErrorCorrectionLevel eccLevel)
    {
        binaryData = binaryData ?? throw new ArgumentNullException(nameof(binaryData));
        
        int version = GetVersion(binaryData.Length, EncodingMode.Byte, eccLevel);

        string modeIndicator = DecToBin((int)EncodingMode.Byte, 4);
        string countIndicator = DecToBin(binaryData.Length, GetCountIndicatorLength(version, EncodingMode.Byte));

        string bitString = modeIndicator + countIndicator;
        foreach (byte b in binaryData)
        {
            bitString += DecToBin(b, 8);
        }

        return GenerateQrCode(bitString, eccLevel, version);
    }

    private static QrCode GenerateQrCode(string bitString, ErrorCorrectionLevel eccLevel, int version)
    {
            //Fill up data code word
            var eccInfo = CapacityEccTable.Single(x => x.Version == version && x.ErrorCorrectionLevel == eccLevel);
            var dataLength = eccInfo.TotalDataCodewords * 8;
            var lengthDiff = dataLength - bitString.Length;
            if (lengthDiff > 0)
                bitString += new string('0', Math.Min(lengthDiff, 4));
            if ((bitString.Length % 8) != 0)
                bitString += new string('0', 8 - (bitString.Length % 8));
            while (bitString.Length < dataLength)
                bitString += "1110110000010001";
            if (bitString.Length > dataLength)
                bitString = bitString.Substring(0, dataLength);

            //Calculate error correction words
            var codeWordWithEcc = new List<CodewordBlock>(
                eccInfo.BlocksInGroup1 + eccInfo.BlocksInGroup2);
            for (var i = 0; i < eccInfo.BlocksInGroup1; i++)
            {
                var bitStr = bitString.Substring(i * eccInfo.CodewordsInGroup1 * 8, eccInfo.CodewordsInGroup1 * 8);
                var bitBlockList = BinaryStringToBitBlockList(bitStr);
                var bitBlockListDec = BinaryStringListToDecList(bitBlockList);
                var eccWordList = CalculateEccWords(bitStr, eccInfo);
                var eccWordListDec = BinaryStringListToDecList(eccWordList);
                codeWordWithEcc.Add(
                    new CodewordBlock(1,
                                      i + 1,
                                      bitStr,
                                      bitBlockList,
                                      eccWordList,
                                      bitBlockListDec,
                                      eccWordListDec)
                                );
            }
            bitString = bitString.Substring(eccInfo.BlocksInGroup1 * eccInfo.CodewordsInGroup1 * 8);
            for (var i = 0; i < eccInfo.BlocksInGroup2; i++)
            {
                var bitStr = bitString.Substring(i * eccInfo.CodewordsInGroup2 * 8, eccInfo.CodewordsInGroup2 * 8);
                var bitBlockList = BinaryStringToBitBlockList(bitStr);
                var bitBlockListDec = BinaryStringListToDecList(bitBlockList);
                var eccWordList = CalculateEccWords(bitStr, eccInfo);
                var eccWordListDec = BinaryStringListToDecList(eccWordList);
                codeWordWithEcc.Add(new CodewordBlock(
                    groupNumber: 2,
                    blockNumber: i + 1,
                    bitString: bitStr,
                    codeWords: bitBlockList,
                    eccWords: eccWordList,
                    codeWordsInt: bitBlockListDec,
                    eccWordsInt: eccWordListDec)
                );
            }


            //Interleave code words
            var interleavedWordsSb = new StringBuilder();
            for (var i = 0; i < Math.Max(eccInfo.CodewordsInGroup1, eccInfo.CodewordsInGroup2); i++)
            {
                foreach (var codeBlock in codeWordWithEcc)
                    if (codeBlock.CodeWords.Count > i)
                        interleavedWordsSb.Append(codeBlock.CodeWords[i]);
            }


            for (var i = 0; i < eccInfo.EccPerBlock; i++)
            {
                foreach (var codeBlock in codeWordWithEcc)
                    if (codeBlock.EccWords.Count > i)
                        interleavedWordsSb.Append(codeBlock.EccWords[i]);
            }
            interleavedWordsSb.Append(new string('0', RemainderBits[version - 1]));
            var interleavedData = interleavedWordsSb.ToString();


            //Place interleaved data on module matrix
            var qr = new QrCode(version);
            var blockedModules = new List<Rectangle>();
            ModulePlacer.PlaceFinderPatterns(ref qr, ref blockedModules);
            ModulePlacer.ReserveSeparatorAreas(qr.ModuleMatrix.Count, ref blockedModules);
            ModulePlacer.PlaceAlignmentPatterns(ref qr, AlignmentPatternTable.Where(x => x.Version == version).Select(x => x.PatternPositions).First(), ref blockedModules);
            ModulePlacer.PlaceTimingPatterns(ref qr, ref blockedModules);
            ModulePlacer.PlaceDarkModule(ref qr, version, ref blockedModules);
            ModulePlacer.ReserveVersionAreas(qr.ModuleMatrix.Count, version, ref blockedModules);
            ModulePlacer.PlaceDataWords(ref qr, interleavedData, ref blockedModules);
            var maskVersion = ModulePlacer.MaskCode(ref qr, version, ref blockedModules, eccLevel);
            var formatStr = GetFormatString(eccLevel, maskVersion);

            ModulePlacer.PlaceFormat(ref qr, formatStr);
            if (version >= 7)
            {
                var versionString = GetVersionString(version);
                ModulePlacer.PlaceVersion(ref qr, versionString);
            }


            ModulePlacer.AddQuietZone(ref qr);
            return qr;
        }

    private static string GetFormatString(ErrorCorrectionLevel level, int maskVersion)
    {
            var generator = "10100110111";
            var fStrMask = "101010000010010";

            var fStr = (level == ErrorCorrectionLevel.Low) ? "01" : (level == ErrorCorrectionLevel.Medium) ? "00" : (level == ErrorCorrectionLevel.Quartile) ? "11" : "10";
            fStr += DecToBin(maskVersion, 3);
            var fStrEcc = fStr.PadRight(15, '0').TrimStart('0');
            while (fStrEcc.Length > 10)
            {
                var sb = new StringBuilder();
                generator = generator.PadRight(fStrEcc.Length, '0');
                for (var i = 0; i < fStrEcc.Length; i++)
                    sb.Append((Convert.ToInt32(fStrEcc[i]) ^ Convert.ToInt32(generator[i])).ToString(CultureInfo.InvariantCulture));
                fStrEcc = sb.ToString().TrimStart('0');
            }
            fStrEcc = fStrEcc.PadLeft(10, '0');
            fStr += fStrEcc;

            var sbMask = new StringBuilder();
            for (var i = 0; i < fStr.Length; i++)
                sbMask.Append((Convert.ToInt32(fStr[i]) ^ Convert.ToInt32(fStrMask[i])).ToString(CultureInfo.InvariantCulture));
            return sbMask.ToString();
        }

    private static string GetVersionString(int version)
    {
            var generator = "1111100100101";

            var vStr = DecToBin(version, 6);
            var vStrEcc = vStr.PadRight(18, '0').TrimStart('0');
            while (vStrEcc.Length > 12)
            {
                var sb = new StringBuilder();
                generator = generator.PadRight(vStrEcc.Length, '0');
                for (var i = 0; i < vStrEcc.Length; i++)
                    sb.Append((Convert.ToInt32(vStrEcc[i]) ^ Convert.ToInt32(generator[i])).ToString(CultureInfo.InvariantCulture));
                vStrEcc = sb.ToString().TrimStart('0');
            }
            vStrEcc = vStrEcc.PadLeft(12, '0');
            vStr += vStrEcc;

            return vStr;
        }

    private static class ModulePlacer
    {
        public static void AddQuietZone(ref QrCode qrCode)
        {
            var quietLine = new bool[qrCode.ModuleMatrix.Count + 8];
            for (var i = 0; i < quietLine.Length; i++)
                quietLine[i] = false;
            for (var i = 0; i < 4; i++)
                qrCode.ModuleMatrix.Insert(0, new BitArray(quietLine));
            for (var i = 0; i < 4; i++)
                qrCode.ModuleMatrix.Add(new BitArray(quietLine));
            for (var i = 4; i < qrCode.ModuleMatrix.Count - 4; i++)
            {
                bool[] quietPart = [false, false, false, false];
                var tmpLine = new List<bool>(quietPart);
                tmpLine.AddRange(qrCode.ModuleMatrix[i].Cast<bool>());
                tmpLine.AddRange(quietPart);
                qrCode.ModuleMatrix[i] = new BitArray(tmpLine.ToArray());
            }
        }

        private static string ReverseString(string inp)
        {
                string newStr = string.Empty;
                if (inp.Length > 0)
                {
                    for (int i = inp.Length - 1; i >= 0; i--)
                        newStr += inp[i];
                }
                return newStr;
            }

        public static void PlaceVersion(ref QrCode qrCode, string versionStr)
        {
            var size = qrCode.ModuleMatrix.Count;

            var vStr = ReverseString(versionStr);

            for (var x = 0; x < 6; x++)
            {
                for (var y = 0; y < 3; y++)
                {
                    qrCode.ModuleMatrix[y + size - 11][x] = vStr[x * 3 + y] == '1';
                    qrCode.ModuleMatrix[x][y + size - 11] = vStr[x * 3 + y] == '1';
                }
            }
        }

        public static void PlaceFormat(ref QrCode qrCode, string formatStr)
        {
            var size = qrCode.ModuleMatrix.Count;
            var fStr = ReverseString(formatStr);
            var modules = new[,] {
                { 8, 0, size - 1, 8 },
                { 8, 1, size - 2, 8 },
                { 8, 2, size - 3, 8 },
                { 8, 3, size - 4, 8 },
                { 8, 4, size - 5, 8 },
                { 8, 5, size - 6, 8 },
                { 8, 7, size - 7, 8 },
                { 8, 8, size - 8, 8 },
                { 7, 8, 8, size - 7 },
                { 5, 8, 8, size - 6 },
                { 4, 8, 8, size - 5 },
                { 3, 8, 8, size - 4 },
                { 2, 8, 8, size - 3 },
                { 1, 8, 8, size - 2 },
                { 0, 8, 8, size - 1 } };
            for (var i = 0; i < 15; i++)
            {
                var p1 = new Point(modules[i, 0], modules[i, 1]);
                var p2 = new Point(modules[i, 2], modules[i, 3]);
                qrCode.ModuleMatrix[p1.Y][p1.X] = fStr[i] == '1';
                qrCode.ModuleMatrix[p2.Y][p2.X] = fStr[i] == '1';
            }
        }

        public static int MaskCode(ref QrCode qrCode, int version, ref List<Rectangle> blockedModules, ErrorCorrectionLevel eccLevel)
        {
            int? selectedPattern = null;
            var patternScore = 0;

            var size = qrCode.ModuleMatrix.Count;

            var methods = new Dictionary<int, Func<int, int, bool>>(8) {
                { 1, MaskPattern.Pattern1 }, {2, MaskPattern.Pattern2 }, {3, MaskPattern.Pattern3 }, {4, MaskPattern.Pattern4 },
                {5,  MaskPattern.Pattern5 }, {6, MaskPattern.Pattern6 }, {7, MaskPattern.Pattern7 }, {8, MaskPattern.Pattern8 }
            };

            foreach (var pattern in methods)
            {
                var qrTemp = new QrCode(version);
                for (var y = 0; y < size; y++)
                {
                    for (var x = 0; x < size; x++)
                    {
                        qrTemp.ModuleMatrix[y][x] = qrCode.ModuleMatrix[y][x];
                    }

                }

                var formatStr = GetFormatString(eccLevel, pattern.Key - 1);
                PlaceFormat(ref qrTemp, formatStr);
                if (version >= 7)
                {
                    var versionString = GetVersionString(version);
                    PlaceVersion(ref qrTemp, versionString);
                }

                for (var x = 0; x < size; x++)
                {
                    for (var y = 0; y < x; y++)
                    {
                        if (!IsBlocked(new Rectangle(x, y, 1, 1), blockedModules))
                        {
                            qrTemp.ModuleMatrix[y][x] ^= pattern.Value(x, y);
                            qrTemp.ModuleMatrix[x][y] ^= pattern.Value(y, x);
                        }
                    }

                    if (!IsBlocked(new Rectangle(x, x, 1, 1), blockedModules))
                    {
                        qrTemp.ModuleMatrix[x][x] ^= pattern.Value(x, x);
                    }
                }

                var score = MaskPattern.Score(ref qrTemp);
                if (!selectedPattern.HasValue || patternScore > score)
                {
                    selectedPattern = pattern.Key;
                    patternScore = score;
                }
            }

            for (var x = 0; x < size; x++)
            {
                for (var y = 0; y < x; y++)
                {
                    if (!IsBlocked(new Rectangle(x, y, 1, 1), blockedModules))
                    {
                        qrCode.ModuleMatrix[y][x] ^= methods[selectedPattern!.Value](x, y);
                        qrCode.ModuleMatrix[x][y] ^= methods[selectedPattern.Value](y, x);
                    }
                }

                if (!IsBlocked(new Rectangle(x, x, 1, 1), blockedModules))
                {
                    qrCode.ModuleMatrix[x][x] ^= methods[selectedPattern!.Value](x, x);
                }
            }
            
            return selectedPattern!.Value - 1;
        }

        public static void PlaceDataWords(ref QrCode qrCode, string data, ref List<Rectangle> blockedModules)
        {
            var size = qrCode.ModuleMatrix.Count;
            var up = true;
            var dataWords = new Queue<bool>();
            foreach (var t in data)
            {
                dataWords.Enqueue(t != '0');
            }
            for (var x = size - 1; x >= 0; x = x - 2)
            {
                if (x == 6)
                    x = 5;
                for (var yMod = 1; yMod <= size; yMod++)
                {
                    int y;
                    if (up)
                    {
                        y = size - yMod;
                        if (dataWords.Count > 0 && !IsBlocked(new Rectangle(x, y, 1, 1), blockedModules))
                            qrCode.ModuleMatrix[y][x] = dataWords.Dequeue();
                        if (dataWords.Count > 0 && x > 0 && !IsBlocked(new Rectangle(x - 1, y, 1, 1), blockedModules))
                            qrCode.ModuleMatrix[y][x - 1] = dataWords.Dequeue();
                    }
                    else
                    {
                        y = yMod - 1;
                        if (dataWords.Count > 0 && !IsBlocked(new Rectangle(x, y, 1, 1), blockedModules))
                            qrCode.ModuleMatrix[y][x] = dataWords.Dequeue();
                        if (dataWords.Count > 0 && x > 0 && !IsBlocked(new Rectangle(x - 1, y, 1, 1), blockedModules))
                            qrCode.ModuleMatrix[y][x - 1] = dataWords.Dequeue();
                    }
                }
                up = !up;
            }
        }

        public static void ReserveSeparatorAreas(int size, ref List<Rectangle> blockedModules)
        {
            blockedModules.AddRange(new[]{
                new Rectangle(7, 0, 1, 8),
                new Rectangle(0, 7, 7, 1),
                new Rectangle(0, size-8, 8, 1),
                new Rectangle(7, size-7, 1, 7),
                new Rectangle(size-8, 0, 1, 8),
                new Rectangle(size-7, 7, 7, 1)
            });
        }

        public static void ReserveVersionAreas(int size, int version, ref List<Rectangle> blockedModules)
        {
            blockedModules.AddRange(new[]{
                new Rectangle(8, 0, 1, 6),
                new Rectangle(8, 7, 1, 1),
                new Rectangle(0, 8, 6, 1),
                new Rectangle(7, 8, 2, 1),
                new Rectangle(size-8, 8, 8, 1),
                new Rectangle(8, size-7, 1, 7)
            });

            if (version >= 7)
            {
                blockedModules.AddRange(new[]{
                    new Rectangle(size-11, 0, 3, 6),
                    new Rectangle(0, size-11, 6, 3)
                });
            }
        }
        
        public static void PlaceDarkModule(ref QrCode qrCode, int version, ref List<Rectangle> blockedModules)
        {
            qrCode.ModuleMatrix[4 * version + 9][8] = true;
            blockedModules.Add(new Rectangle(8, 4 * version + 9, 1, 1));
        }

        public static void PlaceFinderPatterns(ref QrCode qrCode, ref List<Rectangle> blockedModules)
        {
            var size = qrCode.ModuleMatrix.Count;
            int[] locations = [0, 0, size - 7, 0, 0, size - 7];

            for (var i = 0; i < 6; i = i + 2)
            {
                for (var x = 0; x < 7; x++)
                {
                    for (var y = 0; y < 7; y++)
                    {
                        if (!((x is 1 or 5 && y is > 0 and < 6) ||
                              (x is > 0 and < 6 && y is 1 or 5)))
                        {
                            qrCode.ModuleMatrix[y + locations[i + 1]][x + locations[i]] = true;
                        }
                    }
                }
                blockedModules.Add(new Rectangle(locations[i], locations[i + 1], 7, 7));
            }
        }

        public static void PlaceAlignmentPatterns(ref QrCode qrCode, List<Point> alignmentPatternLocations, ref List<Rectangle> blockedModules)
        {
            foreach (var loc in alignmentPatternLocations)
            {
                var alignmentPatternRect = new Rectangle(loc.X, loc.Y, 5, 5);
                var blocked = false;
                foreach (var blockedRect in blockedModules)
                {
                    if (Intersects(alignmentPatternRect, blockedRect))
                    {
                        blocked = true;
                        break;
                    }
                }
                if (blocked)
                    continue;

                for (var x = 0; x < 5; x++)
                {
                    for (var y = 0; y < 5; y++)
                    {
                        if (y == 0 || y == 4 || x == 0 || x == 4 || (x == 2 && y == 2))
                        {
                            qrCode.ModuleMatrix[loc.Y + y][loc.X + x] = true;
                        }
                    }
                }
                blockedModules.Add(new Rectangle(loc.X, loc.Y, 5, 5));
            }
        }

        public static void PlaceTimingPatterns(ref QrCode qrCode, ref List<Rectangle> blockedModules)
        {
                var size = qrCode.ModuleMatrix.Count;
                for (var i = 8; i < size - 8; i++)
                {
                    if (i % 2 == 0)
                    {
                        qrCode.ModuleMatrix[6][i] = true;
                        qrCode.ModuleMatrix[i][6] = true;
                    }
                }
                blockedModules.AddRange(new[]{
                    new Rectangle(6, 8, 1, size-16),
                    new Rectangle(8, 6, size-16, 1)
                });
            }

        private static bool Intersects(Rectangle r1, Rectangle r2)
        {
                return r2.X < r1.X + r1.Width && r1.X < r2.X + r2.Width && r2.Y < r1.Y + r1.Height && r1.Y < r2.Y + r2.Height;
            }

        private static bool IsBlocked(Rectangle r1, List<Rectangle> blockedModules)
        {
                foreach (var blockedMod in blockedModules)
                {
                    if (Intersects(blockedMod, r1))
                        return true;
                }
                return false;
            }

        private static class MaskPattern
        {
            public static bool Pattern1(int x, int y)
            {
                    return (x + y) % 2 == 0;
                }

            public static bool Pattern2(int x, int y)
            {
                    return y % 2 == 0;
                }

            public static bool Pattern3(int x, int y)
            {
                    return x % 3 == 0;
                }

            public static bool Pattern4(int x, int y)
            {
                    return (x + y) % 3 == 0;
                }

            public static bool Pattern5(int x, int y)
            {
                    return ((int)(Math.Floor(y / 2d) + Math.Floor(x / 3d)) % 2) == 0;
                }

            public static bool Pattern6(int x, int y)
            {
                    return ((x * y) % 2) + ((x * y) % 3) == 0;
                }

            public static bool Pattern7(int x, int y)
            {
                    return (((x * y) % 2) + ((x * y) % 3)) % 2 == 0;
                }

            public static bool Pattern8(int x, int y)
            {
                    return (((x + y) % 2) + ((x * y) % 3)) % 2 == 0;
                }

            public static int Score(ref QrCode qrCode)
            {
                int score1 = 0,
                    score2 = 0,
                    score3 = 0;
                var size = qrCode.ModuleMatrix.Count;

                //Penalty 1
                for (var y = 0; y < size; y++)
                {
                    var modInRow = 0;
                    var modInColumn = 0;
                    var lastValRow = qrCode.ModuleMatrix[y][0];
                    var lastValColumn = qrCode.ModuleMatrix[0][y];
                    for (var x = 0; x < size; x++)
                    {
                        if (qrCode.ModuleMatrix[y][x] == lastValRow)
                            modInRow++;
                        else
                            modInRow = 1;
                        if (modInRow == 5)
                            score1 += 3;
                        else if (modInRow > 5)
                            score1++;
                        lastValRow = qrCode.ModuleMatrix[y][x];


                        if (qrCode.ModuleMatrix[x][y] == lastValColumn)
                            modInColumn++;
                        else
                            modInColumn = 1;
                        if (modInColumn == 5)
                            score1 += 3;
                        else if (modInColumn > 5)
                            score1++;
                        lastValColumn = qrCode.ModuleMatrix[x][y];
                    }
                }


                //Penalty 2
                for (var y = 0; y < size - 1; y++)
                {
                    for (var x = 0; x < size - 1; x++)
                    {
                        if (qrCode.ModuleMatrix[y][x] == qrCode.ModuleMatrix[y][x + 1] &&
                            qrCode.ModuleMatrix[y][x] == qrCode.ModuleMatrix[y + 1][x] &&
                            qrCode.ModuleMatrix[y][x] == qrCode.ModuleMatrix[y + 1][x + 1])
                            score2 += 3;
                    }
                }

                //Penalty 3
                for (var y = 0; y < size; y++)
                {
                    for (var x = 0; x < size - 10; x++)
                    {
                        if ((qrCode.ModuleMatrix[y][x] &&
                            !qrCode.ModuleMatrix[y][x + 1] &&
                            qrCode.ModuleMatrix[y][x + 2] &&
                            qrCode.ModuleMatrix[y][x + 3] &&
                            qrCode.ModuleMatrix[y][x + 4] &&
                            !qrCode.ModuleMatrix[y][x + 5] &&
                            qrCode.ModuleMatrix[y][x + 6] &&
                            !qrCode.ModuleMatrix[y][x + 7] &&
                            !qrCode.ModuleMatrix[y][x + 8] &&
                            !qrCode.ModuleMatrix[y][x + 9] &&
                            !qrCode.ModuleMatrix[y][x + 10]) ||
                            (!qrCode.ModuleMatrix[y][x] &&
                            !qrCode.ModuleMatrix[y][x + 1] &&
                            !qrCode.ModuleMatrix[y][x + 2] &&
                            !qrCode.ModuleMatrix[y][x + 3] &&
                            qrCode.ModuleMatrix[y][x + 4] &&
                            !qrCode.ModuleMatrix[y][x + 5] &&
                            qrCode.ModuleMatrix[y][x + 6] &&
                            qrCode.ModuleMatrix[y][x + 7] &&
                            qrCode.ModuleMatrix[y][x + 8] &&
                            !qrCode.ModuleMatrix[y][x + 9] &&
                            qrCode.ModuleMatrix[y][x + 10]))
                        {
                            score3 += 40;
                        }

                        if ((qrCode.ModuleMatrix[x][y] &&
                            !qrCode.ModuleMatrix[x + 1][y] &&
                            qrCode.ModuleMatrix[x + 2][y] &&
                            qrCode.ModuleMatrix[x + 3][y] &&
                            qrCode.ModuleMatrix[x + 4][y] &&
                            !qrCode.ModuleMatrix[x + 5][y] &&
                            qrCode.ModuleMatrix[x + 6][y] &&
                            !qrCode.ModuleMatrix[x + 7][y] &&
                            !qrCode.ModuleMatrix[x + 8][y] &&
                            !qrCode.ModuleMatrix[x + 9][y] &&
                            !qrCode.ModuleMatrix[x + 10][y]) ||
                            (!qrCode.ModuleMatrix[x][y] &&
                            !qrCode.ModuleMatrix[x + 1][y] &&
                            !qrCode.ModuleMatrix[x + 2][y] &&
                            !qrCode.ModuleMatrix[x + 3][y] &&
                            qrCode.ModuleMatrix[x + 4][y] &&
                            !qrCode.ModuleMatrix[x + 5][y] &&
                            qrCode.ModuleMatrix[x + 6][y] &&
                            qrCode.ModuleMatrix[x + 7][y] &&
                            qrCode.ModuleMatrix[x + 8][y] &&
                            !qrCode.ModuleMatrix[x + 9][y] &&
                            qrCode.ModuleMatrix[x + 10][y]))
                        {
                            score3 += 40;
                        }
                    }
                }

                //Penalty 4
                double blackModules = 0;
                foreach (var row in qrCode.ModuleMatrix)
                    foreach (bool bit in row)
                        if (bit)
                            blackModules++;

                var percent = (blackModules / (qrCode.ModuleMatrix.Count * qrCode.ModuleMatrix.Count)) * 100;
                var prevMultipleOf5 = Math.Abs((int) Math.Floor(percent/5) * 5 - 50) / 5;
                var nextMultipleOf5 = Math.Abs((int)Math.Floor(percent / 5) * 5 -45) / 5;
                var score4 = Math.Min(prevMultipleOf5, nextMultipleOf5) * 10;

                return score1 + score2 + score3 + score4;
            }
        }
    }

    private static List<string> CalculateEccWords(string bitString, EccInfo eccInfo)
    {
            var eccWords = eccInfo.EccPerBlock;
            var messagePolynom = CalculateMessagePolynom(bitString);
            var generatorPolynom = CalculateGeneratorPolynom(eccWords);

            for (var i = 0; i < messagePolynom.PolyItems.Count; i++)
                messagePolynom.PolyItems[i] = new PolynomItem(messagePolynom.PolyItems[i].Coefficient,
                    messagePolynom.PolyItems[i].Exponent + eccWords);

            for (var i = 0; i < generatorPolynom.PolyItems.Count; i++)
                generatorPolynom.PolyItems[i] = new PolynomItem(generatorPolynom.PolyItems[i].Coefficient,
                    generatorPolynom.PolyItems[i].Exponent + (messagePolynom.PolyItems.Count-1));

            var leadTermSource = messagePolynom;
            for (var i = 0; (leadTermSource.PolyItems.Count > 0 && leadTermSource.PolyItems[leadTermSource.PolyItems.Count - 1].Exponent > 0); i++)
            {
                if (leadTermSource.PolyItems[0].Coefficient == 0)
                {
                    leadTermSource.PolyItems.RemoveAt(0);
                    leadTermSource.PolyItems.Add(new PolynomItem(0, leadTermSource.PolyItems[leadTermSource.PolyItems.Count - 1].Exponent - 1));
                }
                else
                {
                    var resPoly = MultiplyGeneratorPolynomByLeadTerm(generatorPolynom, ConvertToAlphaNotation(leadTermSource).PolyItems[0], i);
                    resPoly = ConvertToDecNotation(resPoly);
                    resPoly = XorPolynoms(leadTermSource, resPoly);
                    leadTermSource = resPoly;
                }
            }
            return leadTermSource.PolyItems.Select(x => DecToBin(x.Coefficient, 8)).ToList();
        }

    private static Polynom ConvertToAlphaNotation(Polynom poly)
    {
        var newPoly = new Polynom();
        for (var i = 0; i < poly.PolyItems.Count; i++)
            newPoly.PolyItems.Add(
                new PolynomItem(
                    (poly.PolyItems[i].Coefficient != 0
                        ? GetAlphaExpFromIntVal(poly.PolyItems[i].Coefficient)
                        : 0), poly.PolyItems[i].Exponent));
        return newPoly;
    }

    private static Polynom ConvertToDecNotation(Polynom poly)
    {
            var newPoly = new Polynom();
            for (var i = 0; i < poly.PolyItems.Count; i++)
                newPoly.PolyItems.Add(new PolynomItem(GetIntValFromAlphaExp(poly.PolyItems[i].Coefficient), poly.PolyItems[i].Exponent));
            return newPoly;
        }

    private static int GetVersion(int length, EncodingMode encMode, ErrorCorrectionLevel eccLevel)
    {
        var fittingVersions = CapacityTable
            .Where(x => x.Details.Any(
                y => (y.ErrorCorrectionLevel == eccLevel
                      && y.CapacityDict[encMode] >= Convert.ToInt32(length)
                      )
                ))
            .Select(x => new
            {
                version = x.Version,
                capacity = x.Details
                    .Single(y => y.ErrorCorrectionLevel == eccLevel)
                    .CapacityDict[encMode]
            })
            .ToArray();

        if (fittingVersions.Length != 0)
            return fittingVersions.Min(x => x.version);

        var maxSizeByte = CapacityTable
            .Where(x => x.Details.Any(
                y => (y.ErrorCorrectionLevel == eccLevel))
            )
            .Max(x => x.Details.Single(y => y.ErrorCorrectionLevel == eccLevel).CapacityDict[encMode]);
        throw new Exceptions.DataTooLongException(eccLevel.ToString(), encMode.ToString(), maxSizeByte);
    }

    internal static EncodingMode GetEncodingFromPlaintext(string plainText, bool forceUtf8)
    {
        if (forceUtf8) return EncodingMode.Byte;
        EncodingMode result = EncodingMode.Numeric; // assume numeric
        foreach (char c in plainText)
        {
            if (IsInRange(c, '0', '9')) continue;   // numeric - char.IsDigit() for Latin1
            result = EncodingMode.Alphanumeric;     // not numeric, assume alphanumeric
            if (IsInRange(c, 'A', 'Z') || AlphanumEncTable.Contains(c)) continue; // alphanumeric
            return EncodingMode.Byte;               // not numeric or alphanumeric, assume byte
        }
        return result;                              // either numeric or alphanumeric
    }

    private static bool IsInRange(char c, char min, char max)
    {
            return (uint)(c - min) <= (uint)(max - min);
        }

    private static Polynom CalculateMessagePolynom(string bitString)
    {
            var messagePol = new Polynom();
            for (var i = bitString.Length / 8 - 1; i >= 0; i--)
            {
                messagePol.PolyItems.Add(new PolynomItem(BinToDec(bitString.Substring(0, 8)), i));
                bitString = bitString.Remove(0, 8);
            }
            return messagePol;
        }


    private static Polynom CalculateGeneratorPolynom(int numEccWords)
    {
            var generatorPolynom = new Polynom();
            generatorPolynom.PolyItems.AddRange(new[]{
                new PolynomItem(0,1),
                new PolynomItem(0,0)
            });
            for (var i = 1; i <= numEccWords - 1; i++)
            {
                var multiplierPolynom = new Polynom();
                multiplierPolynom.PolyItems.AddRange(new[]{
                   new PolynomItem(0,1),
                new PolynomItem(i,0)
                });

                generatorPolynom = MultiplyAlphaPolynoms(generatorPolynom, multiplierPolynom);
            }

            return generatorPolynom;
        }

    private static List<string> BinaryStringToBitBlockList(string bitString)
    {
            const int blockSize = 8;
            var numberOfBlocks = (int)Math.Ceiling(bitString.Length / (double)blockSize);
            var blocklist = new List<string>(numberOfBlocks);

            for (int i = 0; i < bitString.Length; i += blockSize)
            {
                blocklist.Add(bitString.Substring(i, blockSize));
            }

            return blocklist;
        }

    private static List<int> BinaryStringListToDecList(List<string> binaryStringList)
    {
        return binaryStringList.Select(BinToDec).ToList();
    }

    private static int BinToDec(string binStr)
    {
        return Convert.ToInt32(binStr, 2);
    }

    private static string DecToBin(int decNum)
    {
        return Convert.ToString(decNum, 2);
    }

    private static string DecToBin(int decNum, int padLeftUpTo)
    {
        var binStr = DecToBin(decNum);
        return binStr.PadLeft(padLeftUpTo, '0');
    }

    private static int GetCountIndicatorLength(int version, EncodingMode encMode)
    {
        if (version < 10)
        {
            if (encMode == EncodingMode.Numeric)
                return 10;
            else if (encMode == EncodingMode.Alphanumeric)
                return 9;
            else
                return 8;
        }
        else if (version < 27)
        {
            if (encMode == EncodingMode.Numeric)
                return 12;
            else if (encMode == EncodingMode.Alphanumeric)
                return 11;
            else if (encMode == EncodingMode.Byte)
                return 16;
            else
                return 10;
        }
        else
        {
            if (encMode == EncodingMode.Numeric)
                return 14;
            else if (encMode == EncodingMode.Alphanumeric)
                return 13;
            else if (encMode == EncodingMode.Byte)
                return 16;
            else
                return 12;
        }
    }

    private static int GetDataLength(EncodingMode encoding, string plainText, string codedText, bool forceUtf8)
    {
            return forceUtf8 || IsUtf8(encoding, plainText, forceUtf8) ? (codedText.Length / 8) : plainText.Length;
        }

    private static bool IsUtf8(EncodingMode encoding, string plainText, bool forceUtf8)
    {
        return (encoding == EncodingMode.Byte && (!IsValidIso(plainText) || forceUtf8));
    }

    private static bool IsValidIso(string input)
    {
        var bytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(input);
        //var result = Encoding.GetEncoding("ISO-8859-1").GetString(bytes);
        var result = Encoding.GetEncoding("ISO-8859-1").GetString(bytes,0,bytes.Length);
        
        return string.Equals(input, result, StringComparison.Ordinal);
    }

    private static string PlainTextToBinary(string plainText, EncodingMode encMode, ExtendedChannelInterpolationMode eciMode, bool utf8Bom, bool forceUtf8)
    {
            switch(encMode)
            {
                case EncodingMode.Alphanumeric:
                    return PlainTextToBinaryAlphanumeric(plainText);
                case EncodingMode.Numeric:
                    return PlainTextToBinaryNumeric(plainText);
                case EncodingMode.Byte:
                    return PlainTextToBinaryByte(plainText, eciMode, utf8Bom, forceUtf8);
                case EncodingMode.Kanji:
                    return string.Empty;
                case EncodingMode.Eci:
                default:
                    return string.Empty;
            }
        }

    private static string PlainTextToBinaryNumeric(string plainText)
    {
            var codeText = string.Empty;
            while (plainText.Length >= 3)
            {
                var dec = Convert.ToInt32(plainText.Substring(0, 3), CultureInfo.InvariantCulture);
                codeText += DecToBin(dec, 10);
                plainText = plainText.Substring(3);

            }
            if (plainText.Length == 2)
            {
                var dec = Convert.ToInt32(plainText, CultureInfo.InvariantCulture);
                codeText += DecToBin(dec, 7);
            }
            else if (plainText.Length == 1)
            {
                var dec = Convert.ToInt32(plainText, CultureInfo.InvariantCulture);
                codeText += DecToBin(dec, 4);
            }
            return codeText;
        }

    private static string PlainTextToBinaryAlphanumeric(string plainText)
    {
            var codeText = string.Empty;
            while (plainText.Length >= 2)
            {
                var token = plainText.Substring(0, 2);
                var dec = AlphanumEncDict[token[0]] * 45 + AlphanumEncDict[token[1]];
                codeText += DecToBin(dec, 11);
                plainText = plainText.Substring(2);

            }
            if (plainText.Length > 0)
            {
                codeText += DecToBin(AlphanumEncDict[plainText[0]], 6);
            }
            return codeText;
        }

    // ReSharper disable once UnusedMember.Local
    private static string PlainTextToBinaryEci(string plainText)
    {
        var codeText = string.Empty;
        var bytes = Encoding.GetEncoding("ascii").GetBytes(plainText);
        foreach(var @byte in bytes)
        {
            codeText += DecToBin(@byte, 8);
        }
        
        return codeText;
    }

    private static string ConvertToIso8859(string value, string iso = "ISO-8859-2")
    {
        var isoEncoding = Encoding.GetEncoding(iso);
        var isoBytes = Encoding.Convert(
            srcEncoding: Encoding.UTF8,
            dstEncoding: isoEncoding,
            bytes: Encoding.UTF8.GetBytes(value));
        
        return isoEncoding.GetString(isoBytes);
    }

    private static string PlainTextToBinaryByte(string plainText, ExtendedChannelInterpolationMode eciMode, bool utf8Bom, bool forceUtf8)
    {
        byte[] codeBytes;
        var codeText = string.Empty;

        if (IsValidIso(plainText) && !forceUtf8)
            codeBytes = Encoding.GetEncoding("ISO-8859-1").GetBytes(plainText);
        else
        {
            switch(eciMode)
            {
                case ExtendedChannelInterpolationMode.Iso8859Part1:
                    codeBytes = Encoding.GetEncoding("ISO-8859-1")
                        .GetBytes(ConvertToIso8859(plainText, iso: "ISO-8859-1"));
                    break;
                case ExtendedChannelInterpolationMode.Iso8859Part2:
                    codeBytes = Encoding.GetEncoding("ISO-8859-2")
                        .GetBytes(ConvertToIso8859(plainText, iso: "ISO-8859-2"));
                    break;
                case ExtendedChannelInterpolationMode.Default:
                case ExtendedChannelInterpolationMode.Utf8:
                default:
                    codeBytes = utf8Bom ? Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(plainText)).ToArray() : Encoding.UTF8.GetBytes(plainText);
                    break;
            }
        }

        foreach (var b in codeBytes)
            codeText += DecToBin(b, 8);

        return codeText;
    }

    private static Polynom XorPolynoms(Polynom messagePolynom, Polynom resPolynom)
    {
        var resultPolynom = new Polynom();
        Polynom longPoly, shortPoly;
        if (messagePolynom.PolyItems.Count >= resPolynom.PolyItems.Count)
        {
            longPoly = messagePolynom;
            shortPoly = resPolynom;
        }
        else
        {
            longPoly = resPolynom;
            shortPoly = messagePolynom;
        }

        for (var i = 0; i < longPoly.PolyItems.Count; i++)
        {
            var polItemRes = new PolynomItem
            (

                    longPoly.PolyItems[i].Coefficient ^
                    (shortPoly.PolyItems.Count > i ? shortPoly.PolyItems[i].Coefficient : 0),
                messagePolynom.PolyItems[0].Exponent - i
            );
            resultPolynom.PolyItems.Add(polItemRes);
        }
        resultPolynom.PolyItems.RemoveAt(0);
        return resultPolynom;
    }


    private static Polynom MultiplyGeneratorPolynomByLeadTerm(
        Polynom genPolynom,
        PolynomItem leadTerm,
        int lowerExponentBy)
    {
        var resultPolynom = new Polynom();
        foreach (var polItemBase in genPolynom.PolyItems)
        {
            var polItemRes = new PolynomItem(
                (polItemBase.Coefficient + leadTerm.Coefficient) % 255,
                polItemBase.Exponent - lowerExponentBy
            );
            resultPolynom.PolyItems.Add(polItemRes);
        }
        
        return resultPolynom;
    }


    private static Polynom MultiplyAlphaPolynoms(Polynom polynomBase, Polynom polynomMultiplier)
    {
            var resultPolynom = new Polynom();
            foreach (var polItemBase in polynomMultiplier.PolyItems)
            {
                foreach (var polItemMulti in polynomBase.PolyItems)
                {
                    var polItemRes = new PolynomItem
                    (
                        ShrinkAlphaExp(polItemBase.Coefficient + polItemMulti.Coefficient),
                        (polItemBase.Exponent + polItemMulti.Exponent)
                    );
                    resultPolynom.PolyItems.Add(polItemRes);
                }
            }
            var exponentsToGlue = resultPolynom.PolyItems.GroupBy(x => x.Exponent).Where(x => x.Count() > 1).Select(x => x.First().Exponent);
            var toGlue = exponentsToGlue as IList<int> ?? exponentsToGlue.ToList();
            var gluedPolynoms = new List<PolynomItem>(toGlue.Count);
            foreach (var exponent in toGlue)
            {
                var coefficient = resultPolynom.PolyItems.Where(x => x.Exponent == exponent).Aggregate(0, (current, polynomOld)
                    => current ^ GetIntValFromAlphaExp(polynomOld.Coefficient));
                var polynomFixed = new PolynomItem(GetAlphaExpFromIntVal(coefficient), exponent);
                gluedPolynoms.Add(polynomFixed);
            }
            resultPolynom.PolyItems.RemoveAll(x => toGlue.Contains(x.Exponent));
            resultPolynom.PolyItems.AddRange(gluedPolynoms);
            resultPolynom.PolyItems.Sort((x, y) => -x.Exponent.CompareTo(y.Exponent));
            return resultPolynom;
        }

    private static int GetIntValFromAlphaExp(int exp)
    {
            return GaloisField.Find(antilog => antilog.ExponentAlpha == exp).IntegerValue;
        }

    private static int GetAlphaExpFromIntVal(int intVal)
    {
        return GaloisField.Find(antilog => antilog.IntegerValue == intVal).ExponentAlpha;
    }

    private static int ShrinkAlphaExp(int alphaExp)
    {
        // ReSharper disable once PossibleLossOfFraction
        return (int)((alphaExp % 256) + Math.Floor((double)(alphaExp / 256)));
    }

    private static Dictionary<char, int> CreateAlphanumEncDict()
    {
        var localAlphanumEncDict = new Dictionary<char, int>(45);
        //Add numbers
        for (int i = 0; i < 10; i++)
            localAlphanumEncDict.Add($"{i}"[0], i);
        //Add chars
        for (char c = 'A'; c <= 'Z'; c++)
            localAlphanumEncDict.Add(c, localAlphanumEncDict.Count);
        //Add special chars
        foreach (var t in AlphanumEncTable)
            localAlphanumEncDict.Add(t, localAlphanumEncDict.Count);

        return localAlphanumEncDict;
    }

    private static List<AlignmentPattern> CreateAlignmentPatternTable()
    {
            var localAlignmentPatternTable = new List<AlignmentPattern>(40);

            for (var i = 0; i < (7 * 40); i = i + 7)
            {
                var points = new List<Point>();
                for (var x = 0; x < 7; x++)
                {
                    if (AlignmentPatternBaseValues[i + x] != 0)
                    {
                        for (var y = 0; y < 7; y++)
                        {
                            if (AlignmentPatternBaseValues[i + y] != 0)
                            {
                                var p = new Point(AlignmentPatternBaseValues[i + x] - 2, AlignmentPatternBaseValues[i + y] - 2);
                                if (!points.Contains(p))
                                    points.Add(p);
                            }
                        }
                    }
                }

                localAlignmentPatternTable.Add(new AlignmentPattern()
                {
                    Version = (i + 7) / 7,
                    PatternPositions = points
                }
                );
            }
            return localAlignmentPatternTable;
        }


    private static List<EccInfo> CreateCapacityEccTable()
    {
        var table = new List<EccInfo>(160);
        for (var i = 0; i < (4 * 6 * 40); i = i + (4 * 6))
        {
            table.AddRange(
            new[]
            {
                new EccInfo(
                    (i+24) / 24,
                    ErrorCorrectionLevel.Low,
                    CapacityEccBaseValues[i],
                    CapacityEccBaseValues[i+1],
                    CapacityEccBaseValues[i+2],
                    CapacityEccBaseValues[i+3],
                    CapacityEccBaseValues[i+4],
                    CapacityEccBaseValues[i+5]),
                new EccInfo
                (
                    version: (i + 24) / 24,
                    errorCorrectionLevel: ErrorCorrectionLevel.Medium,
                    totalDataCodewords: CapacityEccBaseValues[i+6],
                    eccPerBlock: CapacityEccBaseValues[i+7],
                    blocksInGroup1: CapacityEccBaseValues[i+8],
                    codewordsInGroup1: CapacityEccBaseValues[i+9],
                    blocksInGroup2: CapacityEccBaseValues[i+10],
                    codewordsInGroup2: CapacityEccBaseValues[i+11]
                ),
                new EccInfo
                (
                    version: (i + 24) / 24,
                    errorCorrectionLevel: ErrorCorrectionLevel.Quartile,
                    totalDataCodewords: CapacityEccBaseValues[i+12],
                    eccPerBlock: CapacityEccBaseValues[i+13],
                    blocksInGroup1: CapacityEccBaseValues[i+14],
                    codewordsInGroup1: CapacityEccBaseValues[i+15],
                    blocksInGroup2: CapacityEccBaseValues[i+16],
                    codewordsInGroup2: CapacityEccBaseValues[i+17]
                ),
                new EccInfo
                (
                    version: (i + 24) / 24,
                    errorCorrectionLevel: ErrorCorrectionLevel.High,
                    totalDataCodewords: CapacityEccBaseValues[i+18],
                    eccPerBlock: CapacityEccBaseValues[i+19],
                    blocksInGroup1: CapacityEccBaseValues[i+20],
                    codewordsInGroup1: CapacityEccBaseValues[i+21],
                    blocksInGroup2: CapacityEccBaseValues[i+22],
                    codewordsInGroup2: CapacityEccBaseValues[i+23]
                )
            });
        }
        return table;
    }

    private static List<VersionInfo> CreateCapacityTable()
    {
            var localCapacityTable = new List<VersionInfo>(40);
            for (var i = 0; i < (16 * 40); i = i + 16)
            {
                localCapacityTable.Add(new VersionInfo(

                    (i + 16) / 16,
                    [
                        new VersionInfoDetails(
                            ErrorCorrectionLevel.Low,
                            new Dictionary<EncodingMode, int>()
                            {
                                { EncodingMode.Numeric, CapacityBaseValues[i] },
                                { EncodingMode.Alphanumeric, CapacityBaseValues[i + 1] },
                                { EncodingMode.Byte, CapacityBaseValues[i + 2] },
                                { EncodingMode.Kanji, CapacityBaseValues[i + 3] },
                            }
                        ),

                        new VersionInfoDetails(
                            ErrorCorrectionLevel.Medium,
                            new Dictionary<EncodingMode, int>()
                            {
                                { EncodingMode.Numeric, CapacityBaseValues[i + 4] },
                                { EncodingMode.Alphanumeric, CapacityBaseValues[i + 5] },
                                { EncodingMode.Byte, CapacityBaseValues[i + 6] },
                                { EncodingMode.Kanji, CapacityBaseValues[i + 7] },
                            }
                        ),

                        new VersionInfoDetails(
                            ErrorCorrectionLevel.Quartile,
                            new Dictionary<EncodingMode, int>()
                            {
                                { EncodingMode.Numeric, CapacityBaseValues[i + 8] },
                                { EncodingMode.Alphanumeric, CapacityBaseValues[i + 9] },
                                { EncodingMode.Byte, CapacityBaseValues[i + 10] },
                                { EncodingMode.Kanji, CapacityBaseValues[i + 11] },
                            }
                        ),

                        new VersionInfoDetails(
                            ErrorCorrectionLevel.High,
                            new Dictionary<EncodingMode, int>()
                            {
                                { EncodingMode.Numeric, CapacityBaseValues[i + 12] },
                                { EncodingMode.Alphanumeric, CapacityBaseValues[i + 13] },
                                { EncodingMode.Byte, CapacityBaseValues[i + 14] },
                                { EncodingMode.Kanji, CapacityBaseValues[i + 15] },
                            }
                        )
                    ]
                ));
            }
            return localCapacityTable;
        }

    private static List<Antilog> CreateAntilogTable()
    {
            var localGaloisField = new List<Antilog>(256);

            int gfItem = 1;
            for (var i = 0; i < 256; i++)
            {
                localGaloisField.Add(new Antilog(i, gfItem));
                gfItem *= 2;
                if (gfItem > 255)
                    gfItem ^= 285;
            }
            return localGaloisField;
        }


    internal enum EncodingMode
    {
        Numeric = 1,
        Alphanumeric = 2,
        Byte = 4,
        Kanji = 8,
        Eci = 7
    }

    internal struct AlignmentPattern
    {
        public int Version;
        public List<Point> PatternPositions;
    }

    private readonly struct CodewordBlock(
        int groupNumber,
        int blockNumber,
        string bitString,
        List<string> codeWords,
        List<string> eccWords,
        List<int> codeWordsInt,
        List<int> eccWordsInt)
    {
        public int GroupNumber { get; } = groupNumber;
        public int BlockNumber { get; } = blockNumber;
        public string BitString { get; } = bitString;
        public List<string> CodeWords { get; } = codeWords;
        public List<int> CodeWordsInt { get; } = codeWordsInt;
        public List<string> EccWords { get; } = eccWords;
        public List<int> EccWordsInt { get; } = eccWordsInt;
    }

    internal readonly struct EccInfo(
        int version,
        ErrorCorrectionLevel errorCorrectionLevel,
        int totalDataCodewords,
        int eccPerBlock,
        int blocksInGroup1,
        int codewordsInGroup1,
        int blocksInGroup2,
        int codewordsInGroup2)
    {
        public int Version { get; } = version;
        public ErrorCorrectionLevel ErrorCorrectionLevel { get; } = errorCorrectionLevel;
        public int TotalDataCodewords { get; } = totalDataCodewords;
        public int EccPerBlock { get; } = eccPerBlock;
        public int BlocksInGroup1 { get; } = blocksInGroup1;
        public int CodewordsInGroup1 { get; } = codewordsInGroup1;
        public int BlocksInGroup2 { get; } = blocksInGroup2;
        public int CodewordsInGroup2 { get; } = codewordsInGroup2;
    }

    internal readonly struct VersionInfo(
        int version,
        List<VersionInfoDetails> versionInfoDetails)
    {
        public int Version { get; } = version;
        public List<VersionInfoDetails> Details { get; } = versionInfoDetails;
    }

    internal readonly struct VersionInfoDetails(
        ErrorCorrectionLevel errorCorrectionLevel,
        Dictionary<EncodingMode, int> capacityDict)
    {
        public ErrorCorrectionLevel ErrorCorrectionLevel { get; } = errorCorrectionLevel;
        public Dictionary<EncodingMode, int> CapacityDict { get; } = capacityDict;
    }

    internal readonly struct Antilog(
        int exponentAlpha,
        int integerValue)
    {
        public int ExponentAlpha { get; } = exponentAlpha;
        public int IntegerValue { get; } = integerValue;
    }

    private readonly struct PolynomItem(
        int coefficient,
        int exponent)
    {
        public int Coefficient { get; } = coefficient;
        public int Exponent { get; } = exponent;
    }

    private sealed class Polynom
    {
        public List<PolynomItem> PolyItems { get; set; } = [];

        private static readonly char[] TrimChars = [' ', '+'];

        public override string ToString()
        {
            var sb = new StringBuilder();
            //this.PolyItems.ForEach(x => sb.Append("a^" + x.Coefficient + "*x^" + x.Exponent + " + "));
            foreach (var polyItem in PolyItems)
            {
                sb.Append("a^" + polyItem.Coefficient + "*x^" + polyItem.Exponent + " + ");
            }

            return sb.ToString().TrimEnd(TrimChars);
        }
    }

    internal sealed class Point(int x, int y)
    {
        public int X { get; } = x;
        public int Y { get; } = y;
    }

    private sealed class Rectangle(int x, int y, int w, int h)
    {
        public int X { get; } = x;
        public int Y { get; } = y;
        public int Width { get; } = w;
        public int Height { get; } = h;
    }
}