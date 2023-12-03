using Kesa.AdventOfCode.Common;
using System.Text.RegularExpressions;

namespace Kesa.AdventOfCode.Aoc2023
{
    internal class Day02 : IAocRunner
    {
        public static string Run(string input)
        {
            var gameRegex = new Regex(@"Game (?<id>\d+)");
            var itemRegex = new Regex(@"(?:(?<count>\d+) (?<color>\w+))");
            var answer = 0;

            foreach (var item in input.Lines())
            {
                if (true
                    && gameRegex.Match(item) is { Success: true } gameMatch
                    && itemRegex.Matches(item) is { Count: > 0 } itemMatches)
                {
                    var gameId = gameMatch.GetGroup<int>("id");
                    var gameInfo = new Day02GameInfo(gameId);
                    var valid = true;

                    foreach (Match match in itemMatches)
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

    public record Day02GameInfo(int Id);
}
