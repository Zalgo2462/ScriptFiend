using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.scriptFiend.IRC.Lines;

namespace org.scriptFiend.Modules.CTCP
{
    public class Version : CTCPModule 
    {
        private const string TAG = "version";
        public Version(CTCPLine line)  : base(line)
        {
        }

        public override bool run(string input)
        {
            line.writeLine("VERSION ScriptFiendClient:1.0:" + Environment.OSVersion.VersionString);
            return true;
        }

        public override bool activate(string input)
        {
            return getMessage(input).Equals("version", StringComparison.OrdinalIgnoreCase);
        }
    }
}
