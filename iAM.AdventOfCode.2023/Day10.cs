using iAM.AdventOfCode.Helpers;

namespace iAM.AdventOfCode._2023
{
    public class Day10 : AoCDay
    {
        public List<(int lineNumber, List<char> original, Dictionary<Coordinate, (Coordinate from, Coordinate to)> coordinates)> PuzzleMeasurements { get; set; }
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

            Console.WriteLine($"========= Total steps to be on the half: {count} =========");
        }

        public override void Puzzle2Content()
        {
            throw new NotImplementedException();
        }

        private int PerformLoopSearch()
        {
            var result = FindConnectingCoordinates(StartCoord);
            var count = 0;
            do
            {
                result = LoopAndPrune(result);
                count++;
            }
            while (result.Any());

            return count;
            
        }

        private IEnumerable<Coordinate> LoopAndPrune(IEnumerable<Coordinate> input)
        {
            List<Coordinate> prunedResult = new() ;

            foreach (var coordinate in input)
            {
                this.ExclusionList.Add(coordinate);

                var res = FindConnectingCoordinates(coordinate);
                prunedResult.AddRange(res.Except(this.ExclusionList));
            }

            return prunedResult;
        }

        private IEnumerable<Coordinate> FindConnectingCoordinates(Coordinate searchForCoord)
        {
            return this.PuzzleMeasurements.SelectMany(m => m.coordinates
                                                .Where(item => item.Value.from == searchForCoord || item.Value.to == searchForCoord)
                                                .Select(coord => coord.Key));

        }

        private void ReadPuzzleInput()
        {
            var lines = FileReader.ReadInputValues<string>(Puzzle1FilePath, '\n');
            this.PuzzleMeasurements = new List<(int, List<char>, Dictionary<Coordinate, (Coordinate, Coordinate)>)>();

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
                    var fromTo = TranslateCharToCoordinate(c, tubeNumber, measurements[line].lineNumber);
                    fromTo = TranslateNegativesToPeriods(fromTo);
                    this.PuzzleMeasurements[line].coordinates.Add(here, fromTo);
                    tubeNumber++;
                }
            }
        }
        private (Coordinate from, Coordinate to) TranslateCharToCoordinate(char input, int x, int y)
        {
            switch (input)
            {
                case '|':
                    return (new(x, y - 1), new(x, y + 1));
                case '-':
                    return (new(x - 1, y), new(x + 1, y));
                case 'L':
                    return (new(x, y - 1), new(x + 1, y));
                case 'J':
                    return (new(x - 1, y), new(x, y - 1));
                case '7':
                    return (new(x - 1, y), new(x, y + 1));
                case 'F':
                    return (new(x, y + 1), new(x + 1, y));
                case '.':
                    return (new(888, 888), new(888, 888));
                case 'S':
                    return (new(999, 999), new(999, 999));
                default:
                    throw new NotImplementedException();
            }
        }

        private (Coordinate from, Coordinate to) TranslateNegativesToPeriods((Coordinate from, Coordinate to) input)
        {
            var output = input;
            if(output.from.X < 0  || output.from.Y < 0)
            {
                output.from = new Coordinate(888, 888);
            }

            if (output.to.X < 0 || output.to.Y < 0)
            {
                output.to = new Coordinate(888, 888);
            }
            return output;
        }

        private void FindStartCoord()
        {
            var list = this.PuzzleMeasurements.SelectMany(x => x.coordinates.Where(x => x.Value.Equals((new Coordinate(999, 999), new Coordinate(999, 999))))).Single();
            this.StartCoord = list.Key;
        }
    }

    public record Coordinate(int X, int Y);
}
