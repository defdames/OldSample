using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Objects;

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

        public static long GetEmployeeBusinessUnit(long EmployeeOrgId)
        {
            using (Entities _context = new Entities())
            {
                string sql = @"select BU
                        from
                            (SELECT             c.organization_id_parent BU,
                                                haou.name ORG_Name
                            FROM                per_organization_structures_v a
                            INNER JOIN          per_org_structure_versions_v b on a.organization_structure_id = b.organization_structure_id
                            INNER JOIN          per_org_structure_elements_v c on b.org_structure_version_id = c.org_structure_version_id
                            INNER JOIN          hr.hr_all_organization_units haou on haou.organization_id = c.organization_id_parent
                            WHERE               SYSDATE BETWEEN b.date_from and nvl(b.date_to,'31-DEC-4712') 
                            AND                 a.organization_structure_id <> 61
                            START WITH          c.organization_id_child = " + EmployeeOrgId.ToString() + @"
                            CONNECT BY PRIOR    c.organization_id_parent = c.organization_id_child
                            UNION
                            SELECT  organization_id,
                                    name
                            FROM    hr.hr_all_organization_units
                            WHERE   organization_id = "+ EmployeeOrgId.ToString() + @"
                            AND     NOT EXISTS(SELECT             c.organization_id_parent
                                                FROM                per_organization_structures_v a
                                                INNER JOIN          per_org_structure_versions_v b on a.organization_structure_id = b.organization_structure_id
                                                INNER JOIN          per_org_structure_elements_v c on b.org_structure_version_id = c.org_structure_version_id
                                                WHERE               SYSDATE BETWEEN b.date_from and nvl(b.date_to,'31-DEC-4712') 
                                                AND                 a.organization_structure_id <> 61
                                                START WITH          c.organization_id_child = " + EmployeeOrgId.ToString() + @"
                                                CONNECT BY PRIOR    c.organization_id_parent = c.organization_id_child))
                        where rownum = 1";

                long OrgId = _context.Database.SqlQuery<long>(sql).Single<long>();

                return OrgId;
            }
        }
    }
}
