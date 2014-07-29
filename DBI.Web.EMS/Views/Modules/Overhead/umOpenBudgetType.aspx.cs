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
            long _leID = 0;

            if (Request.QueryString["leID"] != "" && Request.QueryString["leID"] != null)
                _leID = long.Parse(Request.QueryString["leID"]);

            long _orgID = 0;

            if (Request.QueryString["orgID"] != "" && Request.QueryString["orgID"] != null)
                _orgID = long.Parse(Request.QueryString["orgID"]);

            var _data = OVERHEAD_BUDGET_TYPES.NextAvailBudgetTypeByOrganization(_orgID,_leID, long.Parse(uxFiscalYear.SelectedItem.Value));

            if (_data.Count() > 0)
            {
                uxBudgetNameStore.DataSource = _data;
                uxOpenPeriod.Enable();
            }

        }

        protected void deLoadFiscalYears(object sender, StoreReadDataEventArgs e)
        {
            uxFiscalYearsStore.DataSource = PA.AllFiscalYears().OrderByDescending(x => x.ID_NAME).Take(2);
        }

        protected void deOpenPeriod(object sender, DirectEventArgs e)
        {
            long _businessUnitId = 0;

            if (Request.QueryString["orgID"] != "" && Request.QueryString["orgID"] != null)
                _businessUnitId = long.Parse(Request.QueryString["orgID"]);


            OVERHEAD_ORG_BUDGETS _budget = new OVERHEAD_ORG_BUDGETS();
            _budget.ORGANIZATION_ID = _businessUnitId;
            _budget.STATUS = "O";
            _budget.OVERHEAD_BUDGET_TYPE_ID = long.Parse(uxBudgetName.SelectedItem.Value);
            _budget.CREATE_DATE = DateTime.Now;
            _budget.MODIFY_DATE = DateTime.Now;
            _budget.CREATED_BY = User.Identity.Name;
            _budget.MODIFIED_BY = User.Identity.Name;
            _budget.FISCAL_YEAR = short.Parse(uxFiscalYear.SelectedItem.Value);
            GenericData.Insert<OVERHEAD_ORG_BUDGETS>(_budget);

        }
    }
}