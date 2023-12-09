using AdventOfCodeApp.Util.FileReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace AdventOfCodeApp.DayClasses
{
    internal class Aoc09DayLogic : IDayLogic
    {
        public Dictionary<int, Dictionary<int, long>> ExpectedTestResults => new Dictionary<int, Dictionary<int, long>>()
        {
            {1, new Dictionary<int, long>()
                {
                    { 1, 114 },
                }
            },
            {2, new Dictionary<int, long>()
                {
                    { 1, 2 }
                }
            },
        };

        public long RunQuestion1(FileInfo file, bool isBenchmark = false)
        {
            var reader = new LineSplitFileReader();
            string[] oberservationStrings = reader.GetReadableFileContent(file, isBenchmark);
            
            Observations currObs;
            long result = 0;
            foreach (string s in oberservationStrings)
            {
                currObs = new Observations(s);
                result += currObs.FinalExtrapolationQ1;

            }

            return result;
        }

        public long RunQuestion2(FileInfo file, bool isBenchmark = false)
        {
            var reader = new LineSplitFileReader();
            string[] oberservationStrings = reader.GetReadableFileContent(file, isBenchmark);

            Observations currObs;
            long result = 0;
            foreach (string s in oberservationStrings)
            {
                currObs = new Observations(s);
                result += currObs.FinalExtrapolationQ2;

            }

            return result;
        }

        private class Observations
        {
            public List<long> InitialObservations { get; set; }
            public List<List<long>> Extrapolations { get; set; } = new List<List<long>>();

            public long FinalExtrapolationQ1
            {
                get
                {
                    return FindFinalExtrapolationQ1();
                }
            }

            public long FinalExtrapolationQ2
            {
                get
                {
                    return FindFinalExtrapolationQ2();
                }
            }

            public Observations(string inputString)
            {
                InitialObservations = new List<long>();
                string[] splitString = inputString.Split(' ');
                foreach (string s in splitString)
                {
                    InitialObservations.Add(long.Parse(s));
                }

                List<long> currList = InitialObservations;
                List<long> newList;
                while (currList.Sum() != 0)
                {
                    newList = new List<long>();

                    for (int i = 0; i < currList.Count - 1; i++)
                    {
                        newList.Add(currList[i + 1] - currList[i]);
                    }
                    currList = newList;
                    Extrapolations.Add(currList);
                }
            }

            private long FindFinalExtrapolationQ1()
            {
                var prevExtra = Extrapolations[Extrapolations.Count - 1];
                List<long> currExtra = new List<long>();
                long newValue;


                for (int i = Extrapolations.Count - 2;  i >= 0; i--)
                {
                    currExtra = Extrapolations[i];
                    newValue = currExtra.Last() + prevExtra.Last();
                    currExtra.Add(newValue);
                    prevExtra = currExtra;
                }

                return InitialObservations.Last() + currExtra.Last();
            }

            private long FindFinalExtrapolationQ2()
            {
                var prevExtra = Extrapolations[Extrapolations.Count - 1];
                List<long> currExtra = new List<long>();
                long newValue;


                for (int i = Extrapolations.Count - 2; i >= 0; i--)
                {
                    currExtra = Extrapolations[i];
                    newValue = currExtra[0] - prevExtra[0];
                    currExtra.Insert(0, newValue);
                    prevExtra = currExtra;
                }

                return InitialObservations[0] - currExtra[0];
            }

        }
    }
}
