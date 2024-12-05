using System.Collections;
using System.Data.Common;
using System.Text.RegularExpressions;
using iAM.AdventOfCode._2023.Models;
using iAM.AdventOfCode.Helpers;

namespace iAM.AdventOfCode._2023;

public class Day4
{
    private IEnumerable<string> PuzzleOneMeasurements { get; set; }

    private IEnumerable<ScratchCard> CalculatedCards { get; set; }

    private string Puzzle1FilePath = "Day4Puzzle1.txt";

    public Day4()
    {
        PuzzleOneMeasurements = new List<string>();
    }

    public void StartDay4()
    {
        Console.WriteLine("******** Day 4 ********");
        Puzzle1();
        Puzzle2();
    }

    private void Puzzle1()
    {
        Console.WriteLine("******** Puzzle 1 ********");
        PuzzleOneMeasurements = FileReader.ReadInputValues<string>(Puzzle1FilePath, '\n');

        var cards = GenerateScratchCards();
        CalculatedCards = GetWinningMatches(cards);
        var result = CalculateCardPoints(CalculatedCards.ToList());
        var sum = result.Sum(x => x);

        Console.WriteLine($"1. ======== Total Sum == {sum}");
        //Temp(this.PuzzleOneMeasurements.ToList());
    }

    private void Temp(List<string> input)
    {
        var cards =
            input
                .Select(x => x.Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(2).ToList())
                .Select(x =>
                {
                    var winners = x.TakeWhile(x => x != "|").ToHashSet();
                    var numbers = x.SkipWhile(x => x != "|").Skip(1).ToHashSet();
                    return (winningNumbers: winners.Intersect(numbers).Count(), count: 1);
                })
                .ToArray();

        var result1 = 0;
        var result2 = 0;
        for (int c = 0; c < cards.Length; c++)
        {
            var (winningNumbers, count) = cards[c];
            for (int i = c + 1, n = winningNumbers; i < cards.Length && n > 0; i++, n--)
                cards[i].count += count;

            result1 += (1 << winningNumbers) >> 1;
            result2 += count;
        }

        Console.WriteLine($"Result1 = {result1}");
        Console.WriteLine($"Result2 = {result2}");
    }

    private void Puzzle2()
    {
        Console.WriteLine("******** Puzzle 2 ********");
        var sum = CalculateWinningScratchCards(CalculatedCards.ToList());
        Console.WriteLine($"2. ======== Total Sum == {sum}");
    }

    private int CalculateWinningScratchCards(List<ScratchCard> input)
    {
        var totalOwning = 0;
        
        for (var c = 0; c < input.Count; c++)
        {
            var (winningNumbers, count) = (input[c].WinningMatches.Count(), input[c].OwningTimes);
            for (int i = c + 1, n = winningNumbers; i < input.Count && n > 0; i++, n--)
            {
                input[i].OwningTimes += count;
            }

            totalOwning += count;
        }

        return totalOwning;
    }

private List<ScratchCard> GenerateScratchCards()
    {
        var scratchCards = new List<ScratchCard>();
        foreach (var line in this.PuzzleOneMeasurements.ToList())
        {
            var card = new ScratchCard()
            {
                OwningTimes = 1
            };
            
            const string delimiter = ":";
            var indexDelimiter = line.IndexOf(delimiter, StringComparison.Ordinal);
            var data = line[(indexDelimiter+delimiter.Length)..].Trim();
            var numberGroup = FileReader.ValueSplitter<string>(data, '|').ToList();
            
            for(var group = 0; group < numberGroup.Count; group++)
            {
                var numbers = FileReader.ValueSplitter<int>(numberGroup[group], ' ').ToList();
                
                if (group == 0)
                {
                    card.WinningNumbers = numbers;
                    continue;
                }

                card.PlayingNumbers = numbers;
            }
            scratchCards.Add(card);
        }

        return scratchCards;
    }

    private IEnumerable<ScratchCard> GetWinningMatches(List<ScratchCard> input)
    {
        var output = input;
        foreach (var card in output)
        {
            card.WinningMatches = card.WinningNumbers.Intersect(card.PlayingNumbers);
        }

        return output;
    }

    private IEnumerable<int> CalculateCardPoints(List<ScratchCard> input)
    {
        var pointList = new List<int>();
        foreach (var card in input)
        {
            var points = card.WinningMatches.Count();

            if (points < 1)
            {
                continue;
            }
            
            var x = 1;
            
            for(var c = 1; c < points; c++) {
                x *= 2; // multiply by 2 in each iteration
            }
                
            pointList.Add(x);
        }

        return pointList;
    }
}