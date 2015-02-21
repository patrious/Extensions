using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace Extensions
{
    public class TestHarness
    {
        /// <summary>
        /// Output the max, min, and average amount of computer ticks
        /// </summary>
        /// <param name="results"></param>
        /// <param name="message">Message to identify this output</param>
        private static void OutputResults(List<TimeSpan> results, string message)
        {
            var low = results.Min();
            var high = results.Max();
            var average = results.Average(x => x.Ticks);

            Console.WriteLine(String.Format(CultureInfo.InvariantCulture,"Overall: Min: {0} Ticks\t Max: {1} Ticks\t Average {2} Ticks \t - {3}", low.Ticks, high.Ticks, average, message));
        }

        /// <summary>
        /// Run a piece of code a number of times and produce statistical information on the console.
        /// </summary>
        /// <param name="message">Message identifying the code being tested</param>
        /// <param name="numberOfRuns">Number of times to test the code</param>
        /// <param name="code">Target Code to be tested</param>
        public static void TimeStatistic(string message, int numberOfRuns, Action code)
        {
            var results = new List<TimeSpan>();
            for (var i = 0; i < numberOfRuns; i++)
            {
                results.Add(TimeTrial(message, code));
            }

            OutputResults(results, message);

        }

        /// <summary>
        /// Run a piece of code a number of times and produce statistical information on the console.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message">Message to identify this output</param>
        /// <param name="numberOfRuns">Number of times to test the code</param>
        /// <param name="code">Target Code to be tested</param>
        /// <returns>Returns back the last ouput returned from test code</returns>
        public static T TimeStatistic<T>(string message, int numberOfRuns, Func<T> code)
        {
            var results = new List<TimeSpan>();
            var result = default(T);
            for (var i = 0; i < numberOfRuns; i++)
            {
                var x = TimeTrial(message, code);
                results.Add(x.Item1);
                result = x.Item2;
            }

            OutputResults(results, message);
            return result;
        }

        /// <summary>
        /// Run a time trial on a Function
        /// </summary>
        /// <typeparam name="T">Expected Return Type</typeparam>
        /// <param name="message">Message to identify this test</param>
        /// <param name="code">Target Code to be tested</param>
        /// <returns>Length of test and desired result from code</returns>
        public static Tuple<TimeSpan, T> TimeTrial<T>(string message, Func<T> code)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var returnVal = code();
            stopwatch.Stop();
            var timediff = stopwatch.Elapsed;
            return new Tuple<TimeSpan, T>(timediff, returnVal);
        }

        /// <summary>
        /// Run a time trial on an Action
        /// </summary>
        /// <param name="message">Message to identify this test</param>
        /// <param name="code">arget Code to be tested</param>
        /// <returns>Length of test</returns>
        public static TimeSpan TimeTrial(string message, Action code)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            code();
            stopwatch.Stop();
            var timediff = stopwatch.Elapsed;
            return timediff;
        }
    }
}
