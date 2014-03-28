using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.Overhead
{
    public partial class umSecurity : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LoadHierarchyTree(object sender, NodeLoadEventArgs e)
        {

            long[] ids = {64,63, 3065,6066, 8066,61,10066};
            Entities _context = new Entities();
            List<HIERARCHY_ID_V> _list = _context.HIERARCHY_ID_V.Where(h => ids.Contains(h.HIERARCHY_ID)).ToList();

                foreach (HIERARCHY_ID_V hier in _list)
                {
                    Node treeNode = new Node();
                    treeNode.Text = hier.NAME;
                    treeNode.NodeID = hier.HIERARCHY_ID.ToString();
                    treeNode.Leaf = true;
                    treeNode.Icon = Icon.Database;
                    e.Nodes.Add(treeNode);
                }
        }

        protected void deReadOrganizations(object sender, StoreReadDataEventArgs e)
        {
            long HierarchyID;

            TreeSelectionModel hierarchyTreeSelectionModel = uxHierarchyTreeSelectionModel;
            Boolean check = long.TryParse(hierarchyTreeSelectionModel.SelectedRecordID, out HierarchyID);

            if (check)
            {
                Entities _context = new Entities();
                var data = (from ov in _context.ORG_HIER_V.Where(c => c.HIERARCHY_ID == HierarchyID)
                            select new ORGANIZATION_VIEW { ORGANIZATION_ID = ov.ORG_ID_CHILD, ORGANIZATION_NAME = ov.ORG_HIER }).ToList();

                int count;
                uxOrganizationSecurityStore.DataSource = GenericData.EnumerableFilterHeader<ORGANIZATION_VIEW>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
            }
                
        }

        protected void deShowOrganizationsByHierarchy(object sender, DirectEventArgs e)
        {
            long HierarchyID;

            TreeSelectionModel hierarchyTreeSelectionModel = uxHierarchyTreeSelectionModel;
            Boolean check = long.TryParse(hierarchyTreeSelectionModel.SelectedRecordID, out HierarchyID);

            Entities _context = new Entities();
            var data = (from ov in _context.ORG_HIER_V.Where(c => c.HIERARCHY_ID == HierarchyID)
                        select new ORGANIZATION_VIEW { ORGANIZATION_ID = ov.ORG_ID_CHILD, ORGANIZATION_NAME = ov.ORG_HIER }).ToList();

            uxOrganizationSecurityStore.DataSource = data;
            uxOrganizationSecurityStore.DataBind();
        }

        public class ORGANIZATION_VIEW
        {
            public long ORGANIZATION_ID { get; set; }
            public string ORGANIZATION_NAME { get; set; }
        }
    }
}