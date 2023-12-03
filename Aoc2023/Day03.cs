using Kesa.AdventOfCode.Common;
using System.Text;

namespace Kesa.AdventOfCode.Aoc2023
{
    internal class Day03 : IAocRunner
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
}
