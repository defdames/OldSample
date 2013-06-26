using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBI.Data.DataFactory.Utilities;

namespace DBI.Data.DataFactory.Security
{
    public class Roles
    {

        public static IEnumerable<SYS_ROLES> RoleList()
        {
            Entities db = new Entities();
            SYS_ROLES Roles = new SYS_ROLES();
            return db.SYS_ROLES.AsEnumerable();
        }


        public static void InsertRole(SYS_ROLES role)
        {
            GenericData.Insert<SYS_ROLES>(role);
        }

    }
}
