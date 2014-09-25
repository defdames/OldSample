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

   
        protected void deLoadFiscalYears(object sender, StoreReadDataEventArgs e)
        {
            uxFiscalYearsStore.DataSource = PA.FiscalYearsGL().OrderByDescending(x => x.ID_NAME);
        }

        protected void deOpenPeriod(object sender, DirectEventArgs e)
        {

            long _businessUnitId = 0;

            if (Request.QueryString["orgID"] != "" && Request.QueryString["orgID"] != null)
                _businessUnitId = long.Parse(Request.QueryString["orgID"]);

            long _leID = 0;

            if (Request.QueryString["leID"] != "" && Request.QueryString["leID"] != null)
                _leID = long.Parse(Request.QueryString["leID"]);

            short _fiscalYear = short.Parse(uxFiscalYear.SelectedItem.Value);

            string _adminInsert = string.Empty;
            if (Request.QueryString["bulevel"] != "" && Request.QueryString["bulevel"] != null)
                _adminInsert = Request.QueryString["bulevel"];


            using (Entities _context = new Entities())
            {

                if (_adminInsert == "Y")
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
                            //Return the next budget type id
                            OVERHEAD_BUDGET_TYPE _nextBudgetType = OVERHEAD_BUDGET_TYPES.NextAvailBudgetTypeByOrganization(_version.ORGANIZATION_ID, _organizationID, _fiscalYear);

                            if (_nextBudgetType != null)
                            {

                                //find all budgets for the same year and organization and close them
                                List<OVERHEAD_ORG_BUDGETS> _organizationBudgets = _context.OVERHEAD_ORG_BUDGETS.Where(x => x.ORGANIZATION_ID == _version.ORGANIZATION_ID & x.FISCAL_YEAR == _fiscalYear & x.STATUS != "C" & x.OVERHEAD_BUDGET_TYPE_ID != _nextBudgetType.OVERHEAD_BUDGET_TYPE_ID).ToList();

                                foreach (OVERHEAD_ORG_BUDGETS _budget in _organizationBudgets)
                                {
                                    _budget.STATUS = "C";
                                    _budget.MODIFY_DATE = DateTime.Now;
                                    _budget.MODIFIED_BY = User.Identity.Name;
                                    GenericData.Update<OVERHEAD_ORG_BUDGETS>(_budget);
                                }

                                //Make sure it doesn't exist before you try to create it again
                                bool _budgetExist = _context.OVERHEAD_ORG_BUDGETS.Where(x => x.ORGANIZATION_ID == _version.ORGANIZATION_ID & x.FISCAL_YEAR == _fiscalYear & x.OVERHEAD_BUDGET_TYPE_ID == _nextBudgetType.OVERHEAD_BUDGET_TYPE_ID).Any();

                                if (!_budgetExist)
                                {
                                    //Create the new budget for the selected type.
                                    OVERHEAD_ORG_BUDGETS _newBudget = new OVERHEAD_ORG_BUDGETS();
                                    _newBudget.ORGANIZATION_ID = _version.ORGANIZATION_ID;
                                    _newBudget.STATUS = "O";
                                    _newBudget.OVERHEAD_BUDGET_TYPE_ID = _nextBudgetType.OVERHEAD_BUDGET_TYPE_ID;
                                    _newBudget.CREATE_DATE = DateTime.Now;
                                    _newBudget.MODIFY_DATE = DateTime.Now;
                                    _newBudget.CREATED_BY = User.Identity.Name;
                                    _newBudget.MODIFIED_BY = User.Identity.Name;
                                    _newBudget.FISCAL_YEAR = _fiscalYear;
                                    GenericData.Insert<OVERHEAD_ORG_BUDGETS>(_newBudget);

                                    //Get the parent budget ID if there is one
                                    OVERHEAD_BUDGET_TYPE _budgetType = OVERHEAD_BUDGET_TYPE.BudgetType(_newBudget.OVERHEAD_BUDGET_TYPE_ID);

                                    //If first budget and parent is null there will be nothing to copy
                                    if (_budgetType.PARENT_BUDGET_TYPE_ID != null)
                                    {
                                        //Search for budget header data of parent budget
                                        OVERHEAD_ORG_BUDGETS _prevBudgetData = _context.OVERHEAD_ORG_BUDGETS.Where(x => x.FISCAL_YEAR == _fiscalYear & x.ORGANIZATION_ID == _version.ORGANIZATION_ID & x.OVERHEAD_BUDGET_TYPE_ID == _budgetType.PARENT_BUDGET_TYPE_ID).SingleOrDefault();

                                        //Return detail for this budget
                                        List<OVERHEAD_BUDGET_DETAIL> _budgetDetail = _context.OVERHEAD_BUDGET_DETAIL.Where(x => x.ORG_BUDGET_ID == _prevBudgetData.ORG_BUDGET_ID).ToList();

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

                                        List<OVERHEAD_ACCOUNT_COMMENT> _comments = _context.OVERHEAD_ACCOUNT_COMMENT.Where(x => x.ORG_BUDGET_ID == _prevBudgetData.ORG_BUDGET_ID).ToList();

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

                                        //We need to update the new budget with any comments from the last one
                                        OVERHEAD_ORG_BUDGETS _insertedBudgetData = _context.OVERHEAD_ORG_BUDGETS.Where(x => x.FISCAL_YEAR == _fiscalYear & x.ORGANIZATION_ID == _version.ORGANIZATION_ID & x.OVERHEAD_BUDGET_TYPE_ID == _nextBudgetType.OVERHEAD_BUDGET_TYPE_ID).SingleOrDefault();
                                        _insertedBudgetData.COMMENTS = _prevBudgetData.COMMENTS;
                                        GenericData.Update<OVERHEAD_ORG_BUDGETS>(_insertedBudgetData);
                                    }
                                }
                            }
                            else
                            {
                                throw new DBICustomException("All periods have been created for this fiscal year.");
                            }

                        }

                    }

                }

                else
                {
                   

                    //Return the next budget type id
                            OVERHEAD_BUDGET_TYPE _nextBudgetType = OVERHEAD_BUDGET_TYPES.NextAvailBudgetTypeByOrganization(_businessUnitId, _leID, _fiscalYear);

                            if (_nextBudgetType != null)
                            {

                                //find all budgets for the same year and organization and close them
                                List<OVERHEAD_ORG_BUDGETS> _organizationBudgets = _context.OVERHEAD_ORG_BUDGETS.Where(x => x.ORGANIZATION_ID == _businessUnitId & x.FISCAL_YEAR == _fiscalYear & x.STATUS != "C" & x.OVERHEAD_BUDGET_TYPE_ID != _nextBudgetType.OVERHEAD_BUDGET_TYPE_ID).ToList();

                                foreach (OVERHEAD_ORG_BUDGETS _budget in _organizationBudgets)
                                {
                                    _budget.STATUS = "C";
                                    _budget.MODIFY_DATE = DateTime.Now;
                                    _budget.MODIFIED_BY = User.Identity.Name;
                                    GenericData.Update<OVERHEAD_ORG_BUDGETS>(_budget);
                                }

                                //Make sure it doesn't exist before you try to create it again
                                bool _budgetExist = _context.OVERHEAD_ORG_BUDGETS.Where(x => x.ORGANIZATION_ID == _businessUnitId & x.FISCAL_YEAR == _fiscalYear & x.OVERHEAD_BUDGET_TYPE_ID == _nextBudgetType.OVERHEAD_BUDGET_TYPE_ID).Any();

                                if (!_budgetExist)
                                {
                                    //Create the new budget for the selected type.
                                    OVERHEAD_ORG_BUDGETS _newBudget = new OVERHEAD_ORG_BUDGETS();
                                    _newBudget.ORGANIZATION_ID = _businessUnitId;
                                    _newBudget.STATUS = "O";
                                    _newBudget.OVERHEAD_BUDGET_TYPE_ID = _nextBudgetType.OVERHEAD_BUDGET_TYPE_ID;
                                    _newBudget.CREATE_DATE = DateTime.Now;
                                    _newBudget.MODIFY_DATE = DateTime.Now;
                                    _newBudget.CREATED_BY = User.Identity.Name;
                                    _newBudget.MODIFIED_BY = User.Identity.Name;
                                    _newBudget.FISCAL_YEAR = _fiscalYear;
                                    GenericData.Insert<OVERHEAD_ORG_BUDGETS>(_newBudget);

                                    //Get the parent budget ID if there is one
                                    OVERHEAD_BUDGET_TYPE _budgetType = OVERHEAD_BUDGET_TYPE.BudgetType(_newBudget.OVERHEAD_BUDGET_TYPE_ID);

                                    //If first budget and parent is null there will be nothing to copy
                                    if (_budgetType.PARENT_BUDGET_TYPE_ID != null)
                                    {
                                        //Search for budget header data of parent budget
                                        OVERHEAD_ORG_BUDGETS _prevBudgetData = _context.OVERHEAD_ORG_BUDGETS.Where(x => x.FISCAL_YEAR == _fiscalYear & x.ORGANIZATION_ID == _businessUnitId & x.OVERHEAD_BUDGET_TYPE_ID == _budgetType.PARENT_BUDGET_TYPE_ID).SingleOrDefault();

                                        //Return detail for this budget
                                        List<OVERHEAD_BUDGET_DETAIL> _budgetDetail = _context.OVERHEAD_BUDGET_DETAIL.Where(x => x.ORG_BUDGET_ID == _prevBudgetData.ORG_BUDGET_ID).ToList();

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

                                        List<OVERHEAD_ACCOUNT_COMMENT> _comments = _context.OVERHEAD_ACCOUNT_COMMENT.Where(x => x.ORG_BUDGET_ID == _prevBudgetData.ORG_BUDGET_ID).ToList();

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

                                        //We need to update the new budget with any comments from the last one
                                        OVERHEAD_ORG_BUDGETS _insertedBudgetData = _context.OVERHEAD_ORG_BUDGETS.Where(x => x.FISCAL_YEAR == _fiscalYear & x.ORGANIZATION_ID == _businessUnitId & x.OVERHEAD_BUDGET_TYPE_ID == _nextBudgetType.OVERHEAD_BUDGET_TYPE_ID).SingleOrDefault();
                                        _insertedBudgetData.COMMENTS = _prevBudgetData.COMMENTS;
                                        GenericData.Update<OVERHEAD_ORG_BUDGETS>(_insertedBudgetData);
                                    }
                                }







                            }

                            else
                            {
                                throw new DBICustomException("All periods have been created for this fiscal year.");
                            }

                }



            }
        }
    }
}