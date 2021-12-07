using System;
using System.Threading;
using SafeBlocks.App;
using SafeBlocks.Statement;

namespace SafeBlocks
{
    public class _If : _Block
    {
        private bool ifExecuted;
        private bool elseExecuted;
        private int ifDuration;
        private int elseDuration;
        
        private int averageDuration => (ifDuration + elseDuration) / 2;

        public Func<bool> Condition { private set; get; }

        public object IfAction { private set; get; }
        public object ElseAction { private set; get; }

        public _If(string id, Func<bool> condition, object ifAction, object elseAction,
            int tunedDuration = 0)
        {
            Id = id;
            this.Condition = condition;
            this.IfAction = ifAction;
            this.ElseAction = elseAction;

            if (tunedDuration != 0)
            {
                ifDuration = tunedDuration;
                elseDuration = tunedDuration;
            }
        }

        public static _If _(string id, Func<bool> condition, object ifAction, object elseAction,
            int tunedDuration = 0)
        {
            _If _;

            if (_App.ifs.ContainsKey(id))
            {
                _ = _App.ifs[id];
            }
            else
            {
                _ = new _If(id, condition, ifAction, elseAction, tunedDuration);
                _App.ifs.Add(id, _);
            }

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

        private void DoPartial()
        {
            if (Config.ExectuionMode == ExecutionMode.Debug)
                Console.WriteLine(this);

            var isTrue = Condition();

            if (isTrue)
            {
                if (!ifExecuted)
                {
                    ifExecuted = true;
                    stopWatch.Start();
                    Core.ExecuteAction<object>(IfAction);
                    stopWatch.Stop();
                    ifDuration = (int)stopWatch.ElapsedMilliseconds;
                }
                else if (ifExecuted && elseExecuted)
                {
                    Core.ExecuteAction<object>(IfAction);
                    Console.WriteLine($"If {Id} delaying {averageDuration}mS");
                    Thread.Sleep(rand.Next(averageDuration));
                }
                else
                {
                    Core.ExecuteAction<object>(IfAction);
                }
            }
            else
            {
                if (!elseExecuted)
                {
                    elseExecuted = true;
                    stopWatch.Start();
                    Core.ExecuteAction<object>(ElseAction);
                    stopWatch.Stop();
                    elseDuration = (int)stopWatch.ElapsedMilliseconds;
                }
                else if (ifExecuted && elseExecuted)
                {
                    Core.ExecuteAction<object>(ElseAction);

                    if (Config.ExectuionMode == ExecutionMode.Debug)
                    {
                        Console.WriteLine($"Else {Id} delaying {averageDuration}mS");
                    }

                    Thread.Sleep(rand.Next(averageDuration));
                }
                else
                {
                    Core.ExecuteAction<object>(ElseAction);
                }
            }
        }

        public void DoDefinite()
        {
            if (Config.ExectuionMode == ExecutionMode.Debug)
                Console.WriteLine(this);

            var isTrue = Condition();

            if (isTrue)
            {
                stopWatch.Start();
                Core.ExecuteAction<object>(IfAction);
                stopWatch.Stop();

                ifDuration = (int)Math.Max(ifDuration, stopWatch.ElapsedMilliseconds);
            }
            else
            {
                stopWatch.Start();
                Core.ExecuteAction<object>(ElseAction);
                stopWatch.Stop();

                elseDuration = (int)Math.Max(elseDuration, stopWatch.ElapsedMilliseconds);
            }

            if (Config.ExectuionMode == ExecutionMode.Debug)
            {
                Console.WriteLine($"If/Else {Id} delaying {averageDuration}mS");
            }

            Thread.Sleep((int)(Math.Max(ifDuration, elseDuration) - stopWatch.ElapsedMilliseconds));
        }
  

        public override string ToString() => $"If {Id} {Condition()}";
    }
}
