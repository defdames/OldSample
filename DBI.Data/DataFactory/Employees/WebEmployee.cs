using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data
{
    public partial class DAILY_ACTIVITY_EMPLOYEE
    {
        public static List<DAILY_ACTIVITY_EMPLOYEE> GetEmployees()
        {
            using (Entities _context = new Entities())
            {
                return _context.Set<DAILY_ACTIVITY_EMPLOYEE>().ToList();
            }
        }

        public static bool employeesWithShopTimeByDailyActivityID(long dailyActivityID)
        {
            using (Entities _context = new Entities())
            {
                return _context.DAILY_ACTIVITY_EMPLOYEE.Where(e => e.HEADER_ID == dailyActivityID && (e.SHOPTIME_AM.HasValue || e.SHOPTIME_PM.HasValue)).ToList().Count > 0 ? true : false;
            }
        }


    }
}
