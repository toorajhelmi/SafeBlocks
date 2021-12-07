using System.Collections.Generic;
using SafeBlocks.Statement;

namespace SafeBlocks.App
{
    public abstract class _App
    {
        public virtual void Run()
        {
            ifs.Clear();
        }

        internal static Dictionary<string, _If> ifs = new();

        public _Action Program { protected set; get; }
    }
}
