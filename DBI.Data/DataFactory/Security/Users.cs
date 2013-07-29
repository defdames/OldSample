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
    public class Users
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
        public static IEnumerable<SYS_USER_INFORMATION> UserList(int start, int limit, DataSorter[] sort, string filter, out int count)
        {
            return GenericData.EnumerableFilter<SYS_USER_INFORMATION>(start, limit, sort, filter, out count);
        }




    }







}



