using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DBI.Data;
using DBI.Core.Web;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umOpenBudgetType : DBI.Core.Web.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (!validateComponentSecurity("SYS.OverheadBudget.Security"))
                {
                    X.Redirect("~/Views/uxDefault.aspx");
                }

            }
        }

        protected void deLoadBudgetNames(object sender, StoreReadDataEventArgs e)
        {
            long _businessUnitId = 0;

            if (Request.QueryString["buID"] != "" && Request.QueryString["buID"] != null)
                _businessUnitId = long.Parse(Request.QueryString["buID"]);

            uxBudgetNameStore.DataSource = OVERHEAD_BUDGET_TYPES.NextAvailBudgetTypeByOrganization(_businessUnitId, long.Parse(uxFiscalYear.SelectedItem.Value));
        }

        protected void deLoadFiscalYears(object sender, StoreReadDataEventArgs e)
        {
            uxFiscalYearsStore.DataSource = PA.AllFiscalYears().OrderByDescending(x => x.ID_NAME).Take(2);
        }
    }
}