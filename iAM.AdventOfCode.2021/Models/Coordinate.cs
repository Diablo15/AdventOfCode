namespace iAM.AdventOfCode._2021.Models;

public class Coordinate
{
    public double Latitude { get; }
    public double Longitude { get; }
    
    public int Hit { get; set; }

    public Coordinate(double latitude, double longitude, int hit = 0)
    {
        // if (latitude < -90 || latitude > 90)
        //     throw new ArgumentOutOfRangeException(nameof(latitude), "Latitude must be between -90 and 90.");
        // if (longitude < -180 || longitude > 180)
        //     throw new ArgumentOutOfRangeException(nameof(longitude), "Longitude must be between -180 and 180.");

        Latitude = latitude;
        Longitude = longitude;
        Hit = hit;
    }
    
    public override bool Equals(object? obj)
    {
        if (obj is not Coordinate other) return false;
        return Latitude == other.Latitude && Longitude == other.Longitude;
    }

    public override int GetHashCode() => HashCode.Combine(Latitude, Longitude);

    public static implicit operator Coordinate(Tuple<double, double> tuple)
        => new Coordinate(tuple.Item1, tuple.Item2);
    
    public static implicit operator Tuple<double, double>(Coordinate coordinate)
        => new Tuple<double, double>(coordinate.Latitude, coordinate.Longitude);

    public bool IsEmpty => Latitude == 0 && Longitude == 0;

    public override string ToString() => $"({Latitude}, {Longitude})";

}