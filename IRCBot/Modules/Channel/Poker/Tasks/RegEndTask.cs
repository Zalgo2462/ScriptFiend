using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.scriptFiend.Modules.Util;

namespace org.scriptFiend.Modules.Channel.Poker.Tasks
{
    class RegEndTask : LoopTask
    {

        Poker parent { get; set; }

        public RegEndTask(Poker p)
        {
            parent = p;
        }

        public bool activate()
        {
            return !parent.GameStarted;
        }

        public int task()
        {
            string users = "";
            foreach (Player player in parent.Players.Values)
            {
                users += " " + player.User.Name;
            }
            parent.line.writeLine("Registration is over. Starting Game!");
            parent.line.writeLine("Users:" + users);
            parent.GameStarted = true;
            return 0;
        }
    }
}
