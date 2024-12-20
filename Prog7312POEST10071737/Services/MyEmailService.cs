﻿using EASendMail;
using System;

namespace Prog7312POEST10071737.Services
{
    public class MyEmailService
    {

        /// <summary>
        /// declare variables
        /// </summary>
        private string senderEmail = Properties.Resources.SenderEmail;
        private string receiverEmail;
        private string senderPassword = Properties.Resources.SenderPassword;
        //___________________________________________________________________________________________________________

        /// <summary>
        /// method to send email
        /// </summary>
        /// <param name="subject"></param>
        /// <param name="Body"></param>
        public void EmailSender(string email, string subject, string Body)
        {
            var SingletonService = UserSingleton.Instance;
            if (email.Equals(""))
            {
                receiverEmail = SingletonService.GetEmail();
            }
            else
            {
                receiverEmail = email;
            }


            try
            {
                SmtpMail oMail = new SmtpMail("TryIt");

                // Your gmail email address
                oMail.From = senderEmail;
                // Set recipient email address
                oMail.To = receiverEmail;

                // Set email subject
                oMail.Subject = subject;
                // Set email body
                oMail.TextBody = Body;

                // Gmail SMTP server address
                SmtpServer oServer = new SmtpServer("smtp.gmail.com");
                // Gmail user authentication
                // For example: your email is "gmailid@gmail.com", then the user should be the same
                oServer.User = senderEmail;

                // Create app password in Google account
                // https://support.google.com/accounts/answer/185833?hl=en
                oServer.Password = senderPassword;

                // Set 587 port, if you want to use 25 port, please change 587 5o 25
                oServer.Port = 465;

                // detect SSL/TLS automatically
                oServer.ConnectType = SmtpConnectType.ConnectSSLAuto;


                SmtpClient oSmtp = new SmtpClient();
                oSmtp.SendMail(oServer, oMail);


            }
            catch (Exception ep)
            {

            }
        }

        //___________________________________________________________________________________________________________
    }
}
//____________________________________EOF_________________________________________________________________________