using iAM.AdventOfCode._2021.Models;
using iAM.AdventOfCode.Helpers;

namespace iAM.AdventOfCode._2021;

public class Day5 : AocDay
{
    private IEnumerable<Tuple<Coordinate, Coordinate>> PuzzleOneMeasurements { get; set; }
    private IEnumerable<Coordinate> PipeCoordinates { get; set; } = new List<Coordinate>();

    public Day5() : base(5, false, true)
    {
        this.PuzzleAltFilePath = "..//Examples//Day5Puzzle1Alt.txt";
        this.UseAltFile = false;
    }

    public override void Puzzle1Content()
    {
        PrepareData();
        CreateCoordinateRange(false);

        var counter = this.PipeCoordinates.Where(c => c.Hit > 1).Select(c => c).ToList();
        
        // Answer: 7269
        Console.WriteLine($"Result -- Total overlapping: {counter.Count()} ");
    }

    public override void Puzzle2Content()
    {
        PrepareData();
        CreateCoordinateRange(true);

        var counter = this.PipeCoordinates.Where(c => c.Hit > 1).Select(c => c).ToList();
        
        // Answer: 21140
        Console.WriteLine($"Result -- Total overlapping: {counter.Count()} ");    }

    private void PrepareData()
    {
        var lines = FileReader.ReadInputValues<string>(this.Puzzle1FilePath, "\n", true);
        var measurements = new List<Tuple<Coordinate, Coordinate>>();
        foreach (var line in lines)
        {
            var fromTo = FileReader.ValuesSplitter<string>(line, "->");

            Coordinate from = FileReader.ValueSplitter<double, double>(fromTo.First(), ',');
            Coordinate to = FileReader.ValueSplitter<double, double>(fromTo.Last(), ',');

            var coord = new Tuple<Coordinate, Coordinate>(from, to);
            // Console.WriteLine(coord);
            measurements.Add(coord);
        }

        this.PuzzleOneMeasurements = measurements;
    }

    private void CreateCoordinateRange(bool diag)
    {
        var coordList = new List<Coordinate>();
        
        foreach (var coordinates in this.PuzzleOneMeasurements)
        {
            var range = GetCoordinatesBetween(coordinates.Item1, coordinates.Item2, diagonally: diag);
            
            foreach (var pipeCoord in range)
            {
                if (this.PipeCoordinates.Contains(pipeCoord))
                {
                    var counter = this.PipeCoordinates.Single(c =>
                        (c.Latitude == pipeCoord.Latitude && c.Longitude == pipeCoord.Longitude));
                    counter.Hit += 1;
                    continue;
                }
                else
                {
                    coordList.Add(pipeCoord);
                }
            }
            this.PipeCoordinates = coordList;
        }
    }

    public IEnumerable<Coordinate> GetCoordinatesBetween(Coordinate start, Coordinate end, double step = 1.0, bool diagonally = false)
    {
        double latDiff = end.Latitude - start.Latitude;
        double lonDiff = end.Longitude - start.Longitude;
        
        if (!diagonally && latDiff != 0 && lonDiff != 0)
        {
            yield break;
        }
        
        double totalSteps = Math.Max(Math.Abs(latDiff), Math.Abs(lonDiff)) / step;

        double latStep = (latDiff / totalSteps) * step;
        double lonStep = (lonDiff / totalSteps) * step;
        
        for (int i = 0; i <= (int)totalSteps; i++)
        {
            yield return new Coordinate(
                Math.Round(start.Latitude + i * latStep, 10),
                Math.Round(start.Longitude + i * lonStep, 10),
                1
            );
        }
    }

    private void Logging()
    {
    }
}