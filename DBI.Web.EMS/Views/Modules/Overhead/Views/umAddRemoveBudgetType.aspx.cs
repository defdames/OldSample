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
            long _businessUnitId = 0;

            if (Request.QueryString["buID"] != "" && Request.QueryString["buID"] != null)
                _businessUnitId = long.Parse(Request.QueryString["buID"]);

            uxBudgetNameStore.DataSource = GL.BudgetTypesRemaining(_businessUnitId);
            uxBudgetNameStore.DataBind();

        }

        protected void deAssignBudgetType(object sender, DirectEventArgs e)
        {

            long _businessUnitId = 0;

            if (Request.QueryString["buID"] != "" && Request.QueryString["buID"] != null)
                _businessUnitId = long.Parse(Request.QueryString["buID"]);

            OVERHEAD_BUDGET_TYPE _data = new OVERHEAD_BUDGET_TYPE();
            _data.LE_ORG_ID = _businessUnitId;
            _data.BUDGET_NAME = uxBudgetName.SelectedItem.Value;
            _data.BUDGET_DESCRIPTION = uxBudgetDescription.Text;
            _data.CREATE_DATE = DateTime.Now;
            _data.MODIFY_DATE = DateTime.Now;
            _data.CREATED_BY = User.Identity.Name;
            _data.MODIFIED_BY = User.Identity.Name;

            GenericData.Insert<OVERHEAD_BUDGET_TYPE>(_data);

        }

    }
}