using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MathNet.Numerics;


namespace SafeBlocks.Analysis
{
    public class Adversary<T>
    {
        private const int defaultAttempts = 5;
        private bool showMessage = false;

        private List<(int, long)> stats = new List<(int, long)>();
        private double slope;
        private double intercept;
        private Stopwatch stopWatch = new Stopwatch();
        private List<Action<T>> tests;
        private Func<int, T> generateVariant;
        private int attempts;
        private Range variantRange;

        public Adversary(List<Action<T>> tests, Func<int, T> generateVariant, Range? variantRange = null, int attempts = defaultAttempts)
        {
            this.tests = tests;
            this.generateVariant = generateVariant;
            this.attempts = attempts;
            this.variantRange = variantRange ?? new Range(1, 10);
        }

        public void Calibrate()
        {
            Console.WriteLine("Calibrating Adversary");
            var random = new Random();

            for (int varient = variantRange.Start.Value; varient <= variantRange.End.Value; varient++)
            {
                long totalTime = 0;
                var variant = generateVariant(varient);

                if (showMessage)
                {
                    Console.WriteLine($"Measuring Varient {varient}");
                }

                for (int attempt = 1; attempt <= attempts; attempt++)
                {
                    stopWatch.Restart();
                    tests[random.Next(tests.Count)](variant);
                    stopWatch.Stop();

                    totalTime += stopWatch.ElapsedMilliseconds;

                    Console.Write(".");
                }

                var averateElapsed = totalTime / (variantRange.End.Value - variantRange.Start.Value);
                Console.WriteLine($": {averateElapsed}mS");

                stats.Add((varient, averateElapsed));
            }

            Anaylze();
        }

        private void Anaylze()
        {
            double[] xdata = stats.Select(s => (double)s.Item2).ToArray();
            double[] ydata = stats.Select(s => (double)s.Item1).ToArray();

            Tuple<double, double> p = Fit.Line(xdata, ydata);
            intercept = p.Item1; 
            slope = p.Item2;

            Console.WriteLine($"Adversary is ready.");
        }

        public void StartGuessing()
        {
            stopWatch.Restart();
        }

        public void EndGuessing()
        {
            stopWatch.Stop();

            var key = slope * stopWatch.ElapsedMilliseconds + intercept;

            Console.WriteLine($"Guessed Key: {(int)key}±3");
        }
    }
}
