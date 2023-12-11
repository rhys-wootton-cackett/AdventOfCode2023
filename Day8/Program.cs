using System.Security.Cryptography.X509Certificates;

namespace Day8
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
            var inputs = File.ReadAllLines("input.txt");

            var directionInstructions = inputs[0].ToCharArray();

            var map = inputs[2..].Select(x => new { a = x[0..3], b = new MapDirectionLine(x[7..10], x[12..15]) })
                                 .ToDictionary(m => m.a, m => m.b);

            var stepCount = 0;
            var directionStep = 0;
            var currentLocation = "AAA";

            do
            {
                var currentDirection = directionInstructions[directionStep];
                var currentMapLine = map[currentLocation];
                currentLocation = currentDirection == 'L' ? currentMapLine.Left : currentMapLine.Right;

                stepCount++;
                directionStep = (directionStep + 1) % directionInstructions.Length;
            } while (currentLocation != "ZZZ");

            Console.WriteLine("PART ONE - Number of steps: " + stepCount);
        }

        static void PartTwo()
        {
            var inputs = File.ReadAllLines("input.txt");

            var directionInstructions = inputs[0].ToCharArray();

            var map = inputs[2..].Select(x => new { a = x[0..3], b = new MapDirectionLine(x[7..10], x[12..15]) })
                                 .ToDictionary(m => m.a, m => m.b);

            var stepCount = 0;
            var directionStep = 0;
            var currentLocations = map.Where(kvp => kvp.Key[2] == 'A').Select(kvp => kvp.Key).ToList();
            var locationStatuses = currentLocations.ToDictionary(k => k, _ => (foundZed: false, steps: 0L));

            do
            {
                var currentDirection = directionInstructions[directionStep];

                for (int i = 0; i < currentLocations.ToArray().Length; i++)
                {
                    if (locationStatuses[locationStatuses.ElementAt(i).Key].foundZed) continue;

                    currentLocations[i] = currentDirection == 'L' ? map[currentLocations[i]].Left : map[currentLocations[i]].Right;

                    if (currentLocations[i][2] == 'Z')
                    {
                        locationStatuses[locationStatuses.ElementAt(i).Key] = (true, locationStatuses[locationStatuses.ElementAt(i).Key].steps + 1);
                    } else
                    {
                        locationStatuses[locationStatuses.ElementAt(i).Key] = (false, locationStatuses[locationStatuses.ElementAt(i).Key].steps + 1);
                    }
                }

                directionStep = (directionStep + 1) % directionInstructions.Length;
            } while (locationStatuses.Values.Any(p => !p.foundZed));

            var quickestPath = GetLowestCommonMultiplier(locationStatuses.Values.Select(a => a.steps).ToArray());

            Console.WriteLine("PART TWO - Shortest path to nodes ending in Z: " + quickestPath);

        }

        record MapDirectionLine(string Left, string Right);

        static long GetLowestCommonMultiplier(long[] numbers)
        {
            return numbers.Aggregate(LCM);
        }
        static long LCM(long a, long b)
        {
            return Math.Abs(a * b) / GCD(a, b);
        }
        static long GCD(long a, long b)
        {
            return b == 0 ? a : GCD(b, a % b);
        }
    }
}