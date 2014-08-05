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
            uxFiscalYearsStore.DataSource = PA.AllFiscalYears().OrderByDescending(x => x.ID_NAME).Take(2);
        }

        protected void deOpenPeriod(object sender, DirectEventArgs e)
        {
            long _businessUnitId = 0;

            long _budgetID = long.Parse(Request.QueryString["budget_id"]);


            if (Request.QueryString["orgID"] != "" && Request.QueryString["orgID"] != null)
                _businessUnitId = long.Parse(Request.QueryString["orgID"]);


            //close budget
            using (Entities _context = new Entities())
            {
                OVERHEAD_ORG_BUDGETS _oldBudgetInfo = _context.OVERHEAD_ORG_BUDGETS.Where(x => x.ORG_BUDGET_ID == _budgetID).SingleOrDefault();
                _oldBudgetInfo.STATUS = "C";
                GenericData.Update<OVERHEAD_ORG_BUDGETS>(_oldBudgetInfo);

                //Create new budget
                OVERHEAD_ORG_BUDGETS _budget = new OVERHEAD_ORG_BUDGETS();
                _budget.ORGANIZATION_ID = _businessUnitId;
                _budget.STATUS = "O";
                _budget.OVERHEAD_BUDGET_TYPE_ID = long.Parse(uxBudgetName.SelectedItem.Value);
                _budget.CREATE_DATE = DateTime.Now;
                _budget.MODIFY_DATE = DateTime.Now;
                _budget.CREATED_BY = User.Identity.Name;
                _budget.MODIFIED_BY = User.Identity.Name;
                _budget.FISCAL_YEAR = short.Parse(uxFiscalYear.SelectedItem.Value);
                GenericData.Insert<OVERHEAD_ORG_BUDGETS>(_budget);

                //Copy data from old budget to new budget

                List<OVERHEAD_BUDGET_DETAIL> _budgetDetail = _context.OVERHEAD_BUDGET_DETAIL.Where(x => x.ORG_BUDGET_ID == _budgetID).ToList();

                foreach (OVERHEAD_BUDGET_DETAIL _line in _budgetDetail)
                {
                    OVERHEAD_BUDGET_DETAIL _d = new OVERHEAD_BUDGET_DETAIL();
                    _d.CREATE_DATE = DateTime.Now;
                    _d.MODIFY_DATE = DateTime.Now;
                    _d.CREATED_BY = User.Identity.Name;
                    _d.MODIFIED_BY = User.Identity.Name;
                    _d.ORG_BUDGET_ID = _budget.ORG_BUDGET_ID;
                    _d.PERIOD_NAME = _line.PERIOD_NAME;
                    _d.PERIOD_NUM = _line.PERIOD_NUM;
                    _d.DETAIL_TYPE = _line.DETAIL_TYPE;
                    _d.CODE_COMBINATION_ID = _line.CODE_COMBINATION_ID;
                    _d.AMOUNT = _line.AMOUNT;
                    GenericData.Insert<OVERHEAD_BUDGET_DETAIL>(_d);
                }

                List<OVERHEAD_ACCOUNT_COMMENT> _comments = _context.OVERHEAD_ACCOUNT_COMMENT.Where(x => x.ORG_BUDGET_ID == _budgetID).ToList();

                foreach (OVERHEAD_ACCOUNT_COMMENT _comment in _comments)
                {
                    OVERHEAD_ACCOUNT_COMMENT _c = new OVERHEAD_ACCOUNT_COMMENT();
                    _c.CODE_COMBINATION_ID = _comment.CODE_COMBINATION_ID;
                    _c.COMMENTS = _comment.COMMENTS;
                    _c.CREATE_DATE = DateTime.Now;
                    _c.MODIFY_DATE = DateTime.Now;
                    _c.CREATED_BY = User.Identity.Name;
                    _c.MODIFIED_BY = User.Identity.Name;
                    _c.ORG_BUDGET_ID = _budget.ORG_BUDGET_ID;
                    GenericData.Insert<OVERHEAD_ACCOUNT_COMMENT>(_c);
                }

            }
        }
    }
}