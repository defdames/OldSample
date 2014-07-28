using System;
using System.Collections.Generic;
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
                .Select(x => new OVERHEAD_GL_RANGE_V { GL_RANGE_ID = x.GL_RANGE_ID, ORGANIZATION_ID = x.ORGANIZATION_ID, INCLUDE_EXCLUDE_FLAG = x.INCLUDE_EXCLUDE_FLAG, 
                    SRSEGMENTS = x.SRSEGMENT1 + "." + x.SRSEGMENT2 + "." + x.SRSEGMENT3 + "." + x.SRSEGMENT4 + "." + x.SRSEGMENT5 + "." + x.SRSEGMENT6 + "." + x.SRSEGMENT7,
                    ERSEGMENTS = x.ERSEGMENT1 + "." + x.ERSEGMENT2 + "." + x.ERSEGMENT3 + "." + x.ERSEGMENT4 + "." + x.ERSEGMENT5 + "." + x.ERSEGMENT6 + "." + x.ERSEGMENT7,
                INCLUDE_EXCLUDE = (x.INCLUDE_EXCLUDE_FLAG == "I") ? "Included" : "Excluded"});
            return _data;        
        }
    }

    public partial class OVERHEAD_BUDGET_TYPE
    {

        public static string GetTypeByID(long budgetTypeID)
        {
            using (Entities _context = new Entities())
            {
                return _context.OVERHEAD_BUDGET_TYPE.Where(x => x.OVERHEAD_BUDGET_TYPE_ID == budgetTypeID).SingleOrDefault().BUDGET_NAME;
            }
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
        public string OVERHEAD_BUDGET_TYPE { get; set; }
        public string BUDGET_STATUS { get; set; }
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
        public static IQueryable<OVERHEAD_ORG_BUDGETS> BudgetListByOrganizationID(long organizationID, Entities context)
        {
            var data = context.OVERHEAD_ORG_BUDGETS.Where(x => x.ORGANIZATION_ID == organizationID);
        }
    }



}
