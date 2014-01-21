using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DBI.Core.Web
{
    public class Mailer
    {
        public static void SendMessage(string MailTo, string Subject, string Message, bool IsHtml, string MailFrom)
        {
            MailMessage MessageToSend = new MailMessage(MailFrom, MailTo)
            {
                Subject = Subject,
                Body = Message,
                IsBodyHtml = IsHtml
            };

            MailSend(MessageToSend);
        }

        public static void SendMessage(string MailTo, string Subject, string Message, bool IsHtml)
        {
            string MailFrom =  HttpContext.Current.User.Identity.Name + "@dbiservices.com";
            MailMessage MessageToSend = new MailMessage(MailFrom, MailTo)
            {
                Subject = Subject,
                Body = Message,
                IsBodyHtml = IsHtml
            };

            MailSend(MessageToSend);
               
            
        }

        public static void SendMessage(string MailTo, string Subject, string Message, bool IsHtml, Attachment MailAttachment)
        {
            string MailFrom = HttpContext.Current.User.Identity.Name + "@dbiservices.com";

            MailMessage MessageToSend = new MailMessage(MailFrom, MailTo)
            {
                Subject = Subject,
                Body = Message,
                IsBodyHtml = IsHtml
            };

            MessageToSend.Attachments.Add(MailAttachment);

            MailSend(MessageToSend);
        }

        public static void SendMessage(string MailTo, string Subject, string Message, bool IsHtml, string MailFrom, Attachment MailAttachment)
        {
            MailMessage MessageToSend = new MailMessage(MailFrom, MailTo)
            {
                Subject = Subject,
                Body = Message,
                IsBodyHtml = IsHtml
            };

            MessageToSend.Attachments.Add(MailAttachment);

            MailSend(MessageToSend);
        }

        public static void MailSend(MailMessage Message)
        {
            SmtpClient MailClient = new SmtpClient()
            {
                Host = "owa.dbiservices.com"
            };
#if DEBUG
            MailClient.Credentials = new System.Net.NetworkCredential("gene.lapointe@dbiservices.com", "Password");
#endif
            MailClient.Send(Message);
        }
    }
}
