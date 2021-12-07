using System;
using System.Threading;

namespace UnsafeProgram
{
    public class UnsafeCode
    {
        public void Start()
        {
            SideChannelLeak(100);
        }

        private bool SideChannelLeak(int key)
        {
            for (int i = 0; i<key; i++)
            {
                KeyDependentAction(key);
                CallSlowApi();

                if (!Condition(key))
                    return false;
            }

            return true;
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
