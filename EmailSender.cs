using System.Collections.Generic;
using MailKit;
using MimeKit;


namespace EmailScanner
{
    public class EmailSender : EmailInfo
    {


        public EmailSender(string[] args):base(args){



        }

        public bool SendEmails(List<MimeMessage> errorEmails, List<MimeMessage> uniqueEmails){

            return true;
        }
       
    }
}