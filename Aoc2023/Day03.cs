using Kesa.AdventOfCode.Common;
using System.Text;

namespace Kesa.AdventOfCode.Aoc2023
{
    internal static class Day03Shared
    {
        public static (int X, int Y)[] Offsets { get; } = new[]
        {
            (-1, -1),
            (-1, 0),
            (-1, 1),

            (0, -1),
            (0, 0),
            (0, 1),

            (1, -1),
            (1, 0),
            (1, 1),
        };
    }

    internal class Day03_Part1 : IAocRunner
    {
        public static string Run(string input)
        {
            var lines = input.Lines().ToArray()!;
            var number = new StringBuilder();
            var isPartNumber = false;
            var answer = 0;

            for (int row = 0; row < lines.Length; row++)
            {
                var line = lines[row];

                Reset();

                for (int column = 0; column < line.Length + 1; column++)
                {
                    if (column == line.Length)
                    {
                        Check();
                        continue;
                    }

                    var character = line[column];

                    if (char.IsDigit(character))
                    {
                        number.Append(character);

                        if (HasSymbolAround(row, column))
                        {
                            isPartNumber = true;
                        }
                    }
                    else
                    {
                        Check();
                    }
                }
            }

            return answer.ToString();

            void Check()
            {
                if (number.Length > 0)
                {
                    if (isPartNumber)
                    {
                        answer += int.Parse(number.ToString());
                    }

                    Reset();
                }
            }

            void Reset()
            {
                isPartNumber = false;
                number.Clear();
            }

            bool HasSymbolAround(int row, int column)
            {
                return false
                    || IsPositionSymbol(row - 1, column - 1)
                    || IsPositionSymbol(row - 1, column)
                    || IsPositionSymbol(row - 1, column + 1)

                    || IsPositionSymbol(row, column - 1)
                    || IsPositionSymbol(row, column + 1)

                    || IsPositionSymbol(row + 1, column - 1)
                    || IsPositionSymbol(row + 1, column)
                    || IsPositionSymbol(row + 1, column + 1);
            }

            bool IsPositionSymbol(int row, int column) => TryGetCharacter(row, column, out var character) && IsSymbol(character);

            bool IsSymbol(char character) => !char.IsDigit(character) && character is not '.';

            bool TryGetCharacter(int row, int column, out char character)
            {
                if (row >= 0 && row < lines!.Length)
                {
                    var line = lines[row];

                    if (column >= 0 && column < line.Length)
                    {
                        character = line[column];
                        return true;
                    }
                }

                character = default;
                return false;
            }
        }
    }

    internal class Day03NumberInfo
    {
        public int StartX { get; set; }

        public int StartY { get; set; }

        public int Value { get; set; }

        public bool IsPartNumber { get; set; }
    }

    internal class Day03_Part2 : IAocRunner
    {
        public static string Run(string input)
        {
            var lines = input.Lines().ToArray()!;
            var number = new StringBuilder();
            var numberInfo = new Day03NumberInfo();
            var answer = 0;

            var numberMap = new Dictionary<(int X, int Y), Day03NumberInfo>();

            for (int row = 0; row < lines.Length; row++)
            {
                var line = lines[row];

                Reset();

                for (int column = 0; column < line.Length + 1; column++)
                {
                    if (column == line.Length)
                    {
                        Check();
                        continue;
                    }

                    var character = line[column];

                    if (char.IsDigit(character))
                    {
                        number.Append(character);
                        numberInfo ??= new Day03NumberInfo() { StartX = column, StartY = row };
                        numberMap[(column, row)] = numberInfo;

                        if (HasSymbolAround(row, column))
                        {
                            numberInfo.IsPartNumber = true;
                        }
                    }
                    else
                    {
                        Check();
                    }
                }
            }

            for (int row = 0; row < lines.Length; row++)
            {
                var line = lines[row];

                for (int column = 0; column < line.Length + 1; column++)
                {
                    if (!TryGetCharacter(row, column, out var potentialGear) || potentialGear is not '*')
                    {
                        continue;
                    }

                    var surroundingNumbers = Day03Shared.Offsets
                        .Select(offset => numberMap.TryGetValue((column + offset.X, row + offset.Y), out var numberInfo) ? numberInfo : null)
                        .Where(info => info is { IsPartNumber: true })
                        .Distinct()
                        .ToArray();

                    if (surroundingNumbers.Length == 2)
                    {
                        var ratio = surroundingNumbers.Select(n => n.Value).Aggregate((a, b) => a * b);
                        answer += ratio;
                    }
                }
            }

            return answer.ToString();

            void Check()
            {
                if (number.Length > 0)
                {
                    numberInfo.Value = int.Parse(number.ToString());
                    Reset();
                }
            }

            void Reset()
            {
                number.Clear();
                numberInfo = null;
            }

            bool HasSymbolAround(int row, int column)
            {
                return false
                    || IsPositionSymbol(row - 1, column - 1)
                    || IsPositionSymbol(row - 1, column)
                    || IsPositionSymbol(row - 1, column + 1)

                    || IsPositionSymbol(row, column - 1)
                    || IsPositionSymbol(row, column + 1)

                    || IsPositionSymbol(row + 1, column - 1)
                    || IsPositionSymbol(row + 1, column)
                    || IsPositionSymbol(row + 1, column + 1);
            }

            bool IsPositionSymbol(int row, int column) => TryGetCharacter(row, column, out var character) && IsSymbol(character);

            bool IsSymbol(char character) => !char.IsDigit(character) && character is not '.';

            bool TryGetCharacter(int row, int column, out char character)
            {
                if (row >= 0 && row < lines!.Length)
                {
                    var line = lines[row];

                    if (column >= 0 && column < line.Length)
                    {
                        character = line[column];
                        return true;
                    }
                }

                character = default;
                return false;
            }
        }
    }
}
