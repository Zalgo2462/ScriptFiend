using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using org.scriptFiend.IRC.Lines;
using org.scriptFiend.Modules.Channel.Poker.Tasks;
using org.scriptFiend.Modules.Util;

namespace org.scriptFiend.Modules.Channel.Poker
{
    class Poker : ActivatedModule
    {
        public Poker(ChannelLine chan) : base (chan, "~poker")
        {
            GameStarted = false;
            Tasks = new List<LoopTask>();
            Tasks.Add(new RegistrationTask(this));
            Tasks.Add(new RegEndTask(this));
        }
        
        private Thread GameThread { get; set; }

        private List<LoopTask> Tasks { get; set; }

        public Deck Cards { get; set; }

        public Dictionary<string, Player> Players { get; set; }    

        private DateTime registrationEnd { get; set; }
        
        public bool GameStarted { get; set; }
        
        protected override bool onActivate(string input) {
            line.writeLine("Starting a game of Texas Hold'em Poker. Type !join to join the game. Registration will last for 1 minute.");
            //gameThread = new Thread(new ThreadStart(this.dispatchGame));
            Cards = new Deck();
            Players = new Dictionary<string, Player>();
            registrationEnd = DateTime.Now.AddMinutes(1);
            GameThread = new Thread(new ThreadStart(this.dispatchGame));
            GameThread.Start();
            return true;
        }

        public override bool run(string input)
        {
            if(getMessage(input).Equals("!join", StringComparison.OrdinalIgnoreCase) /* && getMillisToRegistrationEnd() > 0*/) {
                line.writeLine(getUser(input) + " has joined the game");
                Players.Add(getUser(input), new Player(line.Server.getUser(getUser(input))));
                return true;
            }
            return false;
        }

        private void dispatchGame()
        {
            while (Activated)
            {
                foreach (LoopTask Task in Tasks)
                {
                    if (Task.activate())
                    {
                        Thread.Sleep(Task.task());
                        break;
                    }
                }
            }
        }

        public double getMillisToRegistrationEnd()
        {
            return registrationEnd.Subtract(DateTime.Now).TotalMilliseconds;
        }


    }
}
