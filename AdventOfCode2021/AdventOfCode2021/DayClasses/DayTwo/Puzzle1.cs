using System;
using System.Collections.Generic;

namespace AdventOfCode2021.DayClasses.DayTwo
{
    using System.IO;
    using System.Linq;

    public class Puzzle1
    {
        public List<Tuple<string, int>> PuzzleOneMeasurements { get; set; }

        public void Start()
        {
            this.PuzzleOneMeasurements = new ();

            this.ReadInputValues(this.PuzzleOneMeasurements);

            var directions = this.GetDirectionValues(this.PuzzleOneMeasurements);

            Console.WriteLine($"=========================== Total depth: {directions["depth"]}");
            Console.WriteLine($"=========================== Total course: {directions["horizontal"]}");
            Console.WriteLine($"=========================== Total answer: {directions["horizontal"] * directions["depth"]}");
        }

        public List<Tuple<string, int>> ReadInputValues(List<Tuple<string, int>> outputList)
        {
            var path = Path.Combine(@".\DayClasses\DayTwo\inputday2puzzle1.csv");

            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(' ');

                    outputList.Add(new Tuple<string, int>(values[0], Convert.ToInt32(values[1])));
                }
                
                return outputList;
            }
        }

        public virtual Dictionary<string, int> GetDirectionValues(List<Tuple<string, int>> inputList)
        {
            var dict = new Dictionary<string, int>
            {
                { "horizontal", 0 },
                { "depth", 0 }
            };

            foreach (var direction in inputList)
            {
                switch (direction.Item1)
                {
                    case "forward":
                        dict["horizontal"] += direction.Item2;
                        Console.WriteLine($"Horizontal: {dict["horizontal"]} {direction.Item1} {direction.Item2}");
                        break;
                    case "up":
                        dict["depth"] -= direction.Item2;
                        Console.WriteLine($"Depth: {dict["depth"]} {direction.Item1} {direction.Item2}");
                        break;
                    case "down":
                        dict["depth"] += direction.Item2;
                        Console.WriteLine($"Depth: {dict["depth"]} {direction.Item1} {direction.Item2}");
                        break;
                    default:
                        Console.WriteLine("Unexpected commando");
                        break;

                }
            }

            return dict;
        }
    }
}
