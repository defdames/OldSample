﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umImportActualsWindow : DBI.Core.Web.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (!validateComponentSecurity("SYS.OverheadBudget.Maintenance"))
                {
                    X.Redirect("~/Views/uxDefault.aspx");
                }

                uxAccountComments.Text = "IMPORTANT, PLEASE READ!! This area allows you to pull in actuals that have been applied to your gl account in oracle but not yet closed by finance. It will overwrite your totals for the selected months you are importing. There is no going back to your old numbers after this process has been run!";
            }
        }

         public class ACTUAL_BALANCES
        {
            public decimal PERIOD_NET_DR { get; set; }
            public decimal PERIOD_NET_CR { get; set; }
            public short PERIOD_YEAR { get; set; }
            public long CODE_COMBINATION_ID { get; set; }
            public long PERIOD_NUM { get; set; }
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

        protected void deImportActuals(object sender, DirectEventArgs e)
        {
            
            short _fiscal_year = short.Parse(Request.QueryString["fiscalyear"]);
            long _organizationID = long.Parse(Request.QueryString["orgid"]);
            long _budgetid = long.Parse(Request.QueryString["budget_id"]);

            using(Entities _context = new Entities())
            {

                CheckboxSelectionModel _csm = uxPeriodSelectionModel;

                List<string> _periodsToImport = _csm.SelectedRows.Select(x => x.RecordID).ToList();

      
                //First get the periods for the fiscal year
                string sql2 = "select entered_period_name,period_year,period_num,period_type,start_date,end_date from gl.gl_periods where period_set_name = 'DBI Calendar' order by period_num";
                List<GL_PERIODS> _periodMonthList = _context.Database.SqlQuery<GL_PERIODS>(sql2).Where(x => x.PERIOD_YEAR == _fiscal_year & x.PERIOD_TYPE == "Month" & _periodsToImport.Contains(x.PERIOD_NUM.ToString())).ToList();

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

                    string sql = string.Format("select period_net_dr, period_net_cr,period_year,code_combination_id,period_num from gl.gl_balances where actual_flag = 'A' and period_year = {0} and code_combination_id = {1} and set_of_books_id in (select distinct set_of_books_id from apps.hr_operating_units)", _fiscal_year, _validAccount.CODE_COMBINATION_ID);
                    List<ACTUAL_BALANCES> _balance = _context.Database.SqlQuery<ACTUAL_BALANCES>(sql).ToList();

                       foreach (GL_PERIODS _period in _periodMonthList)
                        {
                            List<OVERHEAD_BUDGET_DETAIL> _budgetLineList = _context.OVERHEAD_BUDGET_DETAIL.Where(x => x.ORG_BUDGET_ID == _budgetid).ToList();
                            OVERHEAD_BUDGET_DETAIL _line = _budgetLineList.Where(x => x.ORG_BUDGET_ID == _budgetid & x.CODE_COMBINATION_ID == _validAccount.CODE_COMBINATION_ID & x.PERIOD_NUM == _period.PERIOD_NUM).SingleOrDefault();
                            ACTUAL_BALANCES _actualTotalLine = _balance.Where(x => x.PERIOD_NUM == _period.PERIOD_NUM).SingleOrDefault();
                            decimal _aTotal = 0;

                            if (_actualTotalLine != null)
                            {

                                _aTotal = _actualTotalLine.PERIOD_NET_DR + Decimal.Negate(_actualTotalLine.PERIOD_NET_CR);

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

        protected void uxDetailStore_ReadData(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                short _fiscal_year = short.Parse(Request.QueryString["fiscalyear"]);
                string sql2 = "select entered_period_name,period_year,period_num,period_type,start_date,end_date from gl.gl_periods where period_set_name = 'DBI Calendar' order by period_num";
                List<GL_PERIODS> _periodMonthList = _context.Database.SqlQuery<GL_PERIODS>(sql2).Where(x => x.PERIOD_YEAR == _fiscal_year & x.PERIOD_TYPE == "Month").ToList();

                int count;
                uxDetailStore.DataSource = GenericData.EnumerableFilterHeader<GL_PERIODS>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _periodMonthList, out count);
                e.Total = count;
            }
        }

    }
}