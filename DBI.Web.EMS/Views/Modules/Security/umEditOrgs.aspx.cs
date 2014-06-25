using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Security.Claims;
using DBI.Core.Web;
using DBI.Core.Security;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.Security
{
    public partial class umEditOrgs : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void deReadOrganizationsByHierarchy(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                string _selectedRecordID = uxHierarchyTreeSelectionModel.SelectedRecordID;

                if (_selectedRecordID != "0" && _selectedRecordID.Contains(":"))
                {
                    char[] _delimiterChars = { ':' };
                    string[] _selectedID = _selectedRecordID.Split(_delimiterChars);
                    long _hierarchyID = long.Parse(_selectedID[1].ToString());
                    long _organizationID = long.Parse(_selectedID[0].ToString());

                    var data = HR.ActiveOrganizationsByHierarchy(_hierarchyID, _organizationID);

                    uxOrganizationSecurityStore.DataSource = data;
                }
            }
        }

        protected void deAddOrganizations(object sender, DirectEventArgs e)
        {
            var OrgsToAdd = JSON.Deserialize<List<HIERARCHY_TREEVIEW>>(e.ExtraParams["OrgsToAdd"]);
            var OrgsAdded = JSON.Deserialize<List<HIERARCHY_TREEVIEW>>(e.ExtraParams["OrgsAdded"]);

            foreach (var Org in OrgsAdded)
            {
                if (OrgsToAdd.Exists(x => x.ORGANIZATION_ID == Org.ORGANIZATION_ID))
                {
                    OrgsToAdd.Remove(OrgsToAdd.Find(x => x.ORGANIZATION_ID == Org.ORGANIZATION_ID));
                }
            }
            uxAssignedOrgsStore.Insert(0, OrgsToAdd);
        }

        protected void LoadHierarchyTree(object sender, NodeLoadEventArgs e)
        {
            Entities _context = new Entities();

            string sql = @"select distinct a.organization_id_parent as organization_id,C.ORGANIZATION_STRUCTURE_ID,c.name as hierarchy_name, d.name as organization_name  from per_org_structure_elements_v a
            inner join per_org_structure_versions_v b on B.ORG_STRUCTURE_VERSION_ID = a.org_structure_version_id
            inner join per_organization_structures_v c on C.ORGANIZATION_STRUCTURE_ID = B.ORGANIZATION_STRUCTURE_ID
            inner join apps.hr_all_organization_units d on d.organization_id = a.organization_id_parent
            where a.organization_id_parent in (select organization_id from apps.hr_all_organization_units where type = 'LE' and ((sysdate between date_from and date_to) or (date_to is null)))
            order by 4,3";

            //Load LEs
            if (e.NodeID == "0")
            {
                var data = _context.Database.SqlQuery<HIERARCHY_TREEVIEW>(sql).Select(a => new { a.ORGANIZATION_ID, a.ORGANIZATION_NAME }).Distinct().ToList();

                //Build the treepanel
                foreach (var view in data)
                {
                    //Create the Hierarchy Levels
                    Node node = new Node();
                    node.Text = view.ORGANIZATION_NAME;
                    node.NodeID = view.ORGANIZATION_ID.ToString();
                    e.Nodes.Add(node);
                }
            }
            else
            {
                long nodeID = long.Parse(e.NodeID);

                //Load Hierarchies for LE
                var data = _context.Database.SqlQuery<HIERARCHY_TREEVIEW>(sql).Where(a => a.ORGANIZATION_ID == nodeID).ToList();

                //Build the treepanel
                foreach (var view in data)
                {
                    //Create the Hierarchy Levels
                    Node node = new Node();
                    node.Text = view.HIERARCHY_NAME;
                    node.NodeID = string.Format("{0}:{1}", view.ORGANIZATION_ID.ToString(), view.ORGANIZATION_STRUCTURE_ID.ToString());
                    node.Leaf = true;
                    e.Nodes.Add(node);
                }

            }
        }

        protected void deLoadOrgs(object sender, StoreReadDataEventArgs e)
        {
            //long UserId = long.Parse(Request.QueryString["SelectedUser"]);
            long UserId = long.Parse(Authentication.GetClaimValue("UserId", User as ClaimsPrincipal));
            using (Entities _context = new Entities())
            {
                List<HIERARCHY_TREEVIEW> OrgsList = (from h in _context.SYS_USER_ORGS
                                                     join o in _context.ORG_HIER_V on h.ORG_ID equals o.ORG_ID
                                                     where h.USER_ID == UserId
                                                     select new HIERARCHY_TREEVIEW { ORGANIZATION_NAME = o.ORG_HIER, ORGANIZATION_ID = h.ORG_ID }).ToList();
                uxAssignedOrgsStore.DataSource = OrgsList;
            }
        }
    }
    public class HIERARCHY_TREEVIEW
    {
        public string ORGANIZATION_NAME { get; set; }
        public string HIERARCHY_NAME { get; set; }
        public long ORGANIZATION_STRUCTURE_ID { get; set; }
        public long ORGANIZATION_ID { get; set; }
    }
}