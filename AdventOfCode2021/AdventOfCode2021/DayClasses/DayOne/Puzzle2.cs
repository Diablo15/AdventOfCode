using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode2021.DayClasses.DayOne
{
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
}
