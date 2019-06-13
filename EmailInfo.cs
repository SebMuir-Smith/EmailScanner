using System.Collections.Generic;
using MailKit.Net.Imap;
using MimeKit;
namespace EmailScanner
{
    public class EmailInfo
    {
        public string userName;
        
        public string password;

        public string serverName;

        public int port;


        /// <summary>
        /// Constructor for email info object
        /// </summary>
        /// <param name="args">string[4] containing name, password, server and port</param>
        public EmailInfo(string[] args){

            userName = args[0];

            password = args[1];

            serverName = args[2];

            port = int.Parse(args[3]);

        }
        /// <summary>
        /// Returns a list of Emails using given info
        /// </summary>
        public List<dynamic> GetEmails(){

            using (ImapClient connection = new ImapClient()){

                connection.Connect(this.serverName,this.port);

                connection.Authenticate(this.userName, this.password);



            }
            
            return new List<dynamic>();
        }


    }
}