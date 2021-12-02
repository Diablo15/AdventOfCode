using System;
using System.Collections.Generic;

namespace AdventOfCode2021.DayClasses.DayTwo
{
    public class Puzzle2 : Puzzle1
    {
        public override Dictionary<string, int> GetDirectionValues(List<Tuple<string, int>> inputList)
        {
            var dict = new Dictionary<string, int>
            {
                { "horizontal", 0 },
                { "depth", 0 },
                { "aim", 0 }
            };

            foreach (var direction in inputList)
            {
                switch (direction.Item1)
                {
                    case "forward":
                        dict["horizontal"] += direction.Item2;
                        dict["depth"] += (dict["aim"] * direction.Item2);
                        break;
                    case "up":
                        dict["aim"] -= direction.Item2;
                        break;
                    case "down":
                        dict["aim"] += direction.Item2;
                        break;
                    default:
                        Console.WriteLine("Unexpected commando");
                        break;

                }

                Console.WriteLine($"H: {dict["horizontal"]} / D:{dict["depth"]} A: {dict["aim"]} - {direction.Item1} {direction.Item2}");
            }

            return dict;
        }
    }
}
