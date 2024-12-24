using System.Text.RegularExpressions;

namespace Kesa.AdventOfCode.Common
{
    internal static class Extensions
    {
        public static (T, U, V) Extend<T, U, V>(this (T, U) tuple, Func<T, U, V> extender)
        {
            return (tuple.Item1, tuple.Item2, extender(tuple.Item1, tuple.Item2));
        }

        public static IEnumerable<string> Lines(this string value)
        {
            using var reader = new StringReader(value);

            while (reader.ReadLine() is { } line)
            {
                yield return line;
            }
        }

        public static IEnumerable<string> SplitSpaces(this string value)
        {
            return value.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        }

        public static IEnumerable<int> SplitIntoInts(this string value)
        {
            return SplitSpaces(value).Select(int.Parse);
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

        public static (List<T> Items1, List<U> Items2) ToSeparateLists<T, U>(this IEnumerable<(T, U)> source)
        {
            var items1 = new List<T>();
            var items2 = new List<U>();

            foreach (var item in source)
            {
                items1.Add(item.Item1);
                items2.Add(item.Item2);
            }

            return (items1, items2);
        }

        public static IEnumerable<T> Pipe<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
                yield return item;
            }
        }
    }
}