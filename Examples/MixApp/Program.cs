using System;
using System.Collections.Generic;
using System.Text;
using SafeBlocks.Analysis;
using SafeBlocks.String;

namespace Examples.String
{
    class Program
    {
        public static void StringExample()
        {
            var sb = new StringBuilder("");
            for (int i = 0; i < 1000; i++) sb.Append("HelloWorld");
            var testString = sb.ToString();

            Adversary<int> adversary;

            Console.WriteLine("Unsafe String");

            adversary = CreateAdversary(true);
            adversary.Calibrate();

            var subStringAction = new Action(() => testString.Substring(testString.Length / 2));

            adversary.Guess(() => testString.Substring(testString.Length / 2));

            Console.WriteLine("Safe String");

            adversary = CreateAdversary(false);
            adversary.Calibrate();
            var _testString = new _String(testString);
            adversary.Guess(() => _testString.Substring(testString.Length / 2));
        }

        private static Adversary<int> CreateAdversary(bool isLeaky)
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

            Func<int, int> generateVariant = startIndex => startIndex;
            Action<int> test;

            if (isLeaky)
            {
                test = new Action<int>(index => testStrings[index].Substring(testStrings[index].Length / 2));
            }
            else
            {
                test = new Action<int>(index => _testStrings[index].Substring(_testStrings[index].Value.Length / 2));
            }

            var adversary = new Adversary<int>(test, generateVariant, i => i * 100 * 10, new Range(1, 10), 1, 10);
            return adversary;
        }
    }
}
