using System.Collections.Generic;
using MailKit.Net.Imap;
using MailKit;
using MailKit.Search;
using System.IO;
namespace EmailScanner
{
    public class EmailInfo
    {
        public string userName;

        public string password;

        public string serverName;

        public int port;

        private SearchQuery searchForErrors;


        /// <summary>
        /// Constructor for email info object
        /// </summary>
        /// <param name="args">string[4] containing name, password, server and port</param>
        public EmailInfo(string[] args)
        {

            userName = args[0];

            password = args[1];

            serverName = args[2];

            port = int.Parse(args[3]);

            searchForErrors = GetSearchQuery();

        }

        private SearchQuery GetSearchQuery()
        {
            string[] queries = File.ReadAllLines("queries.txt");

            SearchQuery search = SearchQuery.BodyContains(queries[0]);

            for (int i = 1; i < queries.Length; i++){
                search = search.Or(SearchQuery.BodyContains(queries[i]));
            }

            return search;
        }


    /// <summary>
    /// Returns a list of Emails using given info
    /// </summary>
    public List<dynamic> GetEmails()
    {

        using (ImapClient connection = new ImapClient())
        {

            // Connect to server
            connection.Connect(this.serverName, this.port);

            connection.Authenticate(this.userName, this.password);

            IMailFolder inbox = connection.Inbox;

            inbox.Open(FolderAccess.ReadOnly);



        }

        return new List<dynamic>();
    }


}
}