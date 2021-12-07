using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using MathNet.Numerics;


namespace SafeBlocks.Analysis
{
    public enum MatchingCriteria
    {
        Regression,
        Distance
    }

    public class Profiler<T>
    {
        private List<(string, double, double)> stats = new();
        private double slope;
        private double intercept;
        private double std = 0;
        private Stopwatch stopWatch = new Stopwatch();
        private int increments;
        private int attempts;

        private Dictionary<string, double> variants;
        private Func<double, T> generateLow;
        private Func<double, double> measure;
        private Action<T> test;

        public MatchingCriteria MatchingCriteria { get; set; } = MatchingCriteria.Regression;

        public double ProfilingDuration { get; set; }

        public List<(string, double, double)> EquivalnceSet { get; set; } = new();

        public Profiler(Action<T> test, Func<double, T> generateLow, Func<double, double> measure,
            Dictionary<string, double> variants,
            int increments = 1, int attempts = 5)
        {
            this.test = test;
            this.generateLow = generateLow;
            this.measure = measure;
            this.increments = increments;
            this.attempts = attempts;
            this.variants = variants;
        }

        public void Profile(bool unsafeOnly = false)
        {
            var defaultLatency = GCSettings.LatencyMode;

            GCSettings.LatencyMode = GCLatencyMode.LowLatency;

            Console.WriteLine("\nProfiling Started...");
            var random = new Random();

            var profilingStartAt = DateTime.Now;

            foreach (var variant in variants)
            {
                Console.Write($"[Var ({variant.Key},{variant.Value})]:");

                var low = generateLow(variant.Value);

                var times = new List<double>();

                for (int attempt = 1; attempt <= attempts; attempt++)
                {
                    stopWatch.Restart();
                    test(low);
                    stopWatch.Stop();

                    times.Add(stopWatch.ElapsedTicks);

                    Console.Write(".");
                }

                //Take the median
                var averateElapsed = times.OrderBy(t => t).ElementAt(times.Count / 2);

                Console.WriteLine($": {averateElapsed:0.##} Ticks");

                stats.Add((variant.Key, measure(variant.Value), averateElapsed));
            }

            ProfilingDuration = (DateTime.Now - profilingStartAt).TotalMilliseconds;

            GCSettings.LatencyMode = defaultLatency;


            Anaylze();
        }

        private void Anaylze()
        {
            if (MatchingCriteria == MatchingCriteria.Regression)
            {
                var xdata = stats.Select(s => s.Item3).ToArray();
                var ydata = stats.Select(s => s.Item2).ToArray();

                var x = new List<double>();
                var y = new List<double>();

                RemoveOutliers(xdata, x, ydata, y);

                for (int i = 0; i < x.Count; i++)
                {
                    var id = stats.First(s => s.Item2 == y[i]).Item1;
                    EquivalnceSet.Add((id, x[i], y[i]));
                }

                stats.Clear();

                Tuple<double, double> fit = Fit.Line(x.ToArray(), y.ToArray());
                intercept = fit.Item1;
                slope = fit.Item2;

                var sse = 0.0;

                for (int i = 0; i < x.Count; i++)
                {
                    var yHat = slope * x[i] + intercept;
                    var error = y[i] - yHat;
                    sse += Math.Pow(error, 2);
                }

                std = Math.Sqrt(sse / (x.Count - 2));
            }
            else
            {
                EquivalnceSet.AddRange(stats);
            }

            Console.WriteLine($"Profiler is ready. Profiling took {ProfilingDuration}mS");
        }

        private static void RemoveOutliers(double[] xdata, List<double> x, double[] ydata = null, List<double> y = null)
        {
            x.Clear();
            var x_ave = xdata.Average();
            var x_std = Math.Sqrt(xdata.Sum(d => Math.Pow(d - x_ave, 2) / xdata.Length));

            for (int i = 0; i < xdata.Length; i++)
            {
                var z = (xdata[i] - x_ave) / x_std;
                if (Math.Abs(z) < 1)
                {
                    x.Add(xdata[i]);

                    if (ydata != null)
                    {
                        y.Add(ydata[i]);
                    }
                }
                else
                {
                    Debug.WriteLine($"Outlier removed {i}: {xdata[i]}");
                }
            }
        }

        public string Guess(Action action)
        {
            return MatchingCriteria == MatchingCriteria.Regression ?
                GuessByRegression(action) :
                GuessByDistance(action);
        }

        private string GuessByRegression(Action action)
        {
            List<double> durations = new List<double>();

            for (int i = 0; i < 5; i++)
            {
                stopWatch.Restart();
                action();
                stopWatch.Stop();
                durations.Add(stopWatch.ElapsedTicks);
            }

            RemoveOutliers(durations.ToArray(), durations);
            var duration = Median(durations);
            var guessed = $"{duration * slope + intercept: 0.##}±{std:0.##}";

            Console.WriteLine("\n==========================");
            Console.WriteLine($"Guessed Parameter:");
            Console.WriteLine($"Value: {guessed}");
            Console.WriteLine($"Duration: {duration} Ticks");
            Console.WriteLine("==========================\n");

            return guessed;
        }

        private string GuessByDistance(Action action)
        {
            List<double> duration = new List<double>();

            for (int i = 0; i < 5; i++)
            {
                stopWatch.Restart();
                action();
                stopWatch.Stop();
                duration.Add(stopWatch.ElapsedTicks);
            }

            var measured = Median(duration);
            var minDistance = double.MaxValue;
            var selectedEquivalance = "";

            foreach (var equivalence in EquivalnceSet)
            {
                if (Math.Abs(equivalence.Item3 - measured) < minDistance)
                {
                    minDistance = Math.Abs(equivalence.Item3 - measured);
                    selectedEquivalance = equivalence.Item1;
                }
            }

            Console.WriteLine("\n==========================");
            Console.WriteLine($"Guessed Parameter:");
            Console.WriteLine($"Value: {measured:0.##}");
            Console.WriteLine($"Equivalence ID: {selectedEquivalance}");
            Console.WriteLine("==========================\n");

            return selectedEquivalance;
        }

        private double Median(IEnumerable<double> values)
        {
            return values.OrderBy(v => v).ElementAt(values.Count() / 2);
        }
    }
}
