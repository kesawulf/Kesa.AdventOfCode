using Kesa.AdventOfCode.Common;

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

    internal record Day03TextGrid(string input)
    {
        public int Width { get; } = input.IndexOf('\n');

        public int Height => Width;

        public ReadOnlySpan<char> GetLine(int rowIndex)
        {
            var start = (Width + 1) * rowIndex;
            return input.AsSpan(start, Width);
        }

        public Vector2 GetNumberStart(Vector2 position)
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

        public bool TryReadPartNumber(Vector2 startPosition, out int value)
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
                    if (position.X > Width)
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

        public bool IsPositionSymbol(Vector2 position) => TryGetCharacter(position, out var character) && IsSymbol(character);

        public bool IsSymbol(char character) => !char.IsDigit(character) && character is not '.';

        public bool TryGetCharacter(Vector2 position, out char character)
        {
            var (column, row) = (position.X, position.Y);

            if (row >= 0 && row < Height)
            {
                var line = GetLine(row);

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
            var grid = new Day03TextGrid(input);
            var knownStarts = new HashSet<Vector2>();
            var answer = 0;

            for (int row = 0; row < grid.Height; row++)
            {
                for (int column = 0; column < grid.Width + 1; column++)
                {
                    var position = new Vector2(column, row);

                    if (grid.TryGetCharacter(position, out var potentialDigit) && char.IsDigit(potentialDigit))
                    {
                        var start = grid.GetNumberStart(position);

                        if (knownStarts.Add(start) && grid.TryReadPartNumber(start, out var number))
                        {
                            answer += number;
                        }
                    }
                }
            }

            return answer.ToString();
        }
    }

    internal class Day03_Part2 : IAocRunner
    {
        public static string Run(string input)
        {
            var grid = new Day03TextGrid(input);
            var knownStarts = new HashSet<Vector2>();
            var answer = 0;

            for (int row = 0; row < grid.Height; row++)
            {
                var line = grid.GetLine(row);

                for (int column = 0; column < line.Length + 1; column++)
                {
                    var position = new Vector2(column, row);

                    if (!grid.TryGetCharacter(position, out var potentialGear) || potentialGear is not '*')
                    {
                        continue;
                    }

                    knownStarts.Clear();

                    var count = 0;
                    var ratio = 1;

                    foreach (var offset in Day03Shared.Offsets)
                    {
                        var potentialPosition = position + offset;
                        if (grid.TryGetCharacter(potentialPosition, out var potentialDigit) && char.IsDigit(potentialDigit))
                        {
                            var start = grid.GetNumberStart(potentialPosition);

                            if (knownStarts.Add(start) && grid.TryReadPartNumber(start, out var number))
                            {
                                count++;
                                ratio *= number;
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
        }
    }
}
