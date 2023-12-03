using Kesa.AdventOfCode.Common;
using System.Text;

namespace Kesa.AdventOfCode.Aoc2023
{
    internal record struct Vector2(int X, int Y)
    {
        public static implicit operator Vector2((int X, int Y) tuple) => new Vector2(tuple.X, tuple.Y);

        public static Vector2 operator +(Vector2 l, Vector2 r) => new Vector2(l.X + r.X, l.Y + r.Y);

        public override string ToString()
        {
            return (X, Y).ToString();
        }
    }

    internal static class Day03Shared
    {
        public static Vector2[] Offsets { get; } =
        [
            (-1, -1),
            (-1, 0),
            (-1, 1),

            (0, -1),
            (0, 0),
            (0, 1),

            (1, -1),
            (1, 0),
            (1, 1),
        ];
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

    internal class Day03_Part2 : IAocRunner
    {
        public static string Run(string input)
        {
            var lines = input.Lines().ToArray()!;
            var answer = 0;

            for (int row = 0; row < lines.Length; row++)
            {
                var line = lines[row];
                var knownStarts = new HashSet<Vector2>();

                for (int column = 0; column < line.Length + 1; column++)
                {
                    var position = new Vector2(column, row);

                    if (!TryGetCharacter(position, out var potentialGear) || potentialGear is not '*')
                    {
                        continue;
                    }

                    var count = 0;
                    var ratio = 1;

                    foreach (var offset in Day03Shared.Offsets)
                    {
                        var potentialPosition = position + offset;
                        if (TryGetCharacter(potentialPosition, out var potentialDigit) && char.IsDigit(potentialDigit))
                        {
                            var start = GetNumberStart(potentialPosition);

                            if (TryReadPartNumber(start, out var value) && knownStarts.Add(start))
                            {
                                count++;
                                ratio *= value;
                            }
                        }
                    }

                    if (count == 2)
                    {
                        answer += ratio;
                    }
                }
            }

            return answer.ToString();

            Vector2 GetNumberStart(Vector2 position)
            {
                for (int x = position.X; x >= 0; x--)
                {
                    var potentialPosition = position with { X = x };

                    if (TryGetCharacter(potentialPosition, out var potentialDigit) && char.IsDigit(potentialDigit))
                    {
                        position = potentialPosition;
                    }
                    else
                    {
                        break;
                    }
                }

                return position;
            }

            bool TryReadPartNumber(Vector2 startPosition, out int value)
            {
                var isPartNumber = false;
                var number = 0;

                if (IsPositionSymbol(startPosition + (-1, 0)))
                {
                    isPartNumber = true;
                }

                var position = startPosition + (-1, 0);

                for (; ; position.X++)
                {
                    if (!TryGetCharacter(position, out var currentCharacter))
                    {
                        if (position.X > lines[startPosition.Y].Length)
                        {
                            break;
                        }

                        continue;
                    }

                    if (IsPositionSymbol(position + (0, -1)) || IsPositionSymbol(position + (0, 1)))
                    {
                        isPartNumber = true;
                    }

                    if (char.IsDigit(currentCharacter))
                    {
                        number *= 10;
                        number += (currentCharacter - '0');
                    }
                    else if (position.X >= startPosition.X)
                    {
                        break;
                    }
                }

                if (IsPositionSymbol(position))
                {
                    isPartNumber = true;
                }

                value = number;
                return isPartNumber;
            }

            bool IsPositionSymbol(Vector2 position) => TryGetCharacter(position, out var character) && IsSymbol(character);

            bool IsSymbol(char character) => !char.IsDigit(character) && character is not '.';

            bool TryGetCharacter(Vector2 position, out char character)
            {
                var (column, row) = (position.X, position.Y);

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
