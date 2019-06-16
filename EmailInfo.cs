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

        // Query used to search for errors
        private SearchQuery errorSearchTerm;

        // Simple query used to search for retrospect successful backups, from success.txt file
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
        /// Returns a MailFolder object containing the list of all emails.
        /// Also returns messageIds, a list of indicies marking the emails that are unseen
        /// and thus are to be searched.
        /// </summary>
        public IMailFolder GetNewEmails(out IList<UniqueId> messageIds) {

            IMailFolder inbox;

            ImapClient connection = new ImapClient();

            // Connect to server
            connection.Connect(this.serverName, this.port);

            connection.Authenticate(this.userName, this.password);

            inbox = connection.Inbox;

            inbox.Open(FolderAccess.ReadWrite);

            // Get emails that haven't been marked as seen
            messageIds = inbox.Search(SearchQuery.NotSeen);

            // Mark the emails as seen
            inbox.AddFlags(messageIds, MessageFlags.Seen, true);

            return inbox;
        }

        /// <summary>
        /// Filters the current email inbox object to pull only the "non-error" retrospect emails
        /// Returns a list of indicies that contain the indices that the non-error emails 
        /// can be found at in the original email inbox object
        /// </summary>
        /// <param name="emails">Object containing all emails</param>
        /// <param name="newMessageIds">List of indicies of unseen emails</param>
        /// <returns></returns>
        public IList<UniqueId> GetSuccessful(IMailFolder emails, IList<UniqueId> newMessageIds) {
            // First argument means only search new emails
            IList<UniqueId> messageIds = emails.Search(newMessageIds, this.successfulSearchTerm);
            return messageIds;
        }

        /// <summary>
        /// Filters the current email inbox object to pull only the "error" emails
        /// Returns a list of indicies that contain the indices that the error emails 
        /// can be found at in the original email inbox object
        /// </summary>
        /// <param name="emails">Object containing all emails</param>
        /// <param name="newMessageIds">List of indicies of unseen emails that are not successful retrospect emails</param>
        /// <returns></returns>
        public IList<UniqueId> GetFailure(IMailFolder emails, IList<UniqueId> nonSuccessfulIds) {
            // First argument means only search for error/unique emails
            IList<UniqueId> messageIds = emails.Search(nonSuccessfulIds, this.errorSearchTerm);
            return messageIds;
        }

    }
}