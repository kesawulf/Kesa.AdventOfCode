using System.Text.RegularExpressions;
using Kesa.AdventOfCode.Common;

namespace Kesa.AdventOfCode.Aoc2024;

public partial class Day03 : IAocRunner
{
    [GeneratedRegex(@"(?<inst>don't)\(\)|(?<inst>do)\(\)|(?<inst>mul)\((?<l>\d+),(?<r>\d+)\)")]
    public static partial Regex GetRegex();
    
    public static object RunPart1(string input)
    {
        return GetRegex().Matches(input)
            .Where(match => match.GetGroup("inst") == "mul")
            .Sum(match => match.GetGroup<int>("l") * match.GetGroup<int>("r"));
    }

    public static object RunPart2(string input)
    {
        var total = 0;
        var enabled = true;

        foreach (Match match in GetRegex().Matches(input))
        {
            if (match.GetGroup("inst") == "mul" && enabled)
            {
                total += int.Parse(match.Groups[2].Value) * int.Parse(match.Groups[3].Value);
            }
            else
            {
                enabled = match.GetGroup("inst") == "do";
            }
        }

        return total;
    }
}