using iAM.AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAM.AdventOfCode._2023
{
    public class Day10 : AoCDay
    {
        public List<(int lineNumber, List<char> original, Dictionary<Point, (Point from, Point to)> coordinates)> PuzzleMeasurements { get; set; }
        public Point StartCoord { get; set; }
        public List<Point> FromList { get; set; }

        public Day10() : base(10, true, false)
        {
            PuzzleAltFilePath = @"Examples\Day10_small.txt";
            FromList = new();
        }

        public override void Puzzle1Content()
        {
            ReadPuzzleInput();
            FillCoordinates();
            FindConnectingLoop(startCoord: true);
        }

        public override void Puzzle2Content()
        {
            throw new NotImplementedException();
        }

        private void ReadPuzzleInput()
        {
            var lines = FileReader.ReadInputValues<string>(PuzzleAltFilePath, '\n');
            this.PuzzleMeasurements = new List<(int, List<char>, Dictionary<Point, (Point, Point)>)>();

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
                var dict = new Dictionary<Point, (Point from, Point to)>();
                var tubeNumber = 0;

                foreach (var c in measurements[line].original)
                {
                    var here = new Point(tubeNumber, measurements[line].lineNumber);
                    var fromTo = TranslateCharToCoordinate(c, tubeNumber, measurements[line].lineNumber);

                    this.PuzzleMeasurements[line].coordinates.Add(here, fromTo);
                    tubeNumber++;
                }
            }
        }

        private (Point from, Point to) TranslateCharToCoordinate(char input, int x, int y)
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

        private List<Point> FindConnectingLoop(Point? input = null, Point? source = null, bool startCoord = false)
        {
            var possibleSteps = new List<Point>();
            var findings = new List<Point>();

            if (startCoord)
            {
                FindStartCoord();
                var list = this.PuzzleMeasurements.SelectMany(x => x.coordinates
                                                   .Where(item => item.Value.from.Equals(this.StartCoord) || item.Value.to.Equals(this.StartCoord))
                                                   .Select(coord => coord.Key)); 
                findings.AddRange(list);
            }
            else
            {
                var list = this.PuzzleMeasurements.SelectMany(x => x.coordinates
                                                    .Where(item => item.Value.from.Equals(input) || item.Value.to.Equals(input))
                                                    .Select(coord => coord.Key));
                findings.AddRange(list);
            }

            if(!findings.Any())
            {
                return null;
            }

            foreach (var possibleStep in findings)
            {
                if(this.FromList.Contains(possibleStep))
                {
                    continue;
                }

                this.FromList.Add(possibleStep);

                if (possibleStep.Equals(this.StartCoord))
                {
                    continue;
                }
                var results = FindConnectingLoop(possibleStep);

                if (results is null)
                {
                    continue;
                }

                possibleSteps.Add(possibleStep);
            }

            return possibleSteps;
        }

        private void FindStartCoord()
        {
            var list = this.PuzzleMeasurements.SelectMany(x => x.coordinates.Where(x => x.Value.Equals((new Point(999, 999), new Point(999, 999))))).Single();
            this.StartCoord = list.Key;
        }
    }
}
