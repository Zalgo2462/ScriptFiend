using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.scriptFiend.IRC.Lines;

namespace org.scriptFiend.Modules.CTCP
{
    class Time : CTCPModule
    {
        public Time(CTCPLine line)
            : base(line, "time")
        {
        }

        public override bool run(string input)
        {
            string message = getMessage(input);
            if (message.Equals("time", StringComparison.OrdinalIgnoreCase))
            {
                line.writeLine("TIME " + DateTime.Now.ToString());
            }
            return true;
        }
    }
}
