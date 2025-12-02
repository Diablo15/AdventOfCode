using iAM.AdventOfCode.Helpers;

namespace iAM.AdventOfCode._2025
{
    public class Day1 : AoCDay
    {
        public List<(char direction, int distance)> PuzzleMeasurements { get; set; }
        public int CurrentDistance { get; set; } = 50;
        public int PassedZero { get; set; }

        public Day1() : base(1, true, false)
        {
            PuzzleAltFilePath = @"Day1Puzzle1small.txt";
            UseAltFile = false;
        }

        public override void Puzzle1Content()
        {
            ReadPuzzleInput();

            foreach (var measurement in this.PuzzleMeasurements)
            {
                RotateDial(measurement.direction, measurement.distance);
                CheckPassedZero();
            }

            //Answer: 1141
            Console.WriteLine($"The dial passed zero {PassedZero} times.");
        }

        public override void Puzzle2Content()
        {
            throw new NotImplementedException();
        }

        private void ReadPuzzleInput()
        {
            var lines = FileReader.ReadInputValues<string>(Puzzle1FilePath, '\n');
            this.PuzzleMeasurements = new();

            foreach (var line in lines)
            {
                var direction = line[0];
                var distance = int.Parse(line[1..]);
                this.PuzzleMeasurements.Add((direction, distance));
            }
        }

        private void RotateDial(char direction, int distance)
        {
            switch (direction)
            {
                case 'R':
                    CurrentDistance += distance;
                    break;
                case 'L':
                    CurrentDistance -= distance;
                    break;
            }

            do {
                SetCurrentDistance();
            } while (CurrentDistance < 0 || CurrentDistance > 99);

        }

        private void SetCurrentDistance()
        {
            if (CurrentDistance < 0)
            {
                var negativeDistance = 99 + (CurrentDistance + 1);
                CurrentDistance = negativeDistance;
            }

            if (CurrentDistance > 99)
            {
                var positiveDistance = CurrentDistance - (99 + 1);
                CurrentDistance = positiveDistance;
            }
        }

        private void CheckPassedZero()
        {
            if (CurrentDistance == 0)
            {
                PassedZero++;
            }
        }
    }
}
