﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMSA
{
    class MMSA
    {
        static void Main()
        {
            int numN = int.Parse(Console.ReadLine());

            double[] doubleNumbers = new double[numN];

            for(int i = 0; i< doubleNumbers.Length;i++)
            {
                doubleNumbers[i] = double.Parse(Console.ReadLine());
            }

            //min max sum avrg
            Console.WriteLine("min={0,0:F2}", doubleNumbers.Min());
            Console.WriteLine("max={0,0:F2}", doubleNumbers.Max());
            Console.WriteLine("sum={0,0:F2}", doubleNumbers.Sum());
            Console.WriteLine("avg={0,0:F2}", doubleNumbers.Average());
        }
    }
}
