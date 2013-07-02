using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.scriptFiend.IRC.Lines;

namespace org.scriptFiend.Modules.Channel.Admin
{
    public class Part : Module
    {
        private const string TAG = "~part";
        private new ChannelLine line { get; set; }

        public Part(ChannelLine chan) : base(chan)
        {
            line = chan;
        }

        public override bool run(string input)
        {
            if (line.Server.containsAdmin(line.Server.getUser(getUser(input))))
            {
                line.part();
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
