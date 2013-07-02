using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.scriptFiend.IRC.Lines;

namespace org.scriptFiend.Modules.CTCP
{
    public class UserInfo : CTCPModule
    {
        private const string TAG = "userinfo";
        public UserInfo(CTCPLine line) : base(line)
        {
        }

        public override bool run(string input)
        {
            line.writeLine("USERINFO Author: Zalgo2462");
            return true;
        }

        public override bool activate(string input)
        {
            return getMessage(input).Equals("userinfo", StringComparison.OrdinalIgnoreCase);
        }
    }
}
