using static System.Formats.Asn1.AsnWriter;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text.RegularExpressions;

namespace Day4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PartOne();
            PartTwo();
        }

        /// <summary>
        /// You have to figure out which of the numbers you have appear in the list of winning numbers. The first match makes the card 
        /// worth one point and each match after the first doubles the point value of that card.
        /// 
        /// How many points are they worth in total?
        /// </summary>
        static void PartOne()
        {
            var cardInputs = File.ReadAllLines("input.txt")
                                .Select(input => input[10..])
                                .Select(input => input.Split("|"));

            var winningSum = 0;

            foreach (var card in cardInputs)
            {
                var winningNumbers = card[0].Trim().Replace("  ", " ").Split(" ").Select(int.Parse).ToArray();
                var scratchedNumbers = card[1].Trim().Replace("  ", " ").Split(" ").Select(int.Parse).ToArray();

                var matchingNumbersCount = scratchedNumbers.Count(winningNumbers.Contains);
                winningSum += (int) Math.Pow(2, matchingNumbersCount - 1);
            }

            Console.WriteLine("PART ONE - Winning sum of scratchcards: " + winningSum);
        }

        /// <summary>
        /// There's no such thing as "points". Instead, scratchcards only cause you to win more scratchcards equal to the number of 
        /// winning numbers you have.
        /// 
        /// Specifically, you win copies of the scratchcards below the winning card equal to the number of matches.So, if card 10 were to 
        /// have 5 matching numbers, you would win one copy each of cards 11, 12, 13, 14, and 15.
        /// 
        /// Copies of scratchcards are scored like normal scratchcards and have the same card number as the card they copied. So, if you 
        /// win a copy of card 10 and it has 5 matching numbers, it would then win a copy of the same cards that the original card 10 
        /// won: cards 11, 12, 13, 14, and 15. This process repeats until none of the copies cause you to win any more cards. 
        /// (Cards will never make you copy a card past the end of the table.)
        /// 
        /// Process all of the original and copied scratchcards until no more scratchcards are won. Including the original set of scratchcards,
        /// how many total scratchcards do you end up with?
        /// </summary>
        static void PartTwo()
        {
            var cardInputs = File.ReadAllLines("input.txt")
            .Select(input => input[10..])
            .Select(input => input.Split("|"))
            .ToArray();
            var cardCounts = new int[cardInputs.Length].Select(x => x = 1).ToArray();

            for (int i = 0; i < cardInputs.Length; i++)
            {
                var numberOfIterations = 0;

                var winningNumbers = cardInputs[i][0].Trim().Replace("  ", " ").Split(" ").Select(int.Parse).ToArray();
                var scratchedNumbers = cardInputs[i][1].Trim().Replace("  ", " ").Split(" ").Select(int.Parse).ToArray();
                var matchingNumbersCount = scratchedNumbers.Count(winningNumbers.Contains);

                do
                {
                    for (int j = 1; j <= matchingNumbersCount; j++)
                    {
                        if (i + j <= cardCounts.Length - 1)
                            cardCounts[i + j] += 1;
                    }

                    numberOfIterations++;
                } while (cardCounts[i] > numberOfIterations);
            }

            Console.WriteLine("PART TWO - Sum of all won scratchcards: " + cardCounts.Sum());
        }
    }
}
