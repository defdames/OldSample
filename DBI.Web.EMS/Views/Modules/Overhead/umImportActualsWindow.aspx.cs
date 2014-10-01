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

                if (Request.QueryString["fiscalyear"] != null)
                {
                    uxFiscalYear.SelectedItem.Value = Request.QueryString["fiscalyear"];
                    uxFiscalYear.Text = Request.QueryString["fiscalyear"];
                    uxFiscalYear.ReadOnly = true;
                    uxPeriodName.Enable();
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
            

            string _lockImported = "N";

            if (Request.QueryString["AdminImport"] != null)
            {
                _lockImported = "Y";
            }

                using (Entities _context = new Entities())
                {
                    long _periodToImport = 0;
                    _periodToImport = long.Parse(uxPeriodName.SelectedItem.Value);

                    short _fiscalYear = 0;
                    _fiscalYear = short.Parse(uxFiscalYear.SelectedItem.Value);

                    if (Request.QueryString["budget_id"] == null)
                    {

                        ///We have to open all organizations
                        string _selectedRecordID = Request.QueryString["combinedLEORGID"];
                        if (_selectedRecordID != null)
                        {
                            char[] _delimiterChars = { ':' };
                            string[] _selectedID = _selectedRecordID.Split(_delimiterChars);
                            long _hierarchyID = long.Parse(_selectedID[1].ToString());
                            long _organizationID = long.Parse(_selectedID[0].ToString());

                            //Get a list of all active organizations for the current legal entity
                            var _data = HR.OverheadOrganizationStatusByHierarchy(_hierarchyID, _organizationID).Where(x => x.ORGANIZATION_STATUS == "Active").ToList();

                            foreach (HR.ORGANIZATION_V1 _version in _data)
                            {
                                List<OVERHEAD_ORG_BUDGETS> _budgets = OVERHEAD_BUDGET_FORECAST.BudgetsByOrganizationID(_context, _version.ORGANIZATION_ID).Where(x => x.FISCAL_YEAR == _fiscalYear & x.STATUS == "O").ToList();

                                foreach (var _budget in _budgets)
                                {
                                    var _budgetType = OVERHEAD_BUDGET_TYPE.BudgetType(_budget.OVERHEAD_BUDGET_TYPE_ID);

                                    if (_budgetType.IMPORT_ACTUALS_ALLOWED == "Y")
                                    {
                                        OVERHEAD_BUDGET_FORECAST.ImportActualForBudgetVersion(_context, _periodToImport, _budget.ORG_BUDGET_ID, User.Identity.Name, _lockImported);
                                    }
                                }
                            }
                        }

                    }

                    else
                    {
                        //Import as admin just one period and one budget that was selected.
                        long _budgetID = 0;
                        _budgetID = long.Parse(Request.QueryString["budget_id"]);

                        OVERHEAD_BUDGET_FORECAST.ImportActualForBudgetVersion(_context, _periodToImport, _budgetID, User.Identity.Name, _lockImported);
                    }

                }

        }

        protected void deLoadFiscalYears(object sender, StoreReadDataEventArgs e)
        {
            uxFiscalYearsStore.DataSource = PA.FiscalYearsGL().OrderByDescending(x => x.ID_NAME);
        }

        protected void deLoadPeriodNames(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                short _fiscal_year = short.Parse(uxFiscalYear.SelectedItem.Value);

                string sql2 = "select entered_period_name,period_year,period_num,period_type,start_date,end_date,'N' as ACTUALS_IMPORTED_FLAG from gl.gl_periods where period_set_name = 'DBI Calendar' order by period_num";
                List<GL_PERIODS> _periodMonthList = _context.Database.SqlQuery<GL_PERIODS>(sql2).Where(x => x.PERIOD_YEAR == _fiscal_year & x.PERIOD_TYPE == "Month").ToList();

                List<GL_LOCKED_PERIODS> _periodsLocked = new List<GL_LOCKED_PERIODS>();
                List<DBI.Data.Generic.DoubleComboLongID> _periodList = new List<DBI.Data.Generic.DoubleComboLongID>();

                if (Request.QueryString["AdminImport"] == null)
                {
                    long _budgetID = long.Parse(Request.QueryString["budget_id"]);

                    string sqlLocked = string.Format("select PERIOD_NAME,PERIOD_NUM,ACTUALS_IMPORTED_FLAG, 'N' as ADMIN from xxems.overhead_budget_detail where org_budget_id = {0} and ACTUALS_IMPORTED_FLAG = 'Y' group by PERIOD_NAME,PERIOD_NUM,ACTUALS_IMPORTED_FLAG order by period_num", _budgetID);
                    _periodsLocked = _context.Database.SqlQuery<GL_LOCKED_PERIODS>(sqlLocked).ToList();
                }


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