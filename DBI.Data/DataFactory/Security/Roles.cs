using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBI.Data.DataFactory.Utilities;
using Ext.Net;
using System.Data.Entity;
using DBI.Core.Web;

namespace DBI.Data.DataFactory.Security
{
   public class Roles
    {

        /// <summary>
        /// returns a list of system roles using for paging and to help with memory requirements. 
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
        /// returns a user role by role id
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public static SYS_USER_ROLES UserRoleByID(long uRoleID)
        {
            try
            {
                using (Entities _context = new Entities())
                {
                    return _context.SYS_USER_ROLES.Where(r => r.USER_ROLE_ID == uRoleID).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("User role not found: {0}", ex.InnerException.Message));
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
                    if (!DoesRoleExistByUser(role.ROLE_ID))
                    {
                    GenericData.Delete<SYS_ROLES>(role);
                    }
                    else
                    {
                        throw new Exception("Role is currently is use");
                    }
                        

                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Can not delete system role: {0}", ex.Message));
            }
        }

       /// <summary>
       /// Checks to see if the system role is assigned to any user
       /// </summary>
       /// <param name="roleID"></param>
       /// <returns></returns>
        public static bool DoesRoleExistByUser(long roleID)
        {
            try
            {
                using (Entities _context = new Entities())
                {
                    return _context.Set<SYS_USER_ROLES>().Any(r => r.ROLE_ID.Equals(roleID));
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Can not find a user role: {0}", ex.InnerException.Message));
            }

        }

        /// <summary>
        /// Delete a user role by role id
        /// </summary>
        /// <param name="roleID"></param>
        public static void DeleteUserRoleByID(long UserRoleID)
        {
            try
            {
                using (Entities _context = new Entities())
                {
                    SYS_USER_ROLES urole = UserRoleByID(UserRoleID);
                    GenericData.Delete<SYS_USER_ROLES>(urole);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Can not delete user role: {0}", ex.InnerException.Message));
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


        /// <summary>
        /// return a list of roles by user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static List<SYS_USER_ROLES> RolesByUserID(long userID)
        {
            Entities _context = new Entities();
            List<SYS_USER_ROLES> roles = _context.SYS_USER_ROLES.Include(r => r.SYS_ROLES).Where(a => a.USER_ID == userID).ToList();
            return roles;
        }

        /// <summary>
        /// return a list of roles not assigned to the user
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static IEnumerable<SYS_ROLES> FreeRolesByUserID(long userID)
        {
            Entities _context = new Entities();
            List<SYS_ROLES> roles = _context.Set<SYS_ROLES>().ToList();
            List<SYS_USER_ROLES> uRoles = RolesByUserID(userID);
            roles.RemoveAll(i => uRoles.Select(s => s.ROLE_ID).Contains(i.ROLE_ID));
            return roles;
        }

        public static void AddRoleToUserID(long roleID, long userID)
        {
            //Get role information
            Entities _context = new Entities();
            SYS_ROLES role = _context.Set<SYS_ROLES>().Where(r => r.ROLE_ID == roleID).Single();

            SYS_USER_ROLES uRole = new SYS_USER_ROLES();
            uRole.ROLE_ID = role.ROLE_ID;
            uRole.USER_ID = userID;
            GenericData.Insert<SYS_USER_ROLES>(uRole);
        }

    }
}
