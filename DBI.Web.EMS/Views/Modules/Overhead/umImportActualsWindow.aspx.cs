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
                OVERHEAD_BUDGET_FORECAST.ImportActualForBudgetVersion(_context, long.Parse(uxPeriodName.SelectedItem.Value.ToString()), _budgetid, User.Identity.Name, _lockImported);
            }
        }

        protected void deLoadPeriodNames(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                short _fiscal_year = short.Parse(Request.QueryString["fiscalyear"]);
                long _budgetid = long.Parse(Request.QueryString["budget_id"]);

                string sql2 = "select entered_period_name,period_year,period_num,period_type,start_date,end_date,'N' as ACTUALS_IMPORTED_FLAG from gl.gl_periods where period_set_name = 'DBI Calendar' order by period_num";
                List<GL_PERIODS> _periodMonthList = _context.Database.SqlQuery<GL_PERIODS>(sql2).Where(x => x.PERIOD_YEAR == _fiscal_year & x.PERIOD_TYPE == "Month").ToList();

                string sqlLocked = string.Format("select PERIOD_NAME,PERIOD_NUM,ACTUALS_IMPORTED_FLAG, 'N' as ADMIN from xxems.overhead_budget_detail where org_budget_id = {0} and ACTUALS_IMPORTED_FLAG = 'Y' group by PERIOD_NAME,PERIOD_NUM,ACTUALS_IMPORTED_FLAG order by period_num", _budgetid);
                List<GL_LOCKED_PERIODS> _periodsLocked = _context.Database.SqlQuery<GL_LOCKED_PERIODS>(sqlLocked).ToList();

                List<DBI.Data.Generic.DoubleComboLongID> _periodList = new List<DBI.Data.Generic.DoubleComboLongID>();

                foreach (GL_PERIODS _period in _periodMonthList)
                {
                    var _data = _periodsLocked.Where(x => x.PERIOD_NUM == _period.PERIOD_NUM).SingleOrDefault();

                    if (Request.QueryString["AdminImport"] != null)
                    {
                        DBI.Data.Generic.DoubleComboLongID _item = new DBI.Data.Generic.DoubleComboLongID();
                        _item.ID = _period.PERIOD_NUM;
                        _item.ID_NAME = _period.ENTERED_PERIOD_NAME;
                        _periodList.Add(_item);
                    }
                    else
                    {
                        if (_data == null)
                        {
                            DBI.Data.Generic.DoubleComboLongID _item = new DBI.Data.Generic.DoubleComboLongID();
                            _item.ID = _period.PERIOD_NUM;
                            _item.ID_NAME = _period.ENTERED_PERIOD_NAME;
                            _periodList.Add(_item);
                        }


                    }


                }

                uxPeriodNameStore.DataSource = _periodList;
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