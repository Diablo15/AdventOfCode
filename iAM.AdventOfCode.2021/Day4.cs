using iAM.AdventOfCode.Helpers;

namespace iAM.AdventOfCode._2021;

public class Day4 : AocDay
{
    private IEnumerable<string> PuzzleOneMeasurements { get; set; }
    private IEnumerable<int> DrawnNumbers { get; set; }
    private List<List<Dictionary<int, bool>>> BingoCards { get; set; } = new ();
    
    public Day4() : base(4, true, false)
    {
        this.PuzzleAltFilePath = "..//Examples//Day4Puzzle1Alt.txt";
        this.UseAltFile = true;
    }

    public override void Puzzle1Content()
    {
        this.PuzzleOneMeasurements = FileReader.ReadInputValues<string>(this.Puzzle1FilePath, '\n', true);
        SelectDrawnNumbers(this.PuzzleOneMeasurements);
        CreateBingoCards(this.PuzzleOneMeasurements);
    }

    public override void Puzzle2Content()
    {
        throw new NotImplementedException();
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
            List<Dictionary<int, bool>> bingoLines = new List<Dictionary<int, bool>>();
            foreach (var cardLine in card)
            {
                var numbers = FileReader.ValuesSplitter<int>(cardLine, ' ').ToList();
                var lineNumbers = numbers.ToDictionary(n => n, n => false);
                bingoLines.Add(lineNumbers);
            }
            this.BingoCards.Add(bingoLines);
        }
    }

    private void CheckOnBingo()
    {
        // Check horizontal
        // Check vertical
        // Check diagonal (2x)
    }
}