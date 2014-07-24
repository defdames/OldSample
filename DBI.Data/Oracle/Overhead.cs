using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data
{
    public class OVERHEAD
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

    public class OVERHEAD_GL_RANGE_V : OVERHEAD_GL_RANGE
        {
            public string SRSEGMENTS { get; set; }
            public string ERSEGMENTS { get; set; }
            public string INCLUDE_EXCLUDE { get; set; }
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




}
