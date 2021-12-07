using System;

namespace SafeBlocks.Simulation
{
    public class Time
    {
        public static double DelayFactor()
        {
            switch (Config.ExecutionSpeed)
            {
                case ExecutionSpeed.ExtraSlow: return 100;
                case ExecutionSpeed.Slow: return 10;
                case ExecutionSpeed.Normal: return 1;
                case ExecutionSpeed.Fast: return 0.1;
                case ExecutionSpeed.ExtraFast: return 0.01;
            }

            throw new Exception("Unexpected");
        }
    }
}
