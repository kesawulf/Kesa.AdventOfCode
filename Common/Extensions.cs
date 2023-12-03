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

        public static T GetGroup<T>(this Match match, string group) where T : IParsable<T>
        {
            return T.Parse(match.Groups[group].Value, null);
        }
    }
}
