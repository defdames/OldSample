using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DBI.Data.Models;

namespace DBI.Data.DataFactory
{
    public class SecurityRoles
    {

       public static IEnumerable<DBI.Data.Models.SECURITYROLE> GetSecurityRoles()
       {
           OracleDatabase db = new OracleDatabase();
           return db.SECURITYROLEs.AsEnumerable();
       }


    }
}
