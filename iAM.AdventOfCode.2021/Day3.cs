using System.Text;
using iAM.AdventOfCode._2021.Models;
using iAM.AdventOfCode.Helpers;

namespace iAM.AdventOfCode._2021;

public class Day3
{
    private string Puzzle1FilePath { get; set; } = "Day3Puzzle1.csv";
    
    private IEnumerable<string> PuzzleOneDiagnostics { get; set; }
    
    public IEnumerable<string> OxygenRatings { get; set; }
    
    public IEnumerable<string> CarbonDioxideRatings { get; set; }
    
    public Day3()
    {
    }
    
    public void StartDay3()
    {
        Console.WriteLine("******** Day 3 ********");
        this.Puzzle1();
        this.Puzzle2();
    }
    
    private void Puzzle1()
    {
        Console.WriteLine("******** Puzzle 1 ********");
        
        this.PuzzleOneDiagnostics = FileReader.ReadInputValues<string>(this.Puzzle1FilePath, ';');
        var gammaRates = this.ReadPositions(this.PuzzleOneDiagnostics.ToList());
        var list = (this.ConstructBinaryFromBits(gammaRates.ToList())).ToList();
        
        Console.WriteLine($"1.========================== Total = {list[0] * list[1]}");
    }
    
    private void Puzzle2()
    {
        Console.WriteLine("******** Puzzle 2 ********");
        this.OxygenRatings = this.PuzzleOneDiagnostics;
        this.CarbonDioxideRatings = this.PuzzleOneDiagnostics;
        
        var oxygen = this.DetermineRatings(OxygenRatings.ToList(), false);
        Console.WriteLine(oxygen.First());
        var carbonDiox = this.DetermineRatings(CarbonDioxideRatings.ToList(), true);
        Console.WriteLine(carbonDiox.First());
        
        var oxyNumber = Convert.ToInt32(oxygen.First(), 2);
        var carbNumber = Convert.ToInt32(carbonDiox.First(), 2);

        Console.WriteLine($"=============Total = {oxyNumber} * {carbNumber} = {oxyNumber * carbNumber}");
    }
    
    private IEnumerable<RateBit> ReadPositions(List<string> input)
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
    
    private IEnumerable<int> ConstructBinaryFromBits(List<RateBit> input)
    {
        var binaries = new List<int>();
        var gammaRates = new StringBuilder();
        var epsilonRate = new StringBuilder();

        foreach (var gammaBit in input)
        {
            gammaRates.Append(GetCorrrectBit(gammaBit, false));
            epsilonRate.Append(GetCorrrectBit(gammaBit, true));
        }

        Console.WriteLine($"GammaRates: {gammaRates} - {Convert.ToInt32(gammaRates.ToString(), 2)}");
        Console.WriteLine($"EpsilonRates: {epsilonRate} - {Convert.ToInt32(epsilonRate.ToString(), 2)}");

        binaries.Add(Convert.ToInt32(gammaRates.ToString(), 2));
        binaries.Add(Convert.ToInt32(epsilonRate.ToString(), 2));

        return binaries;
    }
    
    private char GetCorrrectBit(RateBit input, bool leastCommonValue)
    {
        if (leastCommonValue)
        {
            return input.Ones < input.Zeros ? '1' : '0';
        }

        return input.Ones >= input.Zeros ? '1' : '0';
    }
    
    private IEnumerable<string> DetermineRatings(List<string> diag, bool discardMostCommon)
    {
        for(var i = 0; i < diag.First().Length; i++)
        {
            var positions = this.ReadPositions(diag);

            diag = (this.DiscardBits(positions.ToList(), diag, i, discardMostCommon)).ToList();

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

    private IEnumerable<string> DiscardBits(List<RateBit> input, List<string> ratings, int index, bool discardMostCommon = false)
    {
        var bit = this.GetCorrrectBit(input[index], discardMostCommon);
        Console.WriteLine($"Discard bit: {bit}");
        return ratings.Select(r => r).Where(r => r[index].Equals(bit)).ToList();
    }
}