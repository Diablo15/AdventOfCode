namespace iAM.AdventOfCode._2021;

public class Day1
{
    private IEnumerable<int> PuzzleOneMeasurements { get; set; }
    
    private IFileReader Reader { get; set; }
    
    private string Puzzle1FilePath { get; set; } = "Day1Puzzle1.csv";

    public Day1(IFileReader reader)
    {
        this.Reader = reader;
        this.PuzzleOneMeasurements = new List<int>();
    }
    
    public void StartDay1()
    {
        Console.WriteLine("******** Day 1 ********");
        this.Puzzle1();
        this.Puzzle2();
    }
    
    private void Puzzle1()
    {
        Console.WriteLine("******** Puzzle 1 ********");
        
        this.PuzzleOneMeasurements = this.Reader.ReadInputValues<int>(this.Puzzle1FilePath, ';');
        
        var total = this.CheckIncreasedMeasurements(this.PuzzleOneMeasurements.ToList());
        
        Console.WriteLine($"1.========================== Total increased values: {total}");
    }
    
    private void Puzzle2()
    {
        Console.WriteLine("******** Puzzle 2 ********");

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