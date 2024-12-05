using iAM.AdventOfCode.Helpers;
using System.Text.RegularExpressions;

namespace iAM.AdventOfCode._2023.Flaws;

public class Day3
{
    private IEnumerable<string> PuzzleOneMeasurements { get; set; }

    private string Puzzle1FilePath = "Day3Puzzle1.txt";

    private Regex DigitRegex = new(@"(\d)+");

    public Day3()
    {
        PuzzleOneMeasurements = new List<string>();
    }

    public void StartDay3()
    {
        Console.WriteLine("******** Day 3 ********");
        Puzzle1();
    }

    private void Puzzle1()
    {
        Console.WriteLine("******** Puzzle 1 ********");
        PuzzleOneMeasurements = FileReader.ReadInputValues<string>(Puzzle1FilePath, '\n');

        var results = GetAllPartNumbers();
        var sum = results.Sum(x => x);
        
        Console.WriteLine($"1. ======== Total Sum == {sum}");
    }

    private IEnumerable<int> GetAllPartNumbers()
    {
        var parts = new List<int>();
        var regex = new Regex(@"[^\w\s.]");

        MatchCollection previousSymbolMatches = null;
        var previousLength = 0;

        foreach (var line in PuzzleOneMeasurements)
        {
            var maxLenght = CheckMaxDigitsLength(line);
            var matches = regex.Matches(line);

            foreach (Match match in matches) parts.AddRange(CheckCurrentLine(match, line, maxLenght));

            parts.AddRange(CheckPreviousLine(previousSymbolMatches, line, previousLength));

            previousSymbolMatches = matches;
            previousLength = maxLenght;
        }
        
        return parts;
    }

    private IEnumerable<int> CheckPreviousLine(MatchCollection matches, string line, int length)
    {
        var numbers = new List<int>();

        if (matches is null)
        {
            return numbers;
        }

        foreach (Match match in matches)
        {
            var leftMatch = DigitRegex.Match(line, match.Index + 1, length);
            var leftNumber = ConvertToInt(leftMatch.Value);

            if (leftNumber is not null)
            {
                numbers.Add(leftNumber.Value);
            }
            
            var rightMatch = DigitRegex.Match(line, match.Index + 1, length);
            var rightNumber = ConvertToInt(rightMatch.Value);

            if (rightNumber is not null)
            {
                numbers.Add(rightNumber.Value);
            }

            var equalDiv = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(length - 1)));
            var directMatch = DigitRegex.Match(line, match.Index - equalDiv, length + equalDiv);
            
            if(rightMatch.Value.Contains(directMatch.Value) || leftMatch.Value.Contains(directMatch.Value))
            {
                continue;
            }
            
            var directNumber = ConvertToInt(directMatch.Value);

            if (directNumber is not null)
            {
                numbers.Add(directNumber.Value);
            }
        }

        return numbers;
    }

    private IEnumerable<int> CheckCurrentLine(Match match, string line, int length)
    {
        var numbers = new List<int>();

        var rightMatch = DigitRegex.Match(line, match.Index + 1, length);
        var rightNumber = ConvertToInt(rightMatch.Value);

        if (rightNumber is not null)
        {
            numbers.Add(rightNumber.Value);
        }

        var leftMatch = DigitRegex.Match(line, match.Index - length, length);
        var leftNumber = ConvertToInt(leftMatch.Value);

        if (leftNumber is not null)
        {
            numbers.Add(leftNumber.Value);
        }

        return numbers;
    }

    private int CheckMaxDigitsLength(string line)
    {
        var matches = DigitRegex.Matches(line);
        var maxLength = 0;
        foreach (Match match in matches)
        {
            if (maxLength < match.Value.Length)
            {
                maxLength = match.Value.Length;
            }
        }

        return maxLength;
    }

    private int? ConvertToInt(string input)
    {
        if (!string.IsNullOrEmpty(input))
        {
            return Convert.ToInt32(input);
        }

        return null;
    }
}