using iAM.AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAM.AdventOfCode._2023
{
    public class Day6 : AoCDay
    {
        public List<(int gameNr, long gameTimeMs, long recordDistMs)> PuzzleMeasurements { get; set; }

        public Day6() : base(6, false, true)
        {
            this.PuzzleAltFilePath = @"Examples\Day6_small.txt";
            this.PuzzleMeasurements = new();
        }

        public override void Puzzle1Content()
        {
            ReadRaceValues();
            var result = CalculateGamePossibilities();

            // Answer: 2756160
            Console.WriteLine($"========= Total posibilities : {result} =========");
        }

        public override void Puzzle2Content()
        {
            ReadRaceValue();
            var result = CalculateGamePossibilities();

            // Answer: 34788142
            Console.WriteLine($"========= Total posibilities : {result} =========");
        }

        private void ReadRaceValues()
        {
            var lines = FileReader.ReadInputValues<string>(this.Puzzle1FilePath, '\n', true).ToArray();
            var timeLine = FileReader.ValueStartRemover(lines[0], "Time:");
            var distanceLine = FileReader.ValueStartRemover(lines[1], "Distance:");

            var timeValues = FileReader.ValueSplitter<int>(timeLine, ' ').ToArray();
            var distanceValues = FileReader.ValueSplitter<int>(distanceLine, ' ').ToArray();

            for (int i = 0; i < timeValues.Length; i++)
            {
                PuzzleMeasurements.Add((i + 1, timeValues[i], distanceValues[i]));
            }
        }

        private void ReadRaceValue()
        {
            var lines = FileReader.ReadInputValues<string>(this.Puzzle1FilePath, '\n', true).ToArray();
            var timeLine = FileReader.ValueStartRemover(lines[0], "Time:");
            var distanceLine = FileReader.ValueStartRemover(lines[1], "Distance:");

            var timeVal = timeLine.WhiteSpaceRemover();
            var distanceVal = distanceLine.WhiteSpaceRemover();

            var timeValues = FileReader.ValueSplitter<long>(timeVal, ' ').Single();
            var distanceValues = FileReader.ValueSplitter<long>(distanceVal, ' ').Single();

            PuzzleMeasurements.Add((1, timeValues, distanceValues));
        }

        private long CalculateGamePossibilities()
        {
            var numberOfPossibilties = new List<int>();
            foreach (var race in this.PuzzleMeasurements)
            {
                var possibilites = new List<long>();

                for (long holdPress = 0; holdPress <= race.gameTimeMs; holdPress++)
                {
                    var milliMetersPerSec = holdPress;
                    var timeLeft = (race.gameTimeMs - holdPress);
                    var distance = (timeLeft * milliMetersPerSec);

                    possibilites.Add(distance);
                }

                possibilites.RemoveAll(r => r <= race.recordDistMs);

                numberOfPossibilties.Add(possibilites.Count);
            }

            long? prdct = null;
            foreach (var pos in numberOfPossibilties)
            {
                prdct = prdct.HasValue ? prdct.Value * pos : pos;
            }

            return prdct!.Value;
        }
    }
}
