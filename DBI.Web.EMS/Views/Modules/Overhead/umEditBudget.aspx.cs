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

        protected void deLoadOrganizationAccounts(object sender, StoreReadDataEventArgs e)
        {

            long _organizationID;
            bool checkOrgId = long.TryParse(Request.QueryString["orgid"], out _organizationID);

            List<OVERHEAD_BUDGET_DETAIL_V> _accountList = new List<OVERHEAD_BUDGET_DETAIL_V>();

            //First we need a list of accont ranges

            using (Entities _context = new Entities())
            {

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

                foreach (GL_ACCOUNTS_V _validAccount in _rangeOfAccounts)
                {
                    try
                    {
                        OVERHEAD_BUDGET_DETAIL_V _row = new OVERHEAD_BUDGET_DETAIL_V();
                    _row.CODE_COMBINATION_ID = _validAccount.CODE_COMBINATION_ID;
                    _row.ACCOUNT_DESCRIPTION = _validAccount.SEGMENT5_DESC;
                    _row.BUDGET_AMOUNT1 = ReturnLineTotal(_budgetid, _validAccount.CODE_COMBINATION_ID,"NOV", "B");
                    _row.BUDGET_AMOUNT2 = ReturnLineTotal(_budgetid, _validAccount.CODE_COMBINATION_ID,"DEC", "B");
                    _row.BUDGET_AMOUNT3 = ReturnLineTotal(_budgetid, _validAccount.CODE_COMBINATION_ID,"JAN", "B");
                    _row.BUDGET_AMOUNT4 = ReturnLineTotal(_budgetid, _validAccount.CODE_COMBINATION_ID,"FEB", "B");
                    _row.BUDGET_AMOUNT5 = ReturnLineTotal(_budgetid, _validAccount.CODE_COMBINATION_ID,"MAR", "B");                   
                    _row.BUDGET_AMOUNT6 = ReturnLineTotal(_budgetid, _validAccount.CODE_COMBINATION_ID,"APR", "B");
                    _row.BUDGET_AMOUNT7 = ReturnLineTotal(_budgetid, _validAccount.CODE_COMBINATION_ID,"MAY", "B");
                    _row.BUDGET_AMOUNT8 = ReturnLineTotal(_budgetid, _validAccount.CODE_COMBINATION_ID,"JUN", "B");
                    _row.BUDGET_AMOUNT9 = ReturnLineTotal(_budgetid, _validAccount.CODE_COMBINATION_ID,"JUL", "B");                  
                    _row.BUDGET_AMOUNT10 = ReturnLineTotal(_budgetid, _validAccount.CODE_COMBINATION_ID,"AUG", "B");
                    _row.BUDGET_AMOUNT11 = ReturnLineTotal(_budgetid, _validAccount.CODE_COMBINATION_ID,"SEP", "B");
                    _row.BUDGET_AMOUNT12 = ReturnLineTotal(_budgetid, _validAccount.CODE_COMBINATION_ID,"OCT", "B");
                    _row.BUDGET_TOTAL = (_row.BUDGET_AMOUNT1 + _row.BUDGET_AMOUNT2 + _row.BUDGET_AMOUNT3 + _row.BUDGET_AMOUNT4 + _row.BUDGET_AMOUNT5 + _row.BUDGET_AMOUNT6 + _row.BUDGET_AMOUNT7 + _row.BUDGET_AMOUNT8 + _row.BUDGET_AMOUNT9 + _row.BUDGET_AMOUNT10 + _row.BUDGET_AMOUNT11 + _row.BUDGET_AMOUNT12);
 

                    //_row.ACTUAL_AMOUNT1 = ReturnLineTotal(_budgetid, _validAccount.CODE_COMBINATION_ID, "NOV", "A");
                    //_row.ACTUAL_AMOUNT2 = ReturnLineTotal(_budgetid, _validAccount.CODE_COMBINATION_ID, "DEC", "A");
                    //_row.ACTUAL_AMOUNT3 = ReturnLineTotal(_budgetid, _validAccount.CODE_COMBINATION_ID, "JAN", "A");
                    //_row.ACTUAL_AMOUNT4 = ReturnLineTotal(_budgetid, _validAccount.CODE_COMBINATION_ID, "FEB", "A");
                    //_row.ACTUAL_AMOUNT5 = ReturnLineTotal(_budgetid, _validAccount.CODE_COMBINATION_ID, "MAR", "A");
                    //_row.ACTUAL_AMOUNT6 = ReturnLineTotal(_budgetid, _validAccount.CODE_COMBINATION_ID, "APR", "A");
                    //_row.ACTUAL_AMOUNT7 = ReturnLineTotal(_budgetid, _validAccount.CODE_COMBINATION_ID, "MAY", "A");
                    //_row.ACTUAL_AMOUNT8 = ReturnLineTotal(_budgetid, _validAccount.CODE_COMBINATION_ID, "JUN", "A");
                    //_row.ACTUAL_AMOUNT9 = ReturnLineTotal(_budgetid, _validAccount.CODE_COMBINATION_ID, "JUL", "A");
                    //_row.ACTUAL_AMOUNT10 = ReturnLineTotal(_budgetid, _validAccount.CODE_COMBINATION_ID, "AUG", "A");
                    //_row.ACTUAL_AMOUNT11 = ReturnLineTotal(_budgetid, _validAccount.CODE_COMBINATION_ID, "SEP", "A");
                    //_row.ACTUAL_AMOUNT12 = ReturnLineTotal(_budgetid, _validAccount.CODE_COMBINATION_ID, "OCT", "A");

                   
                    //_row.ACTUAL_TOTAL = (_row.ACTUAL_AMOUNT1 + _row.ACTUAL_AMOUNT2 + _row.ACTUAL_AMOUNT3 + _row.ACTUAL_AMOUNT4 + _row.ACTUAL_AMOUNT5 + _row.ACTUAL_AMOUNT6 + _row.ACTUAL_AMOUNT7 + _row.ACTUAL_AMOUNT8 + _row.ACTUAL_AMOUNT9 + _row.ACTUAL_AMOUNT10 + _row.ACTUAL_AMOUNT11 + _row.ACTUAL_AMOUNT12);

                    _accountList.Add(_row);
                    }
                    catch (Exception ex)
                    {
                        
                        throw(ex);
                    }
                  
                }

            }

            int count;
            uxOrganizationAccountStore.DataSource = GenericData.ListFilterHeader<OVERHEAD_BUDGET_DETAIL_V>(e.Start, 1000, e.Sort, e.Parameters["filterheader"], _accountList.AsQueryable(), out count);
            e.Total = count;

        }

        protected decimal ReturnLineTotal(long budget_id, long code_combination_id, string period_name, string type)
        {
            decimal returnvalue = 0;
            using (Entities _context = new Entities())
            {
                OVERHEAD_BUDGET_DETAIL _detail = _context.OVERHEAD_BUDGET_DETAIL.Where(x => x.ORG_BUDGET_ID == budget_id & x.CODE_COMBINATION_ID == code_combination_id & x.DETAIL_TYPE == type & x.PERIOD == period_name).SingleOrDefault();
                if (_detail != null)
                {
                    returnvalue = (decimal)_detail.AMOUNT;
                }
                return returnvalue;
            }

        }

        protected void deItemMaintenance(object sender, DirectEventArgs e)
        {
            string _budgetSelectedID = Request.QueryString["budget_id"];
            string _AccountSelectedID = uxOrganizationAccountSelectionModel.SelectedRow.RecordID;
            string _fiscal_year = Request.QueryString["fiscalyear"];

            string url = "umAddOverheadDetailLine.aspx?budgetID=" + _budgetSelectedID + "&accountID=" + _AccountSelectedID + "&fiscalyear=" + _fiscal_year;

            Window win = new Window
            {
                ID = "uxDetailLineMaintenance",
                Title = "Account Details",
                Height = 300,
                Width = 450,
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




        public class OVERHEAD_BUDGET_DETAIL_V
        {
            public long CODE_COMBINATION_ID { get; set; }
            public string ACCOUNT_DESCRIPTION { get; set; }
            public decimal BUDGET_TOTAL { get; set; }
            public decimal ACTUAL_TOTAL { get; set; }
            public decimal BUDGET_AMOUNT1 { get; set; }
            public decimal ACTUAL_AMOUNT1 { get; set; }
            public decimal BUDGET_AMOUNT2 { get; set; }
            public decimal ACTUAL_AMOUNT2 { get; set; }
            public decimal BUDGET_AMOUNT3 { get; set; }
            public decimal ACTUAL_AMOUNT3 { get; set; }
            public decimal BUDGET_AMOUNT4 { get; set; }
            public decimal ACTUAL_AMOUNT4 { get; set; }
            public decimal BUDGET_AMOUNT5 { get; set; }
            public decimal ACTUAL_AMOUNT5 { get; set; }
            public decimal BUDGET_AMOUNT6 { get; set; }
            public decimal ACTUAL_AMOUNT6 { get; set; }
            public decimal BUDGET_AMOUNT7 { get; set; }
            public decimal ACTUAL_AMOUNT7 { get; set; }
            public decimal BUDGET_AMOUNT8 { get; set; }
            public decimal ACTUAL_AMOUNT8 { get; set; }
            public decimal BUDGET_AMOUNT9 { get; set; }
            public decimal ACTUAL_AMOUNT9 { get; set; }
            public decimal BUDGET_AMOUNT10 { get; set; }
            public decimal ACTUAL_AMOUNT10 { get; set; }
            public decimal BUDGET_AMOUNT11 { get; set; }
            public decimal ACTUAL_AMOUNT11 { get; set; }
            public decimal BUDGET_AMOUNT12 { get; set; }
            public decimal ACTUAL_AMOUNT12 { get; set; }
        }
    }
}