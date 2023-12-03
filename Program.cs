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
                    Execute(chosenRunner);
                    return;
                }

                Console.WriteLine("Invalid year and day arguments.");
                PrintAvailable();
                return;
            }

            while (true)
            {
                PrintAvailable();
                Console.WriteLine("Enter an AoC or leave blank for latest:");
                var chosenRunnerDescription = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(chosenRunnerDescription))
                {
                    chosenRunnerDescription = runners.Last().Description;
                }

                if (runners.FirstOrDefault(runner => runner.Description == chosenRunnerDescription) is { } chosenRunner)
                {
                    Execute(chosenRunner);
                    return;
                }
            }

            void PrintAvailable()
            {
                Console.Clear();
                Console.WriteLine("Available AoCs:");

                foreach (var runner in runners)
                {
                    Console.WriteLine($"    - {runner.Description}");
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

            return GetApplicableTypes()
                .Select(type => (Type: type, Match: regex.Match($"{type.Namespace}.{type.Name}")))
                .Where(pair => pair.Match.Success)
                .Select(pair => new AocRunnerInfo(pair.Type, int.Parse(pair.Match.Groups["year"].Value), int.Parse(pair.Match.Groups["day"].Value)))
                .OrderBy(runner => runner.Year)
                .ThenBy(runner => runner.Day);
        }

        public static void Execute(AocRunnerInfo runnerInfo)
        {
            var method = runnerInfo.Type.GetMethod(nameof(IAocRunner.Run))!;
            var input = GetTextFromResource();

            Console.WriteLine($"Executing AoC {runnerInfo.Description}");
            Console.WriteLine("");

            var stopwatch = Stopwatch.StartNew();
            var result = (string)method.Invoke(null, [input])!;

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
                    .FirstOrDefault(x => x.EndsWith("." + runnerInfo.InputPath, StringComparison.CurrentCultureIgnoreCase))!;

                using var stream = assembly.GetManifestResourceStream(resourceName)!;
                using var reader = new StreamReader(stream);
                return reader.ReadToEnd();
            }
        }
    }

    public record AocRunnerInfo(Type Type, int Year, int Day)
    {
        public string Description { get; } = $"{Year}-{Day:D2}";

        public string InputPath { get; } = $"Aoc{Year}.Input.Day{Day:D2}.txt";
    }
}
