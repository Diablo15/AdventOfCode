namespace iAM.AdventOfCode.Shared.Models
{
    public record Coordinate(int X, int Y);

    public record Pipe(int fromX, int fromY, int toX, int toY);
}
