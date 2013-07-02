using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using org.scriptFiend.IRC.Lines;
using org.scriptFiend.SQL;

namespace org.scriptFiend.Modules.Private
{
    public class Register : Module
    {
        private const string TAG = "!register ";
        private const string SALT = "0x945adfb";

        public Register(PrivateLine line) : base (line)
        {
        }

        public override bool run(string input)
        {
            string message = getMessage(input);
            string password = removeTag(TAG, message);
            string userName = getUser(input);

            line.Server.removeFromRegQueue(line.Server.getUser(userName));

            SHA512 hashClass = SHA512.Create();
            byte[] passBytes = Encoding.UTF8.GetBytes(password + SALT);
            byte[] hashedBytes = hashClass.ComputeHash(passBytes);

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < hashedBytes.Length; i++)
            {
                sBuilder.Append(hashedBytes[i].ToString("x2"));
            }

            string hashedPass = sBuilder.ToString();
            line.writeLine(hashedPass);

            bool success = DBC.executeNonQuery("INSERT INTO Admins (USERNAME, PASSWORD) VALUES ('" + userName + "', '" + hashedPass + "')");
            

            if (success)
            {
                line.writeLine("You have been assigned the role of Admin. Please log in using !login [pass]");
            }
            else
            {
                line.writeLine("Login Failure");
            }
            return true;
        }

        public override bool activate(string input)
        {
            return getMessage(input).StartsWith(TAG) && 
                line.Server.inRegQueue(line.Server.getUser(getUser(input)));
        }
    }
}
