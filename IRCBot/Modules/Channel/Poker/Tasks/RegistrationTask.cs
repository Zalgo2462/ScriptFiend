using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using org.scriptFiend.Modules.Util;

namespace org.scriptFiend.Modules.Channel.Poker.Tasks
{
    class RegistrationTask : LoopTask
    {
        Poker Parent { get; set; }

        public RegistrationTask(Poker p)
        {
            Parent = p;
        }

        public bool activate()
        {
            return Parent.getMillisToRegistrationEnd() > 0;      
        }

        public int task()
        {
            return 500;            
        }

    }
}
