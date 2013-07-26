using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBI.Data.DataFactory.Utilities;
using Ext.Net;

namespace DBI.Data.DataFactory.Security
{
   public class Roles
    {

        /// <summary>
        /// returns a list of user roles using for paging and to help with memory requirements. 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="sort"></param>
        /// <param name="filter"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<SYS_ROLES> RoleList(int start, int limit, DataSorter[] sort, string filter, out int count)
        {
            return GenericData.EnumerableFilter<SYS_ROLES>(start, limit, sort, filter, out count);
        }


       /// <summary>
       /// returns a system role by role id
       /// </summary>
       /// <param name="roleID"></param>
       /// <returns></returns>
        public static SYS_ROLES RoleByID(long roleID)
        {
            try
            {
                using (Entities _context = new Entities())
                {
                    return _context.SYS_ROLES.Where(r => r.ROLE_ID == roleID).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("System role not found: {0}",ex.InnerException.Message));
            }
        }

       /// <summary>
       /// Delete a system role by role id
       /// </summary>
       /// <param name="roleID"></param>
        public static void  DeleteRoleByID(long roleID)
        {
            try
            {
                using (Entities _context = new Entities())
                {
                    SYS_ROLES role = RoleByID(roleID);
                    GenericData.Delete<SYS_ROLES>(role);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Can not delete system role: {0}", ex.InnerException.Message));
            }
        }

        /// <summary>
        /// Adds or Updates a Security Role
        /// </summary>
        /// <param name="role"></param>
        public static void SaveRole(SYS_ROLES role)
        {
            using (Entities _context = new Entities())
            {

                //Get role that matches id
                SYS_ROLES roleCheck = RoleByID(role.ROLE_ID);

                if (roleCheck == null)
                {
                    GenericData.Insert<SYS_ROLES>(role);
                }
                else
                {
                    GenericData.Update<SYS_ROLES>(role);
                }
            }

        }


    }
}
