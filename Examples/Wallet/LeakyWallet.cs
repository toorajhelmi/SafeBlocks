using System;
using System.Threading;

namespace Examples.Wallet
{
    public class LeakyWallet : Wallet
    {
        // p = 61, q = 53, n = (61-1)(53-1) = 3233, e = 17, d = 413

        public LeakyWallet(Key privateKey, Key publicKey, bool showMEssage = true)
            : base(privateKey, publicKey, showMEssage)
        {
        }

        protected override int ModularPow(int b, int exponent, int modulus, bool slowDown = false)
        {
            if (modulus == 1)
                return 0;

            int c = 1;

            for (int e = 0; e <= exponent - 1; e++)
            {
                if (slowDown) Thread.Sleep(delay);
                c = (c * b) % modulus;
            }

            return c;
        }
    }
}
