using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using SafeBlocks.Simulation;
using SafeBlocks.Statement;

namespace SafeBlocks
{
    public class _For : _Block
    {
        private int from;
        private int to;
        private int maxRange;
        private int index;

        public object Action { private set; get; }

        public _For(string id, int from, int to, int maxRange, Action action)
        {
            Id = id;
            this.from = from;
            this.to = to;
            this.maxRange = maxRange;
            this.Action = action;
        }

        public _For(string id, int from, int to, int maxRange, _Block action)
        {
            Id = id;
            this.from = from;
            this.to = to;
            this.maxRange = maxRange;
            this.Action = action;
        }

        public _For(string id, int from, int to, int maxRange, object action)
        {
            Id = id;
            this.from = from;
            this.to = to;
            this.maxRange = maxRange;
            this.Action = action;
        }

        public static _For _(string id, int from, int to, int maxRange, object action)
        {
            var _ = new _For(id, from, to, maxRange, action);
            return _;
        }

        public override void Do()
        {
            if (Config.SafetyMode == SafetyMode.Definite)
            {
                DoDefinite();
            }
            else
            {
                DoPartial();
            }
        }

        public void DoPartial()
        {
            if (Config.ExectuionMode == ExecutionMode.Debug)
                Console.WriteLine(this);

            var duration = new List<int>();
            for (index = from; index < to; index++)
            {
                if (Config.ExectuionMode == ExecutionMode.Debug)
                    Console.WriteLine($"{Id}.{index}");

                stopWatch.Restart();
                Core.ExecuteAction(Action, index);
                stopWatch.Stop();
                duration.Add((int)stopWatch.ElapsedMilliseconds);
            }

            if (Config.ExectuionMode == ExecutionMode.Debug)
                Console.WriteLine();

            var randomDelay = rand.Next((int)(duration.Average() * Time.DelayFactor()) + 1);

            if (Config.ExectuionMode == ExecutionMode.Debug)
            {
                Console.WriteLine($"{Id} delaying: {randomDelay}mS");
            }

            Thread.Sleep(randomDelay);
        }

        public void DoDefinite()
        {
            if (Config.ExectuionMode == ExecutionMode.Debug)
                Console.WriteLine(this);

            var duration = new List<int>();
            for (index = from; index < to; index++)
            {
                if (Config.ExectuionMode == ExecutionMode.Debug)
                    Console.Write(".");

                stopWatch.Restart();
                Core.ExecuteAction(Action, index);
                stopWatch.Stop();
                duration.Add((int)stopWatch.ElapsedTicks);
            }

            if (Config.ExectuionMode == ExecutionMode.Debug)
                Console.WriteLine();

            var iterationDuration = duration.Average() / TimeSpan.TicksPerMillisecond;

            if (maxRange > to - from)
            {
                var delay = ((maxRange - (to - from)) * iterationDuration) * Time.DelayFactor();

                if (Config.ExectuionMode == ExecutionMode.Debug)
                {
                    Console.WriteLine($"For delaying: {delay}mS");
                }

                Thread.Sleep((int)delay);
            }
        }

        public override string ToString() =>
            $"For {Id} {from}:{to}";     
    }
}
