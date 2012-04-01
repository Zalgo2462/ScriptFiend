using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.scriptFiend.IRC;

namespace org.scriptFiend.Modules.Channel.Poker
{
    class Player
    {
        public Hand Hand { get; set; }
        public IRCUser User { get; set; }

        public Player(IRCUser user)
        {
            User = user;
            Hand = new Hand();
        }
    }
}
