using System;
using System.Collections.Generic;
using EmailScanner;

namespace EmailScanner
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load command line info about connection
            EmailInfo settings = new EmailInfo(args);

            List<dynamic> emails = settings.GetEmails();
            Console.WriteLine("Hello World!");
        }
    }
}
