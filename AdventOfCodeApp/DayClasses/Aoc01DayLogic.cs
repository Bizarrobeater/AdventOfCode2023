using AdventOfCodeApp.Util.FileReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeApp.DayClasses
{
    internal class Aoc01DayLogic : IDayLogic
    {
        public Dictionary<int, Dictionary<int, long>> ExpectedTestResults => new Dictionary<int, Dictionary<int, long>>()
        {
            {1, new Dictionary<int, long>()
                {
                { 1, 142 }
                }
            }
        };

        public long RunQuestion1(FileInfo file)
        {
            var reader = new LineSplitFileReader();
            var lines = reader.GetReadableFileContent(file);
            List<int> lineNumbers = new List<int>();
            List<char> chars = new List<char>();

            foreach (var line in lines)
            {
                chars.Clear();
                foreach(char c in line)
                {
                    if (int.TryParse(c.ToString(), out int _))
                    {
                        chars.Add(c);
                    }
                }
                if (chars.Count > 0)
                {
                    lineNumbers.Add(int.Parse($"{chars[0]}{chars[chars.Count - 1]}"));
                }                         
            }

            long result = 0;
            foreach (var number in lineNumbers)
            {
                result += number;
            }
            return result;

        }

        public long RunQuestion2(FileInfo file)
        {
            throw new NotImplementedException();
        }
    }
}
