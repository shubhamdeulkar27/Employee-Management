using System;
using MailKit.Net.Smtp;
using MimeKit;
namespace MSMQ
{
    /// <summary>
    /// Class For Sending Email.
    /// </summary>
    public class SMTP
    {
        /// <summary>
        /// Function For Sending Email.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mail"></param>
        /// <param name="data"></param>
        public void SendMail(string name, string mail, string data)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(name,mail));
                message.To.Add(new MailboxAddress("Employee Management", "shubhamdeulkar27@gmail.com"));
                message.Subject = "Registration";
                message.Body = new TextPart("plain")
                {
                    Text = data
                };

                using(var client = new SmtpClient())
                {
                    client.Connect("smtp.gmail.com",587,false);
                    client.Authenticate("shubhamdeulkar27@gmail.com", "nsdeulkar27");
                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
