using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data
{
    public class ORACLE_HIERARCHIES
    {
        static string sql = @"select distinct a.organization_id_parent as organization_id,C.ORGANIZATION_STRUCTURE_ID,c.name as hierarchy_name, d.name as organization_name  from per_org_structure_elements_v a
                    inner join per_org_structure_versions_v b on B.ORG_STRUCTURE_VERSION_ID = a.org_structure_version_id
                    inner join per_organization_structures_v c on C.ORGANIZATION_STRUCTURE_ID = B.ORGANIZATION_STRUCTURE_ID
                    inner join apps.hr_all_organization_units d on d.organization_id = a.organization_id_parent
                    where a.organization_id_parent in (select organization_id from apps.hr_all_organization_units where type = 'LE' and ((sysdate between date_from and date_to) or (date_to is null)))
                    order by 4,3";

        public static List<ORGANIZATION> LegalEntities()
        {
            using (Entities _context = new Entities())
            {
                List<ORGANIZATION> data = _context.Database.SqlQuery<ORGANIZATION>(sql).Distinct().ToList();
                return data;
            }
        }

        public class ORGANIZATION
        {
            public long ORGANIZATION_ID { get; set; }
            public string ORGANIZATION_NAME { get; set; }
        }




    }
}
