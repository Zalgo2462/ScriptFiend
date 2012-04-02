using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.scriptFiend.Modules;
using org.scriptFiend.Modules.Private;

namespace org.scriptFiend.IRC.Lines
{
    class PrivateLine : Line
    {
        public IRCServer Server { get; set; }
        public IRCUser User { get; set; }
        public List<string> Messages { get; set; }
        public List<Module> Modules { get; set; }

        public PrivateLine(IRCServer server, IRCUser user) {
            Server = server;
            User = user;
            Messages = new List<string>();
            Modules = new List<Module>();
            Modules.Add(new Login(this));
            Modules.Add(new Register(this));
        }

        public void writeLine(string str) 
        {
            Server.writeLine("PRIVMSG " + User.Name + " :" + str);
        }
        public void react(string line) 
        {
            foreach (Module module in Modules)
            {
                module.react(line);
            }
            Messages.Add(line);
        }
    }
}
