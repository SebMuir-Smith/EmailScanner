using System.Collections.Generic;
using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
namespace EmailScanner {
    public class EmailInfo {
        // Email connection parameters set via command line
        public string userName;

        public string password;

        public string serverName;

        public int port;

        // Query used to search for errors, mainly found from errors.txt file
        private SearchQuery errorSearchTerm;

        // Simple query used to search for retrospect successful backups
        private SearchQuery successfulSearchTerm;

        /// <summary>
        /// Constructor for email info object
        /// </summary>
        /// <param name="args">string[4] containing name, password, server and port</param>
        public EmailInfo(string[] args) {

            userName = args[0];

            password = args[1];

            serverName = args[2];

            port = int.Parse(args[3]);

            errorSearchTerm = SearchQuery.FromContains("NewDesktop Retrospect NAS drives");

            successfulSearchTerm = QueryManipulation.GetSearchQuery("success.txt", true);

        }

        /// <summary>
        /// Returns a list of emails recieved in the last 6 hours 
        /// </summary>
        public IMailFolder GetNewEmails(out IList<UniqueId> messageIds) {

            IMailFolder inbox;

            ImapClient connection = new ImapClient();

            // Connect to server
            connection.Connect(this.serverName, this.port);

            connection.Authenticate(this.userName, this.password);

            inbox = connection.Inbox;

            inbox.Open(FolderAccess.ReadWrite);

            int sixHours = 6 * 60 * 60;

            // Get emails from the last 6 hours
            messageIds = inbox.Search(SearchQuery.YoungerThan(sixHours));

            // Mark the emails as seen
            inbox.AddFlags(messageIds, MessageFlags.Seen, true);

            return inbox;
        }

        public IList<UniqueId> GetSuccessful(IMailFolder emails, IList<UniqueId> newMessageIds) {

            IList<UniqueId> messageIds = emails.Search(newMessageIds, this.successfulSearchTerm);
            return messageIds;
        }

        public IList<UniqueId> GetFailure(IMailFolder emails, IList<UniqueId> newMessageIds) {

            IList<UniqueId> messageIds = emails.Search(newMessageIds, this.errorSearchTerm);
            return messageIds;
        }

    }
}