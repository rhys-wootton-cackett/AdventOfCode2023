using System.Windows.Markup;

namespace Day7
{
    internal class Program
    {
        static void Main()
        {
            PartOne();
            PartTwo();
        }

        /// <summary>
        /// In Camel Cards, you get a list of hands, and your goal is to order them based on the strength of each hand. A hand consists of five cards labeled one 
        /// of A, K, Q, J, T, 9, 8, 7, 6, 5, 4, 3, or 2. The relative strength of each card follows this order, where A is the highest and 2 is the lowest.
        /// 
        /// Hands are primarily ordered based on type; for example, every full house is stronger than any three of a kind.
        /// 
        /// If two hands have the same type, a second ordering rule takes effect. Start by comparing the first card in each hand. If these cards are different, 
        /// the hand with the stronger first card is considered stronger. If the first card in each hand have the same label, however, then move on to considering 
        /// the second card in each hand. If they differ, the hand with the higher second card wins; otherwise, continue with the third card in each hand, then 
        /// the fourth, then the fifth.
        /// 
        /// Find the rank of every hand in your set. What are the total winnings?
        /// </summary>
        static void PartOne()
        {
            var inputs = File.ReadAllLines("input.txt")
                             .Select(i => i.Trim().Split(' '))
                             .Select(i => new CamelCardSet(i[0], int.Parse(i[1])))
                             .ToList();

            inputs.Sort((a, b) => CamelCardComparison(a, b, false));

            var totalWinnings = 0;

            for (int i = 0; i < inputs.Count; i++)
            {
                totalWinnings += inputs[i].Bet * (inputs.Count - i);
            }

            Console.WriteLine("PART ONE - Total winnings: " + totalWinnings);

        }


        /// <summary>
        /// To make things a little more interesting, the Elf introduces one additional rule. Now, J cards are jokers - wildcards that can act like whatever card
        /// would make the hand the strongest type possible.
        /// 
        /// To balance this, J cards are now the weakest individual cards, weaker even than 2. The other cards stay in the same order: 
        /// A, K, Q, T, 9, 8, 7, 6, 5, 4, 3, 2, J.
        /// 
        /// Using the new joker rule, find the rank of every hand in your set. What are the new total winnings?
        /// </summary>
        static void PartTwo()
        {
            var inputs = File.ReadAllLines("input.txt")
                             .Select(i => i.Trim().Split(' '))
                             .Select(i => new CamelCardSet(i[0], int.Parse(i[1])))
                             .ToList();

            inputs.Sort((a, b) => CamelCardComparison(a, b, true));

            var totalWinnings = 0;

            for (int i = 0; i < inputs.Count; i++)
            {
                totalWinnings += inputs[i].Bet * (inputs.Count - i);
            }

            Console.WriteLine("PART TWO - Total winnings with Jokers: " + totalWinnings);
        }

        static int CamelCardComparison(CamelCardSet setOne, CamelCardSet setTwo, bool hasJoker = false)
        {
            var setOneType = GetHandType(setOne, hasJoker);
            var setTwoType = GetHandType(setTwo, hasJoker);

            if (setOneType > setTwoType) return -1;

            if (setOneType < setTwoType) return 1;

            for (int i = 0; i < setOne.CamelCards.Length; i++)
            {
                var setOneCardValue = hasJoker ? CardValueMapWithJoker.Find(m => m.Card == setOne.CamelCards[i]) : CardValueMap.Find(m => m.Card == setOne.CamelCards[i]);
                var setTwoCardValue = hasJoker ? CardValueMapWithJoker.Find(m => m.Card == setTwo.CamelCards[i]) : CardValueMap.Find(m => m.Card == setTwo.CamelCards[i]);

                if (setOneCardValue.Value > setTwoCardValue.Value) return -1;

                if (setOneCardValue.Value < setTwoCardValue.Value) return 1;
            }

            return 0;
        }

        static CamelCardHandType GetHandType(CamelCardSet camelCardSet, bool hasJoker)
        {
            var cardGroups = camelCardSet.CamelCards.ToCharArray()
                                                    .GroupBy(c => c)
                                                    .Select(g => new { Card = g.Key, Count = g.Count() })
                                                    .ToDictionary(g => g.Card, g => g.Count);

            if (hasJoker && cardGroups.TryGetValue('J', out int value))
            {
                var highestCard = cardGroups.OrderByDescending(kvp => kvp.Value).FirstOrDefault().Key;
                cardGroups[highestCard] += value;
                cardGroups.Remove('J');

            }

            if (cardGroups.Count == 5)
                return CamelCardHandType.HighCard;
            if (cardGroups.Count == 4)
                return CamelCardHandType.OnePair;
            if (cardGroups.Count == 3 && cardGroups.Values.Max() == 2)
                return CamelCardHandType.TwoPair;
            if (cardGroups.Count == 3)
                return CamelCardHandType.ThreeOfAKind;
            if (cardGroups.Count == 2 && cardGroups.Values.Max() == 3)
                return CamelCardHandType.FullHouse;
            if (cardGroups.Count == 2)
                return CamelCardHandType.FourOfAKind;
            return CamelCardHandType.FiveOfAKind;
        }

        enum CamelCardHandType
        {
            HighCard,
            OnePair,
            TwoPair,
            ThreeOfAKind,
            FullHouse,
            FourOfAKind,
            FiveOfAKind
        }

        internal static List<(char Card, int Value)> CardValueMap = new() { ('A', 14), ('K', 13), ('Q', 12), ('J', 11), ('T', 10), ('9', 9), ('8', 8), ('7', 7), ('6', 6), ('5', 5), ('4', 4), ('3', 3), ('2', 2) };

        internal static List<(char Card, int Value)> CardValueMapWithJoker = new() { ('A', 14), ('K', 13), ('Q', 12), ('T', 10), ('9', 9), ('8', 8), ('7', 7), ('6', 6), ('5', 5), ('4', 4), ('3', 3), ('2', 2), ('J', 1) };

        record CamelCardSet(string CamelCards, int Bet);
    }
}