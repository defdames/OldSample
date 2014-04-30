using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Ext.Net;
using DBI.Data;

using System.Web.UI.WebControls;
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
            if (uxOrgPanel.SelectedNodes != null)
            {
                if (!string.IsNullOrEmpty(uxFiscalYear.SelectedItem.Value))
                {
                    if (!string.IsNullOrEmpty(uxVersion.SelectedItem.Value))
                    {
                        // X.Msg.Alert("Continue", "OK").Show();
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

        ////////////////////////////////////////////////////////////////////////////////////////// NEW
        protected void Test(object sender, StoreReadDataEventArgs e)
        {
            using (Entities _context = new Entities())
            {
                var AllOrgs = _context.ORG_HIER_V.Select(x => new { x.ORG_HIER, x.ORG_ID }).Distinct().ToList();
                uxFiscalYearStore.DataSource = AllOrgs;
                //this.uxFiscalYearStore.DataBind();
            }            
        }
        ////////////////////////////////////////////////////////////////////////////////////////// NEW
                
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

        public class HIERARCHY_TREEVIEW
        {
            public string ORGANIZATION_NAME { get; set; }
            public string HIERARCHY_NAME { get; set; }
            public long ORGANIZATION_STRUCTURE_ID { get; set; }
            public long ORGANIZATION_ID { get; set; }
        }        



    }
}