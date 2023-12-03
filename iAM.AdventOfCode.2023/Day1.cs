using System.Text.RegularExpressions;
using iAM.AdventOfCode._2023.Helpers;

namespace iAM.AdventOfCode._2023;

public class Day1
{
    private IEnumerable<string> PuzzleOneMeasurements { get; set; }

    private IFileReader Reader { get; set; }

    private string Puzzle1FilePath { get; set; } = "Day1Puzzle1.txt";

    public Day1(IFileReader reader)
    {
        Reader = reader;
        PuzzleOneMeasurements = new List<string>();
    }

    public void StartDay1()
    {
        Console.WriteLine("******** Day 1 ********");
        Puzzle1();
        Puzzle2();
    }

    private void Puzzle1()
    {
        Console.WriteLine("******** Puzzle 1 ********");
        PuzzleOneMeasurements = Reader.ReadInputValues<string>(Puzzle1FilePath);

        var result = FindFirstLastDigit(PuzzleOneMeasurements.ToList());
        var sum = result.Sum(x => x);
        Console.WriteLine($"1. ======== Total Sum == {sum}");
    }

    private void Puzzle2()
    {
        Console.WriteLine("******** Puzzle 2 ********");
        PuzzleOneMeasurements = Reader.ReadInputValues<string>(Puzzle1FilePath);

        var correctedValues = ReplaceWrittenValues(PuzzleOneMeasurements.ToList());
        var result = FindFirstLastDigit(correctedValues.ToList());
        var sum = result.Sum(x => x);
        Console.WriteLine($"2. ======== Total Sum == {sum}");
    }

    private IEnumerable<string> ReplaceWrittenValues(List<string> input)
    {
        var results = input;
        var words = new Dictionary<string, string>
        {
            { "one", "1" },
            { "two", "2" },
            { "three", "3" },
            { "four", "4" },
            { "five", "5" },
            { "six", "6" },
            { "seven", "7" },
            { "eight", "8" },
            { "nine", "9" },
            { "zero", "0" },
        };

        var pattern = string.Join("|", words.Keys.Select(Regex.Escape));
        var regex = new Regex(pattern);
        
        do
        {
           results = LoopMatchReplace(results, regex, words).ToList();
        } while (results.Any(line => regex.IsMatch(line)));

        return results;
    }

    private IEnumerable<string> LoopMatchReplace(List<string> input, Regex regex, Dictionary<string, string> replace)
    {
        var output = new List<string>();
        foreach (var line in input)
        {
            var matches = regex.Matches(line);
            var repl = line;
            foreach (Match match in matches)
            {
                repl = line.Insert(match.Index+1, replace[match.Value]);
            }
            
            output.Add(repl);
        }

        return output;
    }

    private IEnumerable<int> FindFirstLastDigit(List<string> input)
    {
        var digitRegex = new Regex(@"[\d]");
        var digits = new List<int>();

        foreach (var line in input)
        {
            var matches = digitRegex.Matches(line);
            var i = string.Empty;

            foreach (Match match in matches) i += match.Groups[0].Value;
            var result = $"{i.First()}{i.Last()}";
            Console.WriteLine($"Found: {i} - First/Last: {result}");
            digits.Add(Convert.ToInt32(result));
        }

        return digits;
    }
}