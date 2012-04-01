using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.scriptFiend.IRC.Lines;

namespace org.scriptFiend.IRC
{
    class IRCUser
    {
        public IRCServer Server { get; set; }
        public List<ChannelLine> Channels { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Realname { get; set; }
        public string Hostname { get; set; }
        public Dictionary<ChannelLine, List<Char>> ChannelModes { get; set; }

        public static string[] Services = new string[] {"ps-akill", "ps-bopm", "ps-ctcp", "ps-dnsbl",
            "ps-mon", "BotServ", "ChanServ", "Global", "HostServ", "MemoServ", "NickServ", "OperServ", 
            "ps-reghelper", "netsec", "netsec2", "Trivia", "Internets", "Quotes"};

        public IRCUser(IRCServer server, string name)
        {
            Server = server;
            Name = name;
            Hostname = "";
            Channels = new List<ChannelLine>();
            ChannelModes = new Dictionary<ChannelLine,List<char>>();
            whois();
        }

        public void updateChannels()
        {
            whois();
        }

        private void whois()
        {
            foreach (string str in Services)
            {
                if (Name == str)
                {
                    return;
                }
            }
            Server.writeLine("WHOIS " + Name);
        }

        public PrivateLine getPrivateLine()
        {
            PrivateLine line = Server.getPrivateLine(Name);
            if (line == null)
            {
                Server.addPrivateLine(Name);
                line = Server.getPrivateLine(Name);
            }
            return line;
        }

        public CTCPLine getCTCPLine()
        {
            CTCPLine line = Server.getCTCPLine(Name);
            if (line == null)
            {
                Server.addCTCPLine(Name);
                line = Server.getCTCPLine(Name);
            }
            return line;
        }

        public static char? SymbolToMode(char symbol) 
        {
            switch (symbol)
            {
                case '+':
                    return 'v';
                case '%':
                    return 'h';
                case '@':
                    return 'o';
                case '&':
                    return 'a';
                case '~':
                    return 'q';
            }
            return null;
        }

        public string getModeString(ChannelLine chan)
        {
            string toReturn = "+";
            foreach (char character in ChannelModes[chan])
            {
                toReturn += character;
            }
            return toReturn;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}   
