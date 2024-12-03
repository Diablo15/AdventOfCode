using iAM.AdventOfCode._2023.Flaws;
using iAM.AdventOfCode._2023.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Net;
using System.Runtime.InteropServices;

namespace iAM.AdventOfCode._2023
{
    public class Day5
    {
        private IEnumerable<string> PuzzleOneMeasurements { get; set; }
        //private string Puzzle1FilePath = "Day5Puzzle1.txt";
        private string Puzzle1FilePath = "Day5_small.txt";

        private IEnumerable<long> Seeds;
        private IEnumerable<(long startSeed, long range)> SeedPairs;
        private IEnumerable<long> DataMappingSets;
        private bool UsesSeedPairs = false;

        public Day5()
        {
            PuzzleOneMeasurements = Enumerable.Empty<string>();
        }

        public void StartDay5()
        {
            Console.WriteLine("******** Day 5 ********");
            Puzzle1();
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

            Console.WriteLine($"1. ======== Min Value == {this.SeedPairs.Min(s => s.startSeed)}");
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
                               range: seed.Skip(1).First().value
                           ));

                Console.WriteLine("Initial Seeds");
                SeedPairs.ToList().ForEach(s => Console.Write($"({s.startSeed}, {s.range})"));
                Console.WriteLine("\n");

                return;
            }

            this.Seeds = seeds;

            Console.WriteLine("Initial Seeds");
            Seeds.ToList().ForEach(s => Console.Write(s + ", "));
            Console.WriteLine("\n");
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
                    continue;
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
            var mappedPairs = new List<(long start, long range)>();

            foreach (var initPair in this.SeedPairs)
            {
                var seedPairsSplit = new List<(long start, long range)>();
                foreach (var map in input)
                {
                    var hasInterSect = initPair.HasIntersection(map, out var oStart, out var oRange);
                    (long s, long r) intersectedPair = new();

                    if (hasInterSect)
                    {
                        seedPairsSplit.Add((initPair.startSeed, (oStart - initPair.startSeed)));
                        intersectedPair = (oStart, oRange);
                        seedPairsSplit.Add(((oStart + oRange), (initPair.startSeed + initPair.range-1) - (oStart + oRange-1)));

                        seedPairsSplit.Add(intersectedPair.TransformMapper(map));
                    }
                }

                seedPairsSplit.RemoveAll(s => s.range < 1);

                if (!seedPairsSplit.Any())
                {
                    mappedPairs.Add(initPair);
                    continue;
                }

                mappedPairs.AddRange(seedPairsSplit.Distinct());
            }

            this.SeedPairs = mappedPairs;

            Console.WriteLine("Seeds");
            SeedPairs.ToList().ForEach(s => Console.Write($"({s.startSeed}, {s.range})"));
            Console.WriteLine("\n");
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
            Console.WriteLine("Seeds");
            Seeds.ToList().ForEach(s => Console.Write(s + ", "));
            Console.WriteLine("\n");
        }
    }

    public static class Day5Helpers
    {
        public static bool IsInRange(this long seed, (long dest, long source, long range) map)
        {
            return seed >= map.source && seed < map.source + map.range;
        }

        public static bool HasIntersection(this (long seed, long range) pair, (long dest, long source, long range) map, out long overlapStart, out long overlapRange)
        {
            overlapStart = 0;
            overlapRange = 0;

            var oStart = Math.Max(pair.seed, map.source);
            var oEnd = Math.Min((pair.seed + pair.range) - 1, (map.source + map.range) - 1);

            if (oEnd < oStart)
            {
                return false;
            }

            overlapStart = oStart;
            overlapRange = (oEnd - oStart) + 1;
            return true;
        }

        public static long TransformMapper(this long seed, (long dest, long source, long range) map)
        {
            return seed + (map.dest - map.source);
        }

        public static (long seed, long range) TransformMapper(this (long seed, long range) pairs, (long dest, long source, long range) map)
        {
            var newSeed = pairs.seed + (map.dest - map.source);
            return (newSeed, pairs.range);
        }
    }
}
