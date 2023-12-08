using AdventOfCodeApp.Util.FileReaders;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeApp.DayClasses
{
    internal class Aoc08DayLogic : IDayLogic
    {
        public Dictionary<int, Dictionary<int, long>> ExpectedTestResults => new Dictionary<int, Dictionary<int, long>>()
        {
            {1, new Dictionary<int, long>()
                {
                    { 1, 2 },
                    { 2, 6 }
                }
            },
            {2, new Dictionary<int, long>()
                {
                    { 1, 6 }
                }
            },
        };

        public long RunQuestion1(FileInfo file, bool isBenchmark = false)
        {
            var reader = new LineSplitFileReader();
            var lines = reader.GetReadableFileContent(file, isBenchmark);

            char[] instructions = lines[0].ToCharArray();
            // Starting Node
            DirectionNodeQ1 currNode = new DirectionNodeQ1(lines[2]);

            HashSet<DirectionNodeQ1> nodes = CreateNodes(lines);
            nodes.Add(currNode);

            foreach (var node in nodes)
            {
                node.SetNodes(nodes);
            }

            int instructionIndex = 0;
            long stepCounter = 0;
            while (!currNode.Equals("ZZZ"))
            {
                if (instructionIndex == instructions.Length)
                    instructionIndex = 0;

                currNode = currNode.GetNextNode(instructions[instructionIndex]);

                stepCounter++;
                instructionIndex++;
            }

            return stepCounter;
        }

        private HashSet<DirectionNodeQ1> CreateNodes(string[] inputs)
        {
            HashSet<DirectionNodeQ1> nodes = new HashSet<DirectionNodeQ1>();

            for (int i = 3; i < inputs.Length; i++)
            {
                nodes.Add(new DirectionNodeQ1(inputs[i]));
            }


            return nodes;
        }

        public long RunQuestion2(FileInfo file, bool isBenchmark = false)
        {
            var reader = new LineSplitFileReader();
            var lines = reader.GetReadableFileContent(file, isBenchmark);

            char[] instructions = lines[0].ToCharArray();
            HashSet<DirectionNodeQ2> currNodes = new HashSet<DirectionNodeQ2>();
            HashSet<DirectionNodeQ2> nodes = new HashSet<DirectionNodeQ2>();
            DirectionNodeQ2 newNode;
            for (int i = 2; i < lines.Length; i++)
            {
                newNode = new DirectionNodeQ2(lines[i]);
                nodes.Add(newNode);
                if (newNode.IsStartingNode) 
                    currNodes.Add(newNode);
            }
            foreach (var node in nodes) 
            {
                node.SetNodes(nodes);
            }

            bool isEnd = false;
            long stepCounter = 0;
            int instructionIndex = 0;
            HashSet<DirectionNodeQ2> tempNodes = new HashSet<DirectionNodeQ2>();
            List<long> stepsToFinish = new List<long>();
            DirectionNodeQ2 currNode;
            foreach (var node in currNodes)
            {
                stepCounter = 0;
                instructionIndex = 0;
                currNode = node;
                while (!currNode.IsEndNode)
                {
                    if (instructionIndex == instructions.Length)
                        instructionIndex = 0;

                    currNode = currNode.GetNextNode(instructions[instructionIndex]);

                    stepCounter++;
                    instructionIndex++;
                }
                stepsToFinish.Add(stepCounter);
            }
            long result = 1;
            Dictionary<int, int> primeDict = GetLowestCommonMultipleDict(stepsToFinish);

            foreach(var prime in primeDict)
            {
                result *= (long)Math.Pow(prime.Key, prime.Value);
            }


            return result;
        }

        private Dictionary<int, int> GetLowestCommonMultipleDict(List<long> stepsToFinish)
        {
            var primeDict = new Dictionary<int, int>();
            Dictionary<int, int> temp;
            foreach (var step in stepsToFinish)
            {
                temp = GetPrimeFactors(step);
                foreach (var factor in temp)
                {
                    if (!primeDict.ContainsKey(factor.Key))
                    {
                        primeDict[factor.Key] = factor.Value;
                    }
                    else if (primeDict[factor.Key] < factor.Value)
                    {
                        primeDict[factor.Key] = factor.Value;
                    }
                }
            }
            return primeDict;
        }

        private Dictionary<int, int> GetPrimeFactors(long steps)
        {
            var result = new Dictionary<int, int>();
            long currNumber = steps;
            for (int i = 2; i <= steps; i++)
            {
                while(currNumber % i == 0)
                {
                    if (!result.ContainsKey(i))
                    {
                        result[i] = 0;
                        currNumber = currNumber / i;
                    }
                    result[i]++;
                }
            }
            return result;
        }

        private class DirectionNodeQ1 : IEquatable<string>, IEquatable<DirectionNodeQ1>
        {
            public string Name { get; set; }
            public DirectionNodeQ1? Right { get; set; }
            public DirectionNodeQ1? Left { get; set; }

            public string RightName { get; set; } = string.Empty;
            public string LeftName { get; set; } = string.Empty;

            public DirectionNodeQ1(string inputString)
            {
                var splitString = inputString.Split(" = ");
                Name = splitString[0];
                SetNodeNames(splitString[1]);
            }

            public DirectionNodeQ1(string name, bool tempNode)
            {
                Name = name;
            }

            public void SetNodes(HashSet<DirectionNodeQ1> nodes)
            {
                DirectionNodeQ1 tempNode = new DirectionNodeQ1(LeftName, true);
                DirectionNodeQ1? actual;
                Left = nodes.TryGetValue(tempNode, out actual) ? actual : null;
                tempNode = new DirectionNodeQ1(RightName, true);
                Right = nodes.TryGetValue(tempNode, out actual) ? actual : null;
            }

            private void SetNodeNames(string input)
            {
                var split = input.Split(", ");
                LeftName = split[0].Substring(1); // "(ABC"
                RightName = split[1].Substring(0, 3); // "ABC)
            }

            public DirectionNodeQ1 GetNextNode(char instruction)
            {
                if (instruction == 'L')
                    return Left;
                else
                    return Right;
            }

            public bool Equals(DirectionNodeQ1? other)
            {
                return other != null && other.Name == Name;
            }

            public bool Equals(string? other)
            {
                return other != string.Empty && other != null && other == Name;
            }

            public override int GetHashCode()
            {
                return Name.GetHashCode();
            }
        }

        private class DirectionNodeQ2 : IEquatable<string>, IEquatable<DirectionNodeQ2>
        {
            public string Name { get; set; }
            public DirectionNodeQ2? Right { get; set; }
            public DirectionNodeQ2? Left { get; set; }

            public string RightName { get; set; } = string.Empty;
            public string LeftName { get; set; } = string.Empty;

            public bool IsEndNode => Name[2] == 'Z';
            public bool IsStartingNode => Name[2] == 'A';

            public DirectionNodeQ2(string inputString)
            {
                var splitString = inputString.Split(" = ");
                Name = splitString[0];
                SetNodeNames(splitString[1]);
            }

            public DirectionNodeQ2(string name, bool tempNode)
            {
                Name = name;
            }

            public void SetNodes(HashSet<DirectionNodeQ2> nodes)
            {
                DirectionNodeQ2 tempNode = new DirectionNodeQ2(LeftName, true);
                DirectionNodeQ2? actual;
                Left = nodes.TryGetValue(tempNode, out actual) ? actual : null;
                tempNode = new DirectionNodeQ2(RightName, true);
                Right = nodes.TryGetValue(tempNode, out actual) ? actual : null;
            }

            private void SetNodeNames(string input)
            {
                var split = input.Split(", ");
                LeftName = split[0].Substring(1); // "(ABC"
                RightName = split[1].Substring(0, 3); // "ABC)
            }

            public DirectionNodeQ2 GetNextNode(char instruction)
            {
                if (instruction == 'L')
                    return Left;
                else
                    return Right;
            }

            public bool Equals(DirectionNodeQ2? other)
            {
                return other != null && other.Name == Name;
            }

            public bool Equals(string? other)
            {
                return other != string.Empty && other != null && other == Name;
            }

            public override int GetHashCode()
            {
                return Name.GetHashCode();
            }
        }
    }
}
