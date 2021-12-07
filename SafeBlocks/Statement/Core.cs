using System;
namespace SafeBlocks.Statement
{
    public class Core
    {
        public static void ExecuteAction<T>(object action, T parameter = default(T))
        {
            if (action is _Block<T>)
            {
                (action as _Block<T>).Do(parameter);
            }
            else if (action is _Block)
            {
                (action as _Block).Do();
            }
            else if (action is Action<T>)
            {
                (action as Action<T>)(parameter);
            }
            else 
            {
                (action as Action)();
            }
        }
    }
}
