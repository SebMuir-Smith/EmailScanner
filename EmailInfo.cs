using System;
using System.Collections.Generic;
using System.IO;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
namespace EmailScanner {
    public class EmailInfo {
        public string userName;

        public string password;

        public string serverName;

        public int port;

        private SearchQuery errorSearchTerm;

        /// <summary>
        /// Constructor for email info object
        /// </summary>
        /// <param name="args">string[4] containing name, password, server and port</param>
        public EmailInfo(string[] args) {

            userName = args[0];

            password = args[1];

            serverName = args[2];

            port = int.Parse(args[3]);

            errorSearchTerm = QueryManipulation.GetSearchQuery("errors.txt");

        }

        /// <summary>e
        /// Returns a list of emails recieved in the last 6 hours 
        /// </summary>
        public List<MimeMessage> GetNewEmails() {

            List<MimeMessage> emailsOutput;

            using(ImapClient connection = new ImapClient()) {

                // Connect to server
                connection.Connect(this.serverName, this.port);

                connection.Authenticate(this.userName, this.password);

                IMailFolder inbox = connection.Inbox;

                inbox.Open(FolderAccess.ReadOnly);

                int sixHours = 6 * 6 * 60;

                IList<UniqueId> messageIds = inbox.Search(SearchQuery.YoungerThan(sixHours));

                emailsOutput = QueryManipulation.ExtractMessages(inbox,messageIds);
            }

            return emailsOutput;
        }

    }
}