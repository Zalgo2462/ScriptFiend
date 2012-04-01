using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.scriptFiend.IRC.Lines;

namespace org.scriptFiend.Modules.CTCP
{
    class UserInfo : CTCPModule
    {
        public UserInfo(CTCPLine line)
            : base(line, "userinfo")
        {
        }

        public override bool run(string input)
        {
            string message = getMessage(input);
            if (message.Equals("userinfo", StringComparison.OrdinalIgnoreCase))
            {
                line.writeLine("USERINFO Author: Zalgo2462");
            }
            return true;
        }
    }
}
