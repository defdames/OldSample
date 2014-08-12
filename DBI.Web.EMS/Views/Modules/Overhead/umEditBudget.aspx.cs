using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Ext.Net;
using DBI.Data;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umEditBudget : DBI.Core.Web.BasePage
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

        protected void deEditBudgetNotes(object sender, DirectEventArgs e)
        {
            using(Entities _context = new Entities())
            {

               long _budgetid = long.Parse(Request.QueryString["budget_id"]);

                //pull budget detail data
                OVERHEAD_ORG_BUDGETS _budgetDetail = _context.OVERHEAD_ORG_BUDGETS.Where(x => x.ORG_BUDGET_ID == _budgetid).SingleOrDefault();
                uxBudgetComments.Text = _budgetDetail.COMMENTS;
            }
            uxBudgetNotesWindow.Center();
            uxBudgetNotesWindow.Show();
         
        }

        protected void deImportActuals(object sender, DirectEventArgs e)
        {
            short _fiscal_year = short.Parse(Request.QueryString["fiscalyear"]);
            long _organizationID = long.Parse(Request.QueryString["orgid"]);
            long _budgetid = long.Parse(Request.QueryString["budget_id"]);

            using(Entities _context = new Entities())
            {
               
                //First get the periods for the fiscal year
                string sql2 = "select entered_period_name,period_year,period_num,period_type,start_date,end_date from gl.gl_periods where period_set_name = 'DBI Calendar' order by period_num";
                List<GL_PERIODS> _periodMonthList = _context.Database.SqlQuery<GL_PERIODS>(sql2).Where(x => x.PERIOD_YEAR == _fiscal_year & x.PERIOD_TYPE == "Month").ToList();

                // Get the range of accounts
                List<GL_ACCOUNTS_V> _rangeOfAccounts = new List<GL_ACCOUNTS_V>();

                var _data = _context.OVERHEAD_GL_RANGE.Where(x => x.ORGANIZATION_ID == _organizationID);

                foreach (OVERHEAD_GL_RANGE _range in _data)
                {
                    var _adata = _context.GL_ACCOUNTS_V.Where(x => String.Compare(x.SEGMENT1, _range.SRSEGMENT1) >= 0 && String.Compare(x.SEGMENT1, _range.ERSEGMENT1) <= 0);
                    _adata = _adata.Where(x => String.Compare(x.SEGMENT2, _range.SRSEGMENT2) >= 0 && String.Compare(x.SEGMENT2, _range.ERSEGMENT2) <= 0);
                    _adata = _adata.Where(x => String.Compare(x.SEGMENT3, _range.SRSEGMENT3) >= 0 && String.Compare(x.SEGMENT3, _range.ERSEGMENT3) <= 0);
                    _adata = _adata.Where(x => String.Compare(x.SEGMENT4, _range.SRSEGMENT4) >= 0 && String.Compare(x.SEGMENT4, _range.ERSEGMENT4) <= 0);
                    _adata = _adata.Where(x => String.Compare(x.SEGMENT5, _range.SRSEGMENT5) >= 0 && String.Compare(x.SEGMENT5, _range.ERSEGMENT5) <= 0);
                    _adata = _adata.Where(x => String.Compare(x.SEGMENT6, _range.SRSEGMENT6) >= 0 && String.Compare(x.SEGMENT6, _range.ERSEGMENT6) <= 0);
                    _adata = _adata.Where(x => String.Compare(x.SEGMENT7, _range.SRSEGMENT7) >= 0 && String.Compare(x.SEGMENT7, _range.ERSEGMENT7) <= 0);
                    List<GL_ACCOUNTS_V> _accountRange = _adata.ToList();
                    _rangeOfAccounts.AddRange(_accountRange);

                }

                //Exclude any accounts added to the list of excluded accounts
                List<OVERHEAD_GL_ACCOUNT> _excludedAccounts = _context.OVERHEAD_GL_ACCOUNT.Where(x => x.ORGANIZATION_ID == _organizationID).ToList();

                //Create a list of accounts matching up with GL_ACCOUNTS_V
                List<GL_ACCOUNTS_V> _eAccountList = new List<GL_ACCOUNTS_V>();

                foreach (OVERHEAD_GL_ACCOUNT _eaccount in _excludedAccounts)
                {
                    var _adata = _context.GL_ACCOUNTS_V.Where(x => x.CODE_COMBINATION_ID == _eaccount.CODE_COMBINATION_ID).Single();
                    _rangeOfAccounts.Remove(_adata);
                }

                //Add total detail
                foreach (GL_ACCOUNTS_V _validAccount in _rangeOfAccounts)
                {

                    string sql = string.Format("select period_net_dr,period_year,code_combination_id,period_num from gl.gl_balances where actual_flag = 'A' and period_year = {0} and code_combination_id = {1}", _fiscal_year, _validAccount.CODE_COMBINATION_ID);
                    List<ACTUAL_BALANCES> _balance = _context.Database.SqlQuery<ACTUAL_BALANCES>(sql).ToList();

                       foreach (GL_PERIODS _period in _periodMonthList)
                        {
                            List<OVERHEAD_BUDGET_DETAIL> _budgetLineList = _context.OVERHEAD_BUDGET_DETAIL.Where(x => x.ORG_BUDGET_ID == _budgetid).ToList();
                            OVERHEAD_BUDGET_DETAIL _line = _budgetLineList.Where(x => x.ORG_BUDGET_ID == _budgetid & x.CODE_COMBINATION_ID == _validAccount.CODE_COMBINATION_ID & x.PERIOD_NUM == _period.PERIOD_NUM).SingleOrDefault();
                            ACTUAL_BALANCES _actualTotalLine = _balance.Where(x => x.PERIOD_NUM == _period.PERIOD_NUM).SingleOrDefault();
                            decimal _aTotal = 0;

                            if (_actualTotalLine != null)
                            {
                                if (_actualTotalLine.PERIOD_NET_DR != 0)
                                {
                                    _aTotal = _actualTotalLine.PERIOD_NET_DR;

                                    if (_line == null)
                                    {
                                        //No data, create it
                                        OVERHEAD_BUDGET_DETAIL _record = new OVERHEAD_BUDGET_DETAIL();
                                        _record.ORG_BUDGET_ID = _budgetid;
                                        _record.PERIOD_NAME = _period.ENTERED_PERIOD_NAME;
                                        _record.PERIOD_NUM = _period.PERIOD_NUM;
                                        _record.CODE_COMBINATION_ID = _validAccount.CODE_COMBINATION_ID;
                                        _record.AMOUNT = _aTotal;
                                        _record.CREATE_DATE = DateTime.Now;
                                        _record.MODIFY_DATE = DateTime.Now;
                                        _record.CREATED_BY = User.Identity.Name;
                                        _record.MODIFIED_BY = User.Identity.Name;
                                        GenericData.Insert<OVERHEAD_BUDGET_DETAIL>(_record);
                                    }
                                    else
                                    {
                                        //Data update it
                                        _line.AMOUNT = _aTotal;
                                        _line.MODIFY_DATE = DateTime.Now;
                                        _line.MODIFIED_BY = User.Identity.Name;
                                        GenericData.Update<OVERHEAD_BUDGET_DETAIL>(_line);
                                    }
                                }
                            }
                    }
                }


            }

            uxOrganizationAccountStore.Reload();


        }

        public class ACTUAL_BALANCES
        {
            public decimal PERIOD_NET_DR { get; set; }
            public short PERIOD_YEAR { get; set; }
            public long CODE_COMBINATION_ID { get; set; }
            public long PERIOD_NUM { get; set; }
        }

        protected void deSaveBudgetNotes(object sender, DirectEventArgs e)
        {
            using (Entities _context = new Entities())
            {

                long _budgetid = long.Parse(Request.QueryString["budget_id"]);

                //pull budget detail data
                OVERHEAD_ORG_BUDGETS _budgetDetail = _context.OVERHEAD_ORG_BUDGETS.Where(x => x.ORG_BUDGET_ID == _budgetid).SingleOrDefault();
                _budgetDetail.COMMENTS = uxBudgetComments.Text;
                GenericData.Update<OVERHEAD_ORG_BUDGETS>(_budgetDetail);
            }
            uxBudgetNotesWindow.Close();

        }

        public class GL_PERIODS
        {
            public string ENTERED_PERIOD_NAME { get; set; }
            public short PERIOD_YEAR { get; set; }
            public long PERIOD_NUM { get; set; }
            public string PERIOD_TYPE { get; set; }
            public DateTime START_DATE { get; set; }
            public DateTime? END_DATE { get; set; }
        }

        protected void deLoadOrganizationAccounts(object sender, StoreReadDataEventArgs e)
        {

            long _organizationID;
            bool checkOrgId = long.TryParse(Request.QueryString["orgid"], out _organizationID);
            short _fiscal_year = short.Parse(Request.QueryString["fiscalyear"]);


            using (Entities _context = new Entities())
            {

                string sql = "select entered_period_name,period_year,period_num,period_type,start_date,end_date from gl.gl_periods where period_set_name = 'DBI Calendar' order by period_num";
                List<GL_PERIODS> _periodMonthList = _context.Database.SqlQuery<GL_PERIODS>(sql).Where(x => x.PERIOD_YEAR == _fiscal_year & x.PERIOD_TYPE == "Month").ToList();
                List<GL_PERIODS> _periodWeekList = _context.Database.SqlQuery<GL_PERIODS>(sql).Where(x => x.PERIOD_YEAR == _fiscal_year & x.PERIOD_TYPE == "Week").ToList();

                foreach (GL_PERIODS _period in _periodMonthList)
                {
                    if (_period.PERIOD_NUM == 1)
                    {
                        var _weekCount = _periodWeekList.Where(x => x.ENTERED_PERIOD_NAME.Contains(_period.ENTERED_PERIOD_NAME)).Count();
                        Column1.Text = string.Format("{0} - ({1} Weeks)", _period.ENTERED_PERIOD_NAME, _weekCount);
                    }
                    if (_period.PERIOD_NUM == 2)
                    {
                        var _weekCount = _periodWeekList.Where(x => x.ENTERED_PERIOD_NAME.Contains(_period.ENTERED_PERIOD_NAME)).Count();
                        Column2.Text = string.Format("{0} - ({1} Weeks)", _period.ENTERED_PERIOD_NAME, _weekCount);
                    }
                    if (_period.PERIOD_NUM == 3)
                    {
                        var _weekCount = _periodWeekList.Where(x => x.ENTERED_PERIOD_NAME.Contains(_period.ENTERED_PERIOD_NAME)).Count();
                        Column3.Text = string.Format("{0} - ({1} Weeks)", _period.ENTERED_PERIOD_NAME, _weekCount);
                    }
                    if (_period.PERIOD_NUM == 4)
                    {
                        var _weekCount = _periodWeekList.Where(x => x.ENTERED_PERIOD_NAME.Contains(_period.ENTERED_PERIOD_NAME)).Count();
                        Column4.Text = string.Format("{0} - ({1} Weeks)", _period.ENTERED_PERIOD_NAME, _weekCount);
                    }
                    if (_period.PERIOD_NUM == 5)
                    {
                        var _weekCount = _periodWeekList.Where(x => x.ENTERED_PERIOD_NAME.Contains(_period.ENTERED_PERIOD_NAME)).Count();
                        Column5.Text = string.Format("{0} - ({1} Weeks)", _period.ENTERED_PERIOD_NAME, _weekCount);
                    }
                    if (_period.PERIOD_NUM == 6)
                    {
                        var _weekCount = _periodWeekList.Where(x => x.ENTERED_PERIOD_NAME.Contains(_period.ENTERED_PERIOD_NAME)).Count();
                        Column6.Text = string.Format("{0} - ({1} Weeks)", _period.ENTERED_PERIOD_NAME, _weekCount);
                    }
                    if (_period.PERIOD_NUM == 7)
                    {
                        var _weekCount = _periodWeekList.Where(x => x.ENTERED_PERIOD_NAME.Contains(_period.ENTERED_PERIOD_NAME)).Count();
                        Column7.Text = string.Format("{0} - ({1} Weeks)", _period.ENTERED_PERIOD_NAME, _weekCount);
                    }
                    if (_period.PERIOD_NUM == 8)
                    {
                        var _weekCount = _periodWeekList.Where(x => x.ENTERED_PERIOD_NAME.Contains(_period.ENTERED_PERIOD_NAME)).Count();
                        Column8.Text = string.Format("{0} - ({1} Weeks)", _period.ENTERED_PERIOD_NAME, _weekCount);
                    }
                    if (_period.PERIOD_NUM == 9)
                    {
                        var _weekCount = _periodWeekList.Where(x => x.ENTERED_PERIOD_NAME.Contains(_period.ENTERED_PERIOD_NAME)).Count();
                        Column9.Text = string.Format("{0} - ({1} Weeks)", _period.ENTERED_PERIOD_NAME, _weekCount);
                    }
                    if (_period.PERIOD_NUM == 10)
                    {
                        var _weekCount = _periodWeekList.Where(x => x.ENTERED_PERIOD_NAME.Contains(_period.ENTERED_PERIOD_NAME)).Count();
                        Column10.Text = string.Format("{0} - ({1} Weeks)", _period.ENTERED_PERIOD_NAME, _weekCount);
                    }
                    if (_period.PERIOD_NUM == 11)
                    {
                        var _weekCount = _periodWeekList.Where(x => x.ENTERED_PERIOD_NAME.Contains(_period.ENTERED_PERIOD_NAME)).Count();
                        Column11.Text = string.Format("{0} - ({1} Weeks)", _period.ENTERED_PERIOD_NAME, _weekCount);
                    }
                    if (_period.PERIOD_NUM == 12)
                    {
                        var _weekCount = _periodWeekList.Where(x => x.ENTERED_PERIOD_NAME.Contains(_period.ENTERED_PERIOD_NAME)).Count();
                        Column12.Text = string.Format("{0} - ({1} Weeks)", _period.ENTERED_PERIOD_NAME, _weekCount);
                    }

                }


                List<OVERHEAD_BUDGET_DETAIL_V> _accountList = new List<OVERHEAD_BUDGET_DETAIL_V>();

                //First we need a list of accont ranges

                List<GL_ACCOUNTS_V> _rangeOfAccounts = new List<GL_ACCOUNTS_V>();

                var _data = _context.OVERHEAD_GL_RANGE.Where(x => x.ORGANIZATION_ID == _organizationID);

                foreach (OVERHEAD_GL_RANGE _range in _data)
                {
                    var _adata = _context.GL_ACCOUNTS_V.Where(x => String.Compare(x.SEGMENT1, _range.SRSEGMENT1) >= 0 && String.Compare(x.SEGMENT1, _range.ERSEGMENT1) <= 0);
                    _adata = _adata.Where(x => String.Compare(x.SEGMENT2, _range.SRSEGMENT2) >= 0 && String.Compare(x.SEGMENT2, _range.ERSEGMENT2) <= 0);
                    _adata = _adata.Where(x => String.Compare(x.SEGMENT3, _range.SRSEGMENT3) >= 0 && String.Compare(x.SEGMENT3, _range.ERSEGMENT3) <= 0);
                    _adata = _adata.Where(x => String.Compare(x.SEGMENT4, _range.SRSEGMENT4) >= 0 && String.Compare(x.SEGMENT4, _range.ERSEGMENT4) <= 0);
                    _adata = _adata.Where(x => String.Compare(x.SEGMENT5, _range.SRSEGMENT5) >= 0 && String.Compare(x.SEGMENT5, _range.ERSEGMENT5) <= 0);
                    _adata = _adata.Where(x => String.Compare(x.SEGMENT6, _range.SRSEGMENT6) >= 0 && String.Compare(x.SEGMENT6, _range.ERSEGMENT6) <= 0);
                    _adata = _adata.Where(x => String.Compare(x.SEGMENT7, _range.SRSEGMENT7) >= 0 && String.Compare(x.SEGMENT7, _range.ERSEGMENT7) <= 0);
                    List<GL_ACCOUNTS_V> _accountRange = _adata.ToList();
                    _rangeOfAccounts.AddRange(_accountRange);

                }

                //Exclude any accounts added to the list of excluded accounts
                List<OVERHEAD_GL_ACCOUNT> _excludedAccounts = _context.OVERHEAD_GL_ACCOUNT.Where(x => x.ORGANIZATION_ID == _organizationID).ToList();

                //Create a list of accounts matching up with GL_ACCOUNTS_V
                List<GL_ACCOUNTS_V> _eAccountList = new List<GL_ACCOUNTS_V>();

                foreach (OVERHEAD_GL_ACCOUNT _eaccount in _excludedAccounts)
                {
                    var _adata = _context.GL_ACCOUNTS_V.Where(x => x.CODE_COMBINATION_ID == _eaccount.CODE_COMBINATION_ID).Single();
                    _rangeOfAccounts.Remove(_adata);
                }


                long _fiscalyear = long.Parse(Request.QueryString["fiscalyear"]);
                long _budgetid = long.Parse(Request.QueryString["budget_id"]);


                //pull budget detail data
                OVERHEAD_ORG_BUDGETS _budgetDetail = _context.OVERHEAD_ORG_BUDGETS.Where(x => x.ORG_BUDGET_ID == _budgetid).SingleOrDefault();

                if (_budgetDetail.STATUS == "P")
                {
                    uxCompleteBudget.Disable();
                    uxCompleteBudget.Text = "Budget Pending";
                    uxCompleteBudget.Icon = Icon.FlagChecked;
                }


                //pull budget detail data
                List<OVERHEAD_BUDGET_DETAIL> _budgetLineList = _context.OVERHEAD_BUDGET_DETAIL.Where(x => x.ORG_BUDGET_ID == _budgetid).ToList();

                foreach (GL_ACCOUNTS_V _validAccount in _rangeOfAccounts)
                {

                    OVERHEAD_BUDGET_DETAIL_V _row = new OVERHEAD_BUDGET_DETAIL_V();
                    //return category infomration 
                    OVERHEAD_ACCOUNT_CATEGORY _accountCategory = _context.OVERHEAD_ACCOUNT_CATEGORY.Where(x => x.ACCOUNT_SEGMENT == _validAccount.SEGMENT5).OrderBy(x => x.ACCOUNT_SEGMENT).SingleOrDefault();
                    OVERHEAD_CATEGORY _category = new OVERHEAD_CATEGORY();


                    if (_accountCategory != null)
                    {
                        _category = _context.OVERHEAD_CATEGORY.Where(x => x.CATEGORY_ID == _accountCategory.CATEGORY_ID).SingleOrDefault();
                        _row.CATEGORY_ID = _accountCategory.CATEGORY_ID;
                        _row.CATEGORY_NAME = _category.NAME;
                        _row.SORT_ORDER = _accountCategory.SORT_ORDER;
                    }
                    else
                    {
                        _row.CATEGORY_NAME = "Other";
                        _row.SORT_ORDER = 0;
                    }

                    _row.CODE_COMBINATION_ID = _validAccount.CODE_COMBINATION_ID;
                    _row.ACCOUNT_SEGMENT = _validAccount.SEGMENT5;
                    _row.ACCOUNT_DESCRIPTION = _validAccount.SEGMENT5_DESC + " (" + _validAccount.SEGMENT5 + ")";
                    _row.AMOUNT1 = ReturnLineTotal(_budgetLineList, _budgetid, _validAccount.CODE_COMBINATION_ID, 1);
                    _row.AMOUNT2 = ReturnLineTotal(_budgetLineList, _budgetid, _validAccount.CODE_COMBINATION_ID, 2);
                    _row.AMOUNT3 = ReturnLineTotal(_budgetLineList, _budgetid, _validAccount.CODE_COMBINATION_ID, 3);
                    _row.AMOUNT4 = ReturnLineTotal(_budgetLineList, _budgetid, _validAccount.CODE_COMBINATION_ID, 4);
                    _row.AMOUNT5 = ReturnLineTotal(_budgetLineList, _budgetid, _validAccount.CODE_COMBINATION_ID, 5);
                    _row.AMOUNT6 = ReturnLineTotal(_budgetLineList, _budgetid, _validAccount.CODE_COMBINATION_ID, 6);
                    _row.AMOUNT7 = ReturnLineTotal(_budgetLineList, _budgetid, _validAccount.CODE_COMBINATION_ID, 7);
                    _row.AMOUNT8 = ReturnLineTotal(_budgetLineList, _budgetid, _validAccount.CODE_COMBINATION_ID, 8);
                    _row.AMOUNT9 = ReturnLineTotal(_budgetLineList, _budgetid, _validAccount.CODE_COMBINATION_ID, 9);
                    _row.AMOUNT10 = ReturnLineTotal(_budgetLineList, _budgetid, _validAccount.CODE_COMBINATION_ID, 10);
                    _row.AMOUNT11 = ReturnLineTotal(_budgetLineList, _budgetid, _validAccount.CODE_COMBINATION_ID, 11);
                    _row.AMOUNT12 = ReturnLineTotal(_budgetLineList, _budgetid, _validAccount.CODE_COMBINATION_ID, 12);
                    _row.TOTAL = (_row.AMOUNT1 + _row.AMOUNT2 + _row.AMOUNT3 + _row.AMOUNT4 + _row.AMOUNT5 + _row.AMOUNT6 + _row.AMOUNT7 + _row.AMOUNT8 + _row.AMOUNT9 + _row.AMOUNT10 + _row.AMOUNT11 + _row.AMOUNT12);

                    //Check toggle button if button is active, hide zero lines (zero total)
                    if (!uxHideBlankLinesCheckbox.Checked)
                    {
                        _accountList.Add(_row);
                    }
                    else
                    {

                        if (e.Parameters["TOGGLE_ACTIVE"] == "N")
                        {
                            _accountList.Add(_row);
                        }
                        else
                        {
                            if (_row.TOTAL > 0 || _row.TOTAL < 0)
                                _accountList.Add(_row);

                        }
                    }

                }


                //Quick check hide summary column
                //foreach (OVERHEAD_BUDGET_DETAIL_V _item in _accountList)
                //{
                //    if (_item.TOTAL > 0)
                //        uxSummary.ToggleSummaryRow(true);
                 //   break;
                //}


            int count;
            uxOrganizationAccountStore.DataSource = GenericData.ListFilterHeader<OVERHEAD_BUDGET_DETAIL_V>(e.Start, 1000, e.Sort, e.Parameters["filterheader"], _accountList.AsQueryable(), out count);
            e.Total = count;
            }

        }

        protected void deHideBlankLines(object sender, DirectEventArgs e)
        {
            Ext.Net.ParameterCollection ps = new Ext.Net.ParameterCollection();

            Ext.Net.StoreParameter _p = new Ext.Net.StoreParameter();
            _p.Mode = ParameterMode.Value;
            _p.Name = "TOGGLE_ACTIVE";
            _p.Value = "N";

            if (uxHideBlankLinesCheckbox.Checked)
                _p.Value = "Y";

            ps.Add(_p);

            uxOrganizationAccountStore.Reload(ps);

        }

        protected decimal ReturnLineTotal(List<OVERHEAD_BUDGET_DETAIL> budgetList, long budget_id, long code_combination_id, long period_num)
        {
            decimal returnvalue = 0;

                OVERHEAD_BUDGET_DETAIL _line = budgetList.Where(x => x.ORG_BUDGET_ID == budget_id & x.CODE_COMBINATION_ID == code_combination_id & x.PERIOD_NUM == period_num).SingleOrDefault();
                if(_line != null)
                {
                    returnvalue =(decimal) _line.AMOUNT; 
                }
                return returnvalue;
       }


        protected void deItemMaintenance(object sender, DirectEventArgs e)
        {
            string _budgetSelectedID = Request.QueryString["budget_id"];
            string _AccountSelectedID = uxOrganizationAccountSelectionModel.SelectedRow.RecordID;
            string _fiscal_year = Request.QueryString["fiscalyear"];
            string _accountDescription = e.ExtraParams["ACCOUNT_DESCRIPTION"];

            long _budgetID = long.Parse(_budgetSelectedID);


            using (Entities _context = new Entities())
            {
                //pull budget detail data
                OVERHEAD_ORG_BUDGETS _budgetDetail = _context.OVERHEAD_ORG_BUDGETS.Where(x => x.ORG_BUDGET_ID == _budgetID).SingleOrDefault();

                if (_budgetDetail.STATUS == "P")
                {
                    return;
                }

            }
            string url = "umAddOverheadDetailLine.aspx?budgetID=" + _budgetSelectedID + "&accountID=" + _AccountSelectedID + "&fiscalyear=" + _fiscal_year;

            Window win = new Window
            {
                ID = "uxDetailLineMaintenance",
                Title = "Account Details - " + _accountDescription,
                Height = 450,
                Width = 850,
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

        protected void deCompleteBudget(object sender, DirectEventArgs e)
        {
            uxCompleteBudget.Disable();
            uxCompleteBudget.Text = "Budget Pending";
            uxCompleteBudget.Icon = Icon.FlagChecked;

            using (Entities _context = new Entities())
            {
                long _budgetSelectedID = long.Parse(Request.QueryString["budget_id"]);
                //pull budget detail data
                OVERHEAD_ORG_BUDGETS _budgetDetail = _context.OVERHEAD_ORG_BUDGETS.Where(x => x.ORG_BUDGET_ID == _budgetSelectedID).SingleOrDefault();

                _budgetDetail.STATUS = "P";
                GenericData.Update<OVERHEAD_ORG_BUDGETS>(_budgetDetail);
            }

            uxOrganizationAccountStore.Reload();

        }


        public class OVERHEAD_BUDGET_DETAIL_V
        {
            public long CATEGORY_ID { get; set; }
            public long? SORT_ORDER { get; set; }
            public string CATEGORY_NAME { get; set; }
            public long CODE_COMBINATION_ID { get; set; }
            public string ACCOUNT_SEGMENT { get; set; }
            public string ACCOUNT_DESCRIPTION { get; set; }
            public decimal TOTAL { get; set; }
            public decimal AMOUNT1 { get; set; }
            public decimal AMOUNT2 { get; set; }
            public decimal AMOUNT3 { get; set; }
            public decimal AMOUNT4 { get; set; }
            public decimal AMOUNT5 { get; set; }
            public decimal AMOUNT6 { get; set; }
            public decimal AMOUNT7 { get; set; }
            public decimal AMOUNT8 { get; set; }
            public decimal AMOUNT9 { get; set; }
            public decimal AMOUNT10 { get; set; }
            public decimal AMOUNT11 { get; set; }
            public decimal AMOUNT12 { get; set; }
        }
    }
}