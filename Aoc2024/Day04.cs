using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Text;
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

        for (var x = 0; x < board.Width; x++)
        {
            for (var y = 0; y < board.Height; y++)
            {
                foreach (var direction in directions)
                {
                    if (board.GetString(x, y, direction.X, direction.Y, 4) is "XMAS")
                    {
                        total++;
                    }
                }
            }
        }

        return total;
    }

    public static object RunPart2(string input)
    {
        var board = new TextBoard(input);
        var total = 0;

        for (var x = 0; x < board.Width; x++)
        {
            for (var y = 0; y < board.Height; y++)
            {
                if (true
                    && board.GetString(x, y, 1, 1, 3) is "MAS" or "SAM"
                    && board.GetString(x + 2, y, -1, 1, 3) is "MAS" or "SAM")
                {
                    total++;
                }
            }
        }

        return total;
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

        public string GetString(int x, int y, int xDirection, int yDirection, int count)
        {
            var text = new StringBuilder(count);

            for (int i = 0; i < count; i++)
            {
                if (TryGetChar(x + (xDirection * i), y + (yDirection * i), out var c))
                {
                    text.Append(c);
                }
            }

            return text.ToString();
        }
    }
}