using Kesa.AdventOfCode.Common;
using MoreLinq;
using System.Collections.Frozen;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace Kesa.AdventOfCode.Aoc2024;

internal class Day05 : IAocRunner
{
    public static object RunPart1(string input)
    {
        var (rules, updates) = Parse(input);
        var total = 0;

        foreach (var update in updates)
        {
            var valid = true;

            for (int x = 0; x < update.Pages.Length; x++)
            {
                for (int y = x; y < update.Pages.Length; y++)
                {
                    if (rules.Contains((update.Pages[y], update.Pages[x])))
                    {
                        valid = false;
                        goto AfterLoop;
                    }
                }
            }

        AfterLoop:

            if (valid)
            {
                total += update.Pages[update.Pages.Length / 2];
            }
        }

        return total;
    }

    public static object RunPart2(string input)
    {
        var (rules, updates) = Parse(input);
        var total = 0;

        foreach (var update in updates)
        {
            var pages = update.Pages;
            var valid = true;

            for (int x = 0; x < pages.Length; x++)
            {
                for (int y = x; y < pages.Length; y++)
                {
                    if (rules.Contains((pages[y], pages[x])))
                    {
                        valid = false;
                        pages.Swap(y, y - 1);
                        x -= 1;
                        break;
                    }
                }
            }

            if (!valid)
            {
                total += pages[pages.Length / 2];
            }
        }

        return total;
    }

    private static (FrozenSet<(int Required, int Page)> Rules, ManualUpdate[] Update) Parse(string input)
    {
        var rules = new HashSet<(int, int)>();
        var updates = new List<ManualUpdate>();
        var processingRules = true;

        foreach (var line in input.Lines())
        {
            if (processingRules)
            {
                if (line.Split('|') is [var requirementText, var pageText])
                {
                    rules.Add((int.Parse(requirementText), int.Parse(pageText)));
                }
                else
                {
                    processingRules = false;
                }
            }
            else
            {
                updates.Add(new ManualUpdate(line.SplitIntoInts(',').ToArray()));
            }
        }

        return (rules.ToFrozenSet(), updates.ToArray());
    }

    private record ManualUpdate(int[] Pages);
}
