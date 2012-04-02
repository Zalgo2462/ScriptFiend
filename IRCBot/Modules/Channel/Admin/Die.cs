using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.scriptFiend.IRC.Lines;

namespace org.scriptFiend.Modules.Channel.Admin
{
    class Kill : Module
    {
        public Kill(ChannelLine chan)
            : base(chan, "~kill")
        {
        }

        public override bool run(string input)
        {
            if (line.Server.containsAdmin(line.Server.getUser(getUser(input))))
            {
                if (!line.Server.Client.kill())
                {
                    line.writeLine("Failed to quit: " + line.Server.ADDRESS);
                }
            }
            return true;
        }
    }
}
