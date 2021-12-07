using System;
using SafeBlocks;
using SafeBlocks.Statement;

namespace Examples.MixApp
{
    public class App1_b
    {
        private const int maxLow = 1000;
        private readonly int high;

        private __ program;

        public App1_b(int high) => this.high = high;

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

        private __ P11(int low) =>
            __._(() =>
            {
                Helper.Display("P11");

                for (int i = 0; i < 2; i++)
                {
                    _For._(1, 5, 10, new _Action<int>(j =>
                    {
                        if (low < high / 2)
                        {
                            P111();
                        }
                        else
                        {
                            P112(low);
                        }
                    }));
                }
            });

        #region 111

        private void P111() => Helper.Sleep(10, "P111");

        #endregion

        #region 112

        private void P112(int low) => _If._(() => low < 3 * high / 4, P1121(low), P1122());

        private _For P1121(int low) =>
            _For._(0, 4, 10, _Action<int>._(i => Helper.Sleep(5, "P1121")));

        private _Action P1122() =>
            _Action._(() => Helper.Sleep(5, "P1122"));


        #endregion

        #endregion

        #region P12

        private __ P12(int low) =>
            __._(() =>
            {
                if (low < high * 2)
                {
                    P121();
                }
                else
                {
                    P122(low);
                }
            });

        #region P121

        private __ P121() =>
            __._(() => Helper.Sleep(10, "121"));

        #endregion

        #region P122

        private void P122(int low) =>
            _For._(0, 20, 20, _Action<int>._(i =>
            {
                _If._(() => low < 3 * high, P1221(), P1222());
            }));

        #region P1221

        private _Action P1221() =>
            _Action._(() => Helper.Sleep(15, "P1221"));

        #endregion

        #region P1222

        private _Action P1222() =>
            _Action._(() => Helper.Sleep(25, "P1222"));

        #endregion

        #endregion

        #endregion

        #endregion
    }
}
