// See https://aka.ms/new-console-template for more information

using iAM.AdventOfCode._2021;

try
{
    var reader = new FileReader();
    //new Day1(reader).StartDay1();
    //new Day2(reader).StartDay2();
    new Day3(reader).StartDay3();
}
catch (Exception ex)
{
    Console.WriteLine($"Something went wrong: {ex}");
    throw;
}