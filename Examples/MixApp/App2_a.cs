using System;
using System.Threading;
using SafeBlocks;
using SafeBlocks.Statement;

namespace Examples.MixApp
{
    public class App1_a
    {
        private const int maxLow = 1000;
        private readonly int high;

        private __ program;

        public App1_a(int high) => this.high = high;

        public void Run()
        {
            program = P1();
            program.Do();
        }

        public Func<int> GetInput { get; set; }

        #region P1

        private __ P1() =>
            __._(() =>
            {
                var low = GetInput();

                if (low < high)
                {
                    P11(low);
                }
                else
                {
                    P12(low);
                }
            });

        #region P11

        private void P11(int low)
        {
            for (int i = 0; i < 2; i++)
            {
                Display("P111");

                for (int j = 1; j < low % 5; j++)
                {
                    if (low < high / 2)
                    {
                        P111();
                    }
                    else
                    {
                        P112(low);
                    }
                }
            }
        }

        #region 111

        private void P111()
        {
            Sleep("P111");
        }

        #endregion

        #region 112

        private void P112(int low)
        {
            if (low < 3 * high / 4)
            {
                P1121(low);
            }
            else
            {
                P1122();
            }
        }

        private void P1121(int low)
        {
            for (int i=0; i<low % 4; i++)
            {
                Thread.Sleep(1000);
            }
        }

        private void P1122()
        {
            Sleep("P1122");
        }

        #endregion

        #endregion

        #region P12

        private void P12(int low)
        {
            if (low < high * 2)
            {
                P121();
            }
            else
            {
                P122(low);
            }
        }

        #region P121

        private void P121()
        {
            Sleep("12");
        }

        #endregion

        #region P122

        private void P122(int low)
        {
            for (int i=0; i< low % 10; i++)
            {
                Sleep("P122");
            }
        }

        #endregion

        #endregion

        #endregion

        private static void Sleep(string path)
        {
            switch (Config.ExecutionSpeed)
            {
                case ExecutionSpeed.ExtraFast: Thread.Sleep(10); break;
                case ExecutionSpeed.Fast: Thread.Sleep(100); break;
                case ExecutionSpeed.Normal: Thread.Sleep(1000); break;
                case ExecutionSpeed.Slow: Thread.Sleep(10000); break;
                case ExecutionSpeed.ExtraSlow: Thread.Sleep(100000); break;
            }
            
            Display(path);
        }

        private static void Display(string path)
        {
            Console.WriteLine($"Path Executing: [{path}]");
        }
    }
}
