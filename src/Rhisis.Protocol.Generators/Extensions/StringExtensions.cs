using System.Text.RegularExpressions;

namespace Rhisis.Protocol.Generators.Extensions;

/// <summary>
/// Provides extensions for string manipulation.
/// </summary>
internal static class StringExtensions
{
    /// <summary>
    /// Converts a string to camelCase format.
    /// </summary>
    /// <param name="s">String input.</param>
    /// <returns></returns>
    public static string ToCamelCase(this string s)
    {
        string x = s.Replace("_", "");
        
        if (x.Length == 0)
        {
            return "null";
        }

        x = Regex.Replace(x, "([A-Z])([A-Z]+)($|[A-Z])", m => m.Groups[1].Value + m.Groups[2].Value.ToLower() + m.Groups[3].Value);
        
        return char.ToLower(x[0]) + x.Substring(1);
    }
}