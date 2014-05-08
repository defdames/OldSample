using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data
{
    public class ORGANIZATIONS
    {
        /// <summary>
        /// Returns a list of legal entities from oracle, only selects what legal entities are current and active.
        /// </summary>
        /// <returns></returns>
        public static List<ORGANIZATION> legalEntities()
        {
            using (Entities _context = new Entities())
            {
                string sql = @"select organization_id,name as organization_name from apps.hr_all_organization_units where type = 'LE' and ((sysdate between date_from and date_to) or (date_to is null)) order by 2";
                List<ORGANIZATION> _returnList = _context.Database.SqlQuery<ORGANIZATION>(sql).ToList();
                return _returnList;
            }
        }

        public static List<ORGANIZATION_V1> organizationsByHierarchy(long hierarchyID, long organizationID)
        {
            using (Entities _context = new Entities())
            {
                string sql = @"SELECT              c.organization_id_child ORGANIZATION_ID,
                        c.d_child_name ORGANIZATION_NAME,
                        level as HIER_LEVEL
                        FROM                per_organization_structures_v a
                        INNER JOIN          per_org_structure_versions_v b on a.organization_structure_id = b.organization_structure_id
                        INNER JOIN          per_org_structure_elements_v c on b.org_structure_version_id = c.org_structure_version_id
                        INNER JOIN          apps.hr_all_organization_units haou on haou.organization_id = c.organization_id_child
                        WHERE               SYSDATE BETWEEN b.date_from and nvl(b.date_to,'31-DEC-4712')
                        AND                 a.organization_structure_id = " + hierarchyID.ToString() + @"
                        START WITH          c.organization_id_parent = " + organizationID.ToString() + @" AND a.organization_structure_id + 0 = " + hierarchyID.ToString() + @"
                        CONNECT BY PRIOR    c.organization_id_child = c.organization_id_parent AND a.organization_structure_id + 0 = " + hierarchyID.ToString() + @"
                        ORDER SIBLINGS BY   c.d_child_name";

                List<ORGANIZATION_V1> _data = _context.Database.SqlQuery<ORGANIZATION_V1>(sql).Select(a => new ORGANIZATION_V1 { ORGANIZATION_ID = a.ORGANIZATION_ID, ORGANIZATION_NAME = a.ORGANIZATION_NAME, HIER_LEVEL = a.HIER_LEVEL }).ToList();

                foreach (var view in _data)
                {
                    view.GL_ASSIGNED = (_context.OVERHEAD_GL_ACCOUNT.Where(a => a.OVERHEAD_ORG_ID == view.ORGANIZATION_ID).Count() > 0 ? "Active" : "No Accounts Found");
                }

                return _data;
            }

        }
 
        public class ORGANIZATION
        {
            public long ORGANIZATION_ID { get; set; }
            public string ORGANIZATION_NAME { get; set; }
        }

        public class ORGANIZATION_V1 : ORGANIZATION
        {
            public long HIER_LEVEL { get; set; }
            public string GL_ASSIGNED { get; set; }
        }
    
    }
}
