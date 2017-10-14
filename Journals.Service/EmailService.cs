using Journals.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

namespace Journals.Service
{
    public class EmailService : IEmailService
    {

        private MailAddress _from;
        private static Semaphore mailSendSemaphore = new Semaphore(10, 10);
        private int _port;
        private string _host;
        private NetworkCredential _credential;

        public EmailService(string senderEmail, string senderName, string host, int port, string UserName, string Password)
        {
            _port = port;
            _host = host;
            _credential = new System.Net.NetworkCredential(UserName, Password);
            _from = new MailAddress(senderEmail, senderName, Encoding.UTF8);
        }
        public void SendEmail(Dictionary<string, string> emails, string subject, string content)
        {
            SmtpClient client = new SmtpClient();
            client.Port = _port;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Host = _host;
            client.EnableSsl = true;
            client.Credentials = _credential;
            MailMessage message = new MailMessage();
            foreach (KeyValuePair<string, string> entry in emails)
                message.To.Add(new MailAddress(entry.Value, entry.Key));
            message.From = _from;
            message.IsBodyHtml = true;
            message.Body = content;
            message.Subject = subject;
            string userState = subject;
            client.SendAsync(message, userState);
            message.Dispose();
        }
    }
}
