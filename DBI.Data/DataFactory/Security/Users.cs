using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data.DataFactory.Security
{
    public class Users
    {
        /// <summary>
        /// Entity function that returns a list of all users in the system for EMS
        /// This view is an left outer join to the FND_USER table that shows oracle user data.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SYS_USERS_V> UserList()
        {
            Entities db = new Entities();
            return db.SYS_USERS_V.AsEnumerable();             
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


    }







}



