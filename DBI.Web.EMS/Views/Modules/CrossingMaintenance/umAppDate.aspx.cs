using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Data.Entity;
using System.Data.Objects;
using System.Net.Mail;
using System.Security.Claims;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Threading;
using DBI.Core.Security;
using DBI.Core.Web;
using DBI.Data;
using Ext.Net;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace DBI.Web.EMS.Views.Modules.CrossingMaintenance
{
    public partial class umAppDate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void deAppDateGrid(object sender, StoreReadDataEventArgs e)
        {

            using (Entities _context = new Entities())
            {
                List<object> data;

                //Get List of all new crossings

                data = (from d in _context.CROSSING_APPLICATION
                        join c in _context.CROSSINGS on d.CROSSING_ID equals c.CROSSING_ID
                        select new
                        {
                            d.CROSSING_ID,
                            c.CROSSING_NUMBER,
                            c.SUB_DIVISION,
                            c.STATE,
                            c.MILE_POST,
                            c.DOT,
                            d.REMARKS,
                            d.APPLICATION_DATE,
                            d.TRUCK_NUMBER
                        }).ToList<object>();

                int count;
                uxAppDateStore.DataSource = GenericData.EnumerableFilterHeader<object>(e.Start, e.Limit, e.Sort, e.Parameters["filterheader"], data, out count);
            }
        }
    }
}