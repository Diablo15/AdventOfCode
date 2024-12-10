using iAM.AdventOfCode._2023.Models;
using iAM.AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAM.AdventOfCode._2023
{
    public class Day8 : AoCDay
    {
        public Dictionary<string, (string left, string right)> PuzzleMeasurements { get; set; }
        public List<string> Directions { get; set; }

        public Day8() : base(8, true, false)
        {
            this.PuzzleAltFilePath = @"Examples\Day8_small.txt";
            PuzzleMeasurements = new();
        }

        public override void Puzzle1Content()
        {
            ReadPuzzleInput();
            var totalSteps = FollowInstructions();

            // Answer: 15871
            Console.WriteLine($"========= Total steps: {totalSteps} =========");
        }

        private void ReadPuzzleInput()
        {
            var lines = FileReader.ReadInputValues<string>(this.Puzzle1FilePath, '\n', true);
            this.Directions = FileReader.ValuesSplitter<string>(lines.First()).ToList();

            foreach (var line in lines.Skip(1))
            {
                var point = FileReader.ValueSplitter<string, string>(line, '=');
                var trimmed = point.Item2.ValueRemover("(").ValueRemover(")");
                var ways = FileReader.ValueSplitter<string, string>(trimmed, ',');

                this.PuzzleMeasurements.Add(point.Item1, (ways.Item1, ways.Item2));
            }
        }

        private double FollowInstructions()
        {
            double steps = 0;
            var i = 0;
            KeyValuePair<string, (string left, string right)> dirs = new();
            
            do
            {
                var direction = this.Directions[i];
                string way = string.Empty;

                if (steps is 0)
                {
                    dirs = this.PuzzleMeasurements.Where(k => k.Key == "AAA").Single();
                }

                if (direction is "L")
                {
                    way = dirs.Value.left;
                }
                else if (direction is "R")
                {
                    way = dirs.Value.right;
                }

                dirs = this.PuzzleMeasurements.Where(k => k.Key == way).Single();
                steps++;
                i++;

                if (i == this.Directions.Count())
                {
                    i = 0;
                }
            } while (dirs.Key != "ZZZ");

            return steps;
        }

        public override void Puzzle2Content()
        {
            throw new NotImplementedException();
        }
    }
}
