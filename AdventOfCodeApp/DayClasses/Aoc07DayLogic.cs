using AdventOfCodeApp.Util.FileReaders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeApp.DayClasses
{
    internal class Aoc07DayLogic : IDayLogic
    {
        public Dictionary<int, Dictionary<int, long>> ExpectedTestResults => new Dictionary<int, Dictionary<int, long>>()
        {
            {1, new Dictionary<int, long>()
                {
                    { 1, 6440 }
                }
            },
            {2, new Dictionary<int, long>()
                {
                    { 1, 5905 }
                }
            },
        };
        public long RunQuestion1(FileInfo file, bool isBenchmark = false)
        {
            var reader = new LineSplitFileReader();
            string[] cardStrings = reader.GetReadableFileContent(file, isBenchmark);

            List<CardHand> cards = new List<CardHand>();
            foreach (var card in cardStrings)
            {
                cards.Add(new CardHand(card));
            }
            cards.Sort();
            long result = 0;
            for (int i = 0; i < cards.Count; i++)
            {
                result += cards[i].Bid * (i + 1);
            }
            return result;
        }

        public long RunQuestion2(FileInfo file, bool isBenchmark = false)
        {
            var reader = new LineSplitFileReader();
            string[] cardStrings = reader.GetReadableFileContent(file, isBenchmark);

            List<CardHand> cards = new List<CardHand>();
            foreach (var card in cardStrings)
            {
                cards.Add(new CardHand(card, true));
            }
            cards.Sort();
            long result = 0;
            for (int i = 0; i < cards.Count; i++)
            {
                result += cards[i].Bid * (i + 1);
            }
            return result;
        }

        private class CardHand : IComparable<CardHand>
        {
            private readonly Dictionary<char, int> _cardsQ1 = new Dictionary<char, int>()
            {
                {'T', 10 },
                {'J', 11 },
                {'Q', 12 },
                {'K', 13 },
                {'A', 14 },
            };

            private readonly Dictionary<char, int> _cardsQ2 = new Dictionary<char, int>()
            {
                {'T', 10 },
                {'J', 1 },
                {'Q', 12 },
                {'K', 13 },
                {'A', 14 },
            };


            public int[] CardValues { get; set; } = new int[5];
            public int Bid { get; set; }
            public int HandValue { get; private set; }

            public CardHand(string cardString, bool Q2 = false)
            {
                string[] strings = cardString.Split(' ');
                Bid = int.Parse(strings[1]);
                if (!Q2) 
                {
                    SetCardValuesQ1(strings[0]);
                }
                else
                {
                    SetCardValuesQ2(strings[0]);
                }
            }

            private void SetCardValuesQ2(string valueString)
            {
                char[] stringValues = valueString.ToArray();
                int result;
                Dictionary<int, int> cardCount = new Dictionary<int, int>();
                for (int i = 0; i < stringValues.Length; i++)
                {
                    if (int.TryParse(stringValues[i].ToString(), out result))
                    {
                        CardValues[i] = result;
                    }
                    else
                    {
                        CardValues[i] = _cardsQ2[stringValues[i]];
                    }
                    if (!cardCount.ContainsKey(CardValues[i]))
                    {
                        cardCount[CardValues[i]] = 0;
                    }
                    cardCount[CardValues[i]]++;
                }
                SetHandValueQ2(cardCount);
            }

            private void SetCardValuesQ1(string valueString)
            {
                char[] stringValues = valueString.ToArray();
                int result;
                Dictionary<int, int> cardCount = new Dictionary<int, int>();
                for (int i = 0; i < stringValues.Length; i++)
                {
                    if (int.TryParse(stringValues[i].ToString(), out result))
                    {
                        CardValues[i] = result;
                    }
                    else
                    {
                        CardValues[i] = _cardsQ1[stringValues[i]];
                    }
                    if (!cardCount.ContainsKey(CardValues[i]) )
                    {
                        cardCount[CardValues[i]] = 0;   
                    }
                    cardCount[CardValues[i]]++;
                }
                SetHandValueQ1(cardCount);
            }

            private void SetHandValueQ2(Dictionary<int, int> cardCount)
            {
                int tempResult = 0;
                int jokerResult = cardCount.GetValueOrDefault(1, 0);
                foreach (var cardAmount in cardCount)
                {
                    if (cardAmount.Key == 1) continue;
                    switch (cardAmount.Value)
                    {
                        case 1:
                            tempResult += 0;
                            break;
                        case 2:
                            tempResult += 1;
                            break;
                        case 3:
                            tempResult += 4;
                            break;
                        case 4:
                            tempResult += 8;
                            break;
                        case 5:
                            tempResult += 16;
                            break;
                    }
                }
                switch (jokerResult)
                {
                    case 1:
                        if (tempResult == 0) tempResult = 1;
                        else if (tempResult == 1) tempResult = 4;
                        else if( tempResult == 2) tempResult = 5;
                        else tempResult *= 2;
                        break;
                    case 2:
                        if (tempResult == 0) tempResult = 4;
                        else if (tempResult == 1) tempResult = 8;
                        else if (tempResult == 4) tempResult = 16;
                        break;
                    case 3:
                        if (tempResult == 0) tempResult = 8;
                        else if (tempResult == 1) tempResult = 16;
                        break;
                    case 4:
                        tempResult = 16;
                        break;
                    case 5:
                        tempResult = 16;
                        break;
                }
                HandValue = tempResult;
            }

            private void SetHandValueQ1(Dictionary<int, int> cardCount)
            {
                int result = 0;
                foreach (var cardAmount in cardCount)
                {
                    switch (cardAmount.Value)
                    {
                        case 1:
                            result += 0;
                            break;
                        case 2:
                            result += 1; 
                            break;
                        case 3:
                            result += 4;
                            break;
                        case 4:
                            result += 8;
                            break;
                        case 5:
                            result += 16;
                            break;
                    }
                }
                HandValue = result;
            }

            public int CompareTo(CardHand? other)
            {
                if (other == null) return 1;
                if (this.HandValue > other.HandValue) return 1;
                if (this.HandValue < other.HandValue) return -1;
                return CompareHands(other);
            }

            private int CompareHands(CardHand other)
            {
                for (int i = 0; i < CardValues.Length; i++)
                {
                    if (this.CardValues[i] == other.CardValues[i])
                        continue;
                    return this.CardValues[i] > other.CardValues[i] ? 1 : -1;

                }
                return 0;
            }
        }
    }
}
