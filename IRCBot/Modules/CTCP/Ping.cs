using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.scriptFiend.IRC.Lines;

namespace org.scriptFiend.Modules.CTCP
{
    class Ping : CTCPModule
    {
        public Ping(CTCPLine line)
            : base(line, "ping")
        {
        }

        public override bool run(string input)
        {
            string message = getMessage(input);
            if (message.Equals("ping", StringComparison.OrdinalIgnoreCase))
            {
                line.writeLine("PING " + DateTime.Now.ToString());
            }
            return true;
        }
    }
}
