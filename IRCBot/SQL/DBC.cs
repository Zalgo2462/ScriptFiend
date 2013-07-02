using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data.Common;
using org.scriptFiend.Modules;
using System.IO;

namespace org.scriptFiend.SQL
{
    public class DBC
    {

        private static string connectionString = "Data Source=" + IrcBot.SQLFILEPATH;
       
        public static string[] getChannels(string server)
        {
            List<String> channels = new List<string>();
            using (SQLiteConnection cnn = new SQLiteConnection(connectionString))
            {
                cnn.Open();
                using (SQLiteTransaction myTransaction = cnn.BeginTransaction())
                {
                    using (SQLiteCommand myCommand = new SQLiteCommand(cnn))
                    {
                        SQLiteParameter myParam = new SQLiteParameter();
                        myParam.Value = server;

                        myCommand.CommandText = "SELECT NAME FROM Channels WHERE SERVER=?";
                        myCommand.Parameters.Add(myParam);

                        SQLiteDataReader reader = myCommand.ExecuteReader();
                        while (reader.Read())
                        {
                            channels.Add(reader.GetString(0));
                        }
                        reader.Close();
                    }
                    myTransaction.Commit();
                }
            }
            return channels.ToArray();
        }

        public static bool addChannel(string newChannel, string server)
        {
            using (SQLiteConnection cnn = new SQLiteConnection(connectionString))
            {
                foreach (string channel in getChannels(server))
                {
                    if (channel == newChannel)
                    {
                        return false;
                    }
                }
                cnn.Open();
                int i = 0;
                using (SQLiteTransaction myTransaction = cnn.BeginTransaction())
                {
                    using (SQLiteCommand chanCommand = new SQLiteCommand(cnn))
                    {
                        SQLiteParameter chanParam = new SQLiteParameter();
                        SQLiteParameter servParam = new SQLiteParameter();

                        chanCommand.CommandText = "INSERT INTO Channels (SERVER, NAME) VALUES (?, ?)";

                        servParam.Value = server;
                        chanParam.Value = newChannel;
                        chanCommand.Parameters.Add(servParam);
                        chanCommand.Parameters.Add(chanParam);

                        i = chanCommand.ExecuteNonQuery();
                    }
                    myTransaction.Commit();
                }
                return i != 0;
            }
        }

        public static Dictionary<string, int> getServers()
        {
            Dictionary<string, int> servers = new Dictionary<string,int>();
            using (SQLiteConnection cnn = new SQLiteConnection(connectionString))
            {
                cnn.Open();
                using (SQLiteTransaction myTransaction = cnn.BeginTransaction())
                {
                    using (SQLiteCommand myCommand = new SQLiteCommand(cnn))
                    {
                        myCommand.CommandText = "SELECT * FROM Servers";
                        SQLiteDataReader reader = myCommand.ExecuteReader();
                        while (reader.Read())
                        {
                            servers.Add(reader.GetString(0), reader.GetInt32(1));
                        }
                        reader.Close();
                    }
                    myTransaction.Commit();
                }
            }
            return servers;
        }

        public static bool addServer(string newServer, int port)
        {
            using (SQLiteConnection cnn = new SQLiteConnection(connectionString))
            {
                foreach (KeyValuePair<string, int> server in getServers())
                {
                    if (server.Key == newServer)
                    {
                        return false;
                    }
                }
                cnn.Open();
                int i = 0;
                using (SQLiteTransaction myTransaction = cnn.BeginTransaction())
                {
                    using (SQLiteCommand servCommand = new SQLiteCommand(cnn))
                    {
                        SQLiteParameter nameParam = new SQLiteParameter();
                        SQLiteParameter portParam = new SQLiteParameter();

                        servCommand.CommandText = "INSERT INTO Servers (URL, PORT) VALUES (?, ?)";
                        nameParam.Value = newServer;
                        portParam.Value = port;

                        servCommand.Parameters.Add(nameParam);
                        servCommand.Parameters.Add(portParam);

                        i = servCommand.ExecuteNonQuery();
                    }
                    myTransaction.Commit();
                }
                return i != 0;
            }
        }

        public static bool executeNonQuery(string command)
        {            
            int i = 0;
            using (SQLiteConnection cnn = new SQLiteConnection(connectionString))
            {
                try
                {
                    cnn.Open();
                }
                catch (Exception tryAgainException)
                {
                    cnn.Open();
                }
                using (SQLiteTransaction myTransaction = cnn.BeginTransaction())
                {
                    using (SQLiteCommand myCommand = new SQLiteCommand(cnn))
                    {
                        myCommand.CommandText = command;
                        i = myCommand.ExecuteNonQuery();
                    }
                    myTransaction.Commit();
                }
            }
            return i != 0;
        }

        public static object[] executeReader(string command, int col)
        {
            List<object> toReturn = new List<object>();
            using (SQLiteConnection cnn = new SQLiteConnection(connectionString))
            {
                try
                {
                    cnn.Open();
                }
                catch (Exception tryAgainException)
                {
                    cnn.Open();
                }
                using (SQLiteTransaction myTransaction = cnn.BeginTransaction())
                {
                    using (SQLiteCommand myCommand = new SQLiteCommand(cnn))
                    {
                        myCommand.CommandText = command;
                        SQLiteDataReader reader = myCommand.ExecuteReader();
                        while (reader.Read())
                        {
                            for (int iii = 0; iii < col; iii++)
                            {
                                toReturn.Add(reader.GetValue(iii));
                            }
                        }
                        
                    }
                    myTransaction.Commit();
                }
            }
            return toReturn.ToArray();
        }

        public static void createDB()
        {
            executeNonQuery("CREATE TABLE Servers (URL TEXT UNIQUE NOT NULL, PORT INTEGER DEFAULT 6667 NOT NULL)");
            executeNonQuery("CREATE TABLE Channels (SERVER TEXT NOT NULL, NAME TEXT NOT NULL)");
            executeNonQuery("CREATE TABLE Admins (USERNAME TEXT UNIQUE NOT NULL, PASSWORD TEXT NOT NULL)");
        }

    }
}
