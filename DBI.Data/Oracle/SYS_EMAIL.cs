using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBI.Data;

using DBI.Core.Web;
using System.Net.Mail;

namespace DBI.Data
{
    public partial class SYS_EMAIL
    {
        public enum MESSAGE_TYPE
        {
            [Description("Generates Email to Project Manager for Submitted DAR")]
            DAR_SUBMITTED = 1
        };

        public static void GenerateEmailAndProcess(SYS_EMAIL dataIn)
        {
            GenericData.Insert<SYS_EMAIL>(dataIn);

            SendMessage(dataIn.EMAIL_ADDRESS,"NoReply-EMS@dbiservices.com", dataIn.SUBJECT_DESC, dataIn.MESSAGE_BODY);

            dataIn.MESSAGE_SENT = "Y";
            dataIn.DATE_SENT = DateTime.Now;
         
            //Update the email to sent datetime
            GenericData.Update<SYS_EMAIL>(dataIn);
        }

        public static void SendMessage(string MailTo, string MailFrom, string Subject, string Message)
        {
            MailMessage MessageToSend = new MailMessage(MailFrom, MailTo)
            {
                Subject = Subject,
                Body = Message,
                IsBodyHtml = true
            };

            MailSend(MessageToSend);
        }

        public static void MailSend(MailMessage Message)
        {
            SmtpClient MailClient = new SmtpClient()
            {
                Host = "smtp.dbiservices.com"
            };

            MailClient.Send(Message);
        }

    }
}
