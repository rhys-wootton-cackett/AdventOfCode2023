namespace Day9
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PartOne();
            PartTwo();
        }

        static void PartOne()
        {
            var inputs = File.ReadAllLines("input.txt")
                             .Select(i => i.Trim()
                                           .Split(' ')
                                           .Select(int.Parse)
                                           .ToList())
                             .ToList();

            List<List<int>> sequenceDifferences = [];
            var sumOfNextTerms = 0;

            foreach (var input in inputs)
            {
                sequenceDifferences.Clear();
                sequenceDifferences.Add(input);

                do
                {
                    sequenceDifferences.Add(GetSequenceDifference(sequenceDifferences.Last()));
                } while (!sequenceDifferences.Last().All(x => x == 0));

                for (int i = sequenceDifferences.Count - 1; i >= 1; i--)
                {
                    sequenceDifferences[i - 1].Add(sequenceDifferences[i - 1].Last() + sequenceDifferences[i].Last());
                }

                sumOfNextTerms += sequenceDifferences[0].Last();
            }

            Console.WriteLine("PART ONE - Sum of next terms: " + sumOfNextTerms);
        }

        static void PartTwo()
        {
            var inputs = File.ReadAllLines("input.txt")
                             .Select(i => i.Trim()
                                           .Split(' ')
                                           .Select(int.Parse)
                                           .ToList())
                             .ToList();

            List<List<int>> sequenceDifferences = [];
            var sumOfNextTerms = 0;

            foreach (var input in inputs)
            {
                sequenceDifferences.Clear();
                sequenceDifferences.Add(input);

                do
                {
                    sequenceDifferences.Add(GetSequenceDifference(sequenceDifferences.Last()));
                } while (!sequenceDifferences.Last().All(x => x == 0));

                for (int i = sequenceDifferences.Count - 1; i >= 1; i--)
                {
                    sequenceDifferences[i - 1].Insert(0, sequenceDifferences[i - 1].First() - sequenceDifferences[i].First());
                }

                sumOfNextTerms += sequenceDifferences[0].First();
            }

            Console.WriteLine("PART ONE - Sum of previous terms: " + sumOfNextTerms);
        }

        static List<int> GetSequenceDifference(List<int> sequence)
        {
            var diffSequence = new List<int>();

            for (var i = 0; i < sequence.Count - 1; i++)
            {
                diffSequence.Add(sequence[i + 1] - sequence[i]);
            }

            return diffSequence;
        }
    }
}
