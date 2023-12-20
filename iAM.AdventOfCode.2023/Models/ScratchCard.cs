namespace iAM.AdventOfCode._2023.Models;

public class ScratchCard
{
    public int OwningTimes { get; set; }
    public IEnumerable<int> WinningNumbers { get; set; }
    
    public IEnumerable<int> PlayingNumbers { get; set; }

    public IEnumerable<int> WinningMatches { get; set; }
}