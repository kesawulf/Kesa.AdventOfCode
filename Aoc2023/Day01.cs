
using Kesa.AdventOfCode.Common;
using MoreLinq;

namespace Kesa.AdventOfCode.Aoc2023
{
    internal class Day01 : IAocRunner
    {
        private delegate int CharacterNumericValueDelegate(ref ReadOnlySpan<char> span);

        private static string RunInternal(string input, CharacterNumericValueDelegate characterConverter)
        {
            var value = 0;

            foreach (var line in input.Lines())
            {
                var tens = -1;
                var ones = -1;

                for (int i = 0; i < line.Length; i++)
                {
                    var span = line.AsSpan(i);
                    if (characterConverter(ref span) is not -1 and { } number)
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

        public static string RunPart1(string input)
        {
            return RunInternal(input, (ref ReadOnlySpan<char> span) => span switch
            {
                _ when char.IsAsciiDigit(span[0]) => span[0] - '0',
                _ => -1,
            });
        }

        public static string RunPart2(string input)
        {
            return RunInternal(input, (ref ReadOnlySpan<char> span) => span switch
            {
                _ when char.IsAsciiDigit(span[0]) => span[0] - '0',

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
            });
        }
    }
}