using System;
using System.Configuration;
using System.Net.Mail;

namespace BL
{
    public class MailHelperBL
    {
        public void SendEmail(string email, string name,string body,string subject)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(ConfigurationManager.AppSettings["SenderMailID"], "Arth Support");
                mail.To.Add(new MailAddress(email, name));
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                using (SmtpClient MailClient = new SmtpClient(ConfigurationManager.AppSettings["Host"], Convert.ToInt32(ConfigurationManager.AppSettings["SenderMailPort"])))
                {
                    MailClient.EnableSsl = false;
                    MailClient.UseDefaultCredentials = false;
                    MailClient.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SenderMailID"], ConfigurationManager.AppSettings["SenderMailPassword"]);
                    MailClient.Send(mail);
                }
            }
        }
    }
}
