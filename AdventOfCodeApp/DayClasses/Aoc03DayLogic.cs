using AdventOfCodeApp.Util.FileReaders;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeApp.DayClasses
{
    internal class Aoc03DayLogic : IDayLogic
    {
        public Dictionary<int, Dictionary<int, long>> ExpectedTestResults => new Dictionary<int, Dictionary<int, long>>()
        {
            {1, new Dictionary<int, long>()
                {
                    { 1, 4361 },
                }
            },
            {2, new Dictionary<int, long>()
                {
                    { 1, 467_835 },
                }
            },
        };

        private HashSet<string> NonSymbols = new HashSet<string>()
        {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            ".",
        };

        public long RunQuestion1(FileInfo file, bool isBenchmark = false)
        {
            var reader = new StringArrayFileReader();
            string[,] strings = reader.GetReadableFileContent(file, isBenchmark);
            int lengthX = strings.GetLength(0);
            var lengthY = strings.GetLength(1);
            HashSet<Number> numbers = new HashSet<Number>();

            for (int y = 0; y < lengthX; y++)
            {
                for (int x = 0; x < lengthY; x++) 
                { 
                    if (!NonSymbols.TryGetValue(strings[y,x], out string? _))
                    {
                        AddNumbersFromSymbolCoordinates(y, x, strings, numbers);
                    }
                }
            }

            long result = 0;
            foreach (var number in numbers)
            {
                result += number.Value;
            }

            return result;
        }

        public long RunQuestion2(FileInfo file, bool isBenchmark = false)
        {
            var reader = new StringArrayFileReader();
            string[,] strings = reader.GetReadableFileContent(file, isBenchmark);
            int lengthX = strings.GetLength(0);
            var lengthY = strings.GetLength(1);
            long result = 0;

            for (int y = 0; y < lengthX; y++)
            {
                for (int x = 0; x < lengthY; x++)
                {
                    if (strings[y, x] == "*")
                    {
                        result += AddGearFromCoordinate(y, x, strings).GearRatio;
                    }
                }
            }
            return result;
        }

        private Gear AddGearFromCoordinate(int y, int x, string[,] strings)
        {
            int lowerY = y - 1 >= 0 ? y - 1 : 0;
            int upperY = y + 1 < strings.GetLength(0) ? y + 1 : y;
            int lowerX = x - 1 >= 0 ? x - 1 : 0;
            int upperX = x + 1 < strings.GetLength(1) ? x + 1 : x;
            Gear gear = new Gear();

            for (int i = lowerY; i <= upperY; i++)
            {
                for (int j = lowerX; j <= upperX; j++)
                {
                    if (i == y && j == x) continue;

                    if (Int32.TryParse(strings[i, j], out int _))
                    {
                        gear.Numbers.Add(CreateNumberFromFoundCoordinates(i, j, strings));
                    }
                }
            }


            return gear;

        }

        private void AddNumbersFromSymbolCoordinates(int y, int x, string[,] strings, HashSet<Number> numbers)
        {
            int lowerY = y - 1 >= 0 ? y - 1 : 0;
            int upperY = y + 1 < strings.GetLength(0) ? y + 1 : y;
            int lowerX = x - 1 >= 0 ? x - 1 : 0;
            int upperX = x + 1 < strings.GetLength(1) ? x + 1 : x;


            for (int i = lowerY; i <= upperY; i++)
            {
                for (int j = lowerX; j <= upperX; j++)
                {
                    if (i == y && j == x) continue;

                    if (Int32.TryParse(strings[i,j], out int _))
                    {
                        numbers.Add(CreateNumberFromFoundCoordinates(i, j, strings));
                    }
                }
            }
        }

        private Number CreateNumberFromFoundCoordinates(int y, int x, string[,] strings)
        {
            int currX = x;

            if (currX == 0)
                return new Number(strings, x, y);

            while (int.TryParse(strings[y, currX], out int _))
            {
                if (currX == 0)
                    return new Number(strings, currX, y);
                currX--;
            }

            return new Number(strings, currX + 1, y);
        }

        private class SymbolCoordinate : IEquatable<SymbolCoordinate>
        {
            public int X { get; set; }
            public int Y { get; set; }

            public bool Equals(SymbolCoordinate? other)
            {
                return other != null && other.X == X && other.Y == Y;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(X, Y);
            }
        }

        private class Number : IEquatable<Number>
        {
            public int Value { get; set; }
            public SymbolCoordinate StartCoordinate { get; set; }

            public Number(string[,] strings, int startX, int startY)
            {
                StartCoordinate = new SymbolCoordinate() { X = startX, Y = startY };
                StringBuilder sb = new StringBuilder();
                string currChar = strings[startY, startX];
                int currX = startX;
                while (int.TryParse(currChar, out int number))
                {
                    sb.Append(number);
                    currX++;
                    try
                    {
                        currChar = strings[startY, currX];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        currChar = ".";
                    }
                }
                Value = int.Parse(sb.ToString());
            }

            public bool Equals(Number? other)
            {
                return other != null && other.Value == Value && other.StartCoordinate.Equals(StartCoordinate);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Value, StartCoordinate.GetHashCode());
            }
        }

        private class Gear
        {
            public HashSet<Number> Numbers { get; set; } = new HashSet<Number>();
            public bool IsValidGear => Numbers.Count == 2;

            public long GearRatio
            {
                get
                {
                    if (!IsValidGear)
                        return 0;

                    long result = 1;
                    foreach (Number number in Numbers)
                        result *= number.Value;
                    return result;
                }
            }
        }

        /*
          Question 1:
            First run, no optimization (1000 runs)
                Benchmark in ticks:
                First Run Time: 359.552
                Last Run Time: 1.534.775
                Average: 577.600,306
                Median: 533.149
                Max Time: 1.534.775
                Min Time: 359.552
            
         */
    }
}
