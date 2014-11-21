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
        public static List<HR.ORGANIZATION> Organizations()
        {
            
                using (Entities _context = new Entities())
                {
                    string sql = @"select organization_id,name as organization_name, type, date_from, date_to from apps.hr_all_organization_units";
                    List<HR.ORGANIZATION> _data = _context.Database.SqlQuery<HR.ORGANIZATION>(sql).ToList();
                    return _data;
                }
           
        }

        /// <summary>
        /// Returns organization information by organization id
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public static HR.ORGANIZATION Organization(long organizationId)
        {
           
                HR.ORGANIZATION _data = Organizations().Where(x => x.ORGANIZATION_ID == organizationId).SingleOrDefault();
                return _data;
           
        }

        /// <summary>
        /// Returns a list of only active organizations in oracle using the current system date and time.
        /// </summary>
        /// <returns></returns>
        public static List<HR.ORGANIZATION> ActiveOrganizations()
        {
           
                List<HR.ORGANIZATION> _data = HR.Organizations().Where(x => x.DATE_FROM <= DateTime.Now && !x.DATE_TO.HasValue).ToList();
                return _data;
           
        }

        /// <summary>
        /// Returns a list of only active organizations by type
        /// </summary>
        /// <returns></returns>
        public static List<HR.ORGANIZATION> ActiveOrganizations(string type)
        {
           
                using (Entities _context = new Entities())
                {
                    List<HR.ORGANIZATION> _data = HR.ActiveOrganizations().Where(x => x.TYPE == type).ToList();
                    return _data;
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
                

                using (Entities _context = new Entities())
                {
                    string sql = @"select    c.organization_id_child ORGANIZATION_ID,
                        c.d_child_name ORGANIZATION_NAME,
                        level as HIER_LEVEL,
                        haou.type,
                        haou.date_from,
                        haou.date_to,
                        ' ' as organization_status
                        FROM                per_organization_structures_v a
                        INNER JOIN          per_org_structure_versions_v b on a.organization_structure_id = b.organization_structure_id
                        INNER JOIN          per_org_structure_elements_v c on b.org_structure_version_id = c.org_structure_version_id
                        INNER JOIN          apps.hr_all_organization_units haou on haou.organization_id = c.organization_id_child
                        WHERE               SYSDATE BETWEEN b.date_from and nvl(b.date_to,'31-DEC-4712')
                        AND                 a.organization_structure_id = " + hierarchyId.ToString() + @"
                        START WITH          c.organization_id_parent = " + organizationId.ToString() + @" AND a.organization_structure_id + 0 = " + hierarchyId.ToString() + @"
                        CONNECT BY PRIOR    c.organization_id_child = c.organization_id_parent AND a.organization_structure_id + 0 = " + hierarchyId.ToString() + @"
                        ORDER SIBLINGS BY   c.d_child_name";

                    List<ORGANIZATION_V1> _data = _context.Database.SqlQuery<ORGANIZATION_V1>(sql).ToList();

                    return _data;
                }
        }

        /// <summary>
        /// returns a list of organizations by hierarchy and business unit or legal entity.
        /// </summary>
        /// <param name="hierarchyId"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public static List<ORGANIZATION> OrganizationsByHierarchy(long hierarchyId, long organizationId)
        {

            using (Entities _context = new Entities())
            {
                string sql = @"select    c.organization_id_child ORGANIZATION_ID,
                        c.d_child_name ORGANIZATION_NAME,
                        haou.type,
                        haou.date_from,
                        haou.date_to
                        FROM                per_organization_structures_v a
                        INNER JOIN          per_org_structure_versions_v b on a.organization_structure_id = b.organization_structure_id
                        INNER JOIN          per_org_structure_elements_v c on b.org_structure_version_id = c.org_structure_version_id
                        INNER JOIN          apps.hr_all_organization_units haou on haou.organization_id = c.organization_id_child
                        WHERE               SYSDATE BETWEEN b.date_from and nvl(b.date_to,'31-DEC-4712')
                        AND                 a.organization_structure_id = " + hierarchyId.ToString() + @"
                        START WITH          c.organization_id_parent = " + organizationId.ToString() + @" AND a.organization_structure_id + 0 = " + hierarchyId.ToString() + @"
                        CONNECT BY PRIOR    c.organization_id_child = c.organization_id_parent AND a.organization_structure_id + 0 = " + hierarchyId.ToString() + @"
                        ORDER SIBLINGS BY   c.d_child_name";

                List<ORGANIZATION> _data = _context.Database.SqlQuery<ORGANIZATION>(sql).ToList();
                return _data;
            }
        }


        public static IQueryable<ORGANIZATION_V1> ActiveOrganizationsByHierarchy(long hierarchyId, long organizationId, Entities _context)
        {
                string sql = @"SELECT              c.organization_id_child ORGANIZATION_ID,
                        c.d_child_name ORGANIZATION_NAME,
                        level as HIER_LEVEL,
                        haou.type,
                        haou.date_from,
                        haou.date_to,
                        ' ' as organization_status
                        FROM                per_organization_structures_v a
                        INNER JOIN          per_org_structure_versions_v b on a.organization_structure_id = b.organization_structure_id
                        INNER JOIN          per_org_structure_elements_v c on b.org_structure_version_id = c.org_structure_version_id
                        INNER JOIN          apps.hr_all_organization_units haou on haou.organization_id = c.organization_id_child
                        WHERE               SYSDATE BETWEEN b.date_from and nvl(b.date_to,'31-DEC-4712')
                        AND                 a.organization_structure_id = " + hierarchyId.ToString() + @"
                        START WITH          c.organization_id_parent = " + organizationId.ToString() + @" AND a.organization_structure_id + 0 = " + hierarchyId.ToString() + @"
                        CONNECT BY PRIOR    c.organization_id_child = c.organization_id_parent AND a.organization_structure_id + 0 = " + hierarchyId.ToString() + @"
                        ORDER SIBLINGS BY   c.d_child_name";

                IQueryable<ORGANIZATION_V1> _data = _context.Database.SqlQuery<ORGANIZATION_V1>(sql).AsQueryable();
                return _data;
        }

        /// <summary>
        /// Returns a list of valid hierarchies in the system, it will return a list of hierarcies by legal entity.
        /// </summary>
        /// <returns></returns>
        public static List<HIERARCHY> LegalEntityHierarchies()
        {

                using (Entities _context = new Entities())
                {
                    string sql = @"select distinct d.date_from, d.date_to, d.type, c.name as HIERARCHY_NAME, C.ORGANIZATION_STRUCTURE_ID AS ORGANIZATION_STRUCTURE_ID, a.organization_id_parent as ORGANIZATION_ID,d.name as ORGANIZATION_NAME   from per_org_structure_elements_v a
                inner join per_org_structure_versions_v b on B.ORG_STRUCTURE_VERSION_ID = a.org_structure_version_id
                inner join per_organization_structures_v c on C.ORGANIZATION_STRUCTURE_ID = B.ORGANIZATION_STRUCTURE_ID
                inner join apps.hr_all_organization_units d on d.organization_id = a.organization_id_parent
                where a.organization_id_parent in (select organization_id from apps.hr_all_organization_units where type = 'LE' and ((sysdate between date_from and date_to) or (date_to is null)))
                order by 4,3";

                    var data = _context.Database.SqlQuery<HIERARCHY>(sql).ToList();
                    return data;
                }
          
        }

        /// <summary>
        /// Shows organizations in oracle and their overhead status based on the organization profile option
        /// </summary>
        /// <param name="hierarchyId"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public static List<HR.ORGANIZATION_V1> OverheadOrganizationStatusByHierarchy(long hierarchyId, long organizationId)
        {
                    List<HR.ORGANIZATION_V1> _data = ActiveOrganizationsByHierarchy(hierarchyId, organizationId);
                    List<SYS_ORG_PROFILE_OPTIONS> _odata = new List<SYS_ORG_PROFILE_OPTIONS>();


                    SYS_PROFILE_OPTIONS _pOption = SYS_PROFILE_OPTIONS.ProfileOption("OverheadBudgetOrganization");
                     using (Entities _context = new Entities())
                    {
                        _odata = _context.SYS_ORG_PROFILE_OPTIONS.Where(x => x.PROFILE_OPTION_ID == _pOption.PROFILE_OPTION_ID).ToList();
                    }
                   
                    foreach (HR.ORGANIZATION_V1 _item in _data)
                    { 
                        SYS_ORG_PROFILE_OPTIONS _option = _odata.Where(x => x.ORGANIZATION_ID == _item.ORGANIZATION_ID).SingleOrDefault();
                        if (_option != null)
                        {
                            _item.ORGANIZATION_STATUS = (_option.PROFILE_VALUE == "Y") ? "Active" : "InActive";
                        }
                        else
                        {
                            _item.ORGANIZATION_STATUS = "InActive";
                        }
                    }

                    return _data;
        }

        /// <summary>
        /// Shows active organization in oracle and that are being used for the overhead system based on the organization profile option
        /// </summary>
        /// <param name="hierarchyId"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public static List<HR.ORGANIZATION> ActiveOverheadOrganizations()
        {
            SYS_PROFILE_OPTIONS profileOption = SYS_PROFILE_OPTIONS.ProfileOption("OverheadBudgetOrganization");
            List<SYS_ORG_PROFILE_OPTIONS.SYS_ORG_PROFILE_OPTIONS_V> _data =  SYS_ORG_PROFILE_OPTIONS.OrganizationProfileOptions().Where(x => x.PROFILE_OPTION_ID == profileOption.PROFILE_OPTION_ID && x.PROFILE_VALUE == "Y").ToList();
            List<HR.ORGANIZATION> _returnData = new List<HR.ORGANIZATION>();

            foreach (var orgOption in _data)
            {
                HR.ORGANIZATION _org = new HR.ORGANIZATION();
                _org.ORGANIZATION_ID = orgOption.ORGANIZATION_ID;
                _org.ORGANIZATION_NAME = HR.Organization(orgOption.ORGANIZATION_ID).ORGANIZATION_NAME;
                _returnData.Add(_org);
            }

            return _returnData;          
        }


        /// <summary>
        /// Returns a list of legal entities from oracle that can have a budget because there is a budget type assigned to that businessunit
        /// </summary>
        /// <returns></returns>
        public static List<HR.ORGANIZATION> ActiveOverheadBudgetLegalEntities()
        {
           
                using (Entities _context = new Entities())
                {
                    List<HR.ORGANIZATION> _data = HR.ActiveOrganizations("LE");
                    List<HR.ORGANIZATION> _returnList = new List<HR.ORGANIZATION>();

                    foreach (HR.ORGANIZATION var in _data)
                    {
                        int count = GL.BudgetTypes(var.ORGANIZATION_ID).Count();
                        if (count > 0)
                        {
                            _returnList.Add(var);
                        }
                    }

                    return _returnList;
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
            public string ORGANIZATION_STATUS { get; set; }
        }

        public class HIERARCHY : ORGANIZATION
        {
            public string HIERARCHY_NAME { get; set; }
            public long ORGANIZATION_STRUCTURE_ID { get; set; }
        }
    }
}
