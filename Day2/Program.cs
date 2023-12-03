namespace Day2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PartOne();
            PartTwo();
        }

        /// <summary>
        /// Determine which games would have been possible if the bag had been loaded with only 12 red cubes, 
        /// 13 green cubes, and 14 blue cubes. What is the sum of the IDs of those games?
        /// </summary>
        static void PartOne()
        {
            var redMax = 12;
            var greenMax = 13;
            var blueMax = 14;

            var sumOfIds = 0;
            var inputs = File.ReadLines("input.txt");

            foreach (var input in inputs)
            {
                var id = int.Parse(input[5..input.IndexOf(':')]);
                var sets = input.Split(':')[1].Split("; ").Select(x => x.Trim().Split(", "));
                var validSet = false;

                foreach (var set in sets)
                {
                    foreach (var cubes in set)
                    {
                        var cubeSplit = cubes.Split(' ');

                        validSet = cubeSplit[1] switch
                        {
                            "red" => int.Parse(cubeSplit[0]) <= redMax,
                            "green" => int.Parse(cubeSplit[0]) <= greenMax,
                            "blue" => int.Parse(cubeSplit[0]) <= blueMax,
                        };

                        if (!validSet) 
                            break;
                    }

                    if (!validSet)
                        break;
                }

                sumOfIds += validSet ? id : 0;
            }

            Console.WriteLine("PART ONE - Sum of all IDs: " + sumOfIds);
        }

        /// <summary>
        /// The power of a set of cubes is equal to the numbers of red, green, and blue cubes multiplied together. The power of the minimum set
        /// of cubes in game 1 is 48. In games 2-5 it was 12, 1560, 630, and 36, respectively. Adding up these five powers produces the sum 2286.
        /// 
        /// For each game, find the minimum set of cubes that must have been present.
        /// What is the sum of the power of these sets?
        /// </summary>
        static void PartTwo()
        {
            var minimumDicePerGame = new Dictionary<int, int[]>();
            var inputs = File.ReadLines("input.txt");

            foreach (var input in inputs)
            {
                var id = int.Parse(input[5..input.IndexOf(':')]);
                var sets = input.Split(':')[1].Split("; ").Select(x => x.Trim().Split(", "));

                var minimumDicePerColour = new int[3] {0, 0, 0};

                foreach (var set in sets)
                {
                    foreach (var cubes in set)
                    {
                        var cubeSplit = cubes.Split(' ');
                        var value = int.Parse(cubeSplit[0]);
                        var colour = cubeSplit[1];

                        switch (colour) 
                        {
                            case "red":
                                if (value > minimumDicePerColour[0]) minimumDicePerColour[0] = value;
                                continue;
                            case "green":
                                if (value > minimumDicePerColour[1]) minimumDicePerColour[1] = value;
                                continue;
                            case "blue":
                                if (value > minimumDicePerColour[2]) minimumDicePerColour[2] = value;
                                continue;
                        }
                    }
                }

                minimumDicePerGame.Add(id, minimumDicePerColour);
            }

            var sum = minimumDicePerGame.Select(kpv => kpv.Value.Aggregate((a, b) => a * b)).Sum();
            Console.WriteLine("PART TWO - Sum of all products of mimimum dice values: " + sum);
        }
    }
}
