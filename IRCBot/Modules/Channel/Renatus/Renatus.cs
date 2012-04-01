using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using org.scriptFiend.IRC.Lines;

namespace org.scriptFiend.Modules.Channel.Renatus
{
    class Renatus : Module
    {
        public Renatus(ChannelLine chan)
            : base(chan, "~check")
        {
        }

        public override bool run(string input)
        {
            bool updated = false;
            WebClient client = new WebClient();
            string text = client.DownloadString("http://renatusbot.com");
            if (File.Exists("Renatus.cache"))
            {
                string cache_text = File.ReadAllText("Renatus.cache");
                if (!text.Equals(cache_text))
                {
                    client.DownloadFile("http://renatusbot.com", "Renatus.cache");
                }
            }
            else
            {
                line.writeLine("Initilizing Renatus checker");
                client.DownloadFile("http://renatusbot.com", "Renatus.cache");
                return true;
            }

            line.writeLine(updated ? "Renatusbot.com has been updated" : "There has been no update");
            return true;
        }
    }
}
