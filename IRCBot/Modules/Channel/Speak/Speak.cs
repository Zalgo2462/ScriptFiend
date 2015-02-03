using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.scriptFiend.Modules;
using org.scriptFiend.IRC.Lines;

namespace org.scriptFiend.Modules.Channel.Speak
{
    public class Speak : Module
    {
        private const string TAG = "~speak";
        public Speak(ChannelLine chan) : base (chan)
        {
        }

        public override void run(string input)
        {
            string message = getMessage(input);
            string sayMessage = removeTag(TAG, message);
            line.writeLine(sayMessage);
        }

        public override bool activate(string input)
        {
            return getMessage(input).StartsWith(TAG);
        }
    }
}
