using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using DBI.Data;
using Ext.Net;

namespace DBI.Web.EMS.Views.Modules.CrossingMaintenance
{
    public partial class umCrossingSecurity : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void deSecurityProjectGrid(object sender, StoreReadDataEventArgs e)
        {
            {
            List<WEB_PROJECTS_V> dataIn;
           
                dataIn = WEB_PROJECTS_V.ProjectList();
            
            int count;
            //Get paged, filterable list of data
            List<WEB_PROJECTS_V> data = GenericData.EnumerableFilterHeader<WEB_PROJECTS_V>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], dataIn, out count).ToList();

            e.Total = count;
            uxCurrentSecurityProjectStore.DataSource = data;
            uxCurrentSecurityProjectStore.DataBind();
            }
        }
        protected void deSecurityCrossingGridData(object sender, StoreReadDataEventArgs e)
        {

            using (Entities _context = new Entities())
            {
                List<object> data;

                //Get List of all new headers

                data = (from d in _context.CROSSINGS
                        select new { d.CROSSING_ID, d.CROSSING_NUMBER, d.SUB_DIVISION, d.MTM }).ToList<object>();

                int count;
                uxCurrentSecurityCrossingStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
            }
        }
            protected void deAssociateCrossings(object sender, DirectEventArgs e)
            {
            
            }
    }
         
}
