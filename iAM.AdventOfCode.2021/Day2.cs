using iAM.AdventOfCode.Helpers;

namespace iAM.AdventOfCode._2021;

public class Day2
{
    private string Puzzle1FilePath { get; set; } = "Day2Puzzle1.csv";
    
    private IEnumerable<Tuple<string, int>> PuzzleOneMeasurements { get; set; }
    
    public Day2()
    {
        this.PuzzleOneMeasurements = new List<Tuple<string, int>>();
    }
    
    public void StartDay2()
    {
        Console.WriteLine("******** Day 2 ********");
        this.Puzzle1();
        this.Puzzle2();
    }
    
    private void Puzzle1()
    {
        Console.WriteLine("******** Puzzle 1 ********");
        
        this.PuzzleOneMeasurements = FileReader.ReadInputValues<string, int>(this.Puzzle1FilePath);
        var directions = this.GetDirectionValues(this.PuzzleOneMeasurements.ToList());
        
        Console.WriteLine($"=========================== Total depth: {directions["depth"]}");
        Console.WriteLine($"=========================== Total course: {directions["horizontal"]}");
        Console.WriteLine($"=========================== Total answer: {directions["horizontal"] * directions["depth"]}");
    }
    
    private void Puzzle2()
    {
        Console.WriteLine("******** Puzzle 2 ********");
        
        this.PuzzleOneMeasurements = FileReader.ReadInputValues<string, int>(this.Puzzle1FilePath);
        var directions = this.GetAimedDirectionValues(this.PuzzleOneMeasurements.ToList());
        
        Console.WriteLine($"=========================== Total depth: {directions["depth"]}");
        Console.WriteLine($"=========================== Total course: {directions["horizontal"]}");
        Console.WriteLine($"=========================== Total answer: {directions["horizontal"] * directions["depth"]}");
    }
    
    private Dictionary<string, int> GetDirectionValues(List<Tuple<string, int>> inputList)
    {
        var dict = new Dictionary<string, int>
        {
            { "horizontal", 0 },
            { "depth", 0 }
        };

        foreach (var direction in inputList)
        {
            switch (direction.Item1)
            {
                case "forward":
                    dict["horizontal"] += direction.Item2;
                    Console.WriteLine($"Horizontal: {dict["horizontal"]} {direction.Item1} {direction.Item2}");
                    break;
                case "up":
                    dict["depth"] -= direction.Item2;
                    Console.WriteLine($"Depth: {dict["depth"]} {direction.Item1} {direction.Item2}");
                    break;
                case "down":
                    dict["depth"] += direction.Item2;
                    Console.WriteLine($"Depth: {dict["depth"]} {direction.Item1} {direction.Item2}");
                    break;
                default:
                    Console.WriteLine("Unexpected commando");
                    break;

            }
        }

        return dict;
    }
    
    private Dictionary<string, int> GetAimedDirectionValues(List<Tuple<string, int>> inputList)
    {
        var dict = new Dictionary<string, int>
        {
            { "horizontal", 0 },
            { "depth", 0 },
            { "aim", 0 }
        };

        foreach (var direction in inputList)
        {
            switch (direction.Item1)
            {
                case "forward":
                    dict["horizontal"] += direction.Item2;
                    dict["depth"] += (dict["aim"] * direction.Item2);
                    break;
                case "up":
                    dict["aim"] -= direction.Item2;
                    break;
                case "down":
                    dict["aim"] += direction.Item2;
                    break;
                default:
                    Console.WriteLine("Unexpected commando");
                    break;

            }

            Console.WriteLine($"H: {dict["horizontal"]} / D:{dict["depth"]} A: {dict["aim"]} - {direction.Item1} {direction.Item2}");
        }

        return dict;
    }
}