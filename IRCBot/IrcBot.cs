using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using org.scriptFiend.IRC;
using org.scriptFiend.SQL;

namespace org.scriptFiend
{

    internal class IrcBot
    {
        public const string admin = "Wade";
        public static string HOMEPATH = (Environment.OSVersion.Platform == PlatformID.Unix ||
               Environment.OSVersion.Platform == PlatformID.MacOSX)
               ? Environment.GetEnvironmentVariable("HOME")
               : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%") + 
               Path.DirectorySeparatorChar + "ScriptFiend";

        public static string SQLFILEPATH = HOMEPATH + Path.DirectorySeparatorChar + "ScriptFiendDB.s3db";

        static void Main(string[] args)
        {            
           try {
                init();
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

        public static void init()
        {
            if(!File.Exists(SQLFILEPATH)) {
                Console.WriteLine("Creating {0}",SQLFILEPATH);
                if (!Directory.Exists(HOMEPATH))
                {
                    Directory.CreateDirectory(HOMEPATH);
                }
                FileStream initializer = File.Open(SQLFILEPATH, FileMode.Create);
                initializer.Close();                
                DBC.createDB();
            }
        }
    }  
}
