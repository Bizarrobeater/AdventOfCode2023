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
            },
            {
                2, new Dictionary<int, long>()
                {
                    { 1, 281 }
                }
            }
        };

        Dictionary<string, char> WordToInt = new Dictionary<string, char>()
        {
            {"one", '1' },
            {"two", '2'},
            {"three", '3'},
            {"four", '4'},
            {"five", '5' },
            {"six", '6' },
            {"seven", '7' },
            {"eight", '8' },
            {"nine", '9' },
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
            var reader = new LineSplitFileReader();
            var lines = reader.GetReadableFileContent(file);
            List<int> lineNumbers = new List<int>();
            List<char> chars = new List<char>();
            string substring;
            char currChar;

            foreach (var line in lines)
            {
                chars.Clear();
                for (int i = 0; i < line.Length; i++)
                {
                    currChar = line[i];
                    if (int.TryParse(currChar.ToString(), out int _))
                    {
                        chars.Add(currChar);
                        continue;
                    }
                    substring = line.Substring(i);
                    if (TryGetNumberFromText(substring, out char numberChar))
                    {
                        chars.Add(numberChar); 
                        continue;
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

        private bool TryGetNumberFromText(string text, out char number)
        {
            foreach (KeyValuePair<string, char> pair in WordToInt)
            {
                if (text.StartsWith(pair.Key))
                {
                    number = pair.Value;
                    return true;
                }
            }
            number = '-';
            return false;
        }
    }
}
