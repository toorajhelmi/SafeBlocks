using System;
using SafeBlocks.Statement;

namespace SafeBlocks
{
    public class _Action : _Block
    {
        public object Action { get; set; }

        public _Action(object action) => Action = action;

        public override void Do()
        {
            Core.ExecuteAction<object>(Action);
        }

        public static _Action _(Action action)
        {
            var _ = new _Action(action);
            return _;
        }
    }

    public class _Action<T> : _Block<T>
    {
        public object Action { private set; get; }

        public _Action(Action<T> action) => this.Action = action;

        public static _Action<T> _(Action<T> action) => new _Action<T>(action);

        public override void Do(T parameter)
        {
            Core.ExecuteAction(Action, parameter);
        }
    }
}
