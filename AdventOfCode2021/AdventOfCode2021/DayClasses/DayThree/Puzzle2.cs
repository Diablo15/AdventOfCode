using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2021.DayClasses.DayThree
{
    using Microsoft.AspNetCore.Mvc.Filters;

    public class Puzzle2
    {
        public List<string> OxygenRatings { get; set; }
        public List<string> CarbonDioxideRatings { get; set; }
        public Puzzle1 Puzzle1 { get; set; } = new Puzzle1();
        public IFileReader FileReaderClass { get; set; } = new FileReader();

        public void Start()
        {
            Puzzle1.PuzzleOneDiagnostics = FileReaderClass.ReadInputValues(@".\DayClasses\DayThree\input.csv");
            this.OxygenRatings = Puzzle1.PuzzleOneDiagnostics;
            this.CarbonDioxideRatings = Puzzle1.PuzzleOneDiagnostics;

            var oxygen = this.DetermineRatings(OxygenRatings, false);
            Console.WriteLine(oxygen.First());
            var carbonDiox = this.DetermineRatings(CarbonDioxideRatings, true);
            Console.WriteLine(carbonDiox.First());

            var oxyNumber = Convert.ToInt32(oxygen.First(), 2);
            var carbNumber = Convert.ToInt32(carbonDiox.First(), 2);

            Console.WriteLine($"=============Total = {oxyNumber} * {carbNumber} = {oxyNumber * carbNumber}");
        }

        public List<string> DetermineRatings(List<string> diag, bool discardMostCommon)
        {
            for(var i = 0; i < diag.First().Length; i++)
            {
                var positions = Puzzle1.ReadPositions(diag);

                diag = this.DiscardBits(positions, diag, i, discardMostCommon);

                foreach (var val in diag)
                {
                    Console.WriteLine(val);
                }

                if (i == diag.First().Length && diag.Count > 1)
                {
                    i = 0;
                }

                if (diag.Count == 1)
                {
                    break;
                }
            }

            return diag;
        }

        public List<string> DiscardBits(List<RateBit> input, List<string> ratings, int index, bool discardMostCommon = false)
        {
            var bit = Puzzle1.GetCorrrectBit(input[index], discardMostCommon);
            Console.WriteLine($"Discard bit: {bit}");
            return ratings.Select(r => r).Where(r => r[index].Equals(bit)).ToList();
        }
    }
}
