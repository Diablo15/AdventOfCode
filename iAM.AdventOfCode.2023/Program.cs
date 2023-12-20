// See https://aka.ms/new-console-template for more information

using iAM.AdventOfCode._2023;
using iAM.AdventOfCode._2023.Helpers;

try
{
    var reader = new FileReader();
    //new Day1(reader).StartDay1();
    //new Day2(reader).StartDay2();
    //new Day3(reader).StartDay3();
    new Day4(reader).StartDay4();
}
catch (Exception ex)
{
    Console.WriteLine($"Something went wrong: {ex}");
    throw;
}