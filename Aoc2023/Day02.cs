using Kesa.AdventOfCode.Common;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Kesa.AdventOfCode.Aoc2023
{
    internal partial class Day02 : IAocRunner
    {
        [GeneratedRegex(@"Game (?<id>\d+)")]
        public static partial Regex GetGameRegex();

        [GeneratedRegex(@"(?:(?<count>\d+) (?<color>\w+))")]
        public static partial Regex GetItemRegex();

        public static object RunPart1(string input)
        {
            var answer = 0;

            foreach (var item in input.Lines())
            {
                if (true
                    && GetGameRegex().Match(item) is { Success: true } gameMatch
                    && GetItemRegex().Matches(item) is { Count: > 0 } itemMatches)
                {
                    var gameId = gameMatch.GetGroup<int>("id");
                    var valid = true;

                    foreach (var match in (IEnumerable<Match>)itemMatches)
                    {
                        var colorName = match.GetGroup("color");
                        var colorCount = match.GetGroup<int>("count");

                        valid = colorName switch
                        {
                            "red" when colorCount > 12 => false,
                            "green" when colorCount > 13 => false,
                            "blue" when colorCount > 14 => false,
                            _ => valid,
                        };
                    }

                    if (valid)
                    {
                        answer += gameId;
                    }
                }
            }

            return answer.ToString();
        }

        public static object RunPart2(string input)
        {
            var colors = new Dictionary<string, int>();
            var answer = 0;

            foreach (var item in input.Lines())
            {
                colors.Clear();
                if (GetItemRegex().Matches(item) is { Count: > 0 } itemMatches)
                {
                    foreach (var match in (IEnumerable<Match>)itemMatches)
                    {
                        var colorName = match.GetGroup("color");
                        var colorCount = match.GetGroup<int>("count");
                        ref var existingCount = ref CollectionsMarshal.GetValueRefOrAddDefault(colors, colorName, out var existed);
                        existingCount = Math.Max(existingCount, colorCount);
                    }

                    var power = colors.Aggregate(1, (a, b) => a * b.Value);
                    answer += power;
                }
            }

            return answer.ToString();
        }
    }
}
