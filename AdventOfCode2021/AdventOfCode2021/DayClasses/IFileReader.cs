using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode2021.DayClasses
{
    public interface IFileReader
    {
        public List<string> ReadInputValues(string path, char delimiter = ' ');
        public string[] ValueSplitter(string line, char delimiter);
    }
}
