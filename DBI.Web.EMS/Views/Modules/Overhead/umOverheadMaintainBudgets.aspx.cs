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

            short _fiscalYear;
            bool checkYear = short.TryParse(e.Parameters["FISCAL_YEAR"], out _fiscalYear);

            long _budgetID = long.Parse(uxBudgetVersionByOrganizationSelectionModel.SelectedRow.RecordID);
            
            List<GL_ACCOUNTS_V> _organizationAccountList = new List<GL_ACCOUNTS_V>();

                using (Entities _context = new Entities())
                {
                    var _rangeList = OVERHEAD_MODULE.OverheadGLRangeByOrganizationId(_orgID,_context).ToList();

                    //We have to make sure all the budgets exist in the system for each period.
                    string sql = "select entered_period_name,period_year,period_num,period_type,start_date,end_date from gl.gl_periods where period_set_name = 'DBI Calendar' order by period_num";
                    List<GL_PERIODS_V> _glPeriods = _context.Database.SqlQuery<GL_PERIODS_V>(sql).Where(x => x.PERIOD_TYPE == "Month" & x.PERIOD_YEAR == _fiscalYear).ToList();

                    foreach (OVERHEAD_GL_RANGE_V _range in _rangeList)
                    {                          
                        var _accountList = GL_ACCOUNTS_V.AccountListByRange(_range.GL_RANGE_ID, _context);

                        foreach(GL_ACCOUNTS_V _account in _accountList)
                        {
                            foreach (GL_PERIODS_V _period in _glPeriods)
                            {

                                int cnt = _context.OVERHEAD_BUDGET_DETAIL.Where(x => x.CODE_COMBINATION_ID == _account.CODE_COMBINATION_ID & x.ORG_BUDGET_ID == _budgetID & x.PERIOD == _period.ENTERED_PERIOD_NAME).Count();

                                //Does period already exist?
                                if (cnt == 0)
                                {
                                    //Create the record because it doesn't exits
                                    OVERHEAD_BUDGET_DETAIL _data = new OVERHEAD_BUDGET_DETAIL();
                                    _data.ORG_BUDGET_ID = _budgetID;
                                    _data.PERIOD = _period.ENTERED_PERIOD_NAME;
                                    _data.CREATE_DATE = DateTime.Now;
                                    _data.MODIFY_DATE = DateTime.Now;
                                    _data.AMOUNT = 0;
                                    _data.CODE_COMBINATION_ID = _account.CODE_COMBINATION_ID;
                                    _data.CREATED_BY = User.Identity.Name;
                                    _data.MODIFIED_BY = User.Identity.Name;
                                    GenericData.Insert<OVERHEAD_BUDGET_DETAIL>(_data);
                                }
                            } 
                        }

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

            Ext.Net.StoreParameter _p2 = new Ext.Net.StoreParameter();
            _p2.Mode = ParameterMode.Value;
            _p2.Name = "FISCAL_YEAR";
            _p2.Value = e.ExtraParams["FISCAL_YEAR"];
            ps.Add(_p2);

            uxOrganizationAccountStore.Reload(ps);
            uxGlAccountSecurityGrid.Collapse();
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

        protected void deDeSelectOrganization(object sender, DirectEventArgs e)
        {
            uxOrganizationAccountStore.Reload();
        }



        protected void deItemMaintenance(object sender, DirectEventArgs e)
        {
            string _budgetSelectedID = uxBudgetVersionByOrganizationSelectionModel.SelectedRow.RecordID;
            string _AccountSelectedID = uxOrganizationAccountSelectionModel.SelectedRow.RecordID;

            string url = "umAddOverheadDetailLine.aspx?budgetID=" + _budgetSelectedID + "&accountID=" + _AccountSelectedID;

            Window win = new Window
            {
                ID = "uxDetailLineMaintenance",
                Title = "Account Details",
                Height = 650,
                Width = 900,
                Modal = true,
                Resizable = false,
                CloseAction = CloseAction.Destroy,
                Loader = new ComponentLoader
                {
                    Mode = LoadMode.Frame,
                    DisableCaching = true,
                    Url = url,
                    AutoLoad = true,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win.Listeners.Close.Handler = "#{uxOrganizationAccountGridPanel}.getStore().load();";

            win.Render(this.Form);
            win.Show();

        }

    }
}