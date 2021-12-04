using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2021.DayClasses
{
    using System.Collections;
    using System.IO;

    public class FileReader : IFileReader
    {
        public FileReader() { }

        public List<string> ReadInputValues(string path, char delimiter = ' ')
        {
            var outputList = new List<string>();
            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = this.ValueSplitter(line, delimiter);
                    outputList.Add(values[0]);
                }

                return outputList;
            }
        }

        public string[] ValueSplitter(string line, char delimiter)
        {
            return line.Split(delimiter);
        }
    }
}
