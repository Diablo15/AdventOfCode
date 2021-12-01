using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2021.DayClasses.DayOne
{
    using System.IO;

    using AdventOfCode2021.Models;

    public class Puzzle1
    {
        public List<int> PuzzleOneMeasurements { get; set; }

        public void Start()
        {
            this.ReadInputValues();

            var total = this.CheckMeasurements();

            Console.WriteLine($"=========================== Total increasements: {total}");
        }

        public List<int> ReadInputValues()
        {
            using (var reader = new StreamReader(@".\DayClasses\DayOne\inputpuzzle1.csv"))
            {
                this.PuzzleOneMeasurements = new List<int>();

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(';');

                    this.PuzzleOneMeasurements.Add(Convert.ToInt32(values[0]));
                }

                return this.PuzzleOneMeasurements;
            }
        }

        public int CheckMeasurements()
        {
            var oldMeasurement = PuzzleOneMeasurements[0];
            var increasingCount = 0;
                
            foreach (var measurement in PuzzleOneMeasurements)
            {
                if (measurement < oldMeasurement)
                {
                    Console.WriteLine($"{measurement} (decreased)");
                }
                else if (measurement > oldMeasurement)
                {
                    increasingCount++;
                    Console.WriteLine($"{measurement} (increased)");
                }
                else
                {
                    Console.WriteLine($"{measurement} (N/A - no previous measurement)");
                }

                oldMeasurement = measurement;
            }

            return increasingCount;
        }
    }
}
