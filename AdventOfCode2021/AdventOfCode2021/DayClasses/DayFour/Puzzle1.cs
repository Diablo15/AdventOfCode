using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2021.DayClasses.DayFour
{
    public class Puzzle1
    {
        public List<int> BingoDrawnNumbers { get; set; }
        public IFileReader FileReader { get; set; } = new FileReader();

        public void Start()
        {
            this.BingoDrawnNumbers = new List<int>();

            var readings = FileReader.ReadInputValuesToLines(@".\DayClasses\DayFour\input-raw.csv", 1);

            //var total = this.CheckMeasurements(this.PuzzleOneMeasurements);

            //Console.WriteLine($"=========================== Total increasements: {total}");
        }
    }
}
