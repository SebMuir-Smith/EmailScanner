using System;
using System.Collections.Generic;
using System.Linq;
using MailKit;

namespace EmailScanner
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load command line and text file info about connection + keywords
            EmailInfo settings = new EmailInfo(args);

            // Pull emails and email indicides from last 6 hours
            IList<UniqueId> newEmailIds;
            IMailFolder emails = settings.GetNewEmails(out newEmailIds);

            // Extract Retrospect error messages and update indices
            IList<UniqueId> errorEmailIds;
            IMailFolder errors = settings.GetErrors(emails, newEmailIds, out errorEmailIds);

            IEnumerable<UniqueId> output = newEmailIds.Union(errorEmailIds);
            Console.WriteLine("Hello World!");
        }
    }
}
