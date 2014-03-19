using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Ext.Net;

namespace DBI.Data
{
    public partial class SYS_USER_INFORMATION
    {
        /// <summary>
        /// Returns a list of user information from oracle that will be used to control system access
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="sort"></param>
        /// <param name="filter"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static IEnumerable<SYS_USER_INFORMATION> Users(int start, int limit, DataSorter[] sort, string filter, out int count)
        {
            return GenericData.EnumerableFilter<SYS_USER_INFORMATION>(start, limit, sort, filter, out count);
        }

        /// <summary>
        /// Returns a list of user information from oracle that will be used in lookups for comboboxes
        /// </summary>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="sort"></param>
        /// <param name="filter"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static Ext.Net.Paging<SYS_USER_INFORMATION> UsersLookup(int start, int limit, string sort, string dir, string filter, string field)
        {
         return GenericData.PagingFilter<SYS_USER_INFORMATION>(start, limit, sort, dir, filter, field);   
        }


        /// <summary>
        /// Return user information by user id
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static SYS_USER_INFORMATION UserByID(long userID)
        {
            using (Entities _context = new Entities())
            {
                return _context.Set<SYS_USER_INFORMATION>().Where(u => u.USER_ID == userID).SingleOrDefault();
            }
        }

        /// <summary>
        /// Return user information by user name
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static SYS_USER_INFORMATION UserByUserName(string username)
        {
            using (Entities _context = new Entities())
            {
                return _context.Set<SYS_USER_INFORMATION>().Where(u => u.USER_NAME == username).SingleOrDefault();
            }
        }


        /// <summary>
        /// Return user id by user name
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static long UserID(string username)
        {
            using (Entities _context = new Entities())
            {
            return _context.Set<SYS_USER_INFORMATION>().Where(u => u.USER_NAME == username).Max(u => u.USER_ID);
            }
        }

        public static string GetJobTitle(long UserId)
        {
            using (Entities _context = new Entities())
            {
                string JobTitle = (from s in _context.SYS_USER_INFORMATION
                                   where s.USER_ID == UserId
                                   select s.JOB_NAME).Single();
                return JobTitle;
            }
        }

        public static long GetJobId(long UserId)
        {
            using (Entities _context = new Entities())
            {
                long JobId = (from s in _context.SYS_USER_INFORMATION
                              where s.USER_ID == UserId
                              select s.JOB_ID).Single();
                return JobId;
            }
        }
    }







}



