using System;
using System.Threading;
using SafeBlocks;

namespace Examples.MixApp
{
    public class Helper
    {
        public static void Sleep(int mS, string path)
        {
            Thread.Sleep(mS);
            //Thread.Sleep((int)(mS * Time.DelayFactor()));
            Display(path);
        }

        public static void Display(string path)
        {
            if (Config.ExectuionMode == ExecutionMode.Debug)
            {
                Console.WriteLine($"Path Executing: [{path}]");
            }
        }
    }
}
