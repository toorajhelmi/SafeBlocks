using System;
using System.Collections.Generic;
using System.IO;
using SafeBlocks;
using SafeBlocks.Analysis;
using static Examples.Wallet.Wallet;

namespace Examples.Wallet
{
    class Program
    {
        private static Profiler<Wallet> adversary;
        private static bool withAttack = true;

        public static void WalletExample()
        {
            Config.ExectuionMode = ExecutionMode.Release;
            Config.ExecutionSpeed = ExecutionSpeed.ExtraFast;

            var senderKeys = LoadKeys("Storage1.txt");
            var receiverKeys = LoadKeys("Storage2.txt");

            var senderWallet_Leaky = new LeakyWallet(senderKeys.Item1, senderKeys.Item2);
            var receiverWallet_Leaky = new LeakyWallet(receiverKeys.Item1, receiverKeys.Item2);

            adversary = CreateAdversary(true);
           
            Console.WriteLine("Sending 0.02 Coins using Leaky Wallet");

            SendTransaction(senderWallet_Leaky, receiverWallet_Leaky, 0.02);

            //Console.WriteLine("Please Enter to continue ...");
            //Console.ReadLine();

            adversary = CreateAdversary(false);
            var senderWallet_Safe = new SafeWallet(senderKeys.Item1, senderKeys.Item2);
            var receiverWallet_Safe = new SafeWallet(receiverKeys.Item1, receiverKeys.Item2);

            Console.WriteLine("Sending 0.02 Coins using Safe Wallet");

            SendTransaction(senderWallet_Safe, receiverWallet_Safe, 0.02);

            Console.ReadLine();
        }

        private static Profiler<Wallet> CreateAdversary(bool isLeaky)
        {
            Func<double, Wallet> generateLow = priKey => isLeaky ?
                new LeakyWallet(new Key((int)priKey, 3233), new Key(1, 3233), false) :
                new SafeWallet(new Key((int)priKey, 3233), new Key(1, 3233), false);

            var test = new Action<Wallet>(senderWallet => senderWallet.SendTransaction(new Key(1, 1), 0.02));

            var eqSet = new Dictionary<string, double>();
            for (int i = 1; i<=10; i++)
            {
                eqSet.Add($"PK = {i}", i);
            }

            var adversary = new Profiler<Wallet>(test, generateLow, key => key, eqSet);

            return adversary;
        }

        private static void SendTransaction(Wallet from, Wallet to, double amount)
        {
            if (withAttack) adversary.Profile();

            adversary.Guess(() => from.SendTransaction(to.PublicKey, amount));
        }

        private static (Key, Key) LoadKeys(string keyFile)
        {
            var keyInfo = File.ReadAllText($"/Users/thelmi/Projects/SafeBlocks/Examples/Wallet/{keyFile}")
                .Split('|');
            return (new Key(int.Parse(keyInfo[0]), int.Parse(keyInfo[2])),
                new Key(int.Parse(keyInfo[1]), int.Parse(keyInfo[2])));
        }
    }
}
