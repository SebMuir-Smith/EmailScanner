using System.Collections.Generic;
using MailKit.Net.Smtp;
using MimeKit;


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

        public bool SendEmails(List<MimeMessage> errorEmails, List<MimeMessage> uniqueEmails){

            SmtpClient connection = new SmtpClient();

            // Connect to server
            connection.Connect(this.serverName, this.outgoingPort);

            connection.Authenticate(this.userName, this.password);

            MimeMessage outgoingMessage;
            foreach (MimeMessage error in errorEmails){
                outgoingMessage = new MimeMessage();

                outgoingMessage.From.Add(new MailboxAddress("email_from", this.userName));
                outgoingMessage.To.Add(new MailboxAddress("email_to", this.recipient));
                
                outgoingMessage.Subject = string.Format("{0}, there is an error in the backup.",adminName);

                outgoingMessage.Body = error.Body;

                connection.Send(outgoingMessage);
            }

            foreach (MimeMessage unique in uniqueEmails){
                outgoingMessage = new MimeMessage();

                outgoingMessage.From.Add(new MailboxAddress("email_from", this.userName));
                outgoingMessage.To.Add(new MailboxAddress("email_to", this.recipient));
                
                outgoingMessage.Subject = string.Format("{0}, you have a new IT email.",adminName);

                outgoingMessage.Body = unique.Body;

                connection.Send(outgoingMessage);
            }

            return true;
        }
       
    }
}