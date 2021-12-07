using System.Threading;
using SafeBlocks;

namespace Examples.Wallet
{
    public class SafeWallet : Wallet
    {
        public SafeWallet(Key privateKey, Key publicKey, bool showMEssage = true)
            : base(privateKey, publicKey, showMEssage)
        {
        }

        protected override int ModularPow(int b, int exponent, int modulus, bool slowDown = false)
        {
            if (modulus == 1)
                return 0;

            int c = 1;

            new _For("MP" ,0, exponent, 10, new _Action<int>(i =>
            {
                if (slowDown) Thread.Sleep(delay);
                c = (c * b) % modulus;
            })).Do();        

            return c;
        }
    }
}
