using System;
using System.Collections.Generic;
using System.Text;
using SafeBlocks;
using SafeBlocks.Analysis;
using SafeBlocks.String;

namespace Examples.String
{
    class Program
    {
        public static void StringExample()
        {
            Config.ExectuionMode = ExecutionMode.Release;
            Config.ExecutionSpeed = ExecutionSpeed.ExtraFast;

            var sb = new StringBuilder("");
            for (int i = 0; i < 1000; i++) sb.Append("HelloWorld");
            var testString = sb.ToString();

            Profiler<int> adversary;

            Console.WriteLine("Unsafe String");

            adversary = CreateAdversary(true);
            adversary.Profile();

            var subStringAction = new Action(() => testString.Substring(testString.Length / 2));

            adversary.Guess(() => testString.Substring(testString.Length / 2));

            Console.WriteLine("Safe String");

            adversary = CreateAdversary(false);
            adversary.Profile();
            var _testString = new _String(testString);
            adversary.Guess(() => _testString.Substring(testString.Length / 2));
        }

        private static Profiler<int> CreateAdversary(bool isLeaky)
        {
            List<string> testStrings = new List<string>();
            List<_String> _testStrings = new List<_String>();
            for (int i = 1; i <= 20; i++)
            {
                var sb = new StringBuilder("");
                for (int j = 0; j < i * 100; j++) sb.Append("HelloWorld");
                var testString = sb.ToString();

                testStrings.Add(testString);
                _testStrings.Add(new _String(testString));
            }

            Func<double, int> generateLow = startIndex => (int)startIndex;
            Action<int> test;

            if (isLeaky)
            {
                test = new Action<int>(index => testStrings[index].Substring(testStrings[index].Length / 2));
            }
            else
            {
                test = new Action<int>(index => _testStrings[index].Substring(_testStrings[index].Value.Length / 2));
            }

            var eqSet = new Dictionary<string, double>();
            for (int i = 1; i <= 10; i++)
            {
                eqSet.Add($"{i * 100 * 10} chars", i);
            }

            var adversary = new Profiler<int>(test, generateLow, i => i * 100 * 10, eqSet);
            return adversary;
        }
    }
}
