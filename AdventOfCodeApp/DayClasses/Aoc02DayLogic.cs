using AdventOfCodeApp.Util.FileReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AdventOfCodeApp.DayClasses
{
    internal class Aoc02DayLogic : IDayLogic
    {
        public Dictionary<int, Dictionary<int, long>> ExpectedTestResults => new Dictionary<int, Dictionary<int, long>>()
        {
            { 1, new Dictionary<int, long>()
                {
                    { 1, 8 },
                }
            },
            {
                2, new Dictionary<int, long>()
                {
                    {1, 2286 }
                }
            }
        };
        public long RunQuestion1(FileInfo file, bool isBenchmark = false) 
        {
            var splitFile = InputSplitter(file, isBenchmark);
            
            Day2Game game;
            long result = 0;
            foreach (var line in splitFile)
            {
                game = new Day2Game(line);
                if (game.IsWithinRules(12, 13, 14))
                    result += game.GameId;
            }
            return result;
        }

        public long RunQuestion2(FileInfo file, bool isBenchmark = false)
        {
            var splitFile = InputSplitter(file, isBenchmark);

            Day2Game game;
            long result = 0;
            foreach (var line in splitFile)
            {
                game = new Day2Game(line);
                result += game.MaxBlueCubes * game.MaxGreenCubes * game.MaxRedCubes;

            }
            return result;
        }

        private string[] InputSplitter(FileInfo file, bool isBenchmark)
        {
            var lineReader = new LineSplitFileReader();
            string[] output = lineReader.GetReadableFileContent(file, isBenchmark);

            return output;
        }
    }

    internal class Day2Game
    {
        private static Regex GameIdRegex = new Regex(@"Game (\d+):(.*)");
        private static Regex GameRegex = new Regex(@"(?: ?([^;]*);?)*");
        private static Regex CubeRegex = new Regex(@"((\d*) (blue|red|green)),?");

        public int GameId { get; set; }
        public int MaxRedCubes { get; set; } = 0;
        public int MaxGreenCubes { get;set;} = 0;
        public int MaxBlueCubes { get; set; } = 0;

        public Day2Game(string inputString)
        {
            SetPropertiesFromInputString(inputString);
        }

        public bool IsWithinRules(int maxReds, int maxGreens, int maxBlues)
        {
            return MaxRedCubes <= maxReds && MaxGreenCubes <= maxGreens && MaxBlueCubes <= maxBlues;
        }

        private void SetPropertiesFromInputString(string inputString)
        {
            MatchCollection matches = GameIdRegex.Matches(inputString);
            GroupCollection groups = matches[0].Groups;
            GameId = int.Parse(groups[1].Value);
            SetCubesFromAllPulls(groups[2].Value);
        }

        private void SetCubesFromAllPulls(string groupMatch)
        {
            MatchCollection matches = GameRegex.Matches(groupMatch);
            GroupCollection groups = matches[0].Groups;
            foreach (Group group in groups)
            {
                SetCubesFromSinglePull(group.Value);
            }

        }

        private void SetCubesFromSinglePull(string cubePull)
        {
            MatchCollection matches = CubeRegex.Matches(cubePull);
            GroupCollection groups;
            foreach (Match match in matches)
            {
                groups = match.Groups;
                SetCubeProperty(int.Parse(groups[2].Value), groups[3].Value);
            }
        }

        private void SetCubeProperty(int amount, string color)
        {
            switch (color)
            {
                case "green":
                    MaxGreenCubes = MaxGreenCubes > amount ? MaxGreenCubes : amount; 
                    break;
                case "blue":
                    MaxBlueCubes = MaxBlueCubes > amount ? MaxBlueCubes : amount;
                    break;
                case "red":
                    MaxRedCubes = MaxRedCubes > amount ? MaxRedCubes : amount;
                    break;
            }
        }
    }
}
