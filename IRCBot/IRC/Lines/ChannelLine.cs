using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.scriptFiend.Modules;

namespace org.scriptFiend.IRC.Lines
{
    class ChannelLine : Line
    {
        public IRCServer Server { get; set; }
        public string Name { get; set; }
        public List<string> Messages { get; set; }
        public List<Module> Modules { get; set; }
        public List<IRCUser> Users { get; set; }
        public List<char> Modes { get; set; }

        public ChannelLine(IRCServer server, string name) {
            this.Server = server;
            this.Name = name;            
            Messages = new List<string>();
            Users = new List<IRCUser>();
            Modes = new List<char>();
            Modules = new List<Module>();
            Modules.Add(new Modules.Channel.Poker.Poker(this));
            Modules.Add(new Modules.Channel.Speak.Speak(this));
            Modules.Add(new Modules.Channel.Renatus.Renatus(this));
            Modules.Add(new Modules.Channel.Admin.Debug(this));
            Modules.Add(new Modules.Channel.Admin.Kill(this));
            Modules.Add(new Modules.Channel.Admin.Part(this));
        }

        public void addUser(string name)
        {
            char[] chars = name.Substring(0, 1).ToCharArray();  
            if (Server.getUser(name) == null)
            {
                Server.addUser(name);
            }

            IRCUser user = Server.getUser(name);            

            if (!Users.Contains(user))
            {
                if (!user.ChannelModes.ContainsKey(this))
                {
                    user.ChannelModes.Add(this, new List<char>());
                }
                if (IRCUser.SymbolToMode(chars[0]) != null)
                {
                    if (!user.ChannelModes[this].Contains(IRCUser.SymbolToMode(chars[0]).Value))
                    {
                        user.ChannelModes[this].Add(IRCUser.SymbolToMode(chars[0]).Value);
                    }
                }
                Users.Add(user);
                Console.WriteLine(user + " added to user list for " + Name);
            }
        }

        public void removeUser(string sUser)
        {
            IRCUser user = Server.getUser(sUser);
            if (Users.Contains(user))
            {
                Users.Remove(user);
                Console.WriteLine(user + " removed from user list for " + Name);
            }
        }

        public bool userIsModerator(IRCUser user)
        {
            if (user.ChannelModes[this].Contains('v') || user.ChannelModes[this].Contains('h') ||
                user.ChannelModes[this].Contains('o') || user.ChannelModes[this].Contains('a') ||
                user.ChannelModes[this].Contains('q'))
            {
                return true;
            }
            return false;
        }

        public void join()
        {
            Server.writeLine("JOIN " + Name);
            Server.writeLine("NAMES " + Name);
            Server.writeLine("MODE " + Name);
        }

        public void writeLine(string str)
        {
            Server.writeLine("PRIVMSG " + Name + " :" + str);
        }

        public void part()
        {
            Server.writeLine("PART " + Name);
        }

        public void react(string line)
        {
            foreach(Module module in Modules) {
                module.react(line);
            }
            Messages.Add(line);
        }
    }
}
