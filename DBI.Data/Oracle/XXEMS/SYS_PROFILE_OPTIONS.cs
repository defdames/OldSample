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
    }
}
