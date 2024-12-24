using System.Data;
using Kesa.AdventOfCode.Common;

namespace Kesa.AdventOfCode.Aoc2024;

public class Day01 : IAocRunner
{
    public static object RunPart1(string input)
    {
        var (left, right) = GetColumns(input).ToSeparateLists();

        left.Sort();
        right.Sort();

        return right.Zip(left)
            .Select(pair => pair.Second - pair.First)
            .Select(Math.Abs)
            .Sum();
    }

    public static object RunPart2(string input)
    {
        var (left, right) = GetColumns(input).ToSeparateLists();
        return left.Aggregate(0,
            (aggregate, current) => aggregate += current * right.Count(item => current == item));
    }

    public static IEnumerable<(int Left, int Right)> GetColumns(string input)
    {
        foreach (var line in input.Lines())
        {
            var data = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            yield return (int.Parse(data[0]), int.Parse((data[1])));
        }
    }
}