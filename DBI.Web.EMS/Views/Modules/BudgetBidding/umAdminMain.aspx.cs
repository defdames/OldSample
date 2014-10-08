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
    public partial class umAdminMain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (!BasePage.validateComponentSecurity("SYS.BudgetBidding.Security"))
                {
                    X.Redirect("~/Views/uxDefault.aspx");
                }
            }
        }
    }
}