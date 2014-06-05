using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBI.Data
{
    public class HR
    {

        /// <summary>
        /// Returns a list of all organizations in the system.
        /// </summary>
        /// <returns></returns>
        public static List<HR.ORGANIZATION> OrganizationList()
        {
            try
            {
                using (Entities _context = new Entities())
                {
                    string sql = @"select organization_id,name as organization_name, type, date_from, date_to from apps.hr_all_organization_units";
                    List<HR.ORGANIZATION> _data = _context.Database.SqlQuery<HR.ORGANIZATION>(sql).ToList();
                    return _data;
                }
            }
            catch (Exception)
            {   
                throw;
            }
        }

        /// <summary>
        /// Returns a list of only active organizations in oracle using the current system date and time.
        /// </summary>
        /// <returns></returns>
        public static List<HR.ORGANIZATION> ActiveOrganizationList()
        {
            try
            {
                List<HR.ORGANIZATION> _data = HR.OrganizationList().Where(x => x.DATE_FROM <= DateTime.Now && !x.DATE_TO.HasValue).ToList();
                return _data;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Returns a list of only active organizations by type
        /// </summary>
        /// <returns></returns>
        public static List<HR.ORGANIZATION> ActiveOrganizationsByType(string type)
        {
            try
            {
                using (Entities _context = new Entities())
                {
                    List<HR.ORGANIZATION> _data = HR.ActiveOrganizationList().Where(x => x.TYPE == type).ToList();
                    return _data;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// Returns a list of only active legal entity organizations, ordered by organization name
        /// </summary>
        /// <returns></returns>
        public static List<HR.ORGANIZATION> ActiveLegalEntityOrganizationList()
        {
            try
            {
                using (Entities _context = new Entities())
                {
                    List<HR.ORGANIZATION> _data = HR.ActiveOrganizationsByType("LE").OrderBy(x => x.ORGANIZATION_NAME).ToList();
                    return _data;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        /// <summary>
        /// returns a list of organizations by hierarchy and business unit or legal entity.
        /// </summary>
        /// <param name="hierarchyId"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public static List<ORGANIZATION_V1> ActiveOrganizationsByHierarchy(long hierarchyId, long organizationId)
        {
            try
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
                        AND                 a.organization_structure_id = " + hierarchyId.ToString() + @"
                        START WITH          c.organization_id_parent = " + organizationId.ToString() + @" AND a.organization_structure_id + 0 = " + hierarchyId.ToString() + @"
                        CONNECT BY PRIOR    c.organization_id_child = c.organization_id_parent AND a.organization_structure_id + 0 = " + hierarchyId.ToString() + @"
                        ORDER SIBLINGS BY   c.d_child_name";

                    List<ORGANIZATION_V1> _data = _context.Database.SqlQuery<ORGANIZATION_V1>(sql).Select(a => new ORGANIZATION_V1 { ORGANIZATION_ID = a.ORGANIZATION_ID, ORGANIZATION_NAME = a.ORGANIZATION_NAME, HIER_LEVEL = a.HIER_LEVEL }).ToList();
                    return _data;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



        public static List<ORGANIZATION_V1> ActiveOverheadOrganizationsByHierarchy(long hierarchyId, long organizationId)
        {
            try
            {
                using (Entities _context = new Entities())
                {
                    List<ORGANIZATION_V1> _data = ActiveOrganizationsByHierarchy(hierarchyId, organizationId);
                    foreach (var view in _data)
                    {
                        view.GL_ASSIGNED = (_context.OVERHEAD_GL_ACCOUNT.Where(a => a.OVERHEAD_ORG_ID == view.ORGANIZATION_ID).Count() > 0 ? "Active" : "No Accounts Found");
                    }
                    return _data;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public static List<HIERARCHY> HierarchyListByLegalEntity()
        {
            try
            {
                using (Entities _context = new Entities())
                {
                    string sql = @"select distinct a.organization_id_parent as organization_id,C.ORGANIZATION_STRUCTURE_ID,c.name as hierarchy_name, d.name as organization_name  from per_org_structure_elements_v a
                inner join per_org_structure_versions_v b on B.ORG_STRUCTURE_VERSION_ID = a.org_structure_version_id
                inner join per_organization_structures_v c on C.ORGANIZATION_STRUCTURE_ID = B.ORGANIZATION_STRUCTURE_ID
                inner join apps.hr_all_organization_units d on d.organization_id = a.organization_id_parent
                where a.organization_id_parent in (select organization_id from apps.hr_all_organization_units where type = 'LE' and ((sysdate between date_from and date_to) or (date_to is null)))
                order by 4,3";

                    var data = _context.Database.SqlQuery<HIERARCHY>(sql).ToList();
                    return data;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
          

        }

        /// <summary>
        /// Returns a list of legal entities from oracle that can have a budget because there is a budget type assigned to that businessunit
        /// </summary>
        /// <returns></returns>
        public static List<HR.ORGANIZATION> LegalEntitiesWithActiveOverheadBudgetTypes()
        {
            try
            {
                using (Entities _context = new Entities())
                {
                    List<HR.ORGANIZATION> _data = HR.ActiveLegalEntityOrganizationList();
                    List<HR.ORGANIZATION> _returnList = new List<HR.ORGANIZATION>();

                    foreach (HR.ORGANIZATION var in _data)
                    {
                        int count = DBI.Data.OVERHEAD_GL_ACCOUNT.CountOverheadGLAccountsAssignedByOrganizationId(var.ORGANIZATION_ID);
                        if (count > 0)
                        {
                            _returnList.Add(var);
                        }
                    }

                    return _returnList;
                }
            }
            catch (Exception)
            {

                throw;
            }

        }


        public class ORGANIZATION
        {
            public long ORGANIZATION_ID { get; set; }
            public string ORGANIZATION_NAME { get; set; }
            public string TYPE { get; set; }
            public DateTime DATE_FROM { get; set; }
            public DateTime? DATE_TO { get; set; }
        }

        public class ORGANIZATION_V1 : ORGANIZATION
        {
            public long HIER_LEVEL { get; set; }
            public string GL_ASSIGNED { get; set; }
        }

        public class HIERARCHY : ORGANIZATION
        {
            public string HIERARCHY_NAME { get; set; }
            public long ORGANIZATION_STRUCTURE_ID { get; set; }
        }

    }


}
