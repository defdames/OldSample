using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data
{
    public partial class SYS_MODULES
    {
        public static List<SYS_MODULES> GetModules()
        {
            using (Entities _context = new Entities())
            {
                List<SYS_MODULES> Modules = (from m in _context.SYS_MODULES
                                             select m).ToList();
                return Modules;
            }
        }

        public static SYS_MODULES GetModules(decimal PermissionId)
        {
            using (Entities _context = new Entities())
            {
                SYS_MODULES Modules = (from m in _context.SYS_MODULES
                                       where m.PERMISSION_ID == PermissionId
                                       select m).Single();
                return Modules;
            }
        }
    }
}
