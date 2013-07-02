using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.scriptFiend.IRC.Lines;

namespace org.scriptFiend.Modules.CTCP
{
    public class Time : CTCPModule
    {
        private const string TAG = "time";
        public Time(CTCPLine line) : base(line)
        {
        }

        public override bool run(string input)
        {
            line.writeLine("TIME " + DateTime.Now.ToString());
            return true;
        }

        public override bool activate(string input)
        {
            return getMessage(input).Equals("time", StringComparison.OrdinalIgnoreCase);
        }
    }
}
