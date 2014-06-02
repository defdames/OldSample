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
    /// Methods and fuctions over the GL_ACCOUNTS_V Entity Object
    /// </summary>
    public partial class GL_ACCOUNTS_V
    {
        /// <summary>
        /// Filters gl accounts by segments
        /// </summary>
        /// <param name="segment1"></param>
        /// <param name="segment2"></param>
        /// <param name="segment3"></param>
        /// <param name="segment4"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public static List<GL_ACCOUNTS_V> Filter(string segment1, string segment2, string segment3, string segment4, long organizationId)
        {
            try
            {
                using (Entities _context = new Entities())
                {
                    var _data = _context.GL_ACCOUNTS_V.AsNoTracking().Where(a => a.SEGMENT1 == segment1);
                    _data = _data.Where(a => a.SEGMENT2 == segment2);
                    _data = _data.Where(a => a.SEGMENT3 == segment3);
                    _data = _data.Where(a => a.SEGMENT4 == segment4);

                    _data = (from dups in _data
                             where !_context.OVERHEAD_GL_ACCOUNT.Where(ac => ac.OVERHEAD_ORG_ID == organizationId).Any(c => c.CODE_COMBO_ID == dups.CODE_COMBINATION_ID)
                             select dups);

                    return _data.ToList();
                }
            }
            catch (Exception)
            {
                
                throw;
            }
           
        }

    }


    public class GL
    {
        /// <summary>
        /// Returns a list of all budget types
        /// </summary>
        /// <returns></returns>
        public static List<BUDGET_TYPE> BudgetTypes()
        {
            try
            {
                using (Entities _context = new Entities())
                {
                    string sql = @"select a.budget_name,a.Description,b.legal_entity_id as LE_ORG_ID, a.status
                             from gl.gl_budgets a
                             inner join apps.hr_operating_units b on b.set_of_books_id = a.set_of_books_idorder by 1";

                    List<BUDGET_TYPE> _returnList = _context.Database.SqlQuery<BUDGET_TYPE>(sql).ToList();

                    return _returnList;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        /// <summary>
        /// Returns a list of active budget types by legal entity organization id
        /// </summary>
        /// <param name="legalEntity"></param>
        /// <returns></returns>
        public static List<BUDGET_TYPE> ActiveBudgetTypes(long legalEntity)
        {
            try
            {
                List<BUDGET_TYPE> _data = BudgetTypes().Where(x => x.STATUS == "O" && x.LE_ORG_ID == legalEntity).ToList();
                return _data;
            }
            catch (Exception)
            {             
                throw;
            }
        }

        public static List<BUDGET_TYPE> UnUsedBudgetTypesByLegalEntity(long legalEntity)
        {
            try
            {
                using (Entities _context = new Entities())
                {
                    string sql = @"select a.budget_name,a.Description,b.legal_entity_id as LE_ORG_ID
                             from gl.gl_budgets a
                             inner join apps.hr_operating_units b on b.set_of_books_id = a.set_of_books_id
                             where a.status = 'O'
                             and a.budget_name not in (select distinct budget_name from xxems.overhead_budget_type where LE_ORD_ID = '" + legalEntity + @"') order by 1";

                    List<BUDGET_TYPE> _data = _context.Database.SqlQuery<BUDGET_TYPE>(sql).Where(a => a.LE_ORG_ID == legalEntity).ToList();
                    return _data;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        
        /// <summary>
        /// Returns a list of unused budget types that are availible for use, if user adds the overheadBudgetTypeId it also make sure not to return that one that they selected.
        /// </summary>
        /// <param name="legalEntity"></param>
        /// <param name="overheadBudgetTypeId"></param>
        /// <returns></returns>
        public static List<BUDGET_TYPE> UnUsedBudgetTypesByLegalEntity(long legalEntity, string overheadBudgetTypeId = null)
        {
            try
            {
                using (Entities _context = new Entities())
                {
                    string sql = string.Empty;

                    if (!string.IsNullOrEmpty(overheadBudgetTypeId))
                    {

                        sql = @"select a.budget_name,a.Description,b.legal_entity_id as LE_ORG_ID
                             from gl.gl_budgets a
                             inner join apps.hr_operating_units b on b.set_of_books_id = a.set_of_books_id
                             where a.status = 'O'
                             and a.budget_name not in (select distinct budget_name from xxems.overhead_budget_type where LE_ORD_ID = '" + legalEntity + @"' and OVERHEAD_BUDGET_TYPE_ID != '" + overheadBudgetTypeId + @"') order by 1";
                    }
                    else
                    {
                        sql = @"select a.budget_name,a.Description,b.legal_entity_id as LE_ORG_ID
                             from gl.gl_budgets a
                             inner join apps.hr_operating_units b on b.set_of_books_id = a.set_of_books_id
                             where a.status = 'O'
                             and a.budget_name not in (select distinct budget_name from xxems.overhead_budget_type where LE_ORD_ID = '" + legalEntity + @"') order by 1";
                    }

                    List<BUDGET_TYPE> _data = _context.Database.SqlQuery<BUDGET_TYPE>(sql).Where(a => a.LE_ORG_ID == legalEntity).ToList();
                    return _data;

                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public class BUDGET_TYPE
        {
            public string BUDGET_NAME { get; set; }
            public string DESCRIPTION { get; set; }
            public long LE_ORG_ID { get; set; }
            public string STATUS { get; set; }
        }

    }
    
}
