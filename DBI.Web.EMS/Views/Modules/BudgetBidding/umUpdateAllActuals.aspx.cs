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
    public partial class umUpdateAllActuals : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (!BasePage.validateComponentSecurity("SYS.BudgetBidding.View"))
                {
                    X.Redirect("~/Views/uxDefault.aspx");
                }
            }
        }

        protected void deLoadWEDateDropdown(object sender, StoreReadDataEventArgs e)
        {
            long hierID = Convert.ToInt64(Request.QueryString["hierID"]);
            uxWEDateStore.DataSource = BB.LoadedWEDates(hierID, true, 5);
        }

        protected void deCheckAllowUpdate(object sender, DirectEventArgs e)
        {
            if (uxWEDate.Text == "")
            {
                uxUpdate.Disable();
            }
            else
            {
                uxUpdate.Enable();
            }
        }

        protected void deUpdate(object sender, DirectEventArgs e)
        {
            string hierID = Request.QueryString["hierID"];
            long leOrgID = long.Parse(Request.QueryString["leOrgID"]);
            long yearID = long.Parse(Request.QueryString["yearID"]);
            long orgID = Convert.ToInt64(Request.QueryString["orgID"]);
            string jcDate = uxWEDate.Text;

            BBSummary.DBUpdateAllJCNums(hierID, orgID, leOrgID, yearID, jcDate);
            X.Js.Call("closeUpdate");
        }
    }
}