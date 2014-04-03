using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using System.Security.Claims;
using DBI.Core.Security;
using DBI.Data;
using Ext.Net;
using DBI.Data.DataFactory;

namespace DBI.Web.EMS.Views.Modules.TimeClock
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string username = Authentication.GetClaimValue("EmployeeNumber", User as ClaimsPrincipal);
            
            txtUser_Name.Text = username;
        }


        protected void deSetTimeIn(object sender, DirectEventArgs e)
        {
            txtTime_In.Text = DateTime.Now.ToString();
        
        }

        protected void deSetTimeOut(object sender, DirectEventArgs e)
        {
            txtTime_Out.Text = DateTime.Now.ToString();

        }


    }
}