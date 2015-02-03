using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.scriptFiend.IRC.Lines;

namespace org.scriptFiend.Modules.Channel.Dice
{
    class Dice : Module
    {
        private const string TAG = "~roll";
        private const int DEFAULT_DICE_SIDES = 6;

        public Dice(ChannelLine line) : base(line) { }

        public override void run(string input)
        {
            string message = getMessage(input);
            Random random = new Random();
            string[] tokens = message.Split(' ');
            if (tokens.Length > 1)
            {
                if (tokens[1] == "-help")
                {
                    line.writeLine("~roll [number of sides] [number of dice]");
                    return;
                }
                else if (tokens.Length == 2)
                {
                    int numberOfSides;
                    if(Int32.TryParse(tokens[1], out numberOfSides) && numberOfSides >= 0) {
                        line.writeLine("ScriptFiend rolled: [" + random.Next(numberOfSides) + "]");
                        return;
                    }
                }
                else if (tokens.Length == 3)
                {
                    int numberOfSides;
                    int numberOfDice;
                    if (Int32.TryParse(tokens[1], out numberOfSides) && numberOfSides >= 0 
                        && Int32.TryParse(tokens[2], out numberOfDice) && numberOfDice >= 0)
                    {
                        string result = "";
                        for (int iii = 0; iii < numberOfDice; iii++)
                        {
                            result += " [" + random.Next(numberOfSides) + "]";
                        }
                        line.writeLine("ScriptFiend rolled:" + result);
                        return;
                    }
                }
            }
            else if (tokens.Length == 1 && tokens[0] == "~dice")
            {
                line.writeLine("ScriptFiend rolled: [" + random.Next(DEFAULT_DICE_SIDES) + "]");
            }
        }

        public override bool activate(string input)
        {
            return getMessage(input).StartsWith(TAG);
        }
    }
}
