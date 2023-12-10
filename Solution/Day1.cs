
public class Day1 : IRun
{
    public async Task Run_1()
    {
        var input = await File.ReadAllLinesAsync("input/day1.txt");
        var result = input.Select((line, iLine) =>
        {
            var digitStartChar = line.FirstOrDefault(c => int.TryParse(c.ToString(), out _));
            if (digitStartChar == default(char))
            {
                return 0;
            }
            var digitEndChar = line.LastOrDefault(c => int.TryParse(c.ToString(), out _));
            var result = int.Parse($"{digitStartChar}{digitEndChar}");
            Console.WriteLine($"Line: {iLine} => {result}");
            return result;
        }).Sum();
        Console.WriteLine($"Result: {result}");
    }

    private List<string> _validStrings = new()  {
        "one",
        "two",
        "three",
        "four",
        "five",
        "six",
        "seven",
        "eight",
        "nine",
        "1",
        "2",
        "3",
        "4",
        "5",
        "6",
        "7",
        "8",
        "9"
    };
    public async Task Run()
    {
        var input = await File.ReadAllLinesAsync("input/day1.txt");
        var result = input.Select((lineRaw, iLine) =>
        {
            var line = lineRaw.ToLowerInvariant();
            var digitStart = _validStrings.Select(x => (line.IndexOf(x), x)).Where(x=>x.Item1>=0).MinBy(x => x.Item1).x;
            var digitEnd = _validStrings.Select(x => (line.LastIndexOf(x), x)).MaxBy(x => x.Item1).x;
            var start = int.TryParse(digitStart, out _) ? int.Parse(digitStart) : (_validStrings.IndexOf(digitStart) + 1);
            var end = int.TryParse(digitEnd, out _) ? int.Parse(digitEnd) : (_validStrings.IndexOf(digitEnd) + 1);
            var result = int.Parse($"{start}{end}");
            Console.WriteLine($"Line: {iLine} => {result}");
            return result;
        }).Sum();
        Console.WriteLine($"Result: {result}");
    }
}