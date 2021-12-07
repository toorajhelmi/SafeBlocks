using System;
using System.Diagnostics;

namespace SafeBlocks
{
    public abstract class _Block 
    {
        public string Id { get; set; }

        protected Random rand = new Random();
        protected Stopwatch stopWatch = new Stopwatch();

        public abstract void Do();
    }

    public abstract class _Block<T>
    {
        public string Id { get; set; }

        protected Random rand = new Random();
        protected Stopwatch stopWatch = new Stopwatch();

        public abstract void Do(T param);
    }
}
