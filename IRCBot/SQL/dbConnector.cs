using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data.Common;

namespace org.scriptFiend.SQL
{
    class dbConnector
    {
        private readonly string connectionString = "Data Source=scriptFiend.s3db";
       
        public string[] getChannels(string server)
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

                        myCommand.CommandText = "SELECT NAME FROM CHANNELS WHERE SERVER=?";
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

        public bool addChannel(string newChannel, string server)
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

                        chanCommand.CommandText = "INSERT INTO Channels (NAME) VALUES (?)";
                        chanParam.Value = newChannel;
                        chanCommand.Parameters.Add(chanParam);
                        i = chanCommand.ExecuteNonQuery();
                    }
                    myTransaction.Commit();
                }
                return i != 0;
            }
        }

        public Dictionary<string, int> getServers()
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

        public bool addServer(string newServer, int port)
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

                        servCommand.CommandText = "INSERT INTO Servers VALUES (?, ?)";
                        nameParam.Value = newServer;
                        portParam.Value = portParam;

                        servCommand.Parameters.Add(nameParam);
                        servCommand.Parameters.Add(portParam);

                        i = servCommand.ExecuteNonQuery();
                    }
                    myTransaction.Commit();
                }
                return i != 0;
            }
        }
    }
}
