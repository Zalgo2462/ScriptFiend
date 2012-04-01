using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.scriptFiend.Modules.Util
{
    interface LoopTask
    {
        bool activate();
        int task();
    }
}
