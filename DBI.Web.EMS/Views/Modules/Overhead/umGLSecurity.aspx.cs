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
    public partial class umGLSecurity : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

    
        protected void LoadOrganizationTree(object sender, NodeLoadEventArgs e)
        {

            Entities _context = new Entities();

                string selectedRecordID = uxHierarchyTreeSelectionModel.SelectedRecordID;

                if (selectedRecordID != "") 
                {
                    char[] delimiterChars = { ':' };
                    string[] selectedID = selectedRecordID.Split(delimiterChars);
                    long hierarchyID = long.Parse(selectedID[1].ToString());
                    long organizationID = long.Parse(selectedID[0].ToString());

                    if (e.NodeID != "0")
                    {
                        organizationID = long.Parse(e.NodeID);
                    }

                    string sql = @"SELECT              c.organization_id_child ORGANIZATION_ID,
                        c.d_child_name ORGANIZATION_NAME,
                        level as HIER_LEVEL
                        FROM                per_organization_structures_v a
                        INNER JOIN          per_org_structure_versions_v b on a.organization_structure_id = b.organization_structure_id
                        INNER JOIN          per_org_structure_elements_v c on b.org_structure_version_id = c.org_structure_version_id
                        INNER JOIN          apps.hr_all_organization_units haou on haou.organization_id = c.organization_id_child
                        WHERE               SYSDATE BETWEEN b.date_from and nvl(b.date_to,'31-DEC-4712')
                        AND                 level = 1
                        AND                 a.organization_structure_id = " + hierarchyID.ToString() + @"
                        START WITH          c.organization_id_parent = " + organizationID.ToString() + @" AND a.organization_structure_id + 0 = " + hierarchyID.ToString() + @"
                        CONNECT BY PRIOR    c.organization_id_child = c.organization_id_parent AND a.organization_structure_id + 0 = " + hierarchyID.ToString() + @"
                        ORDER SIBLINGS BY   c.d_child_name";


                    var data = _context.Database.SqlQuery<ORGANIZATION_VIEW>(sql).Select(a => new ORGANIZATION_VIEW { ORGANIZATION_ID = a.ORGANIZATION_ID, ORGANIZATION_NAME = a.ORGANIZATION_NAME, HIER_LEVEL = a.HIER_LEVEL }).ToList();

                    foreach (var view in data)
                    {
                        view.GL_ASSIGNED = (_context.OVERHEAD_GL_ACCOUNT.Where(a => a.OVERHEAD_ORG_ID == view.ORGANIZATION_ID).Count() > 0 ? "Active" : "No Accounts Found");
                    }

                        //Build the treepanel
                        foreach (var view in data)
                        {
                            //Create the Hierarchy Levels
                            Node node = new Node();
                            //node.Text = view.ORGANIZATION_NAME;
                            node.NodeID = view.ORGANIZATION_ID.ToString();
                            node.CustomAttributes.Add(new ConfigItem { Name = "ORGANIZATION_NAME", Value = view.ORGANIZATION_NAME, Mode = ParameterMode.Value });
                            node.CustomAttributes.Add(new ConfigItem { Name = "GL_ASSIGNED", Value = view.GL_ASSIGNED, Mode = ParameterMode.Value });
                            e.Nodes.Add(node);
                        }

                }
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
                    node.NodeID = string.Format("{0}:{1}",view.ORGANIZATION_ID.ToString(),view.ORGANIZATION_STRUCTURE_ID.ToString());
                    node.Leaf = true;
                    e.Nodes.Add(node);
                }

            }
        }

        protected void deShowOrganizationsByHierarchy(object sender, DirectEventArgs e)
        {
            string selectedRecordID = uxHierarchyTreeSelectionModel.SelectedRecordID;
            if (selectedRecordID != "0" && selectedRecordID.Contains(":"))
            {

                char[] delimiterChars = { ':' };
                string[] selectedID = selectedRecordID.Split(delimiterChars);
                long hierarchyID = long.Parse(selectedID[1].ToString());
                long organizationID = long.Parse(selectedID[0].ToString());
                
                uxOrganizationTreeGridStore.GetRootNode().Reload();
                uxOrganizationTreeGrid.Refresh();

                uxGlAccountSecurityStore.RemoveAll();
                uxGlAccountSecurityGridFilter.ClearFilter();
                uxGlAccountSecurityGrid.Refresh();
            }
        }

        protected void deReadGLSecurityByOrganization(object sender, StoreReadDataEventArgs e)
        {
            long OrganizationID;

            RowSelectionModel selection = TreeSelectionModel1;
            Boolean check = long.TryParse(selection.SelectedRecordID, out OrganizationID);

            if (OrganizationID > 0)
            {
                using (Entities _context = new Entities())
                {
                    var data = (from gl in _context.OVERHEAD_GL_ACCOUNT.Where(c => c.OVERHEAD_ORG_ID == OrganizationID)
                                join gla in _context.GL_ACCOUNTS_V on gl.CODE_COMBO_ID equals gla.CODE_COMBINATION_ID
                                select new GL_ACCOUNT_VIEW
                                {
                                    OVERHEAD_GL_ID = (long)gl.OVERHEAD_GL_ID,
                                    CODE_COMBINATION_ID = gla.CODE_COMBINATION_ID,
                                    SEGMENT1 = gla.SEGMENT1,
                                    SEGMENT2 = gla.SEGMENT2,
                                    SEGMENT3 = gla.SEGMENT3,
                                    SEGMENT4 = gla.SEGMENT4,
                                    SEGMENT5 = gla.SEGMENT5,
                                    SEGMENT6 = gla.SEGMENT6,
                                    SEGMENT7 = gla.SEGMENT7,
                                    SEGMENT5DESC = gla.SEGMENT5_DESC,
                                    SEGMENT1DESC = gla.SEGMENT1_DESC,
                                    SEGMENT2DESC = gla.SEGMENT2_DESC,
                                    SEGMENT3DESC = gla.SEGMENT3_DESC,
                                    SEGMENT4DESC = gla.SEGMENT4_DESC,
                                    SEGMENT6DESC = gla.SEGMENT5_DESC,
                                    SEGMENT7DESC = gla.SEGMENT7_DESC
                                }).ToList();
                    int count;
                    uxGlAccountSecurityStore.DataSource = GenericData.EnumerableFilterHeader<GL_ACCOUNT_VIEW>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                    e.Total = count;
                }
            }
        }

        protected void deOrganizationSelect(object sender, DirectEventArgs e)
        {
            uxGlAccountSecurityStore.RemoveAll();
            uxGlAccountSecurityGridFilter.ClearFilter();
            uxGlAccountSecurityStore.Reload();
        }

        protected void deShowGLAccounts(object sender, DirectEventArgs e)
        {
            long OrganizationID;

            RowSelectionModel selection = TreeSelectionModel1;
            Boolean checkOrganization = long.TryParse(selection.SelectedRecordID, out OrganizationID);

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

            win.Listeners.Close.Handler = "#{uxGlAccountSecurityGrid}.getStore().load();#{uxOrganizationTreeGrid}.reloadNode(" + OrganizationID + ");";

            win.Render(this.Form);
            win.Show();
        }

        protected void deDeleteGLAccounts(object sender, DirectEventArgs e)
        {
            long OrganizationID;

            RowSelectionModel selection = TreeSelectionModel1;
            Boolean checkOrganization = long.TryParse(selection.SelectedRecordID, out OrganizationID);

            RowSelectionModel model = uxGlAccountSecurityGridSelectionModel;

            Entities _context = new Entities();

            foreach (SelectedRow row in model.SelectedRows)
            {
                long recordID = long.Parse(row.RecordID);

                OVERHEAD_GL_ACCOUNT account = _context.OVERHEAD_GL_ACCOUNT.Where(a => a.OVERHEAD_GL_ID == recordID).SingleOrDefault();
                GenericData.Delete<OVERHEAD_GL_ACCOUNT>(account);
            }

            int recordCount = _context.OVERHEAD_GL_ACCOUNT.Where(a => a.OVERHEAD_ORG_ID == OrganizationID).Count();

            if (recordCount == 0)
            {
                uxOrganizationTreeGridStore.GetRootNode().Reload();
                uxOrganizationTreeGrid.Refresh();
            }

            uxGlAccountSecurityStore.RemoveAll();
            uxGlAccountSecurityStore.ClearFilter();
            uxGlAccountSecurityStore.Reload();
        }

        public class HIERARCHY_TREEVIEW
        {
            public string ORGANIZATION_NAME {get; set;}
            public string HIERARCHY_NAME {get; set;}
            public long ORGANIZATION_STRUCTURE_ID {get; set;}
            public long ORGANIZATION_ID {get; set;}
        }

        public class ORGANIZATION_VIEW
        {
            public long HIER_LEVEL { get; set; }
            public string GL_ASSIGNED { get; set; }
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
            public string SEGMENT1DESC {get; set;}
            public string SEGMENT2DESC { get; set; }
            public string SEGMENT3DESC { get; set; }
            public string SEGMENT4DESC { get; set; }
            public string SEGMENT5DESC { get; set; }
            public string SEGMENT6DESC { get; set; }
            public string SEGMENT7DESC { get; set; }

        }


    }
}