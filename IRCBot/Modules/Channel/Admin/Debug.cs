using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.scriptFiend.IRC.Lines;
using org.scriptFiend.IRC;
using System.Threading;

namespace org.scriptFiend.Modules.Channel.Admin
{
    public class Debug : Module
    {
        private const string TAG = "~debug ";
        private new ChannelLine line { get; set; }

        public Debug(ChannelLine chan) : base(chan)
        {
            line = chan;
        }

        public override bool run(string input)
        {
            string command = removeTag(TAG, getMessage(input));
            string[] items = command.Split(' ');
            foreach (string item in items)
            {
                if(item.Equals("users")) {
                    foreach(IRCUser user in line.Users) {
                        line.writeLine(user.Name + ": " + user.Username + " (" + user.Realname + ") " + user.Hostname);
                        string chanString = "Channels:";
                        foreach (ChannelLine chan in user.Channels)
                        {
                            chanString += " " + chan.Name;
                        }
                        Thread.Sleep(3000);
                        line.writeLine(chanString);
                        Thread.Sleep(3000);
                    }
                }

                if(item.Equals("moderators")) {
                    String modString = "";
                    foreach(IRCUser user in line.Users) {
                        modString += line.userIsModerator(user) ? user.Name + "(" + user.getModeString(line) + ") " : "";
                    }
                    line.writeLine(modString);
                }
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
