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
        public static List<DBI.Data.GL_ACCOUNTS_V> filter(string segment1, string segment2, string segment3, string segment4, long organizationId)
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

    /// <summary>
    /// Custom methods and fuctions over GL data
    /// </summary>
    public class GL
    {
        /// <summary>
        /// Returns a list of all budget types
        /// </summary>
        /// <returns></returns>
        public static List<BUDGET_TYPE> budgetTypes()
        {
            try
            {
                using (Entities _context = new Entities())
                {
                    string sql = @"select a.budget_name,a.Description,b.legal_entity_id as LE_ORG_ID
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
        /// <param name="organizationBusinessUnitId"></param>
        /// <returns></returns>
        public static List<BUDGET_TYPE> activeBudgetTypes(long organizationBusinessUnitId)
        {
            try
            {
                List<BUDGET_TYPE> _data = GL.budgetTypes().Where(x => x.STATUS == "O" && x.LE_ORG_ID == organizationBusinessUnitId).ToList();
                return _data;
            }
            catch (Exception)
            {             
                throw;
            }
         
        }


        /// <summary>
        /// Returns a list of legal entities from oracle that can have a budget because there is a budget type assigned to that businessunit
        /// </summary>
        /// <returns></returns>
        public static List<HR.ORGANIZATION> legalEntitiesWithActiveBudgetTypes()
        {
            try
            {
                using (Entities _context = new Entities())
                {
                    List<HR.ORGANIZATION> _data = HR.activeLegalEntityOrganizationList();
                    List<HR.ORGANIZATION> _returnList = new List<HR.ORGANIZATION>();

                    foreach (HR.ORGANIZATION var in _data)
                    {
                        int count = DBI.Data.GL.activeBudgetTypes(var.ORGANIZATION_ID).Count();
                        if (count > 0)
                        {
                            _returnList.Add(var);
                        }
                    }

                    return _returnList;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
     

    }

    //****************************************************************************************
    // Custom classes
    //****************************************************************************************
    
    public class BUDGET_TYPE
    {
        public string BUDGET_NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public long LE_ORG_ID { get; set; }
        public string STATUS { get; set; }
    }

}
