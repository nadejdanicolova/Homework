﻿using System;

using Methods.PersonalInfo;
using Methods.Students;
using Methods.Utils;

namespace Methods
{
    internal class Startup
    {
        internal static void Main()
        {
            Console.WriteLine(CalculationHelpers.CalculateTriangleArea(3, 4, 5));

            Console.WriteLine(CalculationHelpers.DigitToWord(5));

            Console.WriteLine(CalculationHelpers.FindMaximumValue(5, -1, 3, 2, 14, 2, 3));

            ConsoleWriter.PrintToConsoleAsNumberInFormat(1.3, "f");
            ConsoleWriter.PrintToConsoleAsNumberInFormat(0.75, "%");
            ConsoleWriter.PrintToConsoleAsNumberInFormat(2.30, "r");

            bool horizontal;
            bool vertical;
            Console.WriteLine(CalculationHelpers.CalculateDistance(3, -1, 3, 2.5, out horizontal, out vertical));
            Console.WriteLine("Horizontal? " + horizontal);
            Console.WriteLine("Vertical? " + vertical);

            var peterOtherInfo = new PersonalInfo.PersonalInformation("From Sofia, born on 17.03.1992");
            Student peter = new Student("Peter", "Ivanov", peterOtherInfo);

            var stellaOtherInfo = new PersonalInfo.PersonalInformation("From Vidin, gamer, high results, born on 03.11.1993");
            Student stella = new Student("Stella", "Markova", stellaOtherInfo);

            Console.WriteLine("{0} older than {1} -> {2}", peter.FirstName, stella.FirstName, peter.IsOlderThan(stella));
        }
    }
}
