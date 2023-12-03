using System.Text.RegularExpressions;

namespace Kesa.AdventOfCode.Common
{
    internal static class Extensions
    {
        public static IEnumerable<string> Lines(this string value)
        {
            using var reader = new StringReader(value);

            while (reader.ReadLine() is { } line)
            {
                yield return line;
            }
        }

        public static string GetGroup(this Match match, string group)
        {
            return match.Groups[group].Value;
        }

        public static bool TryGetGroup(this Match match, string group, out string? value)
        {
            if (match.Groups[group] is { Success: true, Value: { } groupValue })
            {
                value = groupValue;
                return true;
            }

            value = default!;
            return false;
        }

        public static T GetGroup<T>(this Match match, string group) where T : IParsable<T>
        {
            return T.Parse(match.Groups[group].Value, null);
        }

        public static bool TryGetGroup<T>(this Match match, string group, out T value) where T : IParsable<T>
        {
            value = default!;
            return TryGetGroup(match, group, out var stringValue) && T.TryParse(stringValue, null, out value);
        }
    }
}
