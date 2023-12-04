namespace Day3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PartOne();
            PartTwo();
        }

        /// <summary>
        /// The engine schematic (your puzzle input) consists of a visual representation of the engine. There are lots of numbers and symbols 
        /// you don't really understand, but apparently any number adjacent to a symbol, even diagonally, is a "part number" and should be included 
        /// in your sum. (Periods (.) do not count as a symbol.)
        /// 
        /// What is the sum of all of the part numbers in the engine schematic?
        /// </summary>
        static void PartOne()
        {
            var engineSchematic = File.ReadAllLines("input.txt").Select(x => x.ToCharArray()).ToArray();
            var sumOfValues = 0;

            for (int y = 0; y < engineSchematic.Length; y++)
            {
                var validValue = false;
                var potentialDigits = "";

                // This allows numbers at the end of each line to get captured, a bit sloppy but oh well!
                engineSchematic[y] = [.. engineSchematic[y], '.'];

                for (int x = 0; x < engineSchematic[y].Length; x++)
                {
                    if (Char.IsDigit(engineSchematic[y][x]))
                    {
                        potentialDigits += engineSchematic[y][x];
                        validValue = validValue || HasNeighbouringSymbol(ref engineSchematic, x, y);
                    }
                    else
                    {
                        if (validValue)
                        {
                            sumOfValues += int.Parse(potentialDigits);
                        }

                        potentialDigits = "";
                        validValue = false;
                    }
                }
            }

            Console.WriteLine("PART ONE - Sum of all part numbers: " + sumOfValues);
        }

        /// <summary>
        /// The missing part wasn't the only issue - one of the gears in the engine is wrong. A gear is any * symbol that is adjacent to exactly 
        /// two part numbers. Its gear ratio is the result of multiplying those two numbers together.
        ///
        /// This time, you need to find the gear ratio of every gear and add them all up so that the engineer can figure out which gear 
        /// needs to be replaced.
        /// 
        /// What is the sum of all of the gear ratios in your engine schematic?
        /// </summary>
        static void PartTwo()
        {
            var engineSchematic = File.ReadAllLines("input.txt").Select(x => x.ToCharArray()).ToArray();
            var gearMappings = new Dictionary<(int xPos, int yPos), HashSet<int>> ();

            for (int y = 0; y < engineSchematic.Length; y++)
            {
                var validValue = false;
                var potentialDigits = "";
                var nearGearsOnLine = new HashSet<(int xPos, int yPos)>();

                // This allows numbers at the end of each line to get captured, a bit sloppy but oh well!
                engineSchematic[y] = [.. engineSchematic[y], '.'];

                for (int x = 0; x < engineSchematic[y].Length; x++)
                {
                    if (Char.IsDigit(engineSchematic[y][x]))
                    {
                        potentialDigits += engineSchematic[y][x];
                        nearGearsOnLine.UnionWith(GetGearPositions(ref engineSchematic, x, y));

                        validValue = validValue || nearGearsOnLine.Count > 0;
                    }
                    else
                    {
                        if (validValue)
                        {
                            foreach (var gear in nearGearsOnLine)
                            {
                                if (!gearMappings.TryGetValue(gear, out HashSet<int>? value))
                                    gearMappings.Add(gear, [int.Parse(potentialDigits)]);
                                else
                                    value.Add(int.Parse(potentialDigits));
                            }
                        }

                        potentialDigits = "";
                        nearGearsOnLine.Clear();
                        validValue = false;
                    }
                }
            }

            var sum = gearMappings
                        .Where(kvp => kvp.Value.Count == 2)
                        .Select(kvp => kvp.Value.Aggregate((a, b) => a * b))
                        .Sum();

            Console.WriteLine("PART ONE - Sum of all part numbers: " + sum);
        }

        static bool HasNeighbouringSymbol(ref char[][] engineSchematic, int xPos, int yPos)
        {
            var symbolPositions = new List<char>
            {
                GetCharFromPosition(ref engineSchematic, xPos - 1, yPos - 1),
                GetCharFromPosition(ref engineSchematic, xPos, yPos - 1),
                GetCharFromPosition(ref engineSchematic, xPos + 1, yPos - 1),
                GetCharFromPosition(ref engineSchematic, xPos - 1, yPos),
                GetCharFromPosition(ref engineSchematic, xPos + 1, yPos),
                GetCharFromPosition(ref engineSchematic, xPos - 1, yPos + 1),
                GetCharFromPosition(ref engineSchematic, xPos, yPos + 1),
                GetCharFromPosition(ref engineSchematic, xPos + 1, yPos + 1)
            };

            return symbolPositions.Any(x => !Char.IsDigit(x) && x != '.');
        }

        static List<(int xPos, int yPos)> GetGearPositions(ref char[][] engineSchematic, int xPos, int yPos)
        {
            var gearPositions = new List<(int x, int y, char c)>
            {
                (xPos - 1, yPos - 1, GetCharFromPosition(ref engineSchematic, xPos - 1, yPos - 1)),
                (xPos, yPos - 1, GetCharFromPosition(ref engineSchematic, xPos, yPos - 1)),
                (xPos + 1, yPos - 1, GetCharFromPosition(ref engineSchematic, xPos + 1, yPos - 1)),
                (xPos - 1, yPos, GetCharFromPosition(ref engineSchematic, xPos - 1, yPos)),
                (xPos + 1, yPos, GetCharFromPosition(ref engineSchematic, xPos + 1, yPos)),
                (xPos - 1, yPos + 1, GetCharFromPosition(ref engineSchematic, xPos - 1, yPos + 1)),
                (xPos, yPos + 1, GetCharFromPosition(ref engineSchematic, xPos, yPos + 1)),
                (xPos + 1, yPos + 1, GetCharFromPosition(ref engineSchematic, xPos + 1, yPos + 1))
            };

            return gearPositions.Where(ele => ele.c == '*').Select(ele => (ele.x, ele.y)).ToList();
        }

        static char GetCharFromPosition(ref char[][] engineSchematic, int xPos, int yPos)
        {
            try
            {
                return engineSchematic[yPos][xPos];
            }
            catch (Exception)
            {
                return '.';
            }
        }
    }
}
