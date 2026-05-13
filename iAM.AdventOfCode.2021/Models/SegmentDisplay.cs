namespace iAM.AdventOfCode._2021.Models;

public abstract class SegmentDisplay
{ 
    public int NumberValue { get; }
    public char[] TurnedOnModules { get; }
    public int TurnedOnModulesCount { get; }

    protected SegmentDisplay(int numberValue, char[] modules)
    {
        NumberValue = numberValue;
        TurnedOnModules = modules;
        TurnedOnModulesCount = modules.Length;
    }
}