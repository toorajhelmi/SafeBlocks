using System;
using SafeBlocks.App;

namespace SafeBlocks.Analysis
{
    public class TreeAnalyzer
    {
        public static bool IsSafePath(_App app)
        {
            _Block currentBlock = app.Program;

            while (true)
            {
                if (currentBlock == null)
                {
                    return true;
                }

                if (currentBlock is _Action _action)
                {
                    if (_action.Action is Action)
                    {
                        return false;
                    }
                    else
                    {
                        currentBlock = _action.Action as _Block;
                    }
                }

                if (currentBlock is _If _if)
                {
                    var nextBlock = _if.Condition() ? _if.IfAction : _if.ElseAction;
                    if (nextBlock is Action)
                    {
                        return false;
                    }
                    else
                    {
                        currentBlock = nextBlock as _Block;
                    }
                }

                if (currentBlock is _For _for)
                {
                    if (_for.Action is Action)
                    {
                        return false;
                    }
                    else
                    {
                        currentBlock = _for.Action as _Block;
                    }
                }
            }
        }
    }
}
