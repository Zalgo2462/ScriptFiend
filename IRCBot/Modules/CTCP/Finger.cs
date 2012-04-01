using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.scriptFiend.IRC.Lines;

namespace org.scriptFiend.Modules.CTCP
{
    class Finger : CTCPModule
    {
        public Finger(CTCPLine line)
            : base(line, "finger")
        {
        }

        public override bool run(string input)
        {
            string message = getMessage(input);
            if (message.Equals("finger", StringComparison.OrdinalIgnoreCase))
            {
                line.writeLine("FINGER Author: Zalgo2462");
            }
            return true;
        }
    }
}
