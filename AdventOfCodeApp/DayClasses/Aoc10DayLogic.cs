using AdventOfCodeApp.Util.FileReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeApp.DayClasses
{
    internal class Aoc10DayLogic : IDayLogic
    {
        public Dictionary<int, Dictionary<int, long>> ExpectedTestResults => new Dictionary<int, Dictionary<int, long>>()
        {
            {1, new Dictionary<int, long>()
                {
                    { 1, 4 },
                    { 2, 8 }
                }
            },
            {2, new Dictionary<int, long>()
                {
                    { 1, 4 },
                    { 2, 8 },
                    { 3, 10 },
                }
            },
        };

        public long RunQuestion1(FileInfo file, bool isBenchmark = false)
        {
            var reader = new CharMultiArrayFileReader();
            var pipes = reader.GetReadableFileContent(file, isBenchmark);

            
            Position lastPos = FindStartingPosition(pipes);
            // I know this from looking at the input, couldn't figure out how to do it
            Position currPos = FindNextPositionFromStart(pipes, lastPos);
            Position tempPos;
            char currPipe = pipes[currPos.Y, currPos.X];
            long stepCounter = 0;

            while (currPipe != 'S')
            {
                stepCounter++;
                tempPos = GetNextPosition(currPipe, currPos, lastPos);
                lastPos = currPos;
                currPos = tempPos;
                currPipe = pipes[currPos.Y, currPos.X];
            }

            return (stepCounter + 1) / 2;
        }

        private Position FindNextPositionFromStart(char[,] pipes, Position start)
        {
            char currPipe;
            for (int y = start.Y - 1; y <= start.Y + 1; y++)
            {
                if (y < 0 || y >= pipes.GetLength(0) || y == start.Y)
                    continue;
                currPipe = pipes[y, start.X];

                if (currPipe == '|'
                    || (y > start.Y && currPipe == 'J')
                    || (y > start.Y && currPipe == 'L')
                    || (y < start.Y && currPipe == '7')
                    || (y < start.Y && currPipe == 'F')
                    )
                    return new Position() { Y = y, X = start.X };
                    //return GetNextPosition(currPipe, new Position() { Y = y, X = start.X }, start);
            }

            for (int x = start.X - 1; x <= start.X + 1; x++)
            {
                if (x < 0 || x >= pipes.GetLength(1) || x == start.X)
                    continue;
                currPipe = pipes[start.Y, x];

                if (currPipe == '-'
                    || (x < start.X && (currPipe == 'F' || currPipe == 'L'))
                    || (x > start.X && (currPipe == '7' || currPipe == 'J'))
                    )
                    return new Position() { Y = start.Y, X = x };
                    //return GetNextPosition(currPipe, new Position() { Y = start.Y, X = x }, start);
            }

            return new Position() { Y = -1, X = -1 };
        }

        private Position FindStartingPosition(char[,] pipes)
        {
            for (int y = 0; y < pipes.GetLength(0); y++)
            {
                for (int x = 0; x < pipes.GetLength(1); x++)
                {
                    if (pipes[y, x] == 'S')
                        return new Position() { Y = y, X = x };
                }
            }
            return new Position() { Y = -1, X = -1 };
        }



        public long RunQuestion2(FileInfo file, bool isBenchmark = false)
        {
            throw new NotImplementedException();
        }

        private Position GetNextPosition(char currPipe, Position currPosition, Position lastPosition)
        {
            switch (currPipe)
            {
                case '|':

                    if (lastPosition.Y < currPosition.Y)
                        return new Position() { Y = currPosition.Y + 1, X = currPosition.X  };
                    else
                        return new Position() { Y = currPosition.Y - 1, X = currPosition.X };
                case '-':
                    if (lastPosition.X < currPosition.X)
                        return new Position() { Y = currPosition.Y, X = currPosition.X + 1 };
                    else 
                        return new Position() { Y = currPosition.Y, X = currPosition.X - 1 };
                case 'L':
                    if (lastPosition.Y < currPosition.Y)
                        return new Position() { Y = currPosition.Y, X = currPosition.X + 1 };
                    else
                        return new Position() { Y = currPosition.Y - 1, X = currPosition.X };
                case 'J':
                    if (lastPosition.Y < currPosition.Y)
                        return new Position() { Y = currPosition.Y, X = currPosition.X - 1 };
                    else
                        return new Position() { Y = currPosition.Y - 1, X = currPosition.X };
                case '7':
                    if (lastPosition.Y == currPosition.Y)
                        return new Position() { Y = currPosition.Y + 1, X = currPosition.X };
                    else
                        return new Position() { Y = currPosition.Y, X = currPosition.X - 1 };
                case 'F':
                    if (lastPosition.Y == currPosition.Y)
                        return new Position() { Y = currPosition.Y + 1, X = currPosition.X };
                    else
                        return new Position() { Y = currPosition.Y, X = currPosition.X + 1 };
                case '.':
                    throw new Exception("Hit ground");
                default:
                    return new Position() { Y = -1, X = -1 };
            }
        }

        private record Position
        {
            public int X { get; set; }
            public int Y { get; set; }
        }
    }
}
