using System;
using System.Threading;

namespace UnsafeProgram
{
    public class SafeCode
    {
        public void Start()
        {
            SideChannelLeak(100);
        }

        private bool SideChannelLeak(int key)
        {
            var result = true;

            for (int i = 0; i < key; i++)
            {
                KeyDependentAction(key);
                CallSlowApi();

                if (!Condition(key))
                    result = false;
            }

            return result;
        }

        private void KeyDependentAction(int key)
        {
            Thread.Sleep(10);
        }

        private void CallSlowApi()
        {
            Thread.Sleep(100000);
        }

        private bool Condition(int value)
        {
            return false;
        }
    }
}
