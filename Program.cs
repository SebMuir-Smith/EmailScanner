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

            // Extract Retrospect non-error messages and update indices        
            IList<UniqueId> successfulEmailIds = settings.GetSuccessful(emails, newEmailIds);

            // Get Ids of everything that is not an successful retrospect email
            IList<UniqueId> errorOrUniqueIds = newEmailIds.Except(successfulEmailIds).ToList<UniqueId>();

            // Get Ids of every retrospect error message
            IList<UniqueId> errorIds = settings.GetFailure(emails, errorOrUniqueIds);

            // Get Ids of everything that is not a retrospect email
            IList<UniqueId> uniqueIds = errorOrUniqueIds.Except(errorIds).ToList<UniqueId>();


            Console.WriteLine("Hello World!");
        }
    }
}
