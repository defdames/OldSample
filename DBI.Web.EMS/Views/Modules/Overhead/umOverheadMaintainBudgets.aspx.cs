using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Data;
using Ext.Net;
using DBI.Core.Web;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umOverheadMaintainBudgets : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (!validateComponentSecurity("SYS.OverheadBudget.Maintenance"))
                {
                    X.Redirect("~/Views/uxDefault.aspx");
                }

            }
        }

        protected void deLoadOrganizationsForUser(object sender, StoreReadDataEventArgs e)
        {
            List<long> OrgsList = SYS_USER_ORGS.GetUserOrgs(SYS_USER_INFORMATION.UserID(User.Identity.Name)).Select(x => x.ORG_ID).ToList();

            List<OVERHEAD_ORG_BUDGETS_V> _budgetsByOrganizationIDList = new List<OVERHEAD_ORG_BUDGETS_V>();

            using (Entities _context = new Entities())
            {
                foreach(long _orgID in OrgsList)
                {
                 _budgetsByOrganizationIDList.AddRange(OVERHEAD_ORG_BUDGETS.BudgetListByOrganizationID(_orgID, _context).OrderBy(x => x.ORG_BUDGET_ID).ToList());
                }
            }

            if (uxViewAllToggleButton.Pressed)
                _budgetsByOrganizationIDList = _budgetsByOrganizationIDList.Where(x => x.BUDGET_STATUS == "Open" || x.BUDGET_STATUS == "Pending").ToList();

            int count;
            uxBudgetVersionByOrganizationStore.DataSource = GenericData.EnumerableFilterHeader<OVERHEAD_ORG_BUDGETS_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _budgetsByOrganizationIDList, out count);
            e.Total = count;
        }

        protected void deToggleView(object sender, DirectEventArgs e)
        {
            uxBudgetVersionByOrganizationStore.Reload();
        }

      
        protected void deSelectOrganization(object sender, DirectEventArgs e)
        {
            string _organization_id = e.ExtraParams["ORGANIZATION_ID"];
            string _organization_name = e.ExtraParams["ORGANIZATION_NAME"];
            string _fiscalYear = e.ExtraParams["FISCAL_YEAR"];
            string _description = e.ExtraParams["BUDGET_DESCRIPTION"];
            string _budget_id = uxBudgetVersionByOrganizationSelectionModel.SelectedRow.RecordID;

            X.Js.Call("parent.App.direct.AddTabPanel", "bmw" + _organization_id, _organization_name + " - " + "Budget Maintenance / " + _fiscalYear + " / " + _description, "~/Views/Modules/Overhead/umEditBudget.aspx?orgid=" + _organization_id + "&fiscalyear=" + _fiscalYear + "&budget_id=" + _budget_id);

        }

        public class GL_PERIODS_V
        {
            public string ENTERED_PERIOD_NAME { get; set; }
            public short PERIOD_YEAR { get; set; }
            public short PERIOD_NUM { get; set; }
            public string PERIOD_TYPE { get; set; }
            public DateTime START_DATE { get; set; }
            public DateTime END_DATE { get; set; }
        }

    }
}