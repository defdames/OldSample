using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using DBI.Data;
using DBI.Core.Web;

namespace DBI.Web.EMS.Views.Modules.BudgetBidding
{
    public partial class umReportCompareTo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (!BasePage.validateComponentSecurity("SYS.BudgetBidding.Security"))
                {
                    X.Redirect("~/Views/uxDefault.aspx");
                }

                string reportName = Request.QueryString["report"];
                uxHidReport.Text = reportName;

                uxYearStore.Data = PA.AllFiscalYears();
                long hierID = Convert.ToInt64(Request.QueryString["hierID"]);
                uxVersionStore.Data = BB.BudgetVersions();
            }
        }

        protected void deCheckAllowRun(object sender, DirectEventArgs e)
        {
            if (uxYear.Text == "" || uxVersion.Text == "")
            {
                uxRun.Disable();
            }
            else
            {
                uxRun.Enable();
            }
        }

        protected void deRun(object sender, DirectEventArgs e)
        {
            X.Js.Call("closeCompare");
        }
    }
}