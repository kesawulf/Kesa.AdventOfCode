using Kesa.AdventOfCode.Common;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Kesa.AdventOfCode.Aoc2023
{
    internal class Day02_Part2 : IAocRunner
    {
        public static string Run(string input)
        {
            var gameRegex = new Regex(@"Game (?<id>\d+)");
            var itemRegex = new Regex(@"(?:(?<count>\d+) (?<color>\w+))");
            var colors = new Dictionary<string, int>();
            var answer = 0;

            foreach (var item in input.Lines())
            {
                colors.Clear();

                if (true
                    && gameRegex.Match(item) is { Success: true } gameMatch
                    && itemRegex.Matches(item) is { Count: > 0 } itemMatches)
                {
                    foreach (Match match in itemMatches)
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
