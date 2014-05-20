using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using DBI.Data;
using DBI.Data.Oracle.HR;

namespace DBI.Web.EMS.Views.Modules.BudgetBidding
{
    public partial class umBudgetBiddingMain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!X.IsAjaxRequest)
            {
                // PUT CODE IN HERE THAT YOU ONLY WANT TO EXECUTE ONE TIME AND NOT EVERYTIME SOMETHING IS CLICKED (POST BACK)
            }
        }

        protected void deLoadFiscalYears(object sender, StoreReadDataEventArgs e)
        {
            using (Entities context = new Entities())
            {
                List<object> dataSource;
                dataSource = (from d in context.PA_PERIODS_ALL
                              select new { END_DATE = d.END_DATE.Year }).Distinct().OrderBy(d => d.END_DATE).ToList<object>();
                int count;
                uxFiscalYearStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], dataSource, out count);
                e.Total = count;
            }
        }

        protected void deLoadBudgetVersions(object sender, StoreReadDataEventArgs e)
        {
            List<BudVerStruct> list = new List<BudVerStruct> 
                {
                    new BudVerStruct(1, "Bid"),
                    new BudVerStruct(2, "First Draft"),
                    new BudVerStruct(3, "Final Draft"),
                    new BudVerStruct(4, "1st Reforecast") ,
                    new BudVerStruct(5, "2nd Reforecast"),
                    new BudVerStruct(6, "3rd Reforecast"),
                    new BudVerStruct(7, "4th Reforecast") 
                };
            uxVersionStore.DataSource = list;
        }

        class BudVerStruct  // DELETE WHEN GETTING DATA FROM CORRECT SOURCE
        {
            public long VER_ID { get; set; }
            public string BUD_VERSION { get; set; }
            
            public BudVerStruct(long id, string budName)
            {
                VER_ID = id;
                BUD_VERSION = budName;
            }
        }

        protected void deLoadCorrectBudgetType(object sender, DirectEventArgs e)
        {              
            long orgID;

            // Is an org selected, and is the org an org and not just a legal entity or hierarchy?
            try
            {
                string nodeID = uxOrgPanel.SelectedNodes[0].NodeID;
                char[] delimChars = { ':' };
                string[] selID = nodeID.Split(delimChars);
                long hierarchyID = long.Parse(selID[0].ToString());
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
            string version = uxVersion.SelectedItem.Value;

            if (SYS_USER_ORGS.IsInOrg(SYS_USER_INFORMATION.UserID(User.Identity.Name), orgID) == true)
            {
                if (!string.IsNullOrEmpty(fiscalYear))
                {
                    if (!string.IsNullOrEmpty(version))
                    {
                        LoadBudget("umYearBudget.aspx?orgID=" + orgID + "&fiscalYear=" + fiscalYear + "&version=" + version);
                        string nodeName = uxOrgPanel.SelectedNodes[0].Text;
                        uxYearVersionTitle.Text = nodeName;
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

        protected void deLoadOrgTree(object sender, NodeLoadEventArgs e)
        {
            // User clicked on legal entity or hierarchy
            if (e.NodeID.Contains(":") == false)
            {
                if (e.NodeID == "0")
                {
                    var data = Organizations.legalEntitiesWithActiveBudgetTypes();
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
                    var data = Organizations.hierarchiesByBusinessUnit().Where(a => a.ORGANIZATION_ID == nodeID);
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
                var data = Organizations.organizationsByHierarchy(hierarchyID, orgID);
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
                        var nextData = Organizations.organizationsByHierarchy(hierarchyID, view.ORGANIZATION_ID);

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