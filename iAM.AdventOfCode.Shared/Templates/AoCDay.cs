namespace iAM.AdventOfCode
{
    public abstract class AoCDay(int dayNumber, bool runPuzzle1 = true, bool runPuzzle2 = true)
    {
        private int DayNumber { get; init; } = dayNumber;

        protected string Puzzle1FilePath { get; init; } = $"Day{dayNumber}Puzzle1.txt";

        protected string Puzzle2FilePath { get; init; } = $"Day{dayNumber}Puzzle2.txt";

        protected string PuzzleAltFilePath { get; set; }

        public void StartDay()
        {
            Console.WriteLine($"******** Day {DayNumber} ********");
            Puzzle1();
            Puzzle2();
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
