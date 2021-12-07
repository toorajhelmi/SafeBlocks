using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MathNet.Numerics;
using static SafeBlocks.Apps.Wallet;

namespace SafeBlocks.Apps
{
    public class Adversary
    {
        private bool showMessage = false;

        private List<(int, long)> stats = new List<(int, long)>();
        private double slope;
        private double intercept;
        private Stopwatch stopWatch = new Stopwatch();

        public Adversary()
        {
        }

        public void Initialize()
        {
            Console.WriteLine("Adversary Initializing");

            int attempts = 5;
            for (int i = 1; i <= attempts; i++)
            {
                if (showMessage)
                {
                    Console.WriteLine($"Measuring {i}/{attempts}");
                }

                long totalTime = 0;

                for (int j = 1; j <= 10; j++)
                {
                    var senderWallet = new LeakyWallet(new Key(i, 3233), new Key(1, 3233));
                    senderWallet.ShowMessages = false;

                    stopWatch.Restart();
                    senderWallet.SendTransaction(new Key(1, 1), 0.02);
                    stopWatch.Stop();

                    totalTime += stopWatch.ElapsedMilliseconds;
                }

                stats.Add((i, totalTime / 10));
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
