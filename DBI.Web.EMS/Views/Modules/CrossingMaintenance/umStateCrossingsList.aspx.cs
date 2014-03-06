using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBI.Core.Web;
using DBI.Data;
using Ext.Net;
using DBI.Data.DataFactory;

namespace DBI.Web.EMS.Views.Modules.CrossingMaintenance
{
    public partial class umStateCrossingsList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void deStateCrossingListGrid(object sender, StoreReadDataEventArgs e)
        {

            using (Entities _context = new Entities())
            {
                List<object> data;

                //Get List of all new crossings

                data = (from d in _context.CROSSINGS
                        select new { d.CROSSING_ID, d.CROSSING_NUMBER, d.SUB_DIVISION, d.STATE, d.COUNTY, d.CITY, d.MILE_POST,
                        d.DOT, d.ROWNE, d.ROWNW, d.ROWSE, d.ROWSW, d.STREET, d.SUB_CONTRACTED, d.LONGITUDE,
                        d.LATITUDE, d.SPECIAL_INSTRUCTIONS }).ToList<object>();

                int count;
                uxStateCrossingListStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
            }
        }
    }
}