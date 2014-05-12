using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBI.Data;

namespace DBI.Data
{
    public partial class OVERHEAD_BUDGET_TYPE
    {
        /// <summary>
        /// Returns a list of overhead budget types by legal entity
        /// </summary>
        /// <param name="orgID"></param>
        /// <returns></returns>
        public static List<OVERHEAD_BUDGET_TYPE> budgetTypesByBusinessUnitID(long BusinessUnitID)
        {
            using (Entities _context = new Entities())
            {
                List<OVERHEAD_BUDGET_TYPE> _returnList = _context.OVERHEAD_BUDGET_TYPE.Where(c => c.LE_ORD_ID == BusinessUnitID).ToList();
                return _returnList;
            }
        }

        /// <summary>
        /// Returns a budget type for a record_id
        /// </summary>
        /// <param name="orgID"></param>
        /// <returns></returns>
        public static OVERHEAD_BUDGET_TYPE budgetTypeById(long recordId)
        {
            using (Entities _context = new Entities())
            {
                OVERHEAD_BUDGET_TYPE _returnData = _context.OVERHEAD_BUDGET_TYPE.Where(c => c.OVERHEAD_BUDGET_TYPE_ID == recordId).SingleOrDefault();
                return _returnData;
            }
        }

    }
}
