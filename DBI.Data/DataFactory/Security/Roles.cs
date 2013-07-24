using System;
using System.Collections.Generic;
using System.Data.Objects;
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
        /// return a list of roles by user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static IQueryable<SYS_USER_ROLES> RolesByUserID(long userID)
        {
            Entities _context = new Entities();
            IQueryable<SYS_USER_ROLES> roles = _context.SYS_USER_ROLES.Where(a => a.USER_ID == userID);
            return roles;
        }

        /// <summary>
        /// Returns a list of all roles setup in the system
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SYS_ROLES> RoleList()
        {
            Entities _context = new Entities();
            return _context.SYS_ROLES.AsEnumerable();
        }

        /// <summary>
        /// Add a security role to a user record
        /// </summary>
        /// <param name="role"></param>
        public static void AddUserRole(SYS_USER_ROLES role)
        {
            Entities _context = new Entities();
            GenericData.Insert<SYS_USER_ROLES>(role);
        }

        /// <summary>
        /// Deletes a security role to a user record
        /// </summary>
        /// <param name="role"></param>
        public static void DeleteUserRole(SYS_USER_ROLES role)
        {
            Entities _context = new Entities();
            GenericData.Delete<SYS_USER_ROLES>(role);
        }


        /// <summary>
        /// Returns a user role by finding with the user id and role id
        /// </summary>
        /// <param name="userID">User ID for the user role</param>
        /// <param name="roleID">Role ID for the role</param>
        /// <returns></returns>
        public static SYS_USER_ROLES RoleByUserandRoleID(long userID, long roleID)
        {
            using (Entities _context = new Entities())
            {
                return _context.SYS_USER_ROLES.Where(r => r.USER_ID == userID && r.ROLE_ID == roleID).SingleOrDefault();
            }
        }


        /// <summary>
        /// Deletes a security role by role id
        /// </summary>
        /// <param name="roleID"></param>
        public static void DeleteRoleByID(long roleID)
        {
            using (Entities _context = new Entities())
            {
                //Get role that matches id
                SYS_ROLES role = _context.Set<SYS_ROLES>().Where(r => r.ROLE_ID == roleID).FirstOrDefault();
                GenericData.Delete<SYS_ROLES>(role);
            }
        }

        /// <summary>
        /// Returns a security role by role id
        /// </summary>
        /// <param name="role"></param>
        public static SYS_ROLES RoleByID(long roleID)
        {
            using (Entities _context = new Entities())
            {
                //Get role that matches id
                SYS_ROLES role = _context.Set<SYS_ROLES>().Where(r => r.ROLE_ID == roleID).FirstOrDefault();
                return role;
            }
        }

        /// <summary>
        /// Add a security role
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
