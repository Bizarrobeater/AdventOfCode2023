using AdventOfCodeApp.Util;
using AdventOfCodeApp.Util.FileReaders;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeApp.DayClasses
{
    internal class Aoc05DayLogic : IDayLogic
    {
        public Dictionary<int, Dictionary<int, long>> ExpectedTestResults => new Dictionary<int, Dictionary<int, long>>()
        {
            {1, new Dictionary<int, long>()
                {
                    { 1, 35 }
                }
            },
            {2, new Dictionary<int, long>()
                {
                    { 1, 46 }
                }
            },
        };

        public long RunQuestion1(FileInfo file, bool isBenchmark = false)
        {
            var reader = new Aoc05FileReader();
            var splitContent = reader.GetReadableFileContent(file);
            var seeds = SplitInitialSeedString(splitContent[0]);
            var currValues = seeds;
            Mapper currMapper;
            for (int i = 1; i < splitContent.Length; i++)
            {
                currMapper = new Mapper(splitContent[i]);
                for (int j = 0; j < currValues.Length; j++)
                {
                    currValues[j] = currMapper.GetDestinationValue(currValues[j]);
                }
            }

            long result = long.MaxValue;
            foreach (var value in currValues) 
            {
                if (value < result)
                {
                    result = value;
                }
            }
            return result;
        }

        private long[] SplitInitialSeedString(string seedString)
        {
            string onlySeeds = seedString.Substring(7);
            string[] splitSeeds = onlySeeds.Split(' ');
            long[] result = new long[splitSeeds.Length];
            for (int i = 0; i < splitSeeds.Length; i++)
            {
                result[i] = long.Parse(splitSeeds[i]);
            }
            return result;
        }

        public long RunQuestion2(FileInfo file, bool isBenchmark = false)
        {
            var reader = new Aoc05FileReader();
            var splitContent = reader.GetReadableFileContent(file);
            var seeds = SplitInitialSeedStringQ2(splitContent[0]);
            var currValues = seeds;
            List<SeedRange> newRanges;
            Mapper mapper;
            for (int i = 1; i < splitContent.Length; i++)
            {
                newRanges = new List<SeedRange>();
                mapper = new Mapper(splitContent[i]);
                for (int j = 0; j < currValues.Count; j++)
                {
                    newRanges.AddRange(
                        mapper.GetDestinationRangesFromSource(currValues[j])
                        );
                }
                currValues = newRanges;
            }

            long result = long.MaxValue;
            foreach (var value in currValues)
            {
                if (value.Start < result)
                {
                    result = value.Start;
                }
            }
            return result;
        }

        private List<SeedRange> SplitInitialSeedStringQ2(string seedString)
        {
            List<SeedRange> result = new List<SeedRange>();
            string onlySeeds = seedString.Substring(7);
            string[] splitSeeds = onlySeeds.Split(' ');
            long currInit;
            long range;
            for (int i = 0; i < splitSeeds.Length; i += 2) 
            {
                currInit = long.Parse(splitSeeds[i]);
                range = long.Parse(splitSeeds[i + 1]);
                result.Add(new SeedRange(currInit, range));
            }
            return result;
        }

        private class Mapper
        {
            public string MapperName { get; set; }

            public List<Range> Ranges { get; set; } = new List<Range>();
            public Mapper(string mapperContent)
            {
                string[] splitContent = mapperContent.Split(Environment.NewLine);
                MapperName = splitContent[0];
                string currRange;
                for ( int i = 1; i < splitContent.Length; i++ )
                {
                    currRange = splitContent[i];
                    Ranges.Add(new Range(currRange));
                }
                Ranges.Sort();
            }

            public List<SeedRange> GetDestinationRangesFromSource(SeedRange source)
            {
                var result = new List<SeedRange>();
                Aoc05OverlapEnum overlap;
                long diff;
                foreach (var range in Ranges)
                {
                    overlap = range.GetOverlapType(source);
                    switch (overlap)
                    {
                        case Aoc05OverlapEnum.Below:
                            result.Add(source);
                            return result;
                        case Aoc05OverlapEnum.Outer:
                            result.Add(new SeedRange(range.DestinationStart, range.RangeLength));
                            result.Add(new SeedRange(source.Start, range.SourceStart - source.Start - 1));
                            source.Start = range.SourceEnd + 1;
                            source.Length = source.Length - range.RangeLength - range.SourceStart - source.Start - 1;
                            break;
                        case Aoc05OverlapEnum.Inner:
                            diff = source.Start - range.SourceStart;
                            result.Add(new SeedRange(range.DestinationStart + diff, source.Length));
                            return result;
                        case Aoc05OverlapEnum.StartOut:
                            diff = source.End - range.SourceStart;
                            result.Add(new SeedRange(source.Start, range.SourceStart - source.Start - 1));
                            result.Add(new SeedRange(range.DestinationStart, diff));
                            return result;
                        case Aoc05OverlapEnum.EndOut:
                            diff = range.SourceEnd - source.Start;
                            result.Add(new SeedRange(range.DestinationEnd - diff, diff));
                            source.Start += diff;
                            source.Length -= diff;
                            break;
                        case Aoc05OverlapEnum.Above:
                            continue;
                    }
                }
                result.Add(source);
                return result;
            }

            public long GetDestinationValue(long source)
            {
                if (BinarySearch(source, out Range? foundRange))
                    return foundRange != null ? foundRange.GetDestinationFromSource(source) : -1;
                else
                    return source;
            }

            private bool BinarySearch(long source, out Range? foundRange)
            {
                int min = 0;
                int max = Ranges.Count - 1;

                int curr;
                int compareResult;
                while (min <= max)
                {
                    curr = min + (max - min) / 2;
                    foundRange = Ranges[curr];
                    compareResult = foundRange.CompareTo(source);
                    if (compareResult == 0)
                        return true;
                    else if (compareResult == 1)
                        min = curr + 1;
                    else
                        max = curr - 1;
                }
                foundRange = null;
                return false;
            }
        }

        private class SeedRange
        {
            public long Start { get; set; }
            public long Length { get; set; }
            public long End => Start + Length;

            public SeedRange(long start, long length)
            {
                Start = start;
                Length = length;
            }
        }

        private class Range : IComparable<Range>, IComparable<long>, IEquatable<Range>, IEquatable<long>
        {
            public long SourceStart { get; set; }
            public long DestinationStart { get; set; }
            public long RangeLength { get; set; }
            public long SourceEnd => SourceStart + RangeLength;
            public long DestinationEnd => DestinationStart + RangeLength;
            public Range(string rangeContent)
            {
                var splitContent = rangeContent.Split(' ');
                SourceStart = long.Parse(splitContent[1]);
                DestinationStart = long.Parse(splitContent[0]);
                RangeLength = long.Parse(splitContent[2]);
            }

            public long GetDestinationFromSource(long source)
            {
                if (CompareTo(source) != 0) return -1;

                long diff = source - SourceStart;
                return DestinationStart + diff;
            }

            public Aoc05OverlapEnum GetOverlapType(SeedRange range)
            {
                if (range.End < SourceStart)
                    return Aoc05OverlapEnum.Below;
                if (range.Start < SourceStart && range.End > SourceEnd)
                    return Aoc05OverlapEnum.Outer;
                if (range.Start >= SourceStart && range.End <= SourceEnd)
                    return Aoc05OverlapEnum.Inner;
                if (range.Start >= SourceStart && range.Start <= SourceEnd && range.End > SourceEnd)
                    return Aoc05OverlapEnum.EndOut;
                if (range.Start < SourceStart && range.End <= SourceEnd)
                    return Aoc05OverlapEnum.StartOut;
                if (range.Start > SourceEnd)
                    return Aoc05OverlapEnum.Above;
                return Aoc05OverlapEnum.None;
            }

            public int CompareTo(long other)
            {
                if (other < SourceStart) return -1;
                else if (other >= SourceStart && other < SourceEnd) return 0;
                else return 1;
            }

            public int CompareTo(Range? other)
            {
                if (other == null) return 1;

                return SourceStart.CompareTo(other.SourceStart);
            }

            public bool Equals(Range? other)
            {
                return other != null && SourceStart == other.SourceStart && DestinationStart == other.DestinationStart && RangeLength == other.RangeLength;
            }

            public bool Equals(long other)
            {
                return CompareTo(other) == 0;
            }
        }
    }
}
