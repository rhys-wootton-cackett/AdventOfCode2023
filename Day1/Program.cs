using System.Numerics;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace Day1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            PartOne();
            PartTwo();
        }

        /// <summary>
        /// A newly-improved calibration document consists of lines of text; each line originally contained a specific calibration value that the 
        /// Elves now need to recover. On each line, the calibration value can be found by combining the first digit and the last digit (in that order) 
        /// to form a single two-digit number.
        /// 
        /// For example:
        /// 	1abc2
        /// 	pqr3stu8vwx
        /// 	a1b2c3d4e5f
        /// 	treb7uchet
        /// 	
        /// In this example, the calibration values of these four lines are 12, 38, 15, and 77. Adding these together produces 142.
        /// 
        /// Consider your entire calibration document.What is the sum of all of the calibration values?
        /// </summary>
        static void PartOne()
        {
            int sumOfCalibrationValues = 0;

            var inputs = File.ReadLines("input.txt");

            foreach (var input in inputs)
            {
                var onlyInts = input.Where(Char.IsDigit).ToArray();
                int calibrationValue = int.Parse(onlyInts.First().ToString() + onlyInts.Last().ToString());
                sumOfCalibrationValues += calibrationValue;
            }

            Console.WriteLine("PART ONE - Sum of calibration values: " + sumOfCalibrationValues);
        }

        /// <summary>
        /// Your calculation isn't quite right. It looks like some of the digits are actually spelled out with letters: one, two, three, four, five, 
        /// six, seven, eight, and nine also count as valid "digits".
        ///
        /// Equipped with this new information, you now need to find the real first and last digit on each line. For example:
        ///
        ///		two1nine
        ///		eightwothree
        ///		abcone2threexyz
        ///		xtwone3four
        ///		4nineeightseven2
        ///		zoneight234
        ///		7pqrstsixteen
        ///
        /// In this example, the calibration values are 29, 83, 13, 24, 42, 14, and 76. Adding these together produces 281.
        ///
        /// What is the sum of all of the calibration values?
        /// </summary>
        static void PartTwo()
        {
            int sumOfCalibrationValues = 0;
            string[] validDigitsAsStrings = ["one", "two", "three", "four", "five", "six", "seven", "eight", "nine"];

            var inputs = File.ReadLines("input.txt");

            foreach (var input in inputs)
            {
                var inputChars = input.ToCharArray();
                string calibrationValue = "";
                string inputStringDigitCheck = "";

                // Start by reading from the beginning until we find a valid number.
                for (int i = 0; i < input.Length; i++)
                {
                    if (Char.IsDigit(inputChars[i]))
                    {
                        calibrationValue += inputChars[i];
                        break;
                    }

                    inputStringDigitCheck += inputChars[i];
                    var stringDigit = Array.Find(validDigitsAsStrings, inputStringDigitCheck.Contains);

                    if (stringDigit != null)
                    {
                        calibrationValue += GetNumericalValueFromString(stringDigit);
                        break;
                    } 
                }

                inputStringDigitCheck = "";

                // Then start reading backwards so we don't waste time!
                for (int i = input.Length - 1; i >= 0; i--)
                {
                    if (Char.IsDigit(inputChars[i]))
                    {
                        calibrationValue += inputChars[i];
                        break;
                    }

                    inputStringDigitCheck = inputChars[i] + inputStringDigitCheck;
                    var stringDigit = Array.Find(validDigitsAsStrings, inputStringDigitCheck.Contains);

                    if (stringDigit != null)
                    {
                        calibrationValue += GetNumericalValueFromString(stringDigit);
                        break;
                    }
                }

                sumOfCalibrationValues += int.Parse(calibrationValue);
            }

            Console.WriteLine("PART TWO - Sum of calibration values: " + sumOfCalibrationValues);
        }

        static int GetNumericalValueFromString(string numericValue)
        {
            return numericValue switch
            {
                "one" => 1,
                "two" => 2,
                "three" => 3,
                "four" => 4,
                "five" => 5,
                "six" => 6,
                "seven" => 7,
                "eight" => 8,
                "nine" => 9,
                _ => 0,
            };
        }

    }
}
