using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using DBI.Data;
using DBI.Data.DataFactory;

namespace DBI.Web.EMS.Views.Modules.BudgetBidding
{
    public partial class umBudgetBiddingMain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!X.IsAjaxRequest)
            //{
                // PUT CODE IN HERE THAT YOU ONLY WANT TO EXECUTE ONE TIME AND NOT EVERYTIME SOMETHING IS CLICKED (POST BACK)
            //}
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

        protected void deLoadFiscalYears(object sender, StoreReadDataEventArgs e)
        {
            uxFiscalYearStore.DataSource = PA.AllFiscalYears();
        }

        protected void deLoadBudgetVersions(object sender, StoreReadDataEventArgs e)
        {
            uxVersionStore.DataSource = StaticLists.BudgetVersions();
        }
        
        protected void deLoadCorrectBudgetType(object sender, DirectEventArgs e)
        {
            long hierarchyID;
            long orgID;

            // Is an org selected, and is the org an org and not just a legal entity or hierarchy?
            try
            {
                string nodeID = uxOrgPanel.SelectedNodes[0].NodeID;
                char[] delimChars = { ':' };
                string[] selID = nodeID.Split(delimChars);
                hierarchyID = long.Parse(selID[0].ToString());
                orgID = long.Parse(selID[1].ToString());
            }
            
            catch (NullReferenceException)
            {
                return;
            }

            catch (IndexOutOfRangeException)
            {
                return;
            }

            // Is an org, year and version selected?
            string fiscalYear = uxFiscalYear.SelectedItem.Value;
            string verID = uxVersion.SelectedItem.Value;

            if (SYS_USER_ORGS.IsInOrg(SYS_USER_INFORMATION.UserID(User.Identity.Name), orgID) == true)
            {
                if (!string.IsNullOrEmpty(fiscalYear))
                {
                    if (!string.IsNullOrEmpty(verID))
                    {
                        string nodeName = uxOrgPanel.SelectedNodes[0].Text;
                        uxYearVersionTitle.Text = nodeName;
                        LoadBudget("umYearBudget.aspx?hierID=" + hierarchyID + "&orgID=" + orgID + "&orgName=" + nodeName + "&fiscalYear=" + fiscalYear + "&verID=" + verID);
                    }
                }
            }
        }

        protected void LoadBudget(string url)
        {
            uxBudgetPanel.Loader.SuspendScripting();
            uxBudgetPanel.Loader.Url = url;
            uxBudgetPanel.Loader.Mode = LoadMode.Frame;
            uxBudgetPanel.Loader.DisableCaching = true;
            uxBudgetPanel.LoadContent();
        }        
    }
}