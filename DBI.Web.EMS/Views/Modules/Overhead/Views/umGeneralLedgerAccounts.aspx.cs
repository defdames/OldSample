using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.Overhead.Views
{
    public partial class umGeneralLedgerAccounts : DBI.Core.Web.BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                if (!validateComponentSecurity("SYS.OverheadBudget.Security"))
                {
                    X.Redirect("~/Views/uxDefault.aspx");
                }               
            }
        }

        protected void deLoadActiveOrganizations(object sender, StoreReadDataEventArgs e)
        {

            var data = HR.ActiveOverheadOrganizations();
            int count;
            uxOrganizationSecurityStore.DataSource = GenericData.EnumerableFilterHeader<HR.ORGANIZATION>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
            e.Total = count;
        }

        protected void deViewOrganizationGlAccounts(object sender, DirectEventArgs e)
        {
            uxGLAccountRangeStore.Reload();
            uxShowGLRangeWindow.Enable();
        }

        protected void deLoadGLAccountRange(object sender, StoreReadDataEventArgs e)
        {
            long _organizationSelected = long.Parse(uxOrganizationsGridRowSelection.SelectedRecordID);

            using(Entities _context = new Entities())
            {
                var data = OVERHEAD_GL_ACCOUNT.OverheadGLAccountsByOrganization(_context, _organizationSelected);
                int count;
                uxOrganizationSecurityStore.DataSource = GenericData.ListFilterHeader<OVERHEAD_GL_ACCOUNT>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;

            }

        }

        protected void deShowRangeWindow(object sender, DirectEventArgs e)
        {

            string url = "/Views/Modules/Overhead/Views/umAddGlAccountRange.aspx?org_id=" + uxOrganizationsGridRowSelection.SelectedRecordID;
            Window win = new Window
            {
                ID = "uxShowAccountRangeWindow",
                Title = "General Ledger Account Range Filter",
                Height = 750,
                Width = 900,
                Modal = true,
                Resizable = true,
                CloseAction = CloseAction.Destroy,
                Loader = new ComponentLoader
                {
                    Mode = LoadMode.Frame,
                    DisableCaching = true,
                    Url = url,
                    AutoLoad = true,
                    LoadMask =
                    {
                        ShowMask = true
                    }
                }
            };

            win.Listeners.Close.Handler = "#{uxGLAccountRangeGridPanel}.getStore().load();";

            win.Render(this.Form);
            win.Show();
        }

        protected void deLoadOrgTree(object sender, NodeLoadEventArgs e)
        {
            // User clicked on legal entity or hierarchy
            if (e.NodeID.Contains(":") == false)
            {
                if (e.NodeID == "0")
                {
                    var data = HR.ActiveOverheadBudgetLegalEntities();
                    foreach (var view in data)
                    {
                        Node node = new Node();
                        node.Text = view.ORGANIZATION_NAME;
                        node.NodeID = view.ORGANIZATION_ID.ToString();
                        e.Nodes.Add(node);
                    }
                }

                else
                {
                    long nodeID = long.Parse(e.NodeID);
                    var data = HR.LegalEntityHierarchies().Where(a => a.ORGANIZATION_ID == nodeID);
                    foreach (var view in data)
                    {
                        Node node = new Node();
                        node.Text = view.HIERARCHY_NAME;
                        node.NodeID = string.Format("{0}:{1}", view.ORGANIZATION_STRUCTURE_ID.ToString(), view.ORGANIZATION_ID.ToString());
                        e.Nodes.Add(node);
                    }
                }
            }

            // User clicked on org
            else
            {
                char[] delimChars = { ':' };
                string[] selID = e.NodeID.Split(delimChars);
                long hierarchyID = long.Parse(selID[0].ToString());
                long orgID = long.Parse(selID[1].ToString());
                var data = HR.ActiveOrganizationsByHierarchy(hierarchyID, orgID);
                var OrgsList = SYS_USER_ORGS.GetUserOrgs(SYS_USER_INFORMATION.UserID(User.Identity.Name)).Select(x => x.ORG_ID);
                bool addNode;
                bool leafNode;
                bool colorNode;

                foreach (var view in data)
                {
                    if (view.HIER_LEVEL == 1)
                    {
                        addNode = false;
                        leafNode = true;
                        colorNode = false;
                        var nextData = HR.ActiveOrganizationsByHierarchy(hierarchyID, view.ORGANIZATION_ID);

                        // In this org?
                        if (SYS_USER_ORGS.IsInOrg(SYS_USER_INFORMATION.UserID(User.Identity.Name), view.ORGANIZATION_ID) == true)
                        {
                            addNode = true;
                            colorNode = true;
                        }

                        // In next org?
                        foreach (long allowedOrgs in OrgsList)
                        {
                            if (nextData.Select(x => x.ORGANIZATION_ID).Contains(allowedOrgs))
                            {
                                addNode = true;
                                leafNode = false;
                                break;
                            }
                        }

                        if (addNode == true)
                        {
                            Node node = new Node();
                            node.NodeID = string.Format("{0}:{1}", hierarchyID.ToString(), view.ORGANIZATION_ID.ToString());
                            node.Text = view.ORGANIZATION_NAME;
                            node.Leaf = leafNode;
                            if (colorNode == true)
                            {
                                node.Icon = Icon.Tick;
                            }
                            else
                            {
                                node.Icon = Icon.FolderGo;
                            }
                            e.Nodes.Add(node);
                        }
                    }
                }
            }
        }        

    }
}