using iAM.AdventOfCode._2023.Models;
using iAM.AdventOfCode.Helpers;
using iAM.AdventOfCode.Shared.Models;

namespace iAM.AdventOfCode._2023
{
    public class Day10 : AoCDay
    {
        public List<(int lineNumber, List<char> original, Dictionary<Coordinate, Pipe> coordinates)> PuzzleMeasurements { get; set; }
        public Coordinate StartCoord { get; set; }
        public List<Coordinate> ExclusionList { get; set; }

        public Day10() : base(10, true, false)
        {
            PuzzleAltFilePath = @"Examples\Day10_small.txt";
            ExclusionList = [];
        }

        public override void Puzzle1Content()
        {
            ReadPuzzleInput();
            FillCoordinates();
            FindStartCoord();

            var count = PerformLoopSearch();

            // Answer: 6846
            Console.WriteLine($"========= Total steps to be on the half: {count} =========");
        }

        public override void Puzzle2Content()
        {
            throw new NotImplementedException();
        }

        private int PerformLoopSearch()
        {
            var result = FindConnectingCoordinates(StartCoord);
            result = result.Except([this.StartCoord]);

            result.ToList().ForEach(x => Console.WriteLine(x));

            var count = 0;
            while (result.Any())
            { 
                result = LoopAndPrune(result);
                result.ToList().ForEach(x => Console.WriteLine(x));
                count++;
            }

            return count;
            
        }

        private IEnumerable<Coordinate> LoopAndPrune(IEnumerable<Coordinate> input)
        {
            List<Pipe> results = new();
            foreach (var coordinate in input)
            {
                var res = FindConnectingCoordinates(coordinate).First();
                results.Add(res.Value);

                this.ExclusionList.Add(coordinate);
            }

            return results;
        }

        private IEnumerable<KeyValuePair<Coordinate, Pipe>> FindConnectingCoordinates(Coordinate searchForCoord)
        {
            Console.WriteLine($"Searching for {searchForCoord}");
            return this.PuzzleMeasurements.SelectMany(m => m.coordinates
                                                .Where(item => (item.Value.fromX == searchForCoord.X && item.Value.fromY == searchForCoord.Y) ||
                                                                (item.Value.toX == searchForCoord.X && item.Value.toY == searchForCoord.Y))
                                                .Select(coord => coord));

        }

        private void ReadPuzzleInput()
        {
            var lines = FileReader.ReadInputValues<string>(PuzzleAltFilePath, '\n');
            this.PuzzleMeasurements = new List<(int, List<char>, Dictionary<Coordinate, Pipe>)>();

            var lineCounter = 0;
            foreach (var line in lines)
            {
                var initVals = FileReader.ValuesSplitter<char>(line).ToList();
                this.PuzzleMeasurements.Add((lineCounter, initVals, new()));
                lineCounter++;
            }
        }

        private void FillCoordinates()
        {
            var measurements = this.PuzzleMeasurements;
            for (var line = 0; line < measurements.Count; line++)
            {
                var dict = new Dictionary<Coordinate, (Coordinate from, Coordinate to)>();
                var tubeNumber = 0;

                foreach (var c in measurements[line].original)
                {
                    var here = new Coordinate(tubeNumber, measurements[line].lineNumber);
                    var fromTo = c.TranslateCharToCoordinate(tubeNumber, measurements[line].lineNumber);
                    fromTo = fromTo.TranslateNegativesToPeriods();
                    this.PuzzleMeasurements[line].coordinates.Add(here, fromTo);
                    tubeNumber++;
                }
            }
        }

        private void FindStartCoord()
        {
            var list = this.PuzzleMeasurements.SelectMany(x => x.coordinates.Where(x => x.Value.fromX == x.Value.toY)).Single();
            this.StartCoord = list.Key;
        }
    }
}
