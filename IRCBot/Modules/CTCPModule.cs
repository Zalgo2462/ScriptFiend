using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.scriptFiend.IRC.Lines;

namespace org.scriptFiend.Modules
{
    public abstract class CTCPModule : Module
    {
        public CTCPModule(CTCPLine line) : base(line)
        {
        }

        byte control = 1;

        public override string getUser(string input)
        {
            return input.Substring(1, input.IndexOf('!') - 1);
        }        

        public override string getMessage(string input)
        {
            string message = input.Substring(input.IndexOf(":", 1) + 1);
            return message.Trim((char)control);
        }       
    }
}
