﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;

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
            var _data = AccountCategoryAsQueryable(context).Where(x => x.ACCOUNT_CATEGORY_ID == ID).SingleOrDefault();
            return _data;
        }

        /// <summary>
        /// Returns a list of account categories as queryable.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static IQueryable<OVERHEAD_ACCOUNT_CATEGORY> AccountCategoryAsQueryable(Entities context)
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
            string _sql = string.Format("select XXEMS.GET_GL_ACCOUNT_DESCRIPTION (50328,{0},'{1}') from dual", segmentPosition, segmentCode);
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
        public static IQueryable<GL_PERIOD> GeneralLedgerPeriods(Entities context)
        {
            string sql = "select ENTERED_PERIOD_NAME,PERIOD_YEAR,PERIOD_NUM,PERIOD_TYPE,START_DATE,END_DATE from gl.gl_periods where period_set_name = 'DBI Calendar' order by period_num";
            return context.Database.SqlQuery<GL_PERIOD>(sql).AsQueryable();
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
        public static List<OVERHEAD_BUDGET_VIEW> BudgetDetailsViewByBudgetID(Entities context, long budgetID, long organizationID, Boolean printView = false, Boolean hideBlankLines = false)
        {
            var _budgetDetail = BudgetDetailByBudgetID(context, budgetID);
            var _validAccounts = AccountListValidByOrganizationID(context, organizationID);
            var _categoryList = CategoryAsQueryable(context);
            var _accountCategoryList = AccountCategoryAsQueryable(context);

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

                if (printView)
                {
                    _record.ACCOUNT_DESCRIPTION = _account.SEGMENT5_DESC;
                    _record.ACCOUNT_DESCRIPTION2 = "(" + _account.SEGMENT5 + ")";
                }
                else
                {
                    _record.ACCOUNT_DESCRIPTION = _account.SEGMENT5_DESC + " (" + _account.SEGMENT4 + "." + _account.SEGMENT5 + ")";
                }
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
                _record.BUDGET_ID = budgetID;

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

            if (printView)
            {
                var _orderedPrint = _data.OrderBy(x => x.CATEGORY_SORT_ORDER).ThenBy(x => x.SORT_ORDER).ThenBy(x => x.ACCOUNT_SEGMENT);
                return _orderedPrint.ToList();
            }

                return _data;

        }

        public static OVERHEAD_BUDGET_VIEW SummaryViewByCategoryID(IEnumerable<OVERHEAD_BUDGET_VIEW> reportList, long categoryID)
        {

                //Cacluate summary
                var _summaryData = reportList.Where(x => x.CATEGORY_ID == categoryID).GroupBy(I => I.CATEGORY_ID)
                    .Select(g => new OVERHEAD_BUDGET_VIEW
                    {
                        CATEGORY_ID = g.Key,
                        AMOUNT1 = g.Sum(i => i.AMOUNT1),
                        AMOUNT2 = g.Sum(i => i.AMOUNT2),
                        AMOUNT3 = g.Sum(i => i.AMOUNT3),
                        AMOUNT4 = g.Sum(i => i.AMOUNT4),
                        AMOUNT5 = g.Sum(i => i.AMOUNT5),
                        AMOUNT6 = g.Sum(i => i.AMOUNT6),
                        AMOUNT7 = g.Sum(i => i.AMOUNT7),
                        AMOUNT8 = g.Sum(i => i.AMOUNT8),
                        AMOUNT9 = g.Sum(i => i.AMOUNT9),
                        AMOUNT10 = g.Sum(i => i.AMOUNT10),
                        AMOUNT11 = g.Sum(i => i.AMOUNT11),
                        AMOUNT12 = g.Sum(i => i.AMOUNT12),
                        TOTAL = g.Sum(i => i.TOTAL),
                        CATEGORY_NAME = g.Max(i => i.CATEGORY_NAME),
                    }).Where(x => x.CATEGORY_ID == categoryID).Single();

                return _summaryData;
        }

        public static OVERHEAD_BUDGET_VIEW SummaryViewByBudgetID(IEnumerable<OVERHEAD_BUDGET_VIEW> reportList, long budgetID)
        {

            //Cacluate summary
            var _summaryData = reportList.Where(x => x.BUDGET_ID == budgetID).GroupBy(I => I.BUDGET_ID)
                .Select(g => new OVERHEAD_BUDGET_VIEW
                {
                    BUDGET_ID = g.Key,
                    AMOUNT1 = g.Sum(i => i.AMOUNT1),
                    AMOUNT2 = g.Sum(i => i.AMOUNT2),
                    AMOUNT3 = g.Sum(i => i.AMOUNT3),
                    AMOUNT4 = g.Sum(i => i.AMOUNT4),
                    AMOUNT5 = g.Sum(i => i.AMOUNT5),
                    AMOUNT6 = g.Sum(i => i.AMOUNT6),
                    AMOUNT7 = g.Sum(i => i.AMOUNT7),
                    AMOUNT8 = g.Sum(i => i.AMOUNT8),
                    AMOUNT9 = g.Sum(i => i.AMOUNT9),
                    AMOUNT10 = g.Sum(i => i.AMOUNT10),
                    AMOUNT11 = g.Sum(i => i.AMOUNT11),
                    AMOUNT12 = g.Sum(i => i.AMOUNT12),
                    TOTAL = g.Sum(i => i.TOTAL)
                }).Single();

            return _summaryData;
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
            public long BUDGET_ID { get; set; }
            public long CATEGORY_ID { get; set; }
            public long CATEGORY_SORT_ORDER { get; set; }
            public long? SORT_ORDER { get; set; }
            public string ACCOUNT_SEGMENT { get; set; }
            public string CATEGORY_NAME { get; set; }
            public long CODE_COMBINATION_ID { get; set; }
            public string ACCOUNT_DESCRIPTION { get; set; }
            public string ACCOUNT_DESCRIPTION2 { get; set; }
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
        public static IQueryable<GL_ACTUALS> ActualsByYearAndAccountCodeAndPeriodNumber(Entities context, short periodYear, long codeCombinationID, long periodNum)
        {
            string sql = string.Format("select period_net_dr, period_net_cr,period_year,code_combination_id,period_num from gl.gl_balances where actual_flag = 'A' and period_year = {0} and code_combination_id = {1} and period_num = {2} and (Period_net_dr <> 0 or period_net_cr <> 0) and set_of_books_id in (select set_of_books_id from apps.hr_operating_units group by set_of_books_id)", periodYear, codeCombinationID, periodNum);
            IQueryable<GL_ACTUALS> _data = context.Database.SqlQuery<GL_ACTUALS>(sql).AsQueryable();
            return _data;
        }

        public static Boolean ImportActualForBudgetVersion(Entities context, List<string> periodsToImport, long budgetid, string loggedInUser, string lockImportData = "N")
        {
            OVERHEAD_ORG_BUDGETS _budgetHeader = BudgetByID(context, budgetid);
            IQueryable<OVERHEAD_BUDGET_DETAIL> _budgetDetail = OVERHEAD_BUDGET_FORECAST.BudgetDetailByBudgetID(context, budgetid);
            List<OVERHEAD_BUDGET_FORECAST.GL_PERIOD> _periodList = OVERHEAD_BUDGET_FORECAST.GeneralLedgerPeriods(context).Where(x => x.PERIOD_YEAR == _budgetHeader.FISCAL_YEAR & x.PERIOD_TYPE == "Month" & periodsToImport.Contains(x.PERIOD_NUM.ToString())).ToList();
            var _accountList = OVERHEAD_BUDGET_FORECAST.AccountListValidByOrganizationID(context, _budgetHeader.ORGANIZATION_ID);
            List<OVERHEAD_BUDGET_DETAIL> _insertData = new List<OVERHEAD_BUDGET_DETAIL>();
            List<OVERHEAD_BUDGET_DETAIL> _updateData = new List<OVERHEAD_BUDGET_DETAIL>();

            foreach (var _account in _accountList)
            {
                 foreach (OVERHEAD_BUDGET_FORECAST.GL_PERIOD _period in _periodList)
                        {
                          OVERHEAD_BUDGET_DETAIL _line = _budgetDetail.Where(x => x.CODE_COMBINATION_ID == _account.CODE_COMBINATION_ID & x.PERIOD_NUM == _period.PERIOD_NUM).SingleOrDefault();
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

                                    if (_line == null)
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

        #region Add Account Range Window

        public static IQueryable<GL_ACCOUNT_LIST> GeneralLedgerAccounts(Entities context)
        {
            string sql = "SELECT code_combination_id, segment1, segment2, segment3, segment4, segment5, segment6, segment7 FROM apps.gl_code_combinations gccl";
            IQueryable<GL_ACCOUNT_LIST> _data = context.Database.SqlQuery<GL_ACCOUNT_LIST>(sql).AsQueryable();
            return _data;
        }

            
        public class GL_ACCOUNT_LIST
        {
            public long CODE_COMBINATION_ID { get; set; }
            public string SEGMENT1 { get; set; }
            public string SEGMENT2 { get; set; }
            public string SEGMENT3 { get; set; }
            public string SEGMENT4 { get; set; }
            public string SEGMENT5 { get; set; }
            public string SEGMENT6 { get; set; }
            public string SEGMENT7 { get; set; }
        }

        #endregion

        #region Misc functions

        public static MemoryStream GenerateReport(Entities context, long organizationID, short fiscalYear, long budgetID, string description, PRINT_OPTIONS printOptions)
        {

            using (MemoryStream _pdfMemoryStream = new MemoryStream())
            {
                //Create the document
                Document _document = new Document(new Rectangle(288f, 144f), 10, 10, 10, 10);
                _document.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
                PdfWriter ExportWriter = PdfWriter.GetInstance(_document, _pdfMemoryStream);
               
                Paragraph NewLine = new Paragraph("\n");
                Font HeaderFont = FontFactory.GetFont("Verdana", 9);
                Font HeadFootTitleFont = FontFactory.GetFont("Verdana", 9);
                Font HeadFootCellFont = FontFactory.GetFont("Verdana", 9);
                Font CellFont = FontFactory.GetFont("Verdana", 8);
                Font TotalCellFont = FontFactory.GetFont("Verdana", 8, Font.BOLD);

                //Open Document
                _document.Open();

                HeaderFooter _footer = new HeaderFooter(new Phrase(""),true);
                _document.Footer = _footer;

                //Header Table with Columns
                PdfPTable _headerPdfTable = new PdfPTable(14);
                _headerPdfTable.WidthPercentage = 100;
                int[] intTblWidth = { 25, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6, 6 };
                _headerPdfTable.SetWidths(intTblWidth);

                PdfPCell[] Cells;
                PdfPRow Row;
                _headerPdfTable.HeaderRows = 2;

                List<string> _title = description.Split('/').ToList<string>();

                var _glMonthPeriods = OVERHEAD_BUDGET_FORECAST.GeneralLedgerPeriods(context).Where(x => x.PERIOD_YEAR == fiscalYear & x.PERIOD_TYPE == "Month");
                var _glWeekPeriods = OVERHEAD_BUDGET_FORECAST.GeneralLedgerPeriods(context).Where(x => x.PERIOD_YEAR == fiscalYear & x.PERIOD_TYPE == "Week");

                 Cells = new PdfPCell[]{
                     new PdfPCell(new Phrase("Overhead Budget / " + _title[0], TotalCellFont )),
                     new PdfPCell(new Phrase(string.Format("{0}", _glMonthPeriods.Where(x => x.PERIOD_NUM == 1).Single().ENTERED_PERIOD_NAME),HeadFootTitleFont)),
                     new PdfPCell(new Phrase(string.Format("{0}", _glMonthPeriods.Where(x => x.PERIOD_NUM == 2).Single().ENTERED_PERIOD_NAME),HeadFootTitleFont)),
                     new PdfPCell(new Phrase(string.Format("{0}", _glMonthPeriods.Where(x => x.PERIOD_NUM == 3).Single().ENTERED_PERIOD_NAME),HeadFootTitleFont)),
                     new PdfPCell(new Phrase(string.Format("{0}", _glMonthPeriods.Where(x => x.PERIOD_NUM == 4).Single().ENTERED_PERIOD_NAME),HeadFootTitleFont)),
                     new PdfPCell(new Phrase(string.Format("{0}", _glMonthPeriods.Where(x => x.PERIOD_NUM == 5).Single().ENTERED_PERIOD_NAME),HeadFootTitleFont)),
                     new PdfPCell(new Phrase(string.Format("{0}", _glMonthPeriods.Where(x => x.PERIOD_NUM == 6).Single().ENTERED_PERIOD_NAME),HeadFootTitleFont)),
                     new PdfPCell(new Phrase(string.Format("{0}", _glMonthPeriods.Where(x => x.PERIOD_NUM == 7).Single().ENTERED_PERIOD_NAME),HeadFootTitleFont)),
                     new PdfPCell(new Phrase(string.Format("{0}", _glMonthPeriods.Where(x => x.PERIOD_NUM == 8).Single().ENTERED_PERIOD_NAME),HeadFootTitleFont)),
                     new PdfPCell(new Phrase(string.Format("{0}", _glMonthPeriods.Where(x => x.PERIOD_NUM == 9).Single().ENTERED_PERIOD_NAME),HeadFootTitleFont)),
                     new PdfPCell(new Phrase(string.Format("{0}", _glMonthPeriods.Where(x => x.PERIOD_NUM == 10).Single().ENTERED_PERIOD_NAME),HeadFootTitleFont)),
                     new PdfPCell(new Phrase(string.Format("{0}", _glMonthPeriods.Where(x => x.PERIOD_NUM == 11).Single().ENTERED_PERIOD_NAME),HeadFootTitleFont)),
                     new PdfPCell(new Phrase(string.Format("{0}", _glMonthPeriods.Where(x => x.PERIOD_NUM == 12).Single().ENTERED_PERIOD_NAME),HeadFootTitleFont)),
                     new PdfPCell(new Phrase("Total", HeadFootTitleFont)),
                 };

                 foreach (PdfPCell _cell in Cells)
                 {
                     _cell.BackgroundColor = new Color(230, 230, 230);
                     _cell.HorizontalAlignment = PdfCell.ALIGN_CENTER;
                 }

                Row = new PdfPRow(Cells);
                _headerPdfTable.Rows.Add(Row);

                Cells = new PdfPCell[]{
                     new PdfPCell(new Phrase(_title[1] + " / " + _title[2], TotalCellFont )),
                     new PdfPCell(new Phrase(string.Format("{0} Weeks", _glWeekPeriods.Where(x => x.ENTERED_PERIOD_NAME.Contains(_glMonthPeriods.Where(y => y.PERIOD_NUM == 1).Single().ENTERED_PERIOD_NAME)).Count()),HeadFootTitleFont)),
                     new PdfPCell(new Phrase(string.Format("{0} Weeks", _glWeekPeriods.Where(x => x.ENTERED_PERIOD_NAME.Contains(_glMonthPeriods.Where(y => y.PERIOD_NUM == 2).Single().ENTERED_PERIOD_NAME)).Count()),HeadFootTitleFont)),
                     new PdfPCell(new Phrase(string.Format("{0} Weeks", _glWeekPeriods.Where(x => x.ENTERED_PERIOD_NAME.Contains(_glMonthPeriods.Where(y => y.PERIOD_NUM == 3).Single().ENTERED_PERIOD_NAME)).Count()),HeadFootTitleFont)),
                     new PdfPCell(new Phrase(string.Format("{0} Weeks", _glWeekPeriods.Where(x => x.ENTERED_PERIOD_NAME.Contains(_glMonthPeriods.Where(y => y.PERIOD_NUM == 4).Single().ENTERED_PERIOD_NAME)).Count()),HeadFootTitleFont)),
                     new PdfPCell(new Phrase(string.Format("{0} Weeks", _glWeekPeriods.Where(x => x.ENTERED_PERIOD_NAME.Contains(_glMonthPeriods.Where(y => y.PERIOD_NUM == 5).Single().ENTERED_PERIOD_NAME)).Count()),HeadFootTitleFont)),
                     new PdfPCell(new Phrase(string.Format("{0} Weeks", _glWeekPeriods.Where(x => x.ENTERED_PERIOD_NAME.Contains(_glMonthPeriods.Where(y => y.PERIOD_NUM == 6).Single().ENTERED_PERIOD_NAME)).Count()),HeadFootTitleFont)),
                     new PdfPCell(new Phrase(string.Format("{0} Weeks", _glWeekPeriods.Where(x => x.ENTERED_PERIOD_NAME.Contains(_glMonthPeriods.Where(y => y.PERIOD_NUM == 7).Single().ENTERED_PERIOD_NAME)).Count()),HeadFootTitleFont)),
                     new PdfPCell(new Phrase(string.Format("{0} Weeks", _glWeekPeriods.Where(x => x.ENTERED_PERIOD_NAME.Contains(_glMonthPeriods.Where(y => y.PERIOD_NUM == 8).Single().ENTERED_PERIOD_NAME)).Count()),HeadFootTitleFont)),
                     new PdfPCell(new Phrase(string.Format("{0} Weeks", _glWeekPeriods.Where(x => x.ENTERED_PERIOD_NAME.Contains(_glMonthPeriods.Where(y => y.PERIOD_NUM == 9).Single().ENTERED_PERIOD_NAME)).Count()),HeadFootTitleFont)),
                     new PdfPCell(new Phrase(string.Format("{0} Weeks", _glWeekPeriods.Where(x => x.ENTERED_PERIOD_NAME.Contains(_glMonthPeriods.Where(y => y.PERIOD_NUM == 10).Single().ENTERED_PERIOD_NAME)).Count()),HeadFootTitleFont)),
                     new PdfPCell(new Phrase(string.Format("{0} Weeks", _glWeekPeriods.Where(x => x.ENTERED_PERIOD_NAME.Contains(_glMonthPeriods.Where(y => y.PERIOD_NUM == 11).Single().ENTERED_PERIOD_NAME)).Count()),HeadFootTitleFont)),
                     new PdfPCell(new Phrase(string.Format("{0} Weeks", _glWeekPeriods.Where(x => x.ENTERED_PERIOD_NAME.Contains(_glMonthPeriods.Where(y => y.PERIOD_NUM == 12).Single().ENTERED_PERIOD_NAME)).Count()),HeadFootTitleFont)),
                     new PdfPCell(new Phrase("", HeadFootTitleFont)),
                 };

                 foreach (PdfPCell _cell in Cells)
                 {
                     _cell.BackgroundColor = new Color(230, 230, 230);
                     _cell.HorizontalAlignment = PdfCell.ALIGN_CENTER;
                 }


                Row = new PdfPRow(Cells);
                _headerPdfTable.Rows.Add(Row);


                 

                //Details Row
                //Return budget detail information
                IEnumerable<OVERHEAD_BUDGET_VIEW> _budgetView = BudgetDetailsViewByBudgetID(context, budgetID, organizationID,true,printOptions.HIDE_BLANK_LINES);

                NumberFormatInfo nfi = CultureInfo.CurrentCulture.NumberFormat;
                nfi = (NumberFormatInfo)nfi.Clone();
                nfi.CurrencySymbol = "";

                long _lastCategoryID = 0;

                foreach (OVERHEAD_BUDGET_VIEW _row in _budgetView)
                {

                    if (_lastCategoryID != 0)
                    {
                        if (_lastCategoryID != _row.CATEGORY_ID)
                        {
                            //Add a total line
                            OVERHEAD_BUDGET_VIEW _summaryView = SummaryViewByCategoryID(_budgetView, _lastCategoryID);

                            Phrase _totalPhase = new Phrase();
                            _totalPhase.Add(new Chunk(_summaryView.CATEGORY_NAME + " - Total", TotalCellFont));
                            _totalPhase.Add(new Chunk("\n"));

                            

                            Cells = new PdfPCell[]{
                            new PdfPCell(_totalPhase),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryView.AMOUNT1) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryView.AMOUNT2) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryView.AMOUNT3) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryView.AMOUNT4) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryView.AMOUNT5) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryView.AMOUNT6) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryView.AMOUNT7) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryView.AMOUNT8) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryView.AMOUNT9) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryView.AMOUNT10) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryView.AMOUNT11) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryView.AMOUNT12) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryView.TOTAL), TotalCellFont))
                        };

                            int cellCount = 1;
                            foreach (PdfPCell _cell in Cells)
                            {
                                if (cellCount == 1)
                                {
                                    _cell.BackgroundColor = new Color(224, 224, 209);
                                }
                                else
                                {
                                    _cell.BackgroundColor = new Color(224, 224, 209);
                                    _cell.HorizontalAlignment = PdfCell.ALIGN_RIGHT;
                                }

                                cellCount = cellCount + 1;
                            }


                            Row = new PdfPRow(Cells);
                            _headerPdfTable.Rows.Add(Row);
                        }

                    }


                    Phrase _accountPhase = new Phrase();
                    _accountPhase.Add(new Chunk(_row.ACCOUNT_DESCRIPTION, CellFont));
                    _accountPhase.Add(new Chunk("\n"));
                    _accountPhase.Add(new Chunk("     " +_row.ACCOUNT_DESCRIPTION2, CellFont));

                    Cells = new PdfPCell[]{
                     new PdfPCell(_accountPhase),
                     new PdfPCell(new Phrase((_row.AMOUNT1.ToString() == "0")? "" : String.Format(nfi,"{0:C0}", _row.AMOUNT1) ,CellFont)),
                     new PdfPCell(new Phrase((_row.AMOUNT2.ToString() == "0") ? "" :String.Format(nfi,"{0:C0}", _row.AMOUNT2) ,CellFont)),
                     new PdfPCell(new Phrase((_row.AMOUNT3.ToString() == "0") ? "" :String.Format(nfi,"{0:C0}", _row.AMOUNT3) ,CellFont)),
                     new PdfPCell(new Phrase((_row.AMOUNT4.ToString() == "0") ? "" :String.Format(nfi,"{0:C0}", _row.AMOUNT4) ,CellFont)),
                     new PdfPCell(new Phrase((_row.AMOUNT5.ToString() == "0") ? "" :String.Format(nfi,"{0:C0}", _row.AMOUNT5) ,CellFont)),
                     new PdfPCell(new Phrase((_row.AMOUNT6.ToString() == "0") ? "" :String.Format(nfi,"{0:C0}", _row.AMOUNT6) ,CellFont)),
                     new PdfPCell(new Phrase((_row.AMOUNT7.ToString() == "0") ? "" :String.Format(nfi,"{0:C0}", _row.AMOUNT7) ,CellFont)),
                     new PdfPCell(new Phrase((_row.AMOUNT8.ToString() == "0") ? "" :String.Format(nfi,"{0:C0}", _row.AMOUNT8) ,CellFont)),
                     new PdfPCell(new Phrase((_row.AMOUNT9.ToString() == "0") ? "" :String.Format(nfi,"{0:C0}", _row.AMOUNT9) ,CellFont)),
                     new PdfPCell(new Phrase((_row.AMOUNT10.ToString() == "0") ? "" :String.Format(nfi,"{0:C0}", _row.AMOUNT10) ,CellFont)),
                     new PdfPCell(new Phrase((_row.AMOUNT11.ToString() == "0") ? "" :String.Format(nfi,"{0:C0}", _row.AMOUNT11) ,CellFont)),
                     new PdfPCell(new Phrase((_row.AMOUNT12.ToString() == "0") ? "" :String.Format(nfi,"{0:C0}", _row.AMOUNT12) ,CellFont)),
                     new PdfPCell(new Phrase((_row.TOTAL.ToString() == "0") ? "" : String.Format(nfi,"{0:C0}", _row.TOTAL), CellFont))
                 };

                    //Enable 1st column light gray
                    int rowcount = 1;
                    foreach (PdfPCell _cell in Cells)
                    {
                        if (rowcount == 1)
                        {
                            _cell.BackgroundColor = new Color(230, 230, 230);
                        }
                        else
                        {
                            _cell.HorizontalAlignment = PdfCell.ALIGN_RIGHT;
                        }

                        rowcount = rowcount + 1;
                    }

                    Row = new PdfPRow(Cells);
                    _headerPdfTable.Rows.Add(Row);

                    _lastCategoryID = _row.CATEGORY_ID;

                }

                Row = new PdfPRow(Cells);
                _headerPdfTable.Rows.Add(Row);


                //Add Other Row

                //Add a total line based on other
                OVERHEAD_BUDGET_VIEW _summaryViewOther = SummaryViewByCategoryID(_budgetView, _lastCategoryID);

                Phrase _totalPhaseOther = new Phrase();
                _totalPhaseOther.Add(new Chunk(_summaryViewOther.CATEGORY_NAME + " - Total", TotalCellFont));
                _totalPhaseOther.Add(new Chunk("\n"));

                Cells = new PdfPCell[]{
                            new PdfPCell(_totalPhaseOther),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryViewOther.AMOUNT1) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryViewOther.AMOUNT2) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryViewOther.AMOUNT3) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryViewOther.AMOUNT4) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryViewOther.AMOUNT5) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryViewOther.AMOUNT6) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryViewOther.AMOUNT7) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryViewOther.AMOUNT8) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryViewOther.AMOUNT9) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryViewOther.AMOUNT10) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryViewOther.AMOUNT11) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryViewOther.AMOUNT12) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryViewOther.TOTAL), TotalCellFont))
                        };

                int cellCount2 = 1;
                foreach (PdfPCell _cell in Cells)
                {
                    if (cellCount2 == 1)
                    {
                        _cell.BackgroundColor = new Color(224, 224, 209);
                    }
                    else
                    {
                        _cell.BackgroundColor = new Color(224, 224, 209);
                        _cell.HorizontalAlignment = PdfCell.ALIGN_RIGHT;
                    }

                    cellCount2 = cellCount2 + 1;
                }



                Row = new PdfPRow(Cells);
                _headerPdfTable.Rows.Add(Row);


                //Add a total line based on budget

                OVERHEAD_BUDGET_VIEW _summaryViewTotal = SummaryViewByBudgetID(_budgetView, budgetID);

                Phrase _totalPhaseFinal = new Phrase();
                _totalPhaseFinal.Add(new Chunk("Budget Total", FontFactory.GetFont("Verdana", 8, Font.BOLD)));
                _totalPhaseFinal.Add(new Chunk("\n"));

                Cells = new PdfPCell[]{
                            new PdfPCell(_totalPhaseFinal),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryViewTotal.AMOUNT1) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryViewTotal.AMOUNT2) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryViewTotal.AMOUNT3) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryViewTotal.AMOUNT4) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryViewTotal.AMOUNT5) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryViewTotal.AMOUNT6) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryViewTotal.AMOUNT7) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryViewTotal.AMOUNT8) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryViewTotal.AMOUNT9) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryViewTotal.AMOUNT10) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryViewTotal.AMOUNT11) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryViewTotal.AMOUNT12) ,TotalCellFont)),
                            new PdfPCell(new Phrase(String.Format(nfi,"{0:C0}", _summaryViewTotal.TOTAL), TotalCellFont))
                        };

                int cellCount3 = 1;
                foreach (PdfPCell _cell in Cells)
                {
                    if (cellCount3 == 1)
                    {
                        _cell.BackgroundColor = new Color(224, 224, 209);
                    }
                    else
                    {
                        _cell.BackgroundColor = new Color(224, 224, 209);
                        _cell.HorizontalAlignment = PdfCell.ALIGN_RIGHT;
                    }

                    cellCount3 = cellCount3 + 1;
                }



                Row = new PdfPRow(Cells);
                _headerPdfTable.Rows.Add(Row);



                _document.Add(_headerPdfTable);

                ExportWriter.CloseStream = false;
                _document.Close();
                return _pdfMemoryStream;
            }
       
        }


        public class PRINT_OPTIONS
        {
            public Boolean SHOW_ACCOUNT_NOTES {get; set;}
            public Boolean SHOW_BUDGET_NOTES {get; set;}
            public Boolean ROLLUP {get; set;}
            public Boolean HIDE_BLANK_LINES {get; set;}
        }

        
        #endregion
    }
}

