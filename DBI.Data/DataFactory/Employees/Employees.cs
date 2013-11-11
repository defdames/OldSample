using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data
{
    public partial class EMPLOYEES_V
    {
        public static List<EMPLOYEES_V> Employees()
        {
            using (Entities _context = new Entities())
            {
                return _context.Set<EMPLOYEES_V>().ToList();
            }
        }

        public static List<EMPLOYEES_V> EmployeeDropDown(int OrganizationId = 0)
        {
            using (Entities _context = new Entities())
            {
                var data = _context.EMPLOYEES_V.Where(e => e.CURRENT_EMPLOYEE_FLAG == "Y");
                if (OrganizationId != 0)
                {
                    data = data.Where(e => e.ORGANIZATION_ID == OrganizationId);
                }
                return data.ToList();
            }
        }
    }
}
