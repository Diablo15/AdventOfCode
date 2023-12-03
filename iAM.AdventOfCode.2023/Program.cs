// See https://aka.ms/new-console-template for more information

using iAM.AdventOfCode._2023;
using iAM.AdventOfCode._2023.Helpers;

try
{
    var reader = new FileReader();
    new Day1(reader).StartDay1();
}
catch (Exception ex)
{
    Console.WriteLine($"Something went wrong: {ex}");
    throw;
}