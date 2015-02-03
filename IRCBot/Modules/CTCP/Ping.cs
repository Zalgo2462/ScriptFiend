using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.scriptFiend.IRC.Lines;

namespace org.scriptFiend.Modules.CTCP
{
    public class Ping : CTCPModule
    {
        private const string TAG = "ping";
        public Ping(CTCPLine line) : base(line)
        {
        }

        public override void run(string input)
        {
            line.writeLine("PING " + DateTime.Now.ToString());
        }

        public override bool activate(string input)
        {
            return getMessage(input).Equals("ping", StringComparison.OrdinalIgnoreCase);
        }
    }
}
