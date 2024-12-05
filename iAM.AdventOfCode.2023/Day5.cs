using iAM.AdventOfCode._2023.Flaws;
using iAM.AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Net;
using System.Runtime.InteropServices;

namespace iAM.AdventOfCode._2023
{
    public class Day5 : AoCDay
    {
        private IEnumerable<string> PuzzleOneMeasurements { get; set; }
        //private string Puzzle1FilePath = "Examples\\Day5_small.txt";

        private IEnumerable<long> Seeds;
        private IEnumerable<(long startSeed, long range)> SeedPairs;
        private IEnumerable<long> DataMappingSets;
        private bool UsesSeedPairs = false;

        public Day5() : base(5, false)
        {
            PuzzleOneMeasurements = Enumerable.Empty<string>();
        }

        public override void Puzzle1Content()
        {
            PuzzleOneMeasurements = FileReader.ReadInputValues<string>(Puzzle1FilePath, '\n', true);

            FillSeeds(PuzzleOneMeasurements);
            FillAllMaps(PuzzleOneMeasurements);

            // Answer: 621354867
            Console.WriteLine($"1. ======== Min Value == {this.Seeds.Min()}");
        }

        public override void Puzzle2Content()
        {
            Console.WriteLine("******** Puzzle 2 ********");

            PuzzleOneMeasurements = FileReader.ReadInputValues<string>(Puzzle1FilePath, '\n', true);

            this.UsesSeedPairs = true;

            FillSeeds(PuzzleOneMeasurements);
            FillAllMaps(PuzzleOneMeasurements);

            // Answer: 15880236
            Console.WriteLine($"1. ======== Min Value == {this.SeedPairs.Min(s => s.startSeed)}");
        }

        private void FillSeeds(IEnumerable<string> measurements)
        {
            var seedLine = FileReader.ValueStartRemover(measurements.First(), "seeds:");
            var seeds = FileReader.ValueSplitter<long>(seedLine, ' ');

            if (UsesSeedPairs)
            {
                this.SeedPairs = seeds
                           .Select((value, index) => new { value, index })
                           .GroupBy(x => x.index / 2)
                           .Select(seed => (
                               start: seed.First().value,
                               end: (seed.First().value+seed.Skip(1).First().value)-1
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
                var mappedMap = new List<(string type, long dest, long source, long range)>();
                foreach (var line in group.Value)
                {
                    var map = ConstructMap(line);

                    map.type = "seed";
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

        private (string type, long dest, long source, long range) ConstructMap(string line)
        {
            var realvalues = FileReader.ValueSplitter<long>(line, ' ').ToList();

            if (UsesSeedPairs)
            {
                return (string.Empty, realvalues[1], realvalues[1] + realvalues[2] - 1, realvalues[0] - realvalues[1]);
            }

            return (string.Empty, realvalues[0], realvalues[1], realvalues[2]);
        }

        private void SearchFindAndMapPairs(List<(string type, long sourceFrom, long sourceTo, long destOffset)> input)
        {
            List<(long start, long end)> result = [];
                        
            foreach((long start, long end) seedPair in this.SeedPairs) {
                (long from, long to) testRange = seedPair;
                bool allDone = false;

                do
                {
                    // Find the last mapping where the start is less than the start of the range
                    (string type, long sourceFrom, long sourceTo, long destOffset)
                        = input.LastOrDefault(m => m.sourceFrom <= testRange.from && testRange.from <= m.sourceTo, ("",0, 0, 0));
                    // There aren't any
                    if (type == "")
                    {
                        // Does the end fit in any mappings?
                        (type, sourceFrom, sourceTo, destOffset)
                            = input.LastOrDefault(m => m.sourceFrom <= testRange.to && testRange.to <= m.sourceTo, ("", 0, 0, 0));
                        if (type == "")
                        {
                            // If there aren't any, add the whole range and end
                            result.Add(testRange);
                            allDone = true;
                        }
                        else
                        {
                            // Add the start of the range, set the range to the end and continue
                            result.Add((testRange.from, sourceFrom - 1));
                            testRange = (sourceFrom, testRange.to);
                        }
                    }
                    // If the end of the mapping is greater than the end of the range, add the whole range (with offset) and end
                    else if (sourceTo >= testRange.to)
                    {
                        result.Add((testRange.from + destOffset, testRange.to + destOffset));
                        allDone = true;
                    }
                    // Otherwise, add from the start of the range to the end of the mapping (with offsets), set the range start to the mapping end plus one and continue
                    else
                    {
                        //sourceNumber = sourceNumber + destFrom - sourceFrom;
                        result.Add((testRange.from + destOffset, sourceTo + destOffset));
                        testRange = (sourceTo + 1, testRange.to);
                    }
                } while (!allDone);
            }

            this.SeedPairs = result;

            Console.WriteLine("Seeds");
            SeedPairs.ToList().ForEach(s => Console.Write($"({s.startSeed}, {s.range})"));
            Console.WriteLine("\n");
            
        }

        private void SearchFindAndMap(List<(string type, long dest, long source, long range)> input)
        {
            var newSeeds = new List<long>();

            foreach (var seed in this.Seeds.ToList())
            {
                var selectedMap = input.SingleOrDefault(map => seed.IsInRange(map));

                if (selectedMap is ("",0, 0, 0))
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
        public static bool IsInRange(this long seed, (string type, long dest, long source, long range) map)
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

        public static long TransformMapper(this long seed, (string type, long dest, long source, long range) map)
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
