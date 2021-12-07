using System;
using SafeBlocks.App;

namespace Examples.MixApp
{
    public abstract class MixApp : _App
    {
        protected const int maxLow = 1000;
        protected readonly int high;

        public MixApp(int high) => this.high = high;

        public Func<int> GetInput { get; set; }
    }
}
