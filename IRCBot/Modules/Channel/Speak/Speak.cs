using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.scriptFiend.Modules;
using org.scriptFiend.IRC.Lines;

namespace org.scriptFiend.Modules.Channel.Speak
{
    class Speak : Module
    {
        public Speak(ChannelLine chan) : base (chan, "~speak ")
        {
        }

        public override bool run(string input)
        {
            string message = getMessage(input);
            string sayMessage = removeTag(ActivateTrigger, message);
            line.writeLine(sayMessage);
            return true;
        }
    }
}
