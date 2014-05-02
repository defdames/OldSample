using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data.Oracle.GL
{
    public class Budgets
    {
        public static List<BUDGET_TYPE> activeBudgetTypesByBusinessUnit(long businessUnitId)
        {
            using (Entities _context = new Entities())
            {
                string sql = @"select a.budget_name,a.Description,b.legal_entity_id as LE_ORG_ID
                             from gl.gl_budgets a
                             inner join apps.hr_operating_units b on b.set_of_books_id = a.set_of_books_id
                             where a.status = 'O' order by 1";

                List<BUDGET_TYPE> _returnList = _context.Database.SqlQuery<BUDGET_TYPE>(sql).Where(a => a.LE_ORG_ID == businessUnitId.ToString()).ToList();

                return _returnList;
            }
        }

        public static List<BUDGET_TYPE> budgetTypesAvailaibleForUseByBusinessUnit(long businessUnitId, string recordId)
        {
            using (Entities _context = new Entities())
            {
                string sql = string.Empty;

                if (!string.IsNullOrEmpty(recordId))
                {

                    sql = @"select a.budget_name,a.Description,b.legal_entity_id as LE_ORG_ID
                             from gl.gl_budgets a
                             inner join apps.hr_operating_units b on b.set_of_books_id = a.set_of_books_id
                             where a.status = 'O'
                             and a.budget_name not in (select distinct budget_name from xxems.overhead_budget_type where LE_ORD_ID = '" + businessUnitId + @"' and OVERHEAD_BUDGET_TYPE_ID != '" + recordId + @"') order by 1";
                }
                else
                {
                    sql = @"select a.budget_name,a.Description,b.legal_entity_id as LE_ORG_ID
                             from gl.gl_budgets a
                             inner join apps.hr_operating_units b on b.set_of_books_id = a.set_of_books_id
                             where a.status = 'O'
                             and a.budget_name not in (select distinct budget_name from xxems.overhead_budget_type where LE_ORD_ID = '" + businessUnitId + @"') order by 1";
                }

                List<BUDGET_TYPE> _returnList = _context.Database.SqlQuery<BUDGET_TYPE>(sql).Where(a => a.LE_ORG_ID == businessUnitId.ToString()).ToList();

                return _returnList;
            }
        }

    }

    public class BUDGET_TYPE
    {
        public string BUDGET_NAME { get; set; }
        public string DESCRIPTION { get; set; }
        public string   LE_ORG_ID { get; set; }
    }
}
