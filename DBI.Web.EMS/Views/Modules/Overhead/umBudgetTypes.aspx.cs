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
    public partial class umBudgetTypes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LoadLegalEntitiesTreePanel(object sender, NodeLoadEventArgs e)
        {
            //Load LEs
            if (e.NodeID == "0")
            {
                List<ORGANIZATIONS.ORGANIZATION> _legalEntities = DBI.Data.ORGANIZATIONS.legalEntities();

                //Build the treepanel
                foreach (DBI.Data.ORGANIZATIONS.ORGANIZATION view in _legalEntities)
                {
                    //Create the Hierarchy Levels
                    Node node = new Node();
                    node.Text = view.ORGANIZATION_NAME;
                    node.NodeID = view.ORGANIZATION_ID.ToString();
                    node.Leaf = true;
                    e.Nodes.Add(node);
                }
            } 
        }

        

        protected void deReadBudgetTypesByOrganization(object sender, StoreReadDataEventArgs e)
        {
            long _organizationID;

            RowSelectionModel selection = uxLegalEntityTreeSelectionModel;
            Boolean check = long.TryParse(selection.SelectedRecordID, out _organizationID);

            if (_organizationID > 0)
            {
                using (Entities _context = new Entities())
                {
                    List<OVERHEAD_BUDGET_TYPE> data = OVERHEAD_BUDGET_TYPE.budgetTypesByBusinessUnitID(_organizationID);

                    int count;
                    uxBudgetTypeStore.DataSource = GenericData.EnumerableFilterHeader<OVERHEAD_BUDGET_TYPE>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                    e.Total = count;
                }
            }
        }

    }
}