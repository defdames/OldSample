using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace DBI.Web.EMS
{
    public class Global : System.Web.HttpApplication
    {
        // Used to set the default instance of the connection String Values
        public const string serverInstance = "Prod";

        protected void Application_Start(object sender, EventArgs e)
        {
            // Set the license key needed to hide "warning" windows when deploying to server
            Application["Ext.Net.LicenseKey"] = "ODUwODg1NjAsMiw5OTk5LTEyLTMx";
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }


        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
          
        }
    }
}