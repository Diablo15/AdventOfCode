using iAM.AdventOfCode._2021.Models;
using iAM.AdventOfCode.Helpers;

namespace iAM.AdventOfCode._2021;

public class Day7 : AocDay
{
    private IEnumerable<int> PuzzleOneMeasurements { get; set; }
    private long? FuelAmount { get; set; } = null;

    public Day7() : base(7, false, true)
    {
        this.PuzzleAltFilePath = "..//Examples//Day7Puzzle1Alt.txt";
        this.UseAltFile = false;
    }

    public override void Puzzle1Content()
    {
        CreatePuzzleData();
        LoopOverValues();
        // Answer: 340056
        Console.WriteLine($"Result -- Least spend fuel to position {this.FuelAmount} ");
    }

    public override void Puzzle2Content()
    {
        CreatePuzzleData();
        LoopOverValuesExtra();
        // Answer: 96592275
        Console.WriteLine($"Result -- Least spend fuel to position {this.FuelAmount} ");
    }

    private void CreatePuzzleData()
    {
        var file = FileReader.ReadInputValues<string>(this.Puzzle1FilePath, "\n");
        this.PuzzleOneMeasurements = FileReader.ValuesSplitter<int>(file.First(), ",");
    }

    private void LoopOverValues()
    {
        foreach (var position in this.PuzzleOneMeasurements)
        {
            var total = this.PuzzleOneMeasurements.Sum(pos => Math.Abs(position - pos));

            if (this.FuelAmount is null || total < this.FuelAmount.Value)
            {
                this.FuelAmount = total;
            }
        }
    }
    
    private void LoopOverValuesExtra()
    {
        int min = this.PuzzleOneMeasurements.Min();
        int max = this.PuzzleOneMeasurements.Max();

        for (int position = min; position <= max; position++)
        {
            long total = 0;
            foreach (var pos in this.PuzzleOneMeasurements)
            {
                long steps = Math.Abs(position - pos);
                total += steps * (steps + 1) / 2;
            }

            if (this.FuelAmount is null || total < this.FuelAmount.Value)
            {
                this.FuelAmount = total;
            }
        }
    }

    private void Logging()
    {
    }
}