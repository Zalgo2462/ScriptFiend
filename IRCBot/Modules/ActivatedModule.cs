using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.scriptFiend.IRC.Lines;

namespace org.scriptFiend.Modules
{
    abstract class ActivatedModule : Module
    {
        protected bool activated = false;

        private Object switchLock = new Object();

        protected bool Activated
        {
            get
            {
                lock (switchLock)
                {
                    return activated;
                }
            }
            set
            {
                lock (switchLock)
                {
                    activated = value;
                }
            }
        }

        public ActivatedModule(ChannelLine chan, string trigger) : base(chan, trigger)
        {            
        }

        public override bool react(string input)
        {
            if (!Activated)
            {
                if (getMessage(input).StartsWith(ActivateTrigger))
                {
                    return (Activated = onActivate(input));      
                }
                return false;
            }
            else
            {
                return run(input);
            }
        }

        protected abstract bool onActivate(string input);
    }
}
