using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;
using org.scriptFiend.Cmd;
using org.scriptFiend.SQL;

namespace org.scriptFiend.IRC
{
    class IRCClient
    {
        // User information defined in RFC 2812 (Internet Relay Chat: Client Protocol) to be sent to the irc server 
        public string USER = "USER SFiend 0 * :A Quite Fiendish Bot For Scripts";
      
        public string NICK = "ScriptFiend";

        public InputManager IM { get; set; }

        public List<IRCServer> Servers { get; set; }


        public IRCClient()
        {
            IM = new InputManager(this);            
            Servers = new List<IRCServer>();
        }

        public void loadServers()
        {
            Console.WriteLine("Loading Servers");
            foreach (KeyValuePair<string, int> server in DBC.getServers())
            {
                if (!containsServer(server.Key))
                {
                    addServer(server.Key, server.Value);
                }
            }
        }

        public void saveServers()
        {
            foreach (IRCServer server in Servers)
            {
                DBC.addServer(server.ADDRESS, server.PORT);
            }
        }

        public bool addServer(string address, int port)
        {
            if (!containsServer(address))
            {
                IRCServer newServer = new IRCServer(address, port, this);
                newServer.login(USER, NICK);
                Servers.Add(newServer);
                return true;
            }
            return false;
        }

        public bool removeServer(string address)
        {
            if (containsServer(address))
            {
                IRCServer oldServer = getServer(address);
                oldServer.close();
                Servers.Remove(oldServer);
                return true;
            }
            return false;
        }

        public bool containsServer(string address)
        {
            foreach (IRCServer server in Servers)
            {
                if (server.ADDRESS == address)
                {
                    return true;
                }
            }
            return false;
        }

        public IRCServer getServer(string address)
        {
            foreach (IRCServer server in Servers)
            {
                if (server.ADDRESS == address)
                {
                    return server;
                }
            }
            return null;
        }

        public bool kill()
        {
            bool killed = true;
            saveServers();
            foreach (IRCServer server in Servers)
            {
                if (!server.kill())
                {
                    killed = false;
                }
            }
            return killed;
        }
    }
}
