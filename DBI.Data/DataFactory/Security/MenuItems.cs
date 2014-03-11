using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data
{
    public partial class SYS_MENU
    {
        public static List<SYS_MENU> GetMenuItems(decimal ModuleId)
        {
            using (Entities _context = new Entities())
            {
                List<SYS_MENU> MenuItems = (from m in _context.SYS_MENU
                                            where m.MODULE_ID == ModuleId
                                            select m).ToList();
                return MenuItems;
            }
        }

        public static List<SYS_MENU> GetMenuItems(decimal ModuleId, decimal PermissionID)
        {
            using (Entities _context = new Entities())
            {
                List<SYS_MENU> MenuItems = (from m in _context.SYS_MENU
                                            where m.MODULE_ID == ModuleId && m.PERMISSION_ID == PermissionID
                                            select m).ToList();
                return MenuItems;
            }
        }
    }
}
