using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2021.DayClasses.DayOne
{
    using System.IO;

    public class Puzzle2
    {
        public Puzzle1 Puzzle1 { get; set; } = new Puzzle1();
        public List<int> PuzzleTwoTotals { get; set; }

        public void Start()
        {
            Puzzle1.PuzzleOneMeasurements = new List<int>();
            this.Puzzle1.ReadInputValues(Puzzle1.PuzzleOneMeasurements);

            PuzzleTwoTotals = EnrichMeasurements(Puzzle1.PuzzleOneMeasurements);

            var total = this.Puzzle1.CheckMeasurements(this.PuzzleTwoTotals);

            Console.WriteLine($"=========================== Total increasements: {total}");
        }

        public List<int> EnrichMeasurements(List<int> input)
        {
            var list = new List<int>();

            foreach (var measurement in input)
            {
                var indexOfMeasurement = input.IndexOf(measurement);
                var indexOfMeasurementTwo = input.IndexOf(measurement)+1;
                var indexOfMeasurementThree = indexOfMeasurementTwo+1;
                var indexLast = input.IndexOf(input.Last());

                if (indexOfMeasurement >= indexLast - 1)
                {
                    continue;
                }
                
                var addition = measurement + input[indexOfMeasurementTwo] + input[indexOfMeasurementThree];

                list.Add(addition);
            }

            return list;
        }
    }
}
