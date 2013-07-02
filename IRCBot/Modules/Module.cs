using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.scriptFiend.IRC.Lines;

namespace org.scriptFiend.Modules
{
    public abstract class Module
    {
        protected Line line;

        public Module(Line line)
        {
            this.line = line;
        }

        public virtual string getMessage(string input)
        {
            return input.Substring(input.IndexOf(":", 1) + 1);
        }

        public virtual string getUser(string input)
        {
            return input.Substring(1, input.IndexOf('!') -1);
        }

        //Removes a string from the beginning of another string
        public virtual string removeTag(string tag, string message)
        {
            int index = message.IndexOf(tag);
            return (index < 0) ? message : message.Remove(index, tag.Length);          
        }

        public bool react(string input)
        {
            if (activate(input))
            {
                return run(input);
            }
            return false;
        }

        public abstract bool run(string input);

        public abstract bool activate(string input);
    }
}
