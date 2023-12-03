using Kesa.AdventOfCode.Common;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Kesa.AdventOfCode.Aoc2023
{
    internal class Day02Shared
    {
        public static Regex GameRegex = new Regex(@"Game (?<id>\d+)");

        public static Regex ItemRegex = new Regex(@"(?:(?<count>\d+) (?<color>\w+))");
    }

    internal class Day02_Part1 : IAocRunner
    {
        public static string Run(string input)
        {
            var answer = 0;

            foreach (var item in input.Lines())
            {
                if (true
                    && Day02Shared.GameRegex.Match(item) is { Success: true } gameMatch
                    && Day02Shared.ItemRegex.Matches(item) is { Count: > 0 } itemMatches)
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
    }

    internal class Day02_Part2 : IAocRunner
    {
        public static string Run(string input)
        {
            var colors = new Dictionary<string, int>();
            var answer = 0;

            foreach (var item in input.Lines())
            {
                colors.Clear();

                if (true
                    && Day02Shared.GameRegex.Match(item) is { Success: true } gameMatch
                    && Day02Shared.ItemRegex.Matches(item) is { Count: > 0 } itemMatches)
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
