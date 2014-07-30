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

        protected void deOrganizationList(object sender, StoreReadDataEventArgs e)
        {

            long _orgID;
            bool checkOrgId = long.TryParse(e.Parameters["ORGANIZATION_ID"], out _orgID);
            
            List<GL_ACCOUNTS_V> _organizationAccountList = new List<GL_ACCOUNTS_V>();

                using (Entities _context = new Entities())
                {
                    var _rangeList = OVERHEAD_MODULE.OverheadGLRangeByOrganizationId(_orgID,_context).ToList();

                    foreach (OVERHEAD_GL_RANGE_V _range in _rangeList)
                    {
                        var _accountList = GL_ACCOUNTS_V.AccountListByRange(_range.GL_RANGE_ID, _context);
                        _organizationAccountList.AddRange(_accountList.ToList());
                    }
                }

            int count;
            uxOrganizationAccountStore.DataSource = GenericData.ListFilterHeader<GL_ACCOUNTS_V>(e.Start, 1000, e.Sort, e.Parameters["filterheader"], _organizationAccountList.AsQueryable(), out count);
            e.Total = count;

        }

        
        protected void deSelectOrganization(object sender, DirectEventArgs e)
        {
            Ext.Net.ParameterCollection ps = new Ext.Net.ParameterCollection();
            Ext.Net.StoreParameter _p = new Ext.Net.StoreParameter();
            _p.Mode = ParameterMode.Value;
            _p.Name = "ORGANIZATION_ID";
            _p.Value = e.ExtraParams["ORGANIZATION_ID"];
            ps.Add(_p);

            uxOrganizationAccountStore.Reload(ps);
            uxGlAccountSecurityGrid.Collapse();
        }

        protected void deDeSelectOrganization(object sender, DirectEventArgs e)
        {
            uxOrganizationAccountStore.Reload();
        }

    }
}