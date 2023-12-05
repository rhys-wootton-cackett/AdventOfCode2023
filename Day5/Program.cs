using System.Collections;

namespace Day5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PartOne();
            PartTwo();
        }

        /// <summary>
        /// The gardener and his team want to get started as soon as possible, so they'd like to know the closest location that 
        /// needs a seed. Using these maps, find the lowest location number that corresponds to any of the initial seeds.
        /// 
        /// What is the lowest location number that corresponds to any of the initial seed numbers?
        /// </summary>
        static void PartOne()
        {
            var input = File.ReadAllText("input.txt").Split("\n\n");

            var seeds = input[0][7..].Split(' ')
                                     .Select(uint.Parse)
                                     .ToArray();

            var blocks = input[1..];

            foreach (var rangeBlock in blocks)
            {
                var ranges = rangeBlock.Split("\n")[1..]
                                      .Select(r => r.Trim().Split(' '))
                                      .Select(r => new Range(uint.Parse(r[0]), uint.Parse(r[1]), uint.Parse(r[2])))
                                      .ToArray();

                var newSeeds = new List<uint>();

                foreach (var seed in seeds)
                {
                    var isValid = false;

                    foreach (var range in ranges)
                    {
                        if (range.Source <= seed && seed < range.Source + range.Length)
                        {
                            newSeeds.Add((seed - range.Source) + range.Desination);
                            isValid = true;
                            break;
                        }
                    }

                    if (!isValid) newSeeds.Add(seed);
                }

                seeds = [.. newSeeds];
            }

            Console.WriteLine("PART ONE - Minimum seed location: " + seeds.Min());
            Console.WriteLine();
        }

        /// <summary>
        /// The values on the initial seeds line come in pairs. Within each pair, the first value is the start of the range and the 
        /// second value is the length of the range.
        /// 
        /// Consider all of the initial seed numbers listed in the ranges on the first line of the almanac. What is the lowest 
        /// location number that corresponds to any of the initial seed numbers?
        /// 
        /// NOTE: This part currently uses brute forcing to get the minimum location number. I know there is probably a better way
        /// to work this out using some clever maths but I don't have time to wrap my head around that! On my Dell XPS 9720 this
        /// takes 20-30 minutes to run.
        /// </summary>
        static void PartTwo()
        {
            Console.WriteLine("Part two takes about 20-30 minutes to run on a high-end laptop as it currently uses brute forcing.");

            var input = File.ReadAllText("input.txt").Split("\n\n");

            var seedRanges = input[0][7..].Split(' ')
                                     .Select(uint.Parse)
                                     .ToArray();

            var seeds = new List<uint>();

            for (int i = 0; i < seedRanges.Length - 1; i += 2)
            {
                seeds.AddRange(Enumerable.Range((int)seedRanges[i], (int)seedRanges[i + 1]).Select(x => (uint)x));
            }


            var blocks = input[1..];
            var blockCounter = 0;

            foreach (var rangeBlock in blocks)
            {
                var ranges = rangeBlock.Split("\n")[1..]
                                      .Select(r => r.Trim().Split(' '))
                                      .Select(r => new Range(uint.Parse(r[0]), uint.Parse(r[1]), uint.Parse(r[2])))
                                      .ToArray();

                var newSeeds = new List<uint>();

                foreach (var seed in seeds)
                {
                    var isValid = false;

                    foreach (var range in ranges)
                    {
                        if (range.Source <= seed && seed < range.Source + range.Length)
                        {
                            newSeeds.Add((seed - range.Source) + range.Desination);
                            isValid = true;
                            break;
                        }
                    }

                    if (!isValid) newSeeds.Add(seed);
                }

                Console.WriteLine($"Completed block {++blockCounter}");

                seeds = [.. newSeeds];
            }

            Console.WriteLine("PART TWO - Minimum seed location: " + seeds.Min());
        }
    }

    record Range(uint Desination, uint Source, uint Length);

}