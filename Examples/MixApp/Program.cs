using System;
using System.Collections.Generic;
using SafeBlocks;
using SafeBlocks.Analysis;

namespace Examples.MixApp
{
    class Program
    {
        public static void MixAppExample()
        {
            Config.ExectuionMode = ExecutionMode.Release;
            Config.ExecutionSpeed = ExecutionSpeed.ExtraFast;

            //ProfileAndRunProgram(new App1_a(200), new App1_a(300));
            //ProfileAndRunProgram(new App1_b(200), new App1_b(300));
            ProfileAndRunProgram(new App1_c(200), new App1_c(300));
        }

        private static void ProfileAndRunProgram(MixApp profilingApp, MixApp executingApp)
        {
            Console.WriteLine($"\n\nProfiling {profilingApp.GetType().Name}");

            var adversary = CreateAdversary(profilingApp);
            adversary.Profile();

            var travesredPaths = new List<string>();

            var startTime = DateTime.Now;

            for (int low = 0; low < 1000; low += 50)
            {
                Console.WriteLine($"Running with low = {low}");

                var selectedPath = adversary.Guess(() =>
                {
                    executingApp.GetInput = () => low;
                    executingApp.Run();
                });

                if (!travesredPaths.Contains(selectedPath))
                {
                    travesredPaths.Add(selectedPath);
                    if (travesredPaths.Count == adversary.EquivalnceSet.Count)
                    {
                        break;
                    }
                }
            }

            Console.WriteLine("Execution Took " + (DateTime.Now - startTime).TotalMilliseconds / 20);
        }

        private static Profiler<int> CreateAdversary(MixApp profilingApp)
        {
            static int generateLow(double low) => (int)low;

            var test = new Action<int>(low =>
            {
                profilingApp.GetInput = () => low;
                profilingApp.Run();
            });

            //var equvalenceSet = new Dictionary<string, double> { { "P1221", 500 }, { "P1222", 700 } };
            var equvalenceSet = new Dictionary<string, double> { { "P111", 75 }, { "P1121", 125 }, { "P1122", 175 }, { "P121", 300 }, { "P1221", 500 }, { "P1222", 700 } };

            //var safeEquivalences = new List<string>();

            //foreach (var equivalence in equvalenceSet)
            //{
            //    profilingApp.GetInput = () => (int)equivalence.Value;
            //    if (TreeAnalyzer.IsSafePath(profilingApp))
            //    {
            //        safeEquivalences.Add(equivalence.Key);
            //        Console.WriteLine($"Not profiling equivalence ({equivalence.Key}, {equivalence.Value}) since its path is safe.");
            //    }
            //}

            //foreach (var safeEq in safeEquivalences)
            //{
            //    equvalenceSet.Remove(safeEq);
            //}

            var profiler = new Profiler<int>(test, generateLow, val => val, equvalenceSet)
            {
                MatchingCriteria = MatchingCriteria.Distance
            };

            return profiler;
        }
    }
}
