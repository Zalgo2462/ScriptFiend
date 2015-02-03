using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Json;
using org.scriptFiend.IRC.Lines;

namespace org.scriptFiend.Modules.Channel.Urban
{
    class UrbanDictionary : Module
    {
        private const string TAG = "~urban ";
        private const string URBAN_API = "http://api.urbandictionary.com/v0/define?term=";

        public UrbanDictionary(ChannelLine chan)
            : base(chan)
        {
        }

        public override void run(string input)
        {
            string message = removeTag(TAG, getMessage(input));
            WebClient wc = new WebClient();
            dynamic result = JsonValue.Parse(wc.DownloadString(URBAN_API + message));
            if (result.result_type == "no_results")
            {
                line.writeLine("No entry was found in the urban dictionary for: " + message);
                return;
            }
            else if (result.result_type == "exact")
            {
                string definition = result.list[0].definition;
                string example = result.list[0].example;
                line.writeLine(message + ": " + definition);
                if (example != null)
                {
                    line.writeLine("Ex: " + example);
                }
            }
        }

        public override bool activate(string input)
        {
            return getMessage(input).StartsWith(TAG);
        }
    }
}
