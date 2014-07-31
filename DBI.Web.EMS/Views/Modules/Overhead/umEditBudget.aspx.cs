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

                foreach (GL_ACCOUNTS_V _validAccount in _rangeOfAccounts)
                {
                    OVERHEAD_BUDGET_DETAIL_V _row = new OVERHEAD_BUDGET_DETAIL_V();
                    _row.CODE_COMBINATION_ID = _validAccount.CODE_COMBINATION_ID;
                    _row.ACCOUNT_DESCRIPTION = _validAccount.SEGMENT5_DESC;
                    _row.BUDGET_AMOUNT1 = 0;
                    _row.ACTUAL_AMOUNT1 = 0;
                    _row.BUDGET_AMOUNT2 = 0;
                    _row.ACTUAL_AMOUNT2 = 0;
                    _row.BUDGET_AMOUNT3 = 0;
                    _row.ACTUAL_AMOUNT3 = 0;
                    _row.BUDGET_AMOUNT4 = 0;
                    _row.ACTUAL_AMOUNT4 = 0;
                    _row.BUDGET_AMOUNT5 = 0;
                    _row.ACTUAL_AMOUNT5 = 0;
                    _row.BUDGET_AMOUNT6 = 0;
                    _row.ACTUAL_AMOUNT6 = 0;
                    _row.BUDGET_AMOUNT7 = 0;
                    _row.ACTUAL_AMOUNT7 = 0;
                    _row.BUDGET_AMOUNT8 = 0;
                    _row.ACTUAL_AMOUNT8 = 0;
                    _row.BUDGET_AMOUNT9 = 0;
                    _row.ACTUAL_AMOUNT9 = 0;
                    _row.BUDGET_AMOUNT10 = 0;
                    _row.ACTUAL_AMOUNT10 = 0;
                    _row.BUDGET_AMOUNT11 = 0;
                    _row.ACTUAL_AMOUNT11 = 0;
                    _row.BUDGET_AMOUNT12 = 0;
                    _row.ACTUAL_AMOUNT12 = 0;

                    _accountList.Add(_row);
                }

            }

            int count;
            uxOrganizationAccountStore.DataSource = GenericData.ListFilterHeader<OVERHEAD_BUDGET_DETAIL_V>(e.Start, 1000, e.Sort, e.Parameters["filterheader"], _accountList.AsQueryable(), out count);
            e.Total = count;

        }


        protected void deItemMaintenance(object sender, DirectEventArgs e)
        {
            string _budgetSelectedID = Request.QueryString["budget_id"];
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




        public class OVERHEAD_BUDGET_DETAIL_V
        {
            public long CODE_COMBINATION_ID { get; set; }
            public string ACCOUNT_DESCRIPTION { get; set; }
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