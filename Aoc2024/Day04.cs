using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using Kesa.AdventOfCode.Common;

namespace Kesa.AdventOfCode.Aoc2024;

public class Day04 : IAocRunner
{
    public static object RunPart1(string input)
    {
        var board = new TextBoard(input);
        var total = 0;

        var directions = new (int X, int Y)[]
        {
            (-1, 0),
            (1, 0),
            (0, -1),
            (0, 1),

            (-1, -1),
            (1, -1),
            (-1, 1),
            (1, 1)
        };

        for (int x = 0; x < board.Width; x++)
        {
            for (int y = 0; y < board.Height; y++)
            {
                foreach (var direction in directions)
                {
                    if (true
                        && SearchInDirection(0, 0, 'X')
                        && SearchInDirection(direction.X, direction.Y, 'M')
                        && SearchInDirection(direction.X * 2, direction.Y * 2, 'A')
                        && SearchInDirection(direction.X * 3, direction.Y * 3, 'S'))
                    {
                        total += 1;
                    }
                }

                bool SearchInDirection(int xOffset, int yOffset, char desired)
                {
                    return board.TryGetChar(xOffset, yOffset, out var c) && c == desired;
                }
            }
        }

        return total;
    }

    public static object RunPart2(string input)
    {
        throw new NotImplementedException();
    }

    private class TextBoard(string input)
    {
        public string[] _lines = input.Lines().ToArray();

        public int Width => _lines[0].Length;
        public int Height => _lines.Length;

        public bool TryGetChar(int x, int y, out char c)
        {
            if (y < 0 || x < 0 || y >= _lines.Length || x >= _lines[y].Length)
            {
                Unsafe.SkipInit(out c);
                return false;
            }

            c = _lines[y][x];
            return true;
        }
    }
}