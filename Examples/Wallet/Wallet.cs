using System;
using System.Collections.Generic;

namespace Examples.Wallet
{
    public abstract class Wallet
    {
        protected const int delay = 1; //mS

        public class Key
        {
            public int Exponent { get; set; }
            public int Modulus { get; set; }

            public Key (int exponent, int modulus)
            {
                Exponent = exponent;
                Modulus = modulus;
            }

            public override string ToString() => $"{Exponent},{Modulus}";
        }

        protected Key privateKey; 
        public Key PublicKey { get; set; }
        public bool ShowMessages { get; set; } = true;

        public Wallet(Key privateKey, Key publicKey, bool showMessage = true)
        {
            this.privateKey = privateKey;
            PublicKey = publicKey;
            ShowMessages = showMessage;
        }

        public string SendTransaction(Key to, double amount)
        {
            var transaction = $"{PublicKey}|{to}|{amount}";
            var signature = Encrypt(transaction, privateKey);
            var payload = $"{transaction}:{signature}";

            if (ShowMessages)
            {
                Console.WriteLine($"{PublicKey} ==> Sent {payload}");
                Console.WriteLine();
            }

            return payload;
        }

        public void ReceiveTransaction(string payload)
        {
            if (ShowMessages)
            {
                Console.WriteLine($"{PublicKey} ==> Recevied: {payload}");
            }

            var transaction = payload.Split(':')[0];
            var signature = payload.Split(':')[1];
            var senderAddress = transaction.Split('|')[0];
            var senderKey = new Key(int.Parse(senderAddress.Split(',')[0]), int.Parse(senderAddress.Split(',')[1]));

            var decrypted = Decrypt(signature, senderKey);

            if (ShowMessages)
            {
                Console.WriteLine($"{PublicKey} ==> Decrypted Transaction: {decrypted}");

                if (decrypted == transaction)
                {
                    Console.WriteLine($"{PublicKey} ==> Success");
                }
                else
                {
                    Console.WriteLine($"{PublicKey} ==> Failure");
                }
            }
        }

        public string Encrypt(string payload, Key key)
        {
            var encrypted = new List<int>();
            foreach (var c in payload)
            {
                encrypted.Add(ModularPow(c, key.Exponent, key.Modulus, true));
            }

            return string.Join("|", Array.ConvertAll(encrypted.ToArray(), e => e.ToString()));
        }

        public string Decrypt(string payload, Key key)
        {
            var decrypted = "";

            foreach (var c in payload.Split('|'))
            {
                var d  = ModularPow(int.Parse(c), key.Exponent, key.Modulus);
                decrypted += (char)d;
            }

            return decrypted;
        }

        protected abstract int ModularPow(int b, int exponent, int modulus, bool slowDown = false);
    }
}
