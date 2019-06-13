using System;
using System.Collections.Generic;
using MimeKit;

namespace EmailScanner
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load command line info about connection
            EmailInfo settings = new EmailInfo(args);

            List<MimeMessage> emails = settings.GetNewEmails();

            Console.WriteLine("Hello World!");
        }
    }
}
