using iAM.AdventOfCode.Helpers;

namespace iAM.AdventOfCode._2021;

public class Day1 : AocDay
{
    private IEnumerable<int> PuzzleOneMeasurements { get; set; }
    
    public Day1() : base(1, true, true)
    {
        this.PuzzleOneMeasurements = new List<int>();
    }

    public override void Puzzle1Content()
    {
        this.PuzzleOneMeasurements = FileReader.ReadInputValues<int>(this.Puzzle1FilePath, ";");
        
        var total = this.CheckIncreasedMeasurements(this.PuzzleOneMeasurements.ToList());
        
        Console.WriteLine($"1.========================== Total increased values: {total}");
    }
    
    public override void Puzzle2Content()
    {
        var result = this.EnrichMeasurements(this.PuzzleOneMeasurements.ToList());
        
        var total = this.CheckIncreasedMeasurements(result.ToList());
        
        Console.WriteLine($"2.========================== Total increased values: {total}");
    }
    
    private int CheckIncreasedMeasurements(List<int> inputList)
    {
        var oldMeasurement = inputList.First();
        var increasingCount = 0;
                
        foreach (var measurement in inputList)
        {
            if (measurement < oldMeasurement)
            {
                Console.WriteLine($"{measurement} (decreased)");
            }
            else if (measurement > oldMeasurement)
            {
                increasingCount++;
                Console.WriteLine($"{measurement} (increased)");
            }
            else
            {
                Console.WriteLine($"{measurement} (N/A - no previous measurement)");
            }

            oldMeasurement = measurement;
        }

        return increasingCount;
    }
    
    private IEnumerable<int> EnrichMeasurements(List<int> input)
    {
        var list = new List<int>();

        for (int i = 0; i <= input.Count; i++)
        {
            try
            {
                var threeView = input.GetRange(i, 3);
                    
                var addition = threeView[0] + threeView[1] + threeView[2];
                Console.WriteLine($"{addition} = {threeView[0]} + {threeView[1]} + {threeView[2]}");

                list.Add(addition);
            }
            catch
            {
                continue;
            }
        }

        return list;
    }
}