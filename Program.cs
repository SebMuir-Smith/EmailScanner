using System;
using MailKit.Net.Imap;
using EmailScanner;

namespace EmailScanner
{
    class Program
    {
        static void Main(string[] args)
        {
            EmailInfo settings = new EmailInfo(args);
            Console.WriteLine("Hello World!");
        }
    }
}
