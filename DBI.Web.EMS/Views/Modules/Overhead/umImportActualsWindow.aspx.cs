using System;
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

                if (Request.QueryString["AdminImport"] != null)
                {
                    uxInformationPanel.Html = "This will allow you to import actuals into a budget version. Once the data has been imported it will lock those imported columns for that budget version. It will also override all their data with actuals. There is no way to go roll this back. You can import actuals as many times as you need.";
                }
                else
                {
                    uxInformationPanel.Html = "This will allow you to import actuals into a budget version. This process will overwrite all period data for which you are importing and you will not be able to go back after this step. You can import actuals as many times as you need.";
                }
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
            public string ACTUALS_IMPORTED_FLAG { get; set; }
            public string ADMIN { get; set; }
        }

        protected void deImportActuals(object sender, DirectEventArgs e)
        {
            
            short _fiscal_year = short.Parse(Request.QueryString["fiscalyear"]);
            long _organizationID = long.Parse(Request.QueryString["orgid"]);
            long _budgetid = long.Parse(Request.QueryString["budget_id"]);

            string _lockImported = "N";

            if (Request.QueryString["AdminImport"] != null)
            {
                _lockImported = "Y";
            }

            using(Entities _context = new Entities())
            {

                CheckboxSelectionModel _csm = uxPeriodSelectionModel;

                List<string> _periodsToImport = _csm.SelectedRows.Select(x => x.RecordID).ToList();
                List<OVERHEAD_BUDGET_FORECAST.GL_PERIOD> _periodList = OVERHEAD_BUDGET_FORECAST.GeneralLedgerPeriods(_context).Where(x => x.PERIOD_YEAR == _fiscal_year & x.PERIOD_TYPE == "Month" & _periodsToImport.Contains(x.PERIOD_NUM.ToString())).ToList();
                var _validAccounts = OVERHEAD_BUDGET_FORECAST.AccountListValidByOrganizationID(_context, _organizationID);

              
                //Add total detail
                foreach (GL_ACCOUNTS_V _validAccount in _validAccounts)
                {

                    foreach (OVERHEAD_BUDGET_FORECAST.GL_PERIOD _period in _periodList)
                        {
                          OVERHEAD_BUDGET_DETAIL _line = OVERHEAD_BUDGET_FORECAST.BudgetDetailByBudgetID(_context,_budgetid).Where(x => x.ORG_BUDGET_ID == _budgetid & x.CODE_COMBINATION_ID == _validAccount.CODE_COMBINATION_ID & x.PERIOD_NUM == _period.PERIOD_NUM).SingleOrDefault();
                          OVERHEAD_BUDGET_FORECAST.GL_ACTUALS _actualTotalLine = OVERHEAD_BUDGET_FORECAST.ActualsByYearAndAccountCodeAndPeriodNumber(_context, _fiscal_year, _validAccount.CODE_COMBINATION_ID, _period.PERIOD_NUM).SingleOrDefault();
                            decimal _aTotal = 0;

                            if (_actualTotalLine != null)
                            {
                                _aTotal = _actualTotalLine.PERIOD_NET_DR + Decimal.Negate(_actualTotalLine.PERIOD_NET_CR);
                            }
                            else
                            {
                                _aTotal = 0;
                            }

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
                                        _record.ACTUALS_IMPORTED_FLAG = _lockImported;
                                        GenericData.Insert<OVERHEAD_BUDGET_DETAIL>(_record);
                                    }
                                    else
                                    {
                                        //Data update it
                                        _line.AMOUNT = _aTotal;
                                        _line.MODIFY_DATE = DateTime.Now;
                                        _line.MODIFIED_BY = User.Identity.Name;
                                        _line.ACTUALS_IMPORTED_FLAG = _lockImported;
                                        GenericData.Update<OVERHEAD_BUDGET_DETAIL>(_line);
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
                long _budgetid = long.Parse(Request.QueryString["budget_id"]);

                string sql2 = "select entered_period_name,period_year,period_num,period_type,start_date,end_date,'N' as ACTUALS_IMPORTED_FLAG from gl.gl_periods where period_set_name = 'DBI Calendar' order by period_num";
                List<GL_PERIODS> _periodMonthList = _context.Database.SqlQuery<GL_PERIODS>(sql2).Where(x => x.PERIOD_YEAR == _fiscal_year & x.PERIOD_TYPE == "Month").ToList();


                string sqlLocked = string.Format("select PERIOD_NAME,PERIOD_NUM,ACTUALS_IMPORTED_FLAG, 'N' as ADMIN from xxems.overhead_budget_detail where org_budget_id = {0} and ACTUALS_IMPORTED_FLAG = 'Y' group by PERIOD_NAME,PERIOD_NUM,ACTUALS_IMPORTED_FLAG order by period_num", _budgetid);
                List<GL_LOCKED_PERIODS> _periodsLocked = _context.Database.SqlQuery<GL_LOCKED_PERIODS>(sqlLocked).ToList();

                foreach (GL_PERIODS _period in _periodMonthList)
                {
                    var _data = _periodsLocked.Where(x => x.PERIOD_NUM == _period.PERIOD_NUM).SingleOrDefault();
                    if (_data != null)
                    {
                        _period.ACTUALS_IMPORTED_FLAG = _data.ACTUALS_IMPORTED_FLAG;

                        if (Request.QueryString["AdminImport"] != null)
                        {
                            _period.ADMIN = "Y";
                        }
                        else
                        {
                            _period.ADMIN = "N";
                        }
                    }
                }

                int count;
                uxDetailStore.DataSource = GenericData.EnumerableFilterHeader<GL_PERIODS>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _periodMonthList, out count);
                e.Total = count;
            }
        }

        public class GL_LOCKED_PERIODS
        {
            public string PERIOD_NAME { get; set; }
            public long PERIOD_NUM { get; set; }
            public string ACTUALS_IMPORTED_FLAG { get; set; }
            public string ADMIN { get; set; }
        }

    }
}