using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DBI.Data.DataFactory.Utilities;
using Ext.Net;

namespace DBI.Data.DataFactory.Security
{
    public class Roles
    {

        public static IEnumerable<SYS_ROLES> RoleList(int start, int limit, DataSorter[] sort, string filter, out int count)
        {
            return GenericData.EnumerableFilter<SYS_ROLES>(start, limit, sort, filter, out count);
        }

        public static IEnumerable<SYS_ROLES> RoleList()
        {
            Entities _context = new Entities();
            return _context.SYS_ROLES.AsEnumerable();
        }

        public static void SaveRole(SYS_ROLES role)
        {
                // If role has no id, perform insert
                if (role.ROLE_ID == 0)
                {
                    GenericData.Insert<SYS_ROLES>(role);
                }
                else // perform update
                {
                    GenericData.Update<SYS_ROLES>(role);
                }
        }

        public static void DeleteRoleByID(long id)
        {
                Entities _context = new Entities();
                SYS_ROLES role = _context.SYS_ROLES.Where(r => r.ROLE_ID == id).SingleOrDefault();
                if (role != null)
                {
                    GenericData.Delete<SYS_ROLES>(role);
                }
        }

        public static SYS_ROLES RoleByID(decimal id)
        {
            Entities _context = new Entities();
            return _context.SYS_ROLES.Where(r => r.ROLE_ID == id).SingleOrDefault();
        }

        public static void DeleteUserRoleByUserID(decimal userID, decimal roleID)
        {
            Entities _context = new Entities();
            SYS_USER_ROLES role = _context.SYS_USER_ROLES.Where(u => u.SYSTEM_USER_ID == userID && u.ROLE_ID == roleID).SingleOrDefault();
            if (role != null)
            {
                GenericData.Delete<SYS_USER_ROLES>(role);
            }
        }

        public static SYS_USER_ROLES DoesUserRoleExist(decimal userID, decimal roleID)
        {
            Entities _context = new Entities();
            SYS_USER_ROLES role = _context.SYS_USER_ROLES.Where(u => u.SYSTEM_USER_ID == userID && u.ROLE_ID == roleID).SingleOrDefault();
            return role;
        }


        public static IEnumerable<SYS_USER_ROLES_V> RolesByUserID(decimal userID)
        {
            Entities _context = new Entities();
            return _context.SYS_USER_ROLES_V.Where(r => r.SYSTEM_USER_ID == userID).AsEnumerable();
        }


    }
}
