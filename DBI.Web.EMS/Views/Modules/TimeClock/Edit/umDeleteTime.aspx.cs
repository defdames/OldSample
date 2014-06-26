using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Data;
using Ext.Net;
using DBI.Core.Security;
using System.Security.Claims;

namespace DBI.Web.EMS.Views.Modules.TimeClock.Edit
{
    public partial class umDeleteTime : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void deDeleteTime(object sender, DirectEventArgs e)
        {
            string person_name = User.Identity.Name;// Authentication.GetClaimValue("EmployeeName", User as ClaimsPrincipal);
            string _TimeClockId = Request.QueryString["tcID"].ToString();
            string comment = txtDelComment.Text;

            TIME_CLOCK.DeleteEmployeeTime(decimal.Parse(_TimeClockId), comment, person_name);

        }
    }
}