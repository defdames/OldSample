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


        protected void deShowOrganizationsByHierarchy(object sender, DirectEventArgs e)
        {
            uxOrganizationSecurityStore.RemoveAll();
            uxOrganizationSecurityStore.ClearFilter();
            uxOrganizationSecurityStore.Reload();
        }

        protected void deReadOrganizationsByHierarchy(object sender, StoreReadDataEventArgs e)
        {
            long HierarchyID;

            TreeSelectionModel hierarchyTreeSelectionModel = uxHierarchyTreeSelectionModel;
            Boolean check = long.TryParse(hierarchyTreeSelectionModel.SelectedRecordID, out HierarchyID);

                using (Entities _context = new Entities())
                {
                    var data = (from ov in _context.ORG_HIER_V.Where(c => c.HIERARCHY_ID == HierarchyID)
                                select new ORGANIZATION_VIEW { ORGANIZATION_ID = ov.ORG_ID_CHILD, ORGANIZATION_NAME = ov.ORG_HIER }).ToList();
                    int count;
                    uxOrganizationSecurityStore.DataSource = GenericData.EnumerableFilterHeader<ORGANIZATION_VIEW>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                    e.Total = count;
                }
        }

        protected void deReadGLSecurityByOrganization(object sender, StoreReadDataEventArgs e)
        {
            long OrganizationID;

            RowSelectionModel selection = uxOrganizationSelectionModel;
            Boolean check = long.TryParse(selection.SelectedRecordID, out OrganizationID);

            if (OrganizationID > 0)
            {
                using (Entities _context = new Entities())
                {
                    var data = (from gl in _context.OVERHEAD_GL_ACCOUNT.Where(c => c.OVERHEAD_ORG_ID == OrganizationID)
                                join gla in _context.GL_ACCOUNTS_V on gl.CODE_COMBO_ID equals gla.CHART_OF_ACCOUNTS_ID
                                select new GL_ACCOUNT_VIEW
                                {
                                    OVERHEAD_GL_ID = gla.CHART_OF_ACCOUNTS_ID,
                                    CODE_COMBINATION_ID = gl.CODE_COMBO_ID,
                                    SEGMENT1 = gla.SEGMENT1,
                                    SEGMENT2 = gla.SEGMENT2,
                                    SEGMENT3 = gla.SEGMENT3,
                                    SEGMENT4 = gla.SEGMENT4,
                                    SEGMENT5 = gla.SEGMENT5,
                                    SEGMENT6 = gla.SEGMENT6,
                                    SEGMENT7 = gla.SEGMENT7,
                                    SEGMENT5DESC = gla.SEGMENT5_DESC
                                }).ToList();
                    int count;
                    uxOrganizationSecurityStore.DataSource = GenericData.EnumerableFilterHeader<GL_ACCOUNT_VIEW>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                    e.Total = count;
                }
            }
        }

        protected void deOrganizationSelect(object sender, DirectEventArgs e)
        {
            uxGlAccountSecurityStore.RemoveAll();
            uxGlAccountSecurityStore.ClearFilter();
            uxGlAccountSecurityStore.Reload();
        }

        protected void deShowGLAccounts(object sender, DirectEventArgs e)
        {
            long OrganizationID;

            RowSelectionModel selection = uxOrganizationSelectionModel;
            Boolean check = long.TryParse(selection.SelectedRecordID, out OrganizationID);

            Window win = new Window
            {
                ID = "uxGlAccounts",
                Title = "GL Accounts",
                Height = 650,
                Width = 750,
                Modal = true,
                CloseAction = CloseAction.Destroy,
                Loader = new ComponentLoader
                {
                    Mode = LoadMode.Frame,
                    DisableCaching = true,
                    Url = "/Views/Modules/Overhead/umAddGlAccount.aspx?orgID=" + OrganizationID,
                    AutoLoad = true,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win.Listeners.Close.Handler = "#{uxGlAccountSecurityGrid}.getStore().load();";

            win.Render(this.Form);
            win.Show();
        }

        public class ORGANIZATION_VIEW
        {
            public long ORGANIZATION_ID { get; set; }
            public string ORGANIZATION_NAME { get; set; }
        }

        public class GL_ACCOUNT_VIEW
        {
            public long OVERHEAD_GL_ID {get; set;}
            public long CODE_COMBINATION_ID {get; set;}
            public string SEGMENT1 {get; set;}
            public string SEGMENT2 {get; set;}
            public string SEGMENT3 {get; set;}
            public string SEGMENT4 {get; set;}
            public string SEGMENT5 {get; set;}
            public string SEGMENT6 {get; set;}
            public string SEGMENT7 {get; set;}
            public string SEGMENT5DESC {get; set;}


        }


    }
}