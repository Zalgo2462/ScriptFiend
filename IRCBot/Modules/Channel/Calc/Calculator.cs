using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using System.Json;
using org.scriptFiend.IRC.Lines;
using System.Text.RegularExpressions;
using System.Globalization;

namespace org.scriptFiend.Modules.Channel.Calc
{
    class Calculator : Module
    {
        private const string TAG = "~calc ";

        public Calculator(ChannelLine chan)
            : base(chan)
        {
        }

        public override void run(string input)
        {            
            string message = removeTag(TAG, getMessage(input));
            //TODO: WRITE CALC LOGIC
           
        }

        public override bool activate(string input)
        {
            return getMessage(input).StartsWith(TAG);
        }
    }
}
