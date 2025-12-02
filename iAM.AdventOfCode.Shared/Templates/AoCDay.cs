namespace iAM.AdventOfCode
{
    public abstract class AoCDay(int dayNumber, bool runPuzzle1 = true, bool runPuzzle2 = true)
    {
        private int DayNumber { get; init; } = dayNumber;

        protected bool UseAltFile { get; init; } = false;

        protected string Puzzle1FilePath { get; set; } = $"Day{dayNumber}Puzzle1.txt";

        protected string Puzzle2FilePath { get; set; } = $"Day{dayNumber}Puzzle2.txt";

        protected string PuzzleAltFilePath { get; set; } = string.Empty;

        public void StartDay()
        {
            Puzzle1FilePath = UseAltFile ? PuzzleAltFilePath : Puzzle1FilePath;
            Puzzle2FilePath = UseAltFile ? PuzzleAltFilePath : Puzzle2FilePath;

            Console.WriteLine($"******** Day {DayNumber} ********");
            if (runPuzzle1)
            {
                Puzzle1();
            }

            if (runPuzzle2)
            {
                Puzzle2();
            }
        }

        private void Puzzle1()
        {
            Console.WriteLine("******** Puzzle 1 ********");
            Puzzle1Content();
        }

        public abstract void Puzzle1Content();
       

        private void Puzzle2()
        {
            Console.WriteLine("******** Puzzle 2 ********");
            Puzzle2Content();
        }

        public abstract void Puzzle2Content();
    }
}
