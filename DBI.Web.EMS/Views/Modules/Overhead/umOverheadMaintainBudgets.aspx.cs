using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Data;
using Ext.Net;
using DBI.Core.Web;
using System.Text;

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

                //For Admin we need to show all organizations
                if (validateComponentSecurity("SYS.OverheadBudget.Security"))
                {
                    string _selectedRecordID = Request.QueryString["orgid"];
                    if (_selectedRecordID != null)
                    {
                        uxForecastMaintenance.Hidden = false;
                        uxImportActuals.Hidden = false;
                    }
                }
            }
        }

        protected void deLoadOrganizationsForUser(object sender, StoreReadDataEventArgs e)
        {
            List<long> OrgsList = SYS_USER_ORGS.GetUserOrgs(SYS_USER_INFORMATION.UserID(User.Identity.Name)).Select(x => x.ORG_ID).ToList();

            //For Admin we need to show all organizations
            if (validateComponentSecurity("SYS.OverheadBudget.Security"))
            {
                string _selectedRecordID = Request.QueryString["orgid"];

                if (_selectedRecordID != null)
                {

                    char[] _delimiterChars = { ':' };
                    string[] _selectedID = _selectedRecordID.Split(_delimiterChars);
                    long _hierarchyID = long.Parse(_selectedID[1].ToString());
                    long _organizationID = long.Parse(_selectedID[0].ToString());

                    OrgsList = HR.OverheadOrganizationStatusByHierarchy(_hierarchyID, _organizationID).Select(x => x.ORGANIZATION_ID).ToList();
                }
            }

            List<OVERHEAD_ORG_BUDGETS_V> _budgetsByOrganizationIDList = new List<OVERHEAD_ORG_BUDGETS_V>();

            StringBuilder _rangeString = new StringBuilder();

            using (Entities _context = new Entities())
            {
                foreach (long _orgID in OrgsList)
                {
                    _rangeString.Clear();

                    List<OVERHEAD_ORG_BUDGETS_V> _budgetList = OVERHEAD_ORG_BUDGETS.BudgetListByOrganizationID(_orgID, _context).OrderBy(x => x.ORG_BUDGET_ID).ToList();

                    foreach (OVERHEAD_ORG_BUDGETS_V _item in _budgetList)
                    {
                        List<OVERHEAD_GL_RANGE> _accountRanges = _context.OVERHEAD_GL_RANGE.Where(x => x.ORGANIZATION_ID == _item.ORGANIZATION_ID).ToList();
                        foreach (OVERHEAD_GL_RANGE _range in _accountRanges)
                        {
                            if(!_rangeString.ToString().Contains(_range.SRSEGMENT1.ToString() + "." + _range.SRSEGMENT2.ToString() + "." + _range.SRSEGMENT3.ToString() + "." + _range.SRSEGMENT4.ToString()))
                                _rangeString.AppendLine(_range.SRSEGMENT1.ToString() + "." + _range.SRSEGMENT2.ToString() + "." + _range.SRSEGMENT3.ToString() + "." + _range.SRSEGMENT4.ToString());
                        }

                        _item.ACCOUNT_RANGE = _rangeString.ToString();
                    }

                    _budgetsByOrganizationIDList.AddRange(_budgetList);
                }

            }

            if (uxHideClosedBudgetsCheckbox.Checked)
                _budgetsByOrganizationIDList = _budgetsByOrganizationIDList.Where(x => x.BUDGET_STATUS == "Open" || x.BUDGET_STATUS == "Pending").ToList();

            int count;
            uxBudgetVersionByOrganizationStore.DataSource = GenericData.EnumerableFilterHeader<OVERHEAD_ORG_BUDGETS_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _budgetsByOrganizationIDList, out count);
            e.Total = count;
        }

        protected void deHideClosed(object sender, DirectEventArgs e)
        {
            uxBudgetVersionByOrganizationStore.Reload();
        }

      
        protected void deSelectOrganization(object sender, DirectEventArgs e)
        {
            string _organization_id = e.ExtraParams["ORGANIZATION_ID"];
            string _organization_name = e.ExtraParams["ORGANIZATION_NAME"];
            string _fiscalYear = e.ExtraParams["FISCAL_YEAR"];
            string _description = e.ExtraParams["BUDGET_DESCRIPTION"];
            string _accountRange = e.ExtraParams["ACCOUNT_RANGE"];
            string _budget_id = uxBudgetVersionByOrganizationSelectionModel.SelectedRow.RecordID;

            X.Js.Call("parent.App.direct.AddTabPanel", "bmw" + _organization_id + _fiscalYear + _budget_id, _organization_name + " - " + "Budget Maintenance / " + _fiscalYear + " / " + _description + " (" + _accountRange + ")", "~/Views/Modules/Overhead/umEditBudget.aspx?orgid=" + _organization_id + "&fiscalyear=" + _fiscalYear + "&budget_id=" + _budget_id);

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