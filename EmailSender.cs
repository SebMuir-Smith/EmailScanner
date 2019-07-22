using System;
using System.Collections.Generic;
using System.IO;
using MailKit.Net.Smtp;
using MimeKit;

namespace EmailScanner {
    public class EmailSender : EmailInfo {
        // Who the emails are to be addressed to
        public string adminName = "Sebastian";

        // Outgoing info from command line
        public int outgoingPort;

        public string recipient;

        public EmailSender(string[] args) : base(args) {

            outgoingPort = int.Parse(args[4]);

            recipient = args[5];

        }

        /// <summary>
        /// When given error emails and the numberSuccessful summary statistic, emails out reports
        /// to the specified email on errors and unique (non-retrospect) emails 
        /// </summary>
        /// <param name="errorEmails">List of emails of retrospect error emails</param>
        /// <param name="uniqueEmails">List of emails of anything that isn't from retrospect</param>
        /// <param name="numberSuccessful">How many successful emails that were identified in pre-processing</param>
        public void SendEmails(List<MimeMessage> errorEmails, List<MimeMessage> uniqueEmails,
            int numberSuccessful) {

            // Get successful streak count, and increment it with the current successful count
            int numberSuccessfulPrev = int.Parse(new StreamReader("nSuccessful.txt").ReadLine());

            numberSuccessful = numberSuccessfulPrev + numberSuccessful;

            StreamWriter s = new StreamWriter("nSuccessful.txt", false);

            // Record the new number of successful emails if no errors
            if (errorEmails.Count == 0) {
                s.WriteLine(numberSuccessful);
            } else {
                // Reset textfile error counter to 0 if there was errors
                s.WriteLine(0);
            }

            s.Close();

            // Calculate useful statistics to be added to top of email
            TextPart header = new TextPart("Plain") {
                Text = string.Format(
                "This email was automatically forwarded by a script.\n" +
                "Statistics:\n\tTotal unread errors: {0}\n\tTotal unread unique emails: {1}\n\t" +
                "DateTime script was run: {2}\n\tNumber of consecutive successful backups before this error: {3}\n" +
                "Begin message content:\n",
                errorEmails.Count, uniqueEmails.Count, DateTime.Now, numberSuccessful)
            };

            // Create container for email content
            Multipart bodyContent = new Multipart("mixed");
            bodyContent.Add(header);

            // Connect to server
            SmtpClient connection = new SmtpClient();
            connection.Connect(this.serverName, this.outgoingPort);
            connection.Authenticate(this.userName, this.password);

            // Add content to and send first, error message
            MimeMessage outgoingMessage = new MimeMessage();

            outgoingMessage.From.Add(new MailboxAddress("Email Checker", this.userName));
            outgoingMessage.To.Add(new MailboxAddress(adminName, this.recipient));

            outgoingMessage.Subject = string.Format("{0}, there is an error in the backup.", adminName);

            bodyContent = new Multipart("mixed");
            bodyContent.Add(header);

            foreach (MimeMessage error in errorEmails) {
                // Add some spacing between header and also between emails
                bodyContent.Add(new TextPart("Plain") { Text = "\n----------------------------\n\n" });
                // Add content of error message to email
                bodyContent.Add(error.Body);
            }

            // Add constructed email body to email and send it, if there was errors
            outgoingMessage.Body = bodyContent;
            if (errorEmails.Count > 0) {
                connection.Send(outgoingMessage);
            }

            // Do the same thing for the unique emails
            outgoingMessage = new MimeMessage();

            outgoingMessage.From.Add(new MailboxAddress("Email Checker", this.userName));
            outgoingMessage.To.Add(new MailboxAddress(adminName, this.recipient));

            outgoingMessage.Subject = string.Format("{0}, you have new IT emails.", adminName);

            bodyContent = new Multipart("mixed");
            bodyContent.Add(header);

            foreach (MimeMessage unique in uniqueEmails) {
                // Add some spacing between header and also between emails
                bodyContent.Add(new TextPart("Plain") { Text = "\n----------------------------\n\n" });
                // Add content of unique message to email
                bodyContent.Add(unique.Body);
            }

            // Add constructed email body to email and send it, if there was errors
            outgoingMessage.Body = bodyContent;
            if (uniqueEmails.Count > 0) {
                connection.Send(outgoingMessage);
            }
        }

    }
}