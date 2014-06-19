using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DBI.Core.Web;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.Overhead.Views
{
    public partial class umAddRemoveBudgetType : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!validateComponentSecurity("SYS.OverheadBudget.Security"))
            {
                X.Redirect("~/Views/uxDefault.aspx");

            }
        }


        protected void deLoadBudgetNames(object sender, StoreReadDataEventArgs e)
        {
            string _businessUnitId = Request.QueryString["buID"].ToString();
            string _recordId = string.Empty;

            if (Request.QueryString["recordId"] != "" && Request.QueryString["recordId"] != null)
            {
                _recordId = Request.QueryString["recordId"].ToString();
            }

            uxBudgetNameStore.DataSource = GL.BudgetTypesRemaining(long.Parse(_businessUnitId), 0);
            uxBudgetNameStore.DataBind();

        }

    }
}