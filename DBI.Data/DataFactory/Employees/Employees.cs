﻿using System;
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

        public static List<EMPLOYEES_V> EmployeeDropDown(int OrganizationId = 124)
        {
            using (Entities _context = new Entities())
            {
                //todo update Organization ID from claim
                var data = (from e in _context.EMPLOYEES_V
                            where e.CURRENT_EMPLOYEE_FLAG == "Y" && e.ORGANIZATION_ID == OrganizationId
                            select e).ToList();
                return data;
            }
        }
    }
}
