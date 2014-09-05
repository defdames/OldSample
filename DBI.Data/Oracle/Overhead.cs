using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data
{
    public class OVERHEAD_MODULE
    {
        /// <summary>
        /// Returns the gl account ranges for a selected organization.
        /// </summary>
        /// <param name="organizationID"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IQueryable<OVERHEAD_GL_RANGE_V> OverheadGLRangeByOrganizationId(long organizationID, Entities context)
        {
            var _data = context.OVERHEAD_GL_RANGE.Where(x => x.ORGANIZATION_ID == organizationID)
                .Select(x => new OVERHEAD_GL_RANGE_V
                {
                    GL_RANGE_ID = x.GL_RANGE_ID,
                    ORGANIZATION_ID = x.ORGANIZATION_ID,
                    SRSEGMENTS = x.SRSEGMENT1 + "." + x.SRSEGMENT2 + "." + x.SRSEGMENT3 + "." + x.SRSEGMENT4 + "." + x.SRSEGMENT5 + "." + x.SRSEGMENT6 + "." + x.SRSEGMENT7,
                    ERSEGMENTS = x.ERSEGMENT1 + "." + x.ERSEGMENT2 + "." + x.ERSEGMENT3 + "." + x.ERSEGMENT4 + "." + x.ERSEGMENT5 + "." + x.ERSEGMENT6 + "." + x.ERSEGMENT7
                });
            return _data;
        }
    }

    public partial class OVERHEAD_BUDGET_TYPE
    {

        public static string GetDescriptionByTypeId(long budgetTypeID)
        {
            using (Entities _context = new Entities())
            {
                return _context.OVERHEAD_BUDGET_TYPE.Where(x => x.OVERHEAD_BUDGET_TYPE_ID == budgetTypeID).SingleOrDefault().BUDGET_DESCRIPTION;
            }
        }
    }

    public partial class OVERHEAD_GL_RANGE
    {

        public static OVERHEAD_GL_RANGE OverheadRangeByID(long rangeID)
        {
            using (Entities _context = new Entities())
            {
                return _context.OVERHEAD_GL_RANGE.Where(x => x.GL_RANGE_ID == rangeID).SingleOrDefault();
            }

        }

    }

    public partial class GL_ACCOUNTS_V
    {
        public static IQueryable<GL_ACCOUNTS_V> AccountListByRange(long rangeID, Entities context)
        {
            //Return Range
            OVERHEAD_GL_RANGE _range = OVERHEAD_GL_RANGE.OverheadRangeByID(rangeID);

            var _data = context.GL_ACCOUNTS_V.Where(x => String.Compare(x.SEGMENT1, _range.SRSEGMENT1) >= 0 && String.Compare(x.SEGMENT1, _range.ERSEGMENT1) <= 0);
            _data = _data.Where(x => String.Compare(x.SEGMENT2, _range.SRSEGMENT2) >= 0 && String.Compare(x.SEGMENT2, _range.ERSEGMENT2) <= 0);
            _data = _data.Where(x => String.Compare(x.SEGMENT3, _range.SRSEGMENT3) >= 0 && String.Compare(x.SEGMENT3, _range.ERSEGMENT3) <= 0);
            _data = _data.Where(x => String.Compare(x.SEGMENT4, _range.SRSEGMENT4) >= 0 && String.Compare(x.SEGMENT4, _range.ERSEGMENT4) <= 0);
            _data = _data.Where(x => String.Compare(x.SEGMENT5, _range.SRSEGMENT5) >= 0 && String.Compare(x.SEGMENT5, _range.ERSEGMENT5) <= 0);
            _data = _data.Where(x => String.Compare(x.SEGMENT6, _range.SRSEGMENT6) >= 0 && String.Compare(x.SEGMENT6, _range.ERSEGMENT6) <= 0);
            _data = _data.Where(x => String.Compare(x.SEGMENT7, _range.SRSEGMENT7) >= 0 && String.Compare(x.SEGMENT7, _range.ERSEGMENT7) <= 0);

            return _data;

        }

    }

    public partial class OVERHEAD_ORG_BUDGETS
    {
        public static IQueryable<OVERHEAD_ORG_BUDGETS_V> BudgetListByOrganizationID(long organizationID, Entities context)
        {
            var data = context.OVERHEAD_ORG_BUDGETS.Where(x => x.ORGANIZATION_ID == organizationID).ToList();
            List<OVERHEAD_ORG_BUDGETS_V> _rdata = new List<OVERHEAD_ORG_BUDGETS_V>();

            foreach (OVERHEAD_ORG_BUDGETS _budget in data)
            {
                OVERHEAD_ORG_BUDGETS_V _r = new OVERHEAD_ORG_BUDGETS_V();
                _r.BUDGET_DESCRIPTION = OVERHEAD_BUDGET_TYPE.GetDescriptionByTypeId(_budget.OVERHEAD_BUDGET_TYPE_ID);
                _r.OVERHEAD_BUDGET_TYPE_ID = _budget.OVERHEAD_BUDGET_TYPE_ID;
                _r.BUDGET_STATUS = (_budget.STATUS == "O") ? "Open" : (_budget.STATUS == "C") ? "Closed" : (_budget.STATUS == "L") ? "Locked" : "Never Opened";
                _r.ORG_BUDGET_ID = _budget.ORG_BUDGET_ID;
                _r.ORGANIZATION_ID = _budget.ORGANIZATION_ID;
                _r.ORGANIZATION_NAME = HR.Organization(_budget.ORGANIZATION_ID).ORGANIZATION_NAME;
                _r.FISCAL_YEAR = _budget.FISCAL_YEAR;
                _rdata.Add(_r);
            }

            return _rdata.AsQueryable();
        }
    }

    public partial class OVERHEAD_BUDGET_TYPES
    {

        public static List<OVERHEAD_BUDGET_TYPE> NextAvailBudgetTypeByOrganization(long organizationID, long legalEntityID, long fiscalYear)
        {
            using (Entities _context = new Entities())
            {
                IQueryable<OVERHEAD_BUDGET_TYPE> _data = OVERHEAD_BUDGET_TYPE.BudgetTypes(legalEntityID).AsQueryable();

                _data = (from dups in _data
                         where !_context.OVERHEAD_ORG_BUDGETS.Any(x => x.OVERHEAD_BUDGET_TYPE_ID == dups.OVERHEAD_BUDGET_TYPE_ID && x.FISCAL_YEAR == fiscalYear && x.ORGANIZATION_ID == organizationID)
                         select dups);

                var _rdata = _data.OrderBy(x => x.LE_ORG_ID).Take(1).ToList();
                return _rdata;
            }
        }

        public DataSet test()
        {
            DataSet _t = new DataSet();
            return _t;
        }
    }

    public class OVERHEAD_GL_RANGE_V : OVERHEAD_GL_RANGE
    {
        public string SRSEGMENTS { get; set; }
        public string ERSEGMENTS { get; set; }
        public string INCLUDE_EXCLUDE { get; set; }
    }

    public class OVERHEAD_ORG_BUDGETS_V : OVERHEAD_ORG_BUDGETS
    {
        public string BUDGET_DESCRIPTION { get; set; }
        public string BUDGET_STATUS { get; set; }
        public string ORGANIZATION_NAME { get; set; }
        public string ACCOUNT_RANGE { get; set; }
    }

    public class GL_PERIODS_V
    {
        public string ENTERED_PERIOD_NAME { get; set; }
        public short PERIOD_YEAR { get; set; }
        public short PERIOD_NUM { get; set; }
        public string PERIOD_TYPE { get; set; }
        public DateTime START_DATE { get; set; }
        public DateTime END_DATE { get; set; }
    }





    public class OVERHEAD_BUDGET_FORECAST
    {
        #region Category Maintenance

        /// <summary>
        /// Returns a list of Overhead Categories that can be queried
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IQueryable<OVERHEAD_CATEGORY> CategoryAsQueryable(Entities context)
        {
            var _data = context.OVERHEAD_CATEGORY.AsQueryable();
            return _data;
        }

        /// <summary>
        /// Returns a Overhead Category by ID
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static OVERHEAD_CATEGORY CategoryByID(Entities context, long ID)
        {
            var _data = CategoryAsQueryable(context).Where(x => x.CATEGORY_ID == ID).SingleOrDefault();
            return _data;
        }

        /// <summary>
        /// Returns a list of accounts by category Id
        /// </summary>
        /// <param name="context"></param>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public static IQueryable<OVERHEAD_ACCOUNT_CATEGORY> AccountCategoriesByCategoryID(Entities context, long categoryID)
        {
            var _data = context.OVERHEAD_ACCOUNT_CATEGORY.Where(x => x.CATEGORY_ID == categoryID);
            return _data;
        }

        /// <summary>
        /// Returns an account category by account category ID
        /// </summary>
        /// <param name="context"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static OVERHEAD_ACCOUNT_CATEGORY AccountCategoryByID(Entities context, long ID)
        {
            var _data = AccountAsQueryable(context).Where(x => x.ACCOUNT_CATEGORY_ID == ID).SingleOrDefault();
            return _data;
        }

        /// <summary>
        /// Returns a list of account categories as queryable.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IQueryable<OVERHEAD_ACCOUNT_CATEGORY> AccountAsQueryable(Entities context)
        {
            var _data = context.OVERHEAD_ACCOUNT_CATEGORY.AsQueryable();
            return _data;
        }

        /// <summary>
        /// Gets the description of an account by segment number and position
        /// </summary>
        /// <param name="context"></param>
        /// <param name="segmentPosition"></param>
        /// <param name="segmentCode"></param>
        /// <returns></returns>
        public static string AccountDescriptionBySegment(Entities context, long segmentPosition, string segmentCode)
        {
            string _sql = string.Format("select XXEMS.GET_GL_ACCOUNT_DESCRIPTION (50328,{0},{1}) from dual", segmentPosition, segmentCode);
            string _accountDescription = context.Database.SqlQuery<string>(_sql).FirstOrDefault();
            return _accountDescription;
        }







        public class ACCOUNT_CATEGORY_LIST : OVERHEAD_ACCOUNT_CATEGORY
        {
            public string ACCOUNT_SEGMENT_DESC { get; set; }
        }


        #endregion

        #region Budget Versions

        /// <summary>
        /// Returns a list of budgets by organization id
        /// </summary>
        /// <param name="context"></param>
        /// <param name="organizationID"></param>
        /// <returns></returns>
        public static IQueryable<OVERHEAD_ORG_BUDGETS> BudgetsByOrganizationID(Entities context, long organizationID)
        {
            var _data = context.OVERHEAD_ORG_BUDGETS.Where(x => x.ORGANIZATION_ID == organizationID);
            return _data;
        }

        /// <summary>
        /// Returns a description of a budget type using the budget type ID
        /// </summary>
        /// <param name="context"></param>
        /// <param name="budgetTypeID"></param>
        /// <returns></returns>
        public static string BudgetVersionDescriptionByTypeID(Entities context, long budgetTypeID)
        {
            string _data = context.OVERHEAD_BUDGET_TYPE.Where(x => x.OVERHEAD_BUDGET_TYPE_ID == budgetTypeID).SingleOrDefault().BUDGET_DESCRIPTION;
            return _data;
        }

        /// <summary>
        /// Returns 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="organizationID"></param>
        /// <returns></returns>
        public static string AccountRangeByOrganizationID(Entities context, long organizationID)
        {
            List<string> _accountRange = new List<string>();
            var _data = context.OVERHEAD_GL_RANGE.Where(x => x.ORGANIZATION_ID == organizationID);

            foreach (OVERHEAD_GL_RANGE _range in _data)
            {
                if (!_accountRange.Contains(_range.SRSEGMENT1.ToString() + "." + _range.SRSEGMENT2.ToString() + "." + _range.SRSEGMENT3.ToString() + "." + _range.SRSEGMENT4.ToString()))
                    _accountRange.Add(_range.SRSEGMENT1.ToString() + "." + _range.SRSEGMENT2.ToString() + "." + _range.SRSEGMENT3.ToString() + "." + _range.SRSEGMENT4.ToString());
            }

            return string.Join(",", _accountRange.AsEnumerable<string>());

        }

        /// <summary>
        /// List of all budgets setup in the overhead system.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IQueryable<OVERHEAD_ORG_BUDGETS> BudgetOrganizations(Entities context)
        {
            var _data = context.OVERHEAD_ORG_BUDGETS;
            return _data;
        }

        /// <summary>
        /// Shows organizations in oracle and their overhead status based on the organization profile option
        /// </summary>
        /// <param name="hierarchyId"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public static List<BUDGET_VERSION> OrganizationsByHierarchy(Entities context, long hierarchyID = 0, long leOrganizationID = 0, Boolean withSecurity = false)
        {
            List<BUDGET_VERSION> _budgetOrgList = new List<BUDGET_VERSION>();
            IEnumerable<OVERHEAD_ORG_BUDGETS> _budgetlist = BudgetOrganizations(context).ToList();

            //Allow user to view all organizations by that BU. Security Override View
            if (withSecurity)
            {
                _budgetlist = (from b in _budgetlist
                                   where HR.OverheadOrganizationStatusByHierarchy(hierarchyID, leOrganizationID).Any(x => x.ORGANIZATION_ID == b.ORGANIZATION_ID)
                                   select b);
            }
            else
            {
                _budgetlist = (from b in _budgetlist
                                   where SYS_USER_ORGS.GetUserOrgs(SYS_USER_INFORMATION.LoggedInUser().USER_ID).Any(x => x.ORG_ID == b.ORGANIZATION_ID)
                                   select b);
            }

            foreach (OVERHEAD_ORG_BUDGETS _budget in _budgetlist)
            {
                //Get Organization Information
                HR.ORGANIZATION _orgInformation = HR.Organization(_budget.ORGANIZATION_ID);

                    BUDGET_VERSION _data = new BUDGET_VERSION();
                    _data.ORGANIZATION_ID = _orgInformation.ORGANIZATION_ID;
                    _data.ORGANIZATION_NAME = _orgInformation.ORGANIZATION_NAME;
                    _data.BUDGET_STATUS = (_budget.STATUS == "O") ? "Open" : (_budget.STATUS == "C") ? "Closed" : (_budget.STATUS == "P") ? "Pending" : "Locked";
                    _data.FISCAL_YEAR = _budget.FISCAL_YEAR;
                    _data.BUDGET_DESCRIPTION = BudgetVersionDescriptionByTypeID(context, _budget.OVERHEAD_BUDGET_TYPE_ID);
                    _data.ACCOUNT_RANGE = AccountRangeByOrganizationID(context, _data.ORGANIZATION_ID);
                    _data.ORG_BUDGET_ID = _budget.ORG_BUDGET_ID;
                    _budgetOrgList.Add(_data);
            }

            return _budgetOrgList;
        }

        public class BUDGET_VERSION
        {
            public long ORGANIZATION_ID { get; set; }
            public string ORGANIZATION_NAME { get; set; }
            public string BUDGET_STATUS { get; set; }
            public string BUDGET_DESCRIPTION { get; set; }
            public string ACCOUNT_RANGE { get; set; }
            public short FISCAL_YEAR { get; set; }
            public long ORG_BUDGET_ID { get; set; }
        }



        #endregion

        #region Edit Budget Version

        /// <summary>
        /// Return Budget information by Budget ID
        /// </summary>
        /// <param name="context"></param>
        /// <param name="budgetID"></param>
        /// <returns></returns>
        public static OVERHEAD_ORG_BUDGETS BudgetByID(Entities context, long budgetID)
        {
            var _data = context.OVERHEAD_ORG_BUDGETS.Where(x => x.ORG_BUDGET_ID == budgetID).SingleOrDefault();
            return _data;
        }

        /// <summary>
        /// Returns all the valid GL Periods for the DBI Calendar
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static List<GL_PERIOD> GeneralLedgerPeriods(Entities context)
        {
            string sql = "select ENTERED_PERIOD_NAME,PERIOD_YEAR,PERIOD_NUM,PERIOD_TYPE,START_DATE,END_DATE from gl.gl_periods where period_set_name = 'DBI Calendar' order by period_num";
            var _data = context.Database.SqlQuery<GL_PERIOD>(sql).ToList();
            return _data;
        }

        /// <summary>
        /// Range of GL accounts by organization Id
        /// </summary>
        /// <param name="context"></param>
        /// <param name="organizationID"></param>
        /// <returns></returns>
        public static IQueryable<OVERHEAD_GL_RANGE> RangeOfAccountsByOrganizationID(Entities context, long organizationID)
        {
            var _data = context.OVERHEAD_GL_RANGE.Where(x => x.ORGANIZATION_ID == organizationID);
            return _data;
        }

        /// <summary>
        /// Returns all accounts for all ranges by organization id
        /// </summary>
        /// <param name="context"></param>
        /// <param name="organizationID"></param>
        /// <returns></returns>
        public static List<GL_ACCOUNTS_V> AccountListByOrganizationID(Entities context, long organizationID)
        {
            var _rangeList = RangeOfAccountsByOrganizationID(context, organizationID).ToList();
            List<GL_ACCOUNTS_V> _accountList = new List<GL_ACCOUNTS_V>();

            foreach (OVERHEAD_GL_RANGE _range in _rangeList)
            {
                var _adata = context.GL_ACCOUNTS_V.Where(x => String.Compare(x.SEGMENT1, _range.SRSEGMENT1) >= 0 && String.Compare(x.SEGMENT1, _range.ERSEGMENT1) <= 0);
                _adata = _adata.Where(x => String.Compare(x.SEGMENT2, _range.SRSEGMENT2) >= 0 && String.Compare(x.SEGMENT2, _range.ERSEGMENT2) <= 0);
                _adata = _adata.Where(x => String.Compare(x.SEGMENT3, _range.SRSEGMENT3) >= 0 && String.Compare(x.SEGMENT3, _range.ERSEGMENT3) <= 0);
                _adata = _adata.Where(x => String.Compare(x.SEGMENT4, _range.SRSEGMENT4) >= 0 && String.Compare(x.SEGMENT4, _range.ERSEGMENT4) <= 0);
                _adata = _adata.Where(x => String.Compare(x.SEGMENT5, _range.SRSEGMENT5) >= 0 && String.Compare(x.SEGMENT5, _range.ERSEGMENT5) <= 0);
                _adata = _adata.Where(x => String.Compare(x.SEGMENT6, _range.SRSEGMENT6) >= 0 && String.Compare(x.SEGMENT6, _range.ERSEGMENT6) <= 0);
                _adata = _adata.Where(x => String.Compare(x.SEGMENT7, _range.SRSEGMENT7) >= 0 && String.Compare(x.SEGMENT7, _range.ERSEGMENT7) <= 0);
                List<GL_ACCOUNTS_V> _accountRange = _adata.ToList();
                _accountList.AddRange(_accountRange);
            }

            return _accountList;
        }

        /// <summary>
        /// Returns a list of all VALID accounts in all ranges by organization id
        /// </summary>
        /// <param name="context"></param>
        /// <param name="organizationID"></param>
        /// <param name="excludedAccounts"></param>
        /// <returns></returns>
        public static List<GL_ACCOUNTS_V> AccountListValidByOrganizationID(Entities context, long organizationID)
        {
            List<GL_ACCOUNTS_V> _data = AccountListByOrganizationID(context, organizationID);
            _data.Where(x => !AccountListExcludedByOrganizationID(context, organizationID).Any(y => y.CODE_COMBINATION_ID == x.CODE_COMBINATION_ID));
            return _data;
        }

        /// <summary>
        /// Returns a list of excluded accounts by organization.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="organizationID"></param>
        /// <returns></returns>
        public static List<OVERHEAD_GL_ACCOUNT> AccountListExcludedByOrganizationID(Entities context, long organizationID)
        {
            var _data = context.OVERHEAD_GL_ACCOUNT.Where(x => x.ORGANIZATION_ID == organizationID).ToList();
            return _data;
        }

        /// <summary>
        /// Return budget detail by budget id
        /// </summary>
        /// <param name="context"></param>
        /// <param name="budgetID"></param>
        /// <returns></returns>
        public static IQueryable<OVERHEAD_BUDGET_DETAIL> BudgetDetailByBudgetID(Entities context, long budgetID)
        {
            var _data = context.OVERHEAD_BUDGET_DETAIL.Where(x => x.ORG_BUDGET_ID == budgetID);
            return _data;
        }


        /// <summary>
        /// Returns the budget detail by organization id and budget id
        /// </summary>
        /// <param name="context"></param>
        /// <param name="budgetID"></param>
        /// <param name="organizationID"></param>
        /// <returns></returns>
        public static List<OVERHEAD_BUDGET_VIEW> BudgetDetailsViewByBudgetID(Entities context, long budgetID, long organizationID, Boolean hideBlankLines = false)
        {
            var _budgetDetail = BudgetDetailByBudgetID(context, budgetID);
            var _validAccounts = AccountListValidByOrganizationID(context, organizationID);
            var _categoryList = CategoryAsQueryable(context);
            var _accountCategoryList = AccountAsQueryable(context);

            List<OVERHEAD_BUDGET_VIEW> _data = new List<OVERHEAD_BUDGET_VIEW>();

            foreach (GL_ACCOUNTS_V _account in _validAccounts)
            {
                OVERHEAD_CATEGORY _category = new OVERHEAD_CATEGORY();
                OVERHEAD_BUDGET_VIEW _record = new OVERHEAD_BUDGET_VIEW();
                var _accountCategory = _accountCategoryList.Where(x => x.ACCOUNT_SEGMENT == _account.SEGMENT5).OrderBy(x => x.ACCOUNT_SEGMENT).SingleOrDefault();

                if (_accountCategory != null)
                {
                    _category = _categoryList.Where(x => x.CATEGORY_ID == _accountCategory.CATEGORY_ID).SingleOrDefault();
                    _record.CATEGORY_ID = _accountCategory.CATEGORY_ID;
                    _record.CATEGORY_NAME = _category.NAME;
                    _record.SORT_ORDER = _accountCategory.SORT_ORDER;
                    _record.CATEGORY_SORT_ORDER = (long)_category.SORT_ORDER;
                }
                else
                {
                    _record.CATEGORY_NAME = "Other";
                    _record.CATEGORY_SORT_ORDER = 99999;
                    _record.SORT_ORDER = 0;

                }


                //Return the data for the year
                List<OVERHEAD_BUDGET_DETAIL> _condensedBudgetDetail = _budgetDetail.Where(x => x.CODE_COMBINATION_ID == _account.CODE_COMBINATION_ID).ToList();


                _record.ACCOUNT_SEGMENT = _account.SEGMENT5;
                _record.CODE_COMBINATION_ID = _account.CODE_COMBINATION_ID;
                _record.ACCOUNT_DESCRIPTION = _account.SEGMENT5_DESC + " (" + _account.SEGMENT5 + ")";
                _record.AMOUNT1 = GetAccountTotalByPeriod(_condensedBudgetDetail, 1);
                _record.AMOUNT2 = GetAccountTotalByPeriod(_condensedBudgetDetail, 2);
                _record.AMOUNT3 = GetAccountTotalByPeriod(_condensedBudgetDetail, 3);
                _record.AMOUNT4 = GetAccountTotalByPeriod(_condensedBudgetDetail, 4);
                _record.AMOUNT5 = GetAccountTotalByPeriod(_condensedBudgetDetail, 5);
                _record.AMOUNT6 = GetAccountTotalByPeriod(_condensedBudgetDetail, 6);
                _record.AMOUNT7 = GetAccountTotalByPeriod(_condensedBudgetDetail, 7);
                _record.AMOUNT8 = GetAccountTotalByPeriod(_condensedBudgetDetail, 8);
                _record.AMOUNT9 = GetAccountTotalByPeriod(_condensedBudgetDetail, 9);
                _record.AMOUNT10 = GetAccountTotalByPeriod(_condensedBudgetDetail, 10);
                _record.AMOUNT11 = GetAccountTotalByPeriod(_condensedBudgetDetail, 11);
                _record.AMOUNT12 = GetAccountTotalByPeriod(_condensedBudgetDetail, 12);
                _record.TOTAL = (_record.AMOUNT1 + _record.AMOUNT2 + _record.AMOUNT3 + _record.AMOUNT4 + _record.AMOUNT5 + _record.AMOUNT6 + _record.AMOUNT7 + _record.AMOUNT8 + _record.AMOUNT9 + _record.AMOUNT10 + _record.AMOUNT11 + _record.AMOUNT12);

                if (hideBlankLines)
                {
                    if (_record.TOTAL > 0 || _record.TOTAL < 0)
                        _data.Add(_record);
                }
                else
                {
                    _data.Add(_record);
                }


                
            }

            return _data;
        }

        /// <summary>
        /// Returns the total needed for the details view
        /// </summary>
        /// <param name="budgetList"></param>
        /// <param name="period_num"></param>
        /// <returns></returns>
        private static decimal GetAccountTotalByPeriod(List<OVERHEAD_BUDGET_DETAIL> budgetList, long period_num)
        {
            decimal returnvalue = 0;

            OVERHEAD_BUDGET_DETAIL _line = budgetList.Where(x => x.PERIOD_NUM == period_num).SingleOrDefault();
            if (_line != null)
            {
                returnvalue = (decimal)_line.AMOUNT;
            }
            return returnvalue;
        }


        public class OVERHEAD_BUDGET_VIEW
        {
            public long CATEGORY_ID { get; set; }
            public long CATEGORY_SORT_ORDER { get; set; }
            public long? SORT_ORDER { get; set; }
            public string ACCOUNT_SEGMENT { get; set; }
            public string CATEGORY_NAME { get; set; }
            public long CODE_COMBINATION_ID { get; set; }
            public string ACCOUNT_DESCRIPTION { get; set; }
            public decimal TOTAL { get; set; }
            public decimal AMOUNT1 { get; set; }
            public decimal AMOUNT2 { get; set; }
            public decimal AMOUNT3 { get; set; }
            public decimal AMOUNT4 { get; set; }
            public decimal AMOUNT5 { get; set; }
            public decimal AMOUNT6 { get; set; }
            public decimal AMOUNT7 { get; set; }
            public decimal AMOUNT8 { get; set; }
            public decimal AMOUNT9 { get; set; }
            public decimal AMOUNT10 { get; set; }
            public decimal AMOUNT11 { get; set; }
            public decimal AMOUNT12 { get; set; }
        }


        public class GL_ACCOUNT
        {
            public long CODE_COMBINATION_ID { get; set; }    
            public string SEGMENT1 { get; set; }
            public string SEGMENT1_DESC { get; set; }
            public string SEGMENT2 { get; set; }
            public string SEGMENT2_DESC { get; set; }
            public string SEGMENT3 { get; set; }
            public string SEGMENT3_DESC { get; set; }
            public string SEGMENT4 { get; set; }
            public string SEGMENT4_DESC { get; set; }
            public string SEGMENT5 { get; set; }
            public string SEGMENT5_DESC { get; set; }
            public string SEGMENT6 { get; set; }
            public string SEGMENT6_DESC { get; set; }
            public string SEGMENT7 { get; set; }
            public string SEGMENT7_DESC { get; set; }
        }

        public class GL_PERIOD
        {
            public string ENTERED_PERIOD_NAME { get; set; }
            public short PERIOD_YEAR { get; set; }
            public long PERIOD_NUM { get; set; }
            public string PERIOD_TYPE { get; set; }
            public DateTime START_DATE { get; set; }
            public DateTime END_DATE { get; set; }
        }

        #endregion

        #region Import Actuals Window


        /// <summary>
        /// Returns actual balance from gl system by code combination id and period year. 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="periodYear"></param>
        /// <param name="codeCombinationID"></param>
        /// <returns></returns>
        public static List<GL_ACTUALS> ActualsByYearAndAccountCodeAndPeriodNumber(Entities context, short periodYear, long codeCombinationID, long periodNum)
        {
            string sql = string.Format("select period_net_dr, period_net_cr,period_year,code_combination_id,period_num from gl.gl_balances where actual_flag = 'A' and period_year = {0} and code_combination_id = {1} and period_num = {2} and set_of_books_id in (select set_of_books_id from apps.hr_operating_units group by set_of_books_id)", periodYear, codeCombinationID, periodNum);
            List<GL_ACTUALS> _data = context.Database.SqlQuery<GL_ACTUALS>(sql).ToList();
            return _data;
        }

        public static Boolean ImportActualForBudgetVersion(Entities context, List<string> periodsToImport, long budgetid, string loggedInUser, string lockImportData = "N")
        {
            OVERHEAD_ORG_BUDGETS _budgetHeader = BudgetByID(context, budgetid);
            List<OVERHEAD_BUDGET_FORECAST.GL_PERIOD> _periodList = OVERHEAD_BUDGET_FORECAST.GeneralLedgerPeriods(context).Where(x => x.PERIOD_YEAR == _budgetHeader.FISCAL_YEAR & x.PERIOD_TYPE == "Month" & periodsToImport.Contains(x.PERIOD_NUM.ToString())).ToList();
            var _accountList = OVERHEAD_BUDGET_FORECAST.AccountListValidByOrganizationID(context, _budgetHeader.ORGANIZATION_ID);
            List<OVERHEAD_BUDGET_DETAIL> _insertData = new List<OVERHEAD_BUDGET_DETAIL>();
            List<OVERHEAD_BUDGET_DETAIL> _updateData = new List<OVERHEAD_BUDGET_DETAIL>();

            foreach (var _account in _accountList)
            {
                 foreach (OVERHEAD_BUDGET_FORECAST.GL_PERIOD _period in _periodList)
                        {
                          OVERHEAD_BUDGET_DETAIL _line = OVERHEAD_BUDGET_FORECAST.BudgetDetailByBudgetID(context,budgetid).Where(x => x.CODE_COMBINATION_ID == _account.CODE_COMBINATION_ID & x.PERIOD_NUM == _period.PERIOD_NUM).SingleOrDefault();
                          GL_ACTUALS _actualTotalLine = OVERHEAD_BUDGET_FORECAST.ActualsByYearAndAccountCodeAndPeriodNumber(context, _budgetHeader.FISCAL_YEAR, _account.CODE_COMBINATION_ID, _period.PERIOD_NUM).SingleOrDefault();
                            
                            decimal _aTotal = 0;

                            if (_actualTotalLine != null)
                            {
                                _aTotal = _actualTotalLine.PERIOD_NET_DR + Decimal.Negate(_actualTotalLine.PERIOD_NET_CR);
                            }
                            else
                            {
                                _aTotal = 0;
                            }

                                    if (_line == null & _aTotal > 0)
                                    {
                                        //No data, create it
                                        OVERHEAD_BUDGET_DETAIL _record = new OVERHEAD_BUDGET_DETAIL();
                                        _record.ORG_BUDGET_ID = budgetid;
                                        _record.PERIOD_NAME = _period.ENTERED_PERIOD_NAME;
                                        _record.PERIOD_NUM = _period.PERIOD_NUM;
                                        _record.CODE_COMBINATION_ID = _account.CODE_COMBINATION_ID;
                                        _record.AMOUNT = _aTotal;
                                        _record.CREATE_DATE = DateTime.Now;
                                        _record.MODIFY_DATE = DateTime.Now;
                                        _record.CREATED_BY = loggedInUser;
                                        _record.MODIFIED_BY = loggedInUser;
                                        _record.ACTUALS_IMPORTED_FLAG = lockImportData;
                                        _insertData.Add(_record);
                                    }
                                    else if(_line != null)
                                    {
                                        //Data update it
                                        _line.AMOUNT = _aTotal;
                                        _line.MODIFY_DATE = DateTime.Now;
                                        _line.MODIFIED_BY = loggedInUser;
                                        _line.ACTUALS_IMPORTED_FLAG = lockImportData;
                                        _updateData.Add(_line);
                                    }
                    }
                }

            GenericData.Insert<OVERHEAD_BUDGET_DETAIL>(_insertData);
            GenericData.Update<OVERHEAD_BUDGET_DETAIL>(_updateData);

            return true;

        }

        public class GL_ACTUALS
        {
            public decimal PERIOD_NET_DR { get; set; }
            public decimal PERIOD_NET_CR { get; set; }
            public short PERIOD_YEAR { get; set; }
            public long CODE_COMBINATION_ID { get; set; }
            public long PERIOD_NUM { get; set; }
        }

        #endregion

    }
}

