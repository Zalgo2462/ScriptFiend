using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.scriptFiend.IRC.Lines;

namespace org.scriptFiend.Modules.Channel.Admin
{
    public class Kill : Module
    {
        private const string TAG = "~kill";
        public Kill(ChannelLine chan) : base(chan)
        {
        }

        public override bool run(string input)
        {
            if (!line.Server.Client.kill())
            {
                line.writeLine("Failed to quit: " + line.Server.ADDRESS);
            }
            return true;
        }

        public override bool activate(string input)
        {
            return line.Server.containsAdmin(line.Server.getUser(getUser(input))) && 
                getMessage(input).StartsWith(TAG);
        }
    }
}
