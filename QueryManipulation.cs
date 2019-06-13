using System.Collections.Generic;
using System.IO;
using MailKit;
using MailKit.Search;
using MimeKit;

namespace EmailScanner {
    public class QueryManipulation {
        /// <summary>
        /// Returns the query object used to search the email body text,
        /// derived from each line of the input text file
        /// </summary>
        /// <param name="fileName">The relative path of the desired text file</param>
        /// <returns></returns>
        public static SearchQuery GetSearchQuery(string fileName) {
            string[] queries = File.ReadAllLines(fileName);

            SearchQuery search = SearchQuery.BodyContains(queries[0]);

            for (int i = 1; i < queries.Length; i++) {
                search = search.Or(SearchQuery.BodyContains(queries[i]));
            }

            return search;
        }
        /// <summary>
        /// Uses the given indices to extract messages for provided inbox
        /// </summary>
        /// <param name="emails">Mail folder of emails</param>
        /// <param name="ids">List of unique ids of emails to be extracted</param>
        /// <returns></returns>
        public static List<MimeMessage> ExtractMessages(IMailFolder emails, IList<UniqueId> ids) {

            List<MimeMessage> messages = new List<MimeMessage>();
            foreach (UniqueId id in ids) {
                messages.Add(emails.GetMessage(id));
            }

            return messages;
        }
    }
}