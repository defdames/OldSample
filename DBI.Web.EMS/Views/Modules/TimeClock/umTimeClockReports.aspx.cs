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
using DBI.Core;

namespace DBI.Web.EMS.Views.Modules.TimeClock
{
    public partial class umTimeClockReports : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (!validateComponentSecurity("SYS.TimeClock.Payroll"))
                {
                    X.Redirect("~/Views/uxDefault.aspx");
                }
            }

        }
    }
}