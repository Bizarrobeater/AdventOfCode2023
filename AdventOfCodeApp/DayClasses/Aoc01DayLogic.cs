using AdventOfCodeApp.Util.FileReaders;

namespace AdventOfCodeApp.DayClasses
{
    internal class Aoc01DayLogic : IDayLogic
    {
        public Dictionary<int, Dictionary<int, long>> ExpectedTestResults => new Dictionary<int, Dictionary<int, long>>()
        {
            {
                1, new Dictionary<int, long>()
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

        private Dictionary<string, char> WordToInt = new Dictionary<string, char>()
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

        public long RunQuestion1(FileInfo file, bool isBenchmark = false)
        {
            var reader = new LineSplitFileReader();
            var lines = reader.GetReadableFileContent(file, isBenchmark);
            List<int> lineNumbers = new List<int>();
            List<char> chars = new List<char>();

            foreach (var line in lines)
            {
                chars.Clear();
                foreach (char c in line)
                {
                    if (IsCharInt(c))
                        chars.Add(c);
                }
                if (chars.Count > 0)
                    lineNumbers.Add(GetNumberFromFoundNumbers(chars));
            }

            long result = 0;
            foreach (var number in lineNumbers)
                result += number;
            return result;
        }

        public long RunQuestion2(FileInfo file, bool isBenchmark = false)
        {
            var reader = new LineSplitFileReader();
            var lines = reader.GetReadableFileContent(file, isBenchmark);
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
                    if (IsCharInt(currChar))
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
                    lineNumbers.Add(GetNumberFromFoundNumbers(chars));
            }

            long result = 0;
            foreach (var number in lineNumbers)
                result += number;
            return result;
        }

        private bool IsCharInt(char ch)
        {
            return int.TryParse(ch.ToString(), out int _);
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

        private int GetNumberFromFoundNumbers(List<char> numbers)
        {
            return int.Parse($"{numbers[0]}{numbers[numbers.Count - 1]}");
        }
    }
}