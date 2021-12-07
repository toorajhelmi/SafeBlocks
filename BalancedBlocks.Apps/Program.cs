using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using static BalancedBlocks.Apps.Wallet;

namespace BalancedBlocks.Apps
{
    class Program
    {
        static void Main(string[] args)
        {
            // p = 61, q = 53, n = (61-1)(53-1) = 3233, e = 17, d = 413
            // var d = CalcD(3, 280);

            //var senderKeys = LoadKeys("storage1.txt");
            //var senderWallet = new LeakyWallet(senderKeys.Item1, senderKeys.Item2);

            //var receiverKeys = LoadKeys("storage2.txt");
            //var receiverWallet = new LeakyWallet(receiverKeys.Item1, receiverKeys.Item2);

            //var payload = senderWallet.SendTransaction(receiverWallet.PublicKey, 0.02);

            //receiverWallet.ReceiveTransaction(payload);

            //Console.ReadLine();

            var adversary = new Adversary();
            adversary.Attack();
        }

        private static (Key, Key) LoadKeys(string keyFile)
        {
            var keyInfo = File.ReadAllText($"/Users/thelmi/Projects/BalancedBlocks/BalancedBlocks.Apps/{keyFile}")
                .Split('|');
            return (new Key(int.Parse(keyInfo[0]), int.Parse(keyInfo[2])),
                new Key(int.Parse(keyInfo[1]), int.Parse(keyInfo[2])));
        }
    }
}
