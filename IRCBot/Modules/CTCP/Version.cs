using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.scriptFiend.IRC.Lines;

namespace org.scriptFiend.Modules.CTCP
{
    class Version : CTCPModule 
    {
        public Version(CTCPLine line)
            : base(line, "version")
        {
        }

        public override bool run(string input)
        {
            string message = getMessage(input);
            if (message.Equals("version", StringComparison.OrdinalIgnoreCase))
            {
                line.writeLine("VERSION ScriptFiendClient:1.0:" + Environment.OSVersion.VersionString);
            }
            return true;
        }
    }
}
