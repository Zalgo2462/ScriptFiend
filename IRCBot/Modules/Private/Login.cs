using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using org.scriptFiend.IRC.Lines;
using org.scriptFiend.IRC;
using System.Security.Cryptography;
using org.scriptFiend.SQL;
using System.Data.SQLite;

namespace org.scriptFiend.Modules.Private
{
    public class Login : Module
    {
        private const string TAG = "!login ";
        private const string SALT = "0x945adfb";
        public Login(PrivateLine line) : base (line)
        {
        }

        public override void run(string input)
        {
            string message = getMessage(input);
            string password = removeTag(TAG, message);
            string userName = getUser(input);

            SHA512 hashClass = SHA512.Create();
            byte[] passBytes = Encoding.UTF8.GetBytes(password + SALT);
            byte[] hashedBytes = hashClass.ComputeHash(passBytes);

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < hashedBytes.Length; i++)
            {
                sBuilder.Append(hashedBytes[i].ToString("x2"));
            }

            string hashedPass = sBuilder.ToString();

            object[] reader = DBC.executeReader("SELECT USERNAME,PASSWORD FROM Admins WHERE USERNAME = '" + userName + "' AND PASSWORD = '" + hashedPass + "'", 2);
            
            string dbUser = (string)reader[0];
            string dbPass = (string)reader[1];

            if (dbUser == userName && hashedPass == dbPass)
            {
                IRCServer server = line.Server;
                server.addAdmin(server.getUser(userName));
                line.writeLine("You have been assigned the role of Admin.");
            }
            else
            {
                line.writeLine("Login Failure");
            }
        }

        public override bool activate(string input)
        {
            return getMessage(input).StartsWith(TAG);
        }

    }
}
