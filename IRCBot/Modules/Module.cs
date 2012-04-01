using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.scriptFiend.IRC.Lines;

namespace org.scriptFiend.Modules
{
    abstract class Module
    {
        public Line line;

        public string ActivateTrigger { get; set; }

        public Module(Line line, string trigger)
        {
            this.line = line;
            this.ActivateTrigger = trigger;
        }

        public virtual string getMessage(string input)
        {
            return input.Substring(input.IndexOf(":", 1) + 1);
        }

        public virtual string getUser(string input)
        {
            return input.Substring(1, input.IndexOf('!') -1);
        }

        public virtual string removeTag(string tag, string input)
        {
            StringBuilder t = new StringBuilder();
            int index = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == tag[index])
                {
                    index++;
                    if (tag.Length == index)
                    {
                        i++;
                        for (int j = i; j < input.Length; j++)
                        {
                            t.Append(input[j]);
                        }
                        return t.ToString();
                    }
                    continue;
                }
                else
                {
                    if (index != 0)
                    {
                        for (int j = (i - index); j < i; j++)
                        {
                            t.Append(input[j]);
                        }
                        index = 0;
                    }
                    t.Append(input[i]);
                }
            }
            return t.ToString();
        }

        public virtual bool react(string input)
        {
            if (getMessage(input).StartsWith(ActivateTrigger))
            {
                return run(input);
            }
            return false;
        }

        public abstract bool run(string input);
    }
}
