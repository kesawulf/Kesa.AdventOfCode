using Kesa.AdventOfCode.Common;
using MoreLinq.Extensions;

namespace Kesa.AdventOfCode.Aoc2024;

public class Day02 : IAocRunner
{
    public static object RunPart1(string input)
    {
        return GetReports(input).Count(report => report.IsSafe);
    }

    public static object RunPart2(string input)
    {
        return GetReports(input).Count(report => report.IsSafe || Dampen(report).Any(dampened => dampened.IsSafe));
    }

    private static IEnumerable<Report> GetReports(string input)
    {
        return input.Lines()
            .Select(Extensions.SplitIntoInts)
            .Select(values => new Report(values.ToArray()));
    }

    private static IEnumerable<Report> Dampen(Report report)
    {
        var values = report.Values.ToList();
        foreach (var (index, value) in values.Index().ToArray())
        {
            values.RemoveAt(index);
            yield return new Report(values.ToArray());
            values.Insert(index, value);
        }
    }

    private record Report(int[] Values)
    {
        public IEnumerable<ReportDelta> GetDeltas() => Values.Window(2)
            .Select(pair => pair[1] - pair[0])
            .Select(difference => new ReportDelta(Math.Sign(difference), Math.Abs(difference)));

        public bool IsSafe => GetDeltas().GroupBy(delta => delta.Sign)
            .ToArray() is [var deltas] && deltas.All(delta => delta.IsSafe);
    }

    public record ReportDelta(int Sign, int Value)
    {
        public bool IsSafe { get; } = Value is >= 1 and <= 3;
    }
}