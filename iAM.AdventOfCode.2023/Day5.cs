using iAM.AdventOfCode._2023.Helpers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace iAM.AdventOfCode._2023
{
    public class Day5
    {
        private IEnumerable<string> PuzzleOneMeasurements { get; set; }
        private IFileReader Reader { get; set; }
        private string Puzzle1FilePath = "Day5Puzzle1.txt";

        private IEnumerable<string> Seeds { get; set; }
        private Dictionary<long, long> SeedsToSoilMap { get; set; } = new();
        private Dictionary<long, long> SoilToFertMap { get; set; } = new();
        private Dictionary<long, long> FertToWaterMap { get; set; } = new();
        private Dictionary<long, long> WaterToLightMap { get; set; } = new();
        private Dictionary<long, long> LightToTempMap { get; set; } = new();
        private Dictionary<long, long> TempToHumidMap { get; set; } = new();
        private Dictionary<long, long> HumidToLocationMap { get; set; } = new();

        public Day5(IFileReader reader)
        {
            Reader = reader;
            PuzzleOneMeasurements = new List<string>();
        }

        public void StartDay5()
        {
            Console.WriteLine("******** Day 5 ********");
            Puzzle1();
        }

        private void Puzzle1()
        {
            Console.WriteLine("******** Puzzle 1 ********");
            PuzzleOneMeasurements = Reader.ReadInputValues<string>(Puzzle1FilePath, '\n', true);

            FillSeeds(PuzzleOneMeasurements);
            FillAllMaps(PuzzleOneMeasurements);


            //Console.WriteLine($"1. ======== Total Sum == {sum}");
        }

        private void FillSeeds(IEnumerable<string> measurements)
        {
            var seedLine = Reader.ValueRemover(measurements.First(), "seeds:");
            this.Seeds = Reader.ValueSplitter<string>(seedLine, ' ');
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

            List<string> sts = new List<string>();
            List<string> stf = new List<string>();
            List<string> ftw = new List<string>();
            List<string> wtl = new List<string>();
            List<string> ltt = new List<string>();
            List<string> tth = new List<string>();
            List<string> htl = new List<string>();

            foreach (var group in result)
            {


                switch (group.Key)
                {
                    case "seed-to-soil map:":
                        foreach (var line in group.Value)
                        {
                            sts.Add(line);
                        }
                        ConstructMaps(sts, this.SeedsToSoilMap);
                        break;
                    case "soil-to-fertilizer map:":
                        foreach (var line in group.Value)
                        {
                            stf.Add(line);
                        }
                        break;
                    case "fertilizer-to-water map:":
                        foreach (var line in group.Value)
                        {
                            ftw.Add(line);
                        }
                        break;
                    case "water-to-light map:":
                        foreach (var line in group.Value)
                        {
                            wtl.Add(line);
                        }
                        break;
                    case "light-to-temperature map:":
                        foreach (var line in group.Value)
                        {
                            ltt.Add(line);
                        }
                        break;
                    case "temperature-to-humidity map:":
                        foreach (var line in group.Value)
                        {
                            tth.Add(line);
                        }
                        break;
                    case "humidity-to-location map:":
                        foreach (var line in group.Value)
                        {
                            htl.Add(line);
                        }
                        break;
                }
            }
        }

        private void ConstructMaps(IEnumerable<string> input, Dictionary<long, long> dict)
        {
            var values = input.ToList();
            var lists = new List<List<long>>();
            long maxValue = 0;

            foreach (var value in values)
            {
                var realvalues = Reader.ValueSplitter<long>(value, ' ').ToList();
                if (maxValue < realvalues.First())
                {
                    maxValue = realvalues.First();
                }

                lists.Add(realvalues);
            }

            foreach (var value in lists)
            {
                var source = value[1];
                var destination = value[0];
                var range = value[2];

                for(var i = 0; i != range; i++)
                {
                    dict.Add(source, destination);
                    source++;
                    destination++;
                }
            }

            Console.WriteLine(dict);
        }
    }
}
