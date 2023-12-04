using Kesa.AdventOfCode.Common;


namespace Kesa.AdventOfCode.Aoc2023
{
    internal record struct Vector2(int X, int Y)
    {
        public static Vector2 operator +(Vector2 l, Vector2 r) => new Vector2(l.X + r.X, l.Y + r.Y);
    }

    internal record Day03TextGrid(string Input)
    {
        public static IReadOnlyCollection<Vector2> Offsets { get; } =
        [
            new Vector2(-1, -1),
            new Vector2(-1, 0),
            new Vector2(-1, 1),

            new Vector2(0, -1),
            new Vector2(0, 0),
            new Vector2(0, 1),

            new Vector2(1, -1),
            new Vector2(1, 0),
            new Vector2(1, 1),
        ];

        public int Width { get; } = Input.IndexOf('\n');

        public int Height => Width;

        public ReadOnlySpan<char> GetLine(int rowIndex) => Input.AsSpan((Width + 1) * rowIndex, Width);

        public Vector2 GetNumberStart(Vector2 position)
        {
            for (int x = position.X; x >= 0; x--)
            {
                var potentialPosition = position with { X = x };
                if (TryGetCharacter(potentialPosition, out var potentialDigit) && char.IsDigit(potentialDigit))
                {
                    position = potentialPosition;
                    continue;
                }
                break;
            }

            return position;
        }

        public bool TryReadPartNumber(Vector2 startPosition, out int value)
        {
            var isPartNumber = false;
            var number = 0;

            for (int x = startPosition.X; x < Width; x++)
            {
                var position = new Vector2(x, startPosition.Y);

                foreach (var offset in Offsets)
                {
                    if (IsPositionSymbol(position + offset))
                    {
                        isPartNumber = true;
                    }
                }

                if (TryGetCharacter(position, out var currentCharacter))
                {
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
            }

            value = number;
            return isPartNumber;
        }

        public bool IsPositionSymbol(Vector2 position) => TryGetCharacter(position, out var character) && !char.IsDigit(character) && character is not '.';

        public bool TryGetCharacter(Vector2 position, out char character)
        {
            if (position.Y < 0 || position.Y >= Height || position.X < 0 || position.X >= Width)
            {
                character = default;
                return false;
            }

            character = GetLine(position.Y)[position.X];
            return true;
        }
    }

    internal class Day03 : IAocRunner
    {
        public static string RunPart1(string input)
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

        public static string RunPart2(string input)
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

                    foreach (var offset in Day03TextGrid.Offsets)
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
