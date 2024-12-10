using iAM.AdventOfCode.Helpers;
using System.Collections.Immutable;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace iAM.AdventOfCode._2023
{
    public class Day8 : AoCDay
    {
        public Dictionary<string, (string left, string right)> PuzzleMeasurements { get; set; }
        public List<string> Directions { get; set; }
        public string Directions2 { get; set; }

        public Day8() : base(8, false, true)
        {
            this.PuzzleAltFilePath = @"Examples\Day8_small.txt";
            PuzzleMeasurements = new();
            ReadPuzzleInput();
        }

        public override void Puzzle1Content()
        {
            var totalSteps = FollowInstructionsToZZZ();

            // Answer: 15871
            Console.WriteLine($"========= Total steps: {totalSteps} =========");
        }

        public override void Puzzle2Content()
        {
            var lines = FileReader.ReadInputValues<string>(this.Puzzle1FilePath, '\n', true);

            var totalSteps = PartTwo(lines.ToList());
            //var totalSteps = FollowInstructionsToEndingZ();

            // Answer: 11283670395017
            Console.WriteLine($"========= Total steps: {totalSteps} =========");
        }

        private void ReadPuzzleInput()
        {
            var lines = FileReader.ReadInputValues<string>(this.Puzzle1FilePath, '\n', true);
            this.Directions = FileReader.ValuesSplitter<string>(lines.First()).ToList();

            foreach (var line in lines.Skip(1))
            {
                var point = FileReader.ValueSplitter<string, string>(line, '=');
                var trimmed = point.Item2.ValueRemover("(").ValueRemover(")");
                var ways = FileReader.ValueSplitter<string, string>(trimmed, ',');

                this.PuzzleMeasurements.Add(point.Item1, (ways.Item1, ways.Item2));
            }
        }

        private double FollowInstructionsToZZZ()
        {
            double steps = 0;
            var i = 0;
            KeyValuePair<string, (string left, string right)> dirs = new();
            
            do
            {
                var direction = this.Directions[i];
                string way = string.Empty;

                if (steps is 0)
                {
                    dirs = this.PuzzleMeasurements.Where(k => k.Key == "AAA").Single();
                }

                if (direction is "L")
                {
                    way = dirs.Value.left;
                }
                else if (direction is "R")
                {
                    way = dirs.Value.right;
                }

                dirs = this.PuzzleMeasurements.Where(k => k.Key == way).Single();
                steps++;
                i++;

                if (i == this.Directions.Count())
                {
                    i = 0;
                }
            } while (dirs.Key != "ZZZ");

            return steps;
        }

        #region Stolen code Part II
        private readonly Regex nodeRegex = new Regex("\\w+");

        public record Node(string Id, string Left, string Right);

        private ImmutableDictionary<string, Node> ParseNodes(List<string> input) =>
            input.Skip(1)
            .Select(line => nodeRegex.Matches(line)).
                  Select(parts => new Node(parts[0].Value,
                                           parts[1].Value,
                                           parts[2].Value)).
                  ToImmutableDictionary(node => node.Id);

        private string ParseInstructions(string input) => input.Split("\n")[0];

        public List<string> FindPath(string from,
                                     Func<Node, bool> condition,
                                     ImmutableDictionary<string, Node> nodes,
                                     string instructions)
        {
            var path = new List<string>();
            var current = nodes[from];
            var lrCnt = 0;

            while (!condition(current))
            {
                path.Add(current.Id);
                current = nodes[instructions[lrCnt % instructions.Length] == 'L' ? current.Left : current.Right];
                lrCnt++;
            }

            return path;
        }

        private long FindLeastCommonMultiple(IEnumerable<long> numbers) =>
            numbers.Aggregate((long)1, (current, number) => current / GreatestCommonDivisor(current, number) * number);

        private long GreatestCommonDivisor(long a, long b)
        {
            while (b != 0)
            {
                a %= b;
                (a, b) = (b, a);
            }
            return a;
        }

        public object PartTwo(List<string> input)
        {
            var instructions = ParseInstructions(input.First());
            var nodes = ParseNodes(input);

            var deltas = nodes.Values.Where(n => n.Id.EndsWith('A')).
                                     Select(node => FindPath(node.Id, n => n.Id.EndsWith('Z'), nodes, instructions)).
                                     Select(path => (long)path.Count);

            return FindLeastCommonMultiple(deltas);
        }
        #endregion

        private double FollowInstructionsToEndingZ()
        {
            double steps = 0;
            var i = 0;
            List<KeyValuePair<string, (string left, string right)>> dirs = new();

            if (steps is 0)
            {
                dirs = this.PuzzleMeasurements.Where(k => k.Key.EndsWith('A')).ToList();
            }

            while (!dirs.TrueForAll(d => d.Key.EndsWith('Z')))
            {
                var direction = this.Directions[i];
                string way = string.Empty;



                for (var d = 0; d < dirs.Count; d++)
                {
                    if (direction is "L")
                    {
                        way = dirs[d].Value.left;
                    }
                    else if (direction is "R")
                    {
                        way = dirs[d].Value.right;
                    }

                    dirs[d] = this.PuzzleMeasurements.Where(k => k.Key == way).Single();
                }

                steps++;
                i++;

                if (i == this.Directions.Count())
                {
                    i = 0;
                }
            };

            return steps;
        }
    }
}
