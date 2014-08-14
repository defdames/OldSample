using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Ext.Net;
using DBI.Data;
using DBI.Core.Web;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umViewActualsWindow : BasePage
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

        protected void uxDetailStore_ReadData(object sender, StoreReadDataEventArgs e)
        {

            using (Entities _context = new Entities())
            {
                short _fiscal_year = short.Parse(Request.QueryString["fiscalyear"]);
                long _accountID = long.Parse(Request.QueryString["accountID"]);
                
                string sql2 = "select entered_period_name,period_year,period_num,period_type,start_date,end_date, 0 as PERIOD_DR, 0 as PERIOD_CR, 0 as PERIOD_TOTAL from gl.gl_periods where period_set_name = 'DBI Calendar' order by period_num";
                List<GL_PERIODS> _periodMonthList = _context.Database.SqlQuery<GL_PERIODS>(sql2).Where(x => x.PERIOD_YEAR == _fiscal_year & x.PERIOD_TYPE == "Month").ToList();

                string sql = string.Format("select period_net_dr, period_net_cr,period_year,code_combination_id,period_num from gl.gl_balances where actual_flag = 'A' and period_year = {0} and code_combination_id = {1} and set_of_books_id in (select distinct set_of_books_id from apps.hr_operating_units)", _fiscal_year, _accountID);
                    List<ACTUAL_BALANCES> _balance = _context.Database.SqlQuery<ACTUAL_BALANCES>(sql).ToList();

                foreach(GL_PERIODS _period in _periodMonthList)
                {
                    ACTUAL_BALANCES _actualTotalLine = _balance.Where(x => x.PERIOD_NUM == _period.PERIOD_NUM).SingleOrDefault();
                    if (_actualTotalLine != null)
                    {
                        _period.PERIOD_DR = _actualTotalLine.PERIOD_NET_DR;
                        _period.PERIOD_CR = _actualTotalLine.PERIOD_NET_CR;
                        _period.PERIOD_TOTAL = _period.PERIOD_DR + Decimal.Negate(_period.PERIOD_CR);
                    }
                    else
                    {
                        _period.PERIOD_DR = 0;
                        _period.PERIOD_CR = 0;
                        _period.PERIOD_TOTAL = 0;
                    }
                }

                int count;
                uxDetailStore.DataSource = GenericData.EnumerableFilterHeader<GL_PERIODS>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _periodMonthList, out count);
                e.Total = count;
            }
        }

        protected void deLoadDetails(object sender, DirectEventArgs e)
        {
            Store1.Reload();
        }

        public class BALANCE_DETAILS
        {
            public string ROW_ID { get; set; }
            public string LINE_REFERENCE { get; set; }
            public string LINE_DESCRIPTION { get; set; }
            public decimal DEBIT { get; set; }
            public decimal CREDIT { get; set; }
            public string CATEGORY { get; set; }
            public DateTime EFFECTIVE_DATE { get; set; }
            public decimal TOTAL { get; set; }
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
            public Decimal PERIOD_DR { get; set; }
            public Decimal PERIOD_CR { get; set; }
            public Decimal PERIOD_TOTAL { get; set; }
        }

        protected void Store1_ReadData(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                try
                {
                    short _fiscal_year = short.Parse(Request.QueryString["fiscalyear"]);
                    long _accountID = long.Parse(Request.QueryString["accountID"]);
                    string sql2 = string.Format("select ROW_ID as ROW_ID, Line_reference_1 as LINE_REFERENCE, Line_description as LINE_DESCRIPTION, nvl(line_entered_dr,0) AS DEBIT, nvl(line_entered_cr,0) AS CREDIT, je_category AS CATEGORY, header_effective_date AS EFFECTIVE_DATE, 0 as TOTAL from APPS.GL_JE_JOURNAL_LINES_V where period_year = {0} and period_num = {1} and line_code_combination_id = {2} and set_of_books_id in (select distinct set_of_books_id from apps.hr_operating_units)", _fiscal_year, uxPeriodSelectionModel.SelectedRow.RecordID, _accountID);
                    List<BALANCE_DETAILS> _details = _context.Database.SqlQuery<BALANCE_DETAILS>(sql2).ToList();

                    foreach (BALANCE_DETAILS _detail in _details)
                    {
                        _detail.TOTAL = (_detail.DEBIT + Decimal.Negate(_detail.CREDIT));
                    }

                    int count;
                    Store1.DataSource = GenericData.EnumerableFilterHeader<BALANCE_DETAILS>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _details, out count);
                    e.Total = count;
                }
                catch (Exception ex)
                {
                    X.Msg.Alert("Error", ex.ToString()).Show();
                }
               
            }
        }
    }
}