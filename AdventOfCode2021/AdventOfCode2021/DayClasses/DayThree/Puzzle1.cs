namespace AdventOfCode2021.DayClasses.DayThree
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class Puzzle1
    {
        public List<string> PuzzleOneDiagnostics { get; set; } = new List<string>();
        public IFileReader FileReaderClass { get; set; } = new FileReader();

        public void Start()
        {
            PuzzleOneDiagnostics = FileReaderClass.ReadInputValues(@".\DayClasses\DayThree\input.csv");
            var gammaRates = this.ReadPositions(PuzzleOneDiagnostics);
            var list = this.ConstructBinaryFromBits(gammaRates);

            Console.WriteLine($"=============Total = {list[0] * list[1]}");
        }

        public List<RateBit> ReadPositions(List<string> input)
        {
            var rateBits = new List<RateBit>();
            var initializeCount = 0;

            do
            {
                rateBits.Add(new RateBit());
                initializeCount++;

            } while (initializeCount < input.First().Length);

            foreach (var value in input)
            {
                var indexPosition = 0;
                while (indexPosition < value.Length)
                {
                    if (value[indexPosition] == '1')
                    {
                        rateBits[indexPosition].Ones++;
                    }
                    else
                    {
                        rateBits[indexPosition].Zeros++;
                    }

                    indexPosition++;
                }
            }

            return rateBits;
        }

        public List<int> ConstructBinaryFromBits(List<RateBit> input, bool leastCommonValue = false)
        {
            var binaries = new List<int>();
            var gammaRates = new StringBuilder();
            var epsilonRate = new StringBuilder();

            foreach (var gammaBit in input)
            {
                gammaRates.Append(GetCorrrectBit(gammaBit, leastCommonValue));
                epsilonRate.Append(GetCorrrectBit(gammaBit, leastCommonValue));
            }

            Console.WriteLine($"GammaRates: {gammaRates} - {Convert.ToInt32(gammaRates.ToString(), 2)}");
            Console.WriteLine($"EpsilonRates: {epsilonRate} - {Convert.ToInt32(epsilonRate.ToString(), 2)}");

            binaries.Add(Convert.ToInt32(gammaRates.ToString(), 2));
            binaries.Add(Convert.ToInt32(epsilonRate.ToString(), 2));

            return binaries;
        }

        public char GetCorrrectBit(RateBit input, bool leastCommonValue)
        {
            if (leastCommonValue)
            {
                return input.Ones < input.Zeros ? '1' : '0';
            }

            return input.Ones >= input.Zeros ? '1' : '0';
        }
    }
}
