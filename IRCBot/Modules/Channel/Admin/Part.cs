using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.scriptFiend.IRC.Lines;

namespace org.scriptFiend.Modules.Channel.Admin
{
    class Part : Module
    {
        new ChannelLine line { get; set; }

        public Part(ChannelLine chan)
            : base(chan, "~part")
        {
            line = chan;
        }

        public override bool run(string input)
        {
            if (getUser(input).Equals(IrcBot.admin))
            {
                line.part();
            }
            return true;
        }
    }
}
