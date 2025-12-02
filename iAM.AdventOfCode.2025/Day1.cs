using iAM.AdventOfCode.Helpers;

namespace iAM.AdventOfCode._2025
{
    public class Day1 : AoCDay
    {
        public List<(char direction, int distance)> PuzzleMeasurements { get; set; }
        public int CurrentDistance { get; set; } = 50;
        public int AtZero { get; set; }
        public int PassedZero { get; set; }

        public Day1() : base(1, true, true)
        {
            PuzzleAltFilePath = @"Day1Puzzle1small.txt";
            UseAltFile = true;
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
            Console.WriteLine($"The dial ended at zero {AtZero} times.");
        }

        public override void Puzzle2Content()
        {
            ReadPuzzleInput();

            foreach (var measurement in this.PuzzleMeasurements)
            {
                RotateDial(measurement.direction, measurement.distance);
                CheckPassedZero();
            }

            Console.WriteLine($"The dial passed zero {PassedZero} times.");
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
            var startingDistance = CurrentDistance;

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

            PassedZero = (distance < 100 && startingDistance == 0) ? PassedZero - 1 : PassedZero;
        }

        private void SetCurrentDistance()
        {
            if (CurrentDistance < 0)
            {
                var negativeDistance = 99 + (CurrentDistance + 1);
                CurrentDistance = negativeDistance;
                PassedZero++;
            }

            if (CurrentDistance > 99)
            {
                var positiveDistance = CurrentDistance - (99 + 1);
                CurrentDistance = positiveDistance;
                PassedZero++;
            }
        }

        private void CheckPassedZero()
        {
            if (CurrentDistance == 0)
            {
                AtZero++;
                PassedZero--;
            }
        }
    }
}
