using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.scriptFiend.IRC;
using System.Threading;
using org.scriptFiend.IRC.Lines;

namespace org.scriptFiend.Cmd
{
    class InputManager
    {
        Thread inputThread;

        IRCClient client;

        public InputManager(IRCClient cl)
        {
            this.client = cl;
            inputThread = new Thread(new ThreadStart(this.getInput));
            inputThread.Start();
        }

        public void kill() 
        {
            inputThread.Abort();
        }

        private void getInput()
        {
            string input = null;
            while (true)
            {
                while ((input = Console.ReadLine()) != null)
                {
                    if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        client.kill();
                        inputThread.Abort();
                      }
                    else if (input.StartsWith("say", StringComparison.OrdinalIgnoreCase))
                    {
                        string[] splitInput = input.Split(new Char[] { ' ' });
                        try
                        {
                            StringBuilder sb = new StringBuilder();

                            for (int iii = 0; iii < splitInput.Length; iii++)
                            {
                                if (iii > 2)
                                {
                                    sb.Append(splitInput[iii] + " ");
                                }
                            }

                            IRCServer serv = client.getServer(splitInput[1]);
                            if (serv != null)
                            {
                                ChannelLine chan = serv.getChannel(splitInput[2]);
                                if (chan != null)
                                {
                                    chan.writeLine(sb.ToString());
                                }
                                else
                                {
                                    Console.WriteLine("Channel not found");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Server not found");
                            }
                        }
                        catch (Exception ignored)
                        {
                            Console.WriteLine("Malformed Request");
                        }
                    }
                    else if (input.StartsWith("raw", StringComparison.OrdinalIgnoreCase))
                    {
                        string[] splitInput = input.Split(new Char[] { ' ' });
                        try
                        {
                            StringBuilder sb = new StringBuilder();

                            for (int iii = 0; iii < splitInput.Length; iii++)
                            {
                                if (iii > 1)
                                {
                                    sb.Append(splitInput[iii] + " ");
                                }
                            }

                            IRCServer serv = client.getServer(splitInput[1]);
                            if (serv != null)
                            {
                                serv.writeLine(sb.ToString());
                            }
                            else
                            {
                                Console.WriteLine("Server not found");
                            }
                        }
                        catch (Exception ignored)
                        {
                            Console.WriteLine("Malformed Request");
                        }
                    }
                    else if (input.StartsWith("join ", StringComparison.OrdinalIgnoreCase))
                    {
                        string[] splitInput = input.Split(new Char[] { ' ' });
                        try
                        {
                            IRCServer serv = client.getServer(splitInput[1]);
                            if (serv != null)
                            {
                                ChannelLine chan = serv.getChannel(splitInput[2]);
                                if (chan != null)
                                {
                                    Console.WriteLine("Already joined");
                                }
                                else
                                {
                                    serv.addChannel(splitInput[2]);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Server not found, if you would like to join a server use " +
                                    "the joinServer command");
                            }
                        }
                        catch (Exception ignored)
                        {
                            Console.WriteLine("Malformed Request");
                        }
                    }
                    else if (input.StartsWith("part", StringComparison.OrdinalIgnoreCase))
                    {
                        string[] splitInput = input.Split(new Char[] { ' ' });
                        try
                        {
                            IRCServer serv = client.getServer(splitInput[1]);
                            if (serv != null)
                            {
                                ChannelLine chan = serv.getChannel(splitInput[2]);
                                if (chan != null)
                                {
                                    serv.removeChannel(splitInput[2]);
                                }
                                else
                                {
                                    Console.WriteLine("You are not in that channel");
                                }
                            }
                            else
                            {
                                Console.WriteLine("You are not on that server!");
                            }
                        }
                        catch (Exception ignored)
                        {
                            Console.WriteLine("Malformed Request");
                        }
                    }
                    else if (input.StartsWith("joinServer", StringComparison.OrdinalIgnoreCase))
                    {
                        string[] splitInput = input.Split(new Char[] { ' ' });
                        try
                        {
                            int port = 0;
                            int.TryParse(splitInput[2], out port);
                            
                            IRCServer serv = client.getServer(splitInput[1]);
                            if (serv != null)
                            {
                                Console.WriteLine("Already joined");                                
                            }
                            else
                            {
                                client.addServer(splitInput[1], port);
                            }                            
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Malformed Request");
                            Console.WriteLine(e.Message);
                        }
                    }
                    else if (input.Equals("cls", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.Clear();
                    }
                    Console.WriteLine();
                }
            }
        }
    }
}
