using SafeBlocks;

namespace Examples.MixApp
{
    public class App1_a : MixApp
    {
        public App1_a(int high) : base(high)
        {
            Program = P1();
        }

        public override void Run()
        {
            base.Run();
            Program.Do();
        }

        #region P1

        private _Action P1() =>
            _Action._(() =>
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
            Helper.Display("P11");

            for (int i = 0; i < 2; i++)
            {
                for (int j = 1; j < 5; j++)
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
            Helper.Sleep(10, "P111");
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
            for (int i=0; i < 4; i++)
            {
                Helper.Sleep(5, "P1121");
            }
        }

        private void P1122()
        {
            Helper.Sleep(5, "P1122");
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
            Helper.Sleep(10, "P112");
        }

        #endregion

        #region P122

        private void P122(int low)
        {
            for (int i=0; i < 20; i++)
            {
                if (low < 3 * high)
                {
                    P1221(low);
                }
                else
                {
                    P1222(low);
                }
            }
        }

        #region P1221

        private void P1221(int low)
        {
            Helper.Sleep(15, "P1221");
        }

        #endregion

        #region P1222

        private void P1222(int low)
        {
            Helper.Sleep(25, "P1222");
        }

        #endregion

        #endregion

        #endregion

        #endregion
    }
}
