using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using org.scriptFiend.IRC;

namespace org.scriptFiend
{

    class IrcBot
    {
        public const string admin = "Wade";

        static void Main(string[] args)
        {            
            try {                
                IRCClient cl = new IRCClient();
                cl.loadServers();
                foreach (IRCServer server in cl.Servers)
                {
                    server.loadChannels();
                } 
            }
            catch (Exception e)
            {
                // Show the exception, sleep for a while and try to establish a new connection to irc server
                Console.WriteLine(e.ToString());
                Thread.Sleep(5000);
                string[] argv = { };
                Main(argv);
            }
        }
    }  
}
