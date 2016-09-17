﻿using Microsoft.CSharp.RuntimeBinder;

using System;
using System.Collections.Generic;
using System.Diagnostics;

using TestRunners.Runners.Contracts;
using TestRunners.Tests.Contracts;

namespace TestRunners.Runners
{
    public class NaiveTestRunner : ITestRunner
    {
        private const string LogEntryFormat = "{0, -16} - {1, 6}ms - {2, 12:F8}ms";

        private ICollection<string> logEntries;

        public NaiveTestRunner()
        {
            this.logEntries = new LinkedList<string>();
        }

        public IEnumerable<string> LogEntries
        {
            get
            {
                var exposedLogEntries = new List<string>(this.logEntries);
                return exposedLogEntries;
            }
        }

        public void WarmUp(int numberOfRuns)
        {
            Action warmUp = () =>
            {
                dynamic a = Math.Sqrt(2);
                dynamic b = Math.Sqrt(3);
                var c = a * b;
            };

            var totalTimeElapsedInMs = this.MeasureTestExecutionTime(warmUp, numberOfRuns);
            this.CreateNewLogEntry("warmUp", totalTimeElapsedInMs, numberOfRuns);
            this.CreateNewEmptyLogEntry();
        }

        public void EvaluateTests(ITestContainer testsToRunContainer)
        {
            if (testsToRunContainer == null)
            {
                throw new ArgumentNullException("testsToRun");
            }

            var numberOfRuns = testsToRunContainer.NumberOfRuns;
            foreach (var test in testsToRunContainer.Tests)
            {
                var testName = test.Method.Name;
                try
                {
                    var totalTimeElapsedInMs = this.MeasureTestExecutionTime(test, numberOfRuns);
                    this.CreateNewLogEntry(testName, totalTimeElapsedInMs, numberOfRuns);
                }
                catch (RuntimeBinderException ex)
                {
                    this.CreateErrorLogEntry(testName, ex.Message);
                }
            }

            this.CreateNewEmptyLogEntry();
        }

        private long MeasureTestExecutionTime(Action task, int numberOfRuns)
        {
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            for (int run = 0; run < numberOfRuns; run++)
            {
                task.Invoke();
            }

            stopwatch.Stop();

            var totalTimeElapsedInMs = stopwatch.ElapsedMilliseconds;
            return totalTimeElapsedInMs;
        }

        private void CreateErrorLogEntry(string testName, string message)
        {
            var logEntry = testName + " " + message;
            this.logEntries.Add(logEntry);
        }

        private void CreateNewLogEntry(string testName, long totalTimeElapsedInMs, int numberOfRuns)
        {
            var averageTimeElapsedInMs = (double)((double)totalTimeElapsedInMs / (double)numberOfRuns);

            var logEntry = string.Format(
                NaiveTestRunner.LogEntryFormat,
                testName,
                totalTimeElapsedInMs,
                averageTimeElapsedInMs);

            this.logEntries.Add(logEntry);
        }

        private void CreateNewEmptyLogEntry()
        {
            this.logEntries.Add(string.Empty);
        }
    }
}
