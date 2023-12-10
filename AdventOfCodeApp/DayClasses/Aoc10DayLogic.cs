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
            var reader = new CharMultiArrayFileReader();
            var pipes = reader.GetReadableFileContent(file, isBenchmark);

            long totalPositions = pipes.LongLength;

            PositionQ2 lastPos = new(FindStartingPosition(pipes));
            PositionQ2 currPos = new(FindNextPositionFromStart(pipes, lastPos.Position));
            PositionQ2 tempPos;

            

            lastPos.SetNodeOut(currPos);

            HashSet<PositionQ2> loop = new HashSet<PositionQ2>
            {
                lastPos
            };
            char currPipe = pipes[currPos.Y, currPos.X];

            while (currPipe != 'S')
            {
                tempPos = new(GetNextPosition(currPipe, currPos.Position, lastPos.Position));
                lastPos = currPos;
                currPos = tempPos;
                lastPos.SetNodeOut(currPos);
                loop.Add(currPos);
                currPipe = pipes[currPos.Y, currPos.X];
            }

            var activeNonLoopTiles = FindNonLoopEdges(loop, pipes);
            HashSet<Position> inactiveTiles = new HashSet<Position>();
            HashSet<Position> tempActiveTiles;


            while (activeNonLoopTiles.Count > 0)
            {
                tempActiveTiles = new HashSet<Position>();
                foreach ( var tile in activeNonLoopTiles)
                {

                }
            }


            return 0;
        }

        private Position TravelBetweenPipes(PositionQ2 Start)

        private HashSet<Position> GetPositionNeibours(Position position, HashSet<Position> inactiveTiles, HashSet<Position> activeTiles)
        {
            Position newPos;
            HashSet<Position> result = new HashSet<Position>();

            for (int y = position.Y - 1; y >= position.Y + 1; y++)
            {
                newPos = new Position() { Y = y, X = position.X };
                if (activeTiles.Contains(newPos) || inactiveTiles.Contains(newPos))
                    continue;
                result.Add(newPos);
            }
            for (int x = position.X - 1; x >= position.X + 1; x++)
            {
                newPos = new Position() { X = x, Y = position.Y };
                if (activeTiles.Contains(newPos) || inactiveTiles.Contains(newPos))
                    continue;
                result.Add(newPos);
            }
            return result;
        }

        private HashSet<Position> FindNonLoopEdges(HashSet<PositionQ2> loop, char[,] pipes) 
        {
            PositionQ2 tempPos1;
            PositionQ2 tempPos2;
            HashSet<Position> result = new HashSet<Position>();

            for (int x = 0; x < pipes.GetLength(1); x++)
            {
                tempPos1 = new PositionQ2(0, x);
                tempPos2 = new PositionQ2(pipes.GetLength(0) - 1, x);
                if (!loop.Contains(tempPos1))
                    result.Add(tempPos1.Position);
                if (!loop.Contains(tempPos2))
                    result.Add(tempPos2.Position);
            }

            for (int y = 0;  y < pipes.GetLength(0); y++)
            {
                tempPos1 = new PositionQ2(y, 0);
                tempPos2 = new PositionQ2(y, pipes.GetLength(1) - 1);
                if (!loop.Contains(tempPos1))
                    result.Add(tempPos1.Position);
                if (!loop.Contains(tempPos2))
                    result.Add(tempPos2.Position);
            }
            return result;
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

        private class PositionQ2 : IEquatable<PositionQ2>, IEquatable<Position>
        {
            private Position _position;

            public int X => _position.X;
            public int Y => _position.Y;

            public Position Position => _position;

            public PositionQ2? NodeIn { get; set; }
            public PositionQ2? NodeOut { get; private set; }

            public PositionQ2(Position position)
            {
                _position = position;
            }

            public PositionQ2(int y, int x)
            {
                _position = new Position() { Y = y, X = x };
            }

            public void SetNodeOut (PositionQ2 node)
            {
                NodeOut = node;
                node.NodeIn = this;
            }

            public bool Equals(PositionQ2? other)
            {
                return other != null && other.X == X && other.Y == Y;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(X, Y);
            }

            public bool Equals(Position? other)
            {
                return other != null && other.X == X && other.Y == Y;
            }
        }
    }
}
