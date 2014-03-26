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

        protected void deReadOrganizations(object sender,  StoreReadDataEventArgs e)
        {
            long HierarchyID;
            if (long.TryParse(uxHierarchyComboBox.SelectedItem.Value, out HierarchyID))
            {
                Entities _context = new Entities();
                var data = _context.ORG_HIER_V.Where(h => h.HIERARCHY_ID == HierarchyID).ToList();
                int count;
                uxOrganizationSecurityStore.DataSource = GenericData.EnumerableFilterHeader<ORG_HIER_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
            }
        }


        protected void deLoadOrganizationsForHierarchy(object sender, DirectEventArgs e)
        {
            uxOrganizationSecurityStore.Reload(); 
        }


    }
}