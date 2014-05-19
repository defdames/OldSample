using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data
{
    public partial class SYS_PROFILE_OPTIONS
    {
        /// <summary>
        /// Returns a list of profile options in the system.
        /// </summary>
        /// <returns></returns>
        public static List<SYS_PROFILE_OPTIONS> systemProfileOptions()
        {
            using (Entities _context = new Entities())
            {
                return _context.SYS_PROFILE_OPTIONS.ToList();
            }
        }

        public static void deleteProfileOptionByRecordID(long recordID)
        {
            SYS_PROFILE_OPTIONS option;
            using (Entities _context = new Entities())
            {
               option = _context.SYS_PROFILE_OPTIONS.Where(a => a.PROFILE_OPTION_ID == recordID).SingleOrDefault();
            }

            GenericData.Delete<SYS_PROFILE_OPTIONS>(option);
        }

        public static SYS_PROFILE_OPTIONS profileOptionByRecordID(long recordID)
        {
            using (Entities _context = new Entities())
            {
                return _context.SYS_PROFILE_OPTIONS.Where(a => a.PROFILE_OPTION_ID == recordID).SingleOrDefault();
            }

        }
    }
}
