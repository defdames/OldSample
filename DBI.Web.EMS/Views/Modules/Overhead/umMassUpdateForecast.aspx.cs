using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DBI.Data;
using DBI.Core.Web;
using Ext.Net;
using System.Text;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umMassUpdateForecast : BasePage
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
            string _selectedRecordID = Request.QueryString["orgid"];

            if (_selectedRecordID != null)
            {

                char[] _delimiterChars = { ':' };
                string[] _selectedID = _selectedRecordID.Split(_delimiterChars);
                long _hierarchyID = long.Parse(_selectedID[1].ToString());
                long _organizationID = long.Parse(_selectedID[0].ToString());


                using (Entities _context = new Entities())
                {
                    var _data = _context.OVERHEAD_BUDGET_TYPE.Where(x => x.LE_ORG_ID == _organizationID).ToList();
                    if (_data.Count() > 0)
                    {
                        uxBudgetNameStore.DataSource = _data;
                        uxOpenPeriod.Enable();
                    }
                }

            }
        }

        protected void deLoadFiscalYears(object sender, StoreReadDataEventArgs e)
        {
            uxFiscalYearsStore.DataSource = PA.FiscalYearsGL().OrderByDescending(x => x.ID_NAME);
        }

        protected void deExecuteCommand(object sender, DirectEventArgs e)
        {
            string organizationName = e.ExtraParams["Name"];
            string organizationID = e.ExtraParams["ID"];
            string command = e.ExtraParams["command"];

            string _selectedRecordID = Request.QueryString["orgid"];
            char[] _delimiterChars = { ':' };
            string[] _selectedID = _selectedRecordID.Split(_delimiterChars);

            if (command == "periods")
            {

                string url = "~/Views/Modules/Overhead/umOverheadBudgetPeriods.aspx?orgID=" + organizationID + "&leID=" + _selectedID[0].ToString();
                Window win = new Window
                {
                    ID = "uxShowAccountRangeWindow",
                    Title = organizationName + " - " + "Budget Periods",
                    Height = 600,
                    Width = 750,
                    Modal = true,
                    Resizable = true,
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

                win.Render(this.Form);

                win.Show();


            }

        }

        protected void deOpenPeriod(object sender, DirectEventArgs e)
        {
            short _fiscalYear = short.Parse(uxFiscalYear.SelectedItem.Value);

            using (Entities _context = new Entities())
            {
                RowSelectionModel _rsm = uxOrganizationSelectionModel;

                foreach (SelectedRow _row in _rsm.SelectedRows)
                {
                    long _businessUnitId = 0;
                    _businessUnitId = long.Parse(_row.RecordID);

                    //find all budgets for the same year and organization and close them
                    List<OVERHEAD_ORG_BUDGETS> _organizationBudgets = _context.OVERHEAD_ORG_BUDGETS.Where(x => x.ORGANIZATION_ID == _businessUnitId & x.FISCAL_YEAR == _fiscalYear & x.STATUS != "C").ToList();

                    foreach (OVERHEAD_ORG_BUDGETS _budget in _organizationBudgets)
                    {
                        _budget.STATUS = "C";
                        _budget.MODIFY_DATE = DateTime.Now;
                        _budget.MODIFIED_BY = User.Identity.Name;
                    }

                    GenericData.Update<OVERHEAD_ORG_BUDGETS>(_organizationBudgets);

                    //Create the new budget for the selected type.
                    OVERHEAD_ORG_BUDGETS _newBudget = new OVERHEAD_ORG_BUDGETS();
                    _newBudget.ORGANIZATION_ID = _businessUnitId;
                    _newBudget.STATUS = "O";
                    _newBudget.OVERHEAD_BUDGET_TYPE_ID = long.Parse(uxBudgetName.SelectedItem.Value);
                    _newBudget.CREATE_DATE = DateTime.Now;
                    _newBudget.MODIFY_DATE = DateTime.Now;
                    _newBudget.CREATED_BY = User.Identity.Name;
                    _newBudget.MODIFIED_BY = User.Identity.Name;
                    _newBudget.FISCAL_YEAR = short.Parse(uxFiscalYear.SelectedItem.Value);
                    GenericData.Insert<OVERHEAD_ORG_BUDGETS>(_newBudget);

                    long _leID = 0;

                    string _selectedRecordID = Request.QueryString["orgid"];
                    char[] _delimiterChars = { ':' };
                    string[] _selectedID = _selectedRecordID.Split(_delimiterChars);
                    _leID = long.Parse( _selectedID[0].ToString());

                    OVERHEAD_BUDGET_TYPE _budgetTypeData = OVERHEAD_BUDGET_TYPE.BudgetTypes(_leID).Where(x => x.OVERHEAD_BUDGET_TYPE_ID == _newBudget.OVERHEAD_BUDGET_TYPE_ID).SingleOrDefault();

                    if (_budgetTypeData != null)
                    {
                        //Allow Copy of data since there is a parent

                        //Copy data from old budget to new budget

                        //Old Budget Data
                        //OVERHEAD_ORG_BUDGETS _budgetHeader = _context.OVERHEAD_ORG_BUDGETS.Where(x => x.FISCAL_YEAR == _fiscalYear & x.ORGANIZATION_ID == _businessUnitId & x.OVERHEAD_BUDGET_TYPE_ID == _budgetTypeData.PARENT_BUDGET_TYPE_ID).SingleOrDefault();


                        //List<OVERHEAD_BUDGET_DETAIL> _budgetDetail = _context.OVERHEAD_BUDGET_DETAIL.Where(x => x.ORG_BUDGET_ID == _budgetHeader.ORG_BUDGET_ID).ToList();

                        //foreach (OVERHEAD_BUDGET_DETAIL _line in _budgetDetail)
                        //{
                        //    OVERHEAD_BUDGET_DETAIL _d = new OVERHEAD_BUDGET_DETAIL();
                        //    _d.CREATE_DATE = DateTime.Now;
                        //    _d.MODIFY_DATE = DateTime.Now;
                        //    _d.CREATED_BY = User.Identity.Name;
                        //    _d.MODIFIED_BY = User.Identity.Name;
                        //    _d.ORG_BUDGET_ID = _budgetHeader.ORG_BUDGET_ID;
                        //    _d.PERIOD_NAME = _line.PERIOD_NAME;
                        //    _d.PERIOD_NUM = _line.PERIOD_NUM;
                        //    _d.CODE_COMBINATION_ID = _line.CODE_COMBINATION_ID;
                        //    _d.AMOUNT = _line.AMOUNT;
                        //    GenericData.Insert<OVERHEAD_BUDGET_DETAIL>(_d);
                        //}

                        //List<OVERHEAD_ACCOUNT_COMMENT> _comments = _context.OVERHEAD_ACCOUNT_COMMENT.Where(x => x.ORG_BUDGET_ID == _budgetHeader.ORG_BUDGET_ID).ToList();

                        //foreach (OVERHEAD_ACCOUNT_COMMENT _comment in _comments)
                        //{
                        //    OVERHEAD_ACCOUNT_COMMENT _c = new OVERHEAD_ACCOUNT_COMMENT();
                        //    _c.CODE_COMBINATION_ID = _comment.CODE_COMBINATION_ID;
                        //    _c.COMMENTS = _comment.COMMENTS;
                        //    _c.CREATE_DATE = DateTime.Now;
                        //    _c.MODIFY_DATE = DateTime.Now;
                        //    _c.CREATED_BY = User.Identity.Name;
                        //    _c.MODIFIED_BY = User.Identity.Name;
                        //    _c.ORG_BUDGET_ID = _budgetHeader.ORG_BUDGET_ID;
                        //    GenericData.Insert<OVERHEAD_ACCOUNT_COMMENT>(_c);
                        //}


                    }


                }


               


            }



        }

        protected void uxBudgetVersionByOrganizationStore_ReadData(object sender, StoreReadDataEventArgs e)
        {
            List<long> OrgsList = new List<long>();

                string _selectedRecordID = Request.QueryString["orgid"];

                if (_selectedRecordID != null)
                {

                    char[] _delimiterChars = { ':' };
                    string[] _selectedID = _selectedRecordID.Split(_delimiterChars);
                    long _hierarchyID = long.Parse(_selectedID[1].ToString());
                    long _organizationID = long.Parse(_selectedID[0].ToString());

                    OrgsList = HR.ActiveOverheadOrganizations().Select(x => x.ORGANIZATION_ID).ToList();     

                }


            List<OVERHEAD_ORG_BUDGETS_V> _budgetsByOrganizationIDList = new List<OVERHEAD_ORG_BUDGETS_V>();

            StringBuilder _rangeString = new StringBuilder();
            List<OVERHEAD_ORG_BUDGETS_V> _budgetList  = new List<OVERHEAD_ORG_BUDGETS_V>();

            using (Entities _context = new Entities())
            {
                foreach (long _orgID in OrgsList)
                {
                    _rangeString.Clear();

                        List<OVERHEAD_GL_RANGE> _accountRanges = _context.OVERHEAD_GL_RANGE.Where(x => x.ORGANIZATION_ID == _orgID).ToList();
                        foreach (OVERHEAD_GL_RANGE _range in _accountRanges)
                        {
                            if (!_rangeString.ToString().Contains(_range.SRSEGMENT1.ToString() + "." + _range.SRSEGMENT2.ToString() + "." + _range.SRSEGMENT3.ToString() + "." + _range.SRSEGMENT4.ToString()))
                                _rangeString.AppendLine(_range.SRSEGMENT1.ToString() + "." + _range.SRSEGMENT2.ToString() + "." + _range.SRSEGMENT3.ToString() + "." + _range.SRSEGMENT4.ToString());
                        }

                        OVERHEAD_ORG_BUDGETS_V _data = new OVERHEAD_ORG_BUDGETS_V();
                       _data.ORGANIZATION_ID = _orgID;
                       _data.ORGANIZATION_NAME = HR.Organization(_orgID).ORGANIZATION_NAME;
                       _data.ACCOUNT_RANGE = _rangeString.ToString();
                       _budgetsByOrganizationIDList.Add(_data);
                }

            }

            int count;
            uxBudgetVersionByOrganizationStore.DataSource = GenericData.EnumerableFilterHeader<OVERHEAD_ORG_BUDGETS_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _budgetsByOrganizationIDList, out count);
            e.Total = count;
        }
    }
}