using System.Collections.Generic;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.IO;


namespace EmailScanner
{
    public class EmailSender : EmailInfo
    {
        public string adminName = "Sebastian";

        public int outgoingPort;

        public string recipient;




        public EmailSender(string[] args):base(args){

            outgoingPort = int.Parse(args[4]);

            recipient = args[5];


        }

        public bool SendEmails(List<MimeMessage> errorEmails, List<MimeMessage> uniqueEmails,
        int numberSuccessful){

            // Count the number of successful emails if no errors
            if (errorEmails.Count == 0){
                int numberSuccessfulPrev = int.Parse(new StreamReader("nSuccessful.txt").ReadLine());

                numberSuccessful = numberSuccessfulPrev + numberSuccessful;

                StreamWriter s = new StreamWriter("nSuccessful.txt",false);
                s.WriteLine(numberSuccessful);
                s.Close();
            } else {
                StreamWriter s = new StreamWriter("nSuccessful.txt",false);
                s.WriteLine(0);
                s.Close();
            }

            // Calculate useful statistics to be added to bottom of email
            TextPart footer = new TextPart("Plain"){
                Text = string.Format("\n--------------------------------------------------------"+
                "\nThis email was automatically forwarded by a script.\n"+
                "Statistics:\n\tTotal unread errors: {0}\n\tTotal unread unique emails: {1}\n\t"+
                "DateTime script was run: {2}\n\tNumber of consecutive successful backups: {3}",
                errorEmails.Count, uniqueEmails.Count,DateTime.Now, numberSuccessful)
            };

            Multipart bodyContent;

            SmtpClient connection = new SmtpClient();

            // Connect to server
            connection.Connect(this.serverName, this.outgoingPort);

            connection.Authenticate(this.userName, this.password);

            MimeMessage outgoingMessage;
            foreach (MimeMessage error in errorEmails){
                outgoingMessage = new MimeMessage();

                outgoingMessage.From.Add(new MailboxAddress("Email Checker", this.userName));
                outgoingMessage.To.Add(new MailboxAddress(adminName, this.recipient));
                
                outgoingMessage.Subject = string.Format("{0}, there is an error in the backup.",adminName);

                bodyContent = new Multipart("mixed");
                bodyContent.Add(error.Body);
                bodyContent.Add(footer);
                outgoingMessage.Body = bodyContent;

                connection.Send(outgoingMessage);
            }

            foreach (MimeMessage unique in uniqueEmails){
                outgoingMessage = new MimeMessage();

                outgoingMessage.From.Add(new MailboxAddress("Email Checker", this.userName));
                outgoingMessage.To.Add(new MailboxAddress(adminName, this.recipient));
                
                outgoingMessage.Subject = string.Format("{0}, you have a new IT email.",adminName);

                bodyContent = new Multipart("mixed");
                bodyContent.Add(unique.Body);
                bodyContent.Add(footer);
                outgoingMessage.Body = bodyContent;

                connection.Send(outgoingMessage);
            }

            return true;
        }
       
    }
}