using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data
{
    public partial class SYS_USER_PERMS
    {
        public static List<SYS_USER_PERMS> GetUserPermissions(long UserId)
        {
            //using (Entities _context = new Entities())
            //{
                Entities _context = new Entities();
                List<SYS_USER_PERMS> Permissions = (from p in _context.SYS_USER_PERMS
                                                    where p.USER_ID == UserId
                                                    select p).ToList();
                return Permissions;
            //}
        }
    }
}
