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
    public partial class umAddOverheadDetailLine : BasePage
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

        protected void deLoadDetailLinesStore(object sender, StoreReadDataEventArgs e)
        {
            long _budget_id = long.Parse(Request.QueryString["budgetID"]);
            long _account_id = long.Parse(Request.QueryString["accountID"]);
            short _fiscal_year = short.Parse(Request.QueryString["fiscalyear"]);


            using (Entities _context = new Entities())
            {
                List<OVERHEAD_BUDGET_DETAIL> _detail = _context.OVERHEAD_BUDGET_DETAIL.Where(x => x.ORG_BUDGET_ID == _budget_id & x.CODE_COMBINATION_ID == _account_id & x.DETAIL_TYPE == "B").OrderBy(x => x.PERIOD_NUM).ToList();

                if (_detail.Count() == 0)
                {
                    //We need to create the budget lines in the system for each period.
                    string sql = "select entered_period_name,period_year,period_num,period_type,start_date,end_date from gl.gl_periods where period_set_name = 'DBI Calendar' order by period_num";
                    List<GL_PERIODS> _periodList = _context.Database.SqlQuery<GL_PERIODS>(sql).Where(x => x.PERIOD_YEAR == _fiscal_year & x.PERIOD_TYPE == "Month").ToList();

                    foreach (GL_PERIODS _period in _periodList)
                    {
                        OVERHEAD_BUDGET_DETAIL _record = new OVERHEAD_BUDGET_DETAIL();
                        _record.ORG_BUDGET_ID = _budget_id;
                        _record.PERIOD_NAME = _period.ENTERED_PERIOD_NAME;
                        _record.PERIOD_NUM = _period.PERIOD_NUM;
                        _record.DETAIL_TYPE = "B";
                        _record.CODE_COMBINATION_ID = _account_id;
                        _record.AMOUNT = 0;
                        _record.CREATE_DATE = DateTime.Now;
                        _record.MODIFY_DATE = DateTime.Now;
                        _record.CREATED_BY = User.Identity.Name;
                        _record.MODIFIED_BY = User.Identity.Name;
                        GenericData.Insert<OVERHEAD_BUDGET_DETAIL>(_record);
                    }

                    _detail = _context.OVERHEAD_BUDGET_DETAIL.Where(x => x.ORG_BUDGET_ID == _budget_id & x.CODE_COMBINATION_ID == _account_id & x.DETAIL_TYPE == "B").OrderBy(x => x.PERIOD_NUM).ToList();
                }

                int count;
                uxDetailStore.DataSource = GenericData.EnumerableFilterHeader<OVERHEAD_BUDGET_DETAIL>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], _detail, out count);
                e.Total = count;

            }

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

        protected void deSaveDetailLine(object sender, DirectEventArgs e)
        {
            string json = e.ExtraParams["Values"];

            List<OVERHEAD_BUDGET_DETAIL> _gridValues = JSON.Deserialize<List<OVERHEAD_BUDGET_DETAIL>>(json);

            foreach (OVERHEAD_BUDGET_DETAIL _detail in _gridValues)
            {
                _detail.MODIFIED_BY = User.Identity.Name;
                _detail.MODIFY_DATE = DateTime.Now;
            }

            GenericData.Update<OVERHEAD_BUDGET_DETAIL>(_gridValues);

        }

    }
}