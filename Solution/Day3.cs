
using System.Text.RegularExpressions;

public partial class Day3 : IRun
{
    private readonly char[] _digits = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'];
    public async Task Run_1()
    {
        var input = await File.ReadAllLinesAsync("input/day3_1.txt");
        var result = input.Select((line, iLine) =>
        {
            var lineSum = 0;
            var numberIndexStart = -1;
            var numberIndexEnd = -1;
            for (var iChar = 0; iChar < line.Length; iChar++)
            {
                if (_digits.Contains(line[iChar]))
                {
                    if (numberIndexStart == -1)
                    {
                        numberIndexStart = iChar;
                        numberIndexEnd = iChar;
                        continue;
                    }
                    numberIndexEnd = iChar;
                    continue;
                }
                else if (numberIndexStart == -1)
                {
                    continue;
                }
                var (number, isSymbolAround) = ExtractAndCheckNumber(line, iLine, input, numberIndexStart, numberIndexEnd);
                numberIndexStart = -1;
                numberIndexEnd = -1;
                lineSum += isSymbolAround ? number : 0;
            }
            if (numberIndexStart != -1)
            {
                var (number, isSymbolAround) = ExtractAndCheckNumber(line, iLine, input, numberIndexStart, numberIndexEnd);
                lineSum += isSymbolAround ? number : 0;
            }
            Console.WriteLine($"Line: {iLine} => Sum: {lineSum}");
            return lineSum;
        }).Sum();
        Console.WriteLine($"Result: {result}");
    }

    private (int, bool) ExtractAndCheckNumber(string line, int iLine, string[] input, int numberIndexStart, int numberIndexEnd)
    {
        var number = int.Parse(line.Substring(numberIndexStart, numberIndexEnd - numberIndexStart + 1));
        var isSymbolAround = (
            IsSymbolAroundAtLine(iLine, input, numberIndexStart - 1, numberIndexStart - 1) ||
            IsSymbolAroundAtLine(iLine, input, numberIndexEnd + 1, numberIndexEnd + 1) ||
            IsSymbolAroundAtLine(iLine - 1, input, numberIndexStart - 1, numberIndexEnd + 1) ||
            IsSymbolAroundAtLine(iLine + 1, input, numberIndexStart - 1, numberIndexEnd + 1)
        );
        Console.WriteLine($"Line: {iLine} => Number at {numberIndexStart}: {number}, {isSymbolAround}");
        return (number, isSymbolAround);
    }

    private bool IsSymbolAroundAtLine(int iLine, string[] lines, int numberIndexStart, int numberIndexEnd)
    {
        if (iLine < 0 || iLine >= lines.Length)
            return false;
        for (var iChar = numberIndexStart; iChar <= numberIndexEnd; iChar++)
        {
            if (iChar < 0 || iChar >= lines[iLine].Length)
                continue;
            if (lines[iLine][iChar] == '.')
                continue;
            return true;
        }
        return false;
    }

    public async Task Run()
    {
        var input = await File.ReadAllLinesAsync("input/day3_2.txt");
        var result = input.Select((line, iLine) =>
        {
            var lineSum = 0;
            for (var iChar = 0; iChar < line.Length; iChar++)
            {
                if (line[iChar] != '*')
                    continue;
                var numbersAround = new List<int>();
                numbersAround.AddRange(FindNumbersInRange(iLine, input, iChar - 1, iChar - 1));
                numbersAround.AddRange(FindNumbersInRange(iLine, input, iChar + 1, iChar + 1));
                numbersAround.AddRange(FindNumbersInRange(iLine - 1, input, iChar - 1, iChar + 1));
                numbersAround.AddRange(FindNumbersInRange(iLine + 1, input, iChar - 1, iChar + 1));
                if (numbersAround.Count == 2) {
                    lineSum += numbersAround[0] * numbersAround[1];
                    Console.WriteLine($"Line: {iLine} => At {iChar}, {numbersAround[0]} * {numbersAround[1]}");
                }
            }
            Console.WriteLine($"Line: {iLine} => Sum: {lineSum}");
            return lineSum;
        }).Sum();
        Console.WriteLine($"Result: {result}");
    }

    private List<int> FindNumbersInRange(int iLine, string[] input, int lookupStart, int lookupEnd)
    {
        var result = new List<int>();
        if (iLine < 0 || iLine >= input.Length)
            return result;
        for (var iChar = lookupStart; iChar <= lookupEnd; iChar++)
        {
            if (iChar < 0 || iChar >= input[iLine].Length)
                continue;
            if (!_digits.Contains(input[iLine][iChar]))
                continue;
            var (number, numberCharStart, numberCharEnd) = GetWholeNumberAt(input[iLine], iChar);
            result.Add(number);
            iChar = numberCharEnd;
        }
        return result;
    }

    private (int number, int numberCharStart, int numberCharEnd) GetWholeNumberAt(string line, int iCharWithDigit)
    {
        var numberCharStart = iCharWithDigit;
        var numberCharEnd = iCharWithDigit;
        for (var iChar = iCharWithDigit - 1; iChar >= 0; iChar--)
        {
            if (!_digits.Contains(line[iChar]))
                break;
            numberCharStart = iChar;
        }
        for (var iChar = iCharWithDigit + 1; iChar < line.Length; iChar++)
        {
            if (!_digits.Contains(line[iChar]))
                break;
            numberCharEnd = iChar;
        }
        var number = int.Parse(line.Substring(numberCharStart, numberCharEnd - numberCharStart + 1));
        return (number, numberCharStart, numberCharEnd);
    }
}