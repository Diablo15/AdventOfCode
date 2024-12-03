using iAM.AdventOfCode._2023.Flaws;
using iAM.AdventOfCode._2023.Helpers;
using System.Collections.Generic;
using System.Net;
using System.Runtime.InteropServices;

namespace iAM.AdventOfCode._2023
{
    public class Day5
    {
        private IEnumerable<string> PuzzleOneMeasurements { get; set; }
        private string Puzzle1FilePath = "Day5Puzzle1.txt";
        //private string Puzzle1FilePath = "Day5_small.txt";

        private IEnumerable<long> Seeds;
        private IEnumerable<(long startSeed, long endSeed)> SeedPairs;
        private IEnumerable<long> DataMappingSets;
        private bool UsesSeedPairs = false;

        public Day5()
        {
            PuzzleOneMeasurements = Enumerable.Empty<string>();
        }

        public void StartDay5()
        {
            Console.WriteLine("******** Day 5 ********");
            //Puzzle1();
            Puzzle2();
        }

        private void Puzzle1()
        {
            Console.WriteLine("******** Puzzle 1 ********");
            PuzzleOneMeasurements = FileReader.ReadInputValues<string>(Puzzle1FilePath, '\n', true);

            FillSeeds(PuzzleOneMeasurements);
            FillAllMaps(PuzzleOneMeasurements);

            // Answer: 621354867
            Console.WriteLine($"1. ======== Min Value == {this.Seeds.Min()}");
        }

        private void Puzzle2()
        {
            Console.WriteLine("******** Puzzle 2 ********");

            PuzzleOneMeasurements = FileReader.ReadInputValues<string>(Puzzle1FilePath, '\n', true);

            this.UsesSeedPairs = true;

            FillSeeds(PuzzleOneMeasurements);
            FillAllMaps(PuzzleOneMeasurements);

            Console.WriteLine($"1. ======== Min Value == {this.Seeds.Min()}");
        }

        private void FillSeeds(IEnumerable<string> measurements)
        {
            var seedLine = FileReader.ValueRemover(measurements.First(), "seeds:");
            var seeds = FileReader.ValueSplitter<long>(seedLine, ' ');

            if (UsesSeedPairs)
            {
                this.SeedPairs = seeds
                           .Select((value, index) => new { value, index })
                           .GroupBy(x => x.index / 2)
                           .Select(seed => (
                               start: seed.First().value,
                               range: seed.First().value + (seed.Skip(1).First().value -1)
                           ));

                return;
            }

            this.Seeds = seeds;
        }

        //private void FillSeedPairs(IEnumerable<string> measurements)
        //{
        //    var seedLine = FileReader.ValueRemover(measurements.First(), "seeds:");
        //    var seeds = FileReader.ValueSplitter<long>(seedLine, ' ').ToList();


        //}

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

                if (UsesSeedPairs)
                {
                    SearchFindAndMapPairs(mappedMap);
                    UsesSeedPairs = false;
                }

                SearchFindAndMap(mappedMap);
            }
        }

        private (long dest, long source, long range) ConstructMap(string line)
        {
            var realvalues = FileReader.ValueSplitter<long>(line, ' ').ToList();

            return (realvalues[0], realvalues[1], realvalues[2]);
        }

        private void SearchFindAndMapPairs(List<(long dest, long source, long range)> input)
        {
            var newSeedPairs = new List<(long start, long end)>();
            var rangeSeedPairs = new List<(long start, long end)>();
            var newSeeds = new List<long[]>(); 

            foreach (var seedPair in this.SeedPairs.ToArray())
            {
                var seed = seedPair.startSeed;
                var selectedMaps = input.Where(map => seedPair.HasIntersection(map, out long overlapStart, out long overlapEnd)).ToList();

                if (!selectedMaps.Any())
                {
                    newSeedPairs.Add(seedPair);
                    continue;
                }

                foreach (var selectedMap in selectedMaps)
                {
                    seedPair.HasIntersection(selectedMap, out long overlapStart, out long overlapEnd);

                    newSeedPairs.Add((seedPair.startSeed, overlapStart-1));
                    var mapablePair = (overlapStart, overlapEnd);
                    newSeedPairs.Add((overlapEnd + 1, seedPair.endSeed));

                    var pairs = mapablePair.TransformMapper(selectedMap);
                    //newSeedPairs.Append(x => );
                }
            }
        }
        private void SearchFindAndMap(List<(long dest, long source, long range)> input)
        {
            var newSeeds = new List<long>();

            foreach (var seed in this.Seeds.ToList())
            {
                var selectedMap = input.SingleOrDefault(map => seed.IsInRange(map));

                if (selectedMap is (0, 0, 0))
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

        public static bool HasIntersection(this (long seed, long range) pair, (long dest, long source, long range) map, out long overlapStart, out long overlapEnd)
        {
            overlapStart = Math.Max(pair.seed, map.source);
            overlapEnd = Math.Min(pair.range, map.source + map.range);

            return overlapStart <= overlapEnd;
        }

        public static long TransformMapper(this long seed, (long dest, long source, long range) map)
        {
            return seed + (map.dest - map.source);
        }
        
        public static IEnumerable<(long, long)[]> TransformMapper(this (long startSeed, long endSeed) pairs, (long dest, long source, long range) map)
        {
            var seed = pairs.startSeed;
            var mappedSeeds = new List<(long, long)>();

            for (int i = 0; seed <= pairs.endSeed; seed++)
            {
                mappedSeeds.Add((seed + (map.dest - map.source), 1));
            }

            return mappedSeeds.Chunk(10000);
        }
    }
}
