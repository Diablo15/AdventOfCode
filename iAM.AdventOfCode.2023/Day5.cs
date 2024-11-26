using iAM.AdventOfCode._2023.Flaws;
using iAM.AdventOfCode._2023.Helpers;
using System.Collections.Generic;

namespace iAM.AdventOfCode._2023
{
    public class Day5
    {
        private IEnumerable<string> PuzzleOneMeasurements { get; set; }
        private string Puzzle1FilePath = "Day5Puzzle1.txt";

        private IEnumerable<long> Seeds;
        private IEnumerable<long> DataMappingSets;

        public Day5()
        {
            PuzzleOneMeasurements = Enumerable.Empty<string>();
        }

        public void StartDay5()
        {
            Console.WriteLine("******** Day 5 ********");
            Puzzle1();
        }

        private void Puzzle1()
        {
            Console.WriteLine("******** Puzzle 1 ********");
            //Flaws.Day5.Start(Puzzle1FilePath);
            PuzzleOneMeasurements = FileReader.ReadInputValues<string>(Puzzle1FilePath, '\n', true);

            FillSeeds(PuzzleOneMeasurements);
            FillAllMaps(PuzzleOneMeasurements);

            // Answer: 621354867
            Console.WriteLine($"1. ======== Min Value == {this.Seeds.Min()}");
        }

        private void FillSeeds(IEnumerable<string> measurements)
        {
            var seedLine = FileReader.ValueRemover(measurements.First(), "seeds:");
            this.Seeds = FileReader.ValueSplitter<long>(seedLine, ' ');
        }

        private void FillAllMaps(IEnumerable<string> measurements)
        {
            var result = new Dictionary<string, List<string>>();
            string currentMarker = null;

            foreach (var line in measurements)
            {
                if (line.EndsWith(":")) // Identify a marker
                {
                    currentMarker = line;
                    if (!result.ContainsKey(currentMarker))
                    {
                        result[currentMarker] = new List<string>();
                    }
                }
                else if (currentMarker != null) // Add lines under the current marker
                {
                    result[currentMarker].Add(line);
                }
            }

            foreach (var group in result)
            {
                var mappedMap = new List<(long dest, long source, long range)>();
                foreach (var line in group.Value)
                {
                    var map = ConstructMap(line);
                    mappedMap.Add(map);
                }

                SearchFindAndMap(mappedMap);
            }
        }

        private (long dest, long source, long range) ConstructMap(string line)
        {
            var realvalues = FileReader.ValueSplitter<long>(line, ' ').ToList();

            return (realvalues[0], realvalues[1], realvalues[2]);
        }

        private void SearchFindAndMap(List<(long dest, long source, long range)> input)
        {
            var newSeeds = new List<long>();
            foreach (var seed in this.Seeds.ToList())
            {
                var selectedMap = input.SingleOrDefault(map => seed.IsInRange(map));

                if (selectedMap is (0,0,0))
                {
                    newSeeds.Add(seed);
                    continue;
                }

                newSeeds.Add(seed.TransformMapper(selectedMap));
            }
            Seeds = newSeeds;
        }
    }

    public static class Day5Helpers
    {
        public static bool IsInRange(this long seed, (long dest, long source, long range) map)
        {
            return seed >= map.source && seed < map.source + map.range;
        }

        public static long TransformMapper(this long seed, (long dest, long source, long range) map)
        {
            return seed + (map.dest - map.source);
        }
    }
}
