using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBI.Data.Models;

namespace DBI.Data.DataFactory.EMS
{
    public class SystemSecurity
    {
        /// <summary>
        /// Entity function that returns a list of all users in the system for EMS
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<DBI.Data.Models.SYS_USERS> GetSystemUsers()
        {
            OracleDatabase db = new OracleDatabase();
            return db.SYS_USERS.AsEnumerable();
        }



    }







}



