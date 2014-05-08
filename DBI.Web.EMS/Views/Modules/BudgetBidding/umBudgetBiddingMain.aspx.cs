using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using DBI.Data;

using System.Security.Claims;

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

        protected void deReadFiscalYears(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                List<object> dataSource;
                dataSource = (from d in _context.PA_PERIODS_ALL
                              select new { END_DATE = d.END_DATE.Year }).Distinct().OrderBy(d => d.END_DATE).ToList<object>();
                int count;
                uxFiscalYearStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], dataSource, out count);
                e.Total = count;
            }
        }

        protected void deReadBudgetVersions(object sender, StoreReadDataEventArgs e)
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
            //string treeNode1 = e.ExtraParams["treeNode"];
            //X.Msg.Alert("Test", treeNode1).Show();

            if (uxOrgPanel.SelectedNodes != null)
            {
                if (!string.IsNullOrEmpty(uxFiscalYear.SelectedItem.Value))
                {
                    if (!string.IsNullOrEmpty(uxVersion.SelectedItem.Value))
                    {
                        LoadBudget("umYearBudget.aspx");
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

        public class HIERARCHY_TREEVIEW
        {
            public string ORGANIZATION_NAME { get; set; }
            public string HIERARCHY_NAME { get; set; }
            public long ORGANIZATION_STRUCTURE_ID { get; set; }
            public long ORGANIZATION_ID { get; set; }
        }

        protected void LoadOrgTree(object sender, NodeLoadEventArgs e)
        {
            //X.Msg.Alert("Node", e.NodeID).Show();
            if (e.NodeID.Contains(":") == false)
            {
                Entities _context = new Entities();
                string sql = @"select distinct a.organization_id_parent as organization_id,C.ORGANIZATION_STRUCTURE_ID,c.name as hierarchy_name, d.name as organization_name  from per_org_structure_elements_v a
                    inner join per_org_structure_versions_v b on B.ORG_STRUCTURE_VERSION_ID = a.org_structure_version_id
                    inner join per_organization_structures_v c on C.ORGANIZATION_STRUCTURE_ID = B.ORGANIZATION_STRUCTURE_ID
                    inner join apps.hr_all_organization_units d on d.organization_id = a.organization_id_parent
                    where a.organization_id_parent in (select organization_id from apps.hr_all_organization_units where type = 'LE' and ((sysdate between date_from and date_to) or (date_to is null)))
                    order by 4,3";

                if (e.NodeID == "0")
                {
                    var data = ORGANIZATIONS.legalEntities();
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
                    var data = _context.Database.SqlQuery<HIERARCHY_TREEVIEW>(sql).Where(a => a.ORGANIZATION_ID == nodeID).ToList();
                    foreach (var view in data)
                    {
                        Node node = new Node();
                        node.Text = view.HIERARCHY_NAME;
                        node.NodeID = string.Format("{0}:{1}", view.ORGANIZATION_ID.ToString(), view.ORGANIZATION_STRUCTURE_ID.ToString());
                        e.Nodes.Add(node);
                    }
                }
            }

            else
            {
                char[] _delimiterChars = { ':' };
                string[] _selectedID = e.NodeID.Split(_delimiterChars);
                long _hierarchyID = long.Parse(_selectedID[1].ToString());
                long _organizationID = long.Parse(_selectedID[0].ToString());
                var data = ORGANIZATIONS.organizationsByHierarchy(_hierarchyID, _organizationID);

                foreach (var view in data)
                {
                    if (view.HIER_LEVEL == 1)
                    {
                        Node node = new Node();
                        node.Text = view.ORGANIZATION_NAME;
                        node.NodeID = string.Format("{0}:{1}", view.ORGANIZATION_ID.ToString(), _hierarchyID.ToString());

                        // Is it a leaf org?
                        var data1 = ORGANIZATIONS.organizationsByHierarchy(_hierarchyID, view.ORGANIZATION_ID);
                        node.Leaf = true;
                        foreach (var view1 in data1)
                        {
                            node.Leaf = false;
                            break;
                        }

                        // Allowed org based on security?            
                        List<long> OrgsList = SYS_USER_ORGS.GetUserOrgs(SYS_USER_INFORMATION.UserID(User.Identity.Name)).Select(x => x.ORG_ID).ToList();
                        node.Icon = Icon.BorderNone;
                        if (OrgsList.Contains(view.ORGANIZATION_ID))
                        {
                            node.Icon = Icon.StopGreen;
                        }

                        e.Nodes.Add(node);
                    }
                }
            }
        }


    }
}