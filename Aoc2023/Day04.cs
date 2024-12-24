using Kesa.AdventOfCode.Common;
using System.Runtime.InteropServices;

namespace Kesa.AdventOfCode.Aoc2023
{
    public record Day04Scratchcard(int Id, HashSet<int> WinningNumbers, int[] ScratchedNumbers)
    {
        public int Wins { get; } = ScratchedNumbers.Count(WinningNumbers.Contains);
    }

    internal class Day04 : IAocRunner
    {
        internal static readonly char[] CardSeparators = [':', '|'];

        public static Day04Scratchcard ProcessCard(string card)
        {
            var data = card.Split(CardSeparators);
            var cardId = int.Parse(data[0].Split(' ')[^1]);
            var winningNumbers = data[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToHashSet();
            var numbers = data[2].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

            return new Day04Scratchcard(cardId, winningNumbers, numbers);
        }

        public static object RunPart1(string input)
        {
            input = input.Replace("|", ":");

            var answer = 0;

            foreach (var line in input.Lines())
            {
                var card = ProcessCard(line);
                var wins = card.Wins;
                var cardTotal = Math.Min(wins, 1);

                for (; wins > 1; wins--)
                {
                    cardTotal *= 2;
                }

                answer += cardTotal;
            }

            return answer.ToString();
        }

        public static object RunPart2(string input)
        {
            var answer = 0;
            var original = input.Lines().Select(ProcessCard).ToArray();
            var counts = original.ToDictionary(card => card.Id, _ => 1);

            for (int index = 0; index < original.Length; index++)
            {
                var card = original[index];
                var wins = card.Wins;
                var iterations = counts[card.Id];

                for (int iter = 0; iter < iterations; iter++)
                {
                    answer++;

                    for (int offset = 0; offset < wins; offset++)
                    {
                        CollectionsMarshal.GetValueRefOrAddDefault(counts, original[index + offset + 1].Id, out _)++;
                    }
                }
            }

            return answer.ToString();
        }
    }
}
