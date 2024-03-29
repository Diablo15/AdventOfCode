namespace iAM.AdventOfCode._2021;

public class FileReader : IFileReader
{
    public FileReader()
    {
    }

    public IEnumerable<T> ReadInputValues<T>(string path, char delimiter = ' ')
    {
        var outputList = new List<T>();
        using (var reader = new StreamReader(@$"{AppDomain.CurrentDomain.BaseDirectory}Files\{path}"))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = ValueSplitter<T>(line, delimiter);
                outputList.Add(values.First());
            }

            return outputList;
        }
    }

    public IEnumerable<Tuple<T1, T2>> ReadInputValues<T1, T2>(string path, char delimiter = ' ')
    {
        var outputList = new List<Tuple<T1, T2>>();
        using (var reader = new StreamReader(@$"{AppDomain.CurrentDomain.BaseDirectory}Files\{path}"))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = ValueSplitter<T1, T2>(line, delimiter);
                outputList.Add(values.First());
            }

            return outputList;
        }
    }

    private IEnumerable<T> ValueSplitter<T>(string line, char delimiter)
    {
        var result = new List<T>();

        if (!string.IsNullOrEmpty(line))
        {
            var valueArray = line.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var stringValue in valueArray)
                try
                {
                    var value = (T)Convert.ChangeType(stringValue.Trim(), typeof(T));
                    result.Add(value);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error converting value '{stringValue}' to type {typeof(T).Name}: {ex.Message}");
                    // Handle conversion error as needed
                }
        }

        return result;
    }

    private IEnumerable<Tuple<T1, T2>> ValueSplitter<T1, T2>(string line, char delimiter)
    {
        var result = new List<Tuple<T1, T2>>();

        if (!string.IsNullOrEmpty(line))
        {
            var valueArray = line.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                var valueOne = (T1)Convert.ChangeType(valueArray[0].Trim(), typeof(T1));
                var valueTwo = (T2)Convert.ChangeType(valueArray[1].Trim(), typeof(T2));
                result.Add(new Tuple<T1, T2>(valueOne, valueTwo));
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Error converting value '{valueArray}' to type {typeof(Tuple<T1, T2>).Name}: {ex.Message}");
                // Handle conversion error as needed
            }
        }

        return result;
    }
}