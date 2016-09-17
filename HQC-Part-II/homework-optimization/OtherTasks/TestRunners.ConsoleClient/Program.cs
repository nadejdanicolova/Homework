﻿using System;

using TestRunners.Runners;
using TestRunners.Runners.Contracts;
using TestRunners.Tests;

namespace TestRunners.ConsoleClient
{
    public class Program
    {
        private const int TestRunsCount = 10000;

        public static void Main()
        {
            var homeworkTestRunner = new NaiveTestRunner();
            homeworkTestRunner.WarmUp(Program.TestRunsCount);

            ExecuteTask1Tests(homeworkTestRunner);

            var output = string.Join(Environment.NewLine, homeworkTestRunner.LogEntries);
            Console.WriteLine(output);
        }

        private static void ExecuteTask1Tests(ITestRunner testRunner)
        {
            var intTests = new SimpleMathTestContainer<int>(1, 1, Program.TestRunsCount);
            testRunner.EvaluateTests(intTests);

            var longTests = new SimpleMathTestContainer<long>(1, 1, Program.TestRunsCount);
            testRunner.EvaluateTests(longTests);

            var floatTests = new SimpleMathTestContainer<float>(1f, 1f, Program.TestRunsCount);
            testRunner.EvaluateTests(floatTests);

            var doubleTests = new SimpleMathTestContainer<double>(1, 1, Program.TestRunsCount);
            testRunner.EvaluateTests(doubleTests);

            var decimalTests = new SimpleMathTestContainer<decimal>(1, 1, Program.TestRunsCount);
            testRunner.EvaluateTests(decimalTests);
        }
    }
}
