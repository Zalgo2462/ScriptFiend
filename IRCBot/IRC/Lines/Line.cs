using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.scriptFiend.Modules;

namespace org.scriptFiend.IRC.Lines
{
    public interface Line
    {
        IRCServer Server { get; set; }
        List<string> Messages { get; set; }
        List<Module> Modules { get; set; }

        void writeLine(string str);
        void react(string line);
    }
}
