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

        public static IEnumerable<SYS_USERS_V> UserList(int start, int limit, DataSorter[] sort, string filter, out int count)
        {
            return GenericData.EnumerableFilter<SYS_USERS_V>(start, limit, sort, filter, out count);
        }

        public static IEnumerable<SYS_USERS> OracleUserList()
        {
            Entities db = new Entities();
            return db.SYS_USERS.AsEnumerable();
        }

        public static SYS_USERS_V UserDetailsByID(decimal pUserID)
        {
            Entities db = new Entities();
            return db.SYS_USERS_V.Where(u => u.SYSTEM_USER_ID == pUserID).FirstOrDefault();
        }

        public static SYS_USERS_V UserDetailsByUsername(string userName)
        {
            Entities db = new Entities();
            return db.SYS_USERS_V.Where(u => u.USER_NAME == userName).FirstOrDefault();
        }

        public static decimal? UserSystemIDByOracleUserID(long oracleUserID)
        {
            Entities db = new Entities();
            return db.SYS_USERS_V.Where(u => u.USER_ID == oracleUserID).Max(m => m.SYSTEM_USER_ID);

        }


    }







}



