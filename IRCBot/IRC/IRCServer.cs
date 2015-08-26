using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using org.scriptFiend.SQL;
using org.scriptFiend.IRC.Lines;

namespace org.scriptFiend.IRC
{
    public class IRCServer
    {
        public string ADDRESS;
        public int PORT;

        private bool alive;

        public bool Alive {
            get
            {
                lock (switchLock)
                {
                    return alive;
                }
            }
            set
            {
                lock (switchLock)
                {
                    alive = value;
                }
            }
        }

        public IRCClient Client { get; set; }

        public List<ChannelLine> JoinedChannels { get; set; }

        public List<ChannelLine> ChannelBank { get; set; }

        public List<PrivateLine> PrivateLines { get; set; }

        public List<CTCPLine> CTCPLines { get; set; }

        public List<String> Messages { get; set; }

        public List<IRCUser> Users { get; set; }

        public List<IRCUser> Admins { get; set; }

        public List<IRCUser> RegisterQueue { get; set; }
        
        private StreamWriter Writer { get; set; }

        private StreamReader Reader { get; set; }

        private TcpClient TCP { get; set; }

        private Thread MessageListener { get; set; }

        private Object readLock = new Object();

        private Object writeLock = new Object();

        private Object switchLock = new Object();


        public IRCServer(string add, int port, IRCClient cl) {
            this.ADDRESS = add;
            this.PORT = port;
            this.Client = cl;
            this.JoinedChannels = new List<ChannelLine>();
            this.ChannelBank = new List<ChannelLine>();
            this.PrivateLines = new List<PrivateLine>();
            this.CTCPLines = new List<CTCPLine>();
            this.Users = new List<IRCUser>();
            this.Admins = new List<IRCUser>();
            this.RegisterQueue = new List<IRCUser>();
            this.Alive = true;

            this.TCP = new TcpClient(ADDRESS, PORT);
            this.Reader = new StreamReader(TCP.GetStream());
            this.Writer = new StreamWriter(TCP.GetStream());
            this.Messages = new List<string>();

            MessageListener = new Thread(new ThreadStart(this.checkMessages));
            MessageListener.Start();            
        }

        internal void writeLine(string str)
        {
            lock (writeLock)
            {
                Writer.WriteLine(str);
                Writer.Flush();
            }
        }

        internal string readLine()
        {
            lock (readLock)
            {
                try
                {
                    return Reader.ReadLine();
                }
                catch (IOException ignored)
                {
                    return null;
                }
            }
        }

        public void login(string userString, string nick)
        {
            writeLine(userString);
            Console.WriteLine(userString);
            writeLine("NICK " + nick);
            Console.WriteLine("NICK " + nick);     
        }

        public void close()
        {
            this.Reader.Close();
            this.Writer.Close();
            this.TCP.Close();
        }

        public bool kill()
        {            
            saveChannels();
            foreach(ChannelLine chan in JoinedChannels) {
                chan.writeLine("ScriptFiend has been killed");
            }
            Alive = false;
            close();
            Client.IM.kill();            
            MessageListener.Abort();            
            return !MessageListener.IsAlive;
        }

        public void loadChannels()
        {
            foreach(string chan in DBC.getChannels(ADDRESS)) {
                if (!containsChannel(chan))
                {
                    addChannel(chan);
                }
            }
        }

        public void saveChannels()
        {
            foreach(ChannelLine chan in JoinedChannels) {
                DBC.addChannel(chan.Name, ADDRESS);
            }
        }

        public ChannelLine getChannelFromBank(string name)
        {
            foreach (ChannelLine channel in ChannelBank)
            {
                if (channel.Name == name)
                {
                    return channel;
                }
            }
            return null;
        }

        public bool addChannel(string name)
        {
            if (!containsChannel(name))
            {
                ChannelLine newChannel;
                if((newChannel = getChannelFromBank(name)) == null) {
                    newChannel = new ChannelLine(this, name);
                    ChannelBank.Add(newChannel);
                }
                JoinedChannels.Add(newChannel);  
                newChannel.join();                              
                return true;
            }
            return false;
        }

        public bool removeChannel(string name)
        {
            if (containsChannel(name))
            {
                ChannelLine oldChannel = getChannel(name);
                oldChannel.part();
                JoinedChannels.Remove(oldChannel);
                return true;
            }
            return false;
        }

        public bool containsChannel(string name)
        {
            foreach (ChannelLine channel in JoinedChannels)
            {
                if (channel.Name == name)
                {
                    return true;
                }
            }
            return false;
        }

        public ChannelLine getChannel(string name)
        {
            foreach (ChannelLine channel in JoinedChannels)
            {
                if (channel.Name == name)
                {
                    return channel;
                }
            }
            return null;
        }



        public bool addPrivateLine(string user)
        {
            if (!containsPrivateLine(user))
            {
                PrivateLine newLine = new PrivateLine(this, getUser(user));
                PrivateLines.Add(newLine);
                return true;
            }
            return false;
        }

        public bool removePrivateLine(string user)
        {
            if (containsPrivateLine(user))
            {
                PrivateLine oldLine = getPrivateLine(user);
                PrivateLines.Remove(oldLine);
                return true;
            }
            return false;
        }

        public bool containsPrivateLine(string user)
        {
            foreach (PrivateLine line in PrivateLines)
            {
                if (line.User.Name == user)
                {
                    return true;
                }
            }
            return false;
        }

        public PrivateLine getPrivateLine(string user)
        {
            foreach (PrivateLine line in PrivateLines)
            {
                if (line.User.Name == user)
                {
                    return line;
                }
            }
            return null;
        }



        public bool addCTCPLine(string user)
        {
            if (!containsCTCPLine(user))
            {
                CTCPLine newLine = new CTCPLine(this, getUser(user));
                CTCPLines.Add(newLine);
                return true;
            }
            return false;
        }

        public bool removeCTCPLine(string user)
        {
            if (containsCTCPLine(user))
            {
                CTCPLine oldLine = getCTCPLine(user);
                CTCPLines.Remove(oldLine);
                return true;
            }
            return false;
        }

        public bool containsCTCPLine(string user)
        {
            foreach (CTCPLine line in CTCPLines)
            {
                if (line.User.Name == user)
                {
                    return true;
                }
            }
            return false;
        }

        public CTCPLine getCTCPLine(string user)
        {
            foreach (CTCPLine line in CTCPLines)
            {
                if (line.User.Name == user)
                {
                    return line;
                }
            }
            return null;
        }



        public bool addUser(string name)
        {
            try 
            {
                if (name.StartsWith("+") || name.StartsWith("%") || name.StartsWith("@") || name.StartsWith("&") ||
                    name.StartsWith("~"))
                {
                    name = name.Substring(1);
                }

                IRCUser user = new IRCUser(this, name);
                if(!Users.Contains(user)) {
                    Users.Add(user);
                    return true;
                }
            } 
            catch (Exception ignored) 
            {
                return false;
            }
            return false;
        }

        public bool removeUser(string name)
        {
            if (name.StartsWith("+") || name.StartsWith("%") || name.StartsWith("@") || name.StartsWith("&") ||
                    name.StartsWith("~"))
            {
                name = name.Substring(1);
            }

            if (containsUser(name))
            {
                IRCUser oldUser = getUser(name);
                Users.Remove(oldUser);
                return true;
            }
            return false;
        }

        public bool containsUser(string name)
        {
            if (name.StartsWith("+") || name.StartsWith("%") || name.StartsWith("@") || name.StartsWith("&") ||
                    name.StartsWith("~"))
            {
                name = name.Substring(1);
            }

            foreach (IRCUser user in Users)
            {
                if (user.Name == name)
                {
                    return true;
                }
            }
            return false;            
        }

        public IRCUser getUser(string name)
        {
            if (name.StartsWith("+") || name.StartsWith("%") || name.StartsWith("@") || name.StartsWith("&") ||
                    name.StartsWith("~"))
            {
                name = name.Substring(1);
            }

            foreach (IRCUser user in Users)
            {
                if (user.Name == name)
                {
                    return user;
                }
            }
            return null;
        }


        public bool addAdmin(IRCUser user)
        {
            if (!containsAdmin(user))
            {
                Admins.Add(user);
                return true;
            }
            return false;
        }

        public bool removeAdmin(IRCUser user)
        {
            if (containsAdmin(user))
            {
                Admins.Remove(user);
                return true;
            }
            return false;
        }

        public bool containsAdmin(IRCUser user)
        {
            return Admins.Contains(user);
        }


        public bool addToRegQueue(IRCUser user)
        {
            if (!inRegQueue(user))
            {
                RegisterQueue.Add(user);
                return true;
            }
            return false;
        }

        public bool removeFromRegQueue(IRCUser user)
        {
            if (inRegQueue(user))
            {
                RegisterQueue.Remove(user);
                return true;
            }
            return false;
        }

        public bool inRegQueue(IRCUser user)
        {
            return RegisterQueue.Contains(user);
        }



        public static bool isCTCP(string input) {
            string message = input.Substring(input.IndexOf(":", 1) + 1);
            char[] messageArray = message.ToCharArray();
            if (((byte)messageArray[0]) == 1 && ((byte)messageArray[messageArray.Length - 1]) == 1)
            {
                return true;
            }
            return false;
        }

        //TODO: Clean this up. This is just horrible.
        private void checkMessages()
        {
            while (Alive)
            {
                string inputLine = "";
                //do message proccessing
                while (Alive && (inputLine = readLine()) != null)
                {
                    bool reacted = false;

                    inputLine = inputLine.Trim();

                    Console.WriteLine(inputLine);
                    Console.WriteLine();                   

                    string[] splitInput = inputLine.Split(new Char[] { ' ' });

                    foreach(ChannelLine channel in JoinedChannels) {
                        //Pass off processing to the channel
                        if (inputLine.Contains("PRIVMSG " + channel.Name + " :"))
                        {
                            channel.react(inputLine);
                            reacted = true;
                            break;
                        }
                        //ScriptFiend was kicked from the channel. Remove this channel from joined channels.
                        else if(inputLine.Contains("KICK " + channel.Name + " ScriptFiend :")) {
                            removeChannel(channel.Name);
                            reacted = true;
                            break;
                        }
                        //ScriptFiend witnessed a user mode change in the channel
                        else if (inputLine.Contains("MODE " + channel.Name) && splitInput.Length == 5)
                        {
                            string username = splitInput[4];
                            IRCUser user = getUser(username);
                            char[] operations = splitInput[3].ToCharArray();
                            char currentOp = ' ';
                            foreach(char operation in operations) 
                            {
                                if (operation == '+' || operation == '-')
                                {
                                    currentOp = operation;
                                    continue;
                                }
                                else
                                {
                                    if (currentOp == '+')
                                    {
                                        if (!user.ChannelModes[channel].Contains(operation))
                                        {
                                            user.ChannelModes[channel].Add(operation);
                                        }
                                    }
                                    else if (currentOp == '-')
                                    {
                                        if (user.ChannelModes[channel].Contains(operation))
                                        {
                                            user.ChannelModes[channel].Remove(operation);
                                        }
                                    }
                                }
                            }                            
                            reacted = true;
                            break;
                        }
                        //ScriptFiend witnessed a channel mode change in the channel
                        else if (inputLine.Contains("MODE " + channel.Name) && splitInput.Length == 4)
                        {
                            char[] operations = splitInput[3].ToCharArray();
                            char currentOp = ' ';
                            foreach (char operation in operations)
                            {
                                if (operation == '+' || operation == '-')
                                {
                                    currentOp = operation;
                                    continue;
                                }
                                else
                                {
                                    if (currentOp == '+')
                                    {
                                        if (!channel.Modes.Contains(operation))
                                        {
                                            channel.Modes.Add(operation);
                                        }
                                    }
                                    else if (currentOp == '-')
                                    {
                                        if (channel.Modes.Contains(operation))
                                        {
                                            channel.Modes.Remove(operation);
                                        }
                                    }
                                }
                            }
                            reacted = true;
                            break;
                        }
                    }
                    //Pass off processing to the CTCP and PRIVMSG lines
                    if (!reacted && inputLine.Contains("PRIVMSG ScriptFiend :"))
                    { 
                        string userName = inputLine.Substring(1, inputLine.IndexOf('!') - 1);
                        if (!containsUser(userName))
                        {
                            addUser(userName);
                        }
                        if (isCTCP(inputLine))
                        {

                            if (!containsCTCPLine(userName))
                            {
                                addCTCPLine(userName);
                            }
                            getCTCPLine(userName).react(inputLine);
                            reacted = true;
                        }
                        else
                        {
                            if (!containsPrivateLine(userName))
                            {
                                addPrivateLine(userName);
                            }
                            getPrivateLine(userName).react(inputLine);
                            reacted = true;
                        }                      
                        Messages.Add(inputLine);
                    }
                    //Join channels based on invite requests
                    if (!reacted && inputLine.Contains("INVITE ScriptFiend :")) {
                        string channelName = inputLine.Substring(inputLine.IndexOf(":", 1) + 1);
                        string user = inputLine.Substring(1, inputLine.IndexOf('!') -1);
                        addChannel(channelName);
                        getChannel(channelName).writeLine(user + " has requested that I join this channel.");
                        reacted = true;
                    }
                    //Reply to PING requests
                    if (!reacted && splitInput.Length == 2 && splitInput[0] == "PING")
                    {
                        string PongReply = splitInput[1];                        
                        writeLine("PONG " + PongReply);
                        Console.WriteLine("PONG " + PongReply);
                        reacted = true;
                    }
                    //ScriptFiend witnessed a user join or part a channel or change their nick
                    if (!reacted && splitInput.Length == 3)
                    {
                        if (splitInput[1] == "JOIN")
                        {
                            ChannelLine channel = getChannel(splitInput[2].Substring(1));
                            string name = splitInput[0].Substring(1, splitInput[0].IndexOf('!') - 1);                            
                            channel.addUser(name);                            
                            reacted = true;
                        }
                        else if (splitInput[1] == "PART")
                        {
                            if (splitInput[0].Substring(1, splitInput[0].IndexOf('!') - 1) == "ScriptFiend")
                            {
                                reacted = true;
                            }
                            else
                            {
                                ChannelLine channel = getChannel(splitInput[2]);
                                channel.removeUser(splitInput[0].Substring(1, splitInput[0].IndexOf('!') - 1));
                                reacted = true;
                            }
                        }
                        else if (splitInput[1] == "NICK")
                        {
                            string user = inputLine.Substring(1, inputLine.IndexOf('!') - 1);
                            IRCUser ircUser = getUser(user);
                            ircUser.Name = splitInput[2].Substring(1);
                        }
                    }
                    //Proccess RPL_NAMREPLY messages (Replys from the /names command)
                    if (!reacted && splitInput.Length > 5 && splitInput[1] == "353")
                    {
                        ChannelLine channel = getChannel(splitInput[4]);

                        channel.addUser(splitInput[5].Substring(1));
                        for (int iii = 6; iii < splitInput.Length; iii++)
                        {
                            string user = splitInput[iii];                            
                            channel.addUser(user);
                            reacted = true;
                        }
                    }
                    //Proccess RPL_WHOISUSER messages
                    if (!reacted && splitInput.Length >= 7 && splitInput[1] == "311")
                    {
                        if (!containsUser(splitInput[3]))
                        {
                            addUser(splitInput[3]);
                        }
                        IRCUser user = getUser(splitInput[3]);
                        user.Username = splitInput[4];
                        user.Hostname = splitInput[5];
                        user.Realname = inputLine.Substring(inputLine.IndexOf('*') + 3);
                        reacted = true;
                    }
                    //Proccess RPL_WHOISCHANNELS messages
                    if (!reacted && splitInput.Length >= 5 && splitInput[1] == "319")
                    {
                        if (!containsUser(splitInput[3]))
                        {
                            addUser(splitInput[3]);
                        }
                        IRCUser user = getUser(splitInput[3]);
                        for (int iii = 4; iii < splitInput.Length; iii++)
                        {
                            string currentStr = splitInput[iii];
                            if (iii == 4)
                            {
                                currentStr = currentStr.Substring(1);
                            }
                            char[] chars = currentStr.ToCharArray();
                            if (IRCUser.SymbolToMode(chars[0]) != null)
                            {
                                char mode = IRCUser.SymbolToMode(chars[0]).Value;
                                currentStr = currentStr.Substring(1);
                                ChannelLine channel = getChannelFromBank(currentStr);
                                if (channel == null)
                                {
                                    channel = new ChannelLine(this, currentStr);
                                    if (!ChannelBank.Contains(channel))
                                    {
                                        ChannelBank.Add(channel);
                                    }
                                }
                                user.Channels.Add(channel);
                                if (!user.ChannelModes.ContainsKey(channel))
                                {
                                    user.ChannelModes.Add(channel, new List<char>());
                                }

                                if (!user.ChannelModes[channel].Contains(mode))
                                {
                                    user.ChannelModes[channel].Add(mode);
                                }
                            }
                            else
                            {
                                ChannelLine channel = new ChannelLine(this, currentStr);
                                user.Channels.Add(channel);
                            }                            
                        }
                        reacted = true;
                    }
                    //Proccess RPL_CHANNELMODEIS messages
                    if (!reacted && splitInput.Length == 5 && splitInput[1] == "324")
                    {
                        ChannelLine channel = getChannel(splitInput[3]);
                        if (channel == null)
                        {
                            channel = getChannelFromBank(splitInput[3]);
                        }
                        if (channel == null)
                        {
                            channel = new ChannelLine(this, splitInput[3]);
                            ChannelBank.Add(channel);
                        }

                        char[] modes = splitInput[4].Substring(1).ToCharArray();
                        foreach (char mode in modes)
                        {
                            if (!channel.Modes.Contains(mode))
                            {
                                channel.Modes.Add(mode);
                            }
                        }
                    }
                }
            }
        }
    }
}
