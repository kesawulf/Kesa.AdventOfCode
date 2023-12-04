using Kesa.AdventOfCode.Common;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Kesa.AdventOfCode
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var runners = GetRunnerInfos().ToArray();

            if (args is [var yearText, var dayText])
            {
                if (true
                    && int.TryParse(yearText, out var year)
                    && int.TryParse(dayText, out var day)
                    && runners.FirstOrDefault(runner => runner.Year == year && runner.Day == day) is { } chosenRunner)
                {
                    var partNumber = args is { Length: 3 } ? args[^1] : null;
                    var chosenPart = partNumber == null
                        ? chosenRunner.Parts.FirstOrDefault(variant => variant.Number.ToString() == partNumber)
                        : chosenRunner.Parts.OrderBy(p => p.Number).Last();

                    if (chosenPart != null)
                    {
                        Execute(chosenRunner, chosenPart);
                        return;
                    }
                }

                Console.WriteLine("Invalid year and day arguments or invalid variant name.");
                PrintAvailable();
                return;
            }

            var choiceRegex = new Regex(@"(?<year>\d+)-(?<day>\d+)(?:.(?<part>\d+))?");

            while (true)
            {
                PrintAvailable();
                Console.WriteLine("Enter an AoC (Year-Day.Part) or leave blank for latest:");
                var chosenRunnerDescription = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(chosenRunnerDescription))
                {
                    var runner = runners.Last();
                    chosenRunnerDescription = runner.Description + "." + runner.Parts.OrderBy(p => p.Number).Last().Number;
                }

                if (true
                    && choiceRegex.Match(chosenRunnerDescription) is { Success: true } match
                    && match.TryGetGroup<int>("year", out var year)
                    && match.TryGetGroup<int>("day", out var day))
                {
                    if (runners.FirstOrDefault(runner => runner.Year == year && runner.Day == day) is not { } chosenRunner)
                    {
                        continue;
                    }

                    var chosenPart = chosenRunner.Parts.OrderBy(p => p.Number).LastOrDefault();

                    var isSpecificPart = match.TryGetGroup<int>("part", out var part);
                    if (isSpecificPart)
                    {
                        chosenPart = chosenRunner.Parts.FirstOrDefault(p => p.Number == part);
                    }

                    if (chosenPart == null)
                    {
                        continue;
                    }

                    Execute(chosenRunner, chosenPart);
                    Console.ReadLine();
                }
            }

            void PrintAvailable()
            {
                Console.Clear();
                Console.WriteLine("Available AoCs:");

                foreach (var runner in runners)
                {
                    Console.WriteLine($"    - {runner.VariantDescription}");
                }

                Console.WriteLine("");

            }
        }

        public static Type[] GetApplicableTypes()
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => type.GetInterface(nameof(IAocRunner)) != null)
                .ToArray();
        }

        public static IEnumerable<AocRunnerInfo> GetRunnerInfos()
        {
            var regex = new Regex(@"Aoc(?<year>\d+).Day(?<day>\d+)");
            var partRegex = new Regex(@"RunPart(?<part>\w+)");

            return GetApplicableTypes()
                .Select(type => (Type: type, Match: regex.Match($"{type.Namespace}.{type.Name}")))
                .Where(pair => pair.Match.Success)
                .Select(pair => (
                    pair.Type,
                    Year: pair.Match.GetGroup<int>("year"),
                    Day: pair.Match.GetGroup<int>("day")
                ))
                .Select(tuple => GetRunner(tuple.Type, tuple.Year, tuple.Day))
                .OrderBy(runner => runner.Year)
                .ThenBy(runner => runner.Day);

            AocRunnerInfo GetRunner(Type type, int year, int day)
            {
                var parts = new List<AocRunnerPartInfo>();

                foreach (var method in type.GetMethods())
                {
                    if (partRegex.Match(method.Name) is { Success: true } match)
                    {
                        parts.Add(new AocRunnerPartInfo(method, match.GetGroup<int>("part")));
                    }
                }

                return new AocRunnerInfo(type, year, day, parts.OrderBy(p => p.Number).ToArray());
            }
        }

        public static void Execute(AocRunnerInfo runner, AocRunnerPartInfo part)
        {
            var method = part.Method;
            var input = GetTextFromResource();

            if (runner.Parts is not { Length: > 1 })
            {
                Console.WriteLine($"Executing AoC {runner.Description}");
            }
            else
            {
                Console.WriteLine($"Executing AoC {runner.Description} ({part.Number})");
            }

            Console.WriteLine("");

            var stopwatch = Stopwatch.StartNew();
            var result = method.Invoke(null, [input])?.ToString() ?? "[ null ]";

            Console.WriteLine($"Took {stopwatch.ElapsedMilliseconds}ms.");
            Console.WriteLine("");
            Console.WriteLine($"Result Length: {result.Length}");
            Console.WriteLine("");
            Console.WriteLine($"Result: {result}");

            string GetTextFromResource()
            {
                var assembly = Assembly.GetExecutingAssembly();

                var resourceName = assembly
                    .GetManifestResourceNames()
                    .FirstOrDefault(x => x.EndsWith("." + runner.InputPath, StringComparison.CurrentCultureIgnoreCase))!;

                using var stream = assembly.GetManifestResourceStream(resourceName)!;
                using var reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
        }
    }

    public record AocRunnerInfo(Type Type, int Year, int Day, AocRunnerPartInfo[] Parts)
    {
        public string Description { get; } = $"{Year}-{Day:D2}";

        public string VariantDescription => Parts is { Length: > 1 }
            ? $"{Description} ({string.Join("|", Parts.Select(v => v.Number))})"
            : Description;

        public string InputPath { get; } = $"Aoc{Year}.Input.Day{Day:D2}.txt";
    }

    public record AocRunnerPartInfo(MethodInfo Method, int Number);
}
