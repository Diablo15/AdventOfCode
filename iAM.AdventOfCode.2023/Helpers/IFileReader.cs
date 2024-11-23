namespace iAM.AdventOfCode._2023.Helpers;

public interface IFileReader
{
    IEnumerable<T> ReadInputValues<T>(string path, char delimiter = ' ', bool ignoreWhitLine = false);

    IEnumerable<Tuple<T1, T2>> ReadInputValues<T1, T2>(string path, char delimiter = ' ');

    IEnumerable<T> ValueSplitter<T>(string line, char delimiter);

    IEnumerable<Tuple<T1, T2>> ValueSplitter<T1, T2>(string line, char delimiter);

    string ValueRemover(string line, string toRemove);
}