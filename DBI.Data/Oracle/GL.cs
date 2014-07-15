using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DBI.Data;

namespace DBI.Data

{
    /// <summary>
    /// Methods and fuctions over the GL_ACCOUNTS_V entity object
    /// </summary>
    public partial class GL_ACCOUNTS_V
    {

        /// <summary>
        /// Returns a list of filtered general ledger accounts.
        /// </summary>
        /// <param name="segment1"></param>
        /// <param name="segment2"></param>
        /// <param name="segment3"></param>
        /// <param name="segment4"></param>
        /// <returns></returns>
        public static List<GL_ACCOUNTS_V> AccountsFiltered(string segment1, string segment2, string segment3, string segment4)
        {
                using (Entities _context = new Entities())
                {
                    var _data = _context.GL_ACCOUNTS_V.AsNoTracking().Where(a => a.SEGMENT1 == segment1);
                    _data = _data.Where(a => a.SEGMENT2 == segment2);
                    _data = _data.Where(a => a.SEGMENT3 == segment3);
                    _data = _data.Where(a => a.SEGMENT4 == segment4);

                    return _data.ToList();
                }

        }


        public static GL_ACCOUNTS_V AccountInformation(long code_combination_id)
        {
            using (Entities _context = new Entities())
            {
                var _data = _context.GL_ACCOUNTS_V.AsNoTracking().Where(a => a.CODE_COMBINATION_ID == code_combination_id);
                return _data.SingleOrDefault();
            }

        }
       
       

        /// <summary>
        /// Returns a list of filtered general ledger accounts that have not been used in the overhead module.
        /// </summary>
        /// <param name="segment1"></param>
        /// <param name="segment2"></param>
        /// <param name="segment3"></param>
        /// <param name="segment4"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        //public static List<GL_ACCOUNTS_V> AccountsFiltered(string segment1, string segment2, string segment3, string segment4, long organizationId)
        //{

        //        IQueryable<GL_ACCOUNTS_V> _data = AccountsFiltered(segment1, segment2, segment3, segment4).AsQueryable();

        //        using (Entities _context = new Entities())
        //        {
        //            _data = (from dups in _data
        //                     where !_context.OVERHEAD_GL_ACCOUNT.Where(ac => ac.OVERHEAD_ORG_ID == organizationId).Any(c => c.CODE_COMBO_ID == dups.CODE_COMBINATION_ID)
        //                     select dups);

        //            return _data.ToList();
        //        }
           
           
        //}

    }


    /// <summary>
    /// Methods and functions over generic GL data that are not tied to entity objects.
    /// </summary>
    public class GL
    {
        /// <summary>
        /// Returns a list of all budget types
        /// </summary>
        /// <returns></returns>
        public static List<BUDGET_TYPE> BudgetTypes()
        {
          
                using (Entities _context = new Entities())
                {
                    string sql = @"select a.budget_name,a.Description,b.legal_entity_id as LE_ORG_ID, a.status
                             from gl.gl_budgets a
                             inner join apps.hr_operating_units b on b.set_of_books_id = a.set_of_books_id order by 1";

                    List<BUDGET_TYPE> _returnList = _context.Database.SqlQuery<BUDGET_TYPE>(sql).ToList();

                    return _returnList;
                }
           

        }

        /// <summary>
        /// Returns a list of active budget types by legal entity organization id
        /// </summary>
        /// <param name="legalEntity"></param>
        /// <returns></returns>
        public static List<BUDGET_TYPE> BudgetTypes(long legalEntityOrganizationId)
        {
           
                string legalEntityString = legalEntityOrganizationId.ToString();
                List<BUDGET_TYPE> _data = BudgetTypes().Where(x => x.STATUS == "O" && x.LE_ORG_ID == legalEntityString).ToList();
                return _data;
           
        }

        /// <summary>
        /// Returns a list of unused budget types by a legal entity. If they were used in the overhead system, they will not show in this returned list.
        /// </summary>
        /// <param name="legalEntity"></param>
        /// <returns></returns>
        public static List<BUDGET_TYPE> BudgetTypesRemaining(long legalEntityOrganizationId)
        {
           
                //Budget Types that are active by legal entity organization Id
                IQueryable<BUDGET_TYPE> _data = BudgetTypes(legalEntityOrganizationId).AsQueryable();

                //Budget Types in the overhead budget system that have been entered by this legal entity organization.
                List<OVERHEAD_BUDGET_TYPE> _budgetTypes = OVERHEAD_BUDGET_TYPE.BudgetTypes(legalEntityOrganizationId);

                _data = (from dups in _data
                         where !_budgetTypes.Any(x => x.BUDGET_NAME == dups.BUDGET_NAME)
                         select dups);

                return _data.ToList();    
        }

        public static List<OVERHEAD_BUDGET_TYPE> BudgetTypesEnteredAndAvailaible(long legalEntityOrganizationId)
        {

            IQueryable<OVERHEAD_BUDGET_TYPE> _data = OVERHEAD_BUDGET_TYPE.BudgetTypes(legalEntityOrganizationId).AsQueryable();

            List<OVERHEAD_BUDGET_TYPE> _existingData = OVERHEAD_BUDGET_TYPE.BudgetTypes(legalEntityOrganizationId).ToList();

            _data = (from dups in _data
                     where !_existingData.Any(x => x.PARENT_BUDGET_TYPE_ID == dups.OVERHEAD_BUDGET_TYPE_ID)
                     select dups);

            return _data.ToList();
        }

        /// <summary>
        /// Returns a list of unused budget types by a legal entity. If they were used in the overhead system, they will not show in this returned list. This list will also filter out the same type so it can't be picked again.
        /// </summary>
        /// <param name="legalEntityOrganizationId"></param>
        /// <param name="budgetType"></param>
        /// <returns></returns>
        public static List<BUDGET_TYPE> BudgetTypesRemaining(long legalEntityOrganizationId, string budgetType)
        {
           
                //Budget Types that are active by legal entity organization Id
                IQueryable<BUDGET_TYPE> _data = BudgetTypes(legalEntityOrganizationId).AsQueryable();

                //Budget Types in the overhead budget system that have been entered by this legal entity organization.
                List<OVERHEAD_BUDGET_TYPE> _budgetTypes = OVERHEAD_BUDGET_TYPE.BudgetTypes(legalEntityOrganizationId);

                _data = (from dups in _data
                         where !_budgetTypes.Any(x => x.BUDGET_NAME == dups.BUDGET_NAME && x.BUDGET_NAME == budgetType)
                         select dups);

                return _data.ToList();

            
        }
        

        public class BUDGET_TYPE
        {
            public string BUDGET_NAME { get; set; }
            public string DESCRIPTION { get; set; }
            public string LE_ORG_ID { get; set; }
            public string STATUS { get; set; }
        }

    }
    
}
