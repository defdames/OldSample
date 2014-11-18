using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;


namespace OpenIncidentCheckConsole
{
    class OpenIncidentCheck
    {
        static void Main(string[] args)
        {
            //Create the mail message 
        MailMessage mail = new MailMessage(); 
        mail.Subject = "RMCC Incident Alert"; 
        mail.Body = "An Incident has been open for three days";

            //the displayed "from" email address 
        mail.From = new System.Net.Mail.MailAddress("brian.scully@dbiservices.com");  

        mail.IsBodyHtml = false; 
        mail.BodyEncoding = System.Text.Encoding.Unicode; 
        mail.SubjectEncoding = System.Text.Encoding.Unicode; 
            //Add one or more addresses that will receive the mail 
        mail.To.Add("Brian.Scully@dbiservices.com");

            //create the credentials 
        NetworkCredential cred = new NetworkCredential( 
        "brian.scully@dbiservices.com", //from email address of the sending account 
        "password"); //password of the sending account

            //create the smtp client...these settings are for outlook
        SmtpClient smtp = new SmtpClient("smpt.outlook.com");
        smtp.UseDefaultCredentials = false;
        smtp.EnableSsl = true;

            //credentials (username, pass of sending account) assigned here 
        smtp.Credentials = cred;  
        smtp.Port = 587;

            //let her rip 
        smtp.Send(mail);
        }
    }
}
