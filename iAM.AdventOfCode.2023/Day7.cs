using iAM.AdventOfCode._2023.Models;
using iAM.AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iAM.AdventOfCode._2023
{
    public class Day7 : AoCDay
    {
        public List<(string hand, List<CamelCards> cCards, CamelHands cHand, int bid, int rankMultiplier)> PuzzleMeasurements { get; set; }
        public int Multiplier { get; set; }

        public Day7() : base(7, true, false)
        {
            this.PuzzleAltFilePath = @"Examples\Day7_small.txt";
            this.PuzzleMeasurements = new();
        }

        public override void Puzzle1Content()
        {
            ReadHandAndBids();
            this.PuzzleMeasurements = TranslateHand();
            var completeList = DetermineMultiplier();
            var calcResult = CalculateWinnings(completeList);

            // Answer: 249726565
            Console.WriteLine($"========= Total winning: {calcResult} =========");
        }

        private long CalculateWinnings(List<(string hand, List<CamelCards> cCards, CamelHands cHand, int bid, int rankMultiplier)> completeList)
        {
            var winningHand = new List<long>();
            foreach (var hand in completeList)
            {
                winningHand.Add(hand.bid * hand.rankMultiplier);
            }

            return winningHand.Sum(x => x);
        }

        private void ReadHandAndBids()
        {
            var input = FileReader.ReadInputValues<string, int>(this.Puzzle1FilePath);

            foreach (var hand in input)
            {
                PuzzleMeasurements.Add((hand.Item1, new(), new(), hand.Item2, 0));
            }
        }

        private List<(string hand, List<CamelCards> cCards, CamelHands cHand, int bid, int rankMultiplier)> TranslateHand()
        {
            var result = new List<(string hand, List<CamelCards> cCards, CamelHands cHand, int bid, int rankMultiplier)>();

            foreach (var hands in this.PuzzleMeasurements)
            {
                var cCards = new List<CamelCards>();
                var cHand = default(CamelHands);

                var cards = FileReader.ValueSplitter<char>(hands.hand);

                foreach (var card in cards)
                {
                    switch (card)
                    {
                        case '2':
                            cCards.Add(CamelCards.Card2);
                            break;
                        case '3':
                            cCards.Add(CamelCards.Card3);
                            break;
                        case '4':
                            cCards.Add(CamelCards.Card4);
                            break;
                        case '5':
                            cCards.Add(CamelCards.Card5);
                            break;
                        case '6':
                            cCards.Add(CamelCards.Card6);
                            break;
                        case '7':
                            cCards.Add(CamelCards.Card7);
                            break;
                        case '8':
                            cCards.Add(CamelCards.Card8);
                            break;
                        case '9':
                            cCards.Add(CamelCards.Card9);
                            break;
                        case 'T':
                            cCards.Add(CamelCards.CardT);
                            break;
                        case 'J':
                            cCards.Add(CamelCards.CardJ);
                            break;
                        case 'Q':
                            cCards.Add(CamelCards.CardQ);
                            break;
                        case 'K':
                            cCards.Add(CamelCards.CardK);
                            break;
                        case 'A':
                            cCards.Add(CamelCards.CardA);
                            break;
                    }
                }

                var grouped = cCards.GroupBy(c => c)
                    .ToDictionary(x => x.Key, y => y.Count());

                if (grouped.Count is 1)
                {
                    cHand = CamelHands.FiveOfAKind;
                }
                else if (grouped.Count is 2)
                {
                    if (grouped.ContainsValue(4))
                    {
                        cHand = CamelHands.FourOfAKind;
                    }
                    else if (grouped.ContainsValue(3) && grouped.ContainsValue(2))
                    {
                        cHand = CamelHands.FullHouse;
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (grouped.Count is 3)
                {
                    if (grouped.ContainsValue(3))
                    {
                        cHand = CamelHands.ThreeOfAKind;
                    }
                    else if (grouped.ContainsValue(2))
                    {
                        cHand = CamelHands.TwoPair;
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (grouped.Count > 3)
                {
                    if (grouped.ContainsValue(2))
                    {
                        cHand = CamelHands.OnePair;
                    }
                    else
                    {
                        cHand = CamelHands.HighCard;
                    }
                }

                result.Add((hands.hand, cCards, cHand, hands.bid, 0));
            }

            return result;
        }

        private List<(string hand, List<CamelCards> cCards, CamelHands cHand, int bid, int rankMultiplier)> DetermineMultiplier()
        {
            var result = new List<(string hand, List<CamelCards> cCards, CamelHands cHand, int bid, int rankMultiplier)>();

            var grouped = this.PuzzleMeasurements.GroupBy(c => c.cHand)
                .OrderBy(c => (int)c.Key)
                .ToList();

            this.Multiplier = 1;
            foreach (var groupedHand in grouped)
            {
                if (groupedHand.Count() is 1)
                {
                    var gHand = groupedHand.First();
                    result.Add((gHand.hand, gHand.cCards, gHand.cHand, gHand.bid, this.Multiplier));
                    Console.WriteLine($"Hand {gHand.hand} get rank {this.Multiplier}");
                    this.Multiplier++;
                    continue;
                }

                result.AddRange(DetermineCardOrder(groupedHand.ToList(), 0));
            }

            return result;
        }

        public IEnumerable<(string hand, List<CamelCards> cCards, CamelHands cHand, int bid, int rankMultiplier)> DetermineCardOrder(List<(string hand, List<CamelCards> cCards, CamelHands cHand, int bid, int rankMultiplier)> input, int cardSelector)
        {
            var result = new List<(string hand, List<CamelCards> cCards, CamelHands cHand, int bid, int rankMultiplier)>();

            var subGrouped = input.Select(c => c)
                    .GroupBy(c => c.cCards[cardSelector])
                    .OrderBy(c => (int)c.Key).ToList();

            foreach (var grCard in subGrouped)
            {
                // 5 is the max amount of cards in the hand
                if (grCard.Count() is 1 || (cardSelector + 1) == 5)
                {
                    var gHand = grCard.First();
                    result.Add((gHand.hand, gHand.cCards, gHand.cHand, gHand.bid, this.Multiplier));
                    Console.WriteLine($"Hand {gHand.hand} get rank {this.Multiplier}");
                    this.Multiplier++;
                    continue;
                }

                result.AddRange(DetermineCardOrder(grCard.ToList(), (cardSelector + 1)));
            }

            return result;
        }

        public override void Puzzle2Content()
        {
            throw new NotImplementedException();
        }
    }
}
