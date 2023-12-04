using AdventOfCodeApp.Util.FileReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeApp.DayClasses
{
    internal class Aoc04DayLogic : IDayLogic
    {
        public Dictionary<int, Dictionary<int, long>> ExpectedTestResults => new Dictionary<int, Dictionary<int, long>>()
        {
            {1, new Dictionary<int, long>()
                {
                    { 1, 13 }
                }
            },
            {2, new Dictionary<int, long>
                { 
                    { 1, 30 } 
                } 
            },
        };

        public long RunQuestion1(FileInfo file, bool isBenchmark = false)
        {
            long result = 0;
            long cardResult;
            string[] splitLine;
            var reader = new LineSplitFileReader();
            string[] cards = reader.GetReadableFileContent(file);
            HashSet<int> winningNumbers;
            HashSet<int> numbers;

            foreach (var card in cards)
            {
                cardResult = 0;
                splitLine = card.Split(" | ");
                numbers = SplitCardNumbers(splitLine[1]);
                winningNumbers = SplitCardNumbers(splitLine[0].Split(":")[1]);
                foreach (var number in numbers) 
                {
                    if (winningNumbers.Contains(number))
                    {
                        if (cardResult == 0)
                            cardResult = 1;
                        else
                            cardResult *= 2;
                    }

                }
                result += cardResult;
            }
            return result;
        }

        private HashSet<int> SplitCardNumbers(string numberLine)
        {
            var result = new HashSet<int>();
            string[] splitNumbers = numberLine.Split(" ");
            int currNumb;
            foreach (var number in splitNumbers)
            {
                if (int.TryParse(number, out currNumb))
                {
                    result.Add(currNumb);
                }
            }
            return result;
        }

        public long RunQuestion2(FileInfo file, bool isBenchmark = false)
        {
            long result = 0;
            long cardResult;
            string[] splitLine;
            var reader = new LineSplitFileReader();
            string[] cards = reader.GetReadableFileContent(file);
            HashSet<int> winningNumbers;
            HashSet<int> numbers;
            var cardDict = new Dictionary<int, long>();
            string card;
            long multiplier;
            for (int i = 1; i <= cards.Length; i++)
            {
                cardDict[i] = 1;
            }

            for (int i = 0; i < cards.Length; i++)
            {
                cardResult = 0;
                card = cards[i];
                
                splitLine = card.Split(" | ");
                numbers = SplitCardNumbers(splitLine[1]);
                winningNumbers = SplitCardNumbers(splitLine[0].Split(":")[1]);
                foreach (var number in numbers)
                {
                    if (winningNumbers.Contains(number))
                    {
                        cardResult++;
                    }
                }
                multiplier = cardDict[i + 1];

                for (int j = 2; j <= cardResult + 1; j++)
                {
                    cardDict[i+j] += multiplier;
                }
            }

            foreach (var amountCards in cardDict.Values)
            {
                result += amountCards;
            }


            return result;
        }
    }
}
