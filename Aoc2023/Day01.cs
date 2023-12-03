
using Kesa.AdventOfCode.Common;

namespace Kesa.AdventOfCode.Aoc2023
{
    internal class Day01 : IAocRunner
    {
        public static string Run(string input)
        {
            var value = 0;

            foreach (var line in input.Lines())
            {
                var tens = -1;
                var ones = -1;

                for (int i = 0; i < line.Length; i++)
                {
                    var span = line.AsSpan(i);

                    var number = span switch
                    {
                        ['0', ..] => 0,
                        ['1', ..] => 1,
                        ['2', ..] => 2,
                        ['3', ..] => 3,
                        ['4', ..] => 4,
                        ['5', ..] => 5,
                        ['6', ..] => 6,
                        ['7', ..] => 7,
                        ['8', ..] => 8,
                        ['9', ..] => 9,

                        _ when span.StartsWith("zero") => 0,
                        _ when span.StartsWith("one") => 1,
                        _ when span.StartsWith("two") => 2,
                        _ when span.StartsWith("three") => 3,
                        _ when span.StartsWith("four") => 4,
                        _ when span.StartsWith("five") => 5,
                        _ when span.StartsWith("six") => 6,
                        _ when span.StartsWith("seven") => 7,
                        _ when span.StartsWith("eight") => 8,
                        _ when span.StartsWith("nine") => 9,

                        _ => -1,
                    };

                    if (number != -1)
                    {
                        if (tens == -1)
                        {
                            tens = number;
                        }

                        ones = number;
                    }
                }

                value += ((tens * 10) + ones);
            }

            return value.ToString();
        }
    }
}
