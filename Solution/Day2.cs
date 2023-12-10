
using System.Text.RegularExpressions;

public partial class Day2 : IRun
{
    // sample line:
    // Game 1: 19 blue, 12 red; 19 blue, 2 green, 1 red; 13 red, 11 blue
    public async Task Run_1()
    {
        var input = await File.ReadAllLinesAsync("input/day2.txt");
        var result = input.Select((line, iLine) =>
        {
            var gameNumber = int.Parse(GameNumber().Match(line).Groups[1].Value);
            var dataSet = line.Split(':')[1].Trim();
            foreach (var repeat in dataSet.Split(';').Select(s => s.Trim()))
            {
                foreach (var match in RegexBallSet().Matches(repeat).ToList())
                {
                    var count = int.Parse(match.Groups[1].Value);
                    var color = match.Groups[2].Value;
                    switch (color)
                    {
                        case "red" when count > 12:
                        case "green" when count > 13:
                        case "blue" when count > 14:
                            return 0;
                        default:
                            break;
                    }
                }
            }
            return gameNumber;
        }).Sum();
        Console.WriteLine($"Result: {result}");
    }

    public async Task Run()
    {
        var input = await File.ReadAllLinesAsync("input/day2.txt");
        var result = input.Select((line, iLine) =>
        {
            var gameNumber = int.Parse(GameNumber().Match(line).Groups[1].Value);
            var dataSet = line.Split(':')[1].Trim();
            return dataSet.Split(';')
            .Select(s => s.Trim())
            .SelectMany(
                repeat => RegexBallSet()
                .Matches(repeat)
                .Select(match =>
                {
                    var count = int.Parse(match.Groups[1].Value);
                    var color = match.Groups[2].Value;
                    return (count, color);
                })
            )
            .GroupBy(x => x.color)
            .Select(x => (x.Key, x.Max(y => y.count)))
            .Aggregate(0, (acc, x) => acc == 0 ? x.Item2 : acc * x.Item2);
        }).Sum();


        Console.WriteLine($"Result: {result}");
    }

    [GeneratedRegex(@"Game (\d+):")]
    private static partial Regex GameNumber();

    [GeneratedRegex(@"(\d+) (\w+)")]
    private static partial Regex RegexBallSet();
}