using iAM.AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAM.AdventOfCode._2023
{
    public class Day9 : AoCDay
    {
        public List<(List<long> initialVals, List<List<long>> valueDiffs, List<long> newVals)> PuzzleMeasurements { get; set; }
        public Day9() : base(9, false, true)
        {
            PuzzleAltFilePath = @"Examples\Day9_small.txt";
        }

        public override void Puzzle1Content()
        {
            ReadPuzzleInput();
            CalculateDiffsUntilZero();
            CalculateNewValuesInLine();

            var totalSum = this.PuzzleMeasurements.Select(x => x.newVals).Sum(v => v.Last());

            // Answer: 1666172641
            Console.WriteLine($"========= Total sum of extrapolated values: {totalSum} =========");
        }

        public override void Puzzle2Content()
        {
            ReadPuzzleInput();
            CalculateDiffsUntilZero();
            CalculateNewValuesInLineBackwards();

            var totalSum = this.PuzzleMeasurements.Select(x => x.newVals).Sum(v => v.Last());

            // Answer: 933
            Console.WriteLine($"========= Total sum of extrapolated values: {totalSum} =========");
        }

        private void ReadPuzzleInput()
        {
            var lines = FileReader.ReadInputValues<string>(Puzzle1FilePath, '\n');
            this.PuzzleMeasurements = new List<(List<long>, List<List<long>>, List<long>)>();

            foreach (var line in lines)
            {
                var initVals = FileReader.ValuesSplitter<long>(line, ' ').ToList();
                this.PuzzleMeasurements.Add((initVals, new(), new()));
            }
        }

        private void CalculateDiffsUntilZero()
        {
            for (var measurements = 0; measurements < this.PuzzleMeasurements.Count; measurements++)
            {
                var valueDifferences = new List<List<long>>();
                CalculateDiffs(this.PuzzleMeasurements[measurements].initialVals, valueDifferences);

                this.PuzzleMeasurements[measurements].valueDiffs.AddRange(valueDifferences);
            }
        }

        private void CalculateDiffs(List<long> input, List<List<long>> output)
        {
            var valueDifference = new List<long>();
            for (var val = 0; val < input.Count - 1; val++)
            {
                var differens = input[val + 1] - input[val];
                valueDifference.Add(differens);
            }

            output.Add(valueDifference);

            if (!valueDifference.TrueForAll(x => x == 0))
            {
                CalculateDiffs(valueDifference, output);
            };
        }

        private void CalculateNewValuesInLine()
        {
            for (var measurements = 0; measurements < this.PuzzleMeasurements.Count; measurements++)
            {
                var valueDiffs = Enumerable.Reverse(this.PuzzleMeasurements[measurements].valueDiffs).ToList();
                var lastInitValue = this.PuzzleMeasurements[measurements].initialVals.Last();

                var newValues = new List<long>();

                for (var diffLine = 0; diffLine < valueDiffs.Count() - 1; diffLine++)
                {
                    var lastDiff = newValues.Any() ? newValues.Last() : valueDiffs[diffLine].Last();
                    var lastDiffLineToAdd = valueDiffs[diffLine + 1].Last();

                    var newVal = lastDiffLineToAdd + lastDiff;
                    newValues.Add(newVal);
                }

                var newInitVal = lastInitValue + newValues.Last();
                newValues.Add(newInitVal);

                this.PuzzleMeasurements[measurements].newVals.AddRange(newValues);
            }
        }
        
        private void CalculateNewValuesInLineBackwards()
        {
            for (var measurements = 0; measurements < this.PuzzleMeasurements.Count; measurements++)
            {
                var valueDiffs = Enumerable.Reverse(this.PuzzleMeasurements[measurements].valueDiffs).ToList();
                var lastInitValue = this.PuzzleMeasurements[measurements].initialVals.First();

                var newValues = new List<long>();

                for (var diffLine = 0; diffLine < valueDiffs.Count() - 1; diffLine++)
                {
                    var lastDiff = newValues.Any() ? newValues.Last() : valueDiffs[diffLine].First();
                    var lastDiffLineToAdd = valueDiffs[diffLine + 1].First();

                    var newVal = lastDiffLineToAdd - lastDiff;
                    newValues.Add(newVal);
                }

                var newInitVal = lastInitValue - newValues.Last();
                newValues.Add(newInitVal);

                this.PuzzleMeasurements[measurements].newVals.AddRange(newValues);
            }
        }
    }
}
