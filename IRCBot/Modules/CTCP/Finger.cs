using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.scriptFiend.IRC.Lines;

namespace org.scriptFiend.Modules.CTCP
{
    public class Finger : CTCPModule
    {
        private const string TAG = "finger";
        public Finger(CTCPLine line) : base(line)
        {
        }

        public override bool run(string input)
        {
            line.writeLine("FINGER Author: Zalgo2462");
            return true;
        }

        public override bool activate(string input)
        {
            return getMessage(input).Equals("finger", StringComparison.OrdinalIgnoreCase);
        }
    }
}
