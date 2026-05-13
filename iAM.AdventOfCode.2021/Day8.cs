using iAM.AdventOfCode._2021.Models;
using iAM.AdventOfCode.Helpers;

namespace iAM.AdventOfCode._2021;

public class Day8 : AocDay
{
    private IEnumerable<MixedNumbers> PuzzleOneMeasurements { get; set; } = new List<MixedNumbers>();
    private List<string> SignalPatterns { get; set; } = new();
    private List<string> OutputPatterns { get; set; } = new();
    private long? FuelAmount { get; set; } = null;

    public Day8() : base(8, true, false)
    {
        this.PuzzleAltFilePath = "..//Examples//Day8Puzzle1Alt.txt";
        this.UseAltFile = true;
    }

    public override void Puzzle1Content()
    {
        CreatePuzzleData();

        var total = FindAllDigits();
        // Answer: 416
        Console.WriteLine($"Result -- Total of easy number segments {total.Count()} ");
    }

    public override void Puzzle2Content()
    {
        throw new NotImplementedException();
    }

    private void CreatePuzzleData()
    {
        var file = FileReader.ReadInputValues<string>(this.Puzzle1FilePath, "\n");
        var results = new List<MixedNumbers>();
        foreach (var line in file)
        {
          var readings =  FileReader.ValuesSplitter<string>(line, " ");
          this.SignalPatterns = readings.SkipLast(4).ToList();
          this.SignalPatterns.Remove("|");

          this.OutputPatterns = readings.TakeLast(4).ToList();
          results.Add(new MixedNumbers() { SegmentSignal = this.SignalPatterns, SegmentOutput = this.OutputPatterns });
        }

        this.PuzzleOneMeasurements = results;
    }

    private IEnumerable<SegmentDisplay> FindAllDigits()
    {
        var output = new List<SegmentDisplay>();
        foreach (var numbers in this.PuzzleOneMeasurements)
        {
            foreach (var segment in numbers.SegmentOutput)
            {
                var number = FromModuleLength(segment.ToCharArray());

                if (number is null)
                {
                    // to
                    numbers.DecodedOutput.Add(segment);
                    continue;
                }
                
                // to
                numbers.DecodedOutput.Add(number.TurnedOnModules.ToString());
                output.Add(number);
            }
            
        }

        return output;
    }
    
    public SegmentDisplay? FromModuleLength(char[] input)
    {
        var lengthInput = input.Length;

        List<SegmentDisplay> candidates = new()
        {
            new NumberOne(),
            new NumberFour(),
            new NumberSeven(),
            new NumberEight(),
        };

        return candidates.FirstOrDefault(n => 
            n.TurnedOnModulesCount.Equals(lengthInput));
    }
    
    public SegmentDisplay? FromModule(char[] input)
    {
        var sortedInput = input.OrderBy(c => c).ToArray();

        List<SegmentDisplay> candidates = new()
        {
            new NumberOne(),
            new NumberTwo(),
            new NumberThree(),
            new NumberFour(),
            new NumberFive(),
            new NumberSix(),
            new NumberSeven(),
            new NumberEight(),
            new NumberNine(),
            new NumberZero()
        };

        return candidates.FirstOrDefault(n => 
            n.TurnedOnModules.OrderBy(c => c).SequenceEqual(sortedInput));
    }
    
    private void Logging()
    {
    }
}