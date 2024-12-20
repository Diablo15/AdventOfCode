namespace iAM.AdventOfCode.Helpers;

public static class FileReader
{
    static public IEnumerable<T> ReadInputValues<T>(string path, char delimiter = ' ', bool ignoreWhitLine = false)
    {
        var outputList = new List<T>();
        using (var reader = new StreamReader(@$"{AppDomain.CurrentDomain.BaseDirectory}Files\{path}"))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();

                if (ignoreWhitLine && string.IsNullOrEmpty(line))
                {
                    continue;
                }

                var values = ValuesSplitter<T>(line, delimiter);
                outputList.Add(values.First());
            }

            return outputList;
        }
    }

    static public IEnumerable<Tuple<T1, T2>> ReadInputValues<T1, T2>(string path, char delimiter = ' ')
    {
        var outputList = new List<Tuple<T1, T2>>();
        using (var reader = new StreamReader(@$"{AppDomain.CurrentDomain.BaseDirectory}Files\{path}"))
        {
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = ValuesSplitter<T1, T2>(line, delimiter);
                outputList.Add(values.First());
            }

            return outputList;
        }
    }

    static public IEnumerable<T> ValuesSplitter<T>(string line, char delimiter)
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

    static public IEnumerable<T> ValuesSplitter<T>(string line)
    {
        var result = new List<T>();

        if (!string.IsNullOrEmpty(line))
        {
            foreach (var character in line)
                try
                {
                    var value = (T)Convert.ChangeType(character, typeof(T));
                    result.Add(value);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error converting value '{character}' to type {typeof(T).Name}: {ex.Message}");
                    // Handle conversion error as needed
                }
        }

        return result;
    }

    static public IEnumerable<Tuple<T1, T2>> ValuesSplitter<T1, T2>(string line, char delimiter)
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

    static public Tuple<T1, T2> ValueSplitter<T1, T2>(string line, char delimiter)
    {
        Tuple<T1, T2> tupleResult = null;
        if (!string.IsNullOrEmpty(line))
        {
            var valueArray = line.Split(new[] { delimiter }, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                var valueOne = (T1)Convert.ChangeType(valueArray[0].Trim(), typeof(T1));
                var valueTwo = (T2)Convert.ChangeType(valueArray[1].Trim(), typeof(T2));
                tupleResult = new Tuple<T1, T2>(valueOne, valueTwo);
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"Error converting value '{valueArray}' to type {typeof(Tuple<T1, T2>).Name}: {ex.Message}");
                // Handle conversion error as needed
            }
        }

        return tupleResult;
    }

    static public string ValueStartRemover(string line, string toRemove)
    {
        return line.Split(toRemove, StringSplitOptions.TrimEntries)[1];
    }

    static public string ValueRemover(this string line, string toRemove)
    {
        return line.Replace(toRemove, "").Trim();
    }

    static public string WhiteSpaceRemover(this string line)
    {
        var newValue = line.Replace(" ", "");
        return newValue.Trim();
    }
}