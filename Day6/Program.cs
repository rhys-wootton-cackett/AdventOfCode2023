namespace Day6
{
    internal class Program
    {
        static void Main()
        {
            PartOne();
            PartTwo();
        }

        /// <summary>
        /// Your toy boat has a starting speed of zero millimeters per millisecond. For each whole millisecond you spend
        /// at the beginning of the race holding down the button, the boat's speed increases by one millimeter per millisecond.
        /// 
        /// To see how much margin of error you have, determine the number of ways you can beat the record in each race.
        /// 
        /// What do you get if you multiply these numbers together?
        /// </summary>
        static void PartOne()
        {
            var inputs = File.ReadAllLines("input.txt")
                            .Select(r => r.Split(' ').Where(x => !string.IsNullOrEmpty(x)).ToArray())
                            .ToArray();

            var races = new Race[inputs[0].Length - 1];

            for (int i = 1; i < inputs[0].Length; i++)
            {
                races[i - 1] = new Race(int.Parse(inputs[0][i]), int.Parse(inputs[1][i]));
            }

            var winningCombosCount = 1;

            foreach (var race in races)
            {
                var winningComboCount = 0;

                for (int i = 0; i < race.Time; i++)
                {
                    var distanceTravelled = i * (race.Time - i);

                    if (distanceTravelled > race.Distance)
                        winningComboCount++;
                }

                winningCombosCount *= winningComboCount;
            }

            Console.WriteLine("PART ONE - Winning combos count multiplied: " + winningCombosCount);
        }


        /// <summary>
        /// As the race is about to start, you realize the piece of paper with race times and record distances you got earlier actually
        /// just has very bad kerning. There's really only one race - ignore the spaces between the numbers on each line.
        /// 
        /// How many ways can you beat the record in this one much longer race?
        /// </summary>
        static void PartTwo()
        {
            var raceData = File.ReadAllLines("input.txt")
                               .Select(r => r.Trim().Replace(" ", ""))
                               .ToArray();

            var race = new Race(long.Parse(raceData[0][5..]), long.Parse(raceData[1][9..]));

            var winningComboCount = 0;

            for (int i = 0; i < race.Time; i++)
            {
                var distanceTravelled = i * (race.Time - i);

                if (distanceTravelled > race.Distance)
                    winningComboCount++;
            }

            Console.WriteLine("PART TWO - Winning combos count: " + winningComboCount);
        }

        record Race(long Time, long Distance);

    }
}