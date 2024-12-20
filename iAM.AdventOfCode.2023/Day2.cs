using System.Text.RegularExpressions;
using iAM.AdventOfCode._2023.Models;
using iAM.AdventOfCode.Helpers;

namespace iAM.AdventOfCode._2023;

public class Day2
{
    private IEnumerable<string> PuzzleOneMeasurements { get; set; }

    private List<Game> GameData { get; set; }

    private Set MatchingSet = new() { Blue = 14, Red = 12, Green = 13 };
    
    private string Puzzle1FilePath = "Day2Puzzle1.txt";

    public Day2()
    {
        PuzzleOneMeasurements = new List<string>();
    }

    public void StartDay2()
    {
        Console.WriteLine("******** Day 2 ********");
        Puzzle1();
        Puzzle2();
    }

    private void Puzzle1()
    {
        Console.WriteLine("******** Puzzle 1 ********");
        PuzzleOneMeasurements = FileReader.ReadInputValues<string>(Puzzle1FilePath, '\n');
        GameData = CreateGameData(this.PuzzleOneMeasurements.ToList()).ToList();
        var result = CheckPossibleGames(GameData);
        var sum = result.Sum(x => x);
        Console.WriteLine($"1. ======== Total Sum == {sum}");
    }
    
    private void Puzzle2()
    {
        Console.WriteLine("******** Puzzle 2 ********");
        var data = CheckFewestCubesNeeded(this.GameData);
        
        var sum = data.Sum(x => (x.Blue * x.Green * x.Red));
        
        Console.WriteLine($"2. ======== Total Sum == {sum}");
    }

    private IEnumerable<Set> CheckFewestCubesNeeded(List<Game> input)
    {
        var lowestSets = new List<Set>(); 
        foreach (var game in input)
        {
            var lowSet = new Set
            {
                Blue = 0,
                Green = 0,
                Red = 0
            };
            
            foreach (var set in game.Sets)
            {
                if (set.Blue > lowSet.Blue && set.Blue != 0)
                {
                    lowSet.Blue = set.Blue;
                } 
                
                if (set.Green > lowSet.Green && set.Green != 0)
                {
                    lowSet.Green = set.Green;
                }
                
                if (set.Red > lowSet.Red  && set.Red != 0)
                {
                    lowSet.Red = set.Red;
                }
            }
            
            lowestSets.Add(lowSet);
        }

        return lowestSets;
    }
        
    private IEnumerable<int> CheckPossibleGames(List<Game> input)
    {
        var result = new List<int>();
        foreach (var game in input)
        {
            var possible = CheckPossibleSet(game.Sets);
            if (possible)
            {
                result.Add(game.Id);
            }
        }

        return result;
    }

    private bool CheckPossibleSet(List<Set> input)
    {
        foreach (var set in input)
        {
            if (set.Blue > MatchingSet.Blue || set.Green > MatchingSet.Green || set.Red > MatchingSet.Red)
            {
                return false;
            }
        }

        return true;
    }
    
    private IEnumerable<Game> CreateGameData(List<string> input)
    {
        return input.Select(line => 
            new Game()
            {
                Id = ExtractDigit(line),
                Sets = ExtractGameSet(line).ToList()
            }).ToList();
    }

    private IEnumerable<Set> ExtractGameSet(string input)
    {
        const string delimiter = ":";
        var indexDelimiter = input.IndexOf(delimiter, StringComparison.Ordinal);
        var data = input[(indexDelimiter+delimiter.Length)..].Trim();
        var sets = FileReader.ValuesSplitter<string>(data, ';').ToList();

        return ExtractSetData(sets);
    }

    private IEnumerable<Set> ExtractSetData(List<string> input)
    {
        var sets = new List<Set>();
        foreach (var line in input)
        {
            var set = new Set();
            var data = FileReader.ValuesSplitter<string>(line, ',').ToList();

            foreach (var color in data)
            {
                var regexRed = new Regex("red");
                var regexBlue = new Regex("blue");
                var regexGreen = new Regex("green");

                if (regexRed.IsMatch(color))
                {
                    set.Red = ExtractDigit(color);
                }
                
                if (regexBlue.IsMatch(color))
                {
                    set.Blue = ExtractDigit(color);
                }

                if (regexGreen.IsMatch(color))
                {
                    set.Green = ExtractDigit(color);
                }
            }
            
            sets.Add(set);
        }
        
        return sets;
    }
    
    private int ExtractDigit(string input)
    {
        var regex = new Regex(@"\d+");
        var match = regex.Match(input);

        return Convert.ToInt32(match.Value);
    }
}