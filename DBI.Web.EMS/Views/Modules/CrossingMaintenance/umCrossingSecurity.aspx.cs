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

            using (Entities _context = new Entities())
            {
                List<object> data;

                //Get List of all new headers
                data = (from d in _context.DAILY_ACTIVITY_HEADER
                           join p in _context.PROJECTS_V on d.PROJECT_ID equals p.PROJECT_ID
                           join s in _context.DAILY_ACTIVITY_STATUS on d.STATUS equals s.STATUS
                           select new { d.HEADER_ID, d.PROJECT_ID, d.DA_DATE, p.SEGMENT1, p.LONG_NAME, s.STATUS_VALUE }).ToList<object>();
              

                int count;
                uxCurrentSecurityProjectStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
                e.Total = count;
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
         
    }
}