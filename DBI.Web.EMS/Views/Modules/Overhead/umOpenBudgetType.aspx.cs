using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DBI.Data;
using DBI.Core.Web;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umOpenBudgetType : DBI.Core.Web.BasePage
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
            long _leID = 0;

            if (Request.QueryString["leID"] != "" && Request.QueryString["leID"] != null)
                _leID = long.Parse(Request.QueryString["leID"]);

            long _orgID = 0;

            if (Request.QueryString["orgID"] != "" && Request.QueryString["orgID"] != null)
                _orgID = long.Parse(Request.QueryString["orgID"]);

            var _data = OVERHEAD_BUDGET_TYPES.NextAvailBudgetTypeByOrganization(_orgID,_leID, long.Parse(uxFiscalYear.SelectedItem.Value));

            if (_data.Count() > 0)
            {
                uxBudgetNameStore.DataSource = _data;
                uxOpenPeriod.Enable();
            }

        }

        protected void deLoadFiscalYears(object sender, StoreReadDataEventArgs e)
        {
            uxFiscalYearsStore.DataSource = PA.FiscalYearsGL().OrderByDescending(x => x.ID_NAME);
        }

        protected void deOpenPeriod(object sender, DirectEventArgs e)
        {

            try
            {

           
            long _businessUnitId = 0;

            if (Request.QueryString["orgID"] != "" && Request.QueryString["orgID"] != null)
                _businessUnitId = long.Parse(Request.QueryString["orgID"]);

            short _fiscalYear = short.Parse(uxFiscalYear.SelectedItem.Value);



            using (Entities _context = new Entities())
            {
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

                    if (Request.QueryString["leID"] != "" && Request.QueryString["leID"] != null)
                        _leID = long.Parse(Request.QueryString["leID"]);

                    OVERHEAD_BUDGET_TYPE _budgetTypeData = OVERHEAD_BUDGET_TYPE.BudgetTypes(_leID).Where(x => x.OVERHEAD_BUDGET_TYPE_ID == _newBudget.OVERHEAD_BUDGET_TYPE_ID).SingleOrDefault();

                    if(_budgetTypeData != null)
                    {
                        //Allow Copy of data since there is a parent

                        //Copy data from old budget to new budget

                        //Old Budget Data
                        OVERHEAD_ORG_BUDGETS _budgetHeader = _context.OVERHEAD_ORG_BUDGETS.Where(x => x.FISCAL_YEAR == _fiscalYear & x.ORGANIZATION_ID == _businessUnitId & x.OVERHEAD_BUDGET_TYPE_ID == _budgetTypeData.PARENT_BUDGET_TYPE_ID).SingleOrDefault();


                        List<OVERHEAD_BUDGET_DETAIL> _budgetDetail = _context.OVERHEAD_BUDGET_DETAIL.Where(x => x.ORG_BUDGET_ID == _budgetHeader.ORG_BUDGET_ID).ToList();

                        List<OVERHEAD_BUDGET_DETAIL> _newDetail = new List<OVERHEAD_BUDGET_DETAIL>();
                        foreach (OVERHEAD_BUDGET_DETAIL _line in _budgetDetail)
                        {
                            OVERHEAD_BUDGET_DETAIL _record = new OVERHEAD_BUDGET_DETAIL();
                            _record.ORG_BUDGET_ID = _newBudget.ORG_BUDGET_ID;
                            _record.PERIOD_NAME = _line.PERIOD_NAME;
                            _record.PERIOD_NUM = _line.PERIOD_NUM;
                            _record.CODE_COMBINATION_ID = _line.CODE_COMBINATION_ID;
                            _record.AMOUNT = _line.AMOUNT;
                            _record.CREATE_DATE = DateTime.Now;
                            _record.MODIFY_DATE = DateTime.Now;
                            _record.CREATED_BY = User.Identity.Name;
                            _record.MODIFIED_BY = User.Identity.Name;
                            _record.ACTUALS_IMPORTED_FLAG = "N";
                            GenericData.Insert<OVERHEAD_BUDGET_DETAIL>(_record);
                        }

                        List<OVERHEAD_ACCOUNT_COMMENT> _comments = _context.OVERHEAD_ACCOUNT_COMMENT.Where(x => x.ORG_BUDGET_ID == _budgetHeader.ORG_BUDGET_ID).ToList();

                        foreach (OVERHEAD_ACCOUNT_COMMENT _comment in _comments)
                        {
                            OVERHEAD_ACCOUNT_COMMENT _c = new OVERHEAD_ACCOUNT_COMMENT();
                            _c.CODE_COMBINATION_ID = _comment.CODE_COMBINATION_ID;
                            _c.COMMENTS = _comment.COMMENTS;
                            _c.CREATE_DATE = (DateTime?)DateTime.Now;
                            _c.MODIFY_DATE = (DateTime?)DateTime.Now;
                            _c.CREATED_BY = User.Identity.Name;
                            _c.MODIFIED_BY = User.Identity.Name;
                            _c.ORG_BUDGET_ID = _newBudget.ORG_BUDGET_ID;
                            GenericData.Insert<OVERHEAD_ACCOUNT_COMMENT>(_c);
                        }


                    }


            }


            }
            catch (Exception ex)
            {
                X.Msg.Alert("Error", ex.ToString()).Show();
            }


        }
    }
}