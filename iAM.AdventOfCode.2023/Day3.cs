using iAM.AdventOfCode.Helpers;
using System.Collections;
using System.Text.RegularExpressions;

namespace iAM.AdventOfCode._2023;

public class Day3
{
    private IEnumerable<string> PuzzleOneMeasurements { get; set; }

    private string Puzzle1FilePath = "Day3Puzzle1.txt";

    private Regex DigitRegex = new(@"(\d)+");
    
    private List<Symbol> Symbols { get; set; }

    private List<Number> Numbers { get; set; }

    public Day3()
    {
        PuzzleOneMeasurements = new List<string>();
        Symbols = new List<Symbol>();
        Numbers = new List<Number>();
    }

    public void StartDay3()
    {
        Console.WriteLine("******** Day 3 ********");
        Puzzle1();
        Puzzle2();
    }

    private void Puzzle1()
    {
        Console.WriteLine("******** Puzzle 1 ********");
        PuzzleOneMeasurements = FileReader.ReadInputValues<string>(Puzzle1FilePath, '\n');

        var results = (GetPartNumbers()).ToList();
        var sum = results.Sum(x => x.Value);
        
        Console.WriteLine($"1. ======== Total Sum == {sum}");
    }
    
    private void Puzzle2()
    {
        Console.WriteLine("******** Puzzle 2 ********");
        
        var sum = GetGearRatios();
        
        Console.WriteLine($"2. ======== Total Sum == {sum}");
    }

   record Pos(int Row, int ColStart, int ColEnd)
    {
        public bool IsAdjacent(Number number)
        {
            // same row, directly above or below
            if (Row < number.Pos.Row - 1 || Row > number.Pos.Row + 1)
                return false;
            // within or directly adjacent to the number's column range
            return ColStart >= number.Pos.ColStart - 1 && ColStart <= number.Pos.ColEnd + 1;
        }
    };

    record Symbol(string Value, Pos Pos);

    record Number(int Value, Pos Pos);

    private IEnumerable<Number> GetPartNumbers()
    {
        var symbolRegex = new Regex(@"[^\.\d\n]");

        var lineNum = 0;
        foreach (var line in this.PuzzleOneMeasurements.ToList())
        {
            foreach (Match nm in this.DigitRegex.Matches(line))
                Numbers.Add(new Number(
                    Value: int.Parse(nm.Value),
                    Pos: new Pos(lineNum, nm.Index, nm.Index + nm.Length - 1)));

            foreach (Match sm in symbolRegex.Matches(line))
                Symbols.Add(new Symbol(
                    Value: sm.Value,
                    Pos: new Pos(lineNum, sm.Index, sm.Index)));

            lineNum++;
        }

        return Numbers.Where(n => Symbols.Any(s => s.Pos.IsAdjacent(n)));
    }

    private int GetGearRatios()
    {
        return Symbols
            .Where(s => s.Value is "*")
            .Sum(g =>
            {
                var adjacentNumbers = Numbers
                    .Where(n => g.Pos.IsAdjacent(n))
                    .ToList();
                return adjacentNumbers.Count == 2
                    ? adjacentNumbers.Aggregate(1, (acc, n) => acc * n.Value)
                    : 0;
            });
    }
}