using iAM.AdventOfCode._2021.Models;
using iAM.AdventOfCode.Helpers;

namespace iAM.AdventOfCode._2021;

public class Day6 : AocDay
{
    private IEnumerable<int> PuzzleOneMeasurements { get; set; }
    private IEnumerable<int> ShadowList { get; set; }

    public Day6() : base(6, false, true)
    {
        this.PuzzleAltFilePath = "..//Examples//Day6Puzzle1Alt.txt";
        this.UseAltFile = false;
    }

    public override void Puzzle1Content()
    {
        PrepareDataFirstTime();
        Logging();
        var days = 80; 
        do
        {
            ExtractDayLifeTime();
            ReplaceFish();
            days--;
            Logging();
        } while (days > 0);

        // Answer: 375482
        Console.WriteLine($"Result -- Total fish after 80 days: {this.PuzzleOneMeasurements.Count()}");
    }

    public override void Puzzle2Content()
    {
        PrepareDataFirstTime();
        var days = 256;
        var fishes = BucketFishes(days);

        // Answer: 1689540415957
        Console.WriteLine($"Result -- Total fish after 256 days: {fishes}");

    }

    private void PrepareDataFirstTime()
    {
        var vals = FileReader.ReadInputValues<string>(Puzzle1FilePath, "\n", true);
        this.PuzzleOneMeasurements = FileReader.ValuesSplitter<int>(vals.First(), ",");
    }

    private void ExtractDayLifeTime()
    {
        var shadowList = new List<int>();
        foreach (var lifetime in this.PuzzleOneMeasurements)
        {
            var newLifetime = lifetime-1;
            shadowList.Add(newLifetime);
        }

        this.PuzzleOneMeasurements = shadowList;
    }

    private void ReplaceFish()
    {
        var shadowList = new List<int>();
        var newFish = new List<int>();
        foreach (var fish in PuzzleOneMeasurements)
        {
            if (fish < 0)
            {
                shadowList.Add(6);
                AddExtraFish(newFish);
                continue;
            }
            
            shadowList.Add(fish);
        }
        
        shadowList.AddRange(newFish);
        this.PuzzleOneMeasurements = shadowList;
    }

    private void AddExtraFish(List<int> source)
    {
        source.Add(8);
    }

    private long BucketFishes(int days)
    {
        // Instead of a list of fish, track counts per timer bucket
        long[] buckets = new long[9]; // index = timer value, value = fish count

// Populate from your input
        foreach (var fish in PuzzleOneMeasurements)
        {
            buckets[fish]++;
        }

// Simulate days
        for (int day = 0; day < days; day++)
        {
            long[] next = new long[9];

            next[8] = buckets[0]; // spawned new fish
            next[7] = buckets[8];
            next[6] = buckets[7] + buckets[0]; // reset fish rejoin at 6
            next[5] = buckets[6];
            next[4] = buckets[5];
            next[3] = buckets[4];
            next[2] = buckets[3];
            next[1] = buckets[2];
            next[0] = buckets[1];

            buckets = next;
        }

        long totalFish = buckets.Sum();
        return totalFish;
    }

    private void Logging()
    {
        // this.PuzzleOneMeasurements.ToList().ForEach(f => { Console.Write($"{f},"); });
        // Console.Write("\n");
    }
}