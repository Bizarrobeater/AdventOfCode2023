using AdventOfCodeApp.Util.FileReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeApp.DayClasses
{
    internal class Aoc06DayLogic : IDayLogic
    {
        public Dictionary<int, Dictionary<int, long>> ExpectedTestResults => new Dictionary<int, Dictionary<int, long>>()
        {
            {1, new Dictionary<int, long>()
                {
                    { 1, 288 }
                }
            },
            {2, new Dictionary<int, long>()
                {
                    { 1, 71503 }
                }
            },
        };

        public long RunQuestion1(FileInfo file, bool isBenchmark = false)
        {
            var reader = new LineSplitFileReader();
            var stringSplit = reader.GetReadableFileContent(file, isBenchmark);

            var inputDict = ConvertInputToDict(stringSplit);

            int currDivider;
            int time;
            int distance;
            List<int> waysToWinList = new List<int>();
            foreach(KeyValuePair<int, int> pair in inputDict)
            {
                currDivider = 1;
                time = pair.Key;
                distance = pair.Value + 1;
                while (true)
                {
                    if (Math.Ceiling((double)distance / (double)currDivider) <= time - currDivider)
                    {
                        waysToWinList.Add(time - currDivider - currDivider + 1);
                        break;
                    }
                    currDivider++;
                }
            }
            long result = 1;
            foreach (var toWin in  waysToWinList)
            {
                result *= toWin;
            }
            return result;
        }

        private Dictionary<int, int> ConvertInputToDict(string[] strings)
        {
            var times = ConvertStringToInt(strings[0]);
            var distance = ConvertStringToInt(strings[1]);
            var result = new Dictionary<int, int>();
            for (int i = 0; i < times.Length; i++)
            {
                result.Add(times[i], distance[i]);
            }
            return result;
        }

        private int[] ConvertStringToInt(string str)
        {
            var temp = str.Split(" ");
            var result = new List<int>();
            foreach (var i in temp) 
            { 
                if (int.TryParse(i, out int value))
                    result.Add(value);
            }
            return result.ToArray();
        }

        public long RunQuestion2(FileInfo file, bool isBenchmark = false)
        {
            var reader = new LineSplitFileReader();
            var stringSplit = reader.GetReadableFileContent(file, isBenchmark);
            var input = ConvertStringsToUsable(stringSplit);

            int currDivider = 1;
            while (true)
            {
                if (Math.Ceiling((double)input.distance / (double)currDivider) <= input.time - currDivider)
                {
                    return (input.time - currDivider - currDivider + 1);
                }
                currDivider++;
            }
        }

        private (long time, long distance) ConvertStringsToUsable(string[] strings)
        {
            long time = ConvertStringToLong(strings[0]);
            long distance = ConvertStringToLong(strings[1]);
            return (time, distance);
        }

        private long ConvertStringToLong(string str)
        {
            var temp = str.Split(" ");
            var sb = new StringBuilder();
            foreach (var i in temp)
            {
                if (int.TryParse(i, out int value))
                {
                    sb.Append(value);
                }
            }
            return long.Parse(sb.ToString());
        }
    }
}
