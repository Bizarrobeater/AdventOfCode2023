﻿using AdventOfCodeApp.Util.FileReaders;
using System.Data.SqlTypes;

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

        Dictionary<char, Dictionary<string, char>> CharToWordToInt = new Dictionary<char, Dictionary<string, char>>()
        {
            {'o', new Dictionary<string, char>()
                {
                    { "one", '1' }
                }
            },
            {
                't', new Dictionary<string, char>()
                {
                    {"two", '2' },
                    {"three", '3' }
                }
            },
            { 'f', new Dictionary<string, char>()
                {
                    { "four", '4' },
                    {"five", '5' },
                }
            },
            {
                's', new Dictionary<string, char>()
                {
                    {"six", '6' },
                    { "seven", '7' }
                }
            },
            { 'e', new Dictionary<string, char>()
            {
                {"eight", '8' },
            }
            },
            {
                'n', new Dictionary<string, char>()
                {
                    {"nine", '9' }
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
            foreach (var line in lines)
            {
                chars.Clear();
                // This for loop finds the first number in the sequence
                for (int i = 0; i < line.Length; i++)
                {
                    if (CheckAndAddChar(line, i, chars))
                        break;
                }

                // this for loop finds the last number in the sequence
                for (int i = line.Length - 1;i >= 0; i--) 
                {
                    if (CheckAndAddChar(line, i, chars))
                        break;

                }

                if (chars.Count > 0)
                    lineNumbers.Add(GetNumberFromFoundNumbers(chars));
            }

            long result = 0;
            foreach (var number in lineNumbers)
                result += number;
            return result;
        }

        private bool CheckAndAddChar(string line, int index, List<char> chars)
        {
            char currChar = line[index];
            string substring;
            if (IsCharInt(currChar))
            {
                chars.Add(currChar);
                return true;
            }
            if (!CharToWordToInt.TryGetValue(currChar, out Dictionary<string, char>? dict))
                return false;
            substring = line.Substring(index);
            if (TryGetNumberFromText(substring, dict, out char numberChar))
            {
                chars.Add(numberChar);
                return true;
            }
            return false;
        }

        private bool IsCharInt(char ch)
        {
            return int.TryParse(ch.ToString(), out int _);
        }

        private bool TryGetNumberFromText(string text, Dictionary<string, char> inputDict, out char number)
        {
            foreach (KeyValuePair<string, char> pair in inputDict)
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

        /*
        Benchmark improvements
        Original 1000 runs
            Benchmark in ticks:
            First Run Time: 60567
            Last Run Time: 124080
            Average: 64096,611
            Median: 62085
            Max Time: 124080
            Min Time: 60567

        First improvement - only check chars that are relevant (CharToWordToInt dict)
            Benchmark in ticks:
            First Run Time: 12729
            Last Run Time: 64274
            Average: 15363,456
            Median: 13589
            Max Time: 64274
            Min Time: 12729
        
        Second improvement - only look for first and last number
            Benchmark in ticks:
            First Run Time: 5937
            Last Run Time: 27411
            Average: 7195,603
            Median: 6955
            Max Time: 27411
            Min Time: 5937

        Second improvement, but refactored (100.000 runs)
            Benchmark in ticks:
            First Run Time: 5825
            Last Run Time: 349858
            Average: 6159,99742
            Median: 6060
            Max Time: 349858
            Min Time: 5825
         */
    }
}