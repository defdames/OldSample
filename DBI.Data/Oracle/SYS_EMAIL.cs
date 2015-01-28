using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBI.Data;

using DBI.Core.Web;
using System.Net.Mail;
using System.IO;
using System.Reflection;

namespace DBI.Data
{
    public partial class SYS_EMAIL
    {
        public enum MESSAGE_TYPE
        {
            [Description("Generates Email to Project Manager for Submitted DAR")]
            DAR_SUBMITTED = 1
        };

        public static void GenerateEmailAndProcess(List<SYS_EMAIL> dataIn)
        {
            GenericData.Insert<SYS_EMAIL>(dataIn);

            foreach (SYS_EMAIL _email in dataIn)
            {
                SendMessage(_email.EMAIL_ADDRESS, "noreply@dbiservices.com", _email.SUBJECT_DESC + " - EMSv2TESTING", _email.MESSAGE_BODY);
                //SendMessage("ljankowski@dbiservices.com", "noreply@dbiservices.com", _email.SUBJECT_DESC + "   " + _email.EMAIL_ADDRESS, _email.MESSAGE_BODY);
                _email.MESSAGE_SENT = "Y";
                _email.DATE_SENT = DateTime.Now;
            }
         
            //Update the email to sent datetime
            GenericData.Update<SYS_EMAIL>(dataIn);
        }

        public static List<SYS_EMAIL> DARSubmittedEmail(long _headerID)
        {
            SYS_MODULES _module = SYS_MODULES.GetModules().Where(x => x.MODULE_NAME == "Daily Activity").SingleOrDefault();
            DAILY_ACTIVITY_HEADER _header = new DAILY_ACTIVITY_HEADER();
            List<long> _projectManagerList = new List<long>();
            PROJECTS_V _ProjectInformation = new PROJECTS_V();
            List<SYS_EMAIL> _email = new List<SYS_EMAIL>();

            using (Entities _context = new Entities())
            {

                if (_module != null)
                {
                    _header = _context.DAILY_ACTIVITY_HEADER.Where(x => x.HEADER_ID == _headerID).SingleOrDefault();
                    _projectManagerList = PA.GetProjectManagersList((long)_header.PROJECT_ID);

                    if (_projectManagerList.Count() > 0)
                    {
                        foreach (long _projectManager in _projectManagerList)
                        {
                            SYS_USER_INFORMATION _userInformation = new SYS_USER_INFORMATION();

                            //Return user information from project manager
                            _userInformation = _context.SYS_USER_INFORMATION.Where(x => x.PERSON_ID == _projectManager).SingleOrDefault();
                            _ProjectInformation = _context.PROJECTS_V.Where(p => p.PROJECT_ID == _header.PROJECT_ID).SingleOrDefault();

                            var _emailHTML = GetResourceTextFile("DARSubmittedEmail.html");

                            string _daHeaderID = _headerID.ToString();
                            string _segment =  _ProjectInformation.SEGMENT1.ToString();
                            string _projectName =  _ProjectInformation.NAME.ToString();
                            string _projectLongName = _ProjectInformation.LONG_NAME.ToString();
                            string _da_date = _header.DA_DATE.Value.ToLongDateString();

                            string _emailBody = string.Format(_emailHTML, _daHeaderID, _segment, _projectName, _projectLongName, _da_date);

                            if (_userInformation != null)
                            {
                                SYS_EMAIL _mail = new SYS_EMAIL();
                                _mail.TYPE_ID = (long)MESSAGE_TYPE.DAR_SUBMITTED;
                                _mail.MODULE_ID = (long)_module.MODULE_ID;
                                _mail.SUBJECT_DESC = "EMS - Daily Activity Sheet Submitted For Your Approval";
                                _mail.EMAIL_ADDRESS = _userInformation.EMAIL_ADDRESS;
                                _mail.USER_ID = _userInformation.USER_ID;
                                _mail.CREATED_DATE = DateTime.Now;
                                _mail.MESSAGE_VIEWED = "N";
                                _mail.MESSAGE_SENT = "N";
                                _mail.MESSAGE_BODY = _emailBody;
                                _email.Add(_mail);
                            }
                        }
                    }
                }
            }

            return _email;
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

        public static string GetResourceTextFile(string filename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "DBI.Data.Oracle." + filename;
            //string[] resourceNames = Assembly.GetExecutingAssembly().GetManifestResourceNames();
            string returnValue = "";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    returnValue = reader.ReadToEnd();
                }
            }

            return returnValue;
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
