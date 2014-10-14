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

        public static List<SYS_EMAIL> DARSubmittedEmail(DAILY_ACTIVITY_HEADER _header)
        {
            SYS_MODULES _module = SYS_MODULES.GetModules().Where(x => x.MODULE_NAME == "Daily Activity").SingleOrDefault();
            List<long> _projectManagerList = PA.GetProjectManagersList((long)_header.PROJECT_ID);
            PROJECTS_V _ProjectInformation = new PROJECTS_V();
            List<SYS_EMAIL> _email = new List<SYS_EMAIL>();

            if(_module != null)
            {
                if(_projectManagerList.Count() > 0)
                {
                    foreach (long _projectManager in _projectManagerList)
                    {
                        SYS_USER_INFORMATION _userInformation = new SYS_USER_INFORMATION();
                        
                        using(Entities _context = new Entities())
                        {
                        //Return user information from project manager
                            _userInformation = _context.SYS_USER_INFORMATION.Where(x => x.PERSON_ID == _projectManager).SingleOrDefault();
                            _ProjectInformation = _context.PROJECTS_V.Where(p => p.PROJECT_ID == _header.PROJECT_ID).SingleOrDefault();
                        }

                        string _emailBody = string.Format(@"<html><head><title></title><style type='text/css'>
            .auto-style1
            {
                width: 100%;
            }
            .auto-style2
            {
                font-family: Arial, Helvetica, sans-serif;
                font-size: x-small;
            }
            .auto-style3
            {
                font-family: Arial, Helvetica, sans-serif;
                font-size: x-small;
                font-weight: bold;
                border-style: solid;
                border-width: 1px;
                padding: 1px 4px;
                background-color: #CCCCCC;
            }
            .auto-style4
            {
                border-style: solid;
                border-width: 1px;
                padding: 1px 4px;
            }
        </style>
    </head>
    <body>
    <p>
        <span class='auto-style2'>The following daily activity sheet was created in EMSv2 and needs to be approved. </span>
        <br class='auto-style2' />
        <span class='auto-style2'>You are listed as the project manager for this project. If that information is incorrect, please have your project functional owner change it in Oracle.</span></p>
    <table class='auto-style1'>
        <tr>
            <td class='auto-style3'>DRS Number</td>
            <td class='auto-style3'>Project Number</td>
            <td class='auto-style3'>Project Name</td>
            <td class='auto-style3'>Project Description</td>
            <td class='auto-style3'>Daily Activity Date</td>
        </tr>
        <tr>
            <td class='auto-style4'>{0}</td>
            <td class='auto-style4'>{1}</td>
            <td class='auto-style4'>{2}</td>
            <td class='auto-style4'>{3}</td>
            <td class='auto-style4'>{4}</td>
        </tr>
    </table>
</body>
</html>", _header.DA_HEADER_ID.ToString(), _ProjectInformation.SEGMENT1, _ProjectInformation.NAME, _ProjectInformation.LONG_NAME, _header.DA_DATE.ToString());

                        if (_userInformation != null)
                        {
                            SYS_EMAIL _mail = new SYS_EMAIL();
                            _mail.TYPE_ID = (long)MESSAGE_TYPE.DAR_SUBMITTED;
                            _mail.MODULE_ID = (long)_module.MODULE_ID;
                            _mail.SUBJECT_DESC = "EMS - Daily Activity Sheet Submitted For Your Approval";
                            _mail.EMAIL_ADDRESS = _userInformation.EMAIL_ADDRESS;
                            _mail.USER_ID = _userInformation.USER_ID;
                            _mail.CREATED_DATE = DateTime.Now;
                            _mail.MESSAGE_BODY = _emailBody;
                            _email.Add(_mail);
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
