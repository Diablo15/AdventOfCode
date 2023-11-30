namespace iAM.AdventOfCode._2021;

public interface IFileReader
{
    IEnumerable<T> ReadInputValues<T>(string path, char delimiter = ' ');

    IEnumerable<Tuple<T1, T2>> ReadInputValues<T1, T2>(string path, char delimiter = ' ');
}