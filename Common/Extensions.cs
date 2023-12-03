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
    }
}
