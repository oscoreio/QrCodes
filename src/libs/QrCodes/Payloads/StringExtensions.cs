using System.Globalization;
using System.Text.RegularExpressions;

namespace QrCodes.Payloads;

/// <summary>
/// 
/// </summary>
internal static class StringExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="iban"></param>
    /// <returns></returns>
    public static bool IsValidIban(this string iban)
    {
        iban = iban ?? throw new ArgumentNullException(nameof(iban));
        
        //Clean IBAN
        var ibanCleared = iban.ToUpper(CultureInfo.InvariantCulture).Replace(" ", "").Replace("-", "");

        //Check for general structure
        var structurallyValid = Regex.IsMatch(ibanCleared, @"^[a-zA-Z]{2}[0-9]{2}([a-zA-Z0-9]?){16,30}$");

        //Check IBAN checksum
        var sum = $"{ibanCleared.Substring(4)}{ibanCleared.Substring(0, 4)}".ToCharArray().Aggregate("", (current, c) => current + (char.IsLetter(c) ? (c - 55).ToString() : c.ToString()));
        int m = 0;
        for (int i = 0; i < (int)Math.Ceiling((sum.Length - 2) / 7d); i++){
            var offset = (i == 0 ? 0 : 2);
            var start = i * 7 + offset;
            var n = (i == 0 ? "" : m.ToString()) + sum.Substring(start, Math.Min(9 - offset, sum.Length - start));
            if (!int.TryParse(n, NumberStyles.Any, CultureInfo.InvariantCulture, out m))
                break;
            m = m % 97;
        }
        var checksumValid = m == 1;
        
        return structurallyValid && checksumValid;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="bic"></param>
    /// <returns></returns>
    public static bool IsValidBic(this string bic)
    {
        bic = bic ?? throw new ArgumentNullException(nameof(bic));
        
        return Regex.IsMatch(
            bic.Replace(" ", ""),
            "^([a-zA-Z]{4}[a-zA-Z]{2}[a-zA-Z0-9]{2}([a-zA-Z0-9]{3})?)$");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="input"></param>
    /// <param name="simple"></param>
    /// <returns></returns>
    public static string EscapeInput(this string input, bool simple = false)
    {
        input = input ?? throw new ArgumentNullException(nameof(input));
        
        char[] forbiddenChars = ['\\', ';', ',', ':'];
        if (simple)
        {
            forbiddenChars = [':'];
        }
        foreach (var c in forbiddenChars)
        {
            input = input.Replace(c.ToString(), "\\" + c);
        }
        
        return input;
    }
}