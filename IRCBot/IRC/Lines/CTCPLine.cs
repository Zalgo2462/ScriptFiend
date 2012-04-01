using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.scriptFiend.Modules;

namespace org.scriptFiend.IRC.Lines
{
    class CTCPLine : Line
    {
        public IRCServer Server { get; set; }
        public IRCUser User { get; set; }
        public List<string> Messages { get; set; }
        public List<Module> Modules { get; set; }
        byte control = 1;

        public CTCPLine(IRCServer server, IRCUser user) {
            Server = server;
            User = user;
            Messages = new List<string>();
            Modules = new List<Module>();
            Modules.Add(new Modules.CTCP.Finger(this));
            Modules.Add(new Modules.CTCP.Ping(this));
            Modules.Add(new Modules.CTCP.Time(this));
            Modules.Add(new Modules.CTCP.UserInfo(this));
            Modules.Add(new Modules.CTCP.Version(this));
        }

        public void writeLine(string str) 
        {
            Server.writeLine("NOTICE " + User.Name + " :" + (char)control + str + (char)control);
        }
        public void react(string line) {
            foreach (Module module in Modules)
            {
                module.react(line);
            }
            Messages.Add(line);
        }
    }
}
