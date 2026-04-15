namespace iAM.AdventOfCode
{
    public abstract class AocDay
    {
        protected AocDay(int dayNumber, bool runPuzzle1 = true, bool runPuzzle2 = true)
        {
            _dayNumber = dayNumber;
            _runPuzzle1 = runPuzzle1;
            _runPuzzle2 = runPuzzle2;
            
            Puzzle1FilePath = UseAltFile ? PuzzleAltFilePath : $"Day{_dayNumber}Puzzle1.txt";
            Puzzle2FilePath = UseAltFile ? PuzzleAltFilePath : $"Day{_dayNumber}Puzzle2.txt";
        }

        private static int _dayNumber { get; set; }

        private static bool _runPuzzle1 { get; set; }
        
        private static bool _runPuzzle2 { get; set; }

        protected bool UseAltFile { get; init; } = false;

        protected string PuzzleAltFilePath { get; set; } = string.Empty;
        
        protected string Puzzle1FilePath { get; init; }

        protected string Puzzle2FilePath { get; init; }

        public void StartDay()
        {
            Console.WriteLine($"******** Day {_dayNumber} ********");
            if (_runPuzzle1)
            {
                Puzzle1();
            }

            if (_runPuzzle2)
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