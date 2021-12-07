using SafeBlocks;
using SafeBlocks.Statement;

namespace Examples.MixApp
{
    public class App1_b : MixApp
    {
        public App1_b(int high) : base(high)
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
                    P11(low).Do();
                }
                else
                {
                    P12(low).Do();
                }
            });

        #region P11

        private _Action P11(int low) =>
            _Action._(() =>
            {
                Helper.Display("P11");

                for (int i = 0; i < 2; i++)
                {
                    _For._("L2", 1, 5, 10, new _Action<int>(j =>
                    {
                        if (low < high / 2)
                        {
                            P111();
                        }
                        else
                        {
                            P112(low).Do();
                        }
                    })).Do();
                }
            });

        #region 111

        private void P111() => Helper.Sleep(10, "P111");

        #endregion

        #region 112

        private _Block P112(int low) => _If._("P111", () => low < 3 * high / 4, P1121(low), P1122());

        private _For P1121(int low) =>
            _For._("L3", 0, 4, 10, _Action<int>._(i => Helper.Sleep(5, "P1121")));

        private _Action P1122() =>
            _Action._(() => Helper.Sleep(5, "P1122"));


        #endregion

        #endregion

        #region P12

        private _Action P12(int low) =>
            _Action._(() =>
            {
                if (low < high * 2)
                {
                    P121().Do();
                }
                else
                {
                    P122(low).Do();
                }
            });

        #region P121

        private _Action P121() =>
            _Action._(() => Helper.Sleep(10, "121"));

        #endregion

        #region P122

        private _Block P122(int low) =>
            _For._("L4", 0, 20, 20,
                _If._("P122", () => low < 3 * high, P1221(), P1222()));

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
