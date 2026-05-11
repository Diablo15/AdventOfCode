using iAM.AdventOfCode._2021.Models;
using iAM.AdventOfCode.Helpers;

namespace iAM.AdventOfCode._2021;

public class Day4 : AocDay
{
    private IEnumerable<string> PuzzleOneMeasurements { get; set; }
    private IEnumerable<int> DrawnNumbers { get; set; }
    private List<BingoCard> BingoCards { get; set; } = new();

    public Day4() : base(4, false, true)
    {
        this.PuzzleAltFilePath = "..//Examples//Day4Puzzle1Alt.txt";
        this.UseAltFile = false;
    }

    public override void Puzzle1Content()
    {
        this.PuzzleOneMeasurements = FileReader.ReadInputValues<string>(this.Puzzle1FilePath, "\n", true);
        SelectDrawnNumbers(this.PuzzleOneMeasurements);
        CreateBingoCards(this.PuzzleOneMeasurements);

        var result = StartGame();

        // Winning score: 67716
        Console.WriteLine($"Result -- Winning score: {result}");
    }

    public override void Puzzle2Content()
    {
        this.PuzzleOneMeasurements = FileReader.ReadInputValues<string>(this.Puzzle1FilePath, "\n", true);
        SelectDrawnNumbers(this.PuzzleOneMeasurements);
        CreateBingoCards(this.PuzzleOneMeasurements);

        var result = StartGameLoser();

        // Losing score: 1830
        Console.WriteLine($"Result -- Losing score: {result}");
    }

    private void SelectDrawnNumbers(IEnumerable<string> input)
    {
        this.DrawnNumbers = FileReader.ValuesSplitter<int>(input.ToList().First(), ',');
    }

    private void CreateBingoCards(IEnumerable<string> input)
    {
        var rawCards = input.Skip(1).Chunk(5);
        foreach (var card in rawCards)
        {
            var bingoCard = new BingoCard { Numbers = new Dictionary<int, bool>() };
            foreach (var cardLine in card)
            {
                var numbers = FileReader.ValuesSplitter<int>(cardLine, ' ').ToList();
                foreach (var nr in numbers)
                {
                    bingoCard.Numbers.Add(nr, false);
                }
            }

            this.BingoCards.Add(bingoCard);
        }
    }

    private int StartGame()
    {
        var bingoCards = this.BingoCards;
        foreach (var drawnNumber in this.DrawnNumbers)
        {
            CrossOutNumber(drawnNumber);
            var bingo = CheckOnBingo(bingoCards);

            if (bingo is not null)
            {
                var uncalledNumbers = bingo.Numbers.Where(v => !v.Value).Select(k => k.Key);
                return uncalledNumbers.Sum() * drawnNumber;
            }
        }

        throw new Exception("xxxxxxxxx No bingo xxxxxxxxx");
    }

    private int StartGameLoser()
    {
        var bingoCards = this.BingoCards;
        foreach (var drawnNumber in this.DrawnNumbers)
        {
            CrossOutNumber(drawnNumber);

            var bingo = CheckOnBingo(bingoCards);

            if (bingo is not null)
            {
                if (this.BingoCards.Count > 1 && !drawnNumber.Equals(this.DrawnNumbers.Last()))
                {
                    RemoveBingoHorizontally();
                    RemoveBingoVertically();
                    continue;
                }

                var uncalledNumbers = bingo.Numbers.Where(v => !v.Value).Select(k => k.Key);
                return uncalledNumbers.Sum() * drawnNumber;
            }
        }

        throw new Exception("xxxxxxxxx No bingo xxxxxxxxx");
    }

    private BingoCard? CheckOnBingo(IEnumerable<BingoCard> bingoCards)
    {
        foreach (var card in bingoCards)
        {
            var lines = card.Numbers.Values.Chunk(5);

            if (CheckBingoHorizontally(lines) || CheckBingoVertically(lines))
            {
                return card;
            }
        }

        return null;
    }

    private bool CheckBingoHorizontally(IEnumerable<bool[]> cardInput)
    {
        // Check horizontal
        var isBingo = cardInput.Any(line => line.All(nrs => nrs));

        return isBingo ? true : false;
    }

    private void RemoveBingoHorizontally()
    {
        // Remove horizontal
        var removeList = this.BingoCards.Where(card =>
            card.Numbers.Values.Chunk(5).Any(line => line.All(nrs => nrs))
            ).ToList();
        
        removeList.ForEach(card => this.BingoCards.Remove(card));
    }

    private void RemoveBingoVertically()
    {
        // Remove vertically
        var removeList = this.BingoCards.Where(card =>
            Enumerable.Range(0, 5)
                .Any(col => card.Numbers.Values.Chunk(5).All(row => row[col]))
        ).ToList();

        removeList.ForEach(card => this.BingoCards.Remove(card));
    }

    private bool CheckBingoVertically(IEnumerable<bool[]> cardInput)
    {
        // Check vertical
        var isBingo = Enumerable.Range(0, 5)
            .Any(col => cardInput.All(row => row[col]));

        return isBingo ? true : false;
    }

    private bool CheckBingoDiagonally(IEnumerable<bool[]> cardInput)
    {
        // Check diagonal (2x)
        var isBingo = Enumerable.Range(0, 5)
            .All(i => cardInput.ToList()[i][i]);

        if (isBingo)
        {
            return true;
        }

        isBingo = Enumerable.Range(0, 5)
            .All(i => cardInput.ToList()[i][4 - i]);

        return isBingo ? true : false;
    }

    private void CrossOutNumber(int number)
    {
        foreach (var card in this.BingoCards)
        {
            if (card.Numbers.TryGetValue(number, out _))
            {
                card.Numbers[number] = true;
            }
        }
    }

    private void Logging()
    {
        foreach (var card in this.BingoCards)
        {
            Console.WriteLine(" !!!!! Crossed Card:");
            foreach (var row in card.Numbers.Values.Chunk(5))
            {
                Console.WriteLine(string.Join(",", row));
            }

            Console.WriteLine("\n");
        }
    }
}